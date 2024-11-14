using UnityEngine;

public class UIPushButton : UIButtonBase
{
	public enum Command
	{
		Down = 0,
		Up = 1
	}

	protected int m_FingerId;

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
		}
	}

	public UIPushButton()
	{
		m_FingerId = -1;
	}

	public void Reset()
	{
		m_FingerId = -1;
	}

	public void Set(bool down)
	{
		if (down)
		{
			m_State = State.Pressed;
		}
		else
		{
			m_State = State.Normal;
		}
		m_FingerId = -1;
	}

	public bool Get()
	{
		if (m_State == State.Pressed)
		{
			return true;
		}
		return false;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				return true;
			}
			return false;
		}
		if (touch.fingerId == m_FingerId)
		{
			if (touch.phase == TouchPhase.Ended)
			{
				m_FingerId = -1;
				if (PtInRect(touch.position))
				{
					if (m_State == State.Normal)
					{
						m_State = State.Pressed;
						m_Parent.SendEvent(this, 0, 0f, 0f);
					}
					else if (m_State == State.Pressed)
					{
						m_State = State.Normal;
						m_Parent.SendEvent(this, 1, 0f, 0f);
					}
				}
			}
			return true;
		}
		return false;
	}
}
