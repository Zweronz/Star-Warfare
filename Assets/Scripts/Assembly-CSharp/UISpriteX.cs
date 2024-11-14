using UnityEngine;

public class UISpriteX : Sprite
{
	protected bool m_Clip;

	protected Vector2[] m_ClipOffs = new Vector2[4];

	protected Rect m_ClipRect;

	protected Rect m_UVClipRect;

	public new int Layer
	{
		get
		{
			return base.Layer;
		}
	}

	public new Vector2 Size
	{
		get
		{
			return m_Size;
		}
		set
		{
			m_Size = value;
			m_UpdateVertices = true;
			if (m_Clip)
			{
				m_UpdateUV = true;
			}
		}
	}

	public new Vector2 Position
	{
		get
		{
			return m_Position;
		}
		set
		{
			m_Position = value;
			m_UpdateVertices = true;
			if (m_Clip)
			{
				m_UpdateUV = true;
			}
		}
	}

	public UISpriteX()
	{
		base.Layer = 0;
		m_bScaleWithInt = false;
		m_ClipOffs[0] = Vector2.zero;
		m_ClipOffs[1] = Vector2.zero;
		m_ClipOffs[2] = Vector2.zero;
		m_ClipOffs[3] = Vector2.zero;
	}

	public void SetClip(Rect clip_rect)
	{
		m_Clip = true;
		if (UIConstant.ScreenAdaptived.x == 1f && UIConstant.ScreenAdaptived.y == 1f)
		{
			m_ClipRect = clip_rect;
			m_UVClipRect = clip_rect;
		}
		else
		{
			float num = UIConstant.ScreenLocalWidth * 0.5f;
			float num2 = UIConstant.ScreenLocalHeight * 0.5f;
			float num3 = (float)Screen.width * 0.5f + (clip_rect.xMin - num) * UIConstant.ScreenAdaptived.x;
			float num4 = (float)Screen.width * 0.5f + (clip_rect.xMax - num) * UIConstant.ScreenAdaptived.x;
			float num5 = (float)Screen.height * 0.5f + (clip_rect.yMin - num2) * UIConstant.ScreenAdaptived.y;
			float num6 = (float)Screen.height * 0.5f + (clip_rect.yMax - num2) * UIConstant.ScreenAdaptived.y;
			if (base.ScaleWithInt)
			{
				m_ClipRect = new Rect((int)num3, (int)num5, (int)(num4 - num3), (int)(num6 - num5));
			}
			else
			{
				m_ClipRect = new Rect(num3, num5, num4 - num3, num6 - num5);
			}
			m_UVClipRect = clip_rect;
		}
		m_UpdateVertices = true;
		m_UpdateUV = true;
	}

	public void SetClipOffs(int index, Vector2 clip_offs)
	{
		clip_offs.x = UIConstant.GetWidthForScreenAdaptived((int)clip_offs.x);
		clip_offs.y = UIConstant.GetHeightForScreenAdaptived((int)clip_offs.y);
		m_ClipOffs[index] = clip_offs;
	}

	public void ClearClip()
	{
		m_Clip = false;
		m_UpdateVertices = true;
		m_UpdateUV = true;
	}

	protected override void UpdateVertices()
	{
		if (m_Clip)
		{
			UpdateClipVertices();
		}
		else
		{
			base.UpdateVertices();
		}
	}

	protected override void UpdateUV()
	{
		if (m_Clip)
		{
			UpdateClipUV();
		}
		else
		{
			base.UpdateUV();
		}
	}

