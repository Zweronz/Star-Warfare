using System.Collections.Generic;
using UnityEngine;

public class UISliderIAP : UIPanelX, UIHandler
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

		public UINumeric m_mithril;

		public UINumeric m_original;

		public UINumeric m_saving;

		public UIImage m_decorate;

		public UIImage m_rankimg;

		public UIImage m_expBarImgBG;

		public UIImage m_expBarImg;

		public UIImage m_rankdes;

		public int id;

		private Vector2 m_buyBtnOffset;

		private Vector2 m_mithrilOffset;

		private Vector2 m_originalOffset;

		private Vector2 m_savingOffset;

		private Vector2 m_decorateOffset;

		private Vector2 m_rankimgOffset;

		private Vector2 m_expBarImgBGOffset;

		private Vector2 m_ranknameOffset;

		private Vector2 m_rankdesOffset;

		private Vector2 m_expBarImgOffset;

		public int width;

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
				m_buyBtn.Rect = new Rect(value.x + m_buyBtnOffset.x, value.y + m_buyBtnOffset.y, m_buyBtn.Rect.width, m_buyBtn.Rect.height);
				m_mithril.Rect = new Rect(value.x + m_mithrilOffset.x, value.y + m_mithrilOffset.y, m_mithril.Rect.width, m_mithril.Rect.height);
				m_original.Rect = new Rect(value.x + m_originalOffset.x, value.y + m_originalOffset.y, m_original.Rect.width, m_original.Rect.height);
				m_saving.Rect = new Rect(value.x + m_savingOffset.x, value.y + m_savingOffset.y, m_saving.Rect.width, m_saving.Rect.height);
				m_decorate.Rect = new Rect(value.x + m_decorateOffset.x, value.y + m_decorateOffset.y, m_decorate.Rect.width, m_decorate.Rect.height);
				if (AndroidConstant.version == AndroidConstant.Version.Kindle)
				{
					m_rankimg.Rect = new Rect(value.x + m_rankimgOffset.x, value.y + m_rankimgOffset.y, m_rankimg.Rect.width, m_rankimg.Rect.height);
					m_expBarImgBG.Rect = new Rect(value.x + m_expBarImgBGOffset.x, value.y + m_expBarImgBGOffset.y, m_expBarImgBG.Rect.width, m_expBarImgBG.Rect.height);
					m_rankdes.Rect = new Rect(value.x + m_rankdesOffset.x, value.y + m_rankdesOffset.y, m_rankdes.Rect.width, m_rankdes.Rect.height);
					m_expBarImg.Rect = new Rect(value.x + m_expBarImgOffset.x, value.y + m_expBarImgOffset.y, m_expBarImg.Rect.width, m_expBarImg.Rect.height);
				}
			}
		}

		public UIIAPIcon()
		{
			m_buyBtn = new UINumericButton();
			m_background = new UIImage();
			m_mithril = new UINumeric();
			m_original = new UINumeric();
			m_saving = new UINumeric();
			m_decorate = new UIImage();
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				m_rankimg = new UIImage();
				m_expBarImgBG = new UIImage();
				m_expBarImg = new UIImage();
				m_rankdes = new UIImage();
			}
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (m_buyBtn.HandleInput(touch))
			{
				return true;
			}
			return false;
		}

		public void SetRank(UnitUI ui, int frame)
		{
			m_rankimg.AddObject(ui, 0, frame);
			m_rankimg.Rect = m_rankimg.GetObjectRect();
			m_rankimgOffset.x = m_rankimg.Rect.x - m_background.Rect.x;
			m_rankimgOffset.y = m_rankimg.Rect.y - m_background.Rect.y;
		}

		public void ResetRank(UnitUI ui, int frame)
		{
			m_rankimg.SetTexture(ui, 0, frame);
			m_rankimgOffset.x = m_rankimg.Rect.x - m_background.Rect.x;
			m_rankimgOffset.y = m_rankimg.Rect.y - m_background.Rect.y;
		}

		public void SetExpBar(UnitUI ui)
		{
			m_expBarImgBG.AddObject(ui, 0, 27);
			m_expBarImgBG.Rect = m_expBarImgBG.GetObjectRect();
			m_expBarImgBGOffset.x = m_expBarImgBG.Rect.x - m_background.Rect.x;
			m_expBarImgBGOffset.y = m_expBarImgBG.Rect.y - m_background.Rect.y;
			m_expBarImg.AddObject(ui, 0, 28);
			m_expBarImg.Rect = m_expBarImg.GetObjectRect();
			m_expBarImgOffset.x = m_expBarImg.Rect.x - m_background.Rect.x;
			m_expBarImgOffset.y = m_expBarImg.Rect.y - m_background.Rect.y;
		}

		public void SetExpBarClip(int width)
		{
			Rect clip = new Rect(m_expBarImg.Rect.x, m_expBarImg.Rect.y, width, m_expBarImg.Rect.height);
			m_expBarImg.SetClip(clip);
		}

		public void SetRankDes(UnitUI ui, int frame)
		{
			m_rankdes.AddObject(ui, 0, frame);
			m_rankdes.Rect = m_rankdes.GetObjectRect();
			m_rankdesOffset.x = m_rankdes.Rect.x - m_background.Rect.x;
			m_rankdesOffset.y = m_rankdes.Rect.y - m_background.Rect.y;
		}

		public void RestRankDes(UnitUI ui, int frame)
		{
			m_rankdes.SetTexture(ui, 0, frame);
		}

		public void SetBackground(UnitUI ui, int frame)
		{
			m_background.AddObject(ui, frame);
			m_background.Rect = m_background.GetObjectRect();
		}

		public void SetBackground(UnitUI ui, int frame, int type)
		{
			m_background.AddObject(ui, frame, type);
			m_background.Rect = m_background.GetObjectRect();
		}

		public void ResetBackground(UnitUI ui, int frame)
		{
			m_background.SetTexture(ui, frame);
			m_background.Rect = m_background.GetObjectRect();
		}

		public void SetDecorate(UnitUI ui, int frame, int module)
		{
			m_decorate.AddObject(ui, frame, module);
			m_decorate.Rect = m_decorate.GetObjectRect();
			m_decorateOffset.x = m_decorate.Rect.x - m_background.Rect.x;
			m_decorateOffset.y = m_decorate.Rect.y - m_background.Rect.y;
		}

		public void ResetDecorate(UnitUI ui, int frame, int module)
		{
			m_decorate.SetTexture(ui, frame, module);
			m_decorate.Rect = m_decorate.GetObjectRect();
			m_decorateOffset.x = m_decorate.Rect.x - m_background.Rect.x;
			m_decorateOffset.y = m_decorate.Rect.y - m_background.Rect.y;
		}

		public void SetBuyBtn(UnitUI ui, int frame, int normalModule, int pressedModule)
		{
			m_buyBtn.AddObject(UIButtonBase.State.Normal, ui, frame, normalModule);
			m_buyBtn.AddObject(UIButtonBase.State.Pressed, ui, frame, pressedModule);
			m_buyBtn.Rect = m_buyBtn.GetObjectRect(UIButtonBase.State.Normal);
			m_buyBtnOffset.x = m_buyBtn.Rect.x - m_background.Rect.x;
			m_buyBtnOffset.y = m_buyBtn.Rect.y - m_background.Rect.y;
		}

		public void SetBuyBtnPress(UnitUI ui, int frame, int pressedModule)
		{
			m_buyBtn.SetTexture(UIButtonBase.State.Normal, ui, frame, pressedModule);
			m_buyBtn.SetTextColor(UIConstant.fontColor_gray, UIConstant.fontColor_gray);
		}

		public void ResetBuyBtn(UnitUI ui, int frame, int normalModule, int pressedModule)
		{
			m_buyBtn.SetTexture(UIButtonBase.State.Normal, ui, frame, normalModule);
			m_buyBtn.SetTexture(UIButtonBase.State.Pressed, ui, frame, pressedModule);
			m_buyBtn.Rect = m_buyBtn.GetObjectRect(UIButtonBase.State.Normal);
			m_buyBtnOffset.x = m_buyBtn.Rect.x - m_background.Rect.x;
			m_buyBtnOffset.y = m_buyBtn.Rect.y - m_background.Rect.y;
		}

		public void SetBuyBtnText(UnitUI ui, int frame, string price)
		{
			m_buyBtn.SetNumeric(ui, frame, price, -5f);
			m_buyBtn.SetTextColor(UIConstant.fontColor_white, UIConstant.fontColor_gray);
			m_buyBtn.Rect = m_buyBtn.Rect;
		}

		public void SetMithril(UnitUI ui, int frame, string mithril, Rect rct)
		{
			m_mithril.AlignStyle = UINumeric.enAlignStyle.center;
			m_mithril.SpacingOffsetX = -5f;
			m_mithril.SetNumeric(ui, frame, mithril);
			m_mithril.Rect = rct;
			m_mithrilOffset.x = m_mithril.Rect.x - m_background.Rect.x;
			m_mithrilOffset.y = m_mithril.Rect.y - m_background.Rect.y;
		}

		public void SetOriginal(UnitUI ui, int frame, string original, Rect rct)
		{
			m_original.AlignStyle = UINumeric.enAlignStyle.right;
			m_original.SpacingOffsetX = 1f;
			m_original.SetNumeric(ui, frame, original);
			m_original.Rect = rct;
			m_originalOffset.x = m_original.Rect.x - m_background.Rect.x;
			m_originalOffset.y = m_original.Rect.y - m_background.Rect.y;
		}

		public void SetSaving(UnitUI ui, int frame, string saving, Rect rct)
		{
			m_saving.AlignStyle = UINumeric.enAlignStyle.left;
			m_saving.SpacingOffsetX = 1f;
			m_saving.SetNumeric(ui, frame, saving);
			m_saving.Rect = rct;
			m_savingOffset.x = m_saving.Rect.x - m_background.Rect.x;
			m_savingOffset.y = m_saving.Rect.y - m_background.Rect.y;
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_buyBtn.SetParent(parent);
			m_mithril.SetParent(parent);
			m_original.SetParent(parent);
			m_saving.SetParent(parent);
			m_decorate.SetParent(parent);
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				m_rankimg.SetParent(parent);
				m_expBarImgBG.SetParent(parent);
				m_rankdes.SetParent(parent);
				m_expBarImg.SetParent(parent);
			}
			SetParent(parent);
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
			m_buyBtn.SetClip(clip_rect);
			m_mithril.SetClip(clip_rect);
			m_original.SetClip(clip_rect);
			m_saving.SetClip(clip_rect);
			m_decorate.SetClip(clip_rect);
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				m_rankimg.SetClip(clip_rect);
				m_expBarImgBG.SetClip(clip_rect);
				m_rankdes.SetClip(clip_rect);
			}
		}

		public override void Draw()
		{
			m_background.Draw();
			m_buyBtn.Draw();
			m_mithril.Draw();
			m_original.Draw();
			m_saving.Draw();
			m_decorate.Draw();
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				m_rankimg.Draw();
				m_expBarImgBG.Draw();
				m_rankdes.Draw();
				m_expBarImg.Draw();
			}
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
			if (m_original != null)
			{
				m_original.Destory();
			}
			if (m_mithril != null)
			{
				m_mithril.Destory();
			}
			if (m_saving != null)
			{
				m_saving.Destory();
			}
			if (m_decorate != null)
			{
				m_decorate.Destory();
			}
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				if (m_rankimg != null)
				{
					m_rankimg.Destory();
				}
				if (m_expBarImgBG != null)
				{
					m_expBarImgBG.Destory();
				}
				if (m_rankdes != null)
				{
					m_rankdes.Destory();
				}
				if (m_expBarImg != null)
				{
					m_expBarImg.Destory();
				}
			}
		}
	}

	public List<UIIAPIcon> m_IAPIcons = new List<UIIAPIcon>();

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	public void Create(UnitUI ui)
	{
		Rect modulePositionRect = ui.GetModulePositionRect(0, 0, 2);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
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
		m_scroller.Loop = true;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
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
			iAPIcon.m_buyBtn.Draw();
			iAPIcon.m_mithril.Draw();
			iAPIcon.m_original.Draw();
			iAPIcon.m_saving.Draw();
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				iAPIcon.m_rankimg.Draw();
				iAPIcon.m_expBarImgBG.Draw();
				iAPIcon.m_rankdes.Draw();
				iAPIcon.m_expBarImg.Draw();
			}
		}
		foreach (UIIAPIcon iAPIcon2 in m_IAPIcons)
		{
			iAPIcon2.m_decorate.Draw();
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
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + (float)m_IconWidth * 0.5f + 20f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_IAPIcons.Count;
		for (int i = 0; i < count; i++)
		{
			UIIAPIcon uIIAPIcon = m_IAPIcons[i];
			UIImage background = uIIAPIcon.m_background;
			num = num3 + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - 0.5f * (float)m_IconWidth;
			if (num + (float)m_IconWidth < m_showRect.x)
			{
				num = m_showRect.x + (float)count * m_scroller.Spacing - (m_showRect.x - num);
			}
			else if (num + (float)m_IconWidth > m_showRect.x + (float)count * m_scroller.Spacing)
			{
				float num5 = num + (float)m_IconWidth - (m_showRect.x + (float)count * m_scroller.Spacing);
				num = m_showRect.x - (float)m_IconWidth + num5;
			}
			uIIAPIcon.Rect = new Rect(num, num4 - (float)m_IconHeight * 0.5f, m_IconWidth, m_IconHeight);
			uIIAPIcon.SetClip(m_showRect);
			if (AndroidConstant.version == AndroidConstant.Version.Kindle && i == 1)
			{
				uIIAPIcon.SetExpBarClip(uIIAPIcon.width);
			}
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
