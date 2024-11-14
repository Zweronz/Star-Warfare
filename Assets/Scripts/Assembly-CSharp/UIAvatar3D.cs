using UnityEngine;

public class UIAvatar3D : UI3DFrame
{
	protected Vector3 scale;

	protected float lastMotionTime;

	protected Weapon currentWeapon;

	private int[] avatar = new int[6];

	protected bool enableFly;

	protected string idleAnimationName;

	protected bool justCreateNewAvatar;

	public UIAvatar3D(Rect rect, Vector3 pos, Vector3 scale, Vector3 rotation)
		: base(rect, pos, rotation)
	{
		this.scale = scale;
		ChangeAvatar(GetAvatar());
	}

	public int[] GetAvatar()
	{
		int[] array = GameApp.GetInstance().GetUserState().GetAvatar();
		for (int i = 0; i < array.Length; i++)
		{
			avatar[i] = array[i];
		}
		Weapon weapon = GameApp.GetInstance().GetUserState().GetBattleWeapons()[0];
		avatar[5] = weapon.GetGunID();
		return avatar;
	}

	public void ChangeAvatar(int[] avatar)
	{
		ClearModels();
		Armor armor = GameApp.GetInstance().GetUserState().GetArmor(4, avatar[4]);
		enableFly = armor.GetSkill(SkillsType.FLY) > 0f;
		currentWeapon = GameApp.GetInstance().GetUserState().GetWeapons()[avatar[5]];
		GameObject gameObject = AvatarBuilder.GetInstance().CreateUIAvatar(avatar);
		gameObject.transform.rotation = Quaternion.Euler(m_Rotation);
		Transform parent = gameObject.transform.Find(BoneName.Weapon);
		if (currentWeapon.GetWeaponType() == WeaponType.LightBow || currentWeapon.GetWeaponType() == WeaponType.AutoBow || currentWeapon.GetWeaponType() == WeaponType.TheArrow)
		{
			parent = gameObject.transform.Find(BoneName.WeaponL);
		}
		string path = string.Format("NoMipMapRes/Weapon/gun{0:D2}", avatar[5]);
		Object original = Resources.Load(path);
		GameObject gameObject2 = (GameObject)Object.Instantiate(original, gameObject.transform.position, gameObject.transform.rotation);
		WeaponResourceConfig.RotateGun(gameObject2, currentWeapon.GunID);
		gameObject2.transform.parent = parent;
		gameObject.transform.localScale = scale;
		if (currentWeapon.GetWeaponType() == WeaponType.MachineGun || currentWeapon.GetWeaponType() == WeaponType.AdvancedMachineGun)
		{
			idleAnimationName = AnimationString.UIIdleMachineGun;
		}
		else
		{
			idleAnimationName = AnimationString.UIIdle;
		}
		SetGameObject(gameObject);
		gameObject.animation[idleAnimationName].wrapMode = WrapMode.Loop;
		gameObject.animation[AnimationString.Idle + currentWeapon.GetAnimationSuffixAlter()].wrapMode = WrapMode.Loop;
		if (!enableFly)
		{
			gameObject.animation.Play(AnimationString.Idle + currentWeapon.GetAnimationSuffixAlter());
		}
		SetModel(gameObject);
		gameObject.animation.Play(idleAnimationName);
		gameObject.animation.Play(AnimationString.Idle + "_rifle");
		justCreateNewAvatar = true;
	}

	public new void Clear()
	{
		ClearModels();
	}

	public void SetupAnimations()
	{
	}

	public new void Update()
	{
		if (justCreateNewAvatar)
		{
			UpdateAnimation();
			justCreateNewAvatar = false;
		}
		if (currentWeapon != null && (currentWeapon.GetWeaponType() == WeaponType.AutoRocketLauncher || currentWeapon.GetWeaponType() == WeaponType.RocketLauncher || currentWeapon.GetWeaponType() == WeaponType.LightBow || currentWeapon.GetWeaponType() == WeaponType.AutoBow || currentWeapon.GetWeaponType() == WeaponType.TheArrow))
		{
			return;
		}
		GameObject model = GetModel();
		if (Time.time - lastMotionTime > 10f)
		{
			if (enableFly)
			{
				model.animation.CrossFade(AnimationString.FlyIdle, 0.2f);
				model.animation.CrossFade(AnimationString.Fly + "_" + idleAnimationName, 0.2f);
			}
			else
			{
				model.animation.CrossFade(idleAnimationName, 0.6f);
			}
			lastMotionTime = Time.time;
		}
		if (Time.time - lastMotionTime > model.animation[idleAnimationName].clip.length)
		{
			Weapon weapon = GameApp.GetInstance().GetUserState().GetBattleWeapons()[0];
			if (enableFly)
			{
				model.animation.CrossFade(AnimationString.FlyIdle, 0.2f);
				model.animation.CrossFade(AnimationString.FlyIdle + currentWeapon.GetAnimationSuffixAlter(), 0.2f);
			}
			else
			{
				model.animation.CrossFade(AnimationString.Idle + currentWeapon.GetAnimationSuffixAlter(), 0.2f);
			}
		}
	}

