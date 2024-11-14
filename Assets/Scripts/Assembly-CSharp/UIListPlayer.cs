using System.Collections.Generic;
using UnityEngine;

public class UIListPlayer : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UIPlayerIcon : UIPanelX
	{
		public UIImage m_background;

		public UIImage m_rankIcon;

		public string m_userName = string.Empty;

		public Rect m_userNamePos;

		public UIImage m_flagIcon;

		protected int m_TouchFingerId = -1;

		public override void Draw()
		{
			m_background.Draw();
			m_rankIcon.Draw();
			m_flagIcon.Draw();
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_rankIcon.SetParent(parent);
			m_flagIcon.SetParent(parent);
			SetParent(parent);
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (touch.phase == TouchPhase.Began)
			{
				if (PtInRect(touch.position))
				{
					m_TouchFingerId = touch.fingerId;
				}
			}
			else if (touch.phase == TouchPhase.Ended && touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
			{
				m_Parent.SendEvent(this, 0, 0f, 0f);
				m_TouchFingerId = -1;
				return true;
			}
			return false;
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_rankIcon != null)
			{
				m_rankIcon.Destory();
			}
			if (m_flagIcon != null)
			{
				m_flagIcon.Destory();
			}
		}
	}

	public Rect m_showRect;

	public byte m_selectIndex;

	public List<UIPlayerIcon> m_playerLst = new List<UIPlayerIcon>();

	public UIImage m_decorate;

	public void Create(UnitUI ui)
	{
		m_decorate = new UIImage();
		m_decorate.SetParent(this);
		SetUIHandler(this);
		base.Show();
	}

	public virtual void Add(UIPlayerIcon player)
	{
		player.SetParentEx(this);
		m_playerLst.Add(player);
	}

	public new void Clear()
	{
		m_playerLst.Clear();
	}

	public void SetSelection(int index)
	{
		m_selectIndex = (byte)index;
	}

	public void SetPlayerTexture(int listID, UnitUI ui, RoomPlayer player)
	{
		UnitUI ui2 = Res2DManager.GetInstance().vUI[27];
		UnitUI ui3 = Res2DManager.GetInstance().vUI[14];
		float num = 20f;
		float num2 = 50f;
		float num3 = 70f;
		float width = m_playerLst[listID].m_background.Rect.width - num3 - num2;
		m_playerLst[listID].m_rankIcon.Visible = true;
		m_playerLst[listID].m_rankIcon.SetTexture(ui3, 0, 21 + player.RankID);
		m_playerLst[listID].m_rankIcon.SetSize(new Vector2(m_playerLst[listID].m_background.Rect.height - 10f, m_playerLst[listID].m_background.Rect.height - 10f));
		m_playerLst[listID].m_rankIcon.Rect = new Rect(m_playerLst[listID].m_background.Rect.x + m_playerLst[listID].m_background.Rect.width - num3 - 10f, m_playerLst[listID].m_background.Rect.y, num3, m_playerLst[listID].m_background.Rect.height);
		m_playerLst[listID].m_userName = player.getPlayerName();
		m_playerLst[listID].m_userNamePos = UIConstant.GetRectForScreenAdaptived(new Rect(m_playerLst[listID].m_background.Rect.x + num2 + num + 10f, UIConstant.ScreenLocalHeight - m_playerLst[listID].m_background.Rect.y - m_playerLst[listID].m_background.Rect.height, width, m_playerLst[listID].m_background.Rect.height));
		m_playerLst[listID].m_flagIcon.SetTexture(ui2, 0, listID);
		m_playerLst[listID].m_flagIcon.Rect = new Rect(m_playerLst[listID].m_background.Rect.x + num, m_playerLst[listID].m_background.Rect.y, num2, m_playerLst[listID].m_background.Rect.height);
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public override void Draw()
	{
		for (int i = 0; i < m_playerLst.Count; i++)
		{
			m_playerLst[i].Draw();
		}
		m_decorate.Draw();
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_playerLst != null)
		{
			foreach (UIPlayerIcon item in m_playerLst)
			{
				if (item != null)
				{
					item.Destory();
				}
			}
			m_playerLst.Clear();
		}
		if (m_decorate != null)
		{
			m_decorate.Destory();
		}
	}

	public override void Update()
	{
	}

	public override bool HandleInput(UITouchInner touch)
	{
		foreach (UIPlayerIcon item in m_playerLst)
		{
			if (item.HandleInput(touch))
			{
				return true;
			}
		}
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		m_Parent.SendEvent(this, 0, control.Id, lparam);
	}
}
