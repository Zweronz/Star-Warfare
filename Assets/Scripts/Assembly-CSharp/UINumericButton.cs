using UnityEngine;

public class UINumericButton : UIClickButton, UIContainer
{
	public static Color fontColor_orange = new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f);

	public static Color fontColor_yellow = new Color(0.7921569f, 0.5294118f, 7f / 85f, 1f);

	protected UINumeric m_num = new UINumeric();

	protected Color m_NormalColor = fontColor_orange;

	protected Color m_PressedColor = fontColor_yellow;

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
			m_num.Rect = Rect;
		}
	}

	public void SetNumeric(UnitUI ui, int frame, string numeric)
	{
		m_num.SetNumeric(ui, frame, numeric);
		m_num.AlignStyle = UINumeric.enAlignStyle.center;
		m_num.Rect = Rect;
		m_num.SetParent(this);
	}

	public void SetNumeric(UnitUI ui, int frame, string numeric, float spacingOffset)
	{
		m_num.SpacingOffsetX = spacingOffset;
		m_num.SetNumeric(ui, frame, numeric);
		m_num.AlignStyle = UINumeric.enAlignStyle.center;
		m_num.Rect = Rect;
		m_num.SetParent(this);
	}

	public void SetTextColor(Color normalColor, Color pressedColor)
	{
		m_NormalColor = normalColor;
		m_PressedColor = pressedColor;
	}

	public void SetNumRect(Rect rect)
	{
		m_num.Rect = rect;
	}

	public void SetTextAlignment(UINumeric.enAlignStyle type)
	{
		m_num.AlignStyle = type;
		m_num.Rect = Rect;
	}

	public override void Draw()
	{
		base.Draw();
		if (m_State == State.Normal)
		{
			m_num.SetColor(m_NormalColor);
		}
		else if (m_State == State.Pressed)
		{
			m_num.SetColor(m_PressedColor);
		}
		m_num.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_num != null)
		{
			m_num.Destory();
		}
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
		Vector2 originalSize = m_num.GetOriginalSize();
		int num = (int)(originalSize.x * zoom);
		int num2 = (int)(originalSize.y * zoom);
		m_num.SetSize(new Vector2(num, num2));
	}

	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		m_num.SetClip(clip_rect);
	}
}
