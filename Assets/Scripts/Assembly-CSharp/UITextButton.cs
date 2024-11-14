using UnityEngine;

public class UITextButton : UIClickButton, UIContainer
{
	public static Color fontColor_orange = new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f);

	public static Color fontColor_yellow = new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f);

	protected FrUIText m_Text = new FrUIText();

	protected Color m_NormalColor = fontColor_orange;

	protected Color m_PressedColor = fontColor_yellow;

	private Vector2 m_offset = Vector2.zero;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			SetSpritePosition(0, position);
			SetSpritePosition(1, position);
			SetSpritePosition(2, position);
			m_Text.Rect = new Rect(value.x + m_offset.x, value.y + m_offset.y, value.width, value.height);
		}
	}

	public void SetText(string font, string text, Color color)
	{
		m_Text.Set(font, text, color);
		m_Text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		m_Text.Rect = Rect;
		m_Text.SetParent(this);
	}

	public void SetText(string font, string text, Color color, float width)
	{
		m_Text.Set(font, text, color, width);
		m_Text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		m_Text.Rect = Rect;
		m_Text.SetParent(this);
	}

	public void SetText(string text)
	{
		m_Text.SetText(text);
	}

	public void SetTextOffset(float x, float y)
	{
		m_offset = new Vector2(x, y);
	}

	public void SetTextColor(Color normalColor, Color pressedColor)
	{
		m_NormalColor = normalColor;
		m_PressedColor = pressedColor;
	}

	public void SetTextRect(Rect rect)
	{
		m_Text.Rect = rect;
	}

	public void SetTextFont(string font)
	{
		m_Text.SetFont(font);
	}

	public void SetTextAlignment(FrUIText.enAlignStyle type)
	{
		m_Text.AlignStyle = type;
	}

	public override void Draw()
	{
		base.Draw();
		if (m_State == State.Normal)
		{
			m_Text.SetColor(m_NormalColor);
		}
		else if (m_State == State.Pressed)
		{
			m_Text.SetColor(m_PressedColor);
		}
		m_Text.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		m_Text.Destory();
	}

	public void DrawSprite(UISpriteX sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		m_Parent.SendEvent(control, command, wparam, lparam);
	}

	public void Add(UIControl control)
	{
	}

	public void SetAllSize(Vector2 size, float zoom)
	{
		SetSize(State.Normal, size);
		SetSize(State.Pressed, size);
	}

	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		m_Text.SetClip(clip_rect);
	}
}
