using System.Collections.Generic;
using UnityEngine;

public class ImpactWaveScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	public Vector3 targetPos;

	protected Transform proTransform;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public WeaponType gunType;

	public float life = 2f;

	public int damage;

	public bool isLocal = true;

	public bool isPenerating;

	public bool isReflecting;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	protected float initAngel = 50f;

	protected HashSet<PenerateHitInfo> penetratingTargets = new HashSet<PenerateHitInfo>();

	protected HashSet<Enemy> enemyhitSet = new HashSet<Enemy>();

	public bool destroyed;

	public void Start()
	{
		proTransform = base.transform;
		createdTime = Time.time;
		ScaleScript component = base.transform.GetComponent<ScaleScript>();
		if (component != null)
		{
			component.scaleSpeed *= VSMath.IMPACT_WAVE_SPEED_BOOTH;
			component.maxScale = 0.65f;
			component.enableMaxScale = true;
		}
	}

	public void Update()
	{
		deltaTime = Time.deltaTime;
		proTransform.Translate(flySpeed * dir * deltaTime, Space.World);
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
		if (enemyByCollider.layer == PhysicsLayer.GIFT)
		{
			int idByName = CMIGift.GetIdByName(enemyByCollider.name);
			CMIGift giftWithId = GameApp.GetInstance().GetGameWorld().GetGiftWithId(idByName);
			if (giftWithId != null)
			{
				PenerateHitInfo penerateHitInfo2 = null;
				foreach (PenerateHitInfo penetratingTarget2 in penetratingTargets)
				{
					if (giftWithId.GetId() == penetratingTarget2.enemyID)
					{
						penerateHitInfo2 = penetratingTarget2;
						break;
					}
				}
				bool flag2 = false;
				if (penerateHitInfo2 == null)
				{
					flag2 = true;
					penetratingTargets.Add(new PenerateHitInfo(giftWithId.GetId(), Time.time));
				}
				else if (penerateHitInfo2.CouldHit())
				{
					flag2 = true;
				}
				if (flag2 && isLocal)
				{
					PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, giftWithId.GetId(), false, (byte)gunType);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
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
	}

	private void OnTriggerEnter(Collider other)
	{
		if (destroyed)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		bool flag = false;
		if (GameApp.GetInstance().GetGameMode().IsVSMode() && isLocal && other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			return;
		}
		string text = "Effect/update_effect/effect_explosion_004";
		text = "Effect/update_effect/effect_explosion_001";
		GameObject original = Resources.Load("Effect/LaserHit") as GameObject;
		GameObject gameObject = Object.Instantiate(original, proTransform.position, Quaternion.identity) as GameObject;
		if (!isPenerating)
		{
			GameObject original2 = Resources.Load(text) as GameObject;
			Object.Instantiate(original2, proTransform.position, Quaternion.identity);
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
			if (enemyByCollider.layer == PhysicsLayer.GIFT)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				CMIGift giftWithId = GameApp.GetInstance().GetGameWorld().GetGiftWithId(idByName);
				if (giftWithId != null)
				{
					PenerateHitInfo penerateHitInfo2 = null;
					foreach (PenerateHitInfo penetratingTarget2 in penetratingTargets)
					{
						if (giftWithId.GetId() == penetratingTarget2.enemyID)
						{
							penerateHitInfo2 = penetratingTarget2;
							break;
						}
					}
					bool flag3 = false;
					if (penerateHitInfo2 == null)
					{
						flag3 = true;
						penetratingTargets.Add(new PenerateHitInfo(giftWithId.GetId(), Time.time));
					}
					else if (penerateHitInfo2.CouldHit())
					{
						flag3 = true;
					}
					if (flag3 && isLocal)
					{
						PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, giftWithId.GetId(), false, (byte)gunType);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
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
			if (!flag)
			{
				return;
			}
		}
		Collider[] array = Physics.OverlapSphere(proTransform.position, explodeRadius, 1 << PhysicsLayer.ENEMY);
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
				PlayerHitItemRequest request2 = new PlayerHitItemRequest((short)damage, idByName2, false, (byte)gunType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
		DoVSDamage();
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
			Debug.Log("could hit");
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

	private void StealHP(int maxDamage)
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			LocalPlayer player = gameWorld.GetPlayer();
			if (player != null)
			{
				player.RecoveHP((float)maxDamage * 0.5f);
			}
		}
	}
}
