using System.Collections.Generic;
using UnityEngine;

public class UISliderNetStage : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UIStageIcon : UIPanelX
	{
		public UIImage m_Lock;

		public bool m_bLock;

		public UIImage m_background;

		public float m_size;

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
				m_Lock.Rect = value;
				m_background.Rect = value;
			}
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (touch.phase == TouchPhase.Began)
			{
				m_TouchFingerId = touch.fingerId;
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
			if (m_Lock != null)
			{
				m_Lock.Destory();
			}
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_Lock.SetParent(parent);
			SetParent(parent);
		}

		public override void Draw()
		{
			m_background.Draw();
			if (m_bLock)
			{
				m_Lock.Draw();
			}
		}
	}

	public List<UIStageIcon> m_StageIcons = new List<UIStageIcon>();

	public List<UIStageIcon> m_drawStageIcons = new List<UIStageIcon>();

	public UIScroller m_scroller = new UIScroller();

	public static byte[] ARROW_LEFT_IMG = new byte[2] { 16, 17 };

	public static byte[] ARROW_RIGHT_IMG = new byte[2] { 18, 19 };

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected int m_LockIconWidth;

	protected int m_LockIconHeight;

	public void Create(UnitUI ui)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, 0, 45);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		Rect modulePositionRect2 = ui.GetModulePositionRect(0, 0, 96);
		m_LockIconWidth = (int)(modulePositionRect2.width - 25f);
		m_LockIconHeight = (int)(modulePositionRect2.height - 25f);
		SetUIHandler(this);
		base.Show();
	}

	public new void Clear()
	{
		base.Clear();
		m_StageIcons.Clear();
		m_drawStageIcons.Clear();
	}

	public virtual void Add(UIStageIcon stage)
	{
		stage.SetParentEx(this);
		m_StageIcons.Add(stage);
	}

	public void SetClipRect(Rect clip)
	{
		m_showRect = clip;
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = true;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Vertical, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
	}

	public bool GetScrollerState()
	{
		return m_scroller.Moving;
	}

	public override void Draw()
	{
		foreach (UIStageIcon drawStageIcon in m_drawStageIcons)
		{
			drawStageIcon.Draw();
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_StageIcons != null)
		{
			foreach (UIStageIcon stageIcon in m_StageIcons)
			{
				if (stageIcon != null)
				{
					stageIcon.Destory();
				}
			}
			m_StageIcons.Clear();
		}
		if (m_drawStageIcons == null)
		{
			return;
		}
		foreach (UIStageIcon drawStageIcon in m_drawStageIcons)
		{
			if (drawStageIcon != null)
			{
				drawStageIcon.Destory();
			}
		}
		m_drawStageIcons.Clear();
	}

	public override void Update()
	{
		m_drawStageIcons.Clear();
		m_scroller.Update();
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_StageIcons.Count;
		for (int i = 0; i < count; i++)
		{
			UIStageIcon uIStageIcon = m_StageIcons[i];
			UIImage background = uIStageIcon.m_background;
			UIImage @lock = uIStageIcon.m_Lock;
			num = num4 + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - (float)m_IconHeight * 0.5f;
			if (num + (float)m_IconHeight < m_showRect.y)
			{
				num = m_showRect.y + (float)count * m_scroller.Spacing - (m_showRect.y - num);
			}
			else if (num + (float)m_IconHeight > m_showRect.y + (float)count * m_scroller.Spacing)
			{
				float num5 = num + (float)m_IconHeight - (m_showRect.y + (float)count * m_scroller.Spacing);
				num = m_showRect.y - (float)m_IconHeight + num5;
			}
			Rect rect = background.Rect;
			uIStageIcon.Rect = new Rect(m_showRect.x, num, m_IconWidth, m_IconHeight);
			float num6 = num + (float)m_IconHeight * 0.5f;
			float num7 = Mathf.Abs(num4 - num6);
			float num8 = 1f - num7 / m_showRect.height;
			if (num8 < 0.2f)
			{
				num8 = 0.2f;
			}
			int num9 = (int)(background.Rect.x + background.Rect.width * 0.5f);
			int num10 = (int)(background.Rect.y + background.Rect.height * 0.5f);
			if (num8 > 0f)
			{
				if (num8 > 1f)
				{
					num8 = 1f;
				}
				Color color = new Color(1f * num8, 1f * num8, 1f * num8, 1f);
				background.SetColor(color);
				background.SetSize(new Vector2((float)m_IconWidth * num8, (float)m_IconHeight * num8));
				if (uIStageIcon.m_bLock)
				{
					int num11 = (int)((float)m_LockIconWidth * num8);
					int num12 = (int)((float)m_LockIconHeight * num8);
					@lock.SetSize(new Vector2(num11, num12));
				}
			}
			if (uIStageIcon.m_bLock)
			{
				Color color2 = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 1f);
				background.SetColor(color2);
			}
			background.SetClip(m_showRect);
			@lock.SetClip(m_showRect);
			uIStageIcon.m_size = num8;
			Sort(uIStageIcon);
		}
	}

	private void SetEnable(int index, bool enable)
	{
		UIStageIcon uIStageIcon = m_StageIcons[index];
		uIStageIcon.Enable = enable;
	}

	public void SetAllEnable(bool enable)
	{
		foreach (UIStageIcon stageIcon in m_StageIcons)
		{
			stageIcon.Enable = enable;
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		m_scroller.HandleInput(touch);
		if (!m_scroller.Moving)
		{
			foreach (UIStageIcon stageIcon in m_StageIcons)
			{
				if (stageIcon.Enable && stageIcon.HandleInput(touch))
				{
					return true;
				}
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
		if (control == m_scroller)
		{
			if (command == 3)
			{
				wparam %= (float)m_StageIcons.Count;
				m_Parent.SendEvent(this, 1, wparam, lparam);
			}
		}
		else
		{
			m_Parent.SendEvent(this, 0, control.Id, lparam);
		}
	}

	public bool Sort(UIStageIcon icon)
	{
		if (m_drawStageIcons.Count == 0)
		{
			m_drawStageIcons.Add(icon);
		}
		else
		{
			float size = icon.m_size;
			int num = 0;
			int num2 = m_drawStageIcons.Count - 1;
			int num3 = (num + num2) / 2;
			if (size <= m_drawStageIcons[num].m_size)
			{
				m_drawStageIcons.Insert(num, icon);
			}
			else if (size >= m_drawStageIcons[num2].m_size)
			{
				m_drawStageIcons.Insert(num2 + 1, icon);
			}
			else
			{
				while (num2 - num > 1)
				{
					float size2 = m_drawStageIcons[num3].m_size;
					if (size == size2)
					{
						num = num3;
						break;
					}
					if (size > size2)
					{
						num = num3;
					}
					else
					{
						num2 = num3;
					}
					num3 = (num + num2) / 2;
				}
				m_drawStageIcons.Insert(num + 1, icon);
			}
		}
		return true;
	}
}
