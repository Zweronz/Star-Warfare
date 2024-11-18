using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionFirelineScript : MonoBehaviour
{
	public Vector3 beginPos;

	public Vector3 endPos;

	private float dis;

	protected bool growing = true;

	protected float grownTime;

	public float destroyTime = 0.5f;

	public Vector3 hitForce;

	public float damage;

	public bool criticalAttack;

	public WeaponType wType;

	public Vector3 hitpoint;

	public bool isLocal = true;

	protected Vector3 aimTarget;

	public Player localPlayer;

	public bool isTouch;

	public void Start()
	{
		base.transform.position = beginPos;
		base.transform.rotation = Quaternion.LookRotation(endPos - beginPos);
		base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, 0.01f);
		dis = Vector3.Distance(beginPos, endPos);
	}

	public void Update()
	{
		float num = dis / 49f;
		if (growing && base.transform.localScale.z > num)
		{
			growing = false;
			grownTime = Time.time;
		}
		if (growing)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z + 8f * Time.deltaTime);
			return;
		}
		Color color = base.GetComponent<Renderer>().material.GetColor("_TintColor");
		float num2 = Time.time - grownTime;
		color.a = 1f - num2 / destroyTime * (num2 / destroyTime);
		base.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		if (Time.time - grownTime > destroyTime)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Launch()
	{
		Debug.Log("delay destroy ");
		Object.DestroyObject(base.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isTouch || (GameApp.GetInstance().GetGameMode().IsVSMode() && isLocal && other.gameObject.layer == PhysicsLayer.PLAYER))
		{
			return;
		}
		if (isLocal)
		{
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.hitForce = hitForce;
			damageProperty.damage = (int)damage;
			damageProperty.criticalAttack = criticalAttack;
			damageProperty.isLocal = isLocal;
			damageProperty.wType = wType;
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(other);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (enemyByID.GetState() != Enemy.DEAD_STATE)
				{
					isTouch = true;
					enemyByID.HitEnemy(damageProperty);
					ReflectionAttackEnemy(enemyByID);
					return;
				}
			}
			if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER && isLocal && GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				if (GameApp.GetInstance().GetGameMode().IsTeamMode())
				{
					int userID = int.Parse(enemyByCollider.name);
					Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
					if (remotePlayerByUserID != null && remotePlayerByUserID.IsSameTeam(localPlayer))
					{
						return;
					}
				}
				isTouch = true;
				Player playerByUserID = GameApp.GetInstance().GetGameWorld().GetPlayerByUserID(int.Parse(enemyByCollider.name));
				PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, int.Parse(enemyByCollider.name), true, (byte)wType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				ReflectionAttack(playerByUserID);
			}
			if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				PlayerHitItemRequest request2 = new PlayerHitItemRequest((short)damage, idByName, false, (byte)wType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
		else
		{
			InvokeRepeating("Launch", 1f, 0f);
		}
	}

	public Enemy GetNearestAimEnemy(Enemy fromEnemy)
	{
		Enemy result = null;
		if (GameApp.GetInstance().GetGameMode().IsSingle() || GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			Hashtable enemies = GameApp.GetInstance().GetGameWorld().GetEnemies();
			object[] array = new object[enemies.Count];
			enemies.Keys.CopyTo(array, 0);
			float num = 99999f;
			for (int i = 0; i < array.Length; i++)
			{
				Enemy enemy = enemies[array[i]] as Enemy;
				if (enemy.GetState() == Enemy.DEAD_STATE || fromEnemy.EnemyID == enemy.EnemyID)
				{
					continue;
				}
				float num2 = Vector3.Distance(fromEnemy.GetTransform().position, enemy.GetTransform().position);
				Ray ray = default(Ray);
				Vector3 normalized = (enemy.GetPosition() + Vector3.up * 1f - fromEnemy.GetPosition() + Vector3.up * 1f).normalized;
				ray = new Ray(fromEnemy.GetPosition(), normalized);
				RaycastHit[] array2 = Physics.RaycastAll(ray, num2 + 10f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY));
				if (array2.Length <= 0)
				{
					continue;
				}
				for (int j = 0; j < array2.Length && array2[j].collider.gameObject.layer != PhysicsLayer.WALL && array2[j].collider.gameObject.layer != PhysicsLayer.TRANSPARENT_WALL; j++)
				{
					if (num > num2)
					{
						result = enemy;
						num = num2;
					}
				}
			}
		}
		return result;
	}

	public Player GetNearestAimPlayer(Player fromPlayer)
	{
		Player result = null;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		float num = 99999f;
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item == null || item.GetUserID() == fromPlayer.GetUserID() || (GameApp.GetInstance().GetGameMode().IsTeamMode() && item.IsSameTeam(localPlayer)))
			{
				continue;
			}
			Vector3 vector = item.GetTransform().position + new Vector3(0f, 1f, 0f);
			Ray ray = default(Ray);
			Vector3 normalized = (vector - fromPlayer.GetTransform().position).normalized;
			ray = new Ray(fromPlayer.GetTransform().position, normalized);
			float num2 = Vector3.Distance(fromPlayer.GetTransform().position, vector);
			RaycastHit[] array = Physics.RaycastAll(ray, num2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER));
			if (array.Length <= 0)
			{
				continue;
			}
			for (int i = 0; i < array.Length && array[i].collider.gameObject.layer != PhysicsLayer.WALL && array[i].collider.gameObject.layer != PhysicsLayer.TRANSPARENT_WALL; i++)
			{
				if (num > num2)
				{
					result = item;
					num = num2;
				}
			}
		}
		return result;
	}

	public void ReflectionAttack(Player fromPlayer)
	{
		Player nearestAimPlayer = GetNearestAimPlayer(fromPlayer);
		if (nearestAimPlayer != null)
		{
			Ray ray = default(Ray);
			Vector3 normalized = (nearestAimPlayer.GetTransform().position - fromPlayer.GetTransform().position).normalized;
			ray = new Ray(fromPlayer.GetTransform().position + normalized * 1.8f, normalized);
			GameObject original = Resources.Load("Effect/SniperFireLine") as GameObject;
			GameObject gameObject = Object.Instantiate(original, nearestAimPlayer.GetTransform().position, Quaternion.LookRotation(endPos - nearestAimPlayer.GetTransform().position)) as GameObject;
			SniperFirelineScript component = gameObject.GetComponent<SniperFirelineScript>();
			component.beginPos = fromPlayer.GetTransform().position + Vector3.up * 1f;
			component.endPos = nearestAimPlayer.GetTransform().position + Vector3.up * 1f;
			GameObject original2 = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject2 = Object.Instantiate(original2, nearestAimPlayer.GetTransform().position, Quaternion.identity) as GameObject;
			Vector3 normalized2 = (nearestAimPlayer.GetTransform().position - fromPlayer.GetTransform().position).normalized;
			PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)(damage / 2f), nearestAimPlayer.GetUserID(), 17);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			PlayerFireRocketRequest request2 = new PlayerFireRocketRequest(41, fromPlayer.GetTransform().position, normalized2, (int)damage / 2, nearestAimPlayer.GetUserID());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	public void ReflectionAttackEnemy(Enemy fromEnemy)
	{
		if (!isLocal)
		{
			return;
		}
		Enemy nearestAimEnemy = GetNearestAimEnemy(fromEnemy);
		if (nearestAimEnemy != null)
		{
			Ray ray = default(Ray);
			Vector3 normalized = (nearestAimEnemy.GetPosition() - fromEnemy.GetPosition()).normalized;
			ray = new Ray(fromEnemy.GetPosition() + normalized * 1.8f, normalized);
			GameObject original = Resources.Load("Effect/SniperFireLine") as GameObject;
			GameObject gameObject = Object.Instantiate(original, nearestAimEnemy.GetPosition(), Quaternion.LookRotation(nearestAimEnemy.GetPosition())) as GameObject;
			SniperFirelineScript component = gameObject.GetComponent<SniperFirelineScript>();
			component.beginPos = fromEnemy.GetPosition() + Vector3.up * 1f;
			component.endPos = nearestAimEnemy.GetPosition() + Vector3.up * 1f;
			GameObject original2 = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject2 = Object.Instantiate(original2, nearestAimEnemy.GetPosition(), Quaternion.identity) as GameObject;
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.hitForce = ray.direction * 2f;
			damageProperty.damage = (int)damage / 2;
			bool flag = false;
			int num = Random.Range(0, 100);
			if (num < 40)
			{
				flag = true;
			}
			damageProperty.wType = WeaponType.RelectionSniper;
			damageProperty.criticalAttack = flag;
			damageProperty.isLocal = isLocal;
			nearestAimEnemy.HitEnemy(damageProperty);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				ReflectionEnemyRequest request = new ReflectionEnemyRequest(fromEnemy.EnemyID, nearestAimEnemy.EnemyID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}
}
