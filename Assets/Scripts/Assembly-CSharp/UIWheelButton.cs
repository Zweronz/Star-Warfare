using System;
using UnityEngine;

public class UIWheelButton : UIButtonBase
{
	public enum Command
	{
		Down = 0,
		Rotate = 1,
		Up = 2
	}

	protected int m_FingerId;

	protected Vector2 m_Center;

	protected float m_Direction;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			SetSpritePosition(0, position);
			SetSpritePosition(1, position);
			SetSpritePosition(2, position);
			m_Center.x = value.x + value.width / 2f;
			m_Center.y = value.y + value.height / 2f;
		}
	}

	public float Direction
	{
		get
		{
			return m_Direction;
		}
		set
		{
			m_Direction = value;
			SetSpriteRotation(0, m_Direction);
			SetSpriteRotation(1, m_Direction);
			SetSpriteRotation(2, m_Direction);
		}
	}

	public UIWheelButton()
	{
		m_FingerId = -1;
		m_Center = new Vector2(0f, 0f);
		m_Direction = 0f;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_State = State.Pressed;
				m_FingerId = touch.fingerId;
				m_Parent.SendEvent(this, 0, 0f, 0f);
				return true;
			}
			return false;
		}
		if (touch.fingerId == m_FingerId)
		{
			if (touch.phase == TouchPhase.Moved)
			{
				float x = touch.position.x - m_Center.x;
				float num = touch.position.y - m_Center.y;
				if (num >= 0f)
				{
					Direction = Mathf.Atan2(num, x);
				}
				else
				{
					Direction = Mathf.Atan2(num, x) + (float)Math.PI * 2f;
				}
				m_Parent.SendEvent(this, 1, m_Direction, 0f);
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				m_State = State.Normal;
				m_FingerId = -1;
				m_Parent.SendEvent(this, 2, 0f, 0f);
			}
			return true;
		}
		return false;
	}
}
