using UnityEngine;

public class UIDialog : UIPanelX, UIHandler
{
	public UIStateManager stateMgr;

	public UIDialogButton m_ownBtn;

	public UIImage m_BGFrame;

	public UIImage m_BGImage;

	public FrUIText m_text;

	public FrUIText m_tipText;

	public UIBlock m_block;

	public UIScroller m_scroller;

	public UIImage m_DisImage;

	public Rect m_textShowRect;

	public Rect m_textPos;

	protected bool m_useScroller;

	protected int mButtonNum;

	public UIDialog(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public UIDialog(UIStateManager stateMgr, int buttonNum)
	{
		this.stateMgr = stateMgr;
		mButtonNum = buttonNum;
	}

	public void Create()
	{
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		m_scroller = new UIScroller();
		m_scroller.NegativeDir = false;
		m_scroller.SetParent(this);
		m_DisImage = new UIImage();
		m_BGFrame = new UIImage();
		m_BGImage = new UIImage();
		m_text = new FrUIText();
		m_tipText = new FrUIText();
		Add(m_BGFrame);
		Add(m_BGImage);
		Add(m_text);
		Add(m_tipText);
		Add(m_DisImage);
		m_ownBtn = new UIDialogButton(mButtonNum);
		createButton();
		SetUIHandler(this);
	}

	protected void SetScroller(float min, float max, float spacing)
	{
		m_scroller.Loop = false;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Vertical, min, max, spacing);
	}

	protected void SetScrollerRect(Rect rct)
	{
		m_scroller.Rect = rct;
	}

