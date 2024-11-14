using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TPS/FlyGrenadeRainScript")]
public class FlyGrenadeRainScript : MonoBehaviour
{
	public float damage;

	public float explodeRadius;

	public int maxDamageCount;

	public float damageInterval;

	public bool isLocal;

	public float life;

	private float createdTime;

	private int damageCount;

	private float lastDamageTime;

	public void Start()
	{
		createdTime = Time.time;
		damageCount = 0;
		DoDamage();
	}

	public void Update()
	{
		if (Time.time - createdTime > life)
		{
			Object.Destroy(base.gameObject);
		}
		else if (isLocal && damageCount < maxDamageCount && Time.time - lastDamageTime > damageInterval)
		{
			DoDamage();
		}
	}

	private void DoDamage()
	{
		lastDamageTime = Time.time;
		damageCount++;
		HashSet<Enemy> hashSet = new HashSet<Enemy>();
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = (int)damage;
		damageProperty.isLocal = isLocal;
		bool criticalAttack = false;
		int num = Random.Range(0, 100);
		if (num < 80)
		{
			criticalAttack = true;
		}
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.wType = WeaponType.FlyGrenadeLauncher;
		Enemy boss = gameWorld.GetBoss(0);
		if (boss != null && boss.GetState() != Enemy.DEAD_STATE && !hashSet.Contains(boss))
		{
			Vector3 vector = base.transform.position - boss.GetPosition();
			vector.y = 0f;
			if (vector.sqrMagnitude < explodeRadius * explodeRadius)
			{
				damageProperty.hitpoint = boss.GetTransform().position + Vector3.up;
				boss.HitEnemy(damageProperty);
				hashSet.Add(boss);
			}
		}
		Enemy boss2 = gameWorld.GetBoss(1);
		if (boss2 != null && boss2.GetState() != Enemy.DEAD_STATE && !hashSet.Contains(boss2))
		{
			Vector3 vector2 = base.transform.position - boss2.GetPosition();
			vector2.y = 0f;
			if (vector2.sqrMagnitude < explodeRadius * explodeRadius)
			{
				damageProperty.hitpoint = boss2.GetTransform().position + Vector3.up;
				boss2.HitEnemy(damageProperty);
				hashSet.Add(boss2);
			}
		}
		List<Collider> list = new List<Collider>();
		int num2 = 4;
		for (int i = 0; i < num2; i++)
		{
			Vector3 vector3 = Vector3.up * 8f * i / num2;
			Collider[] array = Physics.OverlapSphere(base.transform.position + vector3, explodeRadius, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.GIFT));
			Collider[] array2 = array;
			foreach (Collider item in array2)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		foreach (Collider item2 in list)
		{
			Ray ray = new Ray(base.transform.position, item2.transform.position - base.transform.position);
			float distance = Mathf.Sqrt((base.transform.position - item2.transform.position).sqrMagnitude);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.GIFT)) && hitInfo.collider.gameObject.layer != PhysicsLayer.ENEMY && hitInfo.collider.gameObject.layer != PhysicsLayer.GIFT)
			{
				continue;
			}
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(item2);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (!hashSet.Contains(enemyByID))
				{
					damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up;
					enemyByID.HitEnemy(damageProperty);
					hashSet.Add(enemyByID);
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
		foreach (RemotePlayer item3 in remotePlayers)
		{
			if (GameApp.GetInstance().GetGameMode().IsTeamMode())
			{
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(item3.GetUserID());
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			Vector3 vector4 = item3.GetTransform().position - base.transform.position;
			vector4.y = 0f;
			float sqrMagnitude = vector4.sqrMagnitude;
			float num3 = explodeRadius * explodeRadius;
			if (sqrMagnitude < num3)
			{
				Ray ray2 = new Ray(base.transform.position + Vector3.up * 8f, item3.GetTransform().position + Vector3.up - (base.transform.position + Vector3.up * 8f));
				RaycastHit hitInfo2;
				if (Physics.Raycast(ray2, out hitInfo2, 10f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
				{
					PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest((short)damage, item3.GetUserID(), false, 4);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
		}
	}
}
