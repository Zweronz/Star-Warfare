using System;
using UnityEngine;

public class StatisticsBaseUI : UIHandler, IUIHandle
{
	protected const byte STATE_INIT = 0;

	protected const byte STATE_CREATE = 1;

	protected const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	protected Player player;

	protected UserState userState;

	protected NavigationBarUI navigationBar;

	protected UIImage statisticsImg;

	protected UIImage statisticsBGImg;

	protected UIImage shadowImg;

	protected static byte[] SKIP_NORMAL = new byte[2] { 42, 44 };

	protected static byte[] SKIP_PRESSED = new byte[2] { 41, 43 };

	protected static byte[] CONT_NORMAL = new byte[2] { 47, 49 };

	protected static byte[] CONT_PRESSED = new byte[2] { 45, 48 };

	protected static byte[] CONT_DISABLED = new byte[2] { 46, 48 };

	protected UIClickButton skipBtn;

	protected UIClickButton continueBtn;

	protected byte state;

	protected static byte STATISTICS_BEGIN_IMG = 3;

	protected static byte STATISTICS_COUNT_IMG = 18;

	protected UIImage ipadImg;

	protected UIClickButton twitterImg;

	protected UIClickButton facebookImg;

	protected MessageBoxUI msgUI;

	public StatisticsBaseUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public virtual void Init()
	{
		Time.timeScale = 0f;
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public virtual void Close()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public virtual void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[15];
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("STATISTICS");
		navigationBar.Show();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		statisticsBGImg = new UIImage();
		statisticsBGImg.AddObject(unitUI, 0, 1);
		statisticsBGImg.Rect = statisticsBGImg.GetObjectRect();
		statisticsBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		statisticsImg = new UIImage();
		statisticsImg.AddObject(unitUI, 0, STATISTICS_BEGIN_IMG, STATISTICS_COUNT_IMG);
		statisticsImg.Rect = statisticsImg.GetObjectRect();
		skipBtn = new UIClickButton();
		skipBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, SKIP_NORMAL);
		skipBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, SKIP_PRESSED);
		skipBtn.Rect = skipBtn.GetObjectRect(UIButtonBase.State.Normal);
		continueBtn = new UIClickButton();
		continueBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, CONT_NORMAL);
		continueBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, CONT_PRESSED);
		continueBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 0, CONT_DISABLED);
		continueBtn.Rect = continueBtn.GetObjectRect(UIButtonBase.State.Normal);
		continueBtn.Enable = false;
		twitterImg = new UIClickButton();
		twitterImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 68);
		twitterImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 70);
		twitterImg.Rect = twitterImg.GetObjectRect(UIButtonBase.State.Normal);
		facebookImg = new UIClickButton();
		facebookImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 69);
		facebookImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 71);
		facebookImg.Rect = facebookImg.GetObjectRect(UIButtonBase.State.Normal);
		twitterImg.Visible = false;
		twitterImg.Enable = false;
		facebookImg.Visible = false;
		facebookImg.Enable = false;
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width) / 2;
			num2 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect((0f - num2) / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth + num2 / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height) / 2;
			num3 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, (0f - num3) / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, UIConstant.ScreenLocalHeight + num3 / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
		}
	}

	public virtual bool Update()
	{
		switch (state)
		{
		default:
			return false;
		}
	}

	public void ShowMsgTwitterPosted()
	{
		if (msgUI != null)
		{
			string msg = UIConstant.GetMessage(42).Replace("[n]", "\n");
			msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_SEND_TWITTER_CONFIRM);
			msgUI.Show();
		}
	}

	public void ShowMsgFacebookPosted()
	{
		if (msgUI != null)
		{
			string msg = UIConstant.GetMessage(39).Replace("[n]", "\n");
			msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_SEND_FACEBOOK_CONFIRM);
			msgUI.Show();
		}
	}

	public virtual void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == twitterImg)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				if (!TwitterAndroid.isLoggedIn())
				{
					Lobby.GetInstance().IsPostingScoreToSocialNetwork = true;
					TwitterAndroid.showLoginDialog();
					return;
				}
				string update = UIConstant.SHOW_SCORE_STR + " " + AndroidSwPluginScript.GetVersionUrl();
				Application.CaptureScreenshot("tempscreens.png");
				string url = Application.persistentDataPath + "/tempscreens.png";
				WWW wWW = new WWW(url);
				TwitterAndroid.postUpdateWithImage(update, wWW.bytes);
				wWW = null;
			}
			else
			{
				string msg = UIConstant.GetMessage(40).Replace("[n]", "\n");
				msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_CAN_NOT_ACCESS_INTERNET);
				msgUI.Show();
			}
		}
		else if (control == facebookImg)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				Application.CaptureScreenshot("tempscreens.png");
				if (!FacebookAndroid.isLoggedIn())
				{
					Lobby.GetInstance().IsPostingScoreToSocialNetwork = true;
					FacebookAndroid.login();
					return;
				}
				string url2 = Application.persistentDataPath + "/tempscreens.png";
				WWW wWW2 = new WWW(url2);
				Facebook.instance.postMessageWithLinkAndLinkToImage(UIConstant.SHOW_SCORE_STR, AndroidSwPluginScript.GetVersionUrl(), "Star Warfare:Aliens Invasion", "http://125.141.149.48/iTunesArtwork.png", "Join us!", null);
				Facebook.instance.postImage(wWW2.bytes, "Hey! I am playing Star Warfare!", null);
				wWW2 = null;
			}
			else
			{
				string msg2 = UIConstant.GetMessage(40).Replace("[n]", "\n");
				msgUI.CreateConfirm(msg2, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_CAN_NOT_ACCESS_INTERNET);
				msgUI.Show();
			}
		}
		else
		{
			if (control != msgUI)
			{
				return;
			}
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_SEND_TWITTER_CONFIRM)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
			else if (eventID == MessageBoxUI.EVENT_SEND_FACEBOOK_CONFIRM)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
		}
	}
}
