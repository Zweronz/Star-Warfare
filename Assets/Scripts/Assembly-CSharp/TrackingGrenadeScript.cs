using System.Collections.Generic;
using UnityEngine;

public class TrackingGrenadeScript : MonoBehaviour
{
	public class TrackedTarget
	{
		public Enemy enemy;

		public RemotePlayer remotePlayer;

		public bool IsExist
		{
			get
			{
				if ((enemy == null || enemy.HP <= 0) && (remotePlayer == null || remotePlayer.Hp <= 0))
				{
					return false;
				}
				return true;
			}
		}

		public Vector3 Position
		{
			get
			{
				if (enemy != null)
				{
					return enemy.GetPosition();
				}
				if (remotePlayer != null)
				{
					return remotePlayer.GetGameObject().transform.position;
				}
				return Vector3.zero;
			}
		}
	}

	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	protected Transform proTransform;

	protected WeaponType gunType;

	public bool slime;

	public int slimeDamage = 100;

	public float slimeDisappearTime = 5f;

	public float maxSlimeScale = 1f;

	public Vector3 dir;

	public float explodeRadius;

	public float trackRadius;

	public float trackSpeed;

	public int damage;

	public bool isLocal = true;

	public byte trackID;

	public int userID;

	public Timer explodeTimer;

	public Vector3 targetPos = Vector3.zero;

	public bool isTracking;

	protected HashSet<Enemy> enemyhitSet = new HashSet<Enemy>();

	private TrackedTarget trackedEnemy;

	private float mUpdateSearchTime;

	public WeaponType GunType
	{
		set
		{
			gunType = value;
		}
	}

	public void Start()
	{
		proTransform = base.transform.parent;
		mUpdateSearchTime = Time.time;
		base.animation.wrapMode = WrapMode.Loop;
		base.animation.Play("move");
	}

	public void Update()
	{
		if (explodeTimer != null && explodeTimer.Ready())
		{
			Explode();
			explodeTimer.Do();
		}
		if (isLocal && !isTracking)
		{
			trackedEnemy = SearchForEnemy();
			if (trackedEnemy != null && trackedEnemy.IsExist)
			{
				isTracking = true;
				targetPos = trackedEnemy.Position;
				explodeTimer.Do();
				proTransform.LookAt(targetPos);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					PlayerTrackingGrendeRequest request = new PlayerTrackingGrendeRequest(userID, trackID, targetPos, Vector3.zero);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		if (isTracking && Track(targetPos))
		{
			Explode();
		}
	}

	private bool Track(Vector3 target)
	{
		Vector3 vector = target - proTransform.position;
		if (vector.x * vector.x + vector.z * vector.z < 1f)
		{
			return true;
		}
		Vector3 normalized = vector.normalized;
		proTransform.position += new Vector3(normalized.x, 0f, normalized.z) * trackSpeed * Time.deltaTime;
		return false;
	}

	private void Explode()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		GameObject original = Resources.Load("Effect/update_effect/effect_explosion_001") as GameObject;
		Object.Instantiate(original, proTransform.position, Quaternion.identity);
		Object.DestroyObject(base.gameObject.transform.parent.gameObject);
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", base.transform.position);
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = damage;
		damageProperty.isLocal = isLocal;
		bool criticalAttack = false;
		int num = Random.Range(0, 100);
		if (num < 80)
		{
			criticalAttack = true;
		}
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.wType = gunType;
		Collider[] array = Physics.OverlapSphere(proTransform.position, explodeRadius, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.GIFT));
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			Ray ray = new Ray(base.transform.position, collider.transform.position - base.transform.position);
			float distance = Mathf.Sqrt((base.transform.position - collider.transform.position).sqrMagnitude);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.GIFT)) && hitInfo.collider.gameObject.layer != PhysicsLayer.ENEMY && hitInfo.collider.gameObject.layer != PhysicsLayer.GIFT)
			{
				continue;
			}
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (!enemyhitSet.Contains(enemyByID))
				{
					damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up;
					enemyByID.HitEnemy(damageProperty);
					enemyhitSet.Add(enemyByID);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI && isLocal)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, idByName, false, 4);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode() && isLocal)
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (GameApp.GetInstance().GetGameMode().IsTeamMode())
				{
					Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(item.GetUserID());
					if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
					{
						continue;
					}
				}
				Vector3 vector = item.GetTransform().position + new Vector3(0f, 1f, 0f);
				float sqrMagnitude = (vector - proTransform.position).sqrMagnitude;
				float num2 = explodeRadius * explodeRadius;
				if (sqrMagnitude < num2)
				{
					Ray ray2 = new Ray(proTransform.position, vector - proTransform.position);
					RaycastHit hitInfo2;
					if (Physics.Raycast(ray2, out hitInfo2, explodeRadius, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
					{
						PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest((short)damage, item.GetUserID(), false, 4);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && gameWorld != null)
		{
			Player remotePlayerByUserID2 = gameWorld.GetRemotePlayerByUserID(userID);
			if (remotePlayerByUserID2 != null)
			{
				remotePlayerByUserID2.TrackingGrenadeDic.Remove(trackID);
			}
		}
		if (slime && isLocal)
		{
			CreatePoisonZone();
		}
	}

	private void CreatePoisonZone()
	{
		GameObject original = Resources.Load("Effect/SatanMachine/joke_force") as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		Ray ray = new Ray(proTransform.position + Vector3.up * 2f, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.WALL)) && (hitInfo.collider.gameObject.layer == PhysicsLayer.FLOOR || hitInfo.collider.gameObject.layer == PhysicsLayer.WALL))
		{
			gameObject.transform.position = hitInfo.point + Vector3.up * 0.05f;
			gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
		}
		Vector3 position = gameObject.transform.position;
		Vector3 up = gameObject.transform.up;
		Timer timer = new Timer();
		timer.SetTimer(0.6f, false);
		GrenadeSlimeScript componentInChildren = gameObject.GetComponentInChildren<GrenadeSlimeScript>();
		componentInChildren.slimeDamage = slimeDamage;
		componentInChildren.disappearTime = slimeDisappearTime;
		componentInChildren.slimeTimer = timer;
		componentInChildren.isLocal = isLocal;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(26, position, up);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private TrackedTarget SearchForEnemy()
	{
		if (Time.time - mUpdateSearchTime > 0.2f)
		{
			mUpdateSearchTime = Time.time;
			float num = 9999999f;
			TrackedTarget trackedTarget = null;
			Vector3 vector = proTransform.position + Vector3.up;
			Collider[] array = Physics.OverlapSphere(vector, trackRadius, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.BOSS));
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Vector3 vector2 = collider.transform.position + Vector3.up;
				Ray ray = new Ray(vector, vector2 - vector);
				float num2 = Mathf.Sqrt((vector - vector2).sqrMagnitude);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, num2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
				{
					continue;
				}
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
					Player player = gameWorld.GetPlayer();
					int num3 = int.Parse(collider.gameObject.name);
					RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(num3);
					if (remotePlayerByUserID != null && (!GameApp.GetInstance().GetGameMode().IsTeamMode() || !remotePlayerByUserID.IsSameTeam(player)) && num2 < num)
					{
						trackedTarget = new TrackedTarget();
						trackedTarget.remotePlayer = remotePlayerByUserID;
						num = num2;
					}
				}
				else if (collider.gameObject.name.StartsWith("E_"))
				{
					Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(collider.name);
					if (num2 < num)
					{
						trackedTarget = new TrackedTarget();
						trackedTarget.enemy = enemyByID;
						num = num2;
					}
				}
			}
			return trackedTarget;
		}
		return null;
	}
}
