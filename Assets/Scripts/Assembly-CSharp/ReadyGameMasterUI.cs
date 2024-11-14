using System;
using UnityEngine;

public class ReadyGameMasterUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	private byte state;

	private bool mIsTimeSync;

	private static byte[] BG_IMG = new byte[2] { 0, 1 };

	private static byte BRIEF_BEGIN_IMG = 4;

	private static byte BRIEF_COUNT_IMG = 14;

	private UIImage readyGameImg;

	private NavigationBarUI navigationBar;

	private UIImage mapBGImg;

	private UIImage briefBGImg;

	private UITextImage briefImg;

	private UIClickButton startBtn;

	private static byte[] START_NORMAL = new byte[2] { 20, 32 };

	private static byte[] START_PRESSED = new byte[2] { 18, 31 };

	private static byte[] START_NORMAL_FOR_VS = new byte[2] { 20, 46 };

	private static byte[] START_PRESSED_FOR_VS = new byte[2] { 18, 45 };

	private UIListPlayer listPlayer;

	private NetLoadingUI netLoadingUI;

	private DateTime m_time;

	private UIImage m_waitingTimeImg;

	private UINumeric m_waitingTimeNum;

	private static int WAITING_TIMER = 15;

	private UIImage m_gameModeImg;

	private FrUIText m_winConditionTxt;

	private UINumeric m_winValueNum;

	private FrUIText m_autoBalanceTxt;

	private UIImage m_autoBalanceImg;

	private UIImage ipadImg;

	public ReadyGameMasterUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
		mIsTimeSync = false;
		TimeManager.GetInstance().Init();
		TimeManager.GetInstance().setMaxLoopTimes(5);
		TimeManager.GetInstance().setPeriod(0.2f);
	}

	public void Close()
	{
		listPlayer.Clear();
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public bool Create()
	{
		m_time = DateTime.Now;
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[6];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		if (unitUI == null)
		{
			return false;
		}
		readyGameImg = new UIImage();
		readyGameImg.AddObject(unitUI, 0, BG_IMG);
		readyGameImg.Rect = readyGameImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("READY GAME");
		navigationBar.Show();
		mapBGImg = new UIImage();
		mapBGImg.AddObject(unitUI, 0, 21);
		mapBGImg.Rect = mapBGImg.GetObjectRect();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		briefBGImg = new UIImage();
		briefBGImg.AddObject(unitUI, 0, 3);
		briefBGImg.Rect = briefBGImg.GetObjectRect();
		briefBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		briefImg = new UITextImage();
		briefImg.AddObject(unitUI, 0, BRIEF_BEGIN_IMG, BRIEF_COUNT_IMG);
		briefImg.SetText("font1", "***", UIConstant.fontColor_cyan);
		briefImg.Rect = briefImg.GetObjectRect();
		m_waitingTimeImg = new UIImage();
		m_waitingTimeImg.AddObject(unitUI, 0, 43);
		m_waitingTimeImg.Rect = m_waitingTimeImg.GetObjectRect();
		m_waitingTimeNum = new UINumeric();
		m_waitingTimeNum.AlignStyle = UINumeric.enAlignStyle.left;
		m_waitingTimeNum.SpacingOffsetX = -8f;
		m_waitingTimeNum.SetNumeric(ui, 1, Convert.ToString(WAITING_TIMER));
		m_waitingTimeNum.Rect = unitUI.GetModulePositionRect(0, 0, 44);
		startBtn = new UIClickButton();
		InitStartGameBtn();
		listPlayer = new UIListPlayer();
		listPlayer.Create(unitUI);
		AddDecorate(unitUI);
		ResetUIPlayer();
		stateMgr.m_UIManager.Add(readyGameImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(briefBGImg);
		stateMgr.m_UIManager.Add(briefImg);
		stateMgr.m_UIManager.Add(mapBGImg);
		stateMgr.m_UIManager.Add(startBtn);
		stateMgr.m_UIManager.Add(listPlayer);
		stateMgr.m_UIManager.Add(m_waitingTimeImg);
		stateMgr.m_UIManager.Add(m_waitingTimeNum);
		netLoadingUI = new NetLoadingUI(stateMgr);
		netLoadingUI.Create();
		netLoadingUI.Hide();
		stateMgr.m_UIPopupManager.Add(netLoadingUI);
		m_gameModeImg = new UIImage();
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.Survival)
		{
			m_gameModeImg.AddObject(unitUI, 0, 47);
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.Boss)
		{
			m_gameModeImg.AddObject(unitUI, 0, 48);
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM)
		{
			m_gameModeImg.AddObject(unitUI, 0, 52);
			InitWinInfo();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
		{
			m_gameModeImg.AddObject(unitUI, 0, 50);
			InitWinInfo();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
		{
			m_gameModeImg.AddObject(unitUI, 0, 51);
			InitWinInfo();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA)
		{
			m_gameModeImg.AddObject(unitUI, 0, 53);
			InitWinInfo();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP)
		{
			m_gameModeImg.AddObject(unitUI, 0, 49);
			InitWinInfo();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			m_gameModeImg.AddObject(unitUI, 0, 58);
			InitWinInfo();
		}
		m_gameModeImg.Rect = m_gameModeImg.GetObjectRect();
		stateMgr.m_UIManager.Add(m_gameModeImg);
		UnitUI ui2 = Res2DManager.GetInstance().vUI[22];
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width) / 2;
			num2 *= 1.5f;
			ipadImg = new UIImage();
			ipadImg.AddObject(ui2, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			ipadImg.Rect = new Rect((0f - num2) / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(ipadImg);
			ipadImg = new UIImage();
			ipadImg.AddObject(ui2, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			ipadImg.Rect = new Rect(UIConstant.ScreenLocalWidth + num2 / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(ipadImg);
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height) / 2;
			num3 *= 1.5f;
			ipadImg = new UIImage();
			ipadImg.AddObject(ui2, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			ipadImg.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, (0f - num3) / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(ipadImg);
			ipadImg = new UIImage();
			ipadImg.AddObject(ui2, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			ipadImg.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, UIConstant.ScreenLocalHeight + num3 / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(ipadImg);
		}
		return true;
	}

	public void InitWinInfo()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[6];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		m_winConditionTxt = new FrUIText();
		m_winValueNum = new UINumeric();
		m_autoBalanceTxt = new FrUIText();
		m_autoBalanceImg = new UIImage();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 54);
		m_winConditionTxt.Rect = modulePositionRect;
		m_winConditionTxt.AlignStyle = FrUIText.enAlignStyle.TOP_RIGHT;
		if (Lobby.GetInstance().WinCondition == 1)
		{
			m_winConditionTxt.Set("font3", "SCORE:", UIConstant.FONT_COLOR_WIN_INFO_READY_GAME, modulePositionRect.width);
			short winValue = Lobby.GetInstance().WinValue;
			m_winValueNum.AlignStyle = UINumeric.enAlignStyle.left;
			m_winValueNum.SpacingOffsetX = -7f;
			m_winValueNum.SetNumeric(ui, 0, winValue.ToString());
			m_winValueNum.Rect = new Rect(modulePositionRect.xMax + 10f, modulePositionRect.yMin - 8f, 100f, 25f);
		}
		else
		{
			m_winConditionTxt.Set("font3", "TIME:", UIConstant.FONT_COLOR_WIN_INFO_READY_GAME, modulePositionRect.width);
			int num = (int)((float)Lobby.GetInstance().WinValue * 60f);
			if (num < 0)
			{
				num = 0;
			}
			int num2 = num / 60;
			int num3 = num - num2 * 60;
			m_winValueNum.AlignStyle = UINumeric.enAlignStyle.left;
			m_winValueNum.SpacingOffsetX = -7f;
			m_winValueNum.SetNumeric(ui, 0, string.Format("{0:D2}", num2) + ":" + string.Format("{0:D2}", num3));
			m_winValueNum.Rect = new Rect(modulePositionRect.xMax + 10f, modulePositionRect.yMin - 8f, 100f, 25f);
		}
		modulePositionRect = unitUI.GetModulePositionRect(0, 0, 55);
		m_autoBalanceTxt.Rect = modulePositionRect;
		m_autoBalanceTxt.AlignStyle = FrUIText.enAlignStyle.TOP_RIGHT;
		m_autoBalanceTxt.Set("font3", "AUTOBALANCE:", UIConstant.FONT_COLOR_WIN_INFO_READY_GAME, modulePositionRect.width);
		if (Lobby.GetInstance().AutoBalance == 1)
		{
			m_autoBalanceImg.AddObject(unitUI, 0, 56);
		}
		else
		{
			m_autoBalanceImg.AddObject(unitUI, 0, 57);
		}
		m_autoBalanceImg.Rect = m_autoBalanceImg.GetObjectRect();
		stateMgr.m_UIManager.Add(m_winConditionTxt);
		stateMgr.m_UIManager.Add(m_winValueNum);
		stateMgr.m_UIManager.Add(m_autoBalanceTxt);
		stateMgr.m_UIManager.Add(m_autoBalanceImg);
	}

	public void InitStartGameBtn()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[6];
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			startBtn.AddObject(UIButtonBase.State.Normal, ui, 0, START_NORMAL);
			startBtn.AddObject(UIButtonBase.State.Pressed, ui, 0, START_PRESSED);
			startBtn.Rect = startBtn.GetObjectRect(UIButtonBase.State.Normal);
			m_waitingTimeImg.Visible = false;
			m_waitingTimeNum.Visible = false;
		}
		else
		{
			startBtn.AddObject(UIButtonBase.State.Normal, ui, 0, START_NORMAL_FOR_VS);
			startBtn.AddObject(UIButtonBase.State.Pressed, ui, 0, START_PRESSED_FOR_VS);
			startBtn.Rect = startBtn.GetObjectRect(UIButtonBase.State.Normal);
			m_waitingTimeImg.Visible = true;
			m_waitingTimeNum.Visible = true;
		}
	}

	public void AddDecorate(UnitUI ui)
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			listPlayer.m_decorate.AddObject(ui, 0, 39, 4);
			listPlayer.m_decorate.Rect = listPlayer.m_decorate.GetObjectRect();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			listPlayer.m_decorate.AddObject(ui, 1, 8, 8);
			listPlayer.m_decorate.Rect = listPlayer.m_decorate.GetObjectRect();
		}
		else
		{
			listPlayer.m_decorate.AddObject(ui, 2, 8, 4);
			listPlayer.m_decorate.Rect = listPlayer.m_decorate.GetObjectRect();
		}
	}

	public void ResetUIPlayer()
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			ResetUIPlayerForSurvivalOrBoss();
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			ResetUIPlayerForTDM();
		}
		else
		{
			ResetUIPlayerForFFA();
		}
	}

	public void HideNetLoading()
	{
		netLoadingUI.Hide();
	}

	public void ResetUIPlayerForSurvivalOrBoss()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[6];
		if (unitUI == null || listPlayer == null)
		{
			return;
		}
		listPlayer.Clear();
		RoomPlayer[] array = null;
		Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
		if (currentRoom != null)
		{
			array = currentRoom.GetAllPlayers();
			for (int i = 0; i < 6; i++)
			{
				UIListPlayer.UIPlayerIcon uIPlayerIcon = new UIListPlayer.UIPlayerIcon();
				uIPlayerIcon.m_background = new UIImage();
				uIPlayerIcon.m_background.AddObject(unitUI, 0, 33 + i);
				uIPlayerIcon.m_background.Rect = uIPlayerIcon.m_background.GetObjectRect();
				uIPlayerIcon.m_rankIcon = new UIImage();
				uIPlayerIcon.m_flagIcon = new UIImage();
				uIPlayerIcon.Id = i;
				uIPlayerIcon.Enable = false;
				listPlayer.Add(uIPlayerIcon);
				if (array != null && i < array.Length && array[i] != null)
				{
					listPlayer.SetPlayerTexture(i, unitUI, array[i]);
					uIPlayerIcon.m_flagIcon.SetColor(UIConstant.COLOR_PLAYER_ICONS[i]);
				}
			}
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.Survival)
			{
				mapBGImg.SetTexture(unitUI, 3, Lobby.GetInstance().GetCurrentRoomMapID());
			}
			else
			{
				mapBGImg.SetTexture(unitUI, 4, Lobby.GetInstance().GetCurrentRoomMapID() - Global.TOTAL_SURVIVAL_STAGE);
			}
		}
		ResetStartBtn();
	}

	public void ResetUIPlayerForFFA()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[6];
		if (unitUI == null || listPlayer == null)
		{
			return;
		}
		listPlayer.Clear();
		RoomPlayer[] array = null;
		Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
		if (currentRoom == null)
		{
			return;
		}
		array = currentRoom.GetAllPlayers();
		for (int i = 0; i < 8; i++)
		{
			UIListPlayer.UIPlayerIcon uIPlayerIcon = new UIListPlayer.UIPlayerIcon();
			uIPlayerIcon.m_background = new UIImage();
			uIPlayerIcon.m_background.AddObject(unitUI, 2, 0 + i);
			uIPlayerIcon.m_background.Rect = uIPlayerIcon.m_background.GetObjectRect();
			uIPlayerIcon.m_rankIcon = new UIImage();
			uIPlayerIcon.m_flagIcon = new UIImage();
			uIPlayerIcon.Id = i;
			uIPlayerIcon.Enable = false;
			listPlayer.Add(uIPlayerIcon);
			if (array != null && i < array.Length && array[i] != null)
			{
				listPlayer.SetPlayerTexture(i, unitUI, array[i]);
				uIPlayerIcon.m_flagIcon.SetColor(UIConstant.COLOR_PLAYER_ICONS[i]);
			}
		}
		mapBGImg.SetTexture(unitUI, 5, Lobby.GetInstance().GetCurrentRoomMapID() - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
	}

	public void ResetUIPlayerForTDM()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[6];
		if (unitUI == null || listPlayer == null)
		{
			return;
		}
		listPlayer.Clear();
		RoomPlayer[] array = null;
		Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
		if (currentRoom == null)
		{
			return;
		}
		array = currentRoom.GetAllPlayers();
		for (int i = 0; i < 8; i++)
		{
			UIListPlayer.UIPlayerIcon uIPlayerIcon = new UIListPlayer.UIPlayerIcon();
			uIPlayerIcon.m_background = new UIImage();
			uIPlayerIcon.m_background.AddObject(unitUI, 1, 0 + i);
			uIPlayerIcon.m_background.Rect = uIPlayerIcon.m_background.GetObjectRect();
			uIPlayerIcon.m_rankIcon = new UIImage();
			uIPlayerIcon.m_flagIcon = new UIImage();
			uIPlayerIcon.Id = i;
			uIPlayerIcon.Enable = true;
			uIPlayerIcon.Rect = uIPlayerIcon.m_background.Rect;
			listPlayer.Add(uIPlayerIcon);
			if (array != null && i < array.Length && array[i] != null)
			{
				listPlayer.SetPlayerTexture(i, unitUI, array[i]);
				if (i / 4 == 0)
				{
					uIPlayerIcon.m_flagIcon.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[0]);
				}
				else
				{
					uIPlayerIcon.m_flagIcon.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[1]);
				}
			}
		}
		mapBGImg.SetTexture(unitUI, 5, Lobby.GetInstance().GetCurrentRoomMapID() - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
	}

	public void ResetStartBtn()
	{
		if (Lobby.GetInstance().IsMasterPlayer)
		{
			startBtn.Visible = true;
			startBtn.Enable = true;
		}
		else
		{
			startBtn.Visible = false;
			startBtn.Enable = false;
		}
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			if (Create())
			{
				state = 2;
				GetRoomDataRequest request = new GetRoomDataRequest(Lobby.GetInstance().GetCurrentRoomID());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			break;
		case 2:
		{
			if (!GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				TimeSpan timeSpan = DateTime.Now - m_time;
				UnitUI unitUI = Res2DManager.GetInstance().vUI[17];
				if (unitUI != null)
				{
					int num = Mathf.Min((int)timeSpan.TotalSeconds, WAITING_TIMER);
					m_waitingTimeNum.SetNumeric(unitUI, 1, Convert.ToString(WAITING_TIMER - num));
				}
				if (timeSpan.TotalSeconds >= (double)WAITING_TIMER)
				{
					StartGame();
				}
			}
			UITouchInner[] array = iPhoneInputMgr.MockTouches();
			foreach (UITouchInner touch in array)
			{
				if (!(stateMgr.m_UIManager != null) || stateMgr.m_UIManager.HandleInput(touch))
				{
				}
			}
			break;
		}
		}
		return false;
	}

	private void StartGame()
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			StartGameRequest request = new StartGameRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			netLoadingUI.Show(15);
		}
		else if (GameApp.GetInstance().GetGameMode().IsTeamMode())
		{
			PlayerJoinTeamStartGameRequest request2 = new PlayerJoinTeamStartGameRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			netLoadingUI.Show(15);
		}
		else
		{
			PlayerJoinTeamStartGameRequest request3 = new PlayerJoinTeamStartGameRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
			netLoadingUI.Show(15);
		}
	}

	public void OnGUI()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[6];
		if (unitUI == null || listPlayer == null || !listPlayer.Visible || listPlayer.m_playerLst == null)
		{
			return;
		}
		MultiMenuScript multiMenuScript = (MultiMenuScript)stateMgr;
		if (!(multiMenuScript != null))
		{
			return;
		}
		for (int i = 0; i < listPlayer.m_playerLst.Count; i++)
		{
			UIListPlayer.UIPlayerIcon uIPlayerIcon = listPlayer.m_playerLst[i];
			if (!uIPlayerIcon.m_userName.Equals(string.Empty))
			{
				multiMenuScript.playerNameStyle.alignment = TextAnchor.MiddleLeft;
				GUI.Label(uIPlayerIcon.m_userNamePos, uIPlayerIcon.m_userName, multiMenuScript.playerNameStyle);
			}
		}
	}

	private int GetPlayerNumInTeam(RoomPlayer[] players, bool blue)
	{
		int num = 0;
		for (int i = 0; i < players.Length; i++)
		{
			if (blue)
			{
				if (i < 4 && players[i] != null)
				{
					num++;
				}
			}
			else if (i >= 4 && players[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	public bool IsSameTeam(int scrSeat, int dstSeat)
	{
		if (scrSeat < 4 && dstSeat < 4)
		{
			return true;
		}
		if (scrSeat >= 4 && dstSeat >= 4)
		{
			return true;
		}
		return false;
	}

	public bool IsBlueTeam(int seatID)
	{
		if (seatID < 4)
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == navigationBar)
		{
			LeaveRoomRequest request = new LeaveRoomRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			Lobby.GetInstance().IsMasterPlayer = false;
			Lobby.GetInstance().SetCurrentRoomID(-1);
			stateMgr.FrGoToPhase(9, false, false, false);
		}
		else if (control == startBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			StartGame();
		}
		else
		{
			if (control != listPlayer || command != 0)
			{
				return;
			}
			m_time = DateTime.Now;
			RoomPlayer[] array = null;
			Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
			if (currentRoom != null)
			{
				array = currentRoom.GetAllPlayers();
			}
			byte b = (byte)wparam;
			if (Lobby.GetInstance().AutoBalance == 1 && !IsSameTeam(Lobby.GetInstance().CurrentSeatID, b))
			{
				int num = GetPlayerNumInTeam(array, true) + ((!IsBlueTeam(Lobby.GetInstance().CurrentSeatID)) ? 1 : (-1));
				int num2 = GetPlayerNumInTeam(array, false) + (IsBlueTeam(Lobby.GetInstance().CurrentSeatID) ? 1 : (-1));
				if (Mathf.Abs(num - num2) > 1)
				{
					return;
				}
			}
			if (array != null && b < array.Length && array[b] == null)
			{
				ChangeSeatRequest request2 = new ChangeSeatRequest((byte)wparam);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				netLoadingUI.Show(5);
			}
		}
	}
}
