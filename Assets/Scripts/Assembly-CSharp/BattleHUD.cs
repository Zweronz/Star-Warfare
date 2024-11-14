using System.Collections.Generic;
using UnityEngine;

public abstract class BattleHUD : UIHandler, IUIHandle, IExitRoom
{
	protected const int JOYSTICK_R = 46;

	protected const byte STATE_INIT = 0;

	protected const byte STATE_CREATE = 1;

	protected const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	protected Player player;

	protected UserState userState;

	protected UIImage battleImg;

	protected UIImage shootjoystickbgimg;

	protected UIImage joystickbgimg;

	protected UIImage joystickImg;

	protected UIImage shootjoystickInnerImg;

	protected UIImage shootjoystickOuterImg;

	protected UIImage shootjoystickImg;

	protected UIImage hpImg;

	protected UIImage hpBGImg;

	protected UIImage hpEFXImg;

	protected UISliderItem swapWeapon;

	protected UIProps useProps;

	protected UIClickButton menuBtn;

	protected UIImage aimImg;

	protected UIImage aimOnFireImg;

	protected float aimWidth;

	protected float aimHeight;

	protected Timer aimTimer = new Timer();

	protected List<SkillUI> skillLst = new List<SkillUI>();

	protected int joystick;

	public static byte JOYSTICK_IMG = 15;

	public static byte SHOOTJOYSTICK_BG_IMG = 10;

	public static byte JOYSTICK_BG_IMG = 13;

	public static byte SHOOTJOYSTICK_INNER_IMG = 11;

	public static byte SHOOTJOYSTICK_OUTER_IMG = 10;

	public static byte SHOOTJOYSTICK_FG_IMG = 12;

	protected byte state;

	protected static byte[] BG_IMG = new byte[2] { 10, 13 };

	protected static byte HP_BG_IMG = 22;

	protected static byte HP_IMG = 24;

	protected static byte HP_EFX_IMG = 23;

	protected static byte[] MENU_SOLO_NORMAL_IMG = new byte[2] { 3, 4 };

	protected static byte[] MENU_SOLO_PRESSED_IMG = new byte[2] { 3, 4 };

	protected static byte[] MENU_COOP_NORMAL_IMG = new byte[2] { 3, 35 };

	protected static byte[] MENU_COOP_PRESSED_IMG = new byte[2] { 3, 34 };

	protected Timer timer = new Timer();

	protected bool isIpad;

	protected bool adaptived = true;

	protected float destAngle;

	protected byte frameIdx;

	protected int vsFrameIdx = 6;

	protected Rect guideClip;

	protected float mLastUpdateAimTime;

	protected MessageBoxUI msgUI;

	public void Init()
	{
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
		timer.SetTimer(0.3f, true);
		Create();
	}