	protected void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
	}

	public void SetTextShowRect(float x, float y, float width, float height)
	{
		m_textShowRect = new Rect(x, y, width, height);
	}

	public Rect GetTextShowRect()
	{
		return m_textShowRect;
	}

	protected void SetTipTextPosition(Rect pos)
	{
		m_tipText.Rect = base.Rect;
	}

	protected void SetTextPosition(Rect pos)
	{
		m_text.Rect = base.Rect;
	}

	protected void createButton()
	{
		m_ownBtn.CreateButtons();
		m_ownBtn.SetParent(this);
		m_ownBtn.Visible = true;
	}

	public void AddDisImage(UnitUI ui, int frame, byte module)
	{
		m_DisImage.AddObject(ui, frame, module);
		m_DisImage.Rect = m_DisImage.GetObjectRect();
		m_DisImage.Visible = false;
	}

	public void SetDisImage(UnitUI ui, int frame, byte module)
	{
		m_DisImage.SetTexture(ui, frame, module);
		m_DisImage.Rect = m_DisImage.GetObjectRect();
		m_DisImage.Visible = false;
	}

	public void AddBGFrame(UnitUI ui, int frame, byte module)
	{
		m_BGFrame.AddObject(ui, frame, module);
		m_BGFrame.Rect = m_BGFrame.GetObjectRect();
	}

	public void AddBGFrame(UnitUI ui, int frame, byte[] module)
	{
		m_BGFrame.AddObject(ui, frame, module);
		m_BGFrame.Rect = m_BGFrame.GetObjectRect();
	}

	public void AddBGFrame(UnitUI ui, int frame, byte moduleBegin, byte moduleCount)
	{
		m_BGFrame.AddObject(ui, frame, moduleBegin, moduleCount);
		m_BGFrame.Rect = m_BGFrame.GetObjectRect();
	}

	public void AddBGImage(Rect rect, UnitUI ui, int frame, byte module)
	{
		m_BGImage.AddObject(ui, frame, module);
		m_BGImage.Rect = rect;
		m_BGImage.SetSize(new Vector2(rect.width, rect.height));
	}

	public void SetButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, int module)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetButton(buttonId, state, ui, frame, module);
		}
	}

	public void SetButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, byte[] module)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetButton(buttonId, state, ui, frame, module);
		}
	}

	public void AddButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, int module)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.AddButton(buttonId, state, ui, frame, module);
		}
	}

	public void AddButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, byte[] module)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.AddButton(buttonId, state, ui, frame, module);
		}
	}

	public void AddButton(int buttonId, UnitUI ui, int normalFrame, int normalModule, int pressedFrame, int pressedModule)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.AddButton(buttonId, ui, normalFrame, normalModule, pressedFrame, pressedModule);
		}
	}

	public void AddButton(int buttonId, UnitUI ui, int normalFrame, int normalModule, int pressedFrame, int pressedModule, int disableFrame, int disableModule)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.AddButton(buttonId, ui, normalFrame, normalModule, pressedFrame, pressedModule, disableFrame, disableModule);
		}
	}

	public void SetButtonPosition(int buttonId, Rect pos)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetButtonPosition(buttonId, pos);
		}
	}

	public void SetButtonText(int buttonId, string text)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetText(buttonId, text);
		}
	}

	public void SetButtonText(int buttonId, string font, string text, Color color)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetText(buttonId, font, text, color);
		}
	}

	public void SetButtonText(int buttonId, string font, string text, Color color, float width)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetText(buttonId, font, text, color, width);
		}
	}

	public void SetButtonTextColor(int buttonId, Color normalColor, Color pressedColor)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetTextColor(buttonId, normalColor, pressedColor);
		}
	}

	public void SetButtonTextRect(int buttonId, Rect rect)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetTextRect(buttonId, rect);
		}
	}

	public void SetButtonTextFont(int buttonId, string font)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetTextFont(buttonId, font);
		}
	}

	public void SetButtonTextAlignment(int buttonId, FrUIText.enAlignStyle type)
	{
		if (m_ownBtn != null)
		{
			m_ownBtn.SetTextAlignment(buttonId, type);
		}
	}

	public void SetTipText(string font, string text, Color color, float width)
	{
		Rect rect = m_BGFrame.Rect;
		m_tipText.Set(font, text, color, rect.width);
		int cellHeight = m_tipText.Font.CellHeight;
		m_tipText.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		m_tipText.Rect = new Rect(rect.x, rect.y + rect.height - (float)cellHeight - 53f, rect.width, cellHeight);
	}

	public void SetText(string font, string text, Color color)
	{
		SetText(font, text, color, m_textShowRect.width, FrUIText.enAlignStyle.CENTER_CENTER);
	}

	public void SetText(string font, string text, Color color, FrUIText.enAlignStyle alignment)
	{
		SetText(font, text, color, m_textShowRect.width, alignment);
	}

	public void SetText(string font, string text, Color color, float width, FrUIText.enAlignStyle alignment)
	{
		Rect textShowRect = m_textShowRect;
		m_text.AlignStyle = alignment;
		m_text.Set(font, text, color, width);
		float getTextHeight = m_text.GetTextHeight;
		if (getTextHeight < textShowRect.height)
		{
			m_useScroller = false;
		}
		else
		{
			m_useScroller = true;
			if (alignment == FrUIText.enAlignStyle.CENTER_CENTER)
			{
				m_text.AlignStyle = FrUIText.enAlignStyle.TOP_CENTER;
			}
		}
		m_text.Rect = m_textShowRect;
		if (m_useScroller)
		{
			m_text.Rect = new Rect(textShowRect.x, m_textShowRect.y + m_textShowRect.height - getTextHeight, textShowRect.width, getTextHeight);
			SetScroller(0f, getTextHeight - m_textShowRect.height, (float)m_text.Font.CellHeight + m_text.LineSpacing);
			m_scroller.Rect = textShowRect;
		}
		else
		{
			SetScroller(0f, m_textShowRect.height, (float)m_text.Font.CellHeight + m_text.LineSpacing);
			m_text.ClearClip();
		}
		m_textPos = m_text.Rect;
	}

	public void SetBlock(bool enalbe)
	{
		m_block.Enable = enalbe;
	}

	public override void Draw()
	{
		base.Draw();
		m_ownBtn.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_ownBtn != null)
		{
			m_ownBtn.Destory();
		}
		if (m_BGFrame != null)
		{
			m_BGFrame.Destory();
		}
		if (m_BGImage != null)
		{
			m_BGImage.Destory();
		}
		if (m_text != null)
		{
			m_text.Destory();
		}
		if (m_tipText != null)
		{
			m_tipText.Destory();
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_useScroller)
		{
			m_scroller.HandleInput(touch);
		}
		if (m_ownBtn.HandleInput(touch))
		{
			return true;
		}
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public override void Update()
	{
		if (m_useScroller)
		{
			m_scroller.Update();
			m_text.Rect = new Rect(m_textPos.x, m_textPos.y + m_scroller.ScrollerPos, m_textPos.width, m_textPos.height);
			m_text.SetClip(m_textShowRect);
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_scroller)
		{
			m_Parent.SendEvent(this, command, wparam, lparam);
		}
		else
		{
			m_Parent.SendEvent(this, command, control.Id, lparam);
		}
	}
}
