using System;
using UnityEngine;

public class UIRotate : UIControl
{
	public enum Command
	{
		Begin = 0,
		Rotate = 1,
		End = 2
	}

	protected int m_FingerId;

	protected float m_TouchDirection;

	protected Vector2 m_Center;

	protected bool m_Rotate;

	protected float m_MinRotate;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			m_Center.x = value.x + value.width / 2f;
			m_Center.y = value.y + value.height / 2f;
		}
	}

	public float MinRotate
	{
		get
		{
			return m_MinRotate;
		}
		set
		{
			m_MinRotate = value;
		}
	}

	public UIRotate()
	{
		m_FingerId = -1;
		m_TouchDirection = 0f;
		m_Center = new Vector2(0f, 0f);
		m_Rotate = false;
		m_MinRotate = 0f;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				float x = touch.position.x - m_Center.x;
				float num = touch.position.y - m_Center.y;
				if (num >= 0f)
				{
					m_TouchDirection = Mathf.Atan2(num, x);
				}
				else
				{
					m_TouchDirection = Mathf.Atan2(num, x) + (float)Math.PI * 2f;
				}
				m_Rotate = false;
			}
			return false;
		}
		if (touch.fingerId != m_FingerId)
		{
			return false;
		}
		if (!PtInRect(touch.position))
		{
			return false;
		}
		if (touch.phase == TouchPhase.Moved)
		{
			float x2 = touch.position.x - m_Center.x;
			float num2 = touch.position.y - m_Center.y;
			float num3 = ((!(num2 >= 0f)) ? (Mathf.Atan2(num2, x2) + (float)Math.PI * 2f) : Mathf.Atan2(num2, x2));
			float num4 = num3 - m_TouchDirection;
			if (num4 < 0f)
			{
				num4 += (float)Math.PI * 2f;
			}
			if (num4 > (float)Math.PI)
			{
				num4 -= (float)Math.PI * 2f;
			}
			if (m_Rotate)
			{
				m_TouchDirection = num3;
				m_Parent.SendEvent(this, 1, num4, 0f);
			}
			else
			{
				float num5 = ((!(num4 >= 0f)) ? (0f - num4) : num4);
				if (!(num5 > m_MinRotate))
				{
					return false;
				}
				m_TouchDirection = num3;
				m_Rotate = true;
				m_Parent.SendEvent(this, 0, 0f, 0f);
				m_Parent.SendEvent(this, 1, num4, 0f);
			}
			return true;
		}
		if (touch.phase == TouchPhase.Ended)
		{
			bool rotate = m_Rotate;
			m_FingerId = -1;
			m_TouchDirection = 0f;
			m_Rotate = false;
			if (rotate)
			{
				m_Parent.SendEvent(this, 2, 0f, 0f);
				return true;
			}
			return false;
		}
		return false;
	}
}