	public void UpdateAnimation()
	{
		GameObject model = GetModel();
		Weapon weapon = GameApp.GetInstance().GetUserState().GetBattleWeapons()[0];
		model.animation.CrossFade(idleAnimationName, 0.2f);
		if (enableFly)
		{
			model.animation.CrossFade(AnimationString.FlyIdle, 0.2f);
			model.animation.CrossFade(AnimationString.FlyIdle + currentWeapon.GetAnimationSuffixAlter(), 0.2f);
		}
		else
		{
			model.animation.CrossFade(AnimationString.Idle + currentWeapon.GetAnimationSuffixAlter(), 0.2f);
		}
	}

	public void AddMixingTransformAnimation(GameObject playerObj, string weaponSuffix)
	{
		Transform transform = playerObj.transform;
		if (playerObj.animation[AnimationString.FlyIdle + weaponSuffix] != null)
		{
			playerObj.animation[AnimationString.FlyIdle + weaponSuffix].AddMixingTransform(transform.Find(BoneName.UpperBody));
			playerObj.animation[AnimationString.FlyIdle + weaponSuffix].wrapMode = WrapMode.Loop;
		}
	}

	public void SetLowerBodyAnimation(GameObject playerObj, string weaponSuffix)
	{
		if (playerObj.animation[AnimationString.Idle + weaponSuffix] != null)
		{
			playerObj.animation[AnimationString.Idle + weaponSuffix].wrapMode = WrapMode.Loop;
		}
	}

	public void SetGameObject(GameObject playerObj)
	{
		playerObj.animation[AnimationString.FlyIdle].layer = -1;
		playerObj.animation[AnimationString.FlyIdle].wrapMode = WrapMode.Loop;
		SetLowerBodyAnimation(playerObj, "_rifle");
		SetLowerBodyAnimation(playerObj, "_shotgun");
		SetLowerBodyAnimation(playerObj, "_bazinga");
		SetLowerBodyAnimation(playerObj, "_grenade_launcher");
		SetLowerBodyAnimation(playerObj, "_laser");
		SetLowerBodyAnimation(playerObj, "_bow");
		SetLowerBodyAnimation(playerObj, "_fist");
		SetLowerBodyAnimation(playerObj, "_Sniper");
		AddMixingTransformAnimation(playerObj, "_rifle");
		AddMixingTransformAnimation(playerObj, "_shotgun");
		AddMixingTransformAnimation(playerObj, "_bazinga");
		AddMixingTransformAnimation(playerObj, "_grenade_launcher");
		AddMixingTransformAnimation(playerObj, "_laser");
		AddMixingTransformAnimation(playerObj, "_bow");
		AddMixingTransformAnimation(playerObj, "_fist");
		AddMixingTransformAnimation(playerObj, "_Sniper");
		Transform transform = playerObj.transform;
		if (playerObj.animation[AnimationString.Fly + "_" + AnimationString.UIIdle] != null)
		{
			playerObj.animation[AnimationString.Fly + "_" + AnimationString.UIIdle].AddMixingTransform(transform.Find(BoneName.UpperBody));
			playerObj.animation[AnimationString.Fly + "_" + AnimationString.UIIdle].wrapMode = WrapMode.Loop;
		}
		if (playerObj.animation[AnimationString.Fly + "_" + AnimationString.UIIdleMachineGun] != null)
		{
			playerObj.animation[AnimationString.Fly + "_" + AnimationString.UIIdleMachineGun].AddMixingTransform(transform.Find(BoneName.UpperBody));
			playerObj.animation[AnimationString.Fly + "_" + AnimationString.UIIdleMachineGun].wrapMode = WrapMode.Loop;
		}
		if (enableFly)
		{
			AddMixingTransformAnimation(playerObj, "_machinegun");
			AddMixingTransformAnimation(playerObj, "_jian");
		}
	}
}
