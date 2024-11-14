using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TPS/ProjectileScript")]
public class ProjectileScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	public Vector3 targetPos;

	protected Transform proTransform;

	protected Transform trackingTarget;

	public Enemy trackingEnemy;

	public Player trackingPlayer;

	public CMIGift trackingGift;

	protected WeaponType gunType;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 2f;

	public int damage;

	public bool isLocal = true;

	public byte bagIndex;

	public bool isPenerating;

	public bool isReflecting;

	public int NeedReflectTime;

	public int isReflectTime;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	protected float initAngel = 50f;

	protected float trackingAngelRotateTime = 2f;

	protected float trackingFlyAcceleration = 3f;

	protected float trackingDuration = 5.1f;

	protected float trackingUpdateTargetPosInterval = 0.5f;

	protected Timer trackingUpdateTargetPosTimer = new Timer();

	protected Vector3 trackingPos;

	protected float trackingLastUpdatetargetPosTime;

	protected HashSet<PenerateHitInfo> penetratingTargets = new HashSet<PenerateHitInfo>();

	protected HashSet<Enemy> enemyhitSet = new HashSet<Enemy>();

	public Weapon weapon;

	public bool destroyed;

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
		trackingLastUpdatetargetPosTime = createdTime;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			if (gunType == WeaponType.RocketLauncher || gunType == WeaponType.AutoRocketLauncher)
			{
				flySpeed *= VSMath.RPG_FLY_BOOTH;
			}
			if (gunType == WeaponType.LightFist || gunType == WeaponType.Spring)
			{
				ScaleScript component = base.transform.GetComponent<ScaleScript>();
				if (component != null)
				{
					component.scaleSpeed *= VSMath.GLOVE_GROW_SPPED_BOOTH;
				}
			}
		}
		trackingUpdateTargetPosTimer.SetTimer(trackingUpdateTargetPosInterval, true);
	}

	public void Update()
	{
		deltaTime = Time.deltaTime;
		if (trackingEnemy != null)
		{
			flySpeed += Time.deltaTime * trackingFlyAcceleration;
			trackingTarget = trackingEnemy.GetTransform();
			if (trackingTarget != null && trackingEnemy.GetState() != Enemy.DEAD_STATE && Time.time - createdTime < trackingDuration)
			{
				if (trackingUpdateTargetPosTimer.Ready())
				{
					trackingPos = trackingEnemy.GetColliderCenterPosition();
					trackingLastUpdatetargetPosTime = Time.time;
					trackingUpdateTargetPosTimer.Do();
				}
				Vector3 normalized = (trackingPos - proTransform.position).normalized;
				Quaternion to = Quaternion.LookRotation(normalized);
				proTransform.rotation = Quaternion.Lerp(proTransform.rotation, to, (Time.time - trackingLastUpdatetargetPosTime) / trackingAngelRotateTime);
				dir = proTransform.forward;
			}
		}
		if (trackingPlayer != null)
		{
			flySpeed += Time.deltaTime * trackingFlyAcceleration;
			trackingTarget = trackingPlayer.GetTransform();
			if (trackingTarget != null && trackingPlayer.InPlayingState() && Time.time - createdTime < trackingDuration)
			{
				if (trackingUpdateTargetPosTimer.Ready())
				{
					trackingPos = trackingPlayer.GetTransform().position + Vector3.up * 1f;
					trackingLastUpdatetargetPosTime = Time.time;
					trackingUpdateTargetPosTimer.Do();
				}
				Vector3 normalized2 = (trackingPos - proTransform.position).normalized;
				Quaternion to2 = Quaternion.LookRotation(normalized2);
				proTransform.rotation = Quaternion.Lerp(proTransform.rotation, to2, (Time.time - trackingLastUpdatetargetPosTime) / trackingAngelRotateTime);
				dir = proTransform.forward;
			}
		}
		if (trackingGift != null)
		{
			flySpeed += Time.deltaTime * trackingFlyAcceleration;
			trackingTarget = trackingGift.GetTransform();
			if (trackingTarget != null && Time.time - createdTime < trackingDuration)
			{
				if (trackingUpdateTargetPosTimer.Ready())
				{
					trackingPos = trackingGift.GetPosition();
					trackingLastUpdatetargetPosTime = Time.time;
					trackingUpdateTargetPosTimer.Do();
				}
				Vector3 normalized3 = (trackingPos - proTransform.position).normalized;
				Quaternion to3 = Quaternion.LookRotation(normalized3);
				proTransform.rotation = Quaternion.Lerp(proTransform.rotation, to3, (Time.time - trackingLastUpdatetargetPosTime) / trackingAngelRotateTime);
				dir = proTransform.forward;
			}
		}
		proTransform.Translate(flySpeed * dir * deltaTime, Space.World);
		if (gunType == WeaponType.PingPongLauncher)
		{
			ReflectBack();
		}
		if (Time.time - createdTime > life)
		{
			Object.DestroyObject(base.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (!isPenerating)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		GameObject enemyByCollider = Enemy.GetEnemyByCollider(other);
		bool criticalAttack = false;
		int num = Random.Range(0, 100);
		if (num < 80)
		{
			criticalAttack = true;
		}
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = damage;
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.isLocal = isLocal;
		damageProperty.wType = gunType;
		if (enemyByCollider.name.StartsWith("E_"))
		{
			Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
			if (enemyByID != null)
			{
				PenerateHitInfo penerateHitInfo = null;
				foreach (PenerateHitInfo penetratingTarget in penetratingTargets)
				{
					if (enemyByID.EnemyID == penetratingTarget.enemyID)
					{
						penerateHitInfo = penetratingTarget;
						break;
					}
				}
				bool flag = false;
				if (penerateHitInfo == null)
				{
					flag = true;
					penetratingTargets.Add(new PenerateHitInfo(enemyByID.EnemyID, Time.time));
				}
				else if (penerateHitInfo.CouldHit())
				{
					flag = true;
				}
				if (flag)
				{
					damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up * 2f;
					enemyByID.HitEnemy(damageProperty);
				}
			}
		}
		if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(int.Parse(enemyByCollider.name));
			if (remotePlayerByUserID != null)
			{
				DoPenetrateDamage(remotePlayerByUserID);
			}
		}
		if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI && isLocal)
		{
			int idByName = CMIGift.GetIdByName(enemyByCollider.name);
			CMIGift giftWithId = GameApp.GetInstance().GetGameWorld().GetGiftWithId(idByName);
			if (giftWithId != null)
			{
				DoGiftDamage(giftWithId);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (destroyed)
		{
			return;
		}
		if (gunType == WeaponType.LightFist || gunType == WeaponType.Spring)
		{
			explodeRadius = base.transform.localScale.x;
			explodeRadius = Mathf.Clamp(explodeRadius, 5f, 10f);
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		bool flag = false;
		if (GameApp.GetInstance().GetGameMode().IsVSMode() && isLocal && other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			return;
		}
		string path = "Effect/update_effect/effect_explosion_004";
		if (gunType == WeaponType.RocketLauncher || gunType == WeaponType.AutoRocketLauncher)
		{
			path = "Effect/update_effect/effect_explosion_001";
		}
		if (gunType == WeaponType.TrackingGun)
		{
			path = "Effect/update_effect/effect_explosion_005";
		}
		if (gunType == WeaponType.AdvancedMachineGun)
		{
			path = "Effect/update_effect/pumpkin_explosion_01";
		}
		if (gunType == WeaponType.AdvancedSword)
		{
			GameObject original = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject = Object.Instantiate(original, proTransform.position, Quaternion.identity) as GameObject;
		}
		if (isReflecting && (other.gameObject.layer == PhysicsLayer.WALL || ((other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL) | (other.gameObject.layer == PhysicsLayer.FLOOR))))
		{
			if (ReflectBack())
			{
				return;
			}
			GameObject original2 = Resources.Load(path) as GameObject;
			Object.Instantiate(original2, proTransform.position, Quaternion.identity);
			Object.DestroyObject(base.gameObject);
			flag = true;
			destroyed = true;
		}
		if (NeedReflectTime != 0 && (other.gameObject.layer == PhysicsLayer.WALL || ((other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL) | (other.gameObject.layer == PhysicsLayer.FLOOR))))
		{
			if (ReflectBackForTime())
			{
				return;
			}
			GameObject original3 = Resources.Load(path) as GameObject;
			Object.Instantiate(original3, proTransform.position, Quaternion.identity);
			Object.DestroyObject(base.gameObject);
			flag = true;
			destroyed = true;
		}
		if (isPenerating)
		{
			if (!isReflecting && (other.gameObject.layer == PhysicsLayer.WALL || other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL || other.gameObject.layer == PhysicsLayer.FLOOR))
			{
				Object.DestroyObject(base.gameObject);
				destroyed = true;
			}
		}
		else
		{
			GameObject original4 = Resources.Load(path) as GameObject;
			Object.Instantiate(original4, proTransform.position, Quaternion.identity);
			Object.DestroyObject(base.gameObject);
			destroyed = true;
		}
		bool criticalAttack = false;
		int num = Random.Range(0, 100);
		if (num < 80)
		{
			criticalAttack = true;
		}
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = damage;
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.isLocal = isLocal;
		damageProperty.wType = gunType;
		if (isPenerating)
		{
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(other);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (enemyByID != null)
				{
					PenerateHitInfo penerateHitInfo = null;
					foreach (PenerateHitInfo penetratingTarget in penetratingTargets)
					{
						if (enemyByID.EnemyID == penetratingTarget.enemyID)
						{
							penerateHitInfo = penetratingTarget;
							break;
						}
					}
					bool flag2 = false;
					if (penerateHitInfo == null)
					{
						flag2 = true;
						penetratingTargets.Add(new PenerateHitInfo(enemyByID.EnemyID, Time.time));
					}
					else if (penerateHitInfo.CouldHit())
					{
						flag2 = true;
					}
					if (flag2)
					{
						damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up * 2f;
						enemyByID.HitEnemy(damageProperty);
					}
				}
			}
			if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(int.Parse(enemyByCollider.name));
				if (remotePlayerByUserID != null)
				{
					DoPenetrateDamage(remotePlayerByUserID);
				}
			}
			if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI && isLocal)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				CMIGift giftWithId = GameApp.GetInstance().GetGameWorld().GetGiftWithId(idByName);
				if (giftWithId != null)
				{
					DoGiftDamage(giftWithId);
				}
			}
			if (!flag)
			{
				return;
			}
		}
		if (weapon != null)
		{
			AudioManager.GetInstance().PlaySoundAt(weapon.ExplodeSoundName, base.transform.position);
		}
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
			GameObject enemyByCollider2 = Enemy.GetEnemyByCollider(collider);
			if (enemyByCollider2.name.StartsWith("E_"))
			{
				Enemy enemyByID2 = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider2.name);
				if (enemyByID2 != null && !enemyhitSet.Contains(enemyByID2))
				{
					damageProperty.hitpoint = enemyByID2.GetTransform().position + Vector3.up * 2f;
					enemyByID2.HitEnemy(damageProperty);
					enemyhitSet.Add(enemyByID2);
				}
			}
			else if (enemyByCollider2.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI && isLocal)
			{
				int idByName2 = CMIGift.GetIdByName(enemyByCollider2.name);
				PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, idByName2, false, (byte)gunType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		DoVSDamage();
	}

	protected bool ReflectBack()
	{
		Ray ray = new Ray(base.transform.position, dir);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 2f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR)))
		{
			Vector3 normal = hitInfo.normal;
			float num = Vector3.Angle(dir, normal);
			if (num < 155f)
			{
				dir = Vector3.Reflect(dir, normal).normalized;
				proTransform.LookAt(proTransform.position + dir * 2f);
				int num2 = Random.Range(1, 5);
				AudioManager.GetInstance().PlaySoundAt("Audio/diablo/black_disk_bounce0" + num2, proTransform.position);
				return true;
			}
		}
		return false;
	}

	protected bool ReflectBackForTime()
	{
		Ray ray = new Ray(base.transform.position, dir);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 2f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR)))
		{
			Vector3 normal = hitInfo.normal;
			float num = Vector3.Angle(dir, normal);
			if (num < 155f)
			{
				if (isReflectTime < NeedReflectTime)
				{
					dir = Vector3.Reflect(dir, normal).normalized;
					proTransform.LookAt(proTransform.position + dir * 2f);
					int num2 = Random.Range(1, 5);
					AudioManager.GetInstance().PlaySoundAt("Audio/diablo/black_disk_bounce0" + num2, proTransform.position);
					isReflectTime++;
					return true;
				}
				return false;
			}
		}
		return false;
	}

	protected void DoPenetrateDamage(RemotePlayer peneratePlayer)
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
		{
			return;
		}
		PenerateHitInfo penerateHitInfo = null;
		foreach (PenerateHitInfo penetratingTarget in penetratingTargets)
		{
			if (peneratePlayer.GetUserID() == penetratingTarget.enemyID)
			{
				penerateHitInfo = penetratingTarget;
				break;
			}
		}
		bool flag = false;
		if (penerateHitInfo == null)
		{
			flag = true;
			penetratingTargets.Add(new PenerateHitInfo(peneratePlayer.GetUserID(), Time.time));
		}
		else if (penerateHitInfo.CouldHit())
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Debug.Log("could hit");
		if (GameApp.GetInstance().GetGameMode().IsTeamMode())
		{
			Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(peneratePlayer.GetUserID());
			if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
			{
				return;
			}
		}
		PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, peneratePlayer.GetUserID(), false, (byte)gunType);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	protected void DoVSDamage()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
		{
			return;
		}
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			PenerateHitInfo penerateHitInfo = null;
			foreach (PenerateHitInfo penetratingTarget in penetratingTargets)
			{
				if (item.GetUserID() == penetratingTarget.enemyID)
				{
					penerateHitInfo = penetratingTarget;
					break;
				}
			}
			bool flag = false;
			if (penerateHitInfo == null)
			{
				flag = true;
				penetratingTargets.Add(new PenerateHitInfo(item.GetUserID(), Time.time));
			}
			else if (penerateHitInfo.CouldHit())
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
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
			float num = explodeRadius * explodeRadius;
			if (sqrMagnitude < num)
			{
				Ray ray = new Ray(proTransform.position, vector - proTransform.position);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, explodeRadius, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
				{
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, item.GetUserID(), false, (byte)gunType);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
	}

	protected void DoGiftDamage(CMIGift gift)
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gift == null)
		{
			return;
		}
		PenerateHitInfo penerateHitInfo = null;
		foreach (PenerateHitInfo penetratingTarget in penetratingTargets)
		{
			if (gift.GetId() == penetratingTarget.enemyID)
			{
				penerateHitInfo = penetratingTarget;
				break;
			}
		}
		bool flag = false;
		if (penerateHitInfo == null)
		{
			flag = true;
			penetratingTargets.Add(new PenerateHitInfo(gift.GetId(), Time.time));
		}
		else if (penerateHitInfo.CouldHit())
		{
			flag = true;
		}
		if (flag)
		{
			PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, gift.GetId(), false, (byte)gunType);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}
}
