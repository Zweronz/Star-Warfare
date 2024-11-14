using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TPS/TrackingGrenadeShotScript")]
public class TrackingGrenadeShotScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	public Vector3 targetPos;

	protected Transform proTransform;

	protected WeaponType gunType;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float trackRadius;

	public float flySpeed;

	public Vector3 speed;

	public float trackSpeed;

	public float life = 2f;

	public int damage;

	public bool isLocal = true;

	public byte bagIndex;

	public byte trackID;

	public int userID;

	public bool slime;

	public int slimeDamage = 100;

	public float slimeDisappearTime = 5f;

	public float maxSlimeScale = 1f;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	protected float initAngel = 50f;

	protected Timer explodeTimer;

	protected HashSet<Enemy> enemyhitSet = new HashSet<Enemy>();

	public WeaponType GunType
	{
		set
		{
			gunType = value;
		}
	}

	public void Start()
	{
		proTransform = base.transform;
		createdTime = Time.time;
		base.animation.wrapMode = WrapMode.Loop;
		base.animation.Play("fire");
	}

	public void Update()
	{
		if (explodeTimer != null && explodeTimer.Ready())
		{
			Explode();
			explodeTimer.Do();
		}
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
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
		{
			return;
		}
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

	private void OnTriggerEnter(Collider other)
	{
		if (explodeTimer == null && (other.gameObject.layer == PhysicsLayer.WALL || other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL))
		{
			explodeTimer = new Timer();
			float interval = 1.5f;
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				interval = VSMath.GL_EXP_TIME;
			}
			explodeTimer.SetTimer(interval, false);
		}
		else if (other.gameObject.layer == PhysicsLayer.FLOOR)
		{
			if (isLocal)
			{
				CreateTrackingGrenade();
			}
			Object.DestroyObject(base.gameObject.transform.parent.gameObject);
		}
		else
		{
			Explode();
		}
	}

	private void CreateTrackingGrenade()
	{
		GameObject original = Resources.Load("Effect/TrackingRobot") as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		Ray ray = new Ray(proTransform.position + Vector3.up * 2f, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.WALL)) && (hitInfo.collider.gameObject.layer == PhysicsLayer.FLOOR || hitInfo.collider.gameObject.layer == PhysicsLayer.WALL))
		{
			gameObject.transform.position = hitInfo.point + Vector3.up * 0.05f;
		}
		Vector3 position = gameObject.transform.position;
		Timer timer = new Timer();
		timer.SetTimer(2f, false);
		gameObject.transform.LookAt(gameObject.transform.position + dir);
		TrackingGrenadeScript componentInChildren = gameObject.GetComponentInChildren<TrackingGrenadeScript>();
		componentInChildren.damage = damage;
		componentInChildren.explodeTimer = timer;
		componentInChildren.explodeRadius = explodeRadius;
		componentInChildren.trackRadius = trackRadius;
		componentInChildren.trackSpeed = trackSpeed;
		componentInChildren.trackID = trackID;
		componentInChildren.userID = userID;
		componentInChildren.slimeDamage = slimeDamage;
		componentInChildren.slimeDisappearTime = slimeDisappearTime;
		componentInChildren.maxSlimeScale = maxSlimeScale;
		componentInChildren.slime = slime;
		componentInChildren.isLocal = isLocal;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerTrackingGrendeRequest request = new PlayerTrackingGrendeRequest(userID, trackID, position, dir);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}
}
