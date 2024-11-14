using UnityEngine;

public class UITextImage : UIImage, UIContainer
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
			m_Text.Rect = new Rect(value.x + m_offset.x, value.y + m_offset.y, value.width, value.height);
		}
	}

	public void SetText(string font, string text, Color color)
	{
		m_Text.Set(font, text, color);
		m_Text.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		m_Text.Rect = new Rect(Rect.x + m_offset.x, Rect.y + m_offset.y, Rect.width, Rect.height);
		m_Text.SetParent(this);
	}

	public void SetText(string font, string text, Color color, FrUIText.enAlignStyle align)
	{
		m_Text.Set(font, text, color);
		m_Text.AlignStyle = align;
		m_Text.Rect = new Rect(Rect.x + m_offset.x, Rect.y + m_offset.y, Rect.width, Rect.height);
		m_Text.SetParent(this);
	}

	public void SetText(string font, string text, Color color, FrUIText.enAlignStyle align, float width)
	{
		m_Text.Set(font, text, color, width);
		m_Text.AlignStyle = align;
		m_Text.Rect = new Rect(Rect.x + m_offset.x, Rect.y + m_offset.y, Rect.width, Rect.height);
		m_Text.SetParent(this);
	}

	public void SetTextLineSpacing(float lineSpacing)
	{
		m_Text.LineSpacing = lineSpacing;
	}

	public void SetTextOffset(float x, float y)
	{
		m_offset = new Vector2(x, y);
	}

	public void SetText(string text)
	{
		m_Text.SetText(text);
	}

	public void SetTextColor(Color color)
	{
		m_Text.SetColor(color);
	}

	public override void Draw()
	{
		base.Draw();
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

	public void SetSize(Vector2 size, float zoom)
	{
		SetSize(size);
		m_Text.SetSize(zoom, false);
	}

	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		if (m_SpriteSet != null)
		{
			for (int i = 0; i < m_SpriteSet.Length; i++)
			{
				m_SpriteSet[i].SetClip(clip_rect);
			}
		}
		m_Text.SetClip(clip_rect);
	}
}
