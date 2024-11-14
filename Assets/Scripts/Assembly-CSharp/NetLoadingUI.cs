using System;
using UnityEngine;

public class NetLoadingUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0
	}

	public UIStateManager stateMgr;

	protected NetworkManager networkMgr;

	private UIBlock m_block;

	private UIImage m_loadImg;

	private UIImage m_loadLightImg;

	private float rotate;

	private DateTime m_time;

	private int timeout;

	private static byte DEFAULT_TIMEOUT_SECOND = 5;

	private MessageBoxUI msgUI;

	public NetLoadingUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		timeout = DEFAULT_TIMEOUT_SECOND;
	}

	public void Close()
	{
		Clear();
	}

	public void Create()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		byte[] module = new byte[2] { 1, 2 };
		m_loadImg = new UIImage();
		m_loadImg.AddObject(ui, 1, module);
		m_loadImg.Rect = m_loadImg.GetObjectRect();
		m_loadLightImg = new UIImage();
		m_loadLightImg.AddObject(ui, 1, 3);
		m_loadLightImg.Rect = m_loadLightImg.GetObjectRect();
		Add(m_loadImg);
		Add(m_loadLightImg);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		string msg = UIConstant.GetMessage(22).Replace("[n]", "\n");
		msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_NET_TIMEOUT);
		msgUI.Hide();
		Add(msgUI);
		SetUIHandler(this);
	}

	public override void Update()
	{
		if (base.Visible)
		{
			if ((DateTime.Now - m_time).TotalSeconds >= (double)timeout && !msgUI.IsVisiable())
			{
				msgUI.Show();
			}
			rotate -= 0.2f;
			rotate %= 360f;
			m_loadLightImg.SetRotation(rotate);
		}
	}

	public override void Hide()
	{
		stateMgr.m_UIManager.SetEnable(true);
		base.Visible = false;
		base.Enable = false;
		base.Hide();
	}

	public void Show(int duration)
	{
		timeout = duration;
		m_time = DateTime.Now;
		stateMgr.m_UIManager.SetEnable(false);
		base.Visible = true;
		base.Enable = true;
		base.Show();
	}

	public bool IsVisiableMessage()
	{
		return msgUI.IsVisiable();
	}

	public void SetMessage(int textIndex, byte flag, byte eventId)
	{
		string msg = UIConstant.GetMessage(textIndex).Replace("[n]", "\n");
		msgUI.CreateConfirm(msg, flag, eventId);
	}

	public override void Draw()
	{
		base.Draw();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control != msgUI)
		{
			return;
		}
		int eventID = msgUI.GetEventID();
		if (eventID == MessageBoxUI.EVENT_NET_DISCONNECTION)
		{
			if (command == 9)
			{
				msgUI.Hide();
				Hide();
			}
		}
		else if (eventID == MessageBoxUI.EVENT_NET_TIMEOUT)
		{
			if (command == 9)
			{
				msgUI.Hide();
				Hide();
			}
		}
		else if (eventID == MessageBoxUI.EVENT_IAP_NET_TIMEOUT)
		{
			if (command == 9)
			{
				msgUI.Hide();
				Hide();
				stateMgr.m_UIManager.SetEnable(false);
			}
		}
		else if (eventID == MessageBoxUI.EVENT_NET_GAME_CENTER_TIMEOUT && command == 9)
		{
			msgUI.Hide();
			Hide();
			m_Parent.SendEvent(this, 0, 0f, 0f);
		}
	}
}