	protected void UpdateClipVertices()
	{
		Rect rect;
		if (UIConstant.ScreenAdaptived.x == 1f && UIConstant.ScreenAdaptived.y == 1f)
		{
			rect = new Rect((int)(m_Position.x - m_Size.x / 2f), (int)(m_Position.y - m_Size.y / 2f), m_Size.x, m_Size.y);
		}
		else
		{
			rect = new Rect(m_Position.x - m_Size.x * 0.5f, m_Position.y - m_Size.y * 0.5f, m_Size.x, m_Size.y);
			float num = UIConstant.ScreenLocalWidth * 0.5f;
			float num2 = UIConstant.ScreenLocalHeight * 0.5f;
			float num3 = (float)Screen.width * 0.5f + (rect.xMin - num) * UIConstant.ScreenAdaptived.x;
			float num4 = (float)Screen.width * 0.5f + (rect.xMax - num) * UIConstant.ScreenAdaptived.x;
			float num5 = (float)Screen.height * 0.5f + (rect.yMin - num2) * UIConstant.ScreenAdaptived.y;
			float num6 = (float)Screen.height * 0.5f + (rect.yMax - num2) * UIConstant.ScreenAdaptived.y;
			rect = ((!base.ScaleWithInt) ? new Rect(num3, num5, num4 - num3, num6 - num5) : new Rect((int)num3, (int)num5, (int)(num4 - num3), (int)(num6 - num5)));
		}
		if (m_ClipRect.xMin > rect.xMax || m_ClipRect.xMax < rect.xMin || m_ClipRect.yMin > rect.yMax || m_ClipRect.yMax < rect.yMin)
		{
			m_Vertices[0] = (m_Vertices[1] = (m_Vertices[2] = (m_Vertices[3] = new Vector3(-1f, -1f, 0f))));
			m_UpdateVertices = false;
			return;
		}
		if (m_ClipRect.xMin > rect.xMin)
		{
			rect.xMin = m_ClipRect.xMin;
		}
		if (m_ClipRect.xMax < rect.xMax)
		{
			rect.xMax = m_ClipRect.xMax;
		}
		if (m_ClipRect.yMin > rect.yMin)
		{
			rect.yMin = m_ClipRect.yMin;
		}
		if (m_ClipRect.yMax < rect.yMax)
		{
			rect.yMax = m_ClipRect.yMax;
		}
		Vector2 zero = Vector2.zero;
		zero.x = rect.xMin;
		zero.y = rect.yMax;
		Vector2 zero2 = Vector2.zero;
		zero2.x = rect.xMax;
		zero2.y = rect.yMax;
		Vector2 zero3 = Vector2.zero;
		zero3.x = rect.xMax;
		zero3.y = rect.yMin;
		Vector2 zero4 = Vector2.zero;
		zero4.x = rect.xMin;
		zero4.y = rect.yMin;
		if (m_ClipRect.xMin + m_ClipOffs[0].x > rect.xMin)
		{
			zero.x = m_ClipRect.xMin + m_ClipOffs[0].x;
		}
		if (m_ClipRect.yMax + m_ClipOffs[0].y < rect.yMax)
		{
			zero.y = m_ClipRect.yMax + m_ClipOffs[0].y;
		}
		if (m_ClipRect.xMax + m_ClipOffs[1].x < rect.xMax)
		{
			zero2.x = m_ClipRect.xMax + m_ClipOffs[1].x;
		}
		if (m_ClipRect.yMax + m_ClipOffs[1].y < rect.yMax)
		{
			zero2.y = m_ClipRect.yMax + m_ClipOffs[1].y;
		}
		if (m_ClipRect.xMax + m_ClipOffs[2].x < rect.xMax)
		{
			zero3.x = m_ClipRect.xMax + m_ClipOffs[2].x;
		}
		if (m_ClipRect.yMin + m_ClipOffs[2].y > rect.yMin)
		{
			zero3.y = m_ClipRect.yMin + m_ClipOffs[2].y;
		}
		if (m_ClipRect.xMin + m_ClipOffs[3].x > rect.xMin)
		{
			zero4.x = m_ClipRect.xMin + m_ClipOffs[3].x;
		}
		if (m_ClipRect.yMin + m_ClipOffs[3].y > rect.yMin)
		{
			zero4.y = m_ClipRect.yMin + m_ClipOffs[3].y;
		}
		if (m_Rotation == 0f)
		{
			m_Vertices[0] = new Vector3(zero.x, zero.y, 0f);
			m_Vertices[1] = new Vector3(zero2.x, zero2.y, 0f);
			m_Vertices[2] = new Vector3(zero3.x, zero3.y, 0f);
			m_Vertices[3] = new Vector3(zero4.x, zero4.y, 0f);
		}
		else
		{
			float num7 = Mathf.Sin(m_Rotation);
			float num8 = Mathf.Cos(m_Rotation);
			float num9 = m_Position.x - rect.xMin;
			float num10 = rect.yMax - m_Position.y;
			m_Vertices[0] = new Vector3(m_Position.x + ((0f - num9) * num8 - num10 * num7), m_Position.y + ((0f - num9) * num7 + num10 * num8), 0f);
			num9 = rect.xMax - m_Position.x;
			num10 = rect.yMax - m_Position.y;
			m_Vertices[1] = new Vector3(m_Position.x + (num9 * num8 - num10 * num7), m_Position.y + (num9 * num7 + num10 * num8), 0f);
			num9 = rect.xMax - m_Position.x;
			num10 = m_Position.y - rect.yMin;
			m_Vertices[2] = new Vector3(m_Position.x + (num9 * num8 + num10 * num7), m_Position.y + (num9 * num7 - num10 * num8), 0f);
			num9 = m_Position.x - rect.xMin;
			num10 = m_Position.y - rect.yMin;
			m_Vertices[3] = new Vector3(m_Position.x + ((0f - num9) * num8 + num10 * num7), m_Position.y + ((0f - num9) * num7 - num10 * num8), 0f);
		}
		m_UpdateVertices = false;
	}

