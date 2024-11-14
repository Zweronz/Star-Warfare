using System;
using UnityEngine;

public class InGameUIScript : UIStateManager, UIHandler
{
	public static bool bInited;

	public NetworkManager networkMgr;

	private LoadingUI loadingUI;

	private BattleHUD battleHUD;

	private StatisticsBaseUI statisticsUI;

	private NetStatisticsUIBase netStatisticsUI;

	private PauseUI pauseUI;

	private OptionsInGameUI optionsInGameUI;

	private int dataIndex;

	private byte LoadingState;

	private static byte LOADING_SUBSTATE_RES;

	private static byte LOADING_SUBSTATE_MAINSCRIPT = 1;

	private static byte LOADING_SUBSTATE_START = 2;

	private MessageBoxUI msgError;

	private void Awake()
	{
		bInited = false;
		loadingUI = new LoadingUI(this);
		CreateSoloStatistics();
		pauseUI = new PauseUI(this);
		optionsInGameUI = new OptionsInGameUI(this);
		CreateBattleHUD();
		CreateNetStatistics();
	}

	private void Start()
	{
		InitUIManager();
		LoadingState = LOADING_SUBSTATE_RES;
		FrGoToPhase(0, false, true, true);
	}

	private void CreateSoloStatistics()
	{
		Mode modePlay = GameApp.GetInstance().GetGameMode().ModePlay;
		if (modePlay == Mode.Boss)
		{
			statisticsUI = new StatisticsUIForBoss(this);
		}
		else
		{
			statisticsUI = new StatisticsUI(this);
		}
	}

	private void CreateNetStatistics()
	{
		switch (GameApp.GetInstance().GetGameMode().ModePlay)
		{
		case Mode.Survival:
			netStatisticsUI = new NetStatisticsUIForSurvival(this);
			break;
		case Mode.Boss:
			netStatisticsUI = new NetStatisticsUIForBoss(this);
			break;
		case Mode.VS_FFA:
			netStatisticsUI = new NetStatisticsUIForFFA(this);
			break;
		case Mode.VS_TDM:
			netStatisticsUI = new NetStatisticsUIForTDM(this);
			break;
		case Mode.VS_CTF_TDM:
			netStatisticsUI = new NetStatisticsUIForCTFTDM(this);
			break;
		case Mode.VS_CTF_FFA:
			netStatisticsUI = new NetStatisticsUIForCTFFFA(this);
			break;
		case Mode.VS_VIP:
			netStatisticsUI = new NetStatisticsUIForVIP(this);
			break;
		case Mode.VS_CMI:
			netStatisticsUI = new NetStatisticsUIForCMI(this);
			break;
		default:
			netStatisticsUI = new NetStatisticsUIForSurvival(this);
			break;
		}
	}

	private void CreateBattleHUD()
	{
		switch (GameApp.GetInstance().GetGameMode().ModePlay)
		{
		case Mode.CamPain:
		case Mode.Survival:
		case Mode.Boss:
			battleHUD = new CoopBattleHUD(this);
			break;
		case Mode.VS_FFA:
			battleHUD = new FFAVersusBattleHUD(this);
			break;
		case Mode.VS_TDM:
			battleHUD = new TDMVersusBattleHUD(this);
			break;
		case Mode.VS_CTF_TDM:
			battleHUD = new CTFVersusBattleHUD(this);
			break;
		case Mode.VS_CTF_FFA:
			battleHUD = new LLVersusBattleHUD(this);
			break;
		case Mode.TowerDefence:
			battleHUD = new TowerDefenceBattleHUD(this);
			break;
		case Mode.VS_VIP:
			battleHUD = new VIPVersusBattleHUD(this);
			break;
		case Mode.VS_CMI:
			battleHUD = new CMIVersusBattleHUD(this);
			break;
		default:
			battleHUD = new CoopBattleHUD(this);
			break;
		}
	}

