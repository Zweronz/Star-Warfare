using System.Collections.Generic;
using UnityEngine;

public class UISliderProps : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UIPropsBoxIcon : UIPanelX
	{
		private UISliderProps m_props;

		public UIImage m_background;

		public UINumericButton m_buyBtn;

		public UIImage m_plateImg;

		public UIImage m_propsImg;

		public UIImage m_nameImg;

		public FrUIText m_description;

		public int m_nameWidth;

		public int m_nameHeight;

		public float m_size;

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
				float size = m_size;
				Vector2 vector = new Vector2(m_background.Rect.x + m_background.Rect.width * 0.5f, m_background.Rect.y + m_background.Rect.height * 0.5f);
				Rect rect = new Rect(value.x + m_props.m_buyIconOffset.x, value.y + m_props.m_buyIconOffset.y, m_buyBtn.Rect.width, m_buyBtn.Rect.height);
				Vector2 vector2 = new Vector2(rect.x + rect.width * 0.5f, rect.y + rect.height * 0.5f);
				vector2.x = vector.x + (vector2.x - vector.x) * size;
				vector2.y = vector.y + (vector2.y - vector.y) * size;
				m_buyBtn.Rect = new Rect(vector2.x - rect.width * 0.5f, vector2.y - rect.height * 0.5f, rect.width, rect.height);
				Rect rect2 = new Rect(value.x + m_props.m_propsIconOffset.x, value.y + m_props.m_propsIconOffset.y, m_plateImg.Rect.width, m_plateImg.Rect.height);
				Vector2 vector3 = new Vector2(rect2.x + rect2.width * 0.5f, rect2.y + rect2.height * 0.5f);
				vector3.x = vector.x + (vector3.x - vector.x) * size;
				vector3.y = vector.y + (vector3.y - vector.y) * size;
				m_plateImg.Rect = new Rect(vector3.x - rect2.width * 0.5f, vector3.y - rect2.height * 0.5f, rect2.width, rect2.height);
				Rect rect3 = new Rect(value.x + m_props.m_propsIconOffset.x, value.y + m_props.m_propsIconOffset.y, m_propsImg.Rect.width, m_propsImg.Rect.height);
				Vector2 vector4 = new Vector2(rect3.x + rect3.width * 0.5f, rect3.y + rect3.height * 0.5f);
				vector4.x = vector.x + (vector4.x - vector.x) * size;
				vector4.y = vector.y + (vector4.y - vector.y) * size;
				m_propsImg.Rect = new Rect(vector4.x - rect3.width * 0.5f, vector4.y - rect3.height * 0.5f, rect3.width, rect3.height);
				Rect rect4 = new Rect(value.x + m_props.m_descriptionOffset.x, value.y + m_props.m_descriptionOffset.y, m_props.m_propsDescriptionWidth, m_props.m_propsDescriptionHeight);
				int num = (int)((float)m_props.m_propsDescriptionWidth * size);
				int num2 = (int)((float)m_props.m_propsDescriptionHeight * size);
				Vector2 vector5 = new Vector2(rect4.x + rect4.width * 0.5f, rect4.y + rect4.height * 0.5f);
				vector5.x = vector.x + (vector5.x - vector.x) * size;
				vector5.y = vector.y + (vector5.y - vector.y) * size;
				m_description.Rect = new Rect(vector5.x - (float)num * 0.5f, vector5.y - (float)num2 * 0.5f, num, num2);
				Rect rect5 = new Rect(value.x + m_props.m_nameOffset.x, value.y + m_props.m_nameOffset.y, m_nameImg.Rect.width, m_nameImg.Rect.height);
				Vector2 vector6 = new Vector2(rect5.x + rect5.width * 0.5f, rect5.y + rect5.height * 0.5f);
				vector6.x = vector.x + (vector6.x - vector.x) * size;
				vector6.y = vector.y + (vector6.y - vector.y) * size;
				m_nameImg.Rect = new Rect(vector6.x - rect5.width * 0.5f, vector6.y - rect5.height * 0.5f, rect5.width, rect5.height);
			}
		}

		public UIPropsBoxIcon(UISliderProps props)
		{
			m_props = props;
			m_background = new UIImage();
			m_plateImg = new UIImage();
			m_propsImg = new UIImage();
			m_buyBtn = new UINumericButton();
			m_nameImg = new UIImage();
			m_description = new FrUIText();
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (m_buyBtn.HandleInput(touch))
			{
				return true;
			}
			return false;
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_buyBtn.SetParent(parent);
			m_plateImg.SetParent(parent);
			m_propsImg.SetParent(parent);
			m_description.SetParent(parent);
			m_nameImg.SetParent(parent);
			SetParent(parent);
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
			m_buyBtn.SetClip(clip_rect);
			m_plateImg.SetClip(clip_rect);
			m_propsImg.SetClip(clip_rect);
			m_description.SetClip(clip_rect);
			m_nameImg.SetClip(clip_rect);
		}

		public override void Draw()
		{
			m_background.Draw();
			m_buyBtn.Draw();
			m_plateImg.Draw();
			m_propsImg.Draw();
			m_description.Draw();
			m_nameImg.Draw();
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_buyBtn != null)
			{
				m_buyBtn.Destory();
			}
			if (m_plateImg != null)
			{
				m_plateImg.Destory();
			}
			if (m_propsImg != null)
			{
				m_propsImg.Destory();
			}
			if (m_description != null)
			{
				m_description.Destory();
			}
			if (m_nameImg != null)
			{
				m_nameImg.Destory();
			}
		}

		public void SetSize(float factor)
		{
			m_background.SetSize(new Vector2((float)m_props.m_IconWidth * factor, (float)m_props.m_IconHeight * factor));
			m_buyBtn.SetAllSize(new Vector2((float)m_props.m_buyIconWidth * factor, (float)m_props.m_buyIconHeight * factor), factor);
			m_plateImg.SetSize(new Vector2((float)m_props.m_propsIconWidth * factor, (float)m_props.m_propsIconHeight * factor));
			m_propsImg.SetSize(new Vector2((float)m_props.m_propsIconWidth * factor, (float)m_props.m_propsIconHeight * factor));
			int num = (int)((float)m_props.m_propsDescriptionWidth * factor);
			m_description.SetWidth(num);
			m_description.SetSize(factor, false);
			m_nameImg.SetSize(new Vector2((float)m_nameWidth * factor, (float)m_nameHeight * factor));
		}
	}

	public List<UIPropsBoxIcon> m_propsIcons = new List<UIPropsBoxIcon>();

	public List<UIPropsBoxIcon> m_drawPropsIcons = new List<UIPropsBoxIcon>();

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected int m_propsIconWidth;

	protected int m_propsIconHeight;

	protected int m_propsDescriptionWidth;

	protected int m_propsDescriptionHeight;

	private Vector2 m_propsIconOffset;

	private Vector2 m_buyIconOffset;

	private Vector2 m_descriptionOffset;

	private Vector2 m_nameOffset;

	protected int m_buyIconWidth;

	protected int m_buyIconHeight;

	public void Create(UnitUI ui)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, 0, 7);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		Rect modulePositionRect2 = ui.GetModulePositionRect(0, 0, 9);
		m_buyIconWidth = (int)modulePositionRect2.width;
		m_buyIconHeight = (int)modulePositionRect2.height;
		m_buyIconOffset.x = modulePositionRect2.x - modulePositionRect.x;
		m_buyIconOffset.y = modulePositionRect2.y - modulePositionRect.y;
		modulePositionRect2 = ui.GetModulePositionRect(0, 0, 12);
		m_propsIconWidth = (int)modulePositionRect2.width;
		m_propsIconHeight = (int)modulePositionRect2.height;
		m_propsIconOffset.x = modulePositionRect2.x - modulePositionRect.x;
		m_propsIconOffset.y = modulePositionRect2.y - modulePositionRect.y;
		modulePositionRect2 = ui.GetModulePositionRect(0, 0, 13);
		m_propsDescriptionWidth = (int)modulePositionRect2.width;
		m_propsDescriptionHeight = (int)modulePositionRect2.height;
		m_descriptionOffset.x = modulePositionRect2.x - modulePositionRect.x;
		m_descriptionOffset.y = modulePositionRect2.y - modulePositionRect.y;
		modulePositionRect2 = ui.GetModulePositionRect(0, 1, 0);
		m_nameOffset.x = modulePositionRect2.x - modulePositionRect.x;
		m_nameOffset.y = modulePositionRect2.y - modulePositionRect.y;
		SetUIHandler(this);
		base.Show();
	}

	public virtual void Add(UIPropsBoxIcon prop)
	{
		prop.SetParentEx(this);
		m_propsIcons.Add(prop);
	}

	public new void Clear()
	{
		base.Clear();
		m_propsIcons.Clear();
		m_drawPropsIcons.Clear();
	}

	public void SetClipRect(Rect clip)
	{
		m_showRect = clip;
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = false;
		m_scroller.NegativeDir = false;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Vertical, min, max, spacing);
		m_scroller.DeltaTime = 0.016f;
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		SetEnable(index, true);
	}

	public bool GetScrollerState()
	{
		return m_scroller.Moving;
	}

	public override void Draw()
	{
		foreach (UIPropsBoxIcon drawPropsIcon in m_drawPropsIcons)
		{
			if (drawPropsIcon.Visible)
			{
				drawPropsIcon.Draw();
			}
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_propsIcons != null)
		{
			foreach (UIPropsBoxIcon propsIcon in m_propsIcons)
			{
				if (propsIcon != null)
				{
					propsIcon.Destory();
				}
			}
			m_propsIcons.Clear();
		}
		if (m_drawPropsIcons == null)
		{
			return;
		}
		foreach (UIPropsBoxIcon drawPropsIcon in m_drawPropsIcons)
		{
			if (drawPropsIcon != null)
			{
				drawPropsIcon.Destory();
			}
		}
		m_drawPropsIcons.Clear();
	}

	public override void Update()
	{
		m_drawPropsIcons.Clear();
		m_scroller.Update();
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_propsIcons.Count;
		float num5 = 0f;
		for (int i = 0; i < count; i++)
		{
			UIPropsBoxIcon uIPropsBoxIcon = m_propsIcons[i];
			num = num4 - m_scroller.Spacing * (float)i + m_scroller.ScrollerPos - (float)m_IconHeight * 0.5f;
			float num6 = num + (float)m_IconHeight * 0.5f;
			float num7 = Mathf.Abs(num4 - num6);
			float num8 = 1f - num7 / m_showRect.height;
			bool flag = true;
			uIPropsBoxIcon.Visible = true;
			Rect r = new Rect(m_showRect.x, num, m_IconWidth, m_IconHeight);
			if (num8 < 0.3f || !IsRectIntersectsWithRect(r, m_showRect))
			{
				num8 = 0.3f;
				flag = false;
				uIPropsBoxIcon.Visible = false;
			}
			if (flag && num8 > 0f)
			{
				uIPropsBoxIcon.m_size = num8;
				uIPropsBoxIcon.SetSize(num8);
				uIPropsBoxIcon.Rect = new Rect(m_showRect.x, num, m_IconWidth, m_IconHeight);
			}
			uIPropsBoxIcon.SetClip(m_showRect);
			Sort(uIPropsBoxIcon);
		}
	}

	public bool IsRectIntersectsWithRect(Rect r0, Rect r1)
	{
		return r0.xMin < r1.xMax && r0.xMax > r1.xMin && r0.yMin < r1.yMax && r0.yMax > r1.yMin;
	}

	private void SetEnable(int index, bool enable)
	{
		UIPropsBoxIcon uIPropsBoxIcon = m_propsIcons[index];
		uIPropsBoxIcon.Enable = enable;
	}

	public void SetAllEnable(bool enable)
	{
		foreach (UIPropsBoxIcon propsIcon in m_propsIcons)
		{
			propsIcon.Enable = enable;
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (!m_scroller.Moving)
		{
			foreach (UIPropsBoxIcon propsIcon in m_propsIcons)
			{
				if (propsIcon.Enable && propsIcon.HandleInput(touch))
				{
					return true;
				}
			}
		}
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
				wparam %= (float)m_propsIcons.Count;
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

	public bool Sort(UIPropsBoxIcon icon)
	{
		if (m_drawPropsIcons.Count == 0)
		{
			m_drawPropsIcons.Add(icon);
		}
		else
		{
			float size = icon.m_size;
			int num = 0;
			int num2 = m_drawPropsIcons.Count - 1;
			int num3 = (num + num2) / 2;
			if (size <= m_drawPropsIcons[num].m_size)
			{
				m_drawPropsIcons.Insert(num, icon);
			}
			else if (size >= m_drawPropsIcons[num2].m_size)
			{
				m_drawPropsIcons.Insert(num2 + 1, icon);
			}
			else
			{
				while (num2 - num > 1)
				{
					float size2 = m_drawPropsIcons[num3].m_size;
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
				m_drawPropsIcons.Insert(num + 1, icon);
			}
		}
		return true;
	}
}
