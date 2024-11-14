using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiMenuScript : UIStateManager, UIHandler
{
	private float startTime;

	public NetworkManager networkMgr;

	private MultiPlayerUI multiPlayerUI;

	private ReadyGameMasterUI readyGameUI;

	private CustomizeUI customizeUI;

	private StoreUI storeUI;

	private MakePackageUI makePackageUI;

	private OptionsUI optionsUI;

	private LoadingUI loadingUI;

	private PropsStoreUI propsStoreUI;

	private ExtraUI extraUI;

	protected Timer fadeTimer = new Timer();

	private byte LoadingState;

	private static byte LOADING_SUBSTATE_RES;

	private static byte LOADING_SUBSTATE_AVATAR = 1;

	private MessageBoxUI msgError;

	public GUIStyle playerNameStyle;

	public GUIStyle searchRoomStyle;

	private void Awake()
	{
		multiPlayerUI = new MultiPlayerUI(this);
		readyGameUI = new ReadyGameMasterUI(this);
		customizeUI = new CustomizeUI(this);
		storeUI = new StoreUI(this);
		makePackageUI = new MakePackageUI(this);
		propsStoreUI = new PropsStoreUI(this);
		optionsUI = new OptionsUI(this);
		loadingUI = new LoadingUI(this);
		extraUI = new ExtraUI(this);
	}

	private void Start()
	{
		GameApp.GetInstance().GetGameMode().TypeOfNetwork = NetworkType.MultiPlayer_Internet;
		GameApp.GetInstance().GetGameMode().ModePlay = Mode.VS_TDM;
		GameApp.GetInstance().ClearGameWorld();
		InitUIManager();
		LoadingState = LOADING_SUBSTATE_RES;
		FrGoToPhase(0, false, true, true);
		GameObject gameObject = GameObject.Find("MenuMusic");
		Camera.mainCamera.transform.position = new Vector3(-0.5f, 1f, 0f);
		if (gameObject == null)
		{
			GameObject original = Resources.Load("Audio/MenuMusic") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, new Vector3(-0.5f, 1f, 0f), Quaternion.identity) as GameObject;
			gameObject.name = "MenuMusic";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		TimeManager.GetInstance().Init();
		TimeManager.GetInstance().setMaxLoopTimes(-1);
		TimeManager.GetInstance().setPeriod(1f);
	}

	private void InitUIManager()
	{
		Res2DManager.GetInstance().Init(22);
		Res2DManager.GetInstance().LoadImmediately(23);
		m_UIManager = base.gameObject.AddComponent("UIManager") as UIManager;
		m_UIManager.SetParameter(24, 1, false);
		m_UIManager.SetUIHandler(this);
		m_UIPopupManager = GetPopup();
		m_UIPopupManager.SetParameter(24, 3, false);
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
			Res2DManager.GetInstance().SetResUI(14);
			Res2DManager.GetInstance().SetResUI(19);
			Res2DManager.GetInstance().SetResUI(4);
			Res2DManager.GetInstance().SetResUI(5);
			Res2DManager.GetInstance().SetResUI(6);
			Res2DManager.GetInstance().SetResUI(7);
			Res2DManager.GetInstance().SetResUI(8);
			Res2DManager.GetInstance().SetResUI(17);
			Res2DManager.GetInstance().SetResUI(21);
			Res2DManager.GetInstance().SetResUI(27);
			Res2DManager.GetInstance().SetResUI(23);
			StartLoading(0, 100);
			break;
		case 1:
			loadingUI.Init();
			break;
		case 9:
			multiPlayerUI.Init();
			break;
		case 12:
			readyGameUI.Init();
			break;
		case 3:
			UIConstant.GotoShopAndCustomize(2, 3);
			FrFreeGotoShop();
			Application.LoadLevel("ShopAndCustomize");
			break;
		case 4:
			UIConstant.GotoShopAndCustomize(2, 4);
			FrFreeGotoShop();
			Application.LoadLevel("ShopAndCustomize");
			break;
		case 5:
			makePackageUI.Init();
			break;
		case 10:
			propsStoreUI.Init();
			break;
		case 8:
			optionsUI.Init();
			break;
		case 11:
			extraUI.Init();
			break;
		case 2:
		case 6:
		case 7:
			break;
		}
	}

	protected override void FrClose(int phase)
	{
		switch (phase)
		{
		case 0:
			break;
		case 1:
			loadingUI.Close();
			break;
		case 9:
			multiPlayerUI.Close();
			break;
		case 12:
			readyGameUI.Close();
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			makePackageUI.Close();
			break;
		case 10:
			propsStoreUI.Close();
			break;
		case 8:
			optionsUI.Close();
			break;
		case 11:
			extraUI.Close();
			break;
		case 2:
		case 6:
		case 7:
			break;
		}
	}

	protected override void FrUpdate(int phase)
	{
		if (IAPUI.IapProcessing == IAPName.None && UpdateNetwork())
		{
			return;
		}
		TimeManager.GetInstance().Loop();
		switch (phase)
		{
		case 0:
			FrGoToPhase(9, false, false, false);
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
					LoadingState = LOADING_SUBSTATE_AVATAR;
				}
			}
			else if (LoadingState == LOADING_SUBSTATE_AVATAR && loadingUI.FadeOutComplete())
			{
				FrGoToPhase(FrGetPreviousPhase(), true, false, false);
			}
			break;
		case 9:
			multiPlayerUI.Update();
			break;
		case 12:
			readyGameUI.Update();
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			makePackageUI.Update();
			break;
		case 10:
			propsStoreUI.Update();
			break;
		case 8:
			optionsUI.Update();
			break;
		case 11:
			extraUI.Update();
			break;
		case 2:
		case 6:
		case 7:
			break;
		}
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
			networkMgr.ProcessReceivedPackets();
			return false;
		}
		return false;
	}

	public void OnGUI()
	{
		if (msgError == null || !msgError.Visible)
		{
			if (FrGetCurrentPhase() == 9 && multiPlayerUI != null)
			{
				multiPlayerUI.OnGUI();
			}
			if (FrGetCurrentPhase() == 12 && readyGameUI != null)
			{
				readyGameUI.OnGUI();
			}
		}
	}

	public void GotoReadyGame()
	{
		HideNetLoading();
		FrGoToPhase(12, false, false, false);
	}

	public void HideNetLoading()
	{
		if (multiPlayerUI != null)
		{
			multiPlayerUI.HideNetLoading();
		}
	}

	public void StartGame(int mapId)
	{
		FrFree();
		if (mapId == Global.TOTAL_STAGE)
		{
			mapId = 2;
		}
		else if (mapId == Global.TOTAL_STAGE + 1)
		{
			mapId = 4;
		}
		else if (mapId == Global.TOTAL_STAGE + 2)
		{
			mapId = 3;
		}
		else if (mapId == Global.TOTAL_STAGE + 3)
		{
			mapId = 5;
		}
		else if (mapId == Global.TOTAL_STAGE + 4)
		{
			mapId = 6;
		}
		else if (mapId == Global.TOTAL_STAGE + 5)
		{
			mapId = 7;
			Debug.Log("new stage: " + mapId);
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE)
		{
			mapId = 12;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 1)
		{
			mapId = 13;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 2)
		{
			mapId = 14;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 3)
		{
			mapId = 15;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 4)
		{
			mapId = 16;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 5)
		{
			mapId = 17;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 6)
		{
			mapId = 18;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 7)
		{
			mapId = 19;
		}
		else if (mapId == Global.TOTAL_STAGE + Global.TOTAL_BOSS_STAGE + 8)
		{
			mapId = 20;
		}
		GameObject gameObject = GameObject.Find("MenuMusic");
		if (gameObject != null)
		{
			UnityEngine.Object.DestroyObject(gameObject);
		}
		Application.LoadLevel("Level" + (mapId + 1));
	}

	public void SetRoomList(List<Room> roomList, byte recommendRoomNumber)
	{
		Lobby.GetInstance().SetupRoomList(roomList);
		if (FrGetCurrentPhase() == 9)
		{
			multiPlayerUI.HideNetLoading();
			multiPlayerUI.ResetUIRoom(recommendRoomNumber);
		}
	}

	public void SetChangeSeat()
	{
		if (FrGetCurrentPhase() == 12)
		{
			readyGameUI.HideNetLoading();
		}
	}

	public void SetRoomData(Room room)
	{
		Lobby.GetInstance().SetCurrentRoom(room);
		if (FrGetCurrentPhase() == 12)
		{
			readyGameUI.ResetUIPlayer();
			readyGameUI.HideNetLoading();
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
			NetworkManager networkManagerIAP = GameApp.GetInstance().GetNetworkManagerIAP();
			if (networkManagerIAP != null)
			{
				networkManagerIAP.CloseConnection();
				networkManagerIAP.IsDisplayErrorBox = false;
			}
			msgError.Hide();
			FrFree();
			Application.LoadLevel("StartMenu");
		}
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public override void FrFree()
	{
		Res2DManager.GetInstance().FreeUI(14, true);
		Res2DManager.GetInstance().FreeUI(4, true);
		Res2DManager.GetInstance().FreeUI(5, true);
		Res2DManager.GetInstance().FreeUI(6, true);
		Res2DManager.GetInstance().FreeUI(7, true);
		Res2DManager.GetInstance().FreeUI(8, true);
		Res2DManager.GetInstance().FreeUI(17, true);
		Res2DManager.GetInstance().FreeUI(21, true);
		Res2DManager.GetInstance().FreeUI(27, true);
		m_UIManager.Destory();
		m_UIManager.RemoveAll();
		if (m_UIPopupManager != null)
		{
			m_UIPopupManager.Destory();
			m_UIPopupManager.RemoveAll();
		}
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

	public void FrFreeGotoShop()
	{
		Res2DManager.GetInstance().FreeUI(4, true);
		Res2DManager.GetInstance().FreeUI(5, true);
		Res2DManager.GetInstance().FreeUI(3, true);
		Res2DManager.GetInstance().FreeUI(6, true);
		m_UIManager.Destory();
		m_UIManager.RemoveAll();
		if (m_UIPopupManager != null)
		{
			m_UIPopupManager.Destory();
			m_UIPopupManager.RemoveAll();
		}
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

	public void Logout()
	{
		networkMgr.CloseConnection();
	}
}
