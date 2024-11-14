using UnityEngine;

public class UISelectButton : UIButtonBase
{
	public enum Command
	{
		Select = 0,
		Unselect = 1
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

	public UISelectButton()
	{
		m_FingerId = -1;
	}

	public void Reset()
	{
		m_FingerId = -1;
	}

	public void Set(bool select)
	{
		if (select)
		{
			m_State = State.Pressed;
		}
		else
		{
			m_State = State.Normal;
		}
		m_FingerId = -1;
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
				if (PtInRect(touch.position) && m_State == State.Normal)
				{
					m_State = State.Pressed;
					m_Parent.SendEvent(this, 0, 0f, 0f);
				}
			}
			return true;
		}
		return false;
	}
}
