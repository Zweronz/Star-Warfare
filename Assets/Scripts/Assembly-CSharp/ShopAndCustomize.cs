using System;
using UnityEngine;

public class ShopAndCustomize : UIStateManager, UIHandler
{
	private float startTime;

	private CustomizeUI customizeUI;

	private StoreUI storeUI;

	private MakePackageUI makePackageUI;

	private PropsStoreUI propsStoreUI;

	private OptionsUI optionsUI;

	private LoadingUI loadingUI;

	private ExtraUI extraUI;

	private UIUtil util;

	protected Timer fadeTimer = new Timer();

	private byte LoadingState;

	private static byte LOADING_SUBSTATE_RES;

	private static byte LOADING_SUBSTATE_AVATAR = 1;

	private void Awake()
	{
		util = new UIUtil();
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
	}

	private void InitUIManager()
	{
		Res2DManager.GetInstance().Init(22);
		m_UIManager = base.gameObject.AddComponent("UIManager") as UIManager;
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

	protected override void FrInit(int phase)
	{
		switch (phase)
		{
		case 0:
			Res2DManager.GetInstance().SetResUI(14);
			Res2DManager.GetInstance().SetResUI(19);
			Res2DManager.GetInstance().SetResUI(12);
			Res2DManager.GetInstance().SetResUI(10);
			Res2DManager.GetInstance().SetResUI(11);
			Res2DManager.GetInstance().SetResUI(18);
			Res2DManager.GetInstance().SetResUI(13);
			Res2DManager.GetInstance().SetResUI(20);
			Res2DManager.GetInstance().SetResUI(7);
			Res2DManager.GetInstance().SetResUI(8);
			Res2DManager.GetInstance().SetResUI(17);
			Res2DManager.GetInstance().SetResUI(21);
			StartLoading(1, -1);
			break;
		case 1:
			loadingUI.Init();
			break;
		case 3:
			customizeUI.Init();
			break;
		case 4:
			storeUI.Init();
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
		case 9:
			break;
		}
	}

	public CustomizeUI GetCustomizeUI()
	{
		return customizeUI;
	}

	public AdsUI GetAds()
	{
		if (FrGetCurrentPhase() == 3)
		{
			if (customizeUI != null)
			{
				return customizeUI.GetAds();
			}
		}
		else if (FrGetCurrentPhase() == 4 && storeUI != null)
		{
			return storeUI.GetAds();
		}
		return null;
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
		case 3:
			customizeUI.Close();
			break;
		case 4:
			storeUI.Close();
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
		case 9:
			break;
		}
	}

	protected override void FrUpdate(int phase)
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null && !networkManager.IsDisconnected && networkManager.IsConnected())
			{
				networkManager.ProcessReceivedPackets();
				TimeManager.GetInstance().Loop();
			}
		}
		switch (phase)
		{
		case 0:
			FrGoToPhase(UIConstant.GetInitPhase(), false, false, false);
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
			else if (LoadingState == LOADING_SUBSTATE_AVATAR)
			{
				util.loadNoBreak = true;
				if (loadingUI.FadeOutComplete())
				{
					FrGoToPhase(FrGetPreviousPhase(), true, false, false);
				}
			}
			break;
		case 3:
			customizeUI.Update();
			break;
		case 4:
			storeUI.Update();
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
		case 9:
			break;
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void LoadAllAvatar(int type, bool active, bool isShop)
	{
		util.LoadAllAvatar(type, active, isShop);
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public void ShowMsgTwitterPosted()
	{
		if (FrGetCurrentPhase() == 3)
		{
			if (customizeUI != null)
			{
				customizeUI.ShowMsgTwitterPosted();
			}
		}
		else if (FrGetCurrentPhase() == 4 && storeUI != null)
		{
			storeUI.ShowMsgTwitterPosted();
		}
	}

	public void ShowMsgFacebookPosted()
	{
		if (FrGetCurrentPhase() == 3)
		{
			if (customizeUI != null)
			{
				customizeUI.ShowMsgFacebookPosted();
			}
		}
		else if (FrGetCurrentPhase() == 4 && storeUI != null)
		{
			storeUI.ShowMsgFacebookPosted();
		}
	}

	public override void FrFree()
	{
		Res2DManager.GetInstance().FreeUI(12, true);
		Res2DManager.GetInstance().FreeUI(10, true);
		Res2DManager.GetInstance().FreeUI(11, true);
		Res2DManager.GetInstance().FreeUI(18, true);
		Res2DManager.GetInstance().FreeUI(13, true);
		Res2DManager.GetInstance().FreeUI(20, true);
		Res2DManager.GetInstance().FreeUI(28, true);
		util.FreeAllAvatar();
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
}