	protected void UpdateClipUV()
	{
		Rect rect = new Rect((int)(m_Position.x - m_Size.x / 2f), (int)(m_Position.y - m_Size.y / 2f), m_Size.x, m_Size.y);
		if (m_UVClipRect.xMin > rect.xMax || m_UVClipRect.xMax < rect.xMin || m_UVClipRect.yMin > rect.yMax || m_UVClipRect.yMax < rect.yMin)
		{
			m_UV[0] = (m_UV[1] = (m_UV[2] = (m_UV[3] = new Vector2(0f, 0f))));
			m_UpdateVertices = false;
			return;
		}
		Rect textureRect = m_TextureRect;
		Vector2 zero = Vector2.zero;
		zero.x = m_TextureRect.xMin;
		zero.y = m_TextureRect.yMax;
		Vector2 zero2 = Vector2.zero;
		zero2.x = m_TextureRect.xMax;
		zero2.y = m_TextureRect.yMax;
		Vector2 zero3 = Vector2.zero;
		zero3.x = m_TextureRect.xMax;
		zero3.y = m_TextureRect.yMin;
		Vector2 zero4 = Vector2.zero;
		zero4.x = m_TextureRect.xMin;
		zero4.y = m_TextureRect.yMin;
		float num = m_UVClipRect.xMin + m_ClipOffs[0].x - rect.xMin;
		if (num > 0f)
		{
			zero.x += num;
		}
		num = m_UVClipRect.yMin + m_ClipOffs[3].y - rect.yMin;
		if (num > 0f)
		{
			zero.y -= num;
		}
		num = rect.xMax - (m_UVClipRect.xMax + m_ClipOffs[1].x);
		if (num > 0f)
		{
			zero2.x -= num;
		}
		num = m_UVClipRect.yMin + m_ClipOffs[2].y - rect.yMin;
		if (num > 0f)
		{
			zero2.y -= num;
		}
		num = rect.xMax - (m_UVClipRect.xMax + m_ClipOffs[2].x);
		if (num > 0f)
		{
			zero3.x -= num;
		}
		num = rect.yMax - (m_UVClipRect.yMax + m_ClipOffs[1].y);
		if (num > 0f)
		{
			zero3.y += num;
		}
		num = m_UVClipRect.xMin + m_ClipOffs[3].x - rect.xMin;
		if (num > 0f)
		{
			zero4.x += num;
		}
		num = rect.yMax - (m_UVClipRect.yMax + m_ClipOffs[0].y);
		if (num > 0f)
		{
			zero4.y += num;
		}
		float num2 = 1f;
		float num3 = 1f;
		if (m_bLoadMateMode)
		{
			num2 = 1f / (float)Res2DManager.GetInstance().vMaterial[m_MaterialId].GetMaterial().mainTexture.width;
			num3 = 1f / (float)Res2DManager.GetInstance().vMaterial[m_MaterialId].GetMaterial().mainTexture.height;
		}
		else
		{
			num2 = 1f / (float)m_Material.mainTexture.width;
			num3 = 1f / (float)m_Material.mainTexture.height;
		}
		float x = zero.x * num2;
		float y = 1f - zero4.y * num3;
		float x2 = zero2.x * num2;
		float y2 = 1f - zero3.y * num3;
		float x3 = zero3.x * num2;
		float y3 = 1f - zero2.y * num3;
		float x4 = zero4.x * num2;
		float y4 = 1f - zero.y * num3;
		if (!m_FlipX && !m_FlipY)
		{
			m_UV[0] = new Vector2(x, y);
			m_UV[1] = new Vector2(x2, y2);
			m_UV[2] = new Vector2(x3, y3);
			m_UV[3] = new Vector2(x4, y4);
		}
		else if (m_FlipX && !m_FlipY)
		{
			m_UV[0] = new Vector2(x2, y);
			m_UV[1] = new Vector2(x, y2);
			m_UV[2] = new Vector2(x4, y3);
			m_UV[3] = new Vector2(x3, y4);
		}
		else if (!m_FlipX && m_FlipY)
		{
			m_UV[0] = new Vector2(x, y4);
			m_UV[1] = new Vector2(x2, y3);
			m_UV[2] = new Vector2(x3, y2);
			m_UV[3] = new Vector2(x4, y);
		}
		else
		{
			m_UV[0] = new Vector2(x2, y4);
			m_UV[1] = new Vector2(x, y3);
			m_UV[2] = new Vector2(x4, y2);
			m_UV[3] = new Vector2(x3, y);
		}
		m_UpdateUV = false;
	}
}
