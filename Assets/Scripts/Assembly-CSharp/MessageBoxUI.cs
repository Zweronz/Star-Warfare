using System;
using UnityEngine;

public class MessageBoxUI : UIPanelX, UIHandler
{
	public UIStateManager stateMgr;

	public static byte EVENT_UPGRADE = 0;

	public static byte EVENT_PURCHASE_CASH = 1;

	public static byte EVENT_PURCHASE_MITHRIL = 2;

	public static byte EVENT_PURCHASE_RANK = 3;

	public static byte EVENT_QUIT_GAME = 4;

	public static byte EVENT_UPLOAD_DOWNLOAD = 5;

	public static byte EVENT_GAME_CENTER_ERROR = 6;

	public static byte EVENT_NET_DISCONNECTION = 7;

	public static byte EVENT_LOST_CONNECTION = 8;

	public static byte EVENT_DEAD_TIPS = 9;

	public static byte EVENT_MAKE_PACKAGE_WARNING = 10;

	public static byte EVENT_NET_TIMEOUT = 11;

	public static byte EVENT_NET_ACCOUNT_LOCKED = 12;

	public static byte EVENT_NET_SERVER_MIANTENANCE = 13;

	public static byte EVENT_NET_VERSION_MISMATCH = 14;

	public static byte EVENT_IAP_NET_TIMEOUT = 15;

	public static byte EVENT_UPLOAD_CONFIRM = 16;

	public static byte EVENT_DOWNLOAD_CONFIRM = 17;

	public static byte EVENT_LOGIN_COOP = 18;

	public static byte EVENT_GUEST_HINT = 19;

	public static byte EVENT_NET_GAME_CENTER_TIMEOUT = 20;

	public static byte EVENT_PURCHASE_PROPS_TO_MANY = 21;

	public static byte EVENT_RANK_DISMATCH_JOINROOM = 22;

	public static byte EVENT_SEND_TWITTER_CONFIRM = 23;

	public static byte EVENT_CAN_NOT_ACCESS_INTERNET = 24;

	public static byte EVENT_LEAVE_INGAME_FOR_VS = 25;

	public static byte EVENT_SEND_FACEBOOK_CONFIRM = 26;

	public static byte EVENT_RECORD_ERROR = 27;

	public static byte EVENT_AD_REWARD = 28;

	public static byte EVENT_SHOW_DISCOUNT = 29;

	public static byte EVENT_SHOW_MOVIE = 30;

	public static byte EVENT_STORE_LINK = 31;

	public static byte EVENT_STORE_PROMOTION = 32;

	public static byte EVENT_RESTORE_DIALOG = 33;

	private static byte[] BG_IMG = new byte[1] { 1 };

	private UIBlock m_block;

	private UIImage m_shadowImg;

	private UIDialog m_message;

	private byte m_eventID;

	public static byte MESSAGE_QUERY = 0;

	public static byte MESSAGE_CONFIRM = 1;

	public static byte BACKGROUND_BEGIN_IMG = 1;

	public static byte BACKGROUND_COUNT_IMG = 2;

	public static byte MESSAGE_FLAG_WARNING = 0;

	public static byte MESSAGE_FLAG_QUERY = 1;

	public static byte MESSAGE_FLAG_ERROR = 2;

	public static byte MESSAGE_FLAG_MITHRIL = 3;

	public static byte MESSAGE_FLAG_NULL = 9;

	public static byte MESSAGE_FLAG_MOVIE = 10;

	public MessageBoxUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Close()
	{
		Clear();
	}

	public void DestoryMsg()
	{
		if (m_message != null)
		{
			m_message.Clear();
			m_message = null;
		}
	}

