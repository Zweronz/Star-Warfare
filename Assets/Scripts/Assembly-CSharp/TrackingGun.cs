using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingGun : Weapon
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
		gameCamera = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		cameraComponent = gameCamera.GetComponent<Camera>();
		cameraTransform = gameCamera.CameraTransform;
		base.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.Weapon);
		CreateGun();
		gun.transform.parent = weaponBoneTrans;
		shootAudio = gun.GetComponent<AudioSource>();
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

	public Player GetNearestAimPlayer()
	{
		Player result = null;
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		Vector2 b = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		float num = 1E+10f;
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item == null || (GameApp.GetInstance().GetGameMode().IsTeamMode() && item.IsSameTeam(player)))
			{
				continue;
			}
			Vector3 vector = Camera.main.WorldToScreenPoint(item.GetTransform().position);
			Vector2 a = new Vector2(vector.x, vector.y);
			if (!(a.x > 0f) || !(a.x < (float)Screen.width) || !(a.y > 0f) || !(a.y < (float)Screen.height) || !(vector.z > 0f))
			{
				continue;
			}
			Camera mainCamera = Camera.main;
			Transform transform = mainCamera.transform;
			ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
			Ray ray = default(Ray);
			Vector3 normalized = (item.GetTransform().position + Vector3.up * 1f - transform.position).normalized;
			ray = new Ray(transform.position + normalized * 1.8f, normalized);
			RaycastHit hitInfo = default(RaycastHit);
			if (!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) || hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				float num2 = Vector2.Distance(a, b);
				if (num2 < num)
				{
					num = num2;
					result = item;
				}
			}
		}
		return result;
	}

	public Enemy GetNearestAimEnemy()
	{
		Enemy result = null;
		if (GameApp.GetInstance().GetGameMode().IsSingle() || GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			Hashtable enemies = gameWorld.GetEnemies();
			object[] array = new object[enemies.Count];
			enemies.Keys.CopyTo(array, 0);
			Vector2 b = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			float num = 1E+10f;
			for (int i = 0; i < array.Length; i++)
			{
				Enemy enemy = enemies[array[i]] as Enemy;
				Vector3 vector = Camera.main.WorldToScreenPoint(enemy.GetTransform().position);
				Vector2 a = new Vector2(vector.x, vector.y);
				if (!(a.x > 0f) || !(a.x < (float)Screen.width) || !(a.y > 0f) || !(a.y < (float)Screen.height) || !(vector.z > 0f))
				{
					continue;
				}
				Camera mainCamera = Camera.main;
				Transform transform = mainCamera.transform;
				ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
				Ray ray = default(Ray);
				Vector3 normalized = (enemy.GetColliderCenterPosition() - transform.position).normalized;
				ray = new Ray(transform.position + normalized * 1.8f, normalized);
				RaycastHit hitInfo = default(RaycastHit);
				if (!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) || hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY)
				{
					float num2 = Vector2.Distance(a, b);
					if (num2 < num)
					{
						num = num2;
						result = enemy;
					}
				}
			}
		}
		return result;
	}

	public CMIGift GetNearestAimGift()
	{
		CMIGift result = null;
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			List<CMIGift> cMIGifts = GameApp.GetInstance().GetGameWorld().GetCMIGifts();
			Vector2 b = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			float num = 1E+10f;
			for (int i = 0; i < cMIGifts.Count; i++)
			{
				CMIGift cMIGift = cMIGifts[i];
				Vector3 vector = Camera.main.WorldToScreenPoint(cMIGift.GetPosition());
				Vector2 a = new Vector2(vector.x, vector.y);
				if (!(a.x > 0f) || !(a.x < (float)Screen.width) || !(a.y > 0f) || !(a.y < (float)Screen.height) || !(vector.z > 0f))
				{
					continue;
				}
				Camera mainCamera = Camera.main;
				Transform transform = mainCamera.transform;
				ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
				Ray ray = default(Ray);
				Vector3 normalized = (cMIGift.GetPosition() - transform.position).normalized;
				ray = new Ray(transform.position + normalized * 1.8f, normalized);
				RaycastHit hitInfo = default(RaycastHit);
				if (!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT)) || hitInfo.collider.gameObject.layer == PhysicsLayer.GIFT)
				{
					float num2 = Vector2.Distance(a, b);
					if (num2 < num)
					{
						num = num2;
						result = cMIGift;
					}
				}
			}
		}
		return result;
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
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		GameObject original = Resources.Load("Effect/update_effect/effect_arrow_t") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = normalized2;
		component.flySpeed = rocketFlySpeed;
		component.explodeRadius = bombRange;
		component.hitForce = hitForce;
		component.life = life;
		component.damage = (int)damage;
		component.GunType = WeaponType.TrackingGun;
		component.targetPos = aimTarget;
		component.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		int trackingID = -1;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			component.trackingPlayer = GetNearestAimPlayer();
			if (component.trackingPlayer != null)
			{
				trackingID = component.trackingPlayer.GetUserID();
			}
			else
			{
				component.trackingGift = GetNearestAimGift();
				if (component.trackingGift != null)
				{
					trackingID = component.trackingGift.GetId();
				}
			}
		}
		else
		{
			component.trackingEnemy = GetNearestAimEnemy();
			if (component.trackingEnemy != null)
			{
				trackingID = component.trackingEnemy.EnemyID;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(19, gunfire.position, normalized2, trackingID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		AudioManager.GetInstance().PlaySound("Audio/diablo/nailgun_fire");
		attacked = true;
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
