using System;
using UnityEngine;

public class StartMenuScript : UIStateManager, UIHandler
{
	private float startTime;

	public NetworkManager networkMgr;

	private CustomizeUI customizeUI;

	private MainMenuUI mainMenuUI;

	private StoreUI storeUI;

	private MakePackageUI makePackageUI;

	private PropsStoreUI propsStoreUI;

	private OptionsUI optionsUI;

	private ExtraUI extraUI;

	private LoadingUI loadingUI;

	protected Timer fadeTimer = new Timer();

	private UserData playerLoginData;

	private byte LoadingState;

	private static byte LOADING_SUBSTATE_RES;

	private static byte LOADING_SUBSTATE_USERSTATE = 1;

	private static byte LOADING_SUBSTATE_AVATAR = 2;

	private bool bGiveRewards;

	public static bool bFirstLaunch = true;

	private GameObject sceneObj;

	public bool bConnection;

	public byte loadResult;

	private void Awake()
	{
		mainMenuUI = new MainMenuUI(this);
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
		bConnection = false;
		UIConstant.InitScreenInfo();
		InitUIManager();
		UIConstant.InitMessage();
		LoadingState = LOADING_SUBSTATE_RES;
		FrGoToPhase(0, false, true, true);
		TimeManager.GetInstance().Init();
		TimeManager.GetInstance().setMaxLoopTimes(-1);
		TimeManager.GetInstance().setPeriod(3f);
		GameObject gameObject = GameObject.Find("MenuMusic");
		Camera.main.transform.position = new Vector3(-0.5f, 1f, 0f);
		if (gameObject == null)
		{
			GameObject original = Resources.Load("Audio/MenuMusic") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, new Vector3(-0.5f, 1f, 0f), Quaternion.identity) as GameObject;
			gameObject.name = "MenuMusic";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	private void InitUIManager()
	{
		Res2DManager.GetInstance().Init(22);
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(24, 1, false);
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

	private void InitUserState()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (!userState.bInit)
		{
			loadResult = GameApp.GetInstance().Load();
			if (loadResult != 0)
			{
				userState.Init();
			}
			userState.InitProps();
			userState.InitArmorRewards();
			userState.InitRank();
			userState.UnLockEquip();
			userState.InitCompletedLevel();
			userState.Achievement.GotNewAvatar(userState.OwnedSuitCount());
			userState.bInit = true;
			GameApp.GetInstance().StartHttpRequestThread();
			LoginBackground();
			if (userState.GetRewardStatus() == 1)
			{
				userState.SetRewardStatus(2);
				userState.AddMithril(8);
				GameApp.GetInstance().Save();
			}
		}
	}

	public override void FrFree()
	{
		Res2DManager.GetInstance().FreeUI(1, true);
		Res2DManager.GetInstance().FreeUI(2, true);
		Res2DManager.GetInstance().FreeUI(24, true);
		Res2DManager.GetInstance().FreeUI(8, true);
		Res2DManager.GetInstance().FreeUI(7, true);
		Res2DManager.GetInstance().FreeUI(17, true);
		Res2DManager.GetInstance().FreeUI(21, true);
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

	protected override void FrInit(int phase)
	{
		switch (phase)
		{
		case 0:
			Res2DManager.GetInstance().SetResUI(1);
			Res2DManager.GetInstance().SetResUI(2);
			Res2DManager.GetInstance().SetResUI(24);
			Res2DManager.GetInstance().SetResUI(14);
			Res2DManager.GetInstance().SetResUI(19);
			Res2DManager.GetInstance().SetResUI(7);
			Res2DManager.GetInstance().SetResUI(8);
			Res2DManager.GetInstance().SetResUI(17);
			Res2DManager.GetInstance().SetResUI(21);
			Res2DManager.GetInstance().SetResUI(23);
			Res2DManager.GetInstance().SetResData(13);
			Res2DManager.GetInstance().SetResData(14);
			Res2DManager.GetInstance().SetResData(15);
			Res2DManager.GetInstance().SetResData(73);
			Res2DManager.GetInstance().SetResData(74);
			Res2DManager.GetInstance().SetResData(75);
			Res2DManager.GetInstance().SetResData(17);
			Res2DManager.GetInstance().SetResData(16);
			Res2DManager.GetInstance().SetResData(18);
			StartLoading(0, 100);
			break;
		case 1:
			loadingUI.Init();
			break;
		case 2:
			mainMenuUI.Init();
			break;
		case 3:
			UIConstant.GotoShopAndCustomize(0, 3);
			FrFree();
			Application.LoadLevel("ShopAndCustomize");
			break;
		case 4:
			UIConstant.GotoShopAndCustomize(0, 4);
			FrFree();
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
		case 6:
		case 7:
		case 9:
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
		case 2:
			mainMenuUI.Close();
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
		case 6:
		case 7:
		case 9:
			break;
		}
	}

	protected override void FrUpdate(int phase)
	{
		UpdateNetwork();
		if (bConnection)
		{
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			if (networkMgr != null)
			{
				if (networkMgr.conn != null)
				{
					TimeManager.GetInstance().Loop();
				}
				else
				{
					bConnection = false;
				}
			}
			else
			{
				bConnection = false;
			}
		}
		switch (phase)
		{
		case 0:
			FrGoToPhase(2, false, false, false);
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
					LoadingState = LOADING_SUBSTATE_USERSTATE;
				}
			}
			else if (LoadingState == LOADING_SUBSTATE_USERSTATE)
			{
				if (!GameApp.GetInstance().UIInit)
				{
					GameObject original = Resources.Load("NGUI/GameUIManager") as GameObject;
					UnityEngine.Object.Instantiate(original);
					GameApp.GetInstance().UIInit = true;
				}
				InitUserState();
				if (bFirstLaunch)
				{
					GameObject gameObject = GameObject.Find("TimeClock");
					if (gameObject != null)
					{
						TimeScript component = gameObject.GetComponent<TimeScript>();
						if (component != null)
						{
							component.GetPromotion();
						}
					}
				}
				LoadingState = LOADING_SUBSTATE_AVATAR;
			}
			else if (LoadingState == LOADING_SUBSTATE_AVATAR && loadingUI.FadeOutComplete())
			{
				FrGoToPhase(FrGetPreviousPhase(), true, false, false);
			}
			break;
		case 2:
			mainMenuUI.Update();
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
		case 6:
		case 7:
		case 9:
			break;
		}
	}