	public virtual void Close()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public virtual bool Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			player = gameWorld.GetPlayer();
			if (player != null)
			{
				player.InputController.BlockCamera = false;
			}
		}
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		return true;
	}

	protected bool CreateSkillUI()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[17];
		if (unitUI == null)
		{
			return false;
		}
		int num = 0;
		Rect rect = unitUI.GetModulePositionRect(0, frameIdx, 39);
		if (UIConstant.Is16By9())
		{
			rect = new Rect((float)Screen.width - rect.width, rect.y, rect.width, rect.height);
		}
		float skill = player.GetSkills().GetSkill(SkillsType.POWER_UP);
		if (skill != 0f)
		{
			SkillUI skillUI = new SkillUI(0, frameIdx, 41);
			skillUI.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI);
			num += (int)rect.height;
		}
		skill = player.GetSkills().GetSkill(SkillsType.SPEED_UP);
		if (skill != 0f)
		{
			SkillUI skillUI2 = new SkillUI(1, frameIdx, 43);
			skillUI2.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI2);
			num += (int)rect.height;
		}
		skill = player.GetSkills().GetSkill(SkillsType.DEFENCE_UP);
		if (skill != 0f)
		{
			SkillUI skillUI3 = new SkillUI(2, frameIdx, 45);
			skillUI3.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI3);
			num += (int)rect.height;
		}
		skill = player.GetSkills().GetSkill(SkillsType.ANDROMEDA_UP);
		if (skill != 0f)
		{
			SkillUI skillUI4 = new SkillUI(3, frameIdx, 47);
			skillUI4.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI4);
			num += (int)rect.height;
		}
		skill = player.GetSkills().GetSkill(SkillsType.HEALTH_STEAL);
		if (skill != 0f)
		{
			SkillUI skillUI5 = new SkillUI(4, frameIdx, 49);
			skillUI5.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI5);
			num += (int)rect.height;
		}
		skill = player.GetSkills().GetSkill(SkillsType.ATTACK_SHIELD);
		if (skill != 0f)
		{
			SkillUI skillUI6 = new SkillUI(5, frameIdx, 51);
			skillUI6.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI6);
			num += (int)rect.height;
		}
		skill = player.GetSkills().GetSkill(SkillsType.HURT_HEALTH);
		if (skill != 0f)
		{
			SkillUI skillUI7 = new SkillUI(8, frameIdx, 53);
			skillUI7.Rect = new Rect(rect.x, rect.y + (float)num, rect.width, rect.height);
			skillLst.Add(skillUI7);
			num += (int)rect.height;
		}
		foreach (SkillUI item in skillLst)
		{
			stateMgr.m_UIManager.Add(item);
		}
		return true;
	}

	public void ResetUI()
	{
		state = 2;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public bool Update()
	{
		return false;
	}

	private void UpdateJoyStick()
	{
		Vector2 lastTouchPos = player.inputController.LastTouchPos;
		Vector2 lastShootTouch = player.inputController.LastShootTouch;
		Vector2 vector = lastTouchPos - player.inputController.ThumbCenterToScreen;
		Vector2 moveJoyStickPos = vector;
		UserStateUI.GetInstance().SetMoveJoyStickPos(moveJoyStickPos);
		Vector2 vector2 = lastShootTouch - player.inputController.ShootThumbCenterToScreen;
		Vector2 shootJoyStickPos = vector2;
		UserStateUI.GetInstance().SetShootJoyStickPos(shootJoyStickPos);
	}

	protected virtual void UpdateAllHUD()
	{
		UpdateHP();
		UpdateJoyStick();
	}

	protected virtual void UpdateAllHUDWhenWaitingRebirth()
	{
	}

	protected virtual void UpdateAllHUDWhenFinish()
	{
	}

	public void ExitRoom()
	{
		if (GameApp.GetInstance().GetGameWorld().State != GameState.SwitchBossLevel)
		{
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				string msg = UIConstant.GetMessage(36).Replace("[n]", "\n");
				msgUI.CreateQuery(msg, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_LEAVE_INGAME_FOR_VS);
				msgUI.Show();
				player.inputController.Block = true;
			}
			else
			{
				LeaveRoom();
			}
		}
	}

	private void LeaveRoom()
	{
		GameUIManager.GetInstance().RemoveAll();
		LeaveRoomRequest request = new LeaveRoomRequest();
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		Lobby.GetInstance().IsMasterPlayer = false;
		GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
		GameApp.GetInstance().GetGameWorld().Exit = true;
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.SetState(Player.LOSE_STATE);
		stateMgr.FrGoToPhase(16, false, true, false);
	}

	public virtual void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control != msgUI)
		{
			return;
		}
		int eventID = msgUI.GetEventID();
		if (eventID == MessageBoxUI.EVENT_LEAVE_INGAME_FOR_VS)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			player.inputController.Block = false;
			switch (command)
			{
			case 9:
				msgUI.Hide();
				break;
			case 10:
				msgUI.Hide();
				LeaveRoom();
				break;
			}
		}
	}

	public virtual void HideUI()
	{
		HideUIWhenWaitingRebirth();
		useProps.Hide();
		menuBtn.Visible = false;
		menuBtn.Enable = false;
	}

	public virtual void HideUIWhenWaitingRebirth()
	{
		if (UIConstant.Is16By9())
		{
			joystickbgimg.Visible = false;
			shootjoystickbgimg.Visible = false;
		}
		else
		{
			battleImg.Visible = false;
		}
		joystickImg.Visible = false;
		shootjoystickInnerImg.Visible = false;
		shootjoystickOuterImg.Visible = false;
		shootjoystickImg.Visible = false;
		swapWeapon.Hide();
		aimImg.Visible = false;
		aimOnFireImg.Visible = false;
		hpEFXImg.Visible = false;
		hpImg.Visible = false;
		hpBGImg.Visible = false;
		foreach (SkillUI item in skillLst)
		{
			item.powerUpBtn.Visible = false;
			item.powerUpEnableImg.Visible = false;
		}
	}

	public virtual void ShowUI()
	{
		if (UIConstant.Is16By9())
		{
			joystickbgimg.Visible = true;
			shootjoystickbgimg.Visible = true;
		}
		else
		{
			battleImg.Visible = true;
		}
		joystickImg.Visible = true;
		shootjoystickInnerImg.Visible = true;
		shootjoystickOuterImg.Visible = true;
		shootjoystickImg.Visible = true;
		menuBtn.Visible = true;
		menuBtn.Enable = true;
		swapWeapon.Show();
		useProps.Show();
	}

	protected void InitHP()
	{
		Debug.Log("player.Hp : " + player.Hp);
		Debug.Log("player.MaxHp : " + player.MaxHp);
		UserStateUI.GetInstance().SetHpandMaxHp(player.Hp, player.MaxHp);
	}

	protected void InitWeapon()
	{
		List<Weapon> battleWeapons = userState.GetBattleWeapons();
		int weaponIndex = battleWeapons.IndexOf(player.GetWeapon());
		UserStateUI.GetInstance().SetWeaponList(battleWeapons);
		UserStateUI.GetInstance().SetWeaponIndex(weaponIndex);
	}

	protected void SetAim()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[0];
		aimImg.SetTexture(ui, 2, player.GetWeapon().AimID);
		aimImg.Rect = aimImg.GetObjectRect();
		aimImg.Rect = new Rect(UIConstant.ScreenLocalWidth * 0.5f - aimImg.Rect.width * 0.5f, UIConstant.ScreenLocalHeight * 0.5f - aimImg.Rect.height * 0.5f, aimImg.Rect.width, aimImg.Rect.height);
		aimWidth = aimImg.Rect.width;
		aimHeight = aimImg.Rect.height;
		aimImg.SetSize(new Vector2((int)aimWidth, (int)aimHeight));
		aimImg.SetColor(UIConstant.COLOR_AIM);
	}

	protected void SetAimOnFire()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[0];
		aimOnFireImg.SetTexture(ui, 2, player.GetWeapon().AimID);
		aimOnFireImg.Rect = aimOnFireImg.GetObjectRect();
		aimOnFireImg.Rect = new Rect(UIConstant.ScreenLocalWidth * 0.5f - aimOnFireImg.Rect.width * 0.5f, UIConstant.ScreenLocalHeight * 0.5f - aimOnFireImg.Rect.height * 0.5f, aimOnFireImg.Rect.width, aimOnFireImg.Rect.height);
		aimWidth = aimOnFireImg.Rect.width;
		aimHeight = aimOnFireImg.Rect.height;
		aimOnFireImg.SetSize(new Vector2((int)(aimWidth * 1.2f), (int)(aimHeight * 1.2f)));
		aimOnFireImg.SetColor(UIConstant.COLOR_AIM);
	}

	public void ResetUIProps()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, frameIdx, 16);
		useProps.Clear();
		int itemsCount = player.GetItemsCount();
		float num = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)itemsCount * modulePositionRect.width * 0.5f;
		int[] bagPosition = userState.GetBagPosition();
		for (int i = 0; i < bagPosition.Length; i++)
		{
			if (bagPosition[i] > 80)
			{
				UIProps.UIPropsIcon uIPropsIcon = new UIProps.UIPropsIcon();
				uIPropsIcon.m_background = new UIImage();
				uIPropsIcon.m_background.AddObject(unitUI, frameIdx, 16);
				uIPropsIcon.m_background.Rect = new Rect(num, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
				uIPropsIcon.m_propsIcon = new UIImage();
				uIPropsIcon.m_propsIcon.AddObject(ui, 1, bagPosition[i] - 81 - 1);
				uIPropsIcon.m_propsIcon.Rect = uIPropsIcon.m_background.Rect;
				uIPropsIcon.m_propsIcon.SetSize(new Vector2(uIPropsIcon.m_background.Rect.width, uIPropsIcon.m_background.Rect.height));
				uIPropsIcon.Rect = uIPropsIcon.m_background.Rect;
				uIPropsIcon.Visible = true;
				uIPropsIcon.Enable = true;
				uIPropsIcon.Id = i;
				useProps.Add(uIPropsIcon);
				num += modulePositionRect.width;
			}
		}
	}

	public void ResetUIWeapon()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		swapWeapon.Clear();
		List<Weapon> weaponList = player.GetWeaponList();
		int num = 1;
		int num2 = 1;
		switch (weaponList.Count)
		{
		case 1:
			num = 1;
			num2 = 1;
			break;
		case 2:
			num = 4;
			num2 = 3;
			break;
		case 3:
			num = 6;
			num2 = 3;
			break;
		case 4:
			num = 4;
			num2 = 3;
			break;
		case 5:
			num = 10;
			num2 = 5;
			break;
		default:
			num2 = 5;
			num = weaponList.Count;
			break;
		}
		for (int i = 0; i < num; i++)
		{
			UISliderItem.UIGunIcon uIGunIcon = new UISliderItem.UIGunIcon();
			uIGunIcon.m_background = new UIImage();
			uIGunIcon.m_background.AddObject(unitUI, frameIdx, 8);
			uIGunIcon.m_background.Rect = uIGunIcon.m_background.GetObjectRect();
			uIGunIcon.m_background.Visible = false;
			uIGunIcon.m_gunIcon = new UIImage();
			int index = i % weaponList.Count;
			Weapon weapon = (uIGunIcon.m_weapon = weaponList[index]);
			uIGunIcon.m_gunIcon.AddObject(ui, 0, weapon.GetGunID());
			uIGunIcon.m_gunIcon.Rect = uIGunIcon.m_background.Rect;
			uIGunIcon.m_gunIcon.Visible = false;
			uIGunIcon.m_gunDisableIcon = new UIImage();
			if (weapon.GunID == 21)
			{
				uIGunIcon.m_hasCDTimer = true;
				uIGunIcon.m_gunDisableIcon.AddObject(ui, 0, 39);
				uIGunIcon.m_gunDisableIcon.Rect = uIGunIcon.m_background.Rect;
				uIGunIcon.m_gunDisableIcon.Visible = false;
			}
			uIGunIcon.Id = userState.GetWeaponBagIndex(weapon);
			swapWeapon.Add(uIGunIcon);
		}
		Rect rect = unitUI.GetModulePositionRect(0, frameIdx, 8);
		if (UIConstant.Is16By9())
		{
			rect = new Rect(Screen.width - 120 - 3, rect.y, rect.width, rect.height);
		}
		int num3 = (int)Mathf.Clamp(num2 * 57, rect.height, 285f);
		swapWeapon.SetClipRect(new Rect(rect.x, rect.y + rect.height * 0.5f - (float)num3 * 0.5f, rect.width, num3));
		if (num > 1)
		{
			swapWeapon.SetScroller(0f, 62 * num, 62f, swapWeapon.m_showRect);
		}
	}

	public bool InGameFinishState()
	{
		return player.State == Player.WIN_STATE || player.State == Player.LOSE_STATE;
	}

	public bool InWaitRebirthState()
	{
		return player.State == Player.DEAD_STATE || player.State == Player.WAIT_REBIRTH_STATE || player.State == Player.WAIT_VS_REBIRTH_STATE;
	}

	protected virtual void UpdateAimIcon()
	{
		if (player.GetWeapon().GetWeaponType() != WeaponType.Sniper && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedSniper && player.GetWeapon().GetWeaponType() != WeaponType.RelectionSniper)
		{
			if (player.inputController.inputInfo.fire)
			{
				if (player.GetWeapon().AttackFrequency <= 0.4f)
				{
					aimOnFireImg.Visible = true;
					aimImg.Visible = false;
				}
				else if (player.JustMakeAShoot)
				{
					player.JustMakeAShoot = false;
					aimTimer.Do();
					aimOnFireImg.Visible = true;
					aimImg.Visible = false;
				}
				else if (!aimTimer.Ready())
				{
					aimOnFireImg.Visible = true;
					aimImg.Visible = false;
				}
				else
				{
					aimOnFireImg.Visible = false;
					aimImg.Visible = true;
				}
			}
			else
			{
				aimOnFireImg.Visible = false;
				aimImg.Visible = true;
			}
		}
		else if (player.inputController.inputInfo.fire)
		{
			aimOnFireImg.Visible = false;
			aimImg.Visible = true;
		}
		else if (player.JustMakeAShoot)
		{
			player.JustMakeAShoot = false;
			aimTimer.Do();
			aimOnFireImg.Visible = true;
			aimImg.Visible = false;
		}
		else if (!aimTimer.Ready())
		{
			aimOnFireImg.Visible = true;
			aimImg.Visible = false;
		}
		else
		{
			aimOnFireImg.Visible = false;
			aimImg.Visible = true;
		}
	}

	protected void UpdatePowerUp()
	{
		if (skillLst.Count == 0)
		{
			return;
		}
		foreach (SkillUI item in skillLst)
		{
			item.powerUpBtn.Visible = true;
			if (player.IsPowerUpCoolDown(item.skillId))
			{
				item.powerUpBtn.Enable = true;
				if (item.bPowerUpLight)
				{
					item.powerUpZoom += 0.15f;
					item.powerUpZoom = Mathf.Min(item.powerUpZoom, 2f);
					if (item.powerUpZoom >= 2f)
					{
						item.powerUpZoom = 1f;
						item.powerUpEnableImg.SetSize(new Vector2((int)item.powerUpSize.x, (int)item.powerUpSize.y));
						item.powerUpEnableImg.SetColor(new Color(Color.white.r, Color.white.g, Color.white.b, Color.white.a));
						item.powerUpEnableImg.Visible = false;
						item.bPowerUpLight = false;
					}
					else
					{
						item.powerUpEnableImg.Visible = true;
						item.powerUpEnableImg.ClearClip();
						item.powerUpEnableImg.SetSize(new Vector2((int)(item.powerUpSize.x * item.powerUpZoom), (int)(item.powerUpSize.y * item.powerUpZoom)));
						item.powerUpEnableImg.SetColor(new Color(Color.white.r, Color.white.g, Color.white.b, Color.white.a + 1f - item.powerUpZoom));
					}
				}
				continue;
			}
			item.powerUpBtn.Enable = false;
			Timer powerTimer = player.GetPowerTimer(item.skillId);
			if (!powerTimer.Ready())
			{
				int num = (int)(item.powerUpEnableImg.Rect.height * powerTimer.GetTimeSpan() / powerTimer.GetInterval());
				item.powerUpEnableImg.Visible = true;
				item.powerUpEnableImg.SetClip(new Rect(item.powerUpEnableImg.Rect.x, item.powerUpEnableImg.Rect.y, item.powerUpEnableImg.Rect.width, item.powerUpEnableImg.Rect.height - (float)num));
				continue;
			}
			Timer powerCDTimer = player.GetPowerCDTimer(item.skillId);
			if (!powerCDTimer.Ready())
			{
				item.powerUpEnableImg.Visible = true;
				int num2 = (int)(item.powerUpEnableImg.Rect.height * (powerCDTimer.GetTimeSpan() - powerTimer.GetTimeSpan()) / (powerCDTimer.GetInterval() - powerTimer.GetInterval()));
				item.powerUpEnableImg.SetClip(new Rect(item.powerUpEnableImg.Rect.x, item.powerUpEnableImg.Rect.y, item.powerUpEnableImg.Rect.width, num2));
			}
			item.bPowerUpLight = true;
			item.powerUpZoom = 1f;
		}
	}

	protected virtual void UpdateHP()
	{
		if (player.Hp < 0)
		{
			player.Hp = 0;
		}
		UserStateUI.GetInstance().UpdateHp(player.Hp);
	}

	public Vector2 GetPT(Vector2 s, Vector2 e, float r)
	{
		float num6;
		float num7;
		float num8;
		float num9;
		if (s.x != e.x)
		{
			if (s.y != e.y)
			{
				float num = (s.y - e.y) / (s.x - e.x);
				float num2 = s.y - num * s.x;
				float num3 = num * num + 1f;
				float num4 = 2f * num2 * num - 2f * s.y * num - 2f * s.x;
				float num5 = s.x * s.x + (num2 - s.y) * (num2 - s.y) - r * r;
				num6 = (-1f * num4 - Mathf.Sqrt(num4 * num4 - 4f * num3 * num5)) / num3 / 2f;
				num7 = (-1f * num4 + Mathf.Sqrt(num4 * num4 - 4f * num3 * num5)) / num3 / 2f;
				num8 = num * num6 + num2;
				num9 = num * num7 + num2;
			}
			else
			{
				num8 = s.y;
				num9 = s.y;
				num6 = s.x + r;
				num7 = s.x - r;
			}
		}
		else
		{
			num6 = s.x;
			num7 = s.x;
			num8 = s.y + r;
			num9 = s.y - r;
		}
		float x;
		float y;
		if ((num6 - e.x) * (num6 - e.x) + (num8 - e.y) * (num8 - e.y) > (num7 - e.x) * (num7 - e.x) + (num9 - e.y) * (num9 - e.y))
		{
			x = num7;
			y = num9;
		}
		else
		{
			x = num6;
			y = num8;
		}
		Vector2 result = default(Vector2);
		result.x = x;
		result.y = y;
		return result;
	}

	public virtual void AddWhoKillsWho(int killerID, HUDAction action, int killedID)
	{
	}
}
