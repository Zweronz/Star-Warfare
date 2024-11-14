using System.Collections.Generic;
using UnityEngine;

public class UIMarquee : UIPanelX, UIHandler
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
	}

	public List<UINumIcon> m_numsLst = new List<UINumIcon>();

	protected int m_IconWidth;

	protected int m_IconHeight;

	public float m_spacing;

	protected float m_min;

	protected float m_max;

	protected float m_marqueePos;

	protected bool m_bStop;

	protected float m_velocity;

	protected float m_velocityMin;

	protected float m_velocityMax;

	protected float m_acceleration;

	protected float m_deltaTime;

	protected Rect m_showRect;

	protected int m_selectIndex;

	protected int m_destIndex;

	public void Create(UnitUI ui)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, 1, 0);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		m_deltaTime = 0.016f;
		SetVelocity(0f, -20f, -1f);
		m_destIndex = -1;
		m_bStop = true;
		SetUIHandler(this);
		Show();
	}

	public new void Clear()
	{
		m_numsLst.Clear();
		base.Clear();
	}

	public virtual void Add(UINumIcon control)
	{
		control.SetParentEx(this);
		m_numsLst.Add(control);
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
		foreach (UINumIcon item in m_numsLst)
		{
			item.Draw();
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_numsLst == null)
		{
			return;
		}
		foreach (UINumIcon item in m_numsLst)
		{
			if (item != null)
			{
				item.Destory();
			}
		}
		m_numsLst.Clear();
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public int GetSelection()
	{
		return m_selectIndex;
	}

	public void SetVelocity(float velocity, float min, float max)
	{
		m_velocity = velocity;
		m_velocityMin = min;
		m_velocityMax = max;
	}

	public float GetVelocity()
	{
		return m_velocity;
	}

	public float GetMarqueePos()
	{
		return m_marqueePos;
	}

	public float GetMarqueeMaxPos()
	{
		return m_max;
	}

	public void SetSelection(int index)
	{
		m_marqueePos = (float)index * m_spacing;
		m_selectIndex = index;
	}

	public void SetMarquee(float min, float max, float spacing)
	{
		m_spacing = spacing;
		m_min = min;
		m_max = max;
	}

	public void Play(float velocity, float acceleration, int dest)
	{
		m_bStop = false;
		m_destIndex = dest;
		m_velocity = velocity;
		m_acceleration = acceleration;
	}

	public void Stop()
	{
		m_bStop = true;
		m_velocity = 0f;
		m_acceleration = 0f;
	}

	public override void Update()
	{
		m_velocity += m_deltaTime * m_acceleration;
		if (m_velocity > m_velocityMax)
		{
			m_velocity = m_velocityMax;
		}
		else if (m_velocity < m_velocityMin)
		{
			m_velocity = m_velocityMin;
		}
		if (m_bStop)
		{
			m_velocity = 0f;
		}
		m_marqueePos += (int)m_velocity;
		if (m_marqueePos < m_min)
		{
			m_marqueePos = m_max + (m_marqueePos - m_min);
		}
		else if (m_marqueePos > m_max)
		{
			m_marqueePos %= m_max;
		}
		m_marqueePos = Mathf.Clamp(m_marqueePos, m_min, m_max);
		float num = 0f;
		float num2 = m_showRect.x + m_showRect.width * 0.5f;
		float num3 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_numsLst.Count;
		for (int i = 0; i < count; i++)
		{
			UINumIcon uINumIcon = m_numsLst[i];
			num = num3 + m_spacing * (float)i - m_marqueePos - (float)m_IconHeight * 0.5f;
			if (num + (float)m_IconHeight < m_showRect.y)
			{
				num = m_showRect.y + (float)count * m_spacing - (m_showRect.y - num);
			}
			else if (num + (float)m_IconHeight > m_showRect.y + (float)count * m_spacing)
			{
				float num4 = num + (float)m_IconHeight - (m_showRect.y + (float)count * m_spacing);
				num = m_showRect.y - (float)m_IconHeight + num4;
			}
			uINumIcon.Rect = new Rect(m_showRect.x, num, m_showRect.width, m_IconHeight);
			uINumIcon.SetClip(m_showRect);
		}
		int num5 = (int)(m_marqueePos / m_spacing + 0.5f);
		if (num5 == m_destIndex)
		{
			float num6 = (float)num5 * m_spacing;
			if (Mathf.Abs(m_marqueePos - num6) <= Mathf.Abs(m_velocity))
			{
				m_marqueePos = num6;
				m_velocity = 0f;
				m_acceleration = 0f;
				m_selectIndex = num5;
				m_bStop = true;
			}
		}
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
	}
}
