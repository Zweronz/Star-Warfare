using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSearchUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Confirm = 0,
		Cancel = 1
	}

	public UIStateManager stateMgr;

	private static byte BACKGROUND_BEGIN_IMG = 3;

	private static byte BACKGROUND_COUNT_IMG = 32;

	private UIBlock m_block;

	private UIImage shadowImg;

	private UIImage backgroundImg;

	private UIImage fillBGImg;

	private static byte[] CONFIRM_NORMAL = new byte[2] { 35, 37 };

	private static byte[] CONFIRM_PRESSED = new byte[2] { 34, 36 };

	private static byte[] CANCEL_NORMAL = new byte[2] { 33, 39 };

	private static byte[] CANCEL_PRESSED = new byte[2] { 32, 38 };

	private UIComboBox gameModeCoopCmb;

	private UIComboBox gameModeVSCmb;

	private UIComboBox playerNumCmb;

	private UIComboBox winConditionCmb;

	private UIComboBox winValueCmb;

	private UITextImage balanceImg;

	private UISliderSwitch balanceSwitch;

	private UISliderTab vsModeTab;

	private UIImage rankBG;

	private static byte[] RANK_BACKGROUND = new byte[6] { 40, 41, 42, 43, 44, 45 };

	private UIImage rankLowerMaskImg;

	private UIImage rankUpperMaskImg;

	private UISliderNum rankLower;

	private UISliderNum rankUpper;

	private static byte[,] TAB_GAME_MODE = new byte[2, 2]
	{
		{ 49, 50 },
		{ 51, 52 }
	};

	private Mode gameMode;

	private static byte[] COMBOBOX_GAMEMODE_VS = new byte[7] { 4, 6, 0, 1, 2, 20, 1 };

	private static byte[] COMBOBOX_GAMEMODE_COOP = new byte[7] { 4, 5, 0, 1, 2, 20, 1 };

	private static byte[] COMBOBOX_COOP_PLAYER_NUM = new byte[7] { 4, 5, 3, 1, 2, 20, 1 };

	private static byte[] COMBOBOX_WIN_CONDITION = new byte[7] { 4, 6, 3, 4, 5, 20, 4 };

	private static byte[] COMBOBOX_WIN_VALUE = new byte[7] { 4, 6, 6, 4, 5, 20, 4 };

	private UIClickButton confirmBtn;

	private UIClickButton cancelBtn;

	public RoomSearchUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[14];
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 4, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 4, 1);
		fillBGImg = new UIImage();
		fillBGImg.AddObject(unitUI, 4, 2);
		fillBGImg.Rect = modulePositionRect;
		fillBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		backgroundImg = new UIImage();
		backgroundImg.AddObject(unitUI, 4, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		backgroundImg.Rect = backgroundImg.GetObjectRect();
		cancelBtn = new UIClickButton();
		cancelBtn.AddObject(UIButtonBase.State.Normal, unitUI, 4, CANCEL_NORMAL);
		cancelBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 4, CANCEL_PRESSED);
		cancelBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 4, CANCEL_NORMAL);
		cancelBtn.Rect = cancelBtn.GetObjectRect(UIButtonBase.State.Normal);
		confirmBtn = new UIClickButton();
		confirmBtn.AddObject(UIButtonBase.State.Normal, unitUI, 4, CONFIRM_NORMAL);
		confirmBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 4, CONFIRM_PRESSED);
		confirmBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 4, CONFIRM_NORMAL);
		confirmBtn.Rect = confirmBtn.GetObjectRect(UIButtonBase.State.Normal);
		vsModeTab = new UISliderTab();
		vsModeTab.Create();
		InitVSMode();
		gameModeVSCmb = new UIComboBox();
		gameModeVSCmb.Create(unitUI, COMBOBOX_GAMEMODE_VS);
		ResetUIGameSubModeForVS(COMBOBOX_GAMEMODE_VS, UIConstant.STR_SEARCH_ROOM_GAME_MODE_VS);
		gameModeCoopCmb = new UIComboBox();
		gameModeCoopCmb.Create(unitUI, COMBOBOX_GAMEMODE_COOP);
		ResetUIGameSubModeForCoop(COMBOBOX_GAMEMODE_COOP, UIConstant.STR_SEARCH_ROOM_GAME_MODE_COOP);
		playerNumCmb = new UIComboBox();
		playerNumCmb.Create(unitUI, COMBOBOX_COOP_PLAYER_NUM);
		ResetUIPlayerNum(COMBOBOX_COOP_PLAYER_NUM);
		winConditionCmb = new UIComboBox();
		winConditionCmb.Create(unitUI, COMBOBOX_WIN_CONDITION);
		ResetUIWinConditionCmb(COMBOBOX_WIN_CONDITION, UIConstant.STR_SEARCH_ROOM_WIN_CONDITION);
		winValueCmb = new UIComboBox();
		winValueCmb.Create(unitUI, COMBOBOX_WIN_VALUE);
		ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.STR_SEARCH_ROOM_WIN_VALUE_FOR_SCORE);
		winValueCmb.Enable = false;
		balanceImg = new UITextImage();
		balanceImg.AddObject(unitUI, 6, 7);
		balanceImg.Rect = balanceImg.GetObjectRect();
		balanceImg.SetTextOffset(18f, -10f);
		balanceImg.SetText("font2", "AUTO BALANCE", UIConstant.fontColor_white, FrUIText.enAlignStyle.TOP_LEFT, balanceImg.Rect.width);
		balanceSwitch = new UISliderSwitch();
		balanceSwitch.Create(unitUI);
		InitbalanceSwitch();
		InitModePanel();
		rankBG = new UIImage();
		rankBG.AddObject(unitUI, 4, RANK_BACKGROUND);
		rankBG.Rect = rankBG.GetObjectRect();
		rankLowerMaskImg = new UIImage();
		rankLowerMaskImg.AddObject(unitUI, 4, 46);
		rankLowerMaskImg.Rect = rankLowerMaskImg.GetObjectRect();
		rankLower = new UISliderNum();
		rankLower.Create(unitUI2, 0, 21);
		float num = 0f;
		Rect modulePositionRect2 = unitUI2.GetModulePositionRect(0, 0, 21);
		List<Rank> rankList = GameApp.GetInstance().GetUserState().GetRankList();
		for (int i = 0; i < rankList.Count; i++)
		{
			UISliderNum.UINumIcon uINumIcon = new UISliderNum.UINumIcon();
			uINumIcon.m_background = new UIImage();
			uINumIcon.m_background.AddObject(unitUI2, 0, i + 21);
			Rect objectRect = uINumIcon.m_background.GetObjectRect();
			uINumIcon.m_background.Rect = new Rect(rankLowerMaskImg.Rect.x + (rankLowerMaskImg.Rect.width - objectRect.width) * 0.5f, rankLowerMaskImg.Rect.y + (rankLowerMaskImg.Rect.height - objectRect.height) * 0.5f, objectRect.width, objectRect.height);
			uINumIcon.Visible = false;
			uINumIcon.Enable = false;
			uINumIcon.Rect = uINumIcon.m_background.Rect;
			rankLower.Add(uINumIcon);
		}
		rankLower.SetClipRect(rankLowerMaskImg.Rect.x, rankLowerMaskImg.Rect.y, rankLowerMaskImg.Rect.width, rankLowerMaskImg.Rect.height);
		num = modulePositionRect2.height + 5f;
		rankLower.SetScroller(0f, (float)(rankList.Count - 1) * num, num, rankLower.m_showRect, false);
		rankLower.SetSelection(0);
		rankLower.SetRangePos(0, (int)((float)(int)GameApp.GetInstance().GetUserState().GetRank()
			.rankID * num));
		rankUpperMaskImg = new UIImage();
		rankUpperMaskImg.AddObject(unitUI, 4, 47);
		rankUpperMaskImg.Rect = rankUpperMaskImg.GetObjectRect();
		rankUpper = new UISliderNum();
		rankUpper.Create(unitUI2, 0, 21);
		for (int j = 0; j < rankList.Count; j++)
		{
			UISliderNum.UINumIcon uINumIcon2 = new UISliderNum.UINumIcon();
			uINumIcon2.m_background = new UIImage();
			uINumIcon2.m_background.AddObject(unitUI2, 0, j + 21);
			Rect objectRect2 = uINumIcon2.m_background.GetObjectRect();
			uINumIcon2.m_background.Rect = new Rect(rankUpperMaskImg.Rect.x + (rankUpperMaskImg.Rect.width - objectRect2.width) * 0.5f, rankUpperMaskImg.Rect.y + (rankUpperMaskImg.Rect.height - objectRect2.height) * 0.5f, objectRect2.width, objectRect2.height);
			uINumIcon2.Visible = false;
			uINumIcon2.Enable = false;
			uINumIcon2.Rect = uINumIcon2.m_background.Rect;
			rankUpper.Add(uINumIcon2);
		}
		rankUpper.SetClipRect(rankUpperMaskImg.Rect.x, rankUpperMaskImg.Rect.y, rankUpperMaskImg.Rect.width, rankUpperMaskImg.Rect.height);
		num = modulePositionRect2.height + 5f;
		rankUpper.SetScroller(0f, (float)(rankList.Count - 1) * num, num, rankUpper.m_showRect, false);
		rankUpper.SetSelection(rankList.Count - 1);
		rankUpper.SetRangePos((int)((float)(int)GameApp.GetInstance().GetUserState().GetRank()
			.rankID * num), 10000);
		Add(shadowImg);
		Add(backgroundImg);
		Add(fillBGImg);
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
		gameModeCoopCmb.Clear();
		gameModeVSCmb.Clear();
		playerNumCmb.Clear();
		winConditionCmb.Clear();
		winValueCmb.Clear();
		rankLower.Clear();
		rankUpper.Clear();
		base.Clear();
	}

	public void InitModePanel()
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode(gameMode))
		{
			playerNumCmb.Visible = true;
			playerNumCmb.Enable = true;
			gameModeCoopCmb.Visible = true;
			gameModeCoopCmb.Enable = true;
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
			winConditionCmb.SetSelection(1);
			winValueCmb.Enable = true;
			winConditionCmb.Enable = false;
		}
		else
		{
			winConditionCmb.Enable = true;
		}
	}

	public void InitbalanceSwitch()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[4];
		balanceSwitch.SetBackground(ui, 6, 9);
		balanceSwitch.SetSlider(ui, 6, 8);
		balanceSwitch.SetClipRect(balanceSwitch.backgroundImg.Rect.x + 1f, balanceSwitch.backgroundImg.Rect.y, balanceSwitch.backgroundImg.Rect.width - 2f, balanceSwitch.backgroundImg.Rect.height);
		balanceSwitch.SetScroller(0f, 67f, 67f, balanceSwitch.backgroundImg.Rect);
		balanceSwitch.SetSelection(0);
	}

	public void InitVSMode()
	{
		gameMode = Mode.VS_TDM;
		UnitUI ui = Res2DManager.GetInstance().vUI[4];
		vsModeTab.Clear();
		vsModeTab.SetBackground(ui, 4, 48);
		vsModeTab.SetSlider(ui, 4, 53);
		vsModeTab.SetClipRect(vsModeTab.backgroundImg.Rect.x, vsModeTab.backgroundImg.Rect.y, vsModeTab.backgroundImg.Rect.width, vsModeTab.backgroundImg.Rect.height);
		for (int i = 0; i < UIConstant.GAME_MODE_TOTAL; i++)
		{
			UITab uITab = new UITab();
			uITab.AddObject(UITab.State.Normal, ui, 4, TAB_GAME_MODE[i, 0]);
			uITab.AddObject(UITab.State.Selected, ui, 4, TAB_GAME_MODE[i, 1]);
			uITab.Rect = uITab.GetObjectRect(UITab.State.Normal);
			uITab.Enable = false;
			vsModeTab.Add(uITab);
		}
		vsModeTab.SetScroller(0f, 165f, 165f, vsModeTab.backgroundImg.Rect);
		vsModeTab.m_scroller.DeltaTime = 0.016f;
		vsModeTab.SetVelocity(new Vector2(25f, 25f));
		if (GameApp.GetInstance().GetGameMode().IsVSMode(gameMode))
		{
			vsModeTab.SetSelection(0);
		}
		else
		{
			vsModeTab.SetSelection(1);
		}
	}

	public void ResetUIGameSubModeForVS(byte[] resIndex, string[] str)
	{
		gameModeVSCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		gameModeVSCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)str.Length, modulePositionRect.width, modulePositionRect.height * (float)str.Length);
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

	public void ResetUIGameSubModeForCoop(byte[] resIndex, string[] str)
	{
		gameModeCoopCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
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
			uIItemIcon.SetClip(gameModeCoopCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-20f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			gameModeCoopCmb.Add(uIItemIcon);
		}
		gameModeCoopCmb.SetSelection(0);
		gameModeCoopCmb.Enable = true;
	}

	public void ResetUIPlayerNum(byte[] resIndex)
	{
		playerNumCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, resIndex[1], resIndex[2]);
		string[] sTR_SEARCH_ROOM_PLAYER_NUM_COOP = UIConstant.STR_SEARCH_ROOM_PLAYER_NUM_COOP;
		if (modulePositionRect.y - (float)sTR_SEARCH_ROOM_PLAYER_NUM_COOP.Length * modulePositionRect.height < 10f)
		{
			playerNumCmb.SetComboBox(UIComboBox.Dir.UP);
			playerNumCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y + modulePositionRect.height, modulePositionRect.width, modulePositionRect.height * (float)sTR_SEARCH_ROOM_PLAYER_NUM_COOP.Length);
		}
		else
		{
			playerNumCmb.SetClipRect(modulePositionRect.x, modulePositionRect.y - modulePositionRect.height * (float)sTR_SEARCH_ROOM_PLAYER_NUM_COOP.Length, modulePositionRect.width, modulePositionRect.height * (float)sTR_SEARCH_ROOM_PLAYER_NUM_COOP.Length);
		}
		for (int i = 0; i < sTR_SEARCH_ROOM_PLAYER_NUM_COOP.Length; i++)
		{
			UIComboBox.UIItemIcon uIItemIcon = new UIComboBox.UIItemIcon(playerNumCmb);
			uIItemIcon.m_background.AddObject(unitUI, resIndex[1], resIndex[3]);
			uIItemIcon.m_background.Rect = uIItemIcon.m_background.GetObjectRect();
			uIItemIcon.m_text.Set("font2", Convert.ToString(sTR_SEARCH_ROOM_PLAYER_NUM_COOP[i]), UIConstant.fontColor_cyan, uIItemIcon.m_background.Rect.width);
			uIItemIcon.m_text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
			uIItemIcon.m_text.Rect = uIItemIcon.m_background.Rect;
			uIItemIcon.SetClip(playerNumCmb.m_showRect);
			uIItemIcon.Visible = false;
			uIItemIcon.Enable = false;
			uIItemIcon.m_textOffsetLeftTop = new Vector2(-20f, 0f);
			uIItemIcon.Rect = uIItemIcon.m_background.Rect;
			playerNumCmb.Add(uIItemIcon);
		}
		playerNumCmb.SetSelection(0);
	}

	public void ResetUIWinConditionCmb(byte[] resIndex, string[] str)
	{
		winConditionCmb.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
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
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
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

	private Mode GetGameMode(int wparam)
	{
		Mode result = gameMode;
		switch (wparam)
		{
		case 1:
			result = Mode.VS_CTF_TDM;
			break;
		case 2:
			result = Mode.VS_CTF_FFA;
			break;
		case 3:
			result = Mode.VS_TDM;
			break;
		case 4:
			result = Mode.VS_FFA;
			break;
		case 5:
			result = Mode.VS_VIP;
			break;
		case 6:
			result = Mode.VS_CMI;
			break;
		}
		return result;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		ProcessCmb(control);
		if (control == confirmBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			m_Parent.SendEvent(this, 0, 0f, 0f);
		}
		else if (control == cancelBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CANCLE);
			m_Parent.SendEvent(this, 1, 0f, 0f);
		}
		else if (control == gameModeVSCmb)
		{
			if (command != 3)
			{
				return;
			}
			Mode mode = gameMode;
			mode = GetGameMode((int)wparam);
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
			}
			if (gameMode == Mode.VS_CTF_TDM || gameMode == Mode.VS_CTF_FFA || gameMode == Mode.VS_VIP || gameMode == Mode.VS_CMI)
			{
				winConditionCmb.Enable = false;
				winConditionCmb.SetSelection(1);
				ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.STR_SEARCH_ROOM_WIN_VALUE_FOR_SCORE);
				winValueCmb.Enable = true;
			}
			else
			{
				winConditionCmb.Enable = true;
			}
			if (wparam == 0f)
			{
				winConditionCmb.Enable = true;
			}
			Debug.Log("gameMode:" + gameMode);
		}
		else if (control == gameModeCoopCmb)
		{
			if (command == 3)
			{
				Mode mode2 = gameMode;
				if ((int)wparam == 1)
				{
					mode2 = Mode.Survival;
				}
				else if ((int)wparam == 2)
				{
					mode2 = Mode.Boss;
				}
				if (gameMode != mode2)
				{
					gameMode = mode2;
				}
			}
		}
		else if (control == rankLower)
		{
			int num = (int)wparam;
			if (command == 1 || command == 0)
			{
				int rankID = GameApp.GetInstance().GetUserState().GetRank()
					.rankID;
				if (rankLower.GetSelectedIndex() > rankID)
				{
					rankLower.SetSelection(rankID);
				}
			}
		}
		else if (control == rankUpper)
		{
			int num2 = (int)wparam;
			if (command == 1 || command == 0)
			{
				int rankID2 = GameApp.GetInstance().GetUserState().GetRank()
					.rankID;
				if (rankUpper.GetSelectedIndex() < rankID2)
				{
					rankUpper.SetSelection(rankID2);
				}
			}
		}
		else if (control == vsModeTab)
		{
			Mode mode3 = (((int)wparam != 0) ? Mode.Survival : Mode.VS_CTF_TDM);
			if (gameMode != mode3)
			{
				gameMode = mode3;
				InitModePanel();
			}
		}
		else if (control == winConditionCmb && command == 3)
		{
			if ((int)wparam == 1)
			{
				ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.STR_SEARCH_ROOM_WIN_VALUE_FOR_SCORE);
				winValueCmb.Enable = true;
			}
			else if ((int)wparam == 2)
			{
				ResetUIWinValueCmb(COMBOBOX_WIN_VALUE, UIConstant.STR_SEARCH_ROOM_WIN_VALUE_FOR_TIMER);
				winValueCmb.Enable = true;
			}
			else
			{
				winValueCmb.SetSelection(0);
				winValueCmb.Enable = false;
			}
		}
	}

	public void ResetAllConditions()
	{
		gameMode = Mode.VS_TDM;
		gameModeVSCmb.SetSelection(0);
		vsModeTab.SetSelection(0);
		winConditionCmb.SetSelection(0);
		winValueCmb.SetSelection(0);
		winValueCmb.Enable = false;
		balanceSwitch.SetSelection(0);
		playerNumCmb.SetSelection(0);
		rankLower.SetSelection(0);
		rankUpper.SetSelection(GameApp.GetInstance().GetUserState().GetRankList()
			.Count - 1);
		InitModePanel();
	}

	public byte GetOnlineMode()
	{
		return vsModeTab.GetSelectIndex();
	}

	public byte GetGameMode()
	{
		byte b = 0;
		if (vsModeTab.GetSelectIndex() == 0)
		{
			if (gameModeVSCmb.GetSelectIndex() == 1)
			{
				b = 5;
			}
			else if (gameModeVSCmb.GetSelectIndex() == 2)
			{
				b = 4;
			}
			else if (gameModeVSCmb.GetSelectIndex() == 3)
			{
				b = 3;
			}
			else if (gameModeVSCmb.GetSelectIndex() == 4)
			{
				b = 8;
			}
			else if (gameModeVSCmb.GetSelectIndex() == 5)
			{
				b = 9;
				Debug.Log("gameMode:" + b);
			}
		}
		else if (vsModeTab.GetSelectIndex() == 1)
		{
			if (gameModeCoopCmb.GetSelectIndex() == 1)
			{
				b = 1;
			}
			else if (gameModeCoopCmb.GetSelectIndex() == 2)
			{
				b = 2;
			}
		}
		return b;
	}

	public byte GetPlayerNumber()
	{
		return UIConstant.SEARCH_ROOM_PLAYER_NUM_COOP[playerNumCmb.GetSelectIndex()];
	}

	public byte GetWinCondition()
	{
		return UIConstant.SEARCH_ROOM_WIN_CONDITION[winConditionCmb.GetSelectIndex()];
	}

	public short GetWinValue()
	{
		return (winConditionCmb.GetSelectIndex() != 1) ? UIConstant.SEARCH_ROOM_WIN_VALUE_FOR_TIMER[winValueCmb.GetSelectIndex()] : UIConstant.SEARCH_ROOM_WIN_VALUE_FOR_SCORE[winValueCmb.GetSelectIndex()];
	}

	public bool GetIsBalance()
	{
		return (balanceSwitch.GetSelectIndex() == 0) ? true : false;
	}

	public byte GetLowerRankID()
	{
		return (byte)rankLower.GetSelectedIndex();
	}

	public byte GetUpperRankID()
	{
		return (byte)rankUpper.GetSelectedIndex();
	}
}
