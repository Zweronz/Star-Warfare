using UnityEngine;

public class UIControl
{
	protected UIContainer m_Parent;

	protected int m_Id;

	protected Rect m_Rect;

	protected bool m_Visible;

	protected bool m_Enable;

	protected bool m_Clip;

	protected Rect m_ClipRect;

	public int Id
	{
		get
		{
			return m_Id;
		}
		set
		{
			m_Id = value;
		}
	}

	public Rect Rect
	{
		get
		{
			return m_Rect;
		}
		set
		{
			m_Rect = value;
		}
	}

	public bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			m_Visible = value;
		}
	}

	public bool Enable
	{
		get
		{
			return m_Enable;
		}
		set
		{
			m_Enable = value;
		}
	}

	public UIControl()
	{
		m_Parent = null;
		m_Id = 0;
		m_Rect = new Rect(0f, 0f, 0f, 0f);
		m_Visible = true;
		m_Enable = true;
	}

	public void SetParent(UIContainer parent)
	{
		m_Parent = parent;
	}

	public void SetClip(Rect clip_rect)
	{
		m_Clip = true;
		m_ClipRect = clip_rect;
	}

	public void ClearClip()
	{
		m_Clip = false;
	}

	public virtual bool PtInRect(Vector2 pt)
	{
		int num = (int)((m_Rect.xMin + (m_Rect.xMax - m_Rect.xMin) * 0.5f - UIConstant.ScreenLocalWidth * 0.5f) * UIConstant.ScreenAdaptived.x);
		int num2 = (int)((m_Rect.yMin + (m_Rect.yMax - m_Rect.yMin) * 0.5f - UIConstant.ScreenLocalHeight * 0.5f) * UIConstant.ScreenAdaptived.y);
		int num3 = (int)((m_Rect.xMax - m_Rect.xMin) * UIConstant.ScreenAdaptived.x);
		int num4 = (int)((m_Rect.yMax - m_Rect.yMin) * UIConstant.ScreenAdaptived.y);
		int num5 = (int)((float)Screen.width * 0.5f) + num;
		int num6 = (int)((float)Screen.height * 0.5f) + num2;
		Rect rect = new Rect(num5 - num3 / 2, num6 - num4 / 2, num3, num4);
		if (pt.x >= rect.xMin && pt.x < rect.xMax && pt.y >= rect.yMin && pt.y < rect.yMax)
		{
			if (m_Clip)
			{
				return pt.x >= m_ClipRect.xMin && pt.x < m_ClipRect.xMax && pt.y >= m_ClipRect.yMin && pt.y < m_ClipRect.yMax;
			}
			return true;
		}
		return false;
	}

	public virtual void Draw()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void Destory()
	{
	}

	public virtual bool HandleInput(UITouchInner touch)
	{
		return false;
	}
}
