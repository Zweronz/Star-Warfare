using UnityEngine;

public class UIScroller : UIControl
{
	public enum ScrollerDir
	{
		Horizontal = 0,
		Vertical = 1
	}

	public enum Command
	{
		Begin = 0,
		Move = 1,
		End = 2,
		SelectIndex = 3,
		Click = 4
	}

	protected int m_FingerId;

	protected Vector2 m_TouchPosition;

	protected bool m_Move;

	protected bool m_Moving;

	protected float m_MinX;

	protected float m_MinY;

	protected ScrollerDir m_Dir;

	protected float m_minPos;

	protected float m_maxPos;

	protected float m_ScrollerPos;

	protected Vector2 m_Velocity;

	protected Vector2 m_LastMove;

	protected bool m_FingerOn;

	protected float m_Spacing;

	protected bool m_Loop;

	protected float m_beginOffsetPos;

	protected bool m_NegativeDir;

	protected int m_MinSpeed;

	protected float m_deltaTime;

	protected bool m_bQuickMoved;

	protected Vector2 m_lastVelocity;

	protected int m_destIndex;

	public bool QuickMoved
	{
		get
		{
			return m_bQuickMoved;
		}
		set
		{
			m_bQuickMoved = value;
		}
	}

	public bool NegativeDir
	{
		get
		{
			return m_NegativeDir;
		}
		set
		{
			m_NegativeDir = value;
		}
	}

	public int DestIndex
	{
		get
		{
			return m_destIndex;
		}
		set
		{
			m_destIndex = value;
		}
	}

	public ScrollerDir Dir
	{
		get
		{
			return m_Dir;
		}
	}

	public float BeginOffsetPos
	{
		get
		{
			return m_beginOffsetPos;
		}
		set
		{
			m_beginOffsetPos = value;
		}
	}

	public float DeltaTime
	{
		get
		{
			return m_deltaTime;
		}
		set
		{
			m_deltaTime = value;
		}
	}

	public Vector2 Velocity
	{
		get
		{
			return m_Velocity;
		}
		set
		{
			m_Velocity = value;
		}
	}

	public int MinSpeed
	{
		get
		{
			return m_MinSpeed;
		}
		set
		{
			m_MinSpeed = value;
		}
	}

	public Vector2 LastVelocity
	{
		get
		{
			return m_lastVelocity;
		}
		set
		{
			m_lastVelocity = value;
		}
	}

	public bool Loop
	{
		get
		{
			return m_Loop;
		}
		set
		{
			m_Loop = value;
		}
	}

	public bool Moving
	{
		get
		{
			return m_Moving;
		}
		set
		{
			m_Moving = value;
		}
	}

	public float ScrollerPos
	{
		get
		{
			return Mathf.Clamp(m_ScrollerPos, m_minPos, m_maxPos);
		}
		set
		{
			m_ScrollerPos = value;
		}
	}

	public float Spacing
	{
		get
		{
			return m_Spacing;
		}
	}

	public float MinX
	{
		get
		{
			return m_MinX;
		}
		set
		{
			m_MinX = value;
		}
	}

	public float MinY
	{
		get
		{
			return m_MinY;
		}
		set
		{
			m_MinY = value;
		}
	}

	public float Diff
	{
		get
		{
			return m_maxPos - m_minPos;
		}
	}

	public UIScroller()
	{
		m_FingerId = -1;
		m_TouchPosition = new Vector2(0f, 0f);
		m_Move = false;
		m_FingerOn = true;
		m_Loop = false;
		m_Moving = false;
		m_NegativeDir = true;
		m_MinSpeed = 5;
		m_deltaTime = 0f;
		m_beginOffsetPos = 0f;
		m_bQuickMoved = false;
		m_destIndex = -1;
		m_lastVelocity = new Vector2(30f, 30f);
	}

