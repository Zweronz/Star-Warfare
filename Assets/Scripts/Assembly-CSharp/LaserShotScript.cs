using UnityEngine;

public class LaserShotScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	public bool isLocal = true;

	protected Transform proTransform;

	protected WeaponType gunType;

	public byte weaponBagIndex;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 2f;

	public float damage;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	protected float initAngel = 50f;

	protected Timer laserTimer = new Timer();

	public WeaponType GunType
	{
		set
		{
			gunType = value;
		}
	}

	public void Start()
	{
		createdTime = Time.time;
		laserTimer.SetTimer(0.2f, false);
	}

	public void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			deltaTime = 0f;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		Weapon weapon = player.GetWeapon();
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.hitForce = (dir + new Vector3(0f, 0.18f, 0f)) * hitForce;
		damageProperty.damage = (int)damage;
		bool criticalAttack = false;
		int num = Random.Range(0, 100);
		if (num < 60)
		{
			criticalAttack = true;
		}
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.isLocal = isLocal;
		damageProperty.wType = gunType;
		LaserCannon laserCannon = weapon as LaserCannon;
		bool flag = false;
		if (!((!isLocal) ? laserTimer.Ready() : laserCannon.CoolDown()))
		{
			return;
		}
		if (!isLocal)
		{
			laserTimer.Do();
		}
		else
		{
			laserCannon.SetShootTimeNow();
		}
		GameObject enemyByCollider = Enemy.GetEnemyByCollider(other);
		if (enemyByCollider.name.StartsWith("E_"))
		{
			Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
			if (enemyByID != null)
			{
				damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up;
				enemyByID.HitEnemy(damageProperty);
			}
		}
		else if (other.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			if (!isLocal || !GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				return;
			}
			if (GameApp.GetInstance().GetGameMode().IsTeamMode())
			{
				int userID = int.Parse(enemyByCollider.name);
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					return;
				}
			}
			PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, int.Parse(enemyByCollider.name), false, 8);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI && isLocal)
		{
			int idByName = CMIGift.GetIdByName(enemyByCollider.name);
			PlayerHitItemRequest request2 = new PlayerHitItemRequest((short)damage, idByName, false, 8);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}
}
