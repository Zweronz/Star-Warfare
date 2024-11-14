using System.Collections;
using UnityEngine;

public class FrUIText : UIControlVisible
{
	public enum enAlignStyle
	{
		TOP_LEFT = 0,
		TOP_CENTER = 1,
		TOP_RIGHT = 2,
		CENTER_CENTER = 3
	}

	private string m_Text;

	private FrFont m_Font;

	private float m_LineSpacing = 1f;

	private float m_CharacterSpacing = 1f;

	private Color m_Color = Color.black;

	private bool m_bIsAutoLine = true;

	private float m_TextHeight;

	private float m_TextWidth;

	private enAlignStyle m_AlignStyle;

	private float m_zoom = 1f;

	public FrFont Font
	{
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;
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
			m_Rect = value;
			UpdateText();
		}
	}

	public float GetTextHeight
	{
		get
		{
			return m_TextHeight;
		}
	}

	public float CharacterSpacing
	{
		get
		{
			return m_CharacterSpacing;
		}
		set
		{
			m_CharacterSpacing = value;
		}
	}

	public float LineSpacing
	{
		get
		{
			return m_LineSpacing;
		}
		set
		{
			m_LineSpacing = value;
		}
	}

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

	public bool AutoLine
	{
		get
		{
			return m_bIsAutoLine;
		}
		set
		{
			m_bIsAutoLine = value;
		}
	}

	~FrUIText()
	{
	}

	public void Set(string font, string text, Color color)
	{
		m_Font = mgrFont.Instance().getFont(font);
		m_Color = color;
		m_Text = text;
		UpdateText();
	}

	public void Set(string font, string text, Color color, float width)
	{
		m_Font = mgrFont.Instance().getFont(font);
		m_Color = color;
		m_Text = text;
		m_TextWidth = width;
		UpdateText();
	}

	public void SetWidth(float width)
	{
		m_TextWidth = width;
	}

	public void SetColor(Color clr)
	{
		m_Color = clr;
		UpdateText();
	}

	public void SetFont(string name)
	{
		m_Font = mgrFont.Instance().getFont(name);
		UpdateText();
	}

	public void SetText(string text)
	{
		m_Text = text;
		UpdateText();
	}

	public string GetText()
	{
		return m_Text;
	}

	public void SetSize(float zoom, bool bUpdate)
	{
		m_zoom = zoom;
		if (bUpdate)
		{
			UpdateText();
		}
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

	private void UpdateText()
	{
		m_SpriteSet = null;
		m_LineSpacing *= ResolutionConstant.R * m_zoom;
		if (m_Font == null || m_Text == null || m_Text.Length <= 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		string[] array = m_Text.Split('\n');
		if (m_bIsAutoLine)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList3 = new ArrayList();
				string[] array2 = array[i].Split(' ');
				string text = string.Empty;
				float num = 0f;
				for (int j = 0; j < array2.Length; j++)
				{
					float num2 = m_Font.GetTextWidth(array2[j], CharacterSpacing) * m_zoom;
					if (num + num2 <= m_TextWidth * 0.95f)
					{
						text += array2[j];
						num += num2;
					}
					else
					{
						text = text.Trim();
						if (string.Empty != text)
						{
							arrayList3.Add(text);
						}
						text = array2[j];
						num = num2;
					}
					text += " ";
					num += CharacterSpacing;
					num += m_Font.GetTextWidth(" ") * m_zoom;
				}
				text = text.Trim();
				if (string.Empty != text)
				{
					arrayList3.Add(text);
				}
				for (int k = 0; k < arrayList3.Count; k++)
				{
					arrayList2.Add(arrayList3[k]);
				}
			}
		}
		else
		{
			for (int l = 0; l < array.Length; l++)
			{
				arrayList2.Add(array[l]);
			}
		}
		float num3 = (float)m_Font.CellHeight + LineSpacing;
		m_TextHeight = num3 * (float)arrayList2.Count;
		int num4 = m_Font.TextureWidth / m_Font.CellWidth;
		for (int m = 0; m < arrayList2.Count; m++)
		{
			float num5 = 0f;
			for (int n = 0; n < ((string)arrayList2[m]).Length; n++)
			{
				char c = ((string)arrayList2[m])[n];
				float num6 = m_Font.getCharWidth(c) * m_zoom;
				int num7 = c - 32;
				int num8 = num7 % num4;
				int num9 = num7 / num4;
				float left = num8 * m_Font.CellWidth;
				float top = num9 * m_Font.CellHeight;
				UISpriteX uISpriteX = new UISpriteX();
				uISpriteX.LoadMateMode = false;
				uISpriteX.ScaleWithInt = true;
				uISpriteX.Position = new Vector2((int)(m_Rect.x + num5 + (float)(m_Font.CellWidth / 2) * ResolutionConstant.R * m_zoom), m_Rect.y + m_Rect.height - (float)(m + 1) * num3 * ResolutionConstant.R * m_zoom + ResolutionConstant.R * m_zoom * (float)m_Font.CellHeight / 2f);
				uISpriteX.Size = new Vector2((float)m_Font.CellWidth * ResolutionConstant.R * m_zoom, (float)m_Font.CellHeight * ResolutionConstant.R * m_zoom);
				uISpriteX.Material = m_Font.getTexture();
				uISpriteX.TextureRect = new Rect(left, top, m_Font.CellWidth, m_Font.CellHeight);
				uISpriteX.Color = m_Color;
				if (m_Clip)
				{
					uISpriteX.SetClip(m_ClipRect);
				}
				arrayList.Add(uISpriteX);
				num5 += num6 + CharacterSpacing;
			}
		}
		if (AlignStyle == enAlignStyle.TOP_CENTER || AlignStyle == enAlignStyle.CENTER_CENTER)
		{
			int num10 = 0;
			for (int num11 = 0; num11 < arrayList2.Count; num11++)
			{
				string text2 = (string)arrayList2[num11];
				float num12 = m_Font.GetTextWidth(text2, CharacterSpacing) * m_zoom;
				float num13 = (Rect.width - num12) / 2f;
				float num14 = 0f;
				if (AlignStyle == enAlignStyle.CENTER_CENTER)
				{
					num14 = (Rect.height - (float)m_Font.CellHeight * ResolutionConstant.R * m_zoom - Rect.height * 0.1f) / 2f;
				}
				for (int num15 = 0; num15 < text2.Length; num15++)
				{
					((UISpriteX)arrayList[num15 + num10]).Position = new Vector2(((UISpriteX)arrayList[num15 + num10]).Position.x + num13, ((UISpriteX)arrayList[num15 + num10]).Position.y - num14);
				}
				num10 += text2.Length;
			}
		}
		else if (AlignStyle == enAlignStyle.TOP_RIGHT)
		{
			int num16 = 0;
			for (int num17 = 0; num17 < arrayList2.Count; num17++)
			{
				string text3 = (string)arrayList2[num17];
				float num18 = m_Font.GetTextWidth(text3, CharacterSpacing) * m_zoom;
				float num19 = Rect.width - num18;
				for (int num20 = 0; num20 < text3.Length; num20++)
				{
					((UISpriteX)arrayList[num20 + num16]).Position = new Vector2(((UISpriteX)arrayList[num20 + num16]).Position.x + num19, ((UISpriteX)arrayList[num20 + num16]).Position.y);
				}
				num16 += text3.Length;
			}
		}
		m_SpriteSet = new UISpriteSet[arrayList.Count];
		for (int num21 = 0; num21 < arrayList.Count; num21++)
		{
			m_SpriteSet[num21] = new UISpriteSet();
			m_SpriteSet[num21].m_Sprite.Add((UISpriteX)arrayList[num21]);
		}
	}
}
