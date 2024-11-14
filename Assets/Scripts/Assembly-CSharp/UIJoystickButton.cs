using System;
using UnityEngine;

public class UIJoystickButton : UIButtonBase
{
	public enum Command
	{
		Down = 0,
		Move = 1,
		Up = 2
	}

	protected int m_FingerId;

	protected Vector2 m_Center;

	protected float m_Direction;

	protected float m_Distance;

	protected float m_MinDistance;

	protected float m_MaxDistance;

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
			UpdatePosition();
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
			UpdatePosition();
		}
	}

	public float Distance
	{
		get
		{
			return m_Distance;
		}
		set
		{
			m_Distance = value;
			UpdatePosition();
		}
	}

	public float MinDistance
	{
		get
		{
			return m_MinDistance;
		}
		set
		{
			m_MinDistance = value;
		}
	}

	public float MaxDistance
	{
		get
		{
			return m_MaxDistance;
		}
		set
		{
			m_MaxDistance = value;
		}
	}

	public UIJoystickButton()
	{
		m_FingerId = -1;
		m_Center = new Vector2(0f, 0f);
		m_Direction = 0f;
		m_Distance = 0f;
		m_MinDistance = -1f;
		m_MaxDistance = -1f;
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
				float num = touch.position.x - m_Center.x;
				float num2 = touch.position.y - m_Center.y;
				m_Distance = Mathf.Sqrt(num * num + num2 * num2);
				if (m_MinDistance >= 0f && m_Distance < m_MinDistance)
				{
					m_Distance = m_MinDistance;
				}
				if (m_MaxDistance >= 0f && m_Distance > m_MaxDistance)
				{
					m_Distance = m_MaxDistance;
				}
				if (num2 >= 0f)
				{
					m_Direction = Mathf.Atan2(num2, num);
				}
				else
				{
					m_Direction = Mathf.Atan2(num2, num) + (float)Math.PI * 2f;
				}
				UpdatePosition();
				m_Parent.SendEvent(this, 1, m_Direction, m_Distance);
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				m_State = State.Normal;
				m_FingerId = -1;
				m_Distance = 0f;
				UpdatePosition();
				m_Parent.SendEvent(this, 2, 0f, 0f);
			}
			return true;
		}
		return false;
	}

	private void UpdatePosition()
	{
		if (m_Distance == 0f)
		{
			SetSpritePosition(0, m_Center);
			SetSpritePosition(1, m_Center);
			SetSpritePosition(2, m_Center);
		}
		else
		{
			Vector2 position = new Vector2(m_Center.x + m_Distance * Mathf.Cos(m_Direction), m_Center.y + m_Distance * Mathf.Sin(m_Direction));
			SetSpritePosition(0, position);
			SetSpritePosition(1, position);
			SetSpritePosition(2, position);
		}
	}
}
