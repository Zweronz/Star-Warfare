using System.Collections.Generic;
using UnityEngine;

public class HUDBattle : GameUI, GameUIListener
{
	public enum HUDType
	{
		Coop = 0,
		TDM = 1,
		CTF = 2,
		FFA = 3,
		LL = 4,
		VIP = 5,
		CMI = 6
	}

	public const string BUNDLE_KEY_HUD_UISTATEMANAGER = "key_hud_UIStateManager";

	public const string BUNDLE_KEY_HUD_TYPE = "key_hud_type";

	public const string BUNDLE_KEY_HUD_IEXITROOM = "key_hud_exitroom";

	[SerializeField]
	private UIGrid stContainer;

	[SerializeField]
	private GameObject allUI;

	[SerializeField]
	private GameObject buttonExitRoom;

	[SerializeField]
	private GameObject buttonPause;

	[SerializeField]
	private GameObject stateEnemyProcess;

	[SerializeField]
	private GameObject stateSurvival;

	[SerializeField]
	private GameObject stateBossHp;

	[SerializeField]
	private GameObject stateDoubleBossHp;

	[SerializeField]
	private GameObject stateRemotePlayer;

	[SerializeField]
	private GameObject stateKillInfoManager;

	[SerializeField]
	private GameObject stateAmmo;

	[SerializeField]
	private GameObject statePopulation;

	[SerializeField]
	private GameObject stateTeamScore;

	[SerializeField]
	private GameObject statePlayerSeatIcon;

	[SerializeField]
	private GameObject statePlayerHpLabel;

	[SerializeField]
	private GameObject statePlayerHp;

	[SerializeField]
	private GameObject stateWeapon;

	[SerializeField]
	private GameObject stateJoyStick;

	[SerializeField]
	private GameObject stateAimIcon;

	[SerializeField]
	private GameObject stateSkill;

	[SerializeField]
	private GameObject stateWinScore;

	[SerializeField]
	private GameObject stateWinTime;

	[SerializeField]
	private GameObject stateFFA;

	[SerializeField]
	private GameObject statePersonalPopulation;

	[SerializeField]
	private GameObject stateLL;

	[SerializeField]
	private GameObject stateItem;

	[SerializeField]
	private GameObject stateRebirth;

	public StateWaitVSRebirth StateWaitVSRebirth;

	private Player mPlayer;

	private UserState mUserState;

	private UIStateManager mUIStateManager;

	private Timer mAimTimer = new Timer();

	private IExitRoom mIExitRoom;

	public GameObject ButtonExitRoom
	{
		get
		{
			return buttonExitRoom;
		}
	}

	public GameObject ButtonPause
	{
		get
		{
			return buttonPause;
		}
	}

	public GameObject StateEnemyProcess
	{
		get
		{
			return stateEnemyProcess;
		}
	}

	public GameObject StateSurvival
	{
		get
		{
			return stateSurvival;
		}
	}

	public GameObject StateBossHp
	{
		get
		{
			return stateBossHp;
		}
	}

	public GameObject StateDoubleBossHp
	{
		get
		{
			return stateDoubleBossHp;
		}
	}

	public GameObject StateRemotePlayer
	{
		get
		{
			return stateRemotePlayer;
		}
	}

	public GameObject StateKillInfoManager
	{
		get
		{
			return stateKillInfoManager;
		}
	}

	public GameObject StateAmmo
	{
		get
		{
			return stateAmmo;
		}
	}

	public GameObject StatePopulation
	{
		get
		{
			return statePopulation;
		}
	}

	public GameObject StateTeamScore
	{
		get
		{
			return stateTeamScore;
		}
	}

	public GameObject StateWinScore
	{
		get
		{
			return stateWinScore;
		}
	}

	public GameObject StateWinTime
	{
		get
		{
			return stateWinTime;
		}
	}

	public GameObject StatePlayerSeatIcon
	{
		get
		{
			return statePlayerSeatIcon;
		}
	}

	public GameObject StatePlayerHpLabel
	{
		get
		{
			return statePlayerHpLabel;
		}
	}

