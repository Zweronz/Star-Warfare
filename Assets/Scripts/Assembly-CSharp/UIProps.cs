using System;
using System.Collections.Generic;
using UnityEngine;

public class UIProps : UIPanelX, UIHandler
{
	public enum Command
	{
		Hide = 0,
		Click = 1,
		SelectIndex = 2
	}

	public class UIPropsIcon : UIPanelX
	{
		public UIImage m_background;

		public UIImage m_propsIcon;

		protected int m_TouchFingerId = -1;

		public new Rect Rect
		{
			get
			{
				return base.Rect;
			}
			set
			{
				base.Rect = value;
				m_background.Rect = value;
				m_propsIcon.Rect = value;
			}
		}

		public override void Draw()
		{
			m_background.Draw();
			m_propsIcon.Draw();
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_propsIcon != null)
			{
				m_propsIcon.Destory();
			}
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_propsIcon.SetParent(parent);
			SetParent(parent);
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
			m_propsIcon.SetClip(clip_rect);
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (touch.phase == TouchPhase.Began)
			{
				m_TouchFingerId = touch.fingerId;
			}
			else if (touch.phase == TouchPhase.Ended && touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
			{
				m_Parent.SendEvent(this, 1, 0f, 0f);
				m_TouchFingerId = -1;
				return true;
			}
			return false;
		}

		public bool TouchActive(UITouchInner touch, Rect active)
		{
			if (touch.phase == TouchPhase.Began)
			{
				m_TouchFingerId = touch.fingerId;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				bool flag = touch.position.x >= active.xMin && touch.position.x < active.xMax && touch.position.y >= active.yMin && touch.position.y < active.yMax;
				if (touch.fingerId == m_TouchFingerId && flag)
				{
					return true;
				}
				m_TouchFingerId = -1;
			}
			return false;
		}
	}

	public const byte STATE_WORKABLE = 0;

	public const byte STATE_STRETCHING = 1;

	public const byte STATE_RETRACTING = 2;

	public const byte STATE_UNWORKABLE = 3;

	public List<UIPropsIcon> m_propsLst = new List<UIPropsIcon>();

	public byte m_state;

	protected int m_IconWidth;

	protected int m_IconHeight;

	private Rect m_activeRect;

	private DateTime time;

	private float m_idleTimer = 3f;

	public void CreateBG(UnitUI ui)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, 0, 16);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		Rect rct = new Rect(modulePositionRect.x + (float)m_IconWidth * 0.5f - 2.5f * (float)m_IconWidth, modulePositionRect.y, 5 * m_IconWidth, modulePositionRect.height);
		m_activeRect = UIConstant.GetRectForScreenAdaptived(rct);
		m_idleTimer = 3f;
		SetUIHandler(this);
		Show();
		SetState(2);
	}

	public void CreateBGForIPad(UnitUI ui)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, 3, 16);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		Rect rct = new Rect(modulePositionRect.x + (float)m_IconWidth * 0.5f - 2.5f * (float)m_IconWidth, modulePositionRect.y, 5 * m_IconWidth, modulePositionRect.height);
		m_activeRect = UIConstant.GetRectForScreenAdaptived(rct);
		m_idleTimer = 3f;
		SetUIHandler(this);
		Show();
		SetState(2);
	}

	public new void Clear()
	{
		m_propsLst.Clear();
		base.Clear();
	}

	public void SetState(byte state)
	{
		m_state = state;
	}

	public virtual void Add(UIPropsIcon control)
	{
		control.SetParentEx(this);
		m_propsLst.Add(control);
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Draw()
	{
		foreach (UIPropsIcon item in m_propsLst)
		{
			item.Draw();
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_propsLst == null)
		{
			return;
		}
		foreach (UIPropsIcon item in m_propsLst)
		{
			if (item != null)
			{
				item.Destory();
			}
		}
		m_propsLst.Clear();
	}

	public override void Update()
	{
		switch (m_state)
		{
		case 0:
			if ((DateTime.Now - time).TotalSeconds > (double)m_idleTimer)
			{
				SetState(2);
			}
			break;
		case 3:
			break;
		case 2:
		{
			bool flag2 = true;
			foreach (UIPropsIcon item in m_propsLst)
			{
				if (item.Rect.y > (float)(30 - m_IconHeight))
				{
					item.Rect = new Rect(item.Rect.x, item.Rect.y - 10f, item.Rect.width, item.Rect.height);
					flag2 = false;
				}
				if (item.Rect.y <= (float)(30 - m_IconHeight))
				{
					item.Rect = new Rect(item.Rect.x, 30 - m_IconHeight, item.Rect.width, item.Rect.height);
				}
				item.Visible = true;
			}
			if (flag2)
			{
				SetState(3);
			}
			break;
		}
		case 1:
		{
			bool flag = true;
			foreach (UIPropsIcon item2 in m_propsLst)
			{
				if (item2.Rect.y < 0f)
				{
					item2.Rect = new Rect(item2.Rect.x, item2.Rect.y + 10f, item2.Rect.width, item2.Rect.height);
					flag = false;
				}
				if (item2.Rect.y >= 0f)
				{
					item2.Rect = new Rect(item2.Rect.x, 0f, item2.Rect.width, item2.Rect.height);
				}
				item2.Visible = true;
			}
			if (flag)
			{
				time = DateTime.Now;
				SetState(0);
			}
			break;
		}
		}
	}

	public void SetGridTexture(int gridID, UnitUI ui, int frame, int module)
	{
		m_propsLst[gridID].m_propsIcon.Visible = true;
		m_propsLst[gridID].m_propsIcon.SetTexture(ui, frame, module);
		m_propsLst[gridID].m_propsIcon.Rect = m_propsLst[gridID].m_background.Rect;
	}

	public void ClearGridTexture(int gridID)
	{
		m_propsLst[gridID].m_propsIcon.Visible = false;
		m_propsLst[gridID].m_propsIcon.Free();
		m_propsLst[gridID].m_propsIcon.Rect = m_propsLst[gridID].m_background.Rect;
	}

	public void SetIdleTimer(float timer)
	{
		m_idleTimer = timer;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_state == 0)
		{
			foreach (UIPropsIcon item in m_propsLst)
			{
				if (item.Enable && item.HandleInput(touch))
				{
					return true;
				}
			}
		}
		else if (m_state == 3)
		{
			foreach (UIPropsIcon item2 in m_propsLst)
			{
				if (item2.Enable && item2.TouchActive(touch, m_activeRect))
				{
					SetState(1);
					break;
				}
			}
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_state != 0)
		{
			return;
		}
		for (int i = 0; i < m_propsLst.Count; i++)
		{
			if (m_propsLst[i] == control)
			{
				m_Parent.SendEvent(this, 1, m_propsLst[i].Id, i);
				break;
			}
		}
	}
}