	public void Create()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[23];
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		m_shadowImg = new UIImage();
		m_shadowImg.AddObject(ui, 0, 0);
		m_shadowImg.Rect = m_shadowImg.GetObjectRect();
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width);
			num2 *= 1.5f;
			m_shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth + num2, UIConstant.ScreenLocalHeight));
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height);
			num3 *= 1.5f;
			m_shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight + num3));
		}
		else
		{
			m_shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		}
		Add(m_shadowImg);
		SetUIHandler(this);
	}

	public byte GetEventID()
	{
		return m_eventID;
	}

	public void CreateQuery(string msg, byte flag, byte eventID)
	{
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		m_message = new UIDialog(stateMgr, 2);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), unitUI, 1, flag);
		byte[] module = new byte[2] { 1, 5 };
		byte[] module2 = new byte[2] { 2, 6 };
		m_message.AddButton(0, UIButtonBase.State.Normal, unitUI, 2, module);
		m_message.AddButton(0, UIButtonBase.State.Pressed, unitUI, 2, module2);
		m_message.SetButtonPosition(0, unitUI.GetModulePositionRect(0, 0, 5));
		byte[] module3 = new byte[2] { 0, 3 };
		byte[] module4 = new byte[2] { 2, 4 };
		m_message.AddButton(1, UIButtonBase.State.Normal, unitUI, 2, module3);
		m_message.AddButton(1, UIButtonBase.State.Pressed, unitUI, 2, module4);
		m_message.SetButtonPosition(1, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public void CreateQueryNoCancel(string msg, byte flag, byte eventID)
	{
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		m_message = new UIDialog(stateMgr, 2);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), unitUI, 1, flag);
		byte[] module1 = new byte[2] { 0, 3 };
		byte[] module2 = new byte[2] { 2, 4 };
		m_message.AddButton(1, UIButtonBase.State.Normal, unitUI, 2, module1);
		m_message.AddButton(1, UIButtonBase.State.Pressed, unitUI, 2, module2);
		m_message.SetButtonPosition(1, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public void CreateQueryUpload(string msg, byte flag, byte eventID)
	{
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		m_message = new UIDialog(stateMgr, 2);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), unitUI, 1, flag);
		byte[] module = new byte[2] { 0, 5 };
		byte[] module2 = new byte[2] { 2, 6 };
		m_message.AddButton(0, UIButtonBase.State.Normal, unitUI, 3, module);
		m_message.AddButton(0, UIButtonBase.State.Pressed, unitUI, 3, module2);
		m_message.SetButtonPosition(0, unitUI.GetModulePositionRect(0, 0, 5));
		byte[] module3 = new byte[2] { 1, 3 };
		byte[] module4 = new byte[2] { 2, 4 };
		m_message.AddButton(1, UIButtonBase.State.Normal, unitUI, 3, module3);
		m_message.AddButton(1, UIButtonBase.State.Pressed, unitUI, 3, module4);
		m_message.SetButtonPosition(1, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public void CreateQueryGuest(string msg, byte flag, byte eventID)
	{
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		m_message = new UIDialog(stateMgr, 2);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), unitUI, 1, flag);
		byte[] module = new byte[2] { 1, 5 };
		byte[] module2 = new byte[2] { 2, 6 };
		m_message.AddButton(0, UIButtonBase.State.Normal, unitUI, 4, module);
		m_message.AddButton(0, UIButtonBase.State.Pressed, unitUI, 4, module2);
		m_message.SetButtonPosition(0, unitUI.GetModulePositionRect(0, 0, 5));
		byte[] module3 = new byte[2] { 0, 3 };
		byte[] module4 = new byte[2] { 2, 4 };
		m_message.AddButton(1, UIButtonBase.State.Normal, unitUI, 4, module3);
		m_message.AddButton(1, UIButtonBase.State.Pressed, unitUI, 4, module4);
		m_message.SetButtonPosition(1, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public void CreateConfirm(string msg, byte flag, byte eventID)
	{
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		m_message = new UIDialog(stateMgr, 1);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		if (flag != 9)
		{
			m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), unitUI, 1, flag);
		}
		else
		{
			m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), ui, 0, 11);
		}
		byte[] module = new byte[2] { 0, 3 };
		byte[] module2 = new byte[2] { 2, 4 };
		m_message.AddButton(0, UIButtonBase.State.Normal, unitUI, 2, module);
		m_message.AddButton(0, UIButtonBase.State.Pressed, unitUI, 2, module2);
		m_message.SetButtonPosition(0, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public void CreateConfirmMovie(string msg, byte flag, byte eventID)
	{
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		m_message = new UIDialog(stateMgr, 2);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), unitUI, 1, 4);
		byte[] module = new byte[2] { 1, 5 };
		byte[] module2 = new byte[2] { 2, 6 };
		m_message.AddButton(0, UIButtonBase.State.Normal, unitUI, 2, module);
		m_message.AddButton(0, UIButtonBase.State.Pressed, unitUI, 2, module2);
		m_message.SetButtonPosition(0, unitUI.GetModulePositionRect(0, 0, 5));
		byte[] module3 = new byte[2] { 0, 3 };
		byte[] module4 = new byte[2] { 2, 4 };
		m_message.AddButton(1, UIButtonBase.State.Normal, unitUI, 2, module3);
		m_message.AddButton(1, UIButtonBase.State.Pressed, unitUI, 2, module4);
		m_message.SetButtonPosition(1, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public void CreateWeaponConfirm(string msg, int flag, byte eventID)
	{
		Debug.Log("flag =" + flag);
		DestoryMsg();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[23];
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		m_message = new UIDialog(stateMgr, 1);
		m_message.Create();
		m_eventID = eventID;
		m_message.Show();
		m_message.AddBGFrame(unitUI, 0, BACKGROUND_BEGIN_IMG, BACKGROUND_COUNT_IMG);
		if (flag != -1)
		{
			m_message.AddBGImage(unitUI.GetModulePositionRect(0, 0, 3), ui, 0, (byte)flag);
		}
		byte[] module = new byte[2] { 0, 3 };
		byte[] module2 = new byte[2] { 2, 4 };
		m_message.AddButton(0, UIButtonBase.State.Normal, unitUI, 2, module);
		m_message.AddButton(0, UIButtonBase.State.Pressed, unitUI, 2, module2);
		m_message.SetButtonPosition(0, unitUI.GetModulePositionRect(0, 0, 6));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 4);
		m_message.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		m_message.SetText("font2", msg, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		m_message.SetBlock(false);
		Add(m_message);
	}

	public bool IsVisiable()
	{
		return base.Visible;
	}

	public override void Hide()
	{
		stateMgr.m_UIManager.SetEnable(true);
		base.Visible = false;
		base.Enable = false;
		base.Hide();
	}

	public override void Show()
	{
		stateMgr.m_UIManager.SetEnable(false);
		base.Visible = true;
		base.Enable = true;
		base.Show();
	}

	public override void Draw()
	{
		base.Draw();
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
		if (control == m_message)
		{
			m_Parent.SendEvent(this, command, wparam, lparam);
		}
	}
}
