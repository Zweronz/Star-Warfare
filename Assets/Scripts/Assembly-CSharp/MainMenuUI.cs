using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_TOUCH_TO_MENU = 2;

	private const byte STATE_ANIMA = 3;

	private const byte STATE_POP_NAV_MENU = 4;

	private const byte STATE_HANDLE = 5;

	public UIStateManager stateMgr;

	protected NetworkManager networkMgr;

	protected UserState userState;

	private byte state;

	private UIImage touchToMenu;

	private UIImage titleImg;

	private UIImage titleBGImg;

	private int titlePosY;

	private int navigationMenuPosY;

	private static byte[] SOLO_IMG = new byte[2] { 10, 12 };

	private static byte[] SOLO_IMG_1 = new byte[2] { 9, 11 };

	private static byte[] COOP_IMG = new byte[2] { 14, 16 };

	private static byte[] COOP_IMG_1 = new byte[2] { 13, 15 };

	private static byte[] GUARD_IMG = new byte[2] { 17, 18 };

	private static byte[] GUARD_IMG_1 = new byte[2] { 21, 22 };

	private static byte[] VS_IMG = new byte[2] { 25, 26 };

	private static byte[] VS_IMG_1 = new byte[2] { 29, 30 };

	private UIAnimation titleAnima;

	private UIImage bgBtn;

	private UIClickButton soloBtn;

	private UIClickButton coopBtn;

	private Vector2 soloOffset;

	private Vector2 coopOffset;

	private UIClickButton guardBtn;

	private UIClickButton vsBtn;

	private NavigationMenuUI navigationMenu;

	private MessageBoxUI msgUI;

	private NetLoadingUI netLoadingUI;

	private UIGift giftUI;

	private int count;

	private bool logining;

	private DateTime m_beginTime;

	private UIImage freeGotMithrilImg;

	private UIImage twitterImg;

	private UIImage facebookImg;

	private UIImage adsImg;

	private FrUIText versionTxt;

	private GameObject blink;

	private AdsUI adsui;

	private UIImage New;

	private int opentime;

	private int gtime;

	private bool show = true;

	private string[] usernameStringList = new string[62]
	{
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
		"a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
		"k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
		"u", "v", "w", "x", "y", "z", "A", "B", "C", "D",
		"E", "F", "G", "H", "I", "J", "K", "L", "M", "N",
		"O", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
		"Y", "Z"
	};

	public MainMenuUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		userState = GameApp.GetInstance().GetUserState();
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public void Close()
	{
		blink.SetActiveRecursively(false);
		UnityEngine.Object.Destroy(blink);
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		logining = false;
		count = 0;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[1];
		UnitUI ui = Res2DManager.GetInstance().vUI[2];
		touchToMenu = new UIImage();
		touchToMenu.AddObject(unitUI, 1, 0);
		touchToMenu.Rect = touchToMenu.GetObjectRect();
		titleAnima = new UIAnimation(stateMgr.m_UIManager);
		titleAnima.AddAnimation("title", ui);
		titleAnima.SetSize("title", 0.7f);
		titleAnima.Visible = false;
		titleImg = new UIImage();
		titleImg.AddObject(unitUI, 0, 0, 8);
		titleImg.Rect = titleImg.GetObjectRect();
		titleImg.SetSize(new Vector2(titleImg.Rect.width * 0.7f, titleImg.Rect.height * 0.7f));
		titleImg.Visible = false;
		titlePosY = (int)(titleImg.Rect.y + 150f);
		titleBGImg = new UIImage();
		titleBGImg.AddObject(unitUI, 2, 0);
		titleBGImg.Rect = titleBGImg.GetObjectRect();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 8);
		bgBtn = new UIImage();
		bgBtn.AddObject(unitUI, 0, 8);
		bgBtn.Rect = bgBtn.GetObjectRect();
		soloBtn = new UIClickButton();
		soloBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, SOLO_IMG);
		soloBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, SOLO_IMG_1);
		soloBtn.Rect = soloBtn.GetObjectRect(UIButtonBase.State.Normal);
		soloOffset.x = soloBtn.Rect.x - modulePositionRect.x;
		soloOffset.y = soloBtn.Rect.y - modulePositionRect.y;
		coopBtn = new UIClickButton();
		coopBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, COOP_IMG);
		coopBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, COOP_IMG_1);
		coopBtn.Rect = coopBtn.GetObjectRect(UIButtonBase.State.Normal);
		coopOffset.x = coopBtn.Rect.x - modulePositionRect.x;
		coopOffset.y = coopBtn.Rect.y - modulePositionRect.y;
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		navigationMenuPosY = (int)navigationMenu.Rect.y;
		navigationMenu.Rect = new Rect(0f, UIConstant.ScreenLocalHeight, UIConstant.ScreenLocalWidth, navigationMenu.Rect.height);
		stateMgr.m_UIManager.Add(titleBGImg);
		stateMgr.m_UIManager.Add(touchToMenu);
		stateMgr.m_UIManager.Add(titleImg);
		stateMgr.m_UIManager.Add(bgBtn);
		stateMgr.m_UIManager.Add(soloBtn);
		stateMgr.m_UIManager.Add(coopBtn);
		if (GameApp.GetInstance().openfreemithril)
		{
			freeGotMithrilImg = new UIImage();
			freeGotMithrilImg.AddObject(unitUI, 0, 17);
			freeGotMithrilImg.Rect = freeGotMithrilImg.GetObjectRect();
			freeGotMithrilImg.Visible = true;
			stateMgr.m_UIManager.Add(freeGotMithrilImg);
			New = new UIImage();
			New.AddObject(unitUI, 0, 20);
			New.Rect = New.GetObjectRect();
			New.Visible = true;
			stateMgr.m_UIManager.Add(New);
			if (GameApp.GetInstance().AppStatus == 1)
			{
				New.Visible = false;
			}
		}
		twitterImg = new UIImage();
		twitterImg.AddObject(unitUI, 0, 18);
		twitterImg.Rect = twitterImg.GetObjectRect();
		twitterImg.Visible = false;
		stateMgr.m_UIManager.Add(twitterImg);
		facebookImg = new UIImage();
		facebookImg.AddObject(unitUI, 0, 19);
		facebookImg.Rect = facebookImg.GetObjectRect();
		facebookImg.Visible = false;
		stateMgr.m_UIManager.Add(facebookImg);
		GameApp.GetInstance().SetGameStart();
		InitFreeGotMithril();
		versionTxt = new FrUIText();
		versionTxt.Rect = new Rect(10f, UIConstant.ScreenLocalHeight - 20f, 100f, 20f);
		versionTxt.Set("font3", "VER 2.97", UIConstant.fontColor_white, 100f);
		stateMgr.m_UIManager.Add(versionTxt);
		stateMgr.m_UIManager.Add(titleAnima);
		stateMgr.m_UIPopupManager.Add(navigationMenu);
		giftUI = new UIGift(stateMgr);
		giftUI.Create();
		giftUI.Hide();
		stateMgr.m_UIPopupManager.Add(giftUI);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		stateMgr.m_UIPopupManager.Add(msgUI);
		netLoadingUI = new NetLoadingUI(stateMgr);
		netLoadingUI.Create();
		netLoadingUI.Hide();
		stateMgr.m_UIPopupManager.Add(netLoadingUI);
		UnityEngine.Object original = Resources.Load("Effect/update_effect/effect_glow_001");
		blink = UnityEngine.Object.Instantiate(original, new Vector3(-1.18f, 1.4f, 2.5f), Quaternion.identity) as GameObject;
		blink.transform.Rotate(new Vector3(270f, 0f, 0f));
		blink.transform.localScale = new Vector3(1f, 1f, 1f);
		SetRoleName();
		if (userState.GetDiscountStatus() == 2)
		{
			DateTime value = DateTime.Parse(userState.GetDiscountTime());
			if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm")).Subtract(value).Days != 0)
			{
				ClearDisCount();
				GameApp.GetInstance().Save();
			}
		}
		if (userState.GetShowNotify() == 1 && userState.GetDiscountStatus() == 1)
		{
			Debug.Log("Go to Store");
			stateMgr.FrGoToPhase(4, false, false, false);
		}
	}

	public void SetRoleName()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		string roleName = userState.GetRoleName();
		if ("Player".Equals(roleName) || string.Empty.Equals(roleName))
		{
			AndroidSwPluginScript.SetRoleName(0);
		}
	}

	public void InitFreeGotMithril()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (GameApp.GetInstance().openfreemithril)
		{
			if (GameApp.GetInstance().AppStatus == 1)
			{
				freeGotMithrilImg.Visible = true;
				freeGotMithrilImg.Enable = true;
				New.Visible = true;
				New.Enable = true;
			}
			else
			{
				freeGotMithrilImg.Visible = false;
				freeGotMithrilImg.Enable = false;
				New.Visible = false;
				New.Enable = false;
			}
			if (GameApp.GetInstance().GetUserState().GetRewardStatus() == 2)
			{
				New.Visible = false;
				New.Enable = false;
			}
		}
		facebookImg.Visible = false;
		facebookImg.Enable = false;
		twitterImg.Visible = false;
		twitterImg.Enable = false;
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			Create();
			if (!StartMenuScript.bFirstLaunch)
			{
				state = 5;
				touchToMenu.Visible = false;
				titleImg.Visible = true;
				SetPosition(new Rect(0f, 0f, UIConstant.ScreenLocalWidth, 150f));
				navigationMenu.Rect = new Rect(navigationMenu.Rect.x, navigationMenuPosY, navigationMenu.Rect.width, navigationMenu.Rect.height);
				return false;
			}
			state = 2;
			SetPosition(new Rect(0f, -150f, UIConstant.ScreenLocalWidth, 150f));
			titleAnima.Rect = titleImg.Rect;
			titleAnima.SetPostion("title", titleImg.Rect);
			titleAnima.PlayAnimation("title");
			titleAnima.Visible = true;
			StartMenuScript.bFirstLaunch = false;
			break;
		case 2:
		{
			count++;
			if (count % 50 >= 25)
			{
				touchToMenu.Visible = false;
			}
			else
			{
				touchToMenu.Visible = true;
			}
			if (!titleAnima.IsOverPlayAnimation())
			{
				break;
			}
			UITouchInner[] array2 = iPhoneInputMgr.MockTouches();
			for (int j = 0; j < array2.Length; j++)
			{
				UITouchInner uITouchInner = array2[j];
				if (stateMgr.m_UIManager != null && uITouchInner.phase == TouchPhase.Ended)
				{
					state = 3;
					titleImg.Visible = true;
					touchToMenu.Visible = false;
					titleAnima.Visible = false;
					break;
				}
			}
			break;
		}
		case 3:
			if (UpdateAnima())
			{
				state = 4;
			}
			break;
		case 4:
			if (UpdateNavMenu())
			{
				state = 5;
				StartMenuScript startMenuScript = (StartMenuScript)stateMgr;
				if (startMenuScript.loadResult == 1)
				{
					string msg = UIConstant.GetMessage(43).Replace("[n]", "\n");
					msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_RECORD_ERROR);
					msgUI.Show();
				}
			}
			break;
		case 5:
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				msgUI.CreateQuery("Quit game?", MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_QUIT_GAME);
				msgUI.Show();
			}
			break;
		}
		}
		if (state != 0)
		{
			if (navigationMenu.IsWorkingIAP())
			{
				blink.SetActiveRecursively(false);
			}
			else
			{
				blink.SetActiveRecursively(true);
			}
		}
		return false;
	}

	public void ShowPromotion()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null && userState.showPromotion && !msgUI.IsVisiable())
		{
			msgUI.CreateQuery(userState.m_promotion.m_msg, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_STORE_PROMOTION);
			msgUI.Show();
			userState.showPromotion = false;
		}
	}

	public void ShowRewardMsg()
	{
		if (GameApp.GetInstance().GetUserState().showRewardMsg)
		{
			string msg = "Congratulations, you have earned " + GameApp.GetInstance().GetUserState().rewardNumber + " mithril from " + GameApp.GetInstance().GetUserState().rewardAdsName + ".";
			msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_MITHRIL, MessageBoxUI.EVENT_AD_REWARD);
			msgUI.Show();
			GameApp.GetInstance().GetUserState().showRewardMsg = false;
		}
	}

	public void ShowMovieMsg()
	{
		if (GameApp.GetInstance().GetUserState().showmovie)
		{
			string msg = "Watch video of our game!!";
			msgUI.CreateConfirmMovie(msg, MessageBoxUI.MESSAGE_FLAG_MOVIE, MessageBoxUI.EVENT_SHOW_MOVIE);
			msgUI.Show();
			GameApp.GetInstance().GetUserState().showmovie = false;
		}
	}

	private bool UpdateNavMenu()
	{
		if (navigationMenu.Rect.y > (float)navigationMenuPosY)
		{
			navigationMenu.Rect = new Rect(navigationMenu.Rect.x, navigationMenu.Rect.y - 20f, navigationMenu.Rect.width, navigationMenu.Rect.height);
		}
		if (navigationMenu.Rect.y <= (float)navigationMenuPosY)
		{
			navigationMenu.Rect = new Rect(navigationMenu.Rect.x, navigationMenuPosY, navigationMenu.Rect.width, navigationMenu.Rect.height);
			return true;
		}
		return false;
	}

	private bool UpdateAnima()
	{
		if (bgBtn.Rect.y < 0f)
		{
			SetPosition(new Rect(0f, bgBtn.Rect.y + 20f, UIConstant.ScreenLocalWidth, 150f));
		}
		if (bgBtn.Rect.y >= 0f)
		{
			SetPosition(new Rect(0f, 0f, UIConstant.ScreenLocalWidth, 150f));
			return true;
		}
		return false;
	}

	private bool UpdateBlink()
	{
		return false;
	}

	private void SetPosition(Rect pos)
	{
		bgBtn.Rect = pos;
		soloBtn.Rect = new Rect(pos.x + soloOffset.x, pos.y + soloOffset.y, soloBtn.Rect.width, soloBtn.Rect.height);
		coopBtn.Rect = new Rect(pos.x + coopOffset.x, pos.y + coopOffset.y, coopBtn.Rect.width, coopBtn.Rect.height);
	}

	public void HideNetLoading()
	{
		netLoadingUI.Hide();
	}

	public void IsUploadData()
	{
		string msg = UIConstant.GetMessage(16).Replace("[n]", "\n");
		msgUI.CreateQueryUpload(msg, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_UPLOAD_DOWNLOAD);
		msgUI.Show();
	}

	public void PopupServerMessage(byte textIndex, byte eventID)
	{
		string msg = UIConstant.GetMessage(textIndex).Replace("[n]", "\n");
		msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_ERROR, eventID);
		msgUI.Show();
		netLoadingUI.Hide();
	}

	public void PopupQueryConFirm(byte textIndex, byte eventID)
	{
		string msg = UIConstant.GetMessage(textIndex).Replace("[n]", "\n");
		msgUI.CreateQuery(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, eventID);
		msgUI.Show();
	}

	public void ShowGift()
	{
		giftUI.Show();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == soloBtn)
		{
			stateMgr.FrFree();
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null)
			{
				GameApp.GetInstance().DestoryNetWork();
			}
			if (GameApp.GetInstance().GetUserState().GetFirstLunchApp())
			{
				Application.LoadLevel("Tutorial");
			}
			else
			{
				Application.LoadLevel("SoloMenu");
			}
		}
		else if (control == coopBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			Login();
		}
		else
		{
			if (control == guardBtn || control == vsBtn)
			{
				return;
			}
			if (control == navigationMenu)
			{
				switch (command)
				{
				case 1:
					stateMgr.m_UIManager.SetEnable(true);
					break;
				case 0:
					stateMgr.m_UIManager.SetEnable(false);
					break;
				}
				switch (command)
				{
				case 2:
					stateMgr.FrGoToPhase(4, false, false, false);
					break;
				case 3:
					stateMgr.FrGoToPhase(3, false, false, false);
					break;
				case 4:
					stateMgr.FrGoToPhase(8, false, false, false);
					break;
				case 5:
					stateMgr.FrGoToPhase(11, false, false, false);
					break;
				}
			}
			else if (control == msgUI)
			{
				int eventID = msgUI.GetEventID();
				if (eventID == MessageBoxUI.EVENT_UPLOAD_DOWNLOAD)
				{
					switch (command)
					{
					case 10:
						PopupQueryConFirm(26, MessageBoxUI.EVENT_UPLOAD_CONFIRM);
						break;
					case 9:
						PopupQueryConFirm(27, MessageBoxUI.EVENT_DOWNLOAD_CONFIRM);
						break;
					}
				}
				else if (eventID == MessageBoxUI.EVENT_DOWNLOAD_CONFIRM)
				{
					StartMenuScript startMenuScript = stateMgr as StartMenuScript;
					if (command == 10)
					{
						UserData userData = startMenuScript.GetUserData();
						if (userData != null)
						{
							userData.ResetUserData();
							GameApp.GetInstance().Save();
						}
					}
					startMenuScript.ClearUserData();
					msgUI.Hide();
					startMenuScript.GotoMultiMenu();
				}
				else if (eventID == MessageBoxUI.EVENT_UPLOAD_CONFIRM)
				{
					StartMenuScript startMenuScript2 = stateMgr as StartMenuScript;
					if (command == 10)
					{
						startMenuScript2.UpLoadData();
					}
					startMenuScript2.ClearUserData();
					msgUI.Hide();
					startMenuScript2.GotoMultiMenu();
				}
				else if (eventID == MessageBoxUI.EVENT_LOGIN_COOP)
				{
					switch (command)
					{
					case 9:
					{
						msgUI.Hide();
						string msg = UIConstant.GetMessage(30).Replace("[n]", "\n");
						msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_GUEST_HINT);
						msgUI.Show();
						break;
					}
					case 10:
						msgUI.Hide();
						LoginAsUser();
						break;
					}
				}
				else if (eventID == MessageBoxUI.EVENT_NET_ACCOUNT_LOCKED)
				{
					if (command == 9)
					{
						msgUI.Hide();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_NET_SERVER_MIANTENANCE)
				{
					if (command == 9)
					{
						msgUI.Hide();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_NET_VERSION_MISMATCH)
				{
					if (command == 9)
					{
						msgUI.Hide();
						if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
						{
							Application.OpenURL(AndroidSwPluginScript.GetVersionUrl());
						}
						else if (AndroidConstant.version == AndroidConstant.Version.Kindle)
						{
							AndroidAWSPluginScript.OpenAwsUrl(AndroidConstant.BUNDLEID_STARWARFARE_KINDLE);
						}
					}
				}
				else if (eventID == MessageBoxUI.EVENT_GUEST_HINT)
				{
					if (command == 9)
					{
						msgUI.Hide();
						LoginAsGuest();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_CAN_NOT_ACCESS_INTERNET)
				{
					if (command == 9)
					{
						msgUI.Hide();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_RECORD_ERROR)
				{
					if (command == 9)
					{
						msgUI.Hide();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_AD_REWARD)
				{
					if (command == 9)
					{
						msgUI.Hide();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_QUIT_GAME)
				{
					switch (command)
					{
					case 9:
						msgUI.Hide();
						break;
					case 10:
						Application.Quit();
						break;
					}
				}
				else if (eventID == MessageBoxUI.EVENT_SHOW_MOVIE)
				{
					switch (command)
					{
					case 9:
						msgUI.Hide();
						break;
					case 10:
					{
						ShowMovie();
						string msg2 = "Download Our New App NOW!";
						msgUI.CreateConfirmMovie(msg2, MessageBoxUI.MESSAGE_FLAG_MOVIE, MessageBoxUI.EVENT_STORE_LINK);
						msgUI.Show();
						break;
					}
					}
				}
				else if (eventID == MessageBoxUI.EVENT_STORE_LINK)
				{
					switch (command)
					{
					case 9:
						msgUI.Hide();
						break;
					case 10:
						msgUI.Hide();
						Application.OpenURL(UIConstant.RUSH_VERSION_URL);
						break;
					}
				}
				else if (eventID == MessageBoxUI.EVENT_NET_TIMEOUT)
				{
					if (command == 9)
					{
						msgUI.Hide();
					}
				}
				else if (eventID == MessageBoxUI.EVENT_STORE_PROMOTION)
				{
					switch (command)
					{
					case 10:
						Debug.Log("Go to Store");
						msgUI.Hide();
						stateMgr.FrGoToPhase(4, false, false, false);
						break;
					case 9:
						msgUI.Hide();
						break;
					}
				}
			}
			else if (control == netLoadingUI)
			{
				if (command == 0)
				{
					logining = false;
				}
			}
			else if (control == giftUI)
			{
				if (command == 0)
				{
					UserState userState = GameApp.GetInstance().GetUserState();
					int num = (int)wparam;
					int num2 = UIConstant.GIFT_CASH[num];
					if (wparam < 3f)
					{
						userState.AddCash(num2);
					}
					else
					{
						userState.AddMithril(num2);
					}
					num++;
					num %= 5;
					userState.SetTimeSpan((byte)num);
					StartMenuScript startMenuScript3 = stateMgr as StartMenuScript;
					startMenuScript3.UpLoadDataLogic();
					giftUI.Hide();
				}
			}
			else if (control == facebookImg)
			{
				if (GameApp.GetInstance().IsConnectedToInternet())
				{
					Application.OpenURL(UIConstant.FACEBOOK_HOME);
					UserState userState2 = GameApp.GetInstance().GetUserState();
					if (!userState2.GetFacebook())
					{
						userState2.AddMithril(3);
						userState2.SetFacebook(true);
						GameApp.GetInstance().Save();
						facebookImg.Visible = false;
						facebookImg.Enable = false;
					}
				}
			}
			else if (control == twitterImg)
			{
				if (GameApp.GetInstance().IsConnectedToInternet())
				{
					Application.OpenURL(UIConstant.TWITTER_HOME);
					UserState userState3 = GameApp.GetInstance().GetUserState();
					if (!userState3.GetTwitter())
					{
						userState3.AddMithril(3);
						userState3.SetTwitter(true);
						GameApp.GetInstance().Save();
						twitterImg.Visible = false;
						twitterImg.Enable = false;
					}
				}
			}
			else if (control == freeGotMithrilImg)
			{
				if (GameApp.GetInstance().IsConnectedToInternet())
				{
					adsui = new AdsUI(stateMgr);
					adsui.Create();
					stateMgr.m_UIPopupManager.Add(adsui);
					adsui.Show();
				}
				else
				{
					string msg3 = UIConstant.GetMessage(40).Replace("[n]", "\n");
					msgUI.CreateConfirm(msg3, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_CAN_NOT_ACCESS_INTERNET);
					msgUI.Show();
				}
			}
		}
	}

	private string ValueToChar(int value)
	{
		value = Math.Abs(value);
		if (value < usernameStringList.Length)
		{
			return usernameStringList[value];
		}
		return string.Empty;
	}

	private string ValueToString(int value, int length)
	{
		value = Math.Abs(value);
		int num = value.ToString().Length - 1;
		int num2 = usernameStringList.Length;
		string text = string.Empty;
		while (num > -1)
		{
			int num3 = (int)((double)value / Math.Pow(num2, num));
			text += ValueToChar(num3);
			value -= (int)(Math.Pow(num2, num) * (double)num3);
			num--;
		}
		return text.Substring(text.Length - length, length);
	}

	private void LoginAsUser()
	{
		string uUID = GameApp.GetInstance().UUID;
		string roleName = userState.GetRoleName();
		Lobby.GetInstance().SetUserName(roleName);
		netLoadingUI.Show(30);
		if (GameApp.GetInstance().GetUserState().GetMithril() >= 20000)
		{
			netLoadingUI.SetMessage(23, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_NET_TIMEOUT);
		}
		else
		{
			netLoadingUI.SetMessage(2, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_NET_TIMEOUT);
		}
		networkMgr = GameApp.GetInstance().CreateNetwork();
		PlayerLoginRequest playerLoginRequest = new PlayerLoginRequest();
		if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			playerLoginRequest.userName = "P:" + uUID;
		}
		else if (AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			playerLoginRequest.userName = "K:" + uUID;
		}
		playerLoginRequest.passWord = "123";
		playerLoginRequest.version = GameApp.GetInstance().GetUserState().version;
		if (GameApp.GetInstance().MacAddress != null)
		{
			playerLoginRequest.udid = GameApp.GetInstance().MacAddress;
		}
		else
		{
			playerLoginRequest.udid = GameApp.GetInstance().UUID;
		}
		playerLoginRequest.mithril = GameApp.GetInstance().GetUserState().GetMithril();
		playerLoginRequest.platform = "android";
		networkMgr.SendRequest(playerLoginRequest);
		TimeManager.GetInstance().LastSynTime = Time.time;
		Lobby.GetInstance().IsGuest = false;
	}

	private void Login()
	{
		LoginAsUser();
	}

	public string GetRandomString(int length)
	{
		string empty = string.Empty;
		char c = (char)Random.Range(65, 91);
		empty += c;
		for (int i = 0; i < length; i++)
		{
			c = (char)Random.Range(65, 91);
			empty += c;
		}
		return empty;
	}

	public void LoginAsGuest()
	{
		string userName = "Guest_" + GetRandomString(4);
		Lobby.GetInstance().SetUserName(userName);
		netLoadingUI.Show(30);
		if (GameApp.GetInstance().GetUserState().GetMithril() >= 20000)
		{
			netLoadingUI.SetMessage(23, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_NET_TIMEOUT);
		}
		else
		{
			netLoadingUI.SetMessage(2, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_NET_TIMEOUT);
		}
		networkMgr = GameApp.GetInstance().CreateNetwork();
		PlayerLoginRequest playerLoginRequest = new PlayerLoginRequest();
		playerLoginRequest.userName = userName;
		playerLoginRequest.passWord = "g";
		playerLoginRequest.version = GameApp.GetInstance().GetUserState().version;
		if (GameApp.GetInstance().MacAddress != null)
		{
			playerLoginRequest.udid = GameApp.GetInstance().MacAddress;
		}
		else
		{
			playerLoginRequest.udid = GameApp.GetInstance().UUID;
		}
		playerLoginRequest.mithril = GameApp.GetInstance().GetUserState().GetMithril();
		playerLoginRequest.platform = "android";
		networkMgr.SendRequest(playerLoginRequest);
		TimeManager.GetInstance().LastSynTime = Time.time;
		Lobby.GetInstance().IsGuest = true;
	}

	public AdsUI GetAds()
	{
		return adsui;
	}

	public void ClearDisCount()
	{
		userState.SetDiscountStatus(0);
		userState.SetDiscountTime("0000-00-00 00:00");
		userState.SetShowNotify(0);
		userState.SetDiscountWeapon(-1);
		userState.SetGameTime(0);
		Debug.Log("clear");
	}

	public void ShowMovie()
	{
		Debug.Log("showmovie");
		userState.SetShowMovieDate(DateTime.Now.ToString("yyyy-MM-dd"));
		userState.AddShowMovieTime();
		GameApp.GetInstance().Save();
	}
}
