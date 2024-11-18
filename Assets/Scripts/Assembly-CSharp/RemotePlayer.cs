using UnityEngine;

public class RemotePlayer : Player
{
	protected NetworkTransformInterpolation interpolation;

	public RemotePlayer()
	{
		interpolation = new NetworkTransformInterpolation(this);
		userState = new UserState();
	}

	public UserState GetUserState()
	{
		return userState;
	}

	public NetworkTransformInterpolation GetInterpolation()
	{
		return interpolation;
	}

	public void UpdateTransform(Vector3 pos, float eulerAnglesY, int timeStamp, Transform transform)
	{
		bool run = true;
		if (base.inputController.inputInfo.moveDirection == Vector3.zero)
		{
			run = false;
		}
		NetworkTransform nTrans = new NetworkTransform(pos, eulerAnglesY, timeStamp, run);
		interpolation.SetTransform(transform);
		interpolation.ReceiveTransform(nTrans);
	}

	public override void Loop(float deltaTime)
	{
		base.Loop(deltaTime);
		base.State.NextState(this, deltaTime);
		interpolation.Loop();
		if (!base.inputController.inputInfo.fire)
		{
			base.TargetAngleV = 0f;
		}
		base.AngleV = Mathf.LerpAngle(base.AngleV, base.TargetAngleV, deltaTime * 10f);
	}

	public void CreateUserState(byte[] weaponIDs, byte[] armorIDs)
	{
		byte[] ownedWeapons = new byte[47];
		byte[] ownedHeads = new byte[Global.TOTAL_ARMOR_HEAD_NUM];
		byte[] ownedBodies = new byte[Global.TOTAL_ARMOR_BODY_NUM];
		byte[] ownedArms = new byte[Global.TOTAL_ARMOR_ARM_NUM];
		byte[] ownedFeet = new byte[Global.TOTAL_ARMOR_FOOT_NUM];
		byte[] ownedBag = new byte[Global.TOTAL_ARMOR_BAG_NUM];
		userState.InitArmors(ownedHeads, ownedBodies, ownedArms, ownedFeet, ownedBag);
		userState.InitArmorRewards();
		userState.InitWeapons(ownedWeapons);
		userState.SetBagPosition(weaponIDs);
		userState.SetAvatar(armorIDs);
	}

	public override void Init()
	{
		InitSkills();
		base.Init();
		playerObj.name = userID.ToString();
		playerObj.layer = PhysicsLayer.REMOTE_PLAYER;
		PlayAnimation(AnimationString.Run + "_rifle", WrapMode.Loop);
		PlayAnimation(AnimationString.Idle + "_rifle", WrapMode.Loop);
		Transform transform = playerTransform.Find(BoneName.Bag).Find("Bag");
		if (transform != null)
		{
			transform.localPosition = Vector3.zero;
		}
		base.MaxHp += (int)playerSkill.GetSkill(SkillsType.HP_BOOTH);
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			base.MaxHp = VSMath.GetHpInVS(base.MaxHp);
		}
		base.Hp = base.MaxHp;
	}

	public override void ChangeWeaponInBag(int bagIndex)
	{
		Weapon weaponInBag = userState.GetWeaponInBag(bagIndex);
		if (weaponInBag != null)
		{
			ChangeWeapon(weaponInBag);
		}
	}
}
