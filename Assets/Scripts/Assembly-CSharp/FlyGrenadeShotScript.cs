using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TPS/FlyGrenadeShotScript")]
public class FlyGrenadeShotScript : MonoBehaviour
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

		public Transform trans
		{
			get
			{
				if (enemy != null)
				{
					return enemy.GetTransform();
				}
				if (remotePlayer != null)
				{
					return remotePlayer.GetTransform();
				}
				return null;
			}
		}

		public int ID
		{
			get
			{
				if (enemy != null)
				{
					return enemy.EnemyID;
				}
				if (remotePlayer != null)
				{
					return remotePlayer.GetUserID();
				}
				return -1;
			}
		}
	}

	private const float CLOSE_PARAM = 10f;

	private const float VERTICAL_ACCELERATE = 20f;

	public const float FLY_HEIGHT = 8f;

	public int ownerId;

	public byte grenadeId;

	public Vector3 dir;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 3f;

	public int damage;

	public bool isLocal = true;

	public bool destroyed;

	public float searchRange;

	private float createdTime;

	private float lastTriggerTime;

	private Ray ray;

	private RaycastHit rayhit;

	private Vector3 horizontalDir;

	private float verticalSpeed;

	private bool findTarget;

	private Transform targetTransform;

	private Vector3 targetPos = Vector3.zero;

	private float mUpdateSearchTime;

	protected HashSet<Enemy> enemyhitSet = new HashSet<Enemy>();

	public void Start()
	{
		createdTime = Time.time;
		mUpdateSearchTime = Time.time;
		verticalSpeed = Mathf.Sign(dir.y) * Mathf.Sqrt(dir.y * dir.y / (dir.x * dir.x + dir.z * dir.z)) * flySpeed;
		horizontalDir = new Vector3(dir.x, 0f, dir.z).normalized;
	}

	public void Update()
	{
		if (findTarget)
		{
			if (targetTransform != null)
			{
				targetPos = targetTransform.position;
			}
			Vector3 vector = targetPos - base.transform.position;
			if (isLocal && vector.x * vector.x + vector.z * vector.z < 1f)
			{
				Explode();
				if (!destroyed)
				{
					destroyed = true;
					Object.Destroy(base.gameObject);
				}
				return;
			}
			vector.y = 0f;
			Vector3 normalized = vector.normalized;
			horizontalDir = Vector3.Slerp(horizontalDir, normalized, 0.2f);
		}
		Vector3 translation = horizontalDir * flySpeed * Time.deltaTime;
		float num = 0f;
		Ray ray = new Ray(base.transform.position, Vector3.down);
		if (Physics.Raycast(ray, out rayhit, 100f, 1 << PhysicsLayer.FLOOR))
		{
			num = rayhit.point.y;
		}
		if (base.transform.position.y > num + 8f - 1f)
		{
			verticalSpeed = (num + 8f - base.transform.position.y) * 10f;
			if (!findTarget && isLocal)
			{
				TrackedTarget trackedTarget = FindTarget();
				if (findTarget)
				{
					targetTransform = trackedTarget.trans;
					if (targetTransform != null)
					{
						targetPos = targetTransform.position;
					}
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && trackedTarget != null && trackedTarget.ID != -1)
					{
						FlyGrenadeFindTargetRequest request = new FlyGrenadeFindTargetRequest(Lobby.GetInstance().GetChannelID(), grenadeId, trackedTarget.ID, targetPos);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
			}
		}
		else if (base.transform.position.y > num + 1f)
		{
			verticalSpeed += 20f * Time.deltaTime;
		}
		else
		{
			verticalSpeed += 200f * Time.deltaTime;
		}
		translation += Vector3.up * verticalSpeed * Time.deltaTime;
		base.transform.Translate(translation, Space.World);
		if (!destroyed && Time.time - createdTime > life)
		{
			Explode();
			if (!destroyed)
			{
				destroyed = true;
				Object.Destroy(base.gameObject);
			}
		}
	}

	private TrackedTarget FindTarget()
	{
		if (Time.time - mUpdateSearchTime > 0.2f)
		{
			mUpdateSearchTime = Time.time;
			float num = 9999999f;
			TrackedTarget trackedTarget = null;
			Vector3 position = base.transform.position;
			Collider[] array = Physics.OverlapSphere(position, searchRange, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.BOSS));
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Vector3 vector = collider.transform.position + Vector3.up;
				Ray ray = new Ray(position, vector - position);
				float num2 = Mathf.Sqrt((position - vector).sqrMagnitude);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, num2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
				{
					continue;
				}
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
					Player player = gameWorld.GetPlayer();
					int userID = int.Parse(collider.gameObject.name);
					RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
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
			if (trackedTarget != null && trackedTarget.IsExist)
			{
				findTarget = true;
			}
			return trackedTarget;
		}
		return null;
	}

	private void Explode()
	{
		Explode(Vector3.zero);
	}

	private void Explode(Vector3 pos)
	{
		GameObject original = Resources.Load("SW2_Effect/HotWing_Trajectory") as GameObject;
		Vector3 vector = pos;
		if (isLocal)
		{
			vector = base.transform.position + Vector3.down * 8f;
		}
		GameObject gameObject = Object.Instantiate(original, vector, Quaternion.identity) as GameObject;
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", vector);
		if (isLocal)
		{
			FlyGrenadeRainScript flyGrenadeRainScript = gameObject.AddComponent<FlyGrenadeRainScript>();
			flyGrenadeRainScript.damage = damage;
			flyGrenadeRainScript.explodeRadius = explodeRadius;
			flyGrenadeRainScript.maxDamageCount = 4;
			flyGrenadeRainScript.damageInterval = 0.3f;
			flyGrenadeRainScript.isLocal = isLocal;
			flyGrenadeRainScript.life = 2f;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				FlyGrenadeExplodeRequest request = new FlyGrenadeExplodeRequest(Lobby.GetInstance().GetChannelID(), grenadeId, base.transform.position + Vector3.down * 8f);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			return;
		}
		AutoDestroyScript autoDestroyScript = gameObject.AddComponent<AutoDestroyScript>();
		autoDestroyScript.life = 2f;
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(ownerId);
			if (remotePlayerByUserID != null && remotePlayerByUserID.FlyGrenadeDic != null && remotePlayerByUserID.FlyGrenadeDic.ContainsKey(grenadeId))
			{
				remotePlayerByUserID.FlyGrenadeDic.Remove(grenadeId);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isLocal)
		{
			Explode();
			if (!destroyed)
			{
				destroyed = true;
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void OnChangeTarget(int targetId, Vector3 pos)
	{
		findTarget = true;
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Enemy enemyByID = gameWorld.GetEnemyByID("E_" + targetId);
			if (enemyByID != null)
			{
				targetTransform = enemyByID.GetTransform();
			}
			else if (targetId == Lobby.GetInstance().GetChannelID())
			{
				Player player = gameWorld.GetPlayer();
				targetTransform = player.GetTransform();
			}
			else
			{
				RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(targetId);
				if (remotePlayerByUserID != null)
				{
					targetTransform = remotePlayerByUserID.GetTransform();
				}
			}
		}
		if (targetTransform != null)
		{
			targetPos = targetTransform.position;
		}
		else
		{
			targetPos = pos;
		}
	}

	public void ExplodeAndDestroy(Vector3 pos)
	{
		Explode(pos);
		if (!destroyed)
		{
			destroyed = true;
			Object.Destroy(base.gameObject);
		}
	}
}
