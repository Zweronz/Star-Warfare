using UnityEngine;

public class Sprite
{
	public const int MinLayer = 0;

	public const int MaxLayer = 15;

	protected UnitUI m_ui;

	protected int m_frameIdx;

	protected int m_moduleIdx;

	protected Vector2 m_Size;

	protected Vector2 m_Position;

	protected bool m_bScaleWithInt;

	protected float m_Rotation;

	protected int m_Layer;

	protected Material m_Material;

	protected int m_MaterialId;

	protected bool m_bLoadMateMode;

	protected Rect m_TextureRect;

	protected bool m_FlipX;

	protected bool m_FlipY;

	protected Color m_Color;

	protected Vector3[] m_Vertices = new Vector3[4];

	protected bool m_UpdateVertices;

	protected Vector3[] m_UV = new Vector3[4];

	protected bool m_UpdateUV;

	protected static int[] m_Triangles;

	public UnitUI UI
	{
		get
		{
			return m_ui;
		}
		set
		{
			m_ui = value;
		}
	}

	public int FrameIdx
	{
		get
		{
			return m_frameIdx;
		}
		set
		{
			m_frameIdx = value;
		}
	}

	public int ModuleIdx
	{
		get
		{
			return m_moduleIdx;
		}
		set
		{
			m_moduleIdx = value;
		}
	}

	public Vector2 Size
	{
		get
		{
			return m_Size;
		}
		set
		{
			m_Size = value;
			m_UpdateVertices = true;
		}
	}

	public Vector2 Position
	{
		get
		{
			return m_Position;
		}
		set
		{
			m_Position = value;
			m_UpdateVertices = true;
		}
	}

	public bool ScaleWithInt
	{
		get
		{
			return m_bScaleWithInt;
		}
		set
		{
			m_bScaleWithInt = value;
		}
	}

	public float Rotation
	{
		get
		{
			return m_Rotation;
		}
		set
		{
			m_Rotation = value;
			m_UpdateVertices = true;
		}
	}

	public int Layer
	{
		get
		{
			return m_Layer;
		}
		set
		{
			if (value >= 0 && value <= 15)
			{
				m_Layer = value;
				m_UpdateVertices = true;
			}
		}
	}

	public Material Material
	{
		get
		{
			return m_Material;
		}
		set
		{
			m_Material = value;
			m_UpdateUV = true;
		}
	}

	public int MaterialID
	{
		get
		{
			return m_MaterialId;
		}
		set
		{
			m_MaterialId = value;
			m_UpdateUV = true;
		}
	}

	public bool LoadMateMode
	{
		get
		{
			return m_bLoadMateMode;
		}
		set
		{
			m_bLoadMateMode = value;
		}
	}

	public Rect TextureRect
	{
		get
		{
			return m_TextureRect;
		}
		set
		{
			m_TextureRect = value;
			m_UpdateUV = true;
		}
	}

	public bool FlipX
	{
		get
		{
			return m_FlipX;
		}
		set
		{
			m_FlipX = value;
			m_UpdateUV = true;
		}
	}

	public bool FlipY
	{
		get
		{
			return m_FlipY;
		}
		set
		{
			m_FlipY = value;
			m_UpdateUV = true;
		}
	}

