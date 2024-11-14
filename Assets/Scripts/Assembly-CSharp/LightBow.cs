using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBow : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 22f;

	protected static int sbulletCount;

	public override int BulletCount
	{
		get
		{
			return sbulletCount;
		}
		set
		{
			sbulletCount = value;
		}
	}

	public LightBow()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.LightBow;
	}

	public override string GetAnimationSuffix()
	{
		return "_bow";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_bow";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void Init(Player player)
	{
		gameWorld = GameApp.GetInstance().GetGameWorld();
		gameCamera = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		cameraComponent = gameCamera.camera;
		cameraTransform = gameCamera.CameraTransform;
		base.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.WeaponL);
		CreateGun();
		gun.transform.parent = weaponBoneTrans;
		shootAudio = gun.audio;
		if (shootAudio == null)
		{
		}
		GunOff();
		gunfire = WeaponResourceConfig.GetWeaponGunFire(gun, gunID);
		SetWeaponShootSpeed(1.4f);
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.BOW_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.BOW_BOOTH));
	}

	public override void GunOn()
	{
		gun.transform.GetChild(0).renderer.enabled = true;
		gun.transform.GetChild(1).renderer.enabled = true;
		gun.transform.GetChild(2).renderer.enabled = true;
	}

	public override void GunOff()
	{
		if (gun != null)
		{
			gun.transform.GetChild(0).renderer.enabled = false;
			gun.transform.GetChild(1).renderer.enabled = false;
			gun.transform.GetChild(2).renderer.enabled = false;
		}
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
		AudioManager.GetInstance().PlaySoundAt("Audio/specialweapon/lightbow_shot", player.GetTransform().position);
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
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
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		GameObject original = Resources.Load("Effect/LightBow_Shot") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = normalized2;
		component.flySpeed = rocketFlySpeed;
		component.explodeRadius = bombRange;
		component.hitForce = hitForce;
		component.life = 8f;
		component.damage = (int)damage;
		component.GunType = WeaponType.LightBow;
		component.targetPos = aimTarget;
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
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(9, gunfire.position, normalized2, trackingID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
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
