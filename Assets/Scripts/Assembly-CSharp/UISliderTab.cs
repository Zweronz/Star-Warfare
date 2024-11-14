using System.Collections.Generic;
using UnityEngine;

public class UISliderTab : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	private byte m_selectIndex;

	public UIImage backgroundImg;

	public UIImage sliderImg;

	private Vector2 m_velocity;

	public List<UITab> tabList = new List<UITab>();

	public void Create()
	{
		backgroundImg = new UIImage();
		backgroundImg.SetParent(this);
		sliderImg = new UIImage();
		sliderImg.SetParent(this);
		m_velocity = new Vector2(10f, 10f);
		SetUIHandler(this);
		base.Show();
	}

	public void SetBackground(UnitUI ui, int frame, int module)
	{
		backgroundImg.AddObject(ui, frame, module);
		backgroundImg.Rect = backgroundImg.GetObjectRect();
	}

	public void SetSlider(UnitUI ui, int frame, int module)
	{
		sliderImg.AddObject(ui, frame, module);
		sliderImg.Rect = sliderImg.GetObjectRect();
	}

	public void SetVelocity(Vector2 v)
	{
		m_velocity = v;
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = false;
		m_scroller.NegativeDir = false;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetScroller(UIScroller.ScrollerDir dir, float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = false;
		m_scroller.NegativeDir = true;
		m_scroller.SetScroller(dir, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		m_selectIndex = (byte)index;
		foreach (UITab tab in tabList)
		{
			tab.SetState(UITab.State.Normal);
		}
		tabList[m_selectIndex].SetState(UITab.State.Selected);
	}

	public void ShowSlider(bool isShow)
	{
		sliderImg.Visible = isShow;
	}

	public byte GetSelectIndex()
	{
		return m_selectIndex;
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public new void Clear()
	{
		base.Clear();
		tabList.Clear();
	}

	public virtual void Add(UITab tab)
	{
		tab.SetParent(this);
		tabList.Add(tab);
	}

	public override void Draw()
	{
		backgroundImg.Draw();
		foreach (UITab tab in tabList)
		{
			if (tab != null)
			{
				tab.Draw();
			}
		}
		if (sliderImg.Visible)
		{
			sliderImg.Draw();
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		backgroundImg.Destory();
		sliderImg.Destory();
		if (tabList == null)
		{
			return;
		}
		foreach (UITab tab in tabList)
		{
			if (tab != null)
			{
				tab.Destory();
			}
		}
	}

	public override void Update()
	{
		m_scroller.Update();
		if (m_scroller.Dir == UIScroller.ScrollerDir.Horizontal)
		{
			sliderImg.Rect = new Rect(backgroundImg.Rect.x + m_scroller.ScrollerPos, sliderImg.Rect.y, sliderImg.Rect.width, sliderImg.Rect.height);
		}
		else if (m_scroller.Dir == UIScroller.ScrollerDir.Vertical)
		{
			sliderImg.Rect = new Rect(backgroundImg.Rect.x, backgroundImg.Rect.y + backgroundImg.Rect.height - m_scroller.Spacing - m_scroller.ScrollerPos, backgroundImg.Rect.width, sliderImg.Rect.height);
		}
		sliderImg.SetClip(m_showRect);
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
		if (control != m_scroller)
		{
			return;
		}
		switch (command)
		{
		case 3:
			m_scroller.MinSpeed = 5;
			m_selectIndex = (byte)wparam;
			SetSelection(m_selectIndex);
			m_Parent.SendEvent(this, 1, wparam, lparam);
			break;
		case 4:
		{
			m_scroller.Moving = true;
			m_scroller.DestIndex = (int)wparam;
			int num = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
			if (num < (int)wparam)
			{
				m_scroller.Velocity = new Vector2(m_velocity.x, m_velocity.y);
				m_scroller.MinSpeed = (int)m_velocity.x;
			}
			else if (num > (int)wparam)
			{
				m_scroller.Velocity = new Vector2(0f - m_velocity.x, 0f - m_velocity.y);
				m_scroller.MinSpeed = (int)m_velocity.x;
			}
			break;
		}
		}
	}
}
