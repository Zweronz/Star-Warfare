using System;
using UnityEngine;

public class NavigationMenuUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Workable = 0,
		Unworkable = 1,
		GotoStore = 2,
		GotoCustomize = 3,
		GotoOptions = 4,
		GotoExtra = 5
	}

	public const byte STATE_INIT = 0;

	public const byte STATE_WORKABLE = 1;

	public const byte STATE_STRETCHING = 2;

	public const byte STATE_RETRACTING = 3;

	public const byte STATE_UNWORKABLE = 4;

	public const byte STATE_IAP = 5;

	public UIStateManager stateMgr;

	private static byte NAV_BACKGROUND_BEGIN_IMG;

	private static byte NAV_BACKGROUND_COUNT_IMG = 3;

	private IAPUI iapUI;

	private UIImage shadowImg;

	private UIImage navigationMenuImg;

	private UIImage activeMenuImg;

	private UIImage rankImg;

	private UIImage IAPImg;

	private UITextButton optionTxtBtn;

	private UITextButton roleNameBtn;

	private FrUIText roleNameTxt;

	private UITextButton customizeTxtBtn;

	private UITextButton shopTxtBtn;

	private Vector2 navigationOffset;

	private Vector2 activeOffset;

	private Vector2 IAPOffset;

	private Vector2 optionOffset;

	private Vector2 extraOffset;

	private Vector2 customizeOffset;

	private Vector2 shopOffset;

	private Vector2 rankOffset;

	private Vector2 rolenameOffset;

	private Vector2 changenameOffset;

	public byte m_state;

	private static int BEGIN_Y = 93;

	private byte m_rank;

	private float m_time;

	private bool m_bIncrease;

	private DateTime m_beginTime;

	private byte m_count;

	private byte m_interval;

	private UIImage ipadImg;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			shadowImg.Rect = new Rect(0f, 0f, UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight);
			navigationMenuImg.Rect = new Rect(value.x + navigationOffset.x, value.y + navigationOffset.y, navigationMenuImg.Rect.width, navigationMenuImg.Rect.height);
			activeMenuImg.Rect = new Rect(value.x + activeOffset.x, value.y + activeOffset.y, activeMenuImg.Rect.width, activeMenuImg.Rect.height);
			IAPImg.Rect = new Rect(value.x + IAPOffset.x, value.y + IAPOffset.y, IAPImg.Rect.width, IAPImg.Rect.height);
			optionTxtBtn.Rect = new Rect(value.x + optionOffset.x, value.y + optionOffset.y, optionTxtBtn.Rect.width, optionTxtBtn.Rect.height);
			roleNameBtn.Rect = new Rect(value.x + changenameOffset.x, value.y + changenameOffset.y, roleNameBtn.Rect.width, roleNameBtn.Rect.height);
			roleNameTxt.Rect = new Rect(value.x + rolenameOffset.x, value.y + rolenameOffset.y, roleNameTxt.Rect.width, roleNameTxt.Rect.height);
			customizeTxtBtn.Rect = new Rect(value.x + customizeOffset.x, value.y + customizeOffset.y, customizeTxtBtn.Rect.width, customizeTxtBtn.Rect.height);
			shopTxtBtn.Rect = new Rect(value.x + shopOffset.x, value.y + shopOffset.y, shopTxtBtn.Rect.width, shopTxtBtn.Rect.height);
			rankImg.Rect = new Rect(value.x + rankOffset.x, value.y + rankOffset.y, rankImg.Rect.width, rankImg.Rect.height);
		}
	}

	public NavigationMenuUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Create()
	{
		m_state = 0;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[14];
		Vector2 framePosition = unitUI.GetFramePosition(0, 0);
		Vector2 frameSize = unitUI.GetFrameSize(0, 0);
		Rect rect = new Rect(framePosition.x, framePosition.y, frameSize.x, frameSize.y);
		m_interval = 2;
		m_count = 0;
		m_time = 255f;
		m_bIncrease = false;
		m_beginTime = DateTime.Now;
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 1, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		shadowImg.Visible = false;
		navigationMenuImg = new UIImage();
		navigationMenuImg.AddObject(unitUI, 0, NAV_BACKGROUND_BEGIN_IMG, NAV_BACKGROUND_COUNT_IMG);
		navigationMenuImg.Rect = navigationMenuImg.GetObjectRect();
		navigationOffset.x = navigationMenuImg.Rect.x - rect.x;
		navigationOffset.y = navigationMenuImg.Rect.y - rect.y;
		activeMenuImg = new UIImage();
		activeMenuImg.AddObject(unitUI, 0, 3);
		activeMenuImg.Rect = activeMenuImg.GetObjectRect();
		activeMenuImg.Enable = true;
		activeOffset.x = activeMenuImg.Rect.x - rect.x;
		activeOffset.y = activeMenuImg.Rect.y - rect.y;
		Rank rank = GameApp.GetInstance().GetUserState().GetRank();
		m_rank = rank.rankID;
		rankImg = new UIImage();
		rankImg.AddObject(unitUI, 0, 21 + m_rank);
		rankImg.Rect = rankImg.GetObjectRect();
		rankOffset.x = rankImg.Rect.x - rect.x;
		rankOffset.y = rankImg.Rect.y - rect.y;
		IAPImg = new UIImage();
		IAPImg.AddObject(unitUI, 0, 4);
		IAPImg.Rect = IAPImg.GetObjectRect();
		IAPImg.Enable = true;
		IAPOffset.x = IAPImg.Rect.x - rect.x;
		IAPOffset.y = IAPImg.Rect.y - rect.y;
		optionTxtBtn = new UITextButton();
		byte[] module = new byte[2] { 6, 8 };
		byte[] module2 = new byte[2] { 5, 7 };
		optionTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, module);
		optionTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, module2);
		optionTxtBtn.Rect = optionTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		optionTxtBtn.SetText("font2", "OPTIONS", UIConstant.fontColor_cyan);
		optionTxtBtn.SetTextOffset(0f, -40f);
		optionTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		optionOffset.x = optionTxtBtn.Rect.x - rect.x;
		optionOffset.y = optionTxtBtn.Rect.y - rect.y;
		UserState userState = GameApp.GetInstance().GetUserState();
		roleNameBtn = new UITextButton();
		roleNameBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 36);
		roleNameBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 35);
		roleNameBtn.Rect = roleNameBtn.GetObjectRect(UIButtonBase.State.Normal);
		roleNameBtn.SetText("font3", "Edit Name", UIConstant.fontColor_cyan, roleNameBtn.Rect.width);
		roleNameBtn.SetTextOffset(0f, 0f);
		roleNameBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		changenameOffset.x = roleNameBtn.Rect.x - rect.x;
		changenameOffset.y = roleNameBtn.Rect.y - rect.y;
		roleNameTxt = new FrUIText();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 37);
		roleNameTxt = new FrUIText();
		roleNameTxt.Rect = modulePositionRect;
		roleNameTxt.Set("font3", UpdateRoleName(), UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect.width);
		rolenameOffset.x = roleNameTxt.Rect.x - rect.x;
		rolenameOffset.y = roleNameTxt.Rect.y - rect.y;
		customizeTxtBtn = new UITextButton();
		byte[] module3 = new byte[2] { 18, 20 };
		byte[] module4 = new byte[2] { 17, 19 };
		customizeTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, module3);
		customizeTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, module4);
		customizeTxtBtn.Rect = customizeTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		customizeTxtBtn.SetText("font2", "CUSTOMIZE", UIConstant.fontColor_cyan);
		customizeTxtBtn.SetTextOffset(0f, -40f);
		customizeTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		customizeOffset.x = customizeTxtBtn.Rect.x - rect.x;
		customizeOffset.y = IAPImg.Rect.y - rect.y;
		shopTxtBtn = new UITextButton();
		byte[] module5 = new byte[2] { 14, 16 };
		byte[] module6 = new byte[2] { 13, 15 };
		shopTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, module5);
		shopTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, module6);
		shopTxtBtn.Rect = shopTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		shopTxtBtn.SetText("font2", "STORE", UIConstant.fontColor_cyan);
		shopTxtBtn.SetTextOffset(0f, -40f);
		shopTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		shopOffset.x = shopTxtBtn.Rect.x - rect.x;
		shopOffset.y = IAPImg.Rect.y - rect.y;
		Add(shadowImg);
		Add(navigationMenuImg);
		Add(activeMenuImg);
		Add(rankImg);
		Add(optionTxtBtn);
		Add(roleNameBtn);
		Add(roleNameTxt);
		Add(customizeTxtBtn);
		Add(shopTxtBtn);
		//Add(IAPImg);
		iapUI = new IAPUI(stateMgr);
		Add(iapUI);
		Rect = rect;
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width) / 2;
			num2 *= 1.5f;
			ipadImg = new UIImage();
			ipadImg.AddObject(ui, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			ipadImg.Rect = new Rect((0f - num2) / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			Add(ipadImg);
			ipadImg = new UIImage();
			ipadImg.AddObject(ui, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			ipadImg.Rect = new Rect(UIConstant.ScreenLocalWidth + num2 / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			Add(ipadImg);
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height) / 2;
			num3 *= 1.5f;
			ipadImg = new UIImage();
			ipadImg.AddObject(ui, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			ipadImg.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, (0f - num3) / 2f, 0f, 0f);
			Add(ipadImg);
			ipadImg = new UIImage();
			ipadImg.AddObject(ui, 0);
			ipadImg.SetColor(Color.black);
			ipadImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			ipadImg.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, UIConstant.ScreenLocalHeight + num3 / 2f, 0f, 0f);
			Add(ipadImg);
		}
		SetUIHandler(this);
	}

	public string UpdateRoleName()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		return "NickName: " + userState.GetRoleName();
	}

	public override void Update()
	{
		bool flag = false;
		UnitUI ui = Res2DManager.GetInstance().vUI[14];
		Rank rank = GameApp.GetInstance().GetUserState().GetRank();
		if (rank.rankID != m_rank)
		{
			m_rank = rank.rankID;
			rankImg.SetTexture(ui, 0, 21 + m_rank);
		}
		if (GameApp.GetInstance().isChangeName)
		{
			roleNameTxt.Set("font3", UpdateRoleName(), UIConstant.FONT_COLOR_EQUIP_NAME);
			GameApp.GetInstance().isChangeName = false;
		}
		switch (m_state)
		{
		case 0:
			m_state = 4;
			break;
		case 1:
			shadowImg.Visible = true;
			break;
		case 2:
			if (Rect.y + Rect.height > UIConstant.ScreenLocalHeight)
			{
				Rect = new Rect(Rect.x, Rect.y - 45f, Rect.width, Rect.height);
			}
			if (Rect.y + Rect.height <= UIConstant.ScreenLocalHeight)
			{
				Rect = new Rect(Rect.x, UIConstant.ScreenLocalHeight - Rect.height, Rect.width, Rect.height);
				m_state = 1;
			}
			break;
		case 3:
			if (Rect.y + (float)BEGIN_Y < UIConstant.ScreenLocalHeight)
			{
				Rect = new Rect(Rect.x, Rect.y + 60f, Rect.width, Rect.height);
			}
			if (Rect.y + (float)BEGIN_Y >= UIConstant.ScreenLocalHeight)
			{
				Rect = new Rect(Rect.x, UIConstant.ScreenLocalHeight - (float)BEGIN_Y, Rect.width, Rect.height);
				m_state = 4;
			}
			break;
		case 5:
			if (Rect.y + (float)BEGIN_Y < UIConstant.ScreenLocalHeight)
			{
				Rect = new Rect(Rect.x, Rect.y + 60f, Rect.width, Rect.height);
			}
			if (Rect.y + (float)BEGIN_Y >= UIConstant.ScreenLocalHeight)
			{
				Rect = new Rect(Rect.x, UIConstant.ScreenLocalHeight - (float)BEGIN_Y, Rect.width, Rect.height);
			}
			break;
		case 4:
		{
			shadowImg.Visible = false;
			if (!((DateTime.Now - m_beginTime).TotalSeconds >= (double)(int)m_interval))
			{
				break;
			}
			if (m_bIncrease)
			{
				m_time += 10f;
				if (m_time > 255f)
				{
					m_bIncrease = false;
					m_time = 255f;
					m_count++;
				}
			}
			else
			{
				m_time -= 10f;
				if (m_time < 50f)
				{
					m_bIncrease = true;
					m_time = 50f;
				}
			}
			if (m_count >= 5)
			{
				m_beginTime = DateTime.Now;
				m_count = 0;
				m_time = 255f;
				m_bIncrease = false;
				m_interval = 5;
			}
			Color color = new Color(1f, 1f, 1f, m_time / 255f);
			rankImg.SetColor(color);
			break;
		}
		}
		if (iapUI.Visible)
		{
			iapUI.Update();
		}
	}

	private void SetNavVisable(bool bVisable)
	{
		optionTxtBtn.Visible = bVisable;
		roleNameBtn.Visible = bVisable;
		roleNameTxt.Visible = bVisable;
		customizeTxtBtn.Visible = bVisable;
		shopTxtBtn.Visible = bVisable;
	}

	public bool IsWorkingIAP()
	{
		if (iapUI.Visible)
		{
			return true;
		}
		return false;
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override bool HandleInput(UITouchInner touch)
	{
		switch (m_state)
		{
		case 4:
			activeMenuImg.HandleInput(touch);
			break;
		case 1:
			if (base.HandleInput(touch))
			{
				return true;
			}
			break;
		case 5:
			iapUI.HandleInput(touch);
			break;
		}
		return false;
	}

	public void ShowIAP()
	{
		shadowImg.Visible = true;
		iapUI.Create();
		iapUI.Show();
		m_state = 5;
		SetNavVisable(false);
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == activeMenuImg)
		{
			if (m_state == 4)
			{
				m_state = 2;
				shadowImg.Visible = false;
				m_Parent.SendEvent(this, 0, 0f, 0f);
				AudioManager.GetInstance().PlaySound(AudioName.PAUSE_BACK);
			}
			else if (m_state == 1)
			{
				m_state = 3;
				m_Parent.SendEvent(this, 1, 0f, 0f);
				shadowImg.Visible = true;
				AudioManager.GetInstance().PlaySound(AudioName.PAUSE);
			}
		}
		else if (control == optionTxtBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			m_Parent.SendEvent(this, 4, 0f, 0f);
		}
		else if (control == roleNameBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			AndroidSwPluginScript.SetRoleName(1);
		}
		else if (control == customizeTxtBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			m_Parent.SendEvent(this, 3, 0f, 0f);
		}
		else if (control == shopTxtBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			m_Parent.SendEvent(this, 2, 0f, 0f);
		}
		else if (control == IAPImg)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			iapUI.Create();
			iapUI.Show();
			m_state = 5;
			SetNavVisable(false);
		}
		else if (control == iapUI)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			m_state = 4;
			SetNavVisable(true);
			m_Parent.SendEvent(this, 1, 0f, 0f);
		}
	}
}
