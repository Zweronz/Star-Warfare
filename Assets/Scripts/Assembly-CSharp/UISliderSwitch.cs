using UnityEngine;

public class UISliderSwitch : UIPanelX, UIHandler
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

	public void Create(UnitUI ui)
	{
		backgroundImg = new UIImage();
		backgroundImg.SetParent(this);
		sliderImg = new UIImage();
		sliderImg.SetParent(this);
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

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = false;
		m_scroller.NegativeDir = true;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		m_selectIndex = (byte)index;
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
	}

	public override void Draw()
	{
		sliderImg.Draw();
		backgroundImg.Draw();
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		backgroundImg.Destory();
		sliderImg.Destory();
	}

	public override void Update()
	{
		m_scroller.Update();
		sliderImg.Rect = new Rect(backgroundImg.Rect.x - m_scroller.ScrollerPos, sliderImg.Rect.y, sliderImg.Rect.width, sliderImg.Rect.height);
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
			m_selectIndex = (byte)wparam;
			m_Parent.SendEvent(this, 1, wparam, lparam);
			break;
		case 4:
			m_scroller.Moving = true;
			if (m_scroller.ScrollerPos / m_scroller.Spacing == 0f)
			{
				m_scroller.Velocity = new Vector2(10f, 10f);
			}
			else
			{
				m_scroller.Velocity = new Vector2(-10f, -10f);
			}
			break;
		}
	}
}
