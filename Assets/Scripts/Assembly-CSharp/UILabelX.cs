using System.Collections;
using UnityEngine;

public class UILabelX : UIControlVisible
{
	private UIFontInfo m_Font;

	protected Color m_Color;

	protected float m_CharacterSpacing;

	protected float m_LineSpacing;

	protected string m_Text;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			UpdateText();
		}
	}

	public UILabelX()
	{
		m_Font = null;
		m_Color = Color.white;
		m_CharacterSpacing = 0f;
		m_LineSpacing = 0f;
		m_Text = null;
	}

	public void SetFont(UIFontInfo font)
	{
		m_Font = font;
		UpdateText();
	}

	public void SetColor(Color color)
	{
		m_Color = color;
		if (m_SpriteSet != null)
		{
			for (int i = 0; i < m_SpriteSet.Length; i++)
			{
				SetSpriteColor(i, color);
			}
		}
	}

	public void SetCharacterSpacing(float character_spacing)
	{
		m_CharacterSpacing = character_spacing;
		UpdateText();
	}

	public void SetLineSpacing(float line_spacing)
	{
		m_LineSpacing = line_spacing;
		UpdateText();
	}

	public void SetText(string text)
	{
		m_Text = text;
		UpdateText();
	}

	public override void Draw()
	{
		if (m_SpriteSet == null)
		{
			return;
		}
		for (int i = 0; i < m_SpriteSet.Length; i++)
		{
			for (int j = 0; j < m_SpriteSet[i].m_Sprite.Count; j++)
			{
				m_Parent.DrawSprite((UISpriteX)m_SpriteSet[i].m_Sprite[j]);
			}
		}
	}

	public override void Destory()
	{
		if (m_SpriteSet == null)
		{
			return;
		}
		for (int i = 0; i < m_SpriteSet.Length; i++)
		{
			if (m_SpriteSet[i] != null)
			{
				m_SpriteSet[i].Destory();
			}
		}
	}

	private void UpdateText()
	{
		m_SpriteSet = null;
		if (m_Font == null || m_Text == null || m_Text.Length <= 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		float num = 0f;
		float num2 = 0f;
		if (num2 + m_Font.GetHeight() > m_Rect.height)
		{
			return;
		}
		for (int i = 0; i < m_Text.Length; i++)
		{
			char c = m_Text[i];
			if (c == '\n' || c == '\r')
			{
				num = 0f;
				num2 += m_Font.GetHeight() + m_LineSpacing;
				if (num2 + m_Font.GetHeight() > m_Rect.height)
				{
					break;
				}
				continue;
			}
			float width = m_Font.GetWidth(c);
			if (num + width > m_Rect.width)
			{
				break;
			}
			UISpriteX uISpriteX = new UISpriteX();
			uISpriteX.LoadMateMode = false;
			uISpriteX.Position = new Vector2(m_Rect.x + num + width / 2f, m_Rect.y + num2 + m_Font.GetHeight() / 2f);
			uISpriteX.Size = new Vector2(width, m_Font.GetHeight());
			uISpriteX.Material = m_Font.GetMaterial();
			uISpriteX.TextureRect = m_Font.GetUVRect(c);
			uISpriteX.Color = m_Color;
			if (m_Clip)
			{
				uISpriteX.SetClip(m_ClipRect);
			}
			arrayList.Add(uISpriteX);
			num += width + m_CharacterSpacing;
		}
		m_SpriteSet = new UISpriteSet[arrayList.Count];
		for (int j = 0; j < arrayList.Count; j++)
		{
			m_SpriteSet[j].m_Sprite.Add((UISpriteX)arrayList[j]);
		}
	}

	public int GetTextWidth(string text)
	{
		if (m_Font == null)
		{
			return 0;
		}
		return m_Font.GetTextWidth(text);
	}
}
