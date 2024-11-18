using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MachineGun : Weapon
{
	protected ObjectPool firelineObjectPool;

	protected ObjectPool sparksObjectPool;

	protected float startFireTime;

	protected float stopFireTime;

	protected bool stopped = true;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.MachineGun;
	}

	public override string GetAnimationSuffix()
	{
		return "_machinegun";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_machinegun";
	}

	public override void Init(Player player)
	{
		base.Init(player);
		firelineObjectPool = new ObjectPool();
		GameObject prefab = Resources.Load("Effect/Fireline") as GameObject;
		firelineObjectPool.Init("Firelines", prefab, 8, 0.8f);
		sparksObjectPool = new ObjectPool();
		GameObject prefab2 = Resources.Load("Effect/GunBurst") as GameObject;
		sparksObjectPool.Init("Sparks", prefab2, 3, 0.22f);
		UnityEngine.Object original = Resources.Load("Effect/GunFire_M");
		gunfireObj = (GameObject)UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.identity);
		gunfireObj.transform.parent = gunfire;
		waitForAttackAnimationStop = false;
		StopFire();
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.MACHINE_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.MACHINE_BOOTH));
	}

	public override void Loop(float deltaTime)
	{
	}

	public override void AutoDestructEffect()
	{
		if (firelineObjectPool != null)
		{
			firelineObjectPool.AutoDestruct();
		}
		if (sparksObjectPool != null)
		{
			sparksObjectPool.AutoDestruct();
		}
	}

	public override void StopFire()
	{
		if (gunfireObj != null)
		{
			gunfireObj.GetComponent<Renderer>().enabled = false;
		}
		if (firelineObjectPool != null)
		{
			firelineObjectPool.DestructAll();
		}
		if (sparksObjectPool != null)
		{
			sparksObjectPool.DestructAll();
		}
		startFireTime = Time.time;
		AudioManager.GetInstance().StopSound(base.Name + "_FiringSound_01");
		AudioManager.GetInstance().StopSound(base.Name + "_FiringSound_02");
		if (AudioManager.GetInstance().IsPlaying(base.Name + "_FiringSound_02") && !stopped)
		{
			AudioManager.GetInstance().PlaySoundSingle("Audio/machinegun/" + base.Name + "_FiringSound_03");
			stopFireTime = Time.time;
			stopped = true;
		}
		if (Time.time - stopFireTime > 1f)
		{
			gun.GetComponent<Animation>().Stop();
		}
	}

	public override void GunOn()
	{
		base.GunOn();
		player.SetLowerBodyAnimation(GetAnimationSuffix(), 0);
	}

	public override void GunOff()
	{
		base.GunOff();
		player.SetLowerBodyAnimation(GetAnimationSuffix(), -1);
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		gun.GetComponent<Animation>()["Take 001"].wrapMode = WrapMode.Loop;
		gun.GetComponent<Animation>().Play("Take 001");
		if (Time.time - startFireTime < 1f)
		{
			AudioManager.GetInstance().PlaySoundSingle("Audio/machinegun/" + base.Name + "_FiringSound_01");
			return;
		}
		Vector3 normalized = player.GetTransform().forward.normalized;
		gunfire.transform.rotation = player.GetTransform().rotation;
		gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
		normalized = gunfire.transform.forward.normalized;
		GameObject gameObject = firelineObjectPool.CreateObject(gunfire.transform.position + normalized * 2f, normalized, Quaternion.identity);
		gameObject.transform.Rotate(180f, 0f, 0f);
		if (!(gameObject == null))
		{
			FireLineScript component = gameObject.GetComponent<FireLineScript>();
			component.transform.Rotate(90f, 0f, 0f);
			component.beginPos = gunfire.position;
			component.endPos = gunfire.position + normalized * 100f;
		}
		gunfireObj.GetComponent<Renderer>().enabled = true;
	}

	public override void Attack(float deltaTime)
	{
		gun.GetComponent<Animation>()["Take 001"].wrapMode = WrapMode.Loop;
		gun.GetComponent<Animation>().Play("Take 001");
		stopped = false;
		if (Time.time - startFireTime < 1f)
		{
			AudioManager.GetInstance().PlaySoundSingle("Audio/machinegun/" + base.Name + "_FiringSound_01");
			return;
		}
		AudioManager.GetInstance().StopSound(base.Name + "_FiringSound_01");
		AudioManager.GetInstance().PlaySoundSingleLoop("Audio/machinegun/" + base.Name + "_FiringSound_02");
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		if (gunfire != null)
		{
			gunfireObj.GetComponent<Renderer>().enabled = true;
		}
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		Ray ray = default(Ray);
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		ray = new Ray(transform.position + normalized * 1.8f, normalized);
		RaycastHit raycastHit = default(RaycastHit);
		raycastHit.point = transform.position + (vector - transform.position).normalized * 80f;
		float num = Mathf.Tan((float)Math.PI / 3f);
		RaycastHit[] array = Physics.RaycastAll(ray, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT));
		float num2 = 1000000f;
		for (int i = 0; i < array.Length; i++)
		{
			if (GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && array[i].collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(array[i].collider.gameObject.name);
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			Vector3 zero = Vector3.zero;
			if (((array[i].collider.gameObject.layer != PhysicsLayer.ENEMY) ? gunfire.transform.InverseTransformPoint(array[i].point) : gunfire.transform.InverseTransformPoint(array[i].collider.transform.position)).y < 3f)
			{
				float sqrMagnitude = (array[i].point - gunfire.transform.position).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					raycastHit = array[i];
					num2 = sqrMagnitude;
				}
			}
		}
		aimTarget = raycastHit.point;
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		int num3 = Random.Range(-6, 6);
		Vector3 zero2 = Vector3.zero;
		zero2 = ((num3 % 2 != 0) ? gunfire.transform.TransformPoint(Vector3.up * num3 * 0.2f) : gunfire.transform.TransformPoint(Vector3.left * num3 * 0.05f));
		normalized2 = (aimTarget - gunfire.transform.position).normalized;
		GameObject gameObject = firelineObjectPool.CreateObject(zero2 + normalized2 * (Mathf.Abs(num3) + 2), normalized2, Quaternion.identity);
		gameObject.transform.Rotate(180f, 0f, 0f);
		if (!(gameObject == null))
		{
			FireLineScript component2 = gameObject.GetComponent<FireLineScript>();
			component2.transform.Rotate(90f, 0f, 0f);
			component2.beginPos = gunfire.position;
			component2.endPos = raycastHit.point;
			component2.speed = 150f;
		}
		if (raycastHit.collider != null)
		{
			Vector3 vector2 = gunfire.InverseTransformPoint(aimTarget);
			sparksObjectPool.CreateObject(raycastHit.point, Vector3.one, Quaternion.identity);
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(raycastHit.collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (enemyByID.GetState() != Enemy.DEAD_STATE)
				{
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.hitForce = ray.direction * 2f;
					damageProperty.damage = (int)damage;
					bool criticalAttack = false;
					int num4 = Random.Range(0, 100);
					if (num4 < 40)
					{
						criticalAttack = true;
					}
					damageProperty.criticalAttack = criticalAttack;
					damageProperty.hitpoint = raycastHit.point;
					damageProperty.isLocal = true;
					damageProperty.wType = WeaponType.MachineGun;
					enemyByID.HitEnemy(damageProperty);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, int.Parse(enemyByCollider.name), false, (byte)GetWeaponType());
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				PlayerHitItemRequest request2 = new PlayerHitItemRequest((short)damage, idByName, false, (byte)GetWeaponType());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
		attacked = false;
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
	}
}