	public void SetScroller(ScrollerDir dir, float min, float max, float spacing)
	{
		m_Dir = dir;
		m_ScrollerPos = 0f;
		m_minPos = min;
		m_maxPos = max;
		m_Spacing = spacing;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				m_TouchPosition = touch.position;
				m_Move = false;
				return true;
			}
			return false;
		}
		if (touch.fingerId != m_FingerId)
		{
			return false;
		}
		if (touch.phase == TouchPhase.Moved)
		{
			float num = m_TouchPosition.x - touch.position.x;
			float num2 = m_TouchPosition.y - touch.position.y;
			if (!m_NegativeDir)
			{
				num = touch.position.x - m_TouchPosition.x;
				num2 = touch.position.y - m_TouchPosition.y;
			}
			if (m_Move)
			{
				m_TouchPosition = touch.position;
				OnMove(num, num2);
				m_Parent.SendEvent(this, 1, num, num2);
			}
			else
			{
				float num3 = ((!(num >= 0f)) ? (0f - num) : num);
				float num4 = ((!(num2 >= 0f)) ? (0f - num2) : num2);
				if (num3 > m_MinX || num4 > m_MinY)
				{
					m_TouchPosition = touch.position;
					m_Move = true;
					OnMove(num, num2);
				}
			}
			return true;
		}
		if (touch.phase == TouchPhase.Ended)
		{
			bool move = m_Move;
			m_FingerId = -1;
			m_Move = false;
			float x = Mathf.Clamp(m_LastMove.x, 0f - m_lastVelocity.x, m_lastVelocity.x);
			float y = Mathf.Clamp(m_LastMove.y, 0f - m_lastVelocity.y, m_lastVelocity.y);
			m_Velocity = new Vector2(x, y);
			m_FingerOn = false;
			m_LastMove = Vector2.zero;
			if (!move)
			{
				float num5 = ((m_Dir != 0) ? (base.Rect.y + base.Rect.height - m_TouchPosition.y / ((float)Screen.height / UIConstant.ScreenLocalHeight) - BeginOffsetPos) : (m_TouchPosition.x / UIConstant.ScreenAdaptived.x - base.Rect.x - BeginOffsetPos));
				int num6 = (int)(num5 / m_Spacing);
				num6 = (int)Mathf.Clamp(num6, m_minPos / m_Spacing, m_maxPos / m_Spacing);
				if (m_bQuickMoved)
				{
					m_ScrollerPos = (float)num6 * m_Spacing;
				}
				m_TouchPosition = new Vector2(0f, 0f);
				m_Parent.SendEvent(this, 4, num6, 0f);
				return true;
			}
			m_TouchPosition = new Vector2(0f, 0f);
			return true;
		}
		return false;
	}

	private void OnMove(float dx, float dy)
	{
		if (m_Dir == ScrollerDir.Horizontal)
		{
			m_ScrollerPos += dx;
		}
		else if (m_Dir == ScrollerDir.Vertical)
		{
			m_ScrollerPos += dy;
		}
		LoopScrollerPos();
		m_LastMove = new Vector2(dx, dy);
		m_FingerOn = true;
		m_Moving = true;
	}

	public override void Update()
	{
		if (!m_Moving)
		{
			return;
		}
		int minSpeed = m_MinSpeed;
		float deltaTime = Time.deltaTime;
		if (m_deltaTime != 0f)
		{
			deltaTime = m_deltaTime;
		}
		if (Mathf.Abs(m_Velocity.x) >= (float)minSpeed)
		{
			m_Velocity.x -= deltaTime * 60f * Mathf.Sign(m_Velocity.x);
		}
		else if (!m_FingerOn)
		{
			m_Velocity.x = Mathf.Sign(m_Velocity.x) * (float)minSpeed;
		}
		if (Mathf.Abs(m_Velocity.y) >= (float)minSpeed)
		{
			m_Velocity.y -= deltaTime * 60f * Mathf.Sign(m_Velocity.y);
		}
		else if (!m_FingerOn && m_Velocity.y != 0f)
		{
			m_Velocity.y = Mathf.Sign(m_Velocity.y) * (float)minSpeed;
		}
		if (m_Dir == ScrollerDir.Horizontal)
		{
			m_ScrollerPos += (int)(m_Velocity.x * deltaTime * 100f);
			LoopScrollerPos();
			m_ScrollerPos = Mathf.Clamp(m_ScrollerPos, m_minPos, m_maxPos);
			if (!m_FingerOn && Mathf.Abs(m_Velocity.x) <= (float)minSpeed)
			{
				int num = (int)(m_ScrollerPos / m_Spacing + 0.5f);
				if (m_destIndex != -1)
				{
					num = m_destIndex;
				}
				float num2 = (float)num * m_Spacing;
				if (Mathf.Abs(m_ScrollerPos - num2) > 20f)
				{
					m_ScrollerPos += Mathf.Sign(num2 - m_ScrollerPos) * 100f * deltaTime * 10f;
					return;
				}
				m_ScrollerPos = num2;
				m_Velocity.x = 0f;
				m_destIndex = -1;
				m_Moving = false;
				m_Parent.SendEvent(this, 3, num, 0f);
			}
		}
		else
		{
			if (m_Dir != ScrollerDir.Vertical)
			{
				return;
			}
			m_ScrollerPos += m_Velocity.y * deltaTime * 100f;
			LoopScrollerPos();
			m_ScrollerPos = Mathf.Clamp(m_ScrollerPos, m_minPos, m_maxPos);
			if (!m_FingerOn && Mathf.Abs(m_Velocity.y) <= (float)minSpeed)
			{
				int num3 = (int)(m_ScrollerPos / m_Spacing + 0.5f);
				if (m_destIndex != -1)
				{
					num3 = m_destIndex;
				}
				float num4 = (float)num3 * m_Spacing;
				if (Mathf.Abs(m_ScrollerPos - num4) > 20f)
				{
					m_ScrollerPos += Mathf.Sign(num4 - m_ScrollerPos) * 100f * deltaTime * 10f;
					return;
				}
				m_ScrollerPos = num4;
				m_destIndex = -1;
				m_Velocity.y = 0f;
				m_Moving = false;
				m_Parent.SendEvent(this, 3, num3, 0f);
			}
		}
	}

	private void LoopScrollerPos()
	{
		if (m_Loop)
		{
			if (m_ScrollerPos < m_minPos)
			{
				m_ScrollerPos = m_maxPos + (m_ScrollerPos - m_minPos);
			}
			else if (m_ScrollerPos > m_maxPos)
			{
				m_ScrollerPos %= m_maxPos;
			}
		}
	}
}
