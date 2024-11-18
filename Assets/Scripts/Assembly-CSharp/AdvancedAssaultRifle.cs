using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdvancedAssaultRifle : Weapon
{
	protected ObjectPool firelineObjectPool;

	protected ObjectPool sparksObjectPool;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AdvancedAssaultRifle;
	}

	public override string GetAnimationSuffix()
	{
		return "_rifle";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_rifle";
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
		UnityEngine.Object original = Resources.Load("Effect/GunFire");
		gunfireObj = (GameObject)UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.identity);
		gunfireObj.transform.parent = gunfire;
		playSoundTimer.SetTimer(attackFrenquency, true);
		StopFire();
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.ASSAULT_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.ASSAULT_BOOTH));
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
	}

	public override void GunOff()
	{
		base.GunOff();
	}

	public override void PlaySound()
	{
		if (playSoundTimer.Ready())
		{
			AudioManager.GetInstance().PlaySoundAt("Audio/assault_rifle/" + base.Name + "_FiringSound", player.GetTransform().position);
			playSoundTimer.Do();
		}
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
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
			if (((array[i].collider.gameObject.layer != PhysicsLayer.ENEMY) ? gunfire.transform.InverseTransformPoint(array[i].point) : gunfire.transform.InverseTransformPoint(array[i].collider.transform.position)).x < 2f)
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
					bool criticalAttack = false;
					int num3 = Random.Range(0, 100);
					if (num3 < 40)
					{
						criticalAttack = true;
					}
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.hitForce = ray.direction * 2f;
					damageProperty.damage = (int)damage;
					damageProperty.criticalAttack = criticalAttack;
					damageProperty.hitpoint = raycastHit.point;
					damageProperty.isLocal = true;
					damageProperty.wType = WeaponType.AdvancedAssaultRifle;
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
		GameObject gameObject = firelineObjectPool.CreateObject(gunfire.transform.position + normalized2 * 2f, normalized2, Quaternion.identity);
		gameObject.transform.Rotate(180f, 0f, 0f);
		if (!(gameObject == null))
		{
			FireLineScript component2 = gameObject.GetComponent<FireLineScript>();
			component2.transform.Rotate(90f, 0f, 0f);
			component2.beginPos = gunfire.position;
			component2.endPos = raycastHit.point;
		}
		attacked = false;
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
	}
}
