using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetStatisticsUIBase : MonoBehaviour, UIHandler, IUIHandle
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

	protected static byte[] SKIP_NORMAL = new byte[2] { 34, 37 };

	protected static byte[] SKIP_PRESSED = new byte[2] { 35, 36 };

	protected static byte[] CONT_NORMAL = new byte[2] { 40, 42 };

	protected static byte[] CONT_PRESSED = new byte[2] { 38, 41 };

	protected static byte[] CONT_DISABLED = new byte[2] { 39, 41 };

	private static byte STATISTICS_BEGIN_IMG = 3;

	private static byte STATISTICS_COUNT_IMG = 18;

	protected UIClickButton skipBtn;

	protected UIClickButton continueBtn;

	protected byte state;

	protected MessageBoxUI msgUI;

	protected List<Player> players = new List<Player>();

	protected UIClickButton twitterImg;

	protected UIClickButton facebookImg;

	protected UIImage ipadImg;

	public NetStatisticsUIBase(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public virtual void Init()
	{
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public virtual void Close()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		players.Clear();
	}

	public virtual void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[15];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[27];
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 7, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("STATISTICS");
		navigationBar.Show();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 7, 2);
		statisticsBGImg = new UIImage();
		statisticsBGImg.AddObject(unitUI, 7, 1);
		statisticsBGImg.Rect = statisticsBGImg.GetObjectRect();
		statisticsBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		statisticsImg = new UIImage();
		statisticsImg.AddObject(unitUI, 7, STATISTICS_BEGIN_IMG, STATISTICS_COUNT_IMG);
		statisticsImg.Rect = statisticsImg.GetObjectRect();
		skipBtn = new UIClickButton();
		skipBtn.AddObject(UIButtonBase.State.Normal, unitUI, 7, SKIP_NORMAL);
		skipBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 7, SKIP_PRESSED);
		skipBtn.Rect = skipBtn.GetObjectRect(UIButtonBase.State.Normal);
		continueBtn = new UIClickButton();
		continueBtn.AddObject(UIButtonBase.State.Normal, unitUI, 7, CONT_NORMAL);
		continueBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 7, CONT_PRESSED);
		continueBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 7, CONT_DISABLED);
		continueBtn.Rect = continueBtn.GetObjectRect(UIButtonBase.State.Normal);
		continueBtn.Enable = false;
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		twitterImg = new UIClickButton();
		twitterImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 68);
		twitterImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 70);
		twitterImg.Rect = twitterImg.GetObjectRect(UIButtonBase.State.Normal);
		twitterImg.Visible = false;
		twitterImg.Enable = false;
		facebookImg = new UIClickButton();
		facebookImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 69);
		facebookImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 71);
		facebookImg.Rect = facebookImg.GetObjectRect(UIButtonBase.State.Normal);
		facebookImg.Visible = false;
		facebookImg.Enable = false;
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

	private IEnumerator PostTwitter(string pathToImage, string str)
	{
		WWW imageData2 = new WWW(pathToImage);
		yield return imageData2;
		TwitterAndroid.postUpdateWithImage(str, imageData2.bytes);
		imageData2 = null;
	}

	private IEnumerator PostFacebook(string pathToImage)
	{
		WWW imageData2 = new WWW(pathToImage);
		yield return imageData2;
		Facebook.instance.postMessageWithLinkAndLinkToImage(UIConstant.SHOW_SCORE_STR, AndroidSwPluginScript.GetVersionUrl(), "Star Warfare:Aliens Invasion", "http://125.141.149.48/iTunesArtwork.png", "Join us!", null);
		Facebook.instance.postImage(imageData2.bytes, "Hey! I am playing Star Warfare!", null);
		imageData2 = null;
	}

	public virtual void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == twitterImg)
		{
			string str = "My Score! " + AndroidSwPluginScript.GetVersionUrl();
			if (!GameApp.GetInstance().IsConnectedToInternet())
			{
				return;
			}
			Debug.Log("twitter");
			bool flag = TwitterAndroid.isLoggedIn();
			Debug.Log("isLoggedIn = " + flag);
			Lobby.GetInstance().IsPostingScoreToSocialNetwork = true;
			if (flag)
			{
				Debug.Log("post twitterImg");
				ScreenCapture.CaptureScreenshot("tempscreens.png");
				string pathToImage = Application.persistentDataPath + "/tempscreens.png";
				try
				{
					StartCoroutine(PostTwitter(pathToImage, str));
					return;
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
					return;
				}
			}
			Debug.Log("islogin");
			TwitterAndroid.showLoginDialog();
		}
		else if (control == facebookImg)
		{
			if (!GameApp.GetInstance().IsConnectedToInternet())
			{
				return;
			}
			ScreenCapture.CaptureScreenshot("tempscreens.png");
			bool flag2 = FacebookAndroid.isLoggedIn();
			Debug.Log("isLoggedIn = " + flag2);
			if (flag2)
			{
				Debug.Log("post facebookImg");
				string pathToImage2 = Application.persistentDataPath + "/tempscreens.png";
				try
				{
					StartCoroutine(PostFacebook(pathToImage2));
					return;
				}
				catch (Exception ex2)
				{
					Debug.Log(ex2.ToString());
					return;
				}
			}
			Debug.Log("islogin");
			Lobby.GetInstance().IsPostingScoreToSocialNetwork = true;
			FacebookAndroid.login();
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
