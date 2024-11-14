using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMachineGun : Weapon
{
	protected const int TOTAL_SHOT_COUNT = 5;

	protected ObjectPool firelineObjectPool;

	protected ObjectPool sparksObjectPool;

	protected float startFireTime;

	protected float stopFireTime;

	protected bool stopped = true;

	protected float radius = 3.5f;

	protected float angle = 30f;

	protected float shotCount;

	protected float lastMeleeAttackTime;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AdvancedMachineGun;
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
		base.ExplodeSoundName = "Audio/rpg/rpg-31_boom";
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
		if (Time.time - lastMeleeAttackTime >= 0.1f)
		{
			lastMeleeAttackTime = Time.time;
			MeleeAttack();
		}
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
		shotCount = 0f;
		if (gunfireObj != null)
		{
			gunfireObj.renderer.enabled = false;
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
			gun.animation.Stop();
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
		shotCount = 0f;
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		gun.animation["Take 001"].wrapMode = WrapMode.Loop;
		gun.animation.Play("Take 001");
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
		gunfireObj.renderer.enabled = true;
	}

	public override void Attack(float deltaTime)
	{
		gun.animation["Take 001"].wrapMode = WrapMode.Loop;
		gun.animation.Play("Take 001");
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
			gunfireObj.renderer.enabled = true;
		}
		Camera mainCamera = Camera.mainCamera;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
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
					damageProperty.wType = WeaponType.AdvancedMachineGun;
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
		LaunchRPG();
		attacked = false;
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
	}

	protected void MeleeAttack()
	{
		Hashtable enemies = GameApp.GetInstance().GetGameWorld().GetEnemies();
		int num = (int)(damage / 2f);
		foreach (Enemy value in enemies.Values)
		{
			if (value.GetState() != Enemy.GRAVEBORN_STATE && value.GetState() != Enemy.DEAD_STATE)
			{
				Vector3 vector = player.GetTransform().InverseTransformPoint(value.GetTransform().position);
				if (Vector3.Distance(player.GetTransform().position, value.GetTransform().position) < radius && vector.z > 0f && GetHorizontalAbsAngle(player.GetTransform(), value.GetTransform()) < angle)
				{
					Debug.Log("hit: " + value.EnemyID);
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.damage = num;
					damageProperty.wType = WeaponType.AdvancedMachineGun;
					damageProperty.hitpoint = value.GetTransform().position + Vector3.up;
					damageProperty.criticalAttack = true;
					damageProperty.isLocal = true;
					value.HitEnemy(damageProperty);
				}
			}
		}
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			List<CMIGift> cMIGifts = GameApp.GetInstance().GetGameWorld().GetCMIGifts();
			foreach (CMIGift item in cMIGifts)
			{
				if (item != null && item.GetTransform() != null)
				{
					Vector3 vector2 = player.GetTransform().InverseTransformPoint(item.GetPosition());
					if (Vector3.Distance(player.GetTransform().position, item.GetPosition()) < radius && vector2.z > 0f && GetHorizontalAbsAngle(player.GetTransform(), item.GetTransform()) < angle)
					{
						PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, item.GetId(), false, (byte)GetWeaponType());
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
			}
		}
		if (!GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			return;
		}
		short num2 = (short)VSMath.GetDamageInVS(num);
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item2 in remotePlayers)
		{
			if (item2 != null && (!GameApp.GetInstance().GetGameMode().IsTeamMode() || !item2.IsSameTeam(player)))
			{
				Vector3 vector3 = player.GetTransform().InverseTransformPoint(item2.GetTransform().position);
				if (Vector3.Distance(player.GetTransform().position, item2.GetTransform().position) < radius && vector3.z > 0f && GetHorizontalAbsAngle(player.GetTransform(), item2.GetTransform()) < angle)
				{
					PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest(num2, item2.GetUserID(), false, 21);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
		}
	}

	public float GetHorizontalAbsAngle(Transform src, Transform tar)
	{
		if (src == null || tar == null)
		{
			return 0f;
		}
		Vector3 from = tar.position - src.position;
		from.y = 0f;
		Vector3 to = new Vector3(src.forward.x, 0f, src.forward.z);
		return Vector3.Angle(from, to);
	}

	public void LaunchRPG()
	{
		shotCount += 1f;
		if (shotCount < 5f)
		{
			return;
		}
		shotCount = 0f;
		if (Random.Range(0, 10) >= 7)
		{
			Ray ray = default(Ray);
			Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
			Vector3 normalized = (vector - cameraTransform.position).normalized;
			ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)))
			{
				aimTarget = hitInfo.point;
			}
			else
			{
				aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
			}
			Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
			GameObject original = Resources.Load("Effect/pumpkinProjectile") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
			ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
			component.dir = normalized2;
			component.flySpeed = 16f;
			component.explodeRadius = 3f;
			component.hitForce = hitForce;
			component.life = 8f;
			component.damage = (int)(damage * 3f);
			component.GunType = WeaponType.AdvancedMachineGun;
			component.targetPos = aimTarget;
			component.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
			component.weapon = this;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerFireRocketRequest request = new PlayerFireRocketRequest(21, gunfire.position, normalized2);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}
}
