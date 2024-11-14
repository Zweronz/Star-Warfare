using UnityEngine;

public class UIZoom : UIControl
{
	public enum Command
	{
		Begin = 0,
		Zoom = 1,
		End = 2
	}

	protected struct TouchInfo
	{
		public int FingerId;

		public Vector2 TouchPosition;
	}

	protected TouchInfo[] m_TouchInfo;

	protected int m_FingerIndex;

	protected float m_Distance;

	protected bool m_Zoom;

	public UIZoom()
	{
		m_TouchInfo = new TouchInfo[2];
		m_TouchInfo[0].FingerId = -1;
		m_TouchInfo[1].FingerId = -1;
		m_FingerIndex = 0;
		m_Distance = 0f;
		m_Zoom = false;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_TouchInfo[m_FingerIndex].FingerId = touch.fingerId;
				m_TouchInfo[m_FingerIndex].TouchPosition = touch.position;
				if (m_FingerIndex == 0)
				{
					m_FingerIndex = 1;
				}
				else
				{
					m_FingerIndex = 0;
				}
				if (m_TouchInfo[0].FingerId != -1 && m_TouchInfo[1].FingerId != -1)
				{
					m_Distance = (m_TouchInfo[0].TouchPosition - m_TouchInfo[1].TouchPosition).magnitude;
					m_Zoom = true;
					m_Parent.SendEvent(this, 0, 0f, 0f);
				}
			}
			return false;
		}
		if (touch.phase == TouchPhase.Moved)
		{
			if (m_TouchInfo[0].FingerId == -1 || m_TouchInfo[1].FingerId == -1)
			{
				return false;
			}
			if (!PtInRect(touch.position))
			{
				return false;
			}
			if (m_TouchInfo[0].FingerId == touch.fingerId)
			{
				m_TouchInfo[0].TouchPosition = touch.position;
			}
			else
			{
				if (m_TouchInfo[1].FingerId != touch.fingerId)
				{
					return false;
				}
				m_TouchInfo[1].TouchPosition = touch.position;
			}
			float magnitude = (m_TouchInfo[0].TouchPosition - m_TouchInfo[1].TouchPosition).magnitude;
			float wparam = magnitude - m_Distance;
			m_Distance = magnitude;
			m_Parent.SendEvent(this, 1, wparam, 0f);
			return true;
		}
		if (touch.phase == TouchPhase.Ended)
		{
			bool result = false;
			for (int i = 0; i < 2; i++)
			{
				if (m_TouchInfo[i].FingerId == touch.fingerId)
				{
					m_TouchInfo[i].FingerId = -1;
					m_FingerIndex = i;
					if (m_Zoom)
					{
						m_Zoom = false;
						m_Parent.SendEvent(this, 2, 0f, 0f);
						result = true;
					}
				}
			}
			return result;
		}
		return false;
	}
}
