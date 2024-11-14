using System.Collections.Generic;
using UnityEngine;

public class UISliderTag : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UITagIcon : UIPanelX
	{
		public UIImage m_background;

		public UIImage m_tagIcon;

		public float m_size;

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
				m_tagIcon.Rect = value;
			}
		}

		public override void Draw()
		{
			m_background.Draw();
			m_tagIcon.Draw();
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
			m_tagIcon.SetClip(clip_rect);
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_tagIcon.SetParent(parent);
			SetParent(parent);
		}

		public override void Destory()
		{
			m_background.Destory();
			m_tagIcon.Destory();
		}
	}

	public const byte BG_UI_CUSTOMIZE = 0;

	public const byte BG_UI_STORE = 1;

	public static byte[,] BG_IMG = new byte[1, 5] { { 0, 17, 42, 40, 2 } };

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected int m_tagWidth;

	protected int m_tagHeight;

	protected float m_width;

	public List<UITagIcon> m_tagLst = new List<UITagIcon>();

	public List<UITagIcon> m_drawTagLst = new List<UITagIcon>();

	public void Create(UnitUI ui, int BGIndex)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 4]);
		SetClipRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		SetScroller(0f, 540f, 90f, modulePositionRect);
		Vector2 moduleSize = ui.GetModuleSize(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 1]);
		m_IconWidth = (int)moduleSize.x;
		m_IconHeight = (int)moduleSize.y;
		moduleSize = ui.GetModuleSize(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 2]);
		m_tagWidth = (int)moduleSize.x;
		m_tagHeight = (int)moduleSize.y;
		SetUIHandler(this);
		base.Show();
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = true;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
		m_scroller.LastVelocity = new Vector2(10f, 10f);
		m_scroller.DeltaTime = 0.016f;
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		SetEnable(index, true);
	}

	public virtual void Add(UITagIcon control)
	{
		control.m_background.SetParent(this);
		control.m_tagIcon.SetParent(this);
		m_tagLst.Add(control);
	}

	public override void Draw()
	{
		base.Draw();
		for (int i = 0; i < m_drawTagLst.Count; i++)
		{
			m_drawTagLst[i].Draw();
		}
	}

	public override void Destory()
	{
		base.Destory();
		if (m_tagLst != null)
		{
			for (int i = 0; i < m_tagLst.Count; i++)
			{
				if (m_tagLst[i] != null)
				{
					m_tagLst[i].Destory();
				}
			}
			m_tagLst.Clear();
		}
		if (m_drawTagLst == null)
		{
			return;
		}
		for (int j = 0; j < m_drawTagLst.Count; j++)
		{
			if (m_drawTagLst[j] != null)
			{
				m_drawTagLst[j].Destory();
			}
		}
		m_drawTagLst.Clear();
	}

	public override void Update()
	{
		m_scroller.Update();
		m_drawTagLst.Clear();
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_tagLst.Count;
		for (int i = 0; i < count; i++)
		{
			UITagIcon uITagIcon = m_tagLst[i];
			uITagIcon.Visible = true;
			num = num3 + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - (float)m_IconWidth * 0.5f;
			if (num + (float)m_IconWidth < m_showRect.x)
			{
				num = m_showRect.x + (float)count * m_scroller.Spacing - (m_showRect.x - num);
			}
			else if (num + (float)m_IconWidth > m_showRect.x + (float)count * m_scroller.Spacing)
			{
				float num5 = num + (float)m_IconWidth - (m_showRect.x + (float)count * m_scroller.Spacing);
				num = m_showRect.x - (float)m_IconWidth + num5;
			}
			Rect rect = uITagIcon.Rect;
			float num6 = uITagIcon.Rect.x + uITagIcon.Rect.width * 0.5f;
			float num7 = Mathf.Abs(num3 - num6);
			float num8 = 1f - num7 / m_showRect.width;
			if ((double)num8 < 0.2)
			{
				num8 = 0.2f;
			}
			if (num8 > 0f)
			{
				float num9 = (float)m_IconWidth * num8;
				float num10 = (float)m_IconHeight * num8;
				uITagIcon.Rect = new Rect(num, m_showRect.y + ((float)m_IconHeight - num10) * 0.5f, rect.width, rect.height);
				uITagIcon.m_background.SetSize(new Vector2((float)m_IconWidth * num8, (float)m_IconHeight * num8));
				uITagIcon.m_tagIcon.SetSize(new Vector2((float)m_tagWidth * num8, (float)m_tagHeight * num8));
			}
			uITagIcon.SetClip(m_showRect);
			uITagIcon.m_size = num8;
			Sort(uITagIcon);
		}
	}

	private void SetEnable(int index, bool enable)
	{
		UITagIcon uITagIcon = m_tagLst[index];
		uITagIcon.Enable = enable;
	}

	public void SetAllEnable(bool enable)
	{
		foreach (UITagIcon item in m_tagLst)
		{
			item.Enable = enable;
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		m_scroller.HandleInput(touch);
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
				wparam %= (float)m_tagLst.Count;
				m_Parent.SendEvent(this, 1, wparam, lparam);
				SetAllEnable(false);
				SetEnable((int)wparam, true);
			}
		}
		else
		{
			m_Parent.SendEvent(this, 0, control.Id, lparam);
		}
	}

	public bool Sort(UITagIcon icon)
	{
		if (m_drawTagLst.Count == 0)
		{
			m_drawTagLst.Add(icon);
		}
		else
		{
			float size = icon.m_size;
			int num = 0;
			int num2 = m_drawTagLst.Count - 1;
			int num3 = (num + num2) / 2;
			if (size <= m_drawTagLst[num].m_size)
			{
				m_drawTagLst.Insert(num, icon);
			}
			else if (size >= m_drawTagLst[num2].m_size)
			{
				m_drawTagLst.Insert(num2 + 1, icon);
			}
			else
			{
				while (num2 - num > 1)
				{
					float size2 = m_drawTagLst[num3].m_size;
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
				m_drawTagLst.Insert(num + 1, icon);
			}
		}
		return true;
	}
}
