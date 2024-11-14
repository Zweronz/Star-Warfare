using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Confirm = 0,
		Cancel = 1
	}

	private const byte PASSWORD_ON = 59;

	private const byte PASSWORD_OFF = 58;

	private const byte PASSWORD_OFF_BEGIN = 60;

	private const byte PASSWORD_OFF_COUNT = 4;

	public UIStateManager stateMgr;

	private static byte BACKGROUND_BEGIN_IMG = 3;

	private static byte BACKGROUND_COUNT_IMG = 28;

	private UIBlock m_block;

	private UIImage shadowImg;

	private UIImage backgroundImg;

	private UIImage fillBGImg;

	private UIClickButton hostNameBtn;

	private UIClickButton confirmBtn;

	private UIClickButton cancelBtn;

	private static byte[] CONFIRM_NORMAL = new byte[2] { 39, 41 };

	private static byte[] CONFIRM_PRESSED = new byte[2] { 38, 40 };

	private static byte[] CANCEL_NORMAL = new byte[2] { 37, 43 };

	private static byte[] CANCEL_PRESSED = new byte[2] { 36, 42 };

	private static byte[] COMBOBOX_GAMEMODE_VS = new byte[7] { 5, 3, 0, 1, 2, 20, 1 };

	private static byte[] COMBOBOX_COOP_PLAYER_NUM = new byte[7] { 5, 2, 3, 1, 2, 20, 1 };

	private static byte[] COMBOBOX_WIN_CONDITION = new byte[7] { 5, 3, 3, 4, 5, 20, 4 };

	private static byte[] COMBOBOX_WIN_VALUE = new byte[7] { 5, 3, 6, 4, 5, 20, 4 };

	private static byte[] COMBOBOX_GAMEMODE_COOP = new byte[7] { 5, 2, 0, 1, 2, 20, 1 };

	private UIComboBox gameModeCoopCmb;

	private UIComboBox gameModeVSCmb;

	private UIComboBox playerNumCmb;

	private UIComboBox winConditionCmb;

	private UIComboBox winValueCmb;

	private UIImage winPanelBGImg;

	private UIImage winPanelFillImg;

	private static byte WIN_BACKGROUND_BEGIN_IMG = 80;

	private static byte WIN_BACKGROUND_COUNT_IMG = 14;

	private UITextImage balanceImg;

	private UISliderSwitch balanceSwitch;

	private Rect nameRect;

	private UISliderNetStage swapStage;

	private List<UIImage> pageNavImg = new List<UIImage>();

	private UIImage selectStagePageImg;

	private int stageCount;

	public byte stageIdx;

	private UIImage passwordImg;

	private UIImage passwordBGImg;

	private static byte[] PASSWORD_BACKGROUND = new byte[3] { 55, 56, 57 };

	private UISliderNum[] passwordNum;

	private UIImage[] passwordMaskImg;

	private UIImage rankBG;

	private static byte[] RANK_BACKGROUND = new byte[6] { 64, 65, 66, 67, 68, 69 };

	private UIImage rankLowerMaskImg;

	private UIImage rankUpperMaskImg;

	private UISliderNum rankLower;

	private UISliderNum rankUpper;

	private bool bPasswordOn;

	private UISliderTab vsModeTab;

	private Mode gameMode;

	private static byte[,] TAB_GAME_MODE = new byte[2, 2]
	{
		{ 73, 74 },
		{ 75, 76 }
	};

	public CreateRoomUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[14];
		gameMode = GameApp.GetInstance().GetGameMode().ModePlay;
		ReviseNetStage();
		Debug.Log(gameMode);
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		shadowImg.SetColor(new Color(1f, 1f, 1f, 0.9f));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 1);
		fillBGImg = new UIImage();
		fillBGImg.AddObject(unitUI, 0, 2);
		fillBGImg.Rect = modulePositionRect;
		fillBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		backgroundImg = new UIImage();
		backgroundImg.AddObject(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		backgroundImg.Rect = backgroundImg.GetObjectRect();
		hostNameBtn = new UIClickButton();
		hostNameBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 31);
		hostNameBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 31);
		hostNameBtn.Rect = hostNameBtn.GetObjectRect(UIButtonBase.State.Normal);
		hostNameBtn.Visible = false;
		cancelBtn = new UIClickButton();
		cancelBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, CANCEL_NORMAL);
		cancelBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, CANCEL_PRESSED);
		cancelBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 0, CANCEL_NORMAL);
		cancelBtn.Rect = cancelBtn.GetObjectRect(UIButtonBase.State.Normal);
		confirmBtn = new UIClickButton();
		confirmBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, CONFIRM_NORMAL);
		confirmBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, CONFIRM_PRESSED);
		confirmBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 0, CONFIRM_NORMAL);
		confirmBtn.Rect = confirmBtn.GetObjectRect(UIButtonBase.State.Normal);
		vsModeTab = new UISliderTab();
		vsModeTab.Create();
		InitVSMode();
		gameModeVSCmb = new UIComboBox();
		gameModeVSCmb.Create(unitUI, COMBOBOX_GAMEMODE_VS);
		ResetUIGameSubModeForVS(COMBOBOX_GAMEMODE_VS, UIConstant.GAME_MODE_VS);
		gameModeCoopCmb = new UIComboBox();
		gameModeCoopCmb.Create(unitUI, COMBOBOX_GAMEMODE_COOP);
		ResetUIGameSubModeForCoop(COMBOBOX_GAMEMODE_COOP, UIConstant.GAME_MODE_COOP);
		playerNumCmb = new UIComboBox();
		playerNumCmb.Create(unitUI, COMBOBOX_COOP_PLAYER_NUM);
		ResetUIPlayerNum(COMBOBOX_COOP_PLAYER_NUM);
		winConditionCmb = new UIComboBox();
		winConditionCmb.Create(unitUI, COMBOBOX_WIN_CONDITION);
		ResetUIWinConditionCmb(COMBOBOX_WIN_CONDITION, UIConstant.WIN_CONDITION);
		winValueCmb = new UIComboBox();
		winValueCmb.Create(unitUI, COMBOBOX_WIN_VALUE);
		balanceImg = new UITextImage();
		balanceImg.AddObject(unitUI, 3, 7);
		balanceImg.Rect = balanceImg.GetObjectRect();
		balanceImg.SetTextOffset(18f, -10f);
		balanceImg.SetText("font2", "AUTO BALANCE", UIConstant.fontColor_white, FrUIText.enAlignStyle.TOP_LEFT, balanceImg.Rect.width);
		balanceSwitch = new UISliderSwitch();
		balanceSwitch.Create(unitUI);
		InitbalanceSwitch();
		InitModePanel();
		float num = 0f;
		passwordBGImg = new UIImage();
		passwordBGImg.AddObject(unitUI, 0, PASSWORD_BACKGROUND);
		passwordBGImg.Rect = passwordBGImg.GetObjectRect();
		bPasswordOn = false;
		passwordImg = new UIImage();
		passwordImg.AddObject(unitUI, 0, 58);
		passwordImg.Rect = new Rect(passwordImg.GetObjectRect().x - 15f, passwordImg.GetObjectRect().y - 15f, passwordImg.GetObjectRect().width + 30f, passwordImg.GetObjectRect().height + 30f);
		passwordImg.Enable = true;
		passwordMaskImg = new UIImage[4];
		for (int i = 0; i < 4; i++)
		{
			passwordMaskImg[i] = new UIImage();
			passwordMaskImg[i].AddObject(unitUI, 0, 60 + i);
			passwordMaskImg[i].Rect = passwordMaskImg[i].GetObjectRect();
		}
		passwordNum = new UISliderNum[4];
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 1, 0);
		for (int j = 0; j < 4; j++)
		{
			passwordNum[j] = new UISliderNum();
			passwordNum[j].Create(unitUI, 1, 0);
			for (int k = 0; k < 10; k++)
			{
				UISliderNum.UINumIcon uINumIcon = new UISliderNum.UINumIcon();
				uINumIcon.m_background = new UIImage();
				if (k == 0)
				{
					uINumIcon.m_background.AddObject(unitUI, 1, 10);
				}
				else
				{
					uINumIcon.m_background.AddObject(unitUI, 1, k);
				}
				Rect objectRect = uINumIcon.m_background.GetObjectRect();
				uINumIcon.m_background.Rect = new Rect(passwordMaskImg[j].Rect.x + (passwordMaskImg[j].Rect.width - objectRect.width) * 0.5f, passwordMaskImg[j].Rect.y + (passwordMaskImg[j].Rect.height - objectRect.height) * 0.5f, objectRect.width, objectRect.height);
				uINumIcon.Visible = false;
				uINumIcon.Enable = false;
				uINumIcon.Rect = uINumIcon.m_background.Rect;
				passwordNum[j].Add(uINumIcon);
			}
			passwordNum[j].SetClipRect(passwordMaskImg[j].Rect.x, passwordMaskImg[j].Rect.y, passwordMaskImg[j].Rect.width, passwordMaskImg[j].Rect.height);
			num = modulePositionRect2.height + 5f;
			Rect rct = new Rect(passwordNum[j].m_showRect.x - 10f, passwordNum[j].m_showRect.y - 10f, passwordNum[j].m_showRect.width + 20f, passwordNum[j].m_showRect.height + 20f);
			passwordNum[j].SetScroller(0f, 10f * num, num, rct, true);
			passwordNum[j].SetSelection(0);
		}
		rankBG = new UIImage();
		rankBG.AddObject(unitUI, 0, RANK_BACKGROUND);
		rankBG.Rect = rankBG.GetObjectRect();
		rankLowerMaskImg = new UIImage();
		rankLowerMaskImg.AddObject(unitUI, 0, 70);
		rankLowerMaskImg.Rect = rankLowerMaskImg.GetObjectRect();
		rankLower = new UISliderNum();
		rankLower.Create(unitUI2, 0, 21);
		Rect modulePositionRect3 = unitUI2.GetModulePositionRect(0, 0, 21);
		List<Rank> rankList = GameApp.GetInstance().GetUserState().GetRankList();
		for (int l = 0; l < rankList.Count; l++)
		{
			UISliderNum.UINumIcon uINumIcon2 = new UISliderNum.UINumIcon();
			uINumIcon2.m_background = new UIImage();
			uINumIcon2.m_background.AddObject(unitUI2, 0, l + 21);
			Rect objectRect2 = uINumIcon2.m_background.GetObjectRect();
			uINumIcon2.m_background.Rect = new Rect(rankLowerMaskImg.Rect.x + (rankLowerMaskImg.Rect.width - objectRect2.width) * 0.5f, rankLowerMaskImg.Rect.y + (rankLowerMaskImg.Rect.height - objectRect2.height) * 0.5f, objectRect2.width, objectRect2.height);
			uINumIcon2.Visible = false;
			uINumIcon2.Enable = false;
			uINumIcon2.Rect = uINumIcon2.m_background.Rect;
			rankLower.Add(uINumIcon2);
		}
		rankLower.SetClipRect(rankLowerMaskImg.Rect.x, rankLowerMaskImg.Rect.y, rankLowerMaskImg.Rect.width, rankLowerMaskImg.Rect.height);
		num = modulePositionRect3.height + 5f;
		rankLower.SetScroller(0f, (float)(rankList.Count - 1) * num, num, rankLower.m_showRect, false);
		rankLower.SetSelection(0);
		rankLower.SetRangePos(0, (int)((float)(int)GameApp.GetInstance().GetUserState().GetRank()
			.rankID * num));
		rankUpperMaskImg = new UIImage();
		rankUpperMaskImg.AddObject(unitUI, 0, 71);
		rankUpperMaskImg.Rect = rankUpperMaskImg.GetObjectRect();
		rankUpper = new UISliderNum();
		rankUpper.Create(unitUI2, 0, 21);
		for (int m = 0; m < rankList.Count; m++)
		{
			UISliderNum.UINumIcon uINumIcon3 = new UISliderNum.UINumIcon();
			uINumIcon3.m_background = new UIImage();
			uINumIcon3.m_background.AddObject(unitUI2, 0, m + 21);
			Rect objectRect3 = uINumIcon3.m_background.GetObjectRect();
			uINumIcon3.m_background.Rect = new Rect(rankUpperMaskImg.Rect.x + (rankUpperMaskImg.Rect.width - objectRect3.width) * 0.5f, rankUpperMaskImg.Rect.y + (rankUpperMaskImg.Rect.height - objectRect3.height) * 0.5f, objectRect3.width, objectRect3.height);
			uINumIcon3.Visible = false;
			uINumIcon3.Enable = false;
			uINumIcon3.Rect = uINumIcon3.m_background.Rect;
			rankUpper.Add(uINumIcon3);
		}
		rankUpper.SetClipRect(rankUpperMaskImg.Rect.x, rankUpperMaskImg.Rect.y, rankUpperMaskImg.Rect.width, rankUpperMaskImg.Rect.height);
		num = modulePositionRect3.height + 5f;
		rankUpper.SetScroller(0f, (float)(rankList.Count - 1) * num, num, rankUpper.m_showRect, false);
		rankUpper.SetSelection(rankList.Count - 1);
		rankUpper.SetRangePos((int)((float)(int)GameApp.GetInstance().GetUserState().GetRank()
			.rankID * num), 10000);
		nameRect = UIConstant.GetRectForScreenAdaptived(new Rect(hostNameBtn.Rect.x, UIConstant.ScreenLocalHeight - hostNameBtn.Rect.y - hostNameBtn.Rect.height, hostNameBtn.Rect.width, hostNameBtn.Rect.height));
		Add(shadowImg);
		Add(backgroundImg);
		Add(fillBGImg);
		Add(hostNameBtn);
		stageIdx = GameApp.GetInstance().GetUserState().GetNetStage();
		if (stageIdx < Global.TOTAL_SURVIVAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, 0] == 0)
		{
			stageIdx = 0;
			GameApp.GetInstance().GetUserState().SetNetStage(stageIdx);
		}
		swapStage = new UISliderNetStage();
		swapStage.Create(unitUI);
		ResetUIStage();
		Add(swapStage);
		int num2 = 10;
		for (int n = 0; n < num2; n++)
		{
			UIImage uIImage = new UIImage();
			uIImage.AddObject(unitUI, 0, 94);
			uIImage.Rect = uIImage.GetObjectRect();
			pageNavImg.Add(uIImage);
			Add(uIImage);
		}
		selectStagePageImg = new UIImage();
		selectStagePageImg.AddObject(unitUI, 0, 95);
		Add(selectStagePageImg);
		ResetPageNav();
		Add(passwordBGImg);
		Add(passwordImg);
		UISliderNum[] array = passwordNum;
		foreach (UISliderNum control in array)
		{
			Add(control);
		}
		UIImage[] array2 = passwordMaskImg;
		foreach (UIImage control2 in array2)
		{
			Add(control2);
		}
		Add(rankBG);
		Add(rankLower);
		Add(rankLowerMaskImg);
		Add(rankUpper);
		Add(rankUpperMaskImg);
		Add(vsModeTab);
		Add(balanceImg);
		Add(balanceSwitch);
		Add(winConditionCmb);
		Add(winValueCmb);
		Add(playerNumCmb);
		Add(gameModeVSCmb);
		Add(gameModeCoopCmb);
		Add(cancelBtn);
		Add(confirmBtn);
		SetUIHandler(this);
	}

	public new void Clear()
	{
		gameModeVSCmb.Clear();
		gameModeCoopCmb.Clear();
		playerNumCmb.Clear();
		winConditionCmb.Clear();
		winValueCmb.Clear();
		rankLower.Clear();
		rankUpper.Clear();
		UISliderNum[] array = passwordNum;
		foreach (UISliderNum uISliderNum in array)
		{
			uISliderNum.Clear();
		}
		base.Clear();
	}

	public void ResetUIStage()
	{
		swapStage.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		stageCount = 0;
		int num = 0;
		if (gameMode == Mode.Boss)
		{
			stageCount = Global.TOTAL_BOSS_STAGE;
			num = Global.TOTAL_BOSS_STAGE;
		}
		else if (gameMode == Mode.Survival)
		{
			stageCount = Global.TOTAL_SURVIVAL_STAGE;
			num = Global.TOTAL_SURVIVAL_STAGE;
		}
		else
		{
			stageCount = Global.TOTAL_VS_STAGE;
			num = Global.TOTAL_VS_STAGE;
		}
		for (int i = 0; i < num; i++)
		{
			UISliderNetStage.UIStageIcon uIStageIcon = new UISliderNetStage.UIStageIcon();
			uIStageIcon.m_background = new UIImage();
			int num2 = i % stageCount;
			if (gameMode == Mode.Survival)
			{
				uIStageIcon.m_background.AddObject(unitUI, 4, num2);
			}
			else if (gameMode == Mode.Boss)
			{
				uIStageIcon.m_background.AddObject(unitUI, 5, num2);
			}
			else
			{
				uIStageIcon.m_background.AddObject(unitUI, 6, num2);
			}
			uIStageIcon.m_background.Rect = uIStageIcon.m_background.GetObjectRect();
			uIStageIcon.m_Lock = new UIImage();
			uIStageIcon.m_Lock.AddObject(unitUI, 0, 96);
			uIStageIcon.m_Lock.Rect = uIStageIcon.m_Lock.GetObjectRect();
			uIStageIcon.m_bLock = false;
			if (gameMode == Mode.Survival && num2 < Global.TOTAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[num2, 0] == 0)
			{
				uIStageIcon.m_bLock = true;
			}
			else if (gameMode == Mode.VS_CMI && !IsUnlockMapForVS(num2, UIConstant.VS_CMI_SPECIAL_SCENE))
			{
				uIStageIcon.m_bLock = true;
			}
			uIStageIcon.Id = num2;
			uIStageIcon.Rect = uIStageIcon.m_background.Rect;
			uIStageIcon.Show();
			swapStage.Add(uIStageIcon);
		}
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 44);
		Rect rect = new Rect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		swapStage.SetClipRect(rect);
		float num3 = 180f;
		swapStage.SetScroller(0f, num3 * (float)num, num3, rect);
		if (gameMode == Mode.Survival)
		{
			swapStage.SetSelection(stageIdx);
		}
		else if (gameMode == Mode.Boss)
		{
			swapStage.SetSelection(stageIdx - Global.TOTAL_SURVIVAL_STAGE);
		}
		else if (gameMode == Mode.VS_CMI)
		{
			stageIdx = (byte)(Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE + Global.TOTAL_VS_STAGE - 1);
			swapStage.SetSelection(stageIdx - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
		}
		else
		{
			swapStage.SetSelection(stageIdx - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
		}
	}

	public void ResetMapForVS()
	{
		for (int i = 0; i < swapStage.m_StageIcons.Count; i++)
		{
			int mapId = i % Global.TOTAL_VS_STAGE;
			UISliderNetStage.UIStageIcon uIStageIcon = swapStage.m_StageIcons[i];
			if (gameMode == Mode.VS_CMI)
			{
				if (!IsUnlockMapForVS(mapId, UIConstant.VS_CMI_SPECIAL_SCENE))
				{
					uIStageIcon.m_bLock = true;
				}
			}
			else
			{
				uIStageIcon.m_bLock = false;
			}
		}
	}

	public bool IsUnlockMapForVS(int mapId, byte[] maps)
	{
		for (int i = 0; i < maps.Length; i++)
		{
			if (mapId == maps[i])
			{
				return true;
			}
		}
		return false;
	}

	public void ResetPageNav()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		int num = 0;
		num = ((gameMode == Mode.Boss) ? Global.TOTAL_BOSS_STAGE : ((gameMode != Mode.Survival) ? Global.TOTAL_VS_STAGE : Global.TOTAL_SURVIVAL_STAGE));
		int num2 = 32;
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 95);
		float num3 = modulePositionRect.y + modulePositionRect.height * 0.5f - (float)((num - 1) * num2) * 0.5f;
		for (int i = 0; i < pageNavImg.Count; i++)
		{
			pageNavImg[i].Rect = new Rect(modulePositionRect.x, num3 - modulePositionRect.height * 0.5f, modulePositionRect.width, modulePositionRect.height);
			num3 += (float)num2;
			if (i < num)
			{
				pageNavImg[i].Visible = true;
			}
			else
			{
				pageNavImg[i].Visible = false;
			}
		}
		selectStagePageImg.Rect = pageNavImg[ConvertToUIIndex(stageIdx)].Rect;
	}

	private byte ConvertToUIIndex(byte curId)
	{
		byte b = curId;
		if (gameMode == Mode.Survival)
		{
			return curId;
		}
		if (gameMode == Mode.Boss)
		{
			return (byte)(curId - Global.TOTAL_SURVIVAL_STAGE);
		}
		return (byte)(curId - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
	}

	private byte ConvertToStageIndex(byte curId)
	{
		byte b = curId;
		if (gameMode == Mode.Survival)
		{
			return curId;
		}
		if (gameMode == Mode.Boss)
		{
			return (byte)(curId + Global.TOTAL_SURVIVAL_STAGE);
		}
		return (byte)(curId + Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE);
	}

	public void InitModePanel()
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode(gameMode))
		{
			gameModeCoopCmb.Visible = true;
			gameModeCoopCmb.Enable = true;
			playerNumCmb.Visible = true;
			playerNumCmb.Enable = true;
			gameModeVSCmb.Visible = false;
			gameModeVSCmb.Enable = false;
			gameModeVSCmb.SetState(3);
			winConditionCmb.Visible = false;
			winValueCmb.Visible = false;
			balanceSwitch.Visible = false;
			balanceImg.Visible = false;
			return;
		}
		gameModeVSCmb.Visible = true;
		gameModeVSCmb.Enable = true;
		gameModeCoopCmb.Visible = false;
		gameModeCoopCmb.Enable = false;
		gameModeCoopCmb.SetState(3);
		playerNumCmb.Visible = false;
		playerNumCmb.Enable = false;
		playerNumCmb.SetState(3);
		winConditionCmb.Visible = true;
		winValueCmb.Visible = true;
		if (gameMode == Mode.VS_CTF_FFA || gameMode == Mode.VS_CTF_TDM || gameMode == Mode.VS_VIP || gameMode == Mode.VS_CMI)
		{
			winConditionCmb.SetSelection(0);
			winConditionCmb.Enable = false;
		}
		else
		{
			winConditionCmb.Enable = true;
		}
		if (winConditionCmb.GetSelectIndex() == 0)
		{
			ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.WIN_VALUE_FOR_SCORE);
		}
		else
		{
			ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.WIN_VALUE_FOR_TIMER);
		}
		if (gameMode == Mode.VS_TDM || gameMode == Mode.VS_CTF_TDM || gameMode == Mode.VS_VIP || gameMode == Mode.VS_CMI)
		{
			balanceSwitch.Visible = true;
			balanceImg.Visible = true;
		}
		else
		{
			balanceSwitch.Visible = false;
			balanceImg.Visible = false;
		}
	}

	public void InitVSMode()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[5];
		vsModeTab.Clear();
		vsModeTab.SetBackground(ui, 0, 72);
		vsModeTab.SetSlider(ui, 0, 77);
		vsModeTab.SetClipRect(vsModeTab.backgroundImg.Rect.x, vsModeTab.backgroundImg.Rect.y, vsModeTab.backgroundImg.Rect.width, vsModeTab.backgroundImg.Rect.height);
		for (int i = 0; i < UIConstant.GAME_MODE_TOTAL; i++)
		{
			UITab uITab = new UITab();
			uITab.AddObject(UITab.State.Normal, ui, 0, TAB_GAME_MODE[i, 0]);
			uITab.AddObject(UITab.State.Selected, ui, 0, TAB_GAME_MODE[i, 1]);
			uITab.Rect = uITab.GetObjectRect(UITab.State.Normal);
			uITab.Enable = false;
			vsModeTab.Add(uITab);
		}
		vsModeTab.SetScroller(0f, 165f, 165f, vsModeTab.backgroundImg.Rect);
		vsModeTab.m_scroller.DeltaTime = 0.016f;
		if (GameApp.GetInstance().GetGameMode().IsVSMode(gameMode))
		{
			vsModeTab.SetSelection(0);
		}
		else
		{
			vsModeTab.SetSelection(1);
		}
	}

	public void ReviseNetStage()
	{
		int netStage = GameApp.GetInstance().GetUserState().GetNetStage();
		if (gameMode == Mode.Survival)
		{
			if (netStage >= Global.TOTAL_SURVIVAL_STAGE)
			{
				GameApp.GetInstance().GetUserState().SetNetStage(0);
			}
		}
		else if (gameMode == Mode.Boss)
		{
			if (netStage >= Global.TOTAL_BOSS_STAGE || netStage < Global.TOTAL_SURVIVAL_STAGE)
			{
				GameApp.GetInstance().GetUserState().SetNetStage((byte)Global.TOTAL_SURVIVAL_STAGE);
			}
		}
		else if (netStage < Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE)
		{
			GameApp.GetInstance().GetUserState().SetNetStage((byte)(Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE));
		}
	}

	public void InitbalanceSwitch()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[5];
		balanceSwitch.SetBackground(ui, 3, 9);
		balanceSwitch.SetSlider(ui, 3, 8);
		balanceSwitch.SetClipRect(balanceSwitch.backgroundImg.Rect.x + 1f, balanceSwitch.backgroundImg.Rect.y, balanceSwitch.backgroundImg.Rect.width - 2f, balanceSwitch.backgroundImg.Rect.height);
		balanceSwitch.SetScroller(0f, 67f, 67f, balanceSwitch.backgroundImg.Rect);
		balanceSwitch.SetSelection(0);
	}

	public void ResetUIGameSubModeForCoop(byte[] resIndex, string[] str)
	{
		gameModeCoopCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		if (modulePositionRect.y - (float)str.Length * modulePositionRect.height < 10f)
		{
			gameModeCoopCmb.SetComboBox(UIComboBox.Dir.UP);
			gameModeCoopCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y + modulePositionRect.height, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		else
		{
			gameModeCoopCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)str.Length, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		for (int i = 0; i < str.Length; i++)
		{
			UIComboBox.UIItemIcon uIItemIcon = new UIComboBox.UIItemIcon(gameModeCoopCmb);
			uIItemIcon.m_background.AddObject(unitUI, resIndex[1], resIndex[3]);
			uIItemIcon.m_background.Rect = uIItemIcon.m_background.GetObjectRect();
			uIItemIcon.m_text.Set("font2", str[i], UIConstant.fontColor_cyan, uIItemIcon.m_background.Rect.width);
			uIItemIcon.m_text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
			uIItemIcon.m_text.Rect = uIItemIcon.m_background.Rect;
			uIItemIcon.SetClip(gameModeVSCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-20f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			gameModeCoopCmb.Add(uIItemIcon);
		}
		gameModeCoopCmb.SetSelection(0);
		gameModeCoopCmb.Enable = true;
	}

	public void ResetUIGameSubModeForVS(byte[] resIndex, string[] str)
	{
		gameModeVSCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		if (modulePositionRect.y - (float)str.Length * modulePositionRect.height < 10f)
		{
			gameModeVSCmb.SetComboBox(UIComboBox.Dir.UP);
			gameModeVSCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y + modulePositionRect.height, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		else
		{
			gameModeVSCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)str.Length, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		for (int i = 0; i < str.Length; i++)
		{
			UIComboBox.UIItemIcon uIItemIcon = new UIComboBox.UIItemIcon(gameModeVSCmb);
			uIItemIcon.m_background.AddObject(unitUI, resIndex[1], resIndex[3]);
			uIItemIcon.m_background.Rect = uIItemIcon.m_background.GetObjectRect();
			uIItemIcon.m_text.Set("font2", str[i], UIConstant.fontColor_cyan, uIItemIcon.m_background.Rect.width);
			uIItemIcon.m_text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
			uIItemIcon.m_text.Rect = uIItemIcon.m_background.Rect;
			uIItemIcon.SetClip(gameModeVSCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-20f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			gameModeVSCmb.Add(uIItemIcon);
		}
		gameModeVSCmb.SetSelection(0);
		gameModeVSCmb.Enable = true;
	}

	public void ResetUIPlayerNum(byte[] resIndex)
	{
		playerNumCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		string[] pLAYER_NUM_COOP = UIConstant.PLAYER_NUM_COOP;
		if (modulePositionRect.y - (float)pLAYER_NUM_COOP.Length * modulePositionRect.height < 10f)
		{
			playerNumCmb.SetComboBox(UIComboBox.Dir.UP);
			playerNumCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y + modulePositionRect.height, modulePositionRect.width, modulePositionRect.height * (float)pLAYER_NUM_COOP.Length);
		}
		else
		{
			playerNumCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)pLAYER_NUM_COOP.Length, modulePositionRect.width, modulePositionRect.height * (float)pLAYER_NUM_COOP.Length);
		}
		for (int i = 0; i < pLAYER_NUM_COOP.Length; i++)
		{
			UIComboBox.UIItemIcon uIItemIcon = new UIComboBox.UIItemIcon(playerNumCmb);
			uIItemIcon.m_background.AddObject(unitUI, resIndex[1], resIndex[3]);
			uIItemIcon.m_background.Rect = uIItemIcon.m_background.GetObjectRect();
			uIItemIcon.m_text.Set("font2", Convert.ToString(pLAYER_NUM_COOP[i]), UIConstant.fontColor_cyan, uIItemIcon.m_background.Rect.width);
			uIItemIcon.m_text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
			uIItemIcon.m_text.Rect = uIItemIcon.m_background.Rect;
			uIItemIcon.SetClip(playerNumCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-20f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			playerNumCmb.Add(uIItemIcon);
		}
		playerNumCmb.SetSelection(pLAYER_NUM_COOP.Length - 1);
	}

	public void ResetUIWinConditionCmb(byte[] resIndex, string[] str)
	{
		winConditionCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		if (modulePositionRect.y - (float)str.Length * modulePositionRect.height < 10f)
		{
			winConditionCmb.SetComboBox(UIComboBox.Dir.UP);
			winConditionCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y + modulePositionRect.height, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		else
		{
			winConditionCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)str.Length, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		for (int i = 0; i < str.Length; i++)
		{
			UIComboBox.UIItemIcon uIItemIcon = new UIComboBox.UIItemIcon(winConditionCmb);
			uIItemIcon.m_background.AddObject(unitUI, resIndex[1], resIndex[3]);
			uIItemIcon.m_background.Rect = uIItemIcon.m_background.GetObjectRect();
			uIItemIcon.m_text.Set("font2", str[i], UIConstant.fontColor_cyan, uIItemIcon.m_background.Rect.width);
			uIItemIcon.m_text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
			uIItemIcon.m_text.Rect = uIItemIcon.m_background.Rect;
			uIItemIcon.SetClip(winConditionCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-15f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			winConditionCmb.Add(uIItemIcon);
		}
		winConditionCmb.SetSelection(0);
	}

	public void ResetUIWinValueCmb(byte[] resIndex, string[] str)
	{
		winValueCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[5];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		if (modulePositionRect.y - (float)str.Length * modulePositionRect.height < 10f)
		{
			winValueCmb.SetComboBox(UIComboBox.Dir.UP);
			winValueCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y + modulePositionRect.height, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		else
		{
			winValueCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)str.Length, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
		}
		for (int i = 0; i < str.Length; i++)
		{
			UIComboBox.UIItemIcon uIItemIcon = new UIComboBox.UIItemIcon(winValueCmb);
			uIItemIcon.m_background.AddObject(unitUI, resIndex[1], resIndex[3]);
			uIItemIcon.m_background.Rect = uIItemIcon.m_background.GetObjectRect();
			uIItemIcon.m_text.Set("font2", str[i], UIConstant.fontColor_cyan, uIItemIcon.m_background.Rect.width);
			uIItemIcon.m_text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
			uIItemIcon.m_text.Rect = uIItemIcon.m_background.Rect;
			uIItemIcon.SetClip(winConditionCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-15f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			winValueCmb.Add(uIItemIcon);
		}
		winValueCmb.SetSelection(0);
	}

	public byte GetMaxPlayers()
	{
		byte b = 0;
		if (GameApp.GetInstance().GetGameMode().IsCoopMode(gameMode))
		{
			return (byte)(playerNumCmb.GetSelectIndex() + 1);
		}
		if (gameMode == Mode.VS_TDM)
		{
			return 8;
		}
		return 8;
	}

	public void OnGUI()
	{
		string userName = Lobby.GetInstance().GetUserName();
		MultiMenuScript multiMenuScript = (MultiMenuScript)stateMgr;
		if (multiMenuScript != null)
		{
			multiMenuScript.playerNameStyle.alignment = TextAnchor.MiddleCenter;
			GUI.Label(nameRect, userName, multiMenuScript.playerNameStyle);
		}
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void ResetPassword(UISliderNum num)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[5];
		for (int i = 0; i < passwordNum.Length; i++)
		{
			if (bPasswordOn)
			{
				passwordNum[i].m_numIcons[0].m_background.SetTexture(ui, 1, 0);
			}
			else
			{
				passwordNum[i].m_numIcons[0].m_background.SetTexture(ui, 1, 10);
			}
			Rect objectRect = passwordNum[i].m_numIcons[0].m_background.GetObjectRect();
			passwordNum[i].m_numIcons[0].m_background.Rect = new Rect(passwordMaskImg[i].Rect.x + (passwordMaskImg[i].Rect.width - objectRect.width) * 0.5f, passwordMaskImg[i].Rect.y + (passwordMaskImg[i].Rect.height - objectRect.height) * 0.5f, objectRect.width, objectRect.height);
			if (num != passwordNum[i])
			{
				passwordNum[i].SetSelection(0);
			}
		}
		if (bPasswordOn)
		{
			passwordImg.SetTexture(ui, 0, 59);
		}
		else
		{
			passwordImg.SetTexture(ui, 0, 58);
		}
	}

	private short PasswordToShort()
	{
		int num = 0;
		if (bPasswordOn)
		{
			int num2 = 1;
			for (int num3 = passwordNum.Length - 1; num3 >= 0; num3--)
			{
				num += passwordNum[num3].GetSelectedIndex() * num2;
				num2 *= 10;
			}
		}
		else
		{
			num = -1;
		}
		return (short)num;
	}

	public void ResetMap()
	{
		if (gameMode == Mode.Survival)
		{
			stageIdx = 0;
			ResetUIStage();
			ResetPageNav();
		}
		else if (gameMode == Mode.Boss)
		{
			stageIdx = (byte)Global.TOTAL_SURVIVAL_STAGE;
			ResetUIStage();
			ResetPageNav();
		}
		else if (gameMode == Mode.VS_TDM)
		{
			stageIdx = (byte)(Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE);
			ResetUIStage();
			ResetPageNav();
		}
		else
		{
			stageIdx = (byte)(Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE);
			ResetUIStage();
			ResetPageNav();
		}
	}

	public void ProcessCmb(UIControl control)
	{
		if (gameModeCoopCmb.m_state == 0 && control != gameModeCoopCmb)
		{
			gameModeCoopCmb.SetState(2);
		}
		if (gameModeVSCmb.m_state == 0 && control != gameModeVSCmb)
		{
			gameModeVSCmb.SetState(2);
		}
		if (playerNumCmb.m_state == 0 && control != playerNumCmb)
		{
			playerNumCmb.SetState(2);
		}
		if (winConditionCmb.m_state == 0 && control != winConditionCmb)
		{
			winConditionCmb.SetState(2);
		}
		if (winValueCmb.m_state == 0 && control != winValueCmb)
		{
			winValueCmb.SetState(2);
		}
	}

	public Mode GetGameMode(int wparam)
	{
		Mode result = Mode.VS_CTF_TDM;
		switch (wparam)
		{
		case 2:
			result = Mode.VS_CTF_TDM;
			break;
		case 3:
			result = Mode.VS_CTF_FFA;
			break;
		case 4:
			result = Mode.VS_TDM;
			break;
		case 5:
			result = Mode.VS_FFA;
			break;
		case 1:
			result = Mode.VS_VIP;
			break;
		case 0:
			result = Mode.VS_CMI;
			break;
		}
		return result;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		ProcessCmb(control);
		if (control == hostNameBtn)
		{
			return;
		}
		if (control == passwordImg)
		{
			bPasswordOn = !bPasswordOn;
			ResetPassword(null);
			return;
		}
		if (control == confirmBtn)
		{
			if (stageIdx < Global.TOTAL_SURVIVAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, 0] != 1)
			{
				return;
			}
			Debug.Log("enter: " + stageIdx + "mode: " + gameMode);
			if (gameMode != Mode.VS_CMI || IsUnlockMapForVS(stageIdx, Global.VS_CMI_UNLOCK_SCENE))
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				m_Parent.SendEvent(this, 0, 0f, 0f);
				UploadArmorAndBagRequest request = new UploadArmorAndBagRequest(GameApp.GetInstance().GetUserState());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				GameApp.GetInstance().GetGameMode().ModePlay = gameMode;
				Debug.Log("create room " + gameMode);
				if (!GameApp.GetInstance().GetGameMode().IsCoopMode())
				{
					bool flag = ((winConditionCmb.GetSelectIndex() == 0) ? true : false);
					short num = 0;
					num = ((!flag) ? Convert.ToInt16(UIConstant.WIN_VALUE_FOR_TIMER[winValueCmb.GetSelectIndex()]) : Convert.ToInt16(UIConstant.WIN_VALUE_FOR_SCORE[winValueCmb.GetSelectIndex()]));
					bool flag2 = ((balanceSwitch.GetSelectIndex() == 0) ? true : false);
					CreateVSRoomRequest request2 = new CreateVSRoomRequest(Lobby.GetInstance().GetUserName(), PasswordToShort(), GetMaxPlayers(), bPasswordOn, (byte)gameMode, stageIdx, GameApp.GetInstance().GetUserState().GetRank()
						.rankID, TimeManager.GetInstance().Ping, (byte)rankLower.GetSelectedIndex(), (byte)rankUpper.GetSelectedIndex(), flag, num, flag2);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					Lobby.GetInstance().WinCondition = (byte)(flag ? 1u : 0u);
					Lobby.GetInstance().WinValue = num;
					Lobby.GetInstance().AutoBalance = (byte)(flag2 ? 1u : 0u);
				}
				else
				{
					CreateRoomRequest request3 = new CreateRoomRequest(Lobby.GetInstance().GetUserName(), PasswordToShort(), GetMaxPlayers(), bPasswordOn, (byte)gameMode, stageIdx, GameApp.GetInstance().GetUserState().GetRank()
						.rankID, TimeManager.GetInstance().Ping, (byte)rankLower.GetSelectedIndex(), (byte)rankUpper.GetSelectedIndex());
					GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				}
				Lobby.GetInstance().SetCurrentRoomMapID(stageIdx);
				Lobby.GetInstance().CurrentRoomMaxPlayer = GetMaxPlayers();
				GameApp.GetInstance().GetUserState().SetNetStage(stageIdx);
			}
			return;
		}
		if (control == cancelBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CANCLE);
			m_Parent.SendEvent(this, 1, 0f, 0f);
			return;
		}
		if (control == gameModeVSCmb)
		{
			if (command != 3)
			{
				return;
			}
			Mode mode = GetGameMode((int)wparam);
			if (gameMode != mode)
			{
				gameMode = mode;
				if (gameMode == Mode.VS_TDM || gameMode == Mode.VS_CTF_TDM || gameMode == Mode.VS_VIP || gameMode == Mode.VS_CMI)
				{
					balanceSwitch.Visible = true;
					balanceImg.Visible = true;
				}
				else
				{
					balanceSwitch.Visible = false;
					balanceImg.Visible = false;
				}
				if (gameMode == Mode.VS_CTF_FFA || gameMode == Mode.VS_CTF_TDM || gameMode == Mode.VS_VIP || gameMode == Mode.VS_CMI)
				{
					winConditionCmb.Enable = false;
					winConditionCmb.SetSelection(0);
					ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.WIN_VALUE_FOR_SCORE);
				}
				else
				{
					winConditionCmb.Enable = true;
				}
				if (GameApp.GetInstance().GetGameMode().IsVSMode(gameMode))
				{
					ResetMapForVS();
				}
			}
			return;
		}
		if (control == gameModeCoopCmb)
		{
			if (command == 3)
			{
				Mode mode2 = (((int)wparam != 0) ? Mode.Survival : Mode.Boss);
				if (gameMode != mode2)
				{
					gameMode = mode2;
					ResetMap();
				}
			}
			return;
		}
		if (control == playerNumCmb)
		{
			switch (command)
			{
			case 0:
			case 3:
			{
				UISliderNum[] array2 = passwordNum;
				foreach (UISliderNum uISliderNum2 in array2)
				{
					uISliderNum2.Enable = true;
				}
				break;
			}
			case 1:
			{
				UISliderNum[] array = passwordNum;
				foreach (UISliderNum uISliderNum in array)
				{
					uISliderNum.Enable = false;
				}
				break;
			}
			}
			return;
		}
		if (control == swapStage)
		{
			if (command == 1)
			{
				AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
				stageIdx = ConvertToStageIndex((byte)(wparam % (float)stageCount));
				selectStagePageImg.Rect = pageNavImg[(byte)(wparam % (float)stageCount)].Rect;
			}
			return;
		}
		if (control == rankLower)
		{
			int num2 = (int)wparam;
			if (command == 1 || command == 0)
			{
				int rankID = GameApp.GetInstance().GetUserState().GetRank()
					.rankID;
				if (rankLower.GetSelectedIndex() > rankID)
				{
					rankLower.SetSelection(rankID);
				}
			}
			return;
		}
		if (control == rankUpper)
		{
			int num3 = (int)wparam;
			if (command == 1 || command == 0)
			{
				int rankID2 = GameApp.GetInstance().GetUserState().GetRank()
					.rankID;
				if (rankUpper.GetSelectedIndex() < rankID2)
				{
					rankUpper.SetSelection(rankID2);
				}
			}
			return;
		}
		if (control == vsModeTab)
		{
			Mode mode3 = (((int)wparam == 0) ? GetGameMode(gameModeVSCmb.GetSelectIndex()) : ((gameModeCoopCmb.GetSelectIndex() != 0) ? Mode.Survival : Mode.Boss));
			if (gameMode != mode3)
			{
				gameMode = mode3;
				InitModePanel();
				ResetMap();
			}
			return;
		}
		if (control == winConditionCmb)
		{
			if (command == 3)
			{
				if ((int)wparam == 0)
				{
					ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.WIN_VALUE_FOR_SCORE);
				}
				else
				{
					ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.WIN_VALUE_FOR_TIMER);
				}
			}
			return;
		}
		for (int k = 0; k < passwordNum.Length; k++)
		{
			if (passwordNum[k] != control)
			{
				continue;
			}
			switch (command)
			{
			case 1:
				if (!bPasswordOn)
				{
					bPasswordOn = true;
					ResetPassword((UISliderNum)control);
				}
				return;
			case 0:
				if (!bPasswordOn)
				{
					bPasswordOn = true;
					ResetPassword((UISliderNum)control);
				}
				return;
			}
		}
	}
}