	public Color Color
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
		}
	}

	public Vector3[] Vertices
	{
		get
		{
			if (m_UpdateVertices)
			{
				UpdateVertices();
			}
			return m_Vertices;
		}
	}

	public Vector3[] UV
	{
		get
		{
			if (m_UpdateUV)
			{
				UpdateUV();
			}
			return m_UV;
		}
	}

	public static int[] Triangles
	{
		get
		{
			return m_Triangles;
		}
	}

	public Sprite()
	{
		m_Position = Vector2.zero;
		m_Size = Vector2.zero;
		m_Rotation = 0f;
		m_Layer = 0;
		m_MaterialId = -1;
		m_bLoadMateMode = true;
		m_TextureRect = new Rect(0f, 0f, 0f, 0f);
		m_FlipX = false;
		m_FlipY = false;
		m_Color = Color.white;
		m_Vertices[0] = Vector3.zero;
		m_Vertices[1] = Vector3.zero;
		m_Vertices[2] = Vector3.zero;
		m_Vertices[3] = Vector3.zero;
		m_UpdateVertices = true;
		m_UV[0] = Vector3.zero;
		m_UV[1] = Vector3.zero;
		m_UV[2] = Vector3.zero;
		m_UV[3] = Vector3.zero;
		m_UpdateUV = true;
	}

	static Sprite()
	{
		m_Triangles = new int[6];
		m_Triangles[0] = 0;
		m_Triangles[1] = 3;
		m_Triangles[2] = 1;
		m_Triangles[3] = 3;
		m_Triangles[4] = 2;
		m_Triangles[5] = 1;
	}

	~Sprite()
	{
	}

	protected virtual void UpdateVertices()
	{
		Rect rect;
		float num;
		float num2;
		float num3;
		float num4;
		if (UIConstant.ScreenAdaptived.x == 1f && UIConstant.ScreenAdaptived.y == 1f)
		{
			rect = new Rect((int)(m_Position.x - m_Size.x / 2f), (int)(m_Position.y - m_Size.y / 2f), m_Size.x, m_Size.y);
			num = m_Position.x;
			num2 = m_Position.y;
			num3 = m_Size.x;
			num4 = m_Size.y;
		}
		else
		{
			float num5 = (m_Position.x - UIConstant.ScreenLocalWidth * 0.5f) * UIConstant.ScreenAdaptived.x;
			float num6 = (m_Position.y - UIConstant.ScreenLocalHeight * 0.5f) * UIConstant.ScreenAdaptived.y;
			num3 = m_Size.x * UIConstant.ScreenAdaptived.x;
			num4 = m_Size.y * UIConstant.ScreenAdaptived.y;
			num = (float)Screen.width * 0.5f + num5;
			num2 = (float)Screen.height * 0.5f + num6;
			rect = ((!m_bScaleWithInt) ? new Rect(num - num3 * 0.5f, num2 - num4 * 0.5f, num3, num4) : new Rect((int)(num - num3 * 0.5f), (int)(num2 - num4 * 0.5f), (int)num3, (int)num4));
		}
		if (m_Rotation == 0f)
		{
			m_Vertices[0] = new Vector3(rect.xMin, rect.yMax, 0f);
			m_Vertices[1] = new Vector3(rect.xMax, rect.yMax, 0f);
			m_Vertices[2] = new Vector3(rect.xMax, rect.yMin, 0f);
			m_Vertices[3] = new Vector3(rect.xMin, rect.yMin, 0f);
		}
		else
		{
			float num7 = num3 / 2f;
			float num8 = num4 / 2f;
			float num9 = Mathf.Sin(m_Rotation);
			float num10 = Mathf.Cos(m_Rotation);
			m_Vertices[0] = new Vector3(num + ((0f - num7) * num10 - num8 * num9), num2 + ((0f - num7) * num9 + num8 * num10), 0f);
			m_Vertices[1] = new Vector3(num + (num7 * num10 - num8 * num9), num2 + (num7 * num9 + num8 * num10), 0f);
			m_Vertices[2] = new Vector3(num + (num7 * num10 + num8 * num9), num2 + (num7 * num9 - num8 * num10), 0f);
			m_Vertices[3] = new Vector3(num + ((0f - num7) * num10 + num8 * num9), num2 + ((0f - num7) * num9 - num8 * num10), 0f);
		}
		m_UpdateVertices = false;
	}

	protected virtual void UpdateUV()
	{
		float num = 1f;
		float num2 = 1f;
		if (m_bLoadMateMode)
		{
			num = 1f / (float)Res2DManager.GetInstance().vMaterial[m_MaterialId].GetMaterial().mainTexture.width;
			num2 = 1f / (float)Res2DManager.GetInstance().vMaterial[m_MaterialId].GetMaterial().mainTexture.height;
		}
		else
		{
			num = 1f / (float)m_Material.mainTexture.width;
			num2 = 1f / (float)m_Material.mainTexture.height;
		}
		float x = m_TextureRect.xMin * num;
		float x2 = m_TextureRect.xMax * num;
		float y = 1f - m_TextureRect.yMax * num2;
		float y2 = 1f - m_TextureRect.yMin * num2;
		if (!m_FlipX && !m_FlipY)
		{
			m_UV[0] = new Vector2(x, y2);
			m_UV[1] = new Vector2(x2, y2);
			m_UV[2] = new Vector2(x2, y);
			m_UV[3] = new Vector2(x, y);
		}
		else if (m_FlipX && !m_FlipY)
		{
			m_UV[0] = new Vector2(x2, y2);
			m_UV[1] = new Vector2(x, y2);
			m_UV[2] = new Vector2(x, y);
			m_UV[3] = new Vector2(x2, y);
		}
		else if (!m_FlipX && m_FlipY)
		{
			m_UV[0] = new Vector2(x, y);
			m_UV[1] = new Vector2(x2, y);
			m_UV[2] = new Vector2(x2, y2);
			m_UV[3] = new Vector2(x, y2);
		}
		else
		{
			m_UV[0] = new Vector2(x2, y);
			m_UV[1] = new Vector2(x, y);
			m_UV[2] = new Vector2(x, y2);
			m_UV[3] = new Vector2(x2, y2);
		}
		m_UpdateUV = false;
	}
}
