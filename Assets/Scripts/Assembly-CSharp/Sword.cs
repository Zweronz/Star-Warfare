using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
	protected Timer swordTimer = new Timer();

	protected int attackPhase = -1;

	protected float lastMakeOneHitTime;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.Sword;
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override string GetAnimationSuffix()
	{
		return "_jian";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_jian";
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.SWORD_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.SWORD_BOOTH));
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
		swordTimer.SetTimer(0.2f, true);
	}

	public void MakeOneHit(int index)
	{
		if (!(Time.time - lastMakeOneHitTime > 0.3f))
		{
			return;
		}
		lastMakeOneHitTime = Time.time;
		Hashtable enemies = GameApp.GetInstance().GetGameWorld().GetEnemies();
		bool flag = false;
		foreach (Enemy value in enemies.Values)
		{
			if (value.GetState() != Enemy.GRAVEBORN_STATE && value.GetState() != Enemy.DEAD_STATE)
			{
				Vector3 vector = player.GetTransform().InverseTransformPoint(value.GetTransform().position);
				if (Vector3.Distance(player.GetTransform().position, value.GetTransform().position) < 3.5f && vector.z > 0f)
				{
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.damage = (int)damage;
					damageProperty.wType = WeaponType.Sword;
					damageProperty.hitpoint = value.GetTransform().position + Vector3.up;
					damageProperty.criticalAttack = true;
					damageProperty.isLocal = true;
					value.HitEnemy(damageProperty);
					flag = true;
				}
			}
		}
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			List<CMIGift> cMIGifts = GameApp.GetInstance().GetGameWorld().GetCMIGifts();
			foreach (CMIGift item in cMIGifts)
			{
				if (item != null)
				{
					Vector3 vector2 = player.GetTransform().InverseTransformPoint(item.GetPosition());
					if (Vector3.Distance(player.GetTransform().position, item.GetPosition()) < 3.5f && vector2.z > 0f)
					{
						PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, item.GetId(), false, (byte)GetWeaponType());
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item2 in remotePlayers)
			{
				if (GameApp.GetInstance().GetGameMode().IsTeamMode())
				{
					Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(item2.GetUserID());
					if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
					{
						continue;
					}
				}
				Vector3 vector3 = player.GetTransform().InverseTransformPoint(item2.GetTransform().position);
				if (Vector3.Distance(player.GetTransform().position, item2.GetTransform().position) < 3.5f && vector3.z > 0f)
				{
					PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest((short)damage, item2.GetUserID(), false, (byte)GetWeaponType());
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					flag = true;
				}
			}
		}
		if (flag)
		{
			int num = Random.Range(1, 4);
			AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/light_sword0" + num, gun.transform.position);
		}
		else
		{
			AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/light_sword_swing0" + index, gun.transform.position);
		}
		MakeExtraHit(index);
	}

	public virtual void MakeExtraHit(int index)
	{
	}

	protected virtual void CreateSwordTrail()
	{
	}

	public override void Loop(float deltaTime)
	{
		NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
		Transform transform = player.GetTransform();
		if (attackPhase == -1 && ((player.IsPlayingAnimation(AnimationString.Attack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.Attack + GetAnimationSuffix(), 0.2f)) || (player.IsPlayingAnimation(AnimationString.RunAttack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.2f)) || (player.IsPlayingAnimation(AnimationString.FlyAttack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.FlyAttack + GetAnimationSuffix(), 0.2f)) || (player.IsPlayingAnimation(AnimationString.FlyRunAttack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.FlyRunAttack + GetAnimationSuffix(), 0.2f))))
		{
			attackPhase = 0;
			CreateSwordTrail();
		}
		else if (attackPhase == 0 && (player.AnimationPlayed(AnimationString.Attack + GetAnimationSuffix(), 0.2f) || player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.2f) || player.AnimationPlayed(AnimationString.FlyAttack + GetAnimationSuffix(), 0.2f) || player.AnimationPlayed(AnimationString.FlyRunAttack + GetAnimationSuffix(), 0.2f)))
		{
			if (player.IsLocal())
			{
				MakeOneHit(1);
			}
			attackPhase = 1;
		}
		else if (attackPhase == 1 && (player.AnimationPlayed(AnimationString.Attack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.FlyAttack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.FlyRunAttack + GetAnimationSuffix(), 0.5f)))
		{
			if (player.IsLocal())
			{
				MakeOneHit(2);
			}
			attackPhase = -1;
		}
	}

	public override void Attack(float deltaTime)
	{
		attacked = false;
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
	}

	public override void StopFire()
	{
		if (shootAudio != null)
		{
			shootAudio.Stop();
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
}
