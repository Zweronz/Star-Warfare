using UnityEngine;

public class AdsUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Exit = 0
	}

	public UIStateManager stateMgr;

	private UIImage shadowImg;

	private UIImage backBround;

	private UIClickButton backBtn;

	private UIClickButton tapJoyIcon;

	private UIClickButton tapJoyButton;

	private UIClickButton flurryIcon;

	private UIClickButton flurryButton;

	private UIClickButton adcolonyIcon;

	private UIClickButton adcolonyButton;

	private UIImage facebookIcon;

	private UIClickButton facebookButton;

	private UIImage twitterIcon;

	private UIClickButton twitterButton;

	private UIClickButton sponsorpayIcon;

	private UIClickButton sponsorpayButton;

	private UIBlock m_block;

	public UIClickButton FreeMithril_RunBlade;

	private UIImage rushtext;

	public AdsUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Close()
	{
		stateMgr.m_UIManager.SetEnable(true);
		Clear();
		base.Visible = false;
		Res2DManager.GetInstance().FreeUI(28, true);
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[28];
		if (unitUI == null)
		{
			Res2DManager.GetInstance().LoadImmediately(28);
			unitUI = Res2DManager.GetInstance().vUI[28];
		}
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight + 100f));
		backBround = new UIImage();
		backBround.AddObject(unitUI, 0, 1);
		backBround.Rect = backBround.GetObjectRect();
		backBtn = new UIClickButton();
		backBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 2);
		backBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 2);
		backBtn.Rect = backBtn.GetObjectRect(UIButtonBase.State.Normal);
		backBtn.Rect = new Rect(backBtn.Rect.xMin - 40f, backBtn.Rect.yMin - 40f, backBtn.Rect.width + 80f, backBtn.Rect.height + 80f);
		tapJoyIcon = new UIClickButton();
		tapJoyIcon.AddObject(UIButtonBase.State.Normal, unitUI, 0, 3);
		tapJoyIcon.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 3);
		tapJoyIcon.Rect = tapJoyIcon.GetObjectRect(UIButtonBase.State.Normal);
		tapJoyButton = new UIClickButton();
		tapJoyButton.AddObject(UIButtonBase.State.Normal, unitUI, 0, 4);
		tapJoyButton.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 4);
		tapJoyButton.Rect = tapJoyButton.GetObjectRect(UIButtonBase.State.Normal);
		flurryIcon = new UIClickButton();
		flurryIcon.AddObject(UIButtonBase.State.Normal, unitUI, 0, 5);
		flurryIcon.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 5);
		flurryIcon.Rect = flurryIcon.GetObjectRect(UIButtonBase.State.Normal);
		flurryButton = new UIClickButton();
		flurryButton.AddObject(UIButtonBase.State.Normal, unitUI, 0, 6);
		flurryButton.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 6);
		flurryButton.Rect = flurryButton.GetObjectRect(UIButtonBase.State.Normal);
		adcolonyIcon = new UIClickButton();
		adcolonyIcon.AddObject(UIButtonBase.State.Normal, unitUI, 0, 7);
		adcolonyIcon.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 7);
		adcolonyIcon.Rect = adcolonyIcon.GetObjectRect(UIButtonBase.State.Normal);
		adcolonyButton = new UIClickButton();
		adcolonyButton.AddObject(UIButtonBase.State.Normal, unitUI, 0, 12);
		adcolonyButton.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 12);
		adcolonyButton.Rect = adcolonyButton.GetObjectRect(UIButtonBase.State.Normal);
		facebookIcon = new UIImage();
		facebookIcon.AddObject(unitUI, 0, 8);
		facebookIcon.Rect = facebookIcon.GetObjectRect();
		facebookButton = new UIClickButton();
		facebookButton.AddObject(UIButtonBase.State.Normal, unitUI, 0, 9);
		facebookButton.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 9);
		facebookButton.Rect = facebookButton.GetObjectRect(UIButtonBase.State.Normal);
		twitterIcon = new UIImage();
		twitterIcon.AddObject(unitUI, 0, 10);
		twitterIcon.Rect = twitterIcon.GetObjectRect();
		twitterButton = new UIClickButton();
		twitterButton.AddObject(UIButtonBase.State.Normal, unitUI, 0, 11);
		twitterButton.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 11);
		twitterButton.Rect = twitterButton.GetObjectRect(UIButtonBase.State.Normal);
		sponsorpayIcon = new UIClickButton();
		sponsorpayIcon.AddObject(UIButtonBase.State.Normal, unitUI, 0, 14);
		sponsorpayIcon.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 14);
		sponsorpayIcon.Rect = sponsorpayIcon.GetObjectRect(UIButtonBase.State.Normal);
		sponsorpayButton = new UIClickButton();
		sponsorpayButton.AddObject(UIButtonBase.State.Normal, unitUI, 0, 15);
		sponsorpayButton.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 15);
		sponsorpayButton.Rect = sponsorpayButton.GetObjectRect(UIButtonBase.State.Normal);
		FreeMithril_RunBlade = new UIClickButton();
		FreeMithril_RunBlade.AddObject(UIButtonBase.State.Normal, unitUI, 0, 13);
		FreeMithril_RunBlade.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 13);
		FreeMithril_RunBlade.Rect = FreeMithril_RunBlade.GetObjectRect(UIButtonBase.State.Normal);
		rushtext = new UIImage();
		rushtext.AddObject(unitUI, 0, 16);
		rushtext.Rect = rushtext.GetObjectRect();
		Add(shadowImg);
		Add(backBround);
		Add(backBtn);
		if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			Add(tapJoyIcon);
			Add(tapJoyButton);
			Add(flurryIcon);
			Add(flurryButton);
			Add(adcolonyIcon);
			Add(adcolonyButton);
			Add(sponsorpayButton);
			Add(sponsorpayIcon);
		}
		Add(facebookIcon);
		Add(facebookButton);
		Add(twitterIcon);
		Add(twitterButton);
		Add(FreeMithril_RunBlade);
		Add(rushtext);
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState.GetFacebook())
		{
			facebookButton.Enable = false;
			facebookButton.Visible = false;
			facebookIcon.Enable = false;
			facebookIcon.Visible = false;
		}
		if (userState.GetTwitter())
		{
			twitterButton.Enable = false;
			twitterButton.Visible = false;
			twitterIcon.Enable = false;
			twitterIcon.Visible = false;
		}
		if (GameApp.GetInstance().GetUserState().GetRewardStatus() != 2)
		{
			FreeMithril_RunBlade.Enable = true;
			FreeMithril_RunBlade.Visible = true;
			rushtext.Visible = true;
		}
		else
		{
			FreeMithril_RunBlade.Enable = false;
			FreeMithril_RunBlade.Visible = false;
			rushtext.Visible = false;
			rushtext.Enable = false;
		}
		stateMgr.m_UIManager.SetEnable(false);
		SetUIHandler(this);
		AndroidPluginScript.isGoogleServiceReady();
	}

	public void ShowDownloadIcon()
	{
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override void Update()
	{
		base.Update();
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == adcolonyButton || control == adcolonyIcon)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallAdcolonyVideo();
			}
		}
		else if (control == tapJoyButton || control == tapJoyIcon)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallTapjoyOfferWall();
			}
		}
		else if (control == sponsorpayButton || control == sponsorpayIcon)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallSponsorPayOfferWall();
			}
		}
		else if (control == flurryButton || control == flurryIcon)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				AndroidAdsPluginScript.CallFlurryAds();
			}
		}
		else if (control == FreeMithril_RunBlade)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				if (GameApp.GetInstance().GetUserState().GetRewardStatus() != 2)
				{
					GameApp.GetInstance().GetUserState().SetRewardStatus(1);
					GameApp.GetInstance().Save();
				}
				AndroidPluginScript.OpenURL();
			}
		}
		else if (control == facebookButton)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				Application.OpenURL(UIConstant.FACEBOOK_HOME);
				UserState userState = GameApp.GetInstance().GetUserState();
				if (!userState.GetFacebook())
				{
					facebookButton.Enable = false;
					facebookButton.Visible = false;
					facebookIcon.Enable = false;
					facebookIcon.Visible = false;
					userState.AddMithril(3);
					userState.SetFacebook(true);
					GameApp.GetInstance().Save();
				}
			}
		}
		else if (control == twitterButton)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				Application.OpenURL(UIConstant.TWITTER_HOME);
				UserState userState2 = GameApp.GetInstance().GetUserState();
				if (!userState2.GetTwitter())
				{
					twitterButton.Enable = false;
					twitterButton.Visible = false;
					twitterIcon.Enable = false;
					twitterIcon.Visible = false;
					userState2.AddMithril(3);
					userState2.SetTwitter(true);
					GameApp.GetInstance().Save();
				}
			}
		}
		else if (control == backBtn)
		{
			Close();
			m_Parent.SendEvent(this, 0, 0f, 0f);
		}
	}
}
