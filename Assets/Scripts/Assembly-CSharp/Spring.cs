using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 11f;

	protected static int sbulletCount;

	protected float life = 5f;

	protected AlphaAnimationScript aas;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.TrackingGun;
	}

	public override string GetAnimationSuffix()
	{
		return "_fist";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_fist";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		AudioManager.GetInstance().PlaySoundAt("Audio/diablo/nailgun_fire", player.GetTransform().position);
	}

	public override void Init(Player player)
	{
		gameWorld = GameApp.GetInstance().GetGameWorld();
		gameCamera = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		cameraComponent = gameCamera.camera;
		cameraTransform = gameCamera.CameraTransform;
		base.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.Weapon);
		CreateGun();
		gun.transform.parent = weaponBoneTrans;
		shootAudio = gun.audio;
		if (shootAudio == null)
		{
		}
		GunOff();
		gunfire = WeaponResourceConfig.GetWeaponGunFire(gun, gunID);
		aas = gun.GetComponent<AlphaAnimationScript>();
		if (aas != null)
		{
			aas.oneShot = true;
			aas.animationSpeed = 10f;
		}
		SetWeaponShootSpeed(1.4f);
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.TRACKINGGUN_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.TRACKINGGUN_BOOTH));
	}

	public override void GunOff()
	{
		base.GunOff();
		StopFire();
	}

	public override void Loop(float deltaTime)
	{
		if (!player.IsLocal())
		{
			return;
		}
		bool flag = player.GetSkills().GetSkill(SkillsType.FLY) > 0f;
		string text = AnimationString.Attack;
		if (flag)
		{
			text = AnimationString.FlyAttack;
		}
		if (attacked || (!player.AnimationPlayed(text + GetAnimationSuffix(), 0.5f) && !player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.5f)))
		{
			return;
		}
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		if (aas != null)
		{
			aas.StartAnimation();
		}
		Ray ray = default(Ray);
		Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - cameraTransform.position).normalized;
		ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT)))
		{
			aimTarget = hitInfo.point;
		}
		else
		{
			aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
		}
		for (int i = 0; i < 5; i++)
		{
			Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
			GameObject original = Resources.Load("Effect/update_effect/effect_arrow_t_purple") as GameObject;
			GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
			GameApp.SpringBulletCount++;
			int num = 0;
			switch (i)
			{
			case 0:
				num = Random.Range(0, 2);
				gameObject.transform.rotation = Quaternion.AngleAxis(5 + num, Vector3.up) * gameObject.transform.rotation;
				break;
			case 1:
				num = Random.Range(-2, 0);
				gameObject.transform.rotation = Quaternion.AngleAxis(-5 + num, Vector3.up) * gameObject.transform.rotation;
				break;
			case 2:
				num = Random.Range(0, 2);
				gameObject.transform.rotation = Quaternion.AngleAxis(5 + num, Vector3.right) * gameObject.transform.rotation;
				break;
			case 3:
				gameObject.transform.rotation = Quaternion.AngleAxis(0f, Vector3.right) * gameObject.transform.rotation;
				break;
			default:
				num = Random.Range(-2, 0);
				gameObject.transform.rotation = Quaternion.AngleAxis(-5 + num, Vector3.right) * gameObject.transform.rotation;
				break;
			}
			ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
			component.dir = gameObject.transform.forward;
			component.flySpeed = rocketFlySpeed;
			component.explodeRadius = bombRange;
			component.hitForce = hitForce;
			component.life = life;
			component.damage = (int)damage;
			component.GunType = WeaponType.TrackingGun;
			component.targetPos = aimTarget;
			component.NeedReflectTime = 3;
			component.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
			int trackingID = -1;
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				component.trackingPlayer = GetAimPlayer();
				if (component.trackingPlayer != null)
				{
					trackingID = component.trackingPlayer.GetUserID();
				}
			}
			else
			{
				component.trackingEnemy = GetAimEnemy();
				if (component.trackingEnemy != null)
				{
					trackingID = component.trackingEnemy.EnemyID;
				}
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerFireRocketRequest request = new PlayerFireRocketRequest(40, gunfire.position, component.dir, trackingID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			AudioManager.GetInstance().PlaySound("Audio/diablo/nailgun_fire");
		}
		attacked = true;
	}

	public Enemy GetAimEnemy()
	{
		Enemy result = null;
		Hashtable enemies = gameWorld.GetEnemies();
		object[] array = new object[enemies.Count];
		enemies.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			Enemy enemy = enemies[array[i]] as Enemy;
			if (enemy.isAiming)
			{
				result = enemy;
				break;
			}
		}
		return result;
	}

	public Player GetAimPlayer()
	{
		Player result = null;
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		Vector2 vector = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null && (!GameApp.GetInstance().GetGameMode().IsTeamMode() || !item.IsSameTeam(player)) && item.isAiming)
			{
				result = item;
				break;
			}
		}
		return result;
	}

	public override void Attack(float deltaTime)
	{
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
		attacked = false;
	}

	public override void StopFire()
	{
	}
}