	private void UpdateNetwork()
	{
		networkMgr = GameApp.GetInstance().GetNetworkManager();
		if (networkMgr != null)
		{
			networkMgr.ProcessReceivedPackets();
		}
	}

	public void UpLoadData()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		UploadDataRequest request = new UploadDataRequest(userState);
		networkMgr.SendRequest(request);
	}

	public void PopupGift()
	{
		if (mainMenuUI != null)
		{
			if (bGiveRewards)
			{
				mainMenuUI.ShowGift();
			}
			else
			{
				UpLoadDataLogic();
			}
		}
	}

	public void UpLoadDataLogic()
	{
		if (playerLoginData.saveNum > GameApp.GetInstance().GetUserState().GetSaveNum())
		{
			ShowQueryUpload();
			return;
		}
		UpLoadData();
		ClearUserData();
		GotoMultiMenu();
	}

	public void SetGiveRewards(bool bGive)
	{
		bGiveRewards = bGive;
	}

	public void GotoMultiMenu()
	{
		FrFree();
		Application.LoadLevel("MultiMenu");
	}

	public void ShowQueryUpload()
	{
		if (mainMenuUI != null)
		{
			mainMenuUI.IsUploadData();
		}
	}

	public void CorrectLogin()
	{
		if (mainMenuUI != null)
		{
			mainMenuUI.HideNetLoading();
			bConnection = true;
		}
	}

	public UserData CreateUserData()
	{
		if (playerLoginData == null)
		{
			playerLoginData = new UserData();
		}
		return playerLoginData;
	}

	public UserData GetUserData()
	{
		return playerLoginData;
	}

	public void ClearUserData()
	{
		playerLoginData = null;
	}

	public void PopupServerMessage(byte textIndex, byte eventID)
	{
		if (mainMenuUI != null)
		{
			mainMenuUI.PopupServerMessage(textIndex, eventID);
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public AdsUI GetAds()
	{
		if (mainMenuUI != null)
		{
			return mainMenuUI.GetAds();
		}
		return null;
	}

	private void LoginBackground()
	{
		long lastLoginTicks = GameApp.GetInstance().GetUserState().GetLastLoginTicks();
		long nextLoginInterval = GameApp.GetInstance().GetUserState().GetNextLoginInterval();
		long ticks = DateTime.Now.Ticks;
		if (ticks - lastLoginTicks > nextLoginInterval)
		{
			SwBackgroundLoginRequest request = new SwBackgroundLoginRequest();
			GameApp.GetInstance().GetWWWManager().SendRequest(request);
		}
	}
}
