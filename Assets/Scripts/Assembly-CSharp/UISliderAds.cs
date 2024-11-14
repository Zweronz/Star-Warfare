using System.Collections.Generic;
using UnityEngine;

public class UISliderAds : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UIIAPIcon : UIPanelX
	{
		public UIImage m_background;

		public UINumericButton m_buyBtn;

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
				m_buyBtn.Rect = value;
			}
		}

		public UIIAPIcon()
		{
			m_background = new UIImage();
			m_buyBtn = new UINumericButton();
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (m_buyBtn.HandleInput(touch))
			{
				return true;
			}
			return false;
		}

		public void SetBackground(UnitUI ui, int frame)
		{
			m_background.AddObject(ui, frame);
			m_background.Rect = m_background.GetObjectRect();
		}

		public void SetBuyBtn(UnitUI ui, int frame, int normalModule, int pressedModule)
		{
			m_buyBtn.AddObject(UIButtonBase.State.Normal, ui, frame, normalModule);
			m_buyBtn.AddObject(UIButtonBase.State.Pressed, ui, frame, pressedModule);
			m_buyBtn.Rect = m_buyBtn.GetObjectRect(UIButtonBase.State.Normal);
			m_buyBtn.Rect = m_background.Rect;
		}

		public void SetParentEx(UIContainer parent)
		{
			m_buyBtn.SetParent(parent);
			m_background.SetParent(parent);
			SetParent(parent);
		}

		public new void SetClip(Rect clip_rect)
		{
			m_buyBtn.SetClip(clip_rect);
			m_background.SetClip(clip_rect);
		}

		public override void Draw()
		{
			m_background.Draw();
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
		}
	}

	public List<UIIAPIcon> m_IAPIcons = new List<UIIAPIcon>();

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[28];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 1, 1);
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 1, 0);
		m_IconWidth = (int)(modulePositionRect.width + modulePositionRect2.width);
		m_IconHeight = (int)(modulePositionRect.height + modulePositionRect2.height);
		SetUIHandler(this);
		base.Show();
	}

	public new void Clear()
	{
		m_IAPIcons.Clear();
		base.Clear();
	}

	public virtual void Add(UIIAPIcon icon)
	{
		icon.SetParentEx(this);
		m_IAPIcons.Add(icon);
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
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
	}

	public bool GetScrollerState()
	{
		return m_scroller.Moving;
	}

	public override void Draw()
	{
		foreach (UIIAPIcon iAPIcon in m_IAPIcons)
		{
			iAPIcon.m_background.Draw();
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_IAPIcons == null)
		{
			return;
		}
		foreach (UIIAPIcon iAPIcon in m_IAPIcons)
		{
			if (iAPIcon != null)
			{
				iAPIcon.Destory();
			}
		}
		m_IAPIcons.Clear();
	}

	public override void Update()
	{
		m_scroller.Update();
		float num = 0f;
		float num2 = m_showRect.y + m_showRect.height - 80f;
		int count = m_IAPIcons.Count;
		for (int i = 0; i < count; i++)
		{
			UIIAPIcon uIIAPIcon = m_IAPIcons[i];
			num = num2 - m_scroller.Spacing * (float)i + m_scroller.ScrollerPos - (float)m_IconHeight * 0.5f;
			float num3 = num + (float)m_IconHeight * 0.5f;
			uIIAPIcon.SetClip(m_showRect);
			uIIAPIcon.Rect = new Rect(m_showRect.x + 10f, num, m_IconWidth, m_IconHeight);
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		m_scroller.HandleInput(touch);
		foreach (UIIAPIcon iAPIcon in m_IAPIcons)
		{
			if (iAPIcon.Enable && iAPIcon.HandleInput(touch))
			{
				return true;
			}
		}
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
				wparam %= (float)m_IAPIcons.Count;
				m_Parent.SendEvent(this, 1, wparam, lparam);
			}
		}
		else
		{
			m_Parent.SendEvent(this, 0, control.Id, lparam);
		}
	}
}