	private void InitUIManager()
	{
		Res2DManager.GetInstance().Init(22);
		m_UIManager = base.gameObject.AddComponent("UIManager") as UIManager;
		m_UIManager.SetParameter(24, 2, false);
		m_UIManager.SetUIHandler(this);
		m_UIPopupManager = GetPopup();
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, 0);
		uIImage.Rect = uIImage.GetObjectRect();
		uIImage.SetColor(Color.black);
		uIImage.SetSize(new Vector2(Screen.width, Screen.height));
		m_UIManager.Add(uIImage);
	}

	protected override void FrInit(int phase)
	{
		switch (phase)
		{
		case 0:
			Res2DManager.GetInstance().SetResUI(0);
			Res2DManager.GetInstance().SetResUI(15);
			Res2DManager.GetInstance().SetResUI(17);
			Res2DManager.GetInstance().SetResUI(9);
			Res2DManager.GetInstance().SetResUI(23);
			Res2DManager.GetInstance().SetResUI(27);
			Res2DManager.GetInstance().SetResUI(19);
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				if (GameApp.GetInstance().GetUserState().GetStage() >= Global.TOTAL_STAGE)
				{
					dataIndex = Global.TOTAL_STAGE * Global.TOTAL_SUB_STAGE + (GameApp.GetInstance().GetUserState().GetStage() - Global.TOTAL_STAGE);
				}
				else
				{
					dataIndex = GameApp.GetInstance().GetUserState().GetStage() * Global.TOTAL_SUB_STAGE + GameApp.GetInstance().GetUserState().GetSubStage();
				}
			}
			else if (GameApp.GetInstance().GetUserState().GetNetStage() >= Global.TOTAL_STAGE)
			{
				dataIndex = Global.TOTAL_STAGE * Global.TOTAL_SUB_STAGE + GameApp.GetInstance().GetUserState().GetNetStage() - Global.TOTAL_STAGE;
			}
			else
			{
				dataIndex = GameApp.GetInstance().GetUserState().GetNetStage() * Global.TOTAL_SUB_STAGE + (Global.TOTAL_SUB_STAGE - 1);
			}
			Res2DManager.GetInstance().SetResData(19 + dataIndex);
			Res2DManager.GetInstance().SetResData(0);
			Res2DManager.GetInstance().SetResData(1, 12);
			StartLoading(0, 100);
			SetIPadScreen();
			break;
		case 1:
			loadingUI.Init();
			break;
		case 6:
			SetIPadScreen();
			battleHUD.Init();
			break;
		case 7:
			UIConstant.InitScreenInfo();
			statisticsUI.Init();
			break;
		case 16:
			UIConstant.InitScreenInfo();
			netStatisticsUI.Init();
			break;
		case 14:
			pauseUI.Init();
			break;
		case 15:
			optionsInGameUI.Init();
			break;
		}
	}

	protected override void FrClose(int phase)
	{
		switch (phase)
		{
		case 0:
			base.transform.position = Vector3.up * 10000f;
			break;
		case 1:
			loadingUI.Close();
			break;
		case 6:
			battleHUD.Close();
			UIConstant.InitScreenInfo();
			break;
		case 7:
			statisticsUI.Close();
			break;
		case 16:
			netStatisticsUI.Close();
			break;
		case 14:
			pauseUI.Close();
			if (FrGetNextPhase() == 6)
			{
				GotoBattleUI();
			}
			break;
		case 15:
			optionsInGameUI.Close();
			if (FrGetNextPhase() == 6)
			{
				GotoBattleUI();
			}
			break;
		}
	}

	protected override void FrUpdate(int phase)
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && phase != 16 && UpdateNetwork())
		{
			return;
		}
		switch (phase)
		{
		case 0:
			FrGoToPhase(6, false, false, false);
			break;
		case 1:
			loadingUI.Update();
			if (FrGetPreviousPhase() != 0)
			{
				break;
			}
			if (LoadingState == LOADING_SUBSTATE_RES)
			{
				if (Res2DManager.GetInstance().LoadResPro())
				{
					LoadingState = LOADING_SUBSTATE_MAINSCRIPT;
				}
			}
			else if (LoadingState == LOADING_SUBSTATE_MAINSCRIPT)
			{
				MainScript component = GameObject.Find("Game").GetComponent<MainScript>();
				component.Init();
				LoadingState = LOADING_SUBSTATE_START;
			}
			else if (LoadingState == LOADING_SUBSTATE_START && loadingUI.FadeOutComplete())
			{
				bInited = true;
				FadeAnimationScript.GetInstance().FadeOutBlack();
				FrGoToPhase(FrGetPreviousPhase(), true, false, false);
			}
			break;
		case 6:
			battleHUD.Update();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				if (GameApp.GetInstance().GetGameWorld().State == GameState.GameOverUIWin)
				{
					if (GameApp.GetInstance().GetGameMode().IsCoopMode())
					{
						LeaveRoomRequest request = new LeaveRoomRequest();
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						Lobby.GetInstance().IsMasterPlayer = false;
					}
					GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
					FrGoToPhase(16, false, false, false);
				}
				else if (GameApp.GetInstance().GetGameWorld().State == GameState.GameOverUILose)
				{
					if (GameApp.GetInstance().GetGameMode().IsCoopMode())
					{
						LeaveRoomRequest request2 = new LeaveRoomRequest();
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						Lobby.GetInstance().IsMasterPlayer = false;
					}
					GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
					FrGoToPhase(16, false, false, false);
				}
			}
			else if (GameApp.GetInstance().GetGameWorld().State == GameState.GameOverUIWin)
			{
				GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
				FrGoToPhase(7, false, false, false);
			}
			else if (GameApp.GetInstance().GetGameWorld().State == GameState.GameOverUILose)
			{
				GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
				FrGoToPhase(7, false, false, false);
			}
			break;
		case 7:
			statisticsUI.Update();
			break;
		case 16:
			netStatisticsUI.Update();
			break;
		case 14:
			pauseUI.Update();
			break;
		case 15:
			optionsInGameUI.Update();
			break;
		}
	}

	public void RestartGame()
	{
		FrGoToPhase(6, false, false, false);
	}

	private void SetIPadScreen()
	{
	}

	private bool UpdateNetwork()
	{
		networkMgr = GameApp.GetInstance().GetNetworkManager();
		if (networkMgr != null)
		{
			if ((networkMgr.IsDisconnected || !networkMgr.IsConnected()) && msgError == null)
			{
				FadeAnimationScript instance = FadeAnimationScript.GetInstance();
				if (instance != null)
				{
					instance.FadeOutBlack();
				}
				msgError = new MessageBoxUI(this);
				msgError.Create();
				msgError.CreateConfirm(UIConstant.GetMessage(8), MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_LOST_CONNECTION);
				msgError.Show();
				m_UIPopupManager.Add(msgError);
				m_UIPopupManager.SetUIHandler(this);
				return true;
			}
			if (networkMgr.IsDisconnected || networkMgr.IsDisplayErrorBox)
			{
				return true;
			}
		}
		return false;
	}

	public void GotoBattleUI()
	{
		m_UIManager.SetEnable(true);
		m_UIManager.SetUIHandler(battleHUD);
		m_UIPopupManager.SetUIHandler(battleHUD);
	}

	public void ShowMsgTwitterPosted()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (FrGetCurrentPhase() == 16 && netStatisticsUI != null)
			{
				netStatisticsUI.ShowMsgTwitterPosted();
			}
		}
		else if (FrGetCurrentPhase() == 16 && statisticsUI != null)
		{
			statisticsUI.ShowMsgTwitterPosted();
		}
	}

	public void ShowMsgFacebookPosted()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (FrGetCurrentPhase() == 16 && netStatisticsUI != null)
			{
				netStatisticsUI.ShowMsgFacebookPosted();
			}
		}
		else if (FrGetCurrentPhase() == 16 && statisticsUI != null)
		{
			statisticsUI.ShowMsgFacebookPosted();
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control != msgError)
		{
			return;
		}
		int eventID = msgError.GetEventID();
		if (eventID == MessageBoxUI.EVENT_LOST_CONNECTION && command == 9)
		{
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			if (networkMgr != null)
			{
				networkMgr.CloseConnection();
				networkMgr.IsDisplayErrorBox = false;
			}
			msgError.Hide();
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			if (gameWorld != null)
			{
				gameWorld.Exit = true;
			}
			Lobby.GetInstance().IsMasterPlayer = false;
			GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
			FrGoToPhase(16, false, false, false);
		}
	}

	public override void FrFree()
	{
		Res2DManager.GetInstance().FreeUI(0, true);
		Res2DManager.GetInstance().FreeUI(15, true);
		Res2DManager.GetInstance().FreeUI(17, true);
		Res2DManager.GetInstance().FreeUI(9, true);
		Res2DManager.GetInstance().FreeUI(27, true);
		Res2DManager.GetInstance().FreeDataTable(dataIndex);
		Res2DManager.GetInstance().FreeDataTable(0);
		for (int i = 1; i <= 12; i++)
		{
			Res2DManager.GetInstance().FreeDataTable(i);
		}
		m_UIManager.Destory();
		m_UIManager.RemoveAll();
		m_UIPopupManager.Destory();
		m_UIPopupManager.RemoveAll();
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, 0);
		uIImage.Rect = uIImage.GetObjectRect();
		uIImage.SetColor(Color.black);
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width);
			num2 *= 1.5f;
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth + num2, UIConstant.ScreenLocalHeight));
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height);
			num3 *= 1.5f;
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight + num3));
		}
		else
		{
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		}
		m_UIManager.Add(uIImage);
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public void AddWhoKillsWho(int killerID, HUDAction action, int killedID)
	{
		if (battleHUD != null)
		{
			battleHUD.AddWhoKillsWho(killerID, action, killedID);
			UserStateUI.GetInstance().PushKillInfo(killerID, action, killedID);
		}
	}

	public void VSGameAutoBalance()
	{
		if (battleHUD != null)
		{
			UserStateUI.GetInstance().GetWaitVSRebirth().DoAutoBalance();
		}
	}

	public void WaitVSRebirthStart()
	{
		if (battleHUD != null)
		{
			VersusBattleHUD versusBattleHUD = battleHUD as VersusBattleHUD;
			if (versusBattleHUD != null)
			{
				versusBattleHUD.WaitVSRebirthStart();
			}
			UserStateUI.GetInstance().GetWaitVSRebirth().WaitVSRebirthStart();
		}
	}

	public void WaitVSRebirthEnd()
	{
		if (battleHUD != null)
		{
			VersusBattleHUD versusBattleHUD = battleHUD as VersusBattleHUD;
			if (versusBattleHUD != null)
			{
				versusBattleHUD.WaitVSRebirthEnd();
			}
			UserStateUI.GetInstance().GetWaitVSRebirth().WaitVSRebirthEnd();
		}
	}

	public void OnVSRebirth()
	{
		if (battleHUD != null)
		{
			VersusBattleHUD versusBattleHUD = battleHUD as VersusBattleHUD;
			if (versusBattleHUD != null)
			{
				versusBattleHUD.OnVSRebirth();
			}
			UserStateUI.GetInstance().GetWaitVSRebirth().OnVSRebirth();
		}
	}
}
