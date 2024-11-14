using System.Collections.Generic;
using UnityEngine;

public class UISliderNum : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UINumIcon : UIPanelX
	{
		public UIImage m_background;

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
			}
		}

		public override void Draw()
		{
			m_background.Draw();
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			SetParent(parent);
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
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
	}

	public List<UINumIcon> m_numIcons = new List<UINumIcon>();

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected int m_maxPos;

	protected int m_minPos;

	public void Create(UnitUI ui, int frame, int module)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, frame, module);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		m_minPos = 0;
		m_maxPos = 10000;
		SetUIHandler(this);
		base.Show();
	}

	public void SetRangePos(int min, int max)
	{
		m_minPos = min;
		m_maxPos = max;
	}

	public virtual void Add(UINumIcon num)
	{
		num.SetParentEx(this);
		m_numIcons.Add(num);
	}

	public new void Clear()
	{
		base.Clear();
		m_numIcons.Clear();
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public void SetScroller(float min, float max, float spacing, Rect rct, bool loop)
	{
		m_scroller.Loop = loop;
		m_scroller.NegativeDir = false;
		m_scroller.LastVelocity = new Vector2(5f, 5f);
		m_scroller.DeltaTime = 0.016f;
		m_scroller.MinSpeed = 2;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Vertical, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		SetEnable(index, true);
	}

	public int GetSelectedIndex()
	{
		return (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
	}

	public bool GetScrollerState()
	{
		return m_scroller.Moving;
	}

	public override void Draw()
	{
		if (!base.Visible)
		{
			return;
		}
		foreach (UINumIcon numIcon in m_numIcons)
		{
			if (numIcon.Visible)
			{
				numIcon.Draw();
			}
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_numIcons == null)
		{
			return;
		}
		foreach (UINumIcon numIcon in m_numIcons)
		{
			if (numIcon != null)
			{
				numIcon.Destory();
			}
		}
		m_numIcons.Clear();
	}

	public override void Update()
	{
		m_scroller.Update();
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_numIcons.Count;
		float num5 = 0f;
		for (int i = 0; i < count; i++)
		{
			UINumIcon uINumIcon = m_numIcons[i];
			num = num4 - m_scroller.Spacing * (float)i + m_scroller.ScrollerPos - (float)m_IconHeight * 0.5f;
			if (num + (float)m_IconHeight < m_showRect.y)
			{
				num = m_showRect.y + (float)count * m_scroller.Spacing - (m_showRect.y - num);
			}
			else if (num + (float)m_IconHeight > m_showRect.y + (float)count * m_scroller.Spacing)
			{
				float num6 = num + (float)m_IconHeight - (m_showRect.y + (float)count * m_scroller.Spacing);
				num = m_showRect.y - (float)m_IconHeight + num6;
			}
			float num7 = num + (float)m_IconHeight * 0.5f;
			float num8 = Mathf.Abs(num4 - num7);
			uINumIcon.Visible = true;
			if (num8 > m_showRect.height)
			{
				uINumIcon.Visible = false;
			}
			else
			{
				uINumIcon.Rect = new Rect(uINumIcon.Rect.x, num, m_IconWidth, m_IconHeight);
			}
			uINumIcon.SetClip(m_showRect);
		}
	}

	private void SetEnable(int index, bool enable)
	{
		UINumIcon uINumIcon = m_numIcons[index];
		uINumIcon.Enable = enable;
	}

	public void SetAllEnable(bool enable)
	{
		foreach (UINumIcon numIcon in m_numIcons)
		{
			numIcon.Enable = enable;
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_scroller.HandleInput(touch))
		{
			return true;
		}
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_scroller)
		{
			switch (command)
			{
			case 3:
				wparam %= (float)m_numIcons.Count;
				m_Parent.SendEvent(this, 1, wparam, lparam);
				SetAllEnable(false);
				SetEnable((int)wparam, true);
				break;
			case 4:
			{
				int num = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
				if (m_scroller.Loop)
				{
					num++;
					num %= m_numIcons.Count;
					SetAllEnable(false);
					SetSelection(num);
				}
				else
				{
					num++;
					if (num >= m_numIcons.Count - 1)
					{
						num = m_numIcons.Count - 1;
					}
					SetAllEnable(false);
					SetSelection(num);
				}
				m_Parent.SendEvent(this, 0, num, lparam);
				break;
			}
			case 1:
				m_scroller.ScrollerPos = Mathf.Clamp(m_scroller.ScrollerPos, m_minPos, m_maxPos);
				break;
			}
		}
		else
		{
			m_Parent.SendEvent(this, 0, control.Id, lparam);
		}
	}
}