	public GameObject StateFFA
	{
		get
		{
			return stateFFA;
		}
	}

	public GameObject StatePersonalPopulation
	{
		get
		{
			return statePersonalPopulation;
		}
	}

	public GameObject StateLL
	{
		get
		{
			return stateLL;
		}
	}

	public GameObject StateItem
	{
		get
		{
			return stateItem;
		}
	}

	public GameObject StateRebirth
	{
		get
		{
			return stateRebirth;
		}
	}

	public UIGrid STContainer
	{
		get
		{
			return stContainer;
		}
	}

	protected override void OnInit(GameUIBundle bundle)
	{
		base.OnInit(bundle);
		mUserState = GameApp.GetInstance().GetUserState();
		mPlayer = GameApp.GetInstance().GetGameWorld().GetPlayer();
		SetListener(this);
		if (bundle != null)
		{
			mIExitRoom = (IExitRoom)bundle.Get("key_hud_exitroom");
			mUIStateManager = (UIStateManager)bundle.Get("key_hud_UIStateManager");
			HUDType type = (HUDType)(int)bundle.Get("key_hud_type");
			LoadSubUI(type);
		}
		InitHP();
		InitWeapon();
		ResetWeaponAim();
		ResetItem();
		InitSkill();
	}

	private void LoadSubUI(HUDType type)
	{
		switch (type)
		{
		case HUDType.Coop:
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				if (mUserState.GetNetStage() < Global.TOTAL_STAGE)
				{
					LoadSubUI(new SurvivalCoopHUDBattle());
				}
				else
				{
					LoadSubUI(new NormalCoopHUDBattle());
				}
			}
			else if (mUserState.GetStage() >= Global.TOTAL_STAGE)
			{
				LoadSubUI(new NormalCoopHUDBattle());
			}
			else if (mUserState.GetSubStage() == Global.TOTAL_SUB_STAGE - 1)
			{
				LoadSubUI(new SurvivalCoopHUDBattle());
			}
			else
			{
				LoadSubUI(new NormalCoopHUDBattle());
			}
			break;
		case HUDType.TDM:
			LoadSubUI(new TDMVersusHUDBattle());
			break;
		case HUDType.CTF:
			LoadSubUI(new CTFVersusHUDBattle());
			break;
		case HUDType.FFA:
			LoadSubUI(new FFAVersusHUDBattle());
			break;
		case HUDType.LL:
			LoadSubUI(new LLVersusHUDBattle());
			break;
		case HUDType.VIP:
			LoadSubUI(new VIPVersusHUDBattle());
			break;
		case HUDType.CMI:
			LoadSubUI(new CMIVersusHUDBattle());
			break;
		}
	}

	protected override void OnUIUpdate()
	{
		base.OnUIUpdate();
		if (InGameFinishState())
		{
			if (!mIsHide)
			{
				GameUIManager.GetInstance().HideUI(this);
				mIsHide = true;
			}
		}
		else if (InWaitRebirthState())
		{
			HideUIWhenWaitingRebirth();
		}
		else
		{
			ResumeUIIfHide();
			UpdateHP();
			UpdateJoyStick();
			UpdateSkill();
		}
	}

	public void OnTouch(GameUITouchEvent touchEvent)
	{
		switch (touchEvent.EventID)
		{
		case TouchEventID.HUD_Pause:
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				AudioManager.GetInstance().PlaySound(AudioName.PAUSE);
				mUIStateManager.FrGoToPhase(14, false, true, false);
				if (!Application.isMobilePlatform)
				{
					Screen.lockCursor = false;
				}
			}
			else
			{
				mIExitRoom.ExitRoom();
				if (!Application.isMobilePlatform)
				{
					Screen.lockCursor = false;
				}
			}
			break;
		case TouchEventID.HUD_Switch_Weapon:
		{
			AudioManager.GetInstance().PlaySound(AudioName.MOUNT_WEAPON);
			int num2 = (int)touchEvent.ArgObject;
			if (num2 > -1)
			{
				mPlayer.ChangeWeaponInBag(num2);
			}
			ResetWeaponAim();
			break;
		}
		case TouchEventID.HUD_Item_Pop:
			UserStateUI.GetInstance().PopItemList(false);
			break;
		case TouchEventID.HUD_Item:
		{
			int num2 = (int)touchEvent.ArgObject;
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			if (mPlayer.UseItem((byte)num2))
			{
				ResetItem();
			}
			break;
		}
		case TouchEventID.HUD_Skill:
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			int num = (int)touchEvent.ArgObject;
			if (mPlayer.IsPowerUpCoolDown(num))
			{
				mPlayer.PowerUp(true, num);
			}
			break;
		}
		case TouchEventID.HUD_Rebirth:
			StateWaitVSRebirth.DoRebirth();
			break;
		}
	}

	private void InitHP()
	{
		Debug.Log("player.Hp : " + mPlayer.Hp);
		Debug.Log("player.MaxHp : " + mPlayer.MaxHp);
		UserStateUI.GetInstance().SetHpandMaxHp(mPlayer.Hp, mPlayer.MaxHp);
	}

	private void InitWeapon()
	{
		List<Weapon> battleWeapons = mUserState.GetBattleWeapons();
		int weaponIndex = battleWeapons.IndexOf(mPlayer.GetWeapon());
		UserStateUI.GetInstance().SetWeaponList(battleWeapons);
		UserStateUI.GetInstance().SetWeaponIndex(weaponIndex);
	}

	private void ResetWeaponAim()
	{
		UserStateUI.GetInstance().SetAimId(mPlayer.GetWeapon().AimID);
	}

	private void ResetItem()
	{
		UserStateUI.GetInstance().SetItemPosition(mUserState.GetBagPosition());
	}

	private void UpdateHP()
	{
		if (mPlayer.Hp < 0)
		{
			mPlayer.Hp = 0;
		}
		UserStateUI.GetInstance().UpdateHp(mPlayer.Hp);
	}

	private void UpdateJoyStick()
	{
		float thumbRadius = mPlayer.inputController.ThumbRadius;
		Vector2 lastTouchPos = mPlayer.inputController.LastTouchPos;
		Vector2 lastShootTouch = mPlayer.inputController.LastShootTouch;
		Vector2 vector = lastTouchPos - mPlayer.inputController.ThumbCenterToScreen;
		Vector2 moveJoyStickPos = vector;
		UserStateUI.GetInstance().SetMoveJoyStickPos(moveJoyStickPos);
		Vector2 vector2 = lastShootTouch - mPlayer.inputController.ShootThumbCenterToScreen;
		Vector2 shootJoyStickPos = vector2;
		UserStateUI.GetInstance().SetShootJoyStickPos(shootJoyStickPos);
	}

	private void InitSkill()
	{
		List<UserStateUI.SkillUI> list = new List<UserStateUI.SkillUI>();
		float skill = mPlayer.GetSkills().GetSkill(SkillsType.POWER_UP);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(0));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.SPEED_UP);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(1));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.DEFENCE_UP);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(2));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.ANDROMEDA_UP);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(3));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.HEALTH_STEAL);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(4));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.ATTACK_SHIELD);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(5));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.IMPACT_WAVE);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(6));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.TRACK_WAVE);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(7));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.HURT_HEALTH);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(8));
		}
		skill = mPlayer.GetSkills().GetSkill(SkillsType.GRAVITY_FORCE);
		if (skill != 0f)
		{
			list.Add(UserStateUI.SkillUI.CreateSkill(9));
		}
		UserStateUI.GetInstance().SetSkillList(list);
	}

	private void UpdateSkill()
	{
		foreach (UserStateUI.SkillUI skill in UserStateUI.GetInstance().GetSkillList())
		{
			if (mPlayer.IsPowerUpCoolDown(skill.SkillId))
			{
				if (!skill.IsUse && skill.IsCoolDown && !skill.IsJustCoolDownFinish)
				{
					skill.IsJustCoolDownFinish = true;
					skill.IsCoolDown = false;
					skill.IsUse = false;
				}
				continue;
			}
			skill.IsJustCoolDownFinish = false;
			Timer powerTimer = mPlayer.GetPowerTimer(skill.SkillId);
			if (!powerTimer.Ready())
			{
				skill.IsUse = true;
				skill.PercentOfUsingTime = powerTimer.GetTimeSpan() / powerTimer.GetInterval();
				skill.IsCoolDown = false;
				continue;
			}
			Timer powerCDTimer = mPlayer.GetPowerCDTimer(skill.SkillId);
			if (!powerCDTimer.Ready())
			{
				skill.IsUse = false;
				skill.PercentOfCoolDownTime = (powerCDTimer.GetTimeSpan() - powerTimer.GetTimeSpan()) / (powerCDTimer.GetInterval() - powerTimer.GetInterval());
				skill.IsCoolDown = true;
			}
		}
	}

	public void HandleRemotePlayer()
	{
		List<UserStateUI.RemotePlayerUI> list = new List<UserStateUI.RemotePlayerUI>();
		List<UserStateUI.RemotePlayerUI> remotePlayerList = UserStateUI.GetInstance().GetRemotePlayerList();
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (UserStateUI.RemotePlayerUI item in remotePlayerList)
		{
			if (item.RemotePlayer.GetGameObject() == null || !item.RemotePlayer.GetGameObject().active || !remotePlayers.Contains(item.RemotePlayer))
			{
				list.Add(item);
			}
		}
		foreach (UserStateUI.RemotePlayerUI item2 in list)
		{
			UserStateUI.GetInstance().RemoveRemotePlayer(item2);
		}
		for (int i = 0; i < remotePlayers.Count; i++)
		{
			if (remotePlayers[i].GetGameObject() != null && remotePlayers[i].GetGameObject().active)
			{
				UserStateUI.GetInstance().AddRemotePlayerIfNotExist(remotePlayers[i]);
			}
		}
	}

	public bool InWaitRebirthState()
	{
		return mPlayer.State == Player.DEAD_STATE || mPlayer.State == Player.WAIT_REBIRTH_STATE || mPlayer.State == Player.WAIT_VS_REBIRTH_STATE;
	}

	private void HideUIWhenWaitingRebirth()
	{
		if (statePlayerHp.activeSelf)
		{
			statePlayerHp.SetActive(false);
		}
		if (stateWeapon.activeSelf)
		{
			stateWeapon.SetActive(false);
		}
		if (stateJoyStick.activeSelf)
		{
			stateJoyStick.SetActive(false);
		}
		if (stateAimIcon.activeSelf)
		{
			stateAimIcon.SetActive(false);
		}
		if (stateSkill.activeSelf)
		{
			stateSkill.SetActive(false);
		}
	}

	private void ResumeUIIfHide()
	{
		if (!statePlayerHp.activeSelf)
		{
			statePlayerHp.SetActive(true);
		}
		if (!stateWeapon.activeSelf)
		{
			stateWeapon.SetActive(true);
		}
		if (!stateJoyStick.activeSelf)
		{
			stateJoyStick.SetActive(true);
		}
		if (!stateAimIcon.activeSelf)
		{
			stateAimIcon.SetActive(true);
		}
		if (!stateSkill.activeSelf)
		{
			stateSkill.SetActive(true);
		}
	}

	protected override bool IsCanInit()
	{
		return GameApp.GetInstance().GetGameWorld() != null && GameApp.GetInstance().GetGameWorld().GetPlayer() != null;
	}

	public bool InGameFinishState()
	{
		return mPlayer.State == Player.WIN_STATE || mPlayer.State == Player.LOSE_STATE;
	}

	protected override void OnShow()
	{
		base.OnShow();
		allUI.SetActive(true);
		mIsHide = false;
		mPlayer.InputController.BlockCamera = false;
	}

	protected override void OnHide()
	{
		base.OnHide();
		allUI.SetActive(false);
	}

	protected override void OnUIDestroy()
	{
		base.OnUIDestroy();
		UserStateUI.GetInstance().Clear();
	}
}
