using UnityEngine;

public class UIGift : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0
	}

	public UIStateManager stateMgr;

	protected NetworkManager networkMgr;

	private static byte GIFT_BEGIN_IMG = 1;

	private static byte GIFT_COUNT_IMG = 9;

	private UIBlock m_block;

	private UIImage m_shadowImg;

	private UIImage m_giftImg;

	private UINumeric[] m_rewards;

	private UIImage[] m_rewardLabels;

	private UIClickButton m_confirm;

	private byte m_currentDay;

	private static byte[] REWARD_LOCK_IMG = new byte[2] { 4, 5 };

	private static byte[] REWARD_UNLOCK_IMG = new byte[2] { 2, 3 };

	private static byte[] REWARD_SELECT_IMG = new byte[2] { 0, 1 };

	private static byte[] CONFIRM_NORMAL = new byte[2] { 17, 18 };

	private static byte[] CONFIRM_PRESSED = new byte[2] { 15, 16 };

	public UIGift(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Close()
	{
		Clear();
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[24];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		m_giftImg = new UIImage();
		m_giftImg.AddObject(unitUI, 0, GIFT_BEGIN_IMG, GIFT_COUNT_IMG);
		m_giftImg.Rect = m_giftImg.GetObjectRect();
		Add(m_giftImg);
		m_currentDay = GameApp.GetInstance().GetUserState().GetTimeSpan();
		bool flag = false;
		m_rewardLabels = new UIImage[5];
		for (int i = 0; i < m_rewardLabels.Length; i++)
		{
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 10 + i);
			m_rewardLabels[i] = new UIImage();
			if (i < m_currentDay)
			{
				m_rewardLabels[i].AddObject(unitUI, i + 1, REWARD_UNLOCK_IMG);
			}
			else if (i == m_currentDay)
			{
				m_rewardLabels[i].AddObject(unitUI, i + 1, REWARD_SELECT_IMG);
			}
			else
			{
				m_rewardLabels[i].AddObject(unitUI, i + 1, REWARD_LOCK_IMG);
			}
			m_rewardLabels[i].Rect = modulePositionRect;
			Add(m_rewardLabels[i]);
		}
		m_rewards = new UINumeric[5];
		for (int j = 0; j < m_rewards.Length; j++)
		{
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 19 + j);
			m_rewards[j] = new UINumeric();
			m_rewards[j].AlignStyle = UINumeric.enAlignStyle.center;
			m_rewards[j].SpacingOffsetX = -5f;
			if (j < 3)
			{
				m_rewards[j].SetNumeric(ui, 1, "$" + UIConstant.FormatNum(UIConstant.GIFT_CASH[j]));
			}
			else
			{
				m_rewards[j].SetNumeric(ui, 1, "#" + UIConstant.FormatNum(UIConstant.GIFT_CASH[j]));
			}
			m_rewards[j].Rect = modulePositionRect;
			Add(m_rewards[j]);
		}
		m_confirm = new UIClickButton();
		m_confirm.AddObject(UIButtonBase.State.Normal, unitUI, 0, CONFIRM_NORMAL);
		m_confirm.AddObject(UIButtonBase.State.Pressed, unitUI, 0, CONFIRM_PRESSED);
		m_confirm.Rect = m_confirm.GetObjectRect(UIButtonBase.State.Normal);
		Add(m_confirm);
		SetUIHandler(this);
	}

	public override void Hide()
	{
		base.Visible = false;
		base.Enable = false;
		base.Hide();
	}

	public void Show(int duration)
	{
		base.Visible = true;
		base.Enable = true;
		base.Show();
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_shadowImg != null)
		{
			m_shadowImg.Destory();
		}
		if (m_giftImg != null)
		{
			m_giftImg.Destory();
		}
		if (m_confirm != null)
		{
			m_confirm.Destory();
		}
		if (m_rewards != null)
		{
			UINumeric[] rewards = m_rewards;
			foreach (UINumeric uINumeric in rewards)
			{
				if (uINumeric != null)
				{
					uINumeric.Destory();
				}
			}
		}
		if (m_rewardLabels == null)
		{
			return;
		}
		UIImage[] rewardLabels = m_rewardLabels;
		foreach (UIImage uIImage in rewardLabels)
		{
			if (uIImage != null)
			{
				uIImage.Destory();
			}
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_confirm)
		{
			m_Parent.SendEvent(this, 0, (int)m_currentDay, lparam);
		}
	}
}
