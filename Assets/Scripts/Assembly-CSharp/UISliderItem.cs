using System;
using System.Collections.Generic;
using UnityEngine;

public class UISliderItem : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UIGunIcon : UIPanelX
	{
		public UIImage m_background;

		public UIImage m_gunIcon;

		public UIImage m_gunDisableIcon;

		public bool m_hasCDTimer;

		public float m_size;

		public Weapon m_weapon;

		public override void Draw()
		{
			m_background.Draw();
			m_gunIcon.Draw();
			if (m_hasCDTimer)
			{
				m_gunDisableIcon.Draw();
			}
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_gunIcon != null)
			{
				m_gunIcon.Destory();
			}
			if (m_gunDisableIcon != null)
			{
				m_gunDisableIcon.Destory();
			}
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
			m_gunIcon.SetClip(clip_rect);
		}
	}

	public const byte BG_UI_WEAPON = 0;

	public const byte BG_UI_PROPS = 1;

	public static byte[,] BG_IMG = new byte[2, 4]
	{
		{ 0, 8, 30, 31 },
		{ 0, 15, 16, 17 }
	};

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	public byte m_selectIndex;

	public List<UIGunIcon> m_gunLst = new List<UIGunIcon>();

	public List<UIGunIcon> m_drawGunLst = new List<UIGunIcon>();

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected int m_gunIconWidth;

	protected int m_gunIconHeight;

	private DateTime time;

	private bool bWorkable;

	public void CreateBG(UnitUI ui, int BGIndex)
	{
		bWorkable = false;
		Rect modulePositionRect = ui.GetModulePositionRect(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 1]);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[20];
		modulePositionRect = unitUI.GetModulePositionRect(0, 0, 0);
		m_gunIconWidth = (int)modulePositionRect.width;
		m_gunIconHeight = (int)modulePositionRect.height;
		SetUIHandler(this);
		base.Show();
	}

	public virtual void Add(UIGunIcon control)
	{
		control.m_background.SetParent(this);
		control.m_gunIcon.SetParent(this);
		control.m_gunDisableIcon.SetParent(this);
		m_gunLst.Add(control);
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = true;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Vertical, min, max, spacing);
		m_scroller.LastVelocity = new Vector2(10f, 10f);
		m_scroller.DeltaTime = 0.016f;
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		m_selectIndex = (byte)index;
	}

	public void SetClipRect(Rect clip)
	{
		m_showRect = clip;
	}

	public new void Clear()
	{
		base.Clear();
		m_gunLst.Clear();
		m_drawGunLst.Clear();
	}

	public override void Draw()
	{
		for (int i = 0; i < m_drawGunLst.Count; i++)
		{
			if (bWorkable || i == m_drawGunLst.Count - 1)
			{
				m_drawGunLst[i].Draw();
			}
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_gunLst != null)
		{
			foreach (UIGunIcon item in m_gunLst)
			{
				if (item != null)
				{
					item.Destory();
				}
			}
			m_gunLst.Clear();
		}
		if (m_drawGunLst == null)
		{
			return;
		}
		foreach (UIGunIcon item2 in m_drawGunLst)
		{
			if (item2 != null)
			{
				item2.Destory();
			}
		}
		m_drawGunLst.Clear();
	}

	public override void Update()
	{
		m_scroller.Update();
		if (m_scroller.Moving)
		{
			time = DateTime.Now;
			bWorkable = true;
		}
		else if ((DateTime.Now - time).TotalSeconds > 5.0)
		{
			bWorkable = false;
		}
		m_drawGunLst.Clear();
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_gunLst.Count;
		for (int i = 0; i < count; i++)
		{
			UIGunIcon uIGunIcon = m_gunLst[i];
			num = num4 + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - (float)m_IconHeight * 0.5f;
			if (num + (float)m_IconHeight < m_showRect.y)
			{
				num = m_showRect.y + (float)count * m_scroller.Spacing - (m_showRect.y - num);
			}
			else if (num + (float)m_IconHeight > m_showRect.y + (float)count * m_scroller.Spacing)
			{
				float num5 = num + (float)m_IconHeight - (m_showRect.y + (float)count * m_scroller.Spacing);
				num = m_showRect.y - (float)m_IconHeight + num5;
			}
			float num6 = num + (float)m_IconHeight * 0.5f;
			float num7 = Mathf.Abs(num4 - num6);
			float num8 = 1f - num7 / m_showRect.height;
			if (num8 < 0.2f)
			{
				num8 = 0.2f;
			}
			float num9 = (float)m_IconWidth * num8;
			float y = (float)m_IconHeight * num8;
			uIGunIcon.m_background.Rect = new Rect(m_showRect.x + m_showRect.width - num9, num, num9, m_IconHeight);
			uIGunIcon.m_background.SetSize(new Vector2(num9, y));
			uIGunIcon.m_background.SetColor(new Color(1f * num8, 1f * num8, 1f * num8, 1f * num8));
			uIGunIcon.m_gunIcon.Rect = uIGunIcon.m_background.Rect;
			uIGunIcon.m_gunIcon.SetSize(new Vector2((float)m_gunIconWidth * num8, (float)m_gunIconHeight * num8));
			uIGunIcon.m_gunIcon.SetColor(new Color(1f * num8, 1f * num8, 1f * num8, 1f * num8));
			uIGunIcon.m_size = num8;
			if (uIGunIcon.m_hasCDTimer)
			{
				uIGunIcon.m_gunDisableIcon.Rect = uIGunIcon.m_background.Rect;
				uIGunIcon.m_gunDisableIcon.SetSize(new Vector2((float)m_gunIconWidth * num8, (float)m_gunIconHeight * num8));
				uIGunIcon.m_gunDisableIcon.SetColor(new Color(1f * num8, 1f * num8, 1f * num8, 1f * num8));
				Weapon weapon = uIGunIcon.m_weapon;
				LaserCannon laserCannon = (LaserCannon)weapon;
				float chargeEnegy = laserCannon.GetChargeEnegy();
				uIGunIcon.m_gunDisableIcon.Visible = true;
				int num10 = (int)((float)m_gunIconWidth * num8);
				int num11 = (int)((float)m_gunIconHeight * num8);
				int num12 = (int)((float)num10 * chargeEnegy / 100f);
				Rect rect = new Rect(uIGunIcon.m_background.Rect.x + uIGunIcon.m_background.Rect.width * 0.5f - (float)num10 * 0.5f, uIGunIcon.m_background.Rect.y + uIGunIcon.m_background.Rect.height * 0.5f - (float)num11 * 0.5f, num10 - num12, num11);
				float num13 = Mathf.Max(m_showRect.xMin, rect.xMin);
				float num14 = Mathf.Min(m_showRect.xMax, rect.xMax);
				float num15 = Mathf.Max(m_showRect.yMin, rect.yMin);
				float num16 = Mathf.Min(m_showRect.yMax, rect.yMax);
				uIGunIcon.m_gunDisableIcon.SetClip(new Rect(num13, num15, num14 - num13, num16 - num15));
			}
			uIGunIcon.SetClip(m_showRect);
			Sort(uIGunIcon);
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
			switch (command)
			{
			case 3:
				wparam %= (float)m_gunLst.Count;
				m_selectIndex = (byte)wparam;
				m_Parent.SendEvent(this, 1, wparam, lparam);
				break;
			case 4:
				m_Parent.SendEvent(this, 0, (int)m_selectIndex, lparam);
				break;
			}
		}
	}

	public bool Sort(UIGunIcon icon)
	{
		if (m_drawGunLst.Count == 0)
		{
			m_drawGunLst.Add(icon);
		}
		else
		{
			float size = icon.m_size;
			int num = 0;
			int num2 = m_drawGunLst.Count - 1;
			int num3 = (num + num2) / 2;
			if (size <= m_drawGunLst[num].m_size)
			{
				m_drawGunLst.Insert(num, icon);
			}
			else if (size >= m_drawGunLst[num2].m_size)
			{
				m_drawGunLst.Insert(num2 + 1, icon);
			}
			else
			{
				while (num2 - num > 1)
				{
					float size2 = m_drawGunLst[num3].m_size;
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
				m_drawGunLst.Insert(num + 1, icon);
			}
		}
		return true;
	}
}
