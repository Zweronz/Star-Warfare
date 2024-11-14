using UnityEngine;

public class UISliderCustomize : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0
	}

	public class UISliderStruct
	{
		private byte m_bgFrame;

		private byte m_bgModule;

		private byte m_fgFrame;

		private byte m_fgModule;

		private byte m_sliderFrame;

		private byte m_sliderModule;

		public byte BGFrame
		{
			get
			{
				return m_bgFrame;
			}
		}

		public byte BGModule
		{
			get
			{
				return m_bgModule;
			}
		}

		public byte FGFrame
		{
			get
			{
				return m_fgFrame;
			}
		}

		public byte FGModule
		{
			get
			{
				return m_fgModule;
			}
		}

		public byte SliderFrame
		{
			get
			{
				return m_sliderFrame;
			}
		}

		public byte SliderModule
		{
			get
			{
				return m_sliderModule;
			}
		}

		public UISliderStruct(int BGFrame, int BGModule, int FGFrame, int FGModule, int sliderFrame, int sliderModule)
		{
			m_bgFrame = (byte)BGFrame;
			m_bgModule = (byte)BGModule;
			m_fgFrame = (byte)FGFrame;
			m_fgModule = (byte)FGModule;
			m_sliderFrame = (byte)sliderFrame;
			m_sliderModule = (byte)sliderModule;
		}
	}

	public Rect m_showRect;

	public int m_selectIndex;

	private UIScroller m_scroller = new UIScroller();

	private UIImage sliderScaleBGImg;

	private UIImage sliderScaleFGImg;

	private UIImage sliderImg;

	private float sliderPos;

	private UISliderStruct m_sliderStruct;

	private float sliderWidthFactor = 0.5f;

	public void Create(UnitUI ui, UISliderStruct sliderStruct)
	{
		m_sliderStruct = sliderStruct;
		sliderScaleBGImg = new UIImage();
		sliderScaleBGImg.AddObject(ui, sliderStruct.BGFrame, sliderStruct.BGModule);
		sliderScaleBGImg.Rect = sliderScaleBGImg.GetObjectRect();
		sliderScaleBGImg.SetParent(this);
		sliderScaleFGImg = new UIImage();
		sliderScaleFGImg.AddObject(ui, sliderStruct.FGFrame, sliderStruct.FGModule);
		sliderScaleFGImg.Rect = sliderScaleFGImg.GetObjectRect();
		sliderScaleFGImg.SetParent(this);
		sliderImg = new UIImage();
		sliderImg.AddObject(ui, sliderStruct.SliderFrame, sliderStruct.SliderModule);
		sliderImg.Rect = sliderImg.GetObjectRect();
		sliderImg.SetParent(this);
		Add(sliderScaleBGImg);
		Add(sliderScaleFGImg);
		Add(sliderImg);
		sliderPos = sliderImg.Rect.x;
		SetUIHandler(this);
		base.Show();
	}

	public new void Clear()
	{
		m_scroller = null;
		m_sliderStruct = null;
		base.Clear();
	}

	public void SetScroller(float min, float max, float spacing, float beginOffset, Rect rct)
	{
		m_scroller.Loop = false;
		m_scroller.BeginOffsetPos = beginOffset;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.QuickMoved = true;
		m_scroller.NegativeDir = false;
		m_scroller.SetParent(this);
	}

	public void SetScrollerMinSpeed(int min)
	{
		m_scroller.MinSpeed = min;
	}

	public bool GetScrollerMoving()
	{
		return m_scroller.Moving;
	}

	public float GetScrollerPos()
	{
		return m_scroller.ScrollerPos;
	}

	public void SetSelection(float index)
	{
		m_scroller.ScrollerPos = index * m_scroller.Spacing;
		m_selectIndex = (byte)index;
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public void SetWidthFactor(float factor)
	{
		sliderWidthFactor = factor;
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (sliderScaleBGImg != null)
		{
			sliderScaleBGImg.Destory();
		}
		if (sliderScaleFGImg != null)
		{
			sliderScaleFGImg.Destory();
		}
		if (sliderImg != null)
		{
			sliderImg.Destory();
		}
	}

	public override void Update()
	{
		m_scroller.Update();
		sliderImg.Rect = new Rect(sliderPos + m_scroller.ScrollerPos, sliderImg.Rect.y, sliderImg.Rect.width, sliderImg.Rect.height);
		float width = sliderImg.Rect.x + sliderImg.Rect.width * sliderWidthFactor - sliderScaleFGImg.Rect.x;
		sliderScaleFGImg.SetClip(new Rect(sliderScaleFGImg.Rect.x, sliderScaleFGImg.Rect.y, width, sliderScaleFGImg.Rect.height));
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
			case 4:
				m_Parent.SendEvent(this, 0, wparam, lparam);
				break;
			case 3:
				m_selectIndex = (int)wparam;
				m_Parent.SendEvent(this, 0, wparam, lparam);
				break;
			}
		}
		else
		{
			m_Parent.SendEvent(this, 0, control.Id, lparam);
		}
	}
}
