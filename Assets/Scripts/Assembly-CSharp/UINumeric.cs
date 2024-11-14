using UnityEngine;

public class UINumeric : UIImage
{
	public enum enAlignStyle
	{
		left = 0,
		center = 1,
		right = 2
	}

	private enAlignStyle m_AlignStyle;

	protected Color m_NormalColor = UIConstant.fontColor_cyan;

	private float m_width;

	private float m_spacingOffsetX = 1f;

	private float m_height;

	public enAlignStyle AlignStyle
	{
		get
		{
			return m_AlignStyle;
		}
		set
		{
			m_AlignStyle = value;
		}
	}

	public float SpacingOffsetX
	{
		get
		{
			return m_spacingOffsetX;
		}
		set
		{
			m_spacingOffsetX = value;
		}
	}

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
			if (m_AlignStyle == enAlignStyle.left)
			{
				position = new Vector2(value.x + m_width * 0.5f, value.y + value.height / 2f);
			}
			else if (m_AlignStyle == enAlignStyle.right)
			{
				position = new Vector2(value.x + value.width - m_width * 0.5f, value.y + value.height / 2f);
			}
			SetSpritePosition(0, position);
		}
	}

	public UINumeric()
	{
		m_AlignStyle = enAlignStyle.center;
	}

	public void SetNumeric(UnitUI ui, int frame, string numeric)
	{
		Set(ui, frame, numeric);
	}

	private void Set(UnitUI ui, int frame, string text)
	{
		Free();
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			int module = c - 48;
			switch (c)
			{
			case ',':
				module = 10;
				break;
			case '/':
				module = 11;
				break;
			case '$':
				module = 12;
				break;
			case '#':
				module = 13;
				break;
			case '.':
				module = 14;
				break;
			case 'k':
				module = 15;
				break;
			case ':':
				module = 16;
				break;
			case 'x':
				module = 17;
				break;
			case '-':
				module = 18;
				break;
			}
			AddObject(ui, frame, module);
			SetSpritePosition(0, i, new Vector2(num, 0f));
			float charWidth = GetCharWidth(c, ui, frame);
			num += charWidth * 0.5f + SpacingOffsetX;
			if (i < text.Length - 1)
			{
				float charWidth2 = GetCharWidth(text[i + 1], ui, frame);
				num += charWidth2 * 0.5f;
			}
			num2 += charWidth + SpacingOffsetX;
		}
		SetScaleWithInt(true);
		m_width = num2;
		Rect = Rect;
	}

	private float GetCharWidth(char ch, UnitUI ui, int frame)
	{
		int mIndex = ch - 48;
		switch (ch)
		{
		case ',':
			mIndex = 10;
			break;
		case '/':
			mIndex = 11;
			break;
		case '$':
			mIndex = 12;
			break;
		case '#':
			mIndex = 13;
			break;
		case '.':
			mIndex = 14;
			break;
		case 'k':
			mIndex = 15;
			break;
		case ':':
			mIndex = 16;
			break;
		case 'x':
			mIndex = 17;
			break;
		case '-':
			mIndex = 18;
			break;
		}
		Rect modulePositionRect = ui.GetModulePositionRect(0, frame, mIndex);
		m_height = modulePositionRect.height;
		return modulePositionRect.width;
	}

	public Vector2 GetOriginalSize()
	{
		return new Vector2(m_width, m_height);
	}

	public new void SetSize(Vector2 size)
	{
		SetSpriteSize(0, size);
	}

	public new void SetColor(Color color)
	{
		SetSpriteColor(0, color);
	}

	public override void Destory()
	{
		base.Destory();
	}
}
