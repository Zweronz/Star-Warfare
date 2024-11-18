using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sniper : Weapon
{
	protected ObjectPool firelineObjectPool;

	protected ObjectPool sparksObjectPool;

	protected float startAimTime;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.Sniper;
	}

	public override string GetAnimationSuffix()
	{
		return "_Sniper";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_Sniper";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public void Aim(float deltaTime)
	{
		if (startAimTime == 0f)
		{
			startAimTime = Time.time;
		}
		if (Time.time - startAimTime > 0.1f && startAimTime != 0f)
		{
			GameApp.GetInstance().GetGameWorld().GetCamera()
				.ZoomToSniper(deltaTime);
		}
	}

	public bool isAiming()
	{
		return startAimTime != 0f;
	}

	public bool AttackDoneReadyZoomOut()
	{
		return Time.time - lastAttackTime > 0.5f;
	}

	public override void Init(Player player)
	{
		base.Init(player);
		firelineObjectPool = new ObjectPool();
		GameObject prefab = Resources.Load("Effect/Fireline2") as GameObject;
		firelineObjectPool.Init("Firelines", prefab, 8, 0.8f);
		sparksObjectPool = new ObjectPool();
		GameObject prefab2 = Resources.Load("Effect/GunBurst") as GameObject;
		sparksObjectPool.Init("Sparks", prefab2, 3, 0.22f);
		gunfireObj = gun.transform.Find("Rect").gameObject;
		if (gunfireObj != null)
		{
			gunfireObj.GetComponent<Renderer>().enabled = false;
		}
		playSoundTimer.SetTimer(attackFrenquency / 2f, true);
		StopFire();
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.LASER_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.LASER_BOOTH));
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

	public override void GunOn()
	{
		base.GunOn();
		if (gunfireObj != null)
		{
			gunfireObj.GetComponent<Renderer>().enabled = true;
		}
	}

	public override void GunOff()
	{
		base.GunOff();
		if (gunfireObj != null)
		{
			gunfireObj.GetComponent<Renderer>().enabled = false;
		}
	}

	public override void StopFire()
	{
		if (firelineObjectPool != null)
		{
			firelineObjectPool.DestructAll();
		}
		if (sparksObjectPool != null)
		{
			sparksObjectPool.DestructAll();
		}
	}

	public override void PlaySound()
	{
		if (playSoundTimer.Ready())
		{
			playSoundTimer.Do();
		}
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		Vector3 normalized = player.GetTransform().forward.normalized;
		AudioManager.GetInstance().PlaySoundAt("Audio/sniper/r100_railgun", player.GetTransform().position);
		gunfire.transform.rotation = player.GetTransform().rotation;
		gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
		normalized = gunfire.transform.forward.normalized;
		Vector3 vector = gunfire.position + normalized * 80f;
		GameObject original = Resources.Load("Effect/SniperFireLine") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(vector - gunfire.position)) as GameObject;
		SniperFirelineScript component = gameObject.GetComponent<SniperFirelineScript>();
		component.beginPos = gunfire.position;
		component.endPos = vector;
		UnityEngine.Object original2 = Resources.Load("Effect/Sniper/JJQ");
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(original2, gunfire.position, Quaternion.identity);
		gameObject2.transform.parent = gunfire;
	}

	public override void Attack(float deltaTime)
	{
		HashSet<Enemy> hashSet = new HashSet<Enemy>();
		startAimTime = 0f;
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		AudioManager.GetInstance().PlaySoundAt("Audio/sniper/r100_railgun", player.GetTransform().position);
		UnityEngine.Object original = Resources.Load("Effect/Sniper/JJQ");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.identity);
		gameObject.transform.parent = gunfire;
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		Ray ray = default(Ray);
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - transform.position).normalized;
		ray = new Ray(transform.position + normalized * 1.8f, normalized);
		RaycastHit raycastHit = default(RaycastHit);
		raycastHit.point = transform.position + (vector - transform.position).normalized * 80f;
		float num = Mathf.Tan((float)Math.PI / 3f);
		RaycastHit[] array = Physics.RaycastAll(ray, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT));
		float num2 = 1000000f;
		if (array.Length > 0)
		{
			aimTarget = array[array.Length - 1].point;
			raycastHit = array[array.Length - 1];
		}
		else
		{
			aimTarget = raycastHit.point;
		}
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		Vector3 vector2 = gunfire.InverseTransformPoint(aimTarget);
		float num3 = 1000f;
		Vector3 point = raycastHit.point;
		for (int i = 0; i < array.Length; i++)
		{
			if ((array[i].collider.gameObject.layer == PhysicsLayer.WALL || array[i].collider.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL) && array[i].distance < num3)
			{
				num3 = array[i].distance;
				point = array[i].point;
			}
		}
		GameObject original2 = Resources.Load("Effect/SniperFireLine") as GameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original2, gunfire.position, Quaternion.LookRotation(point - gunfire.position)) as GameObject;
		SniperFirelineScript component2 = gameObject2.GetComponent<SniperFirelineScript>();
		component2.beginPos = gunfire.position;
		component2.endPos = point;
		for (int j = 0; j < array.Length; j++)
		{
			Vector3 zero = Vector3.zero;
			if (array[j].distance > num3)
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && array[j].collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(array[j].collider.gameObject.name);
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			if (!(((array[j].collider.gameObject.layer != PhysicsLayer.ENEMY) ? gunfire.transform.InverseTransformPoint(array[j].point) : gunfire.transform.InverseTransformPoint(array[j].collider.transform.position)).z > 0f))
			{
				continue;
			}
			raycastHit = array[j];
			if (!(raycastHit.collider != null))
			{
				continue;
			}
			GameObject original3 = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject3 = UnityEngine.Object.Instantiate(original3, raycastHit.point, Quaternion.identity) as GameObject;
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(raycastHit.collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (enemyByID.GetState() != Enemy.DEAD_STATE && !hashSet.Contains(enemyByID))
				{
					hashSet.Add(enemyByID);
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.hitForce = ray.direction * 2f;
					damageProperty.damage = (int)damage;
					bool criticalAttack = false;
					int num4 = Random.Range(0, 100);
					if (num4 < 40)
					{
						criticalAttack = true;
					}
					damageProperty.wType = GetWeaponType();
					damageProperty.hitpoint = raycastHit.point;
					damageProperty.criticalAttack = criticalAttack;
					damageProperty.isLocal = true;
					enemyByID.HitEnemy(damageProperty);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, int.Parse(enemyByCollider.name), true, (byte)GetWeaponType());
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
