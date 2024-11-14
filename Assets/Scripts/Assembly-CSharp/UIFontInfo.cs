using UnityEngine;

public class UIFontInfo
{
	protected Material m_Material;

	protected TextAsset m_Conf;

	protected Rect[] m_UVRect;

	protected float m_Height;

	protected float[] m_Width;

	public UIFontInfo(string name, Material material, TextAsset conf)
	{
		m_Material = material;
		m_Conf = conf;
		Configure configure = new Configure();
		configure.Load(conf.text);
		m_Height = int.Parse(configure.GetSingle(name, "height"));
		m_UVRect = new Rect[512];
		for (int i = 0; i < 512; i++)
		{
			m_UVRect[i] = new Rect(0f, 0f, 0f, 0f);
		}
		m_Width = new float[512];
		for (int j = 0; j < 512; j++)
		{
			m_Width[j] = 0f;
		}
		int num = configure.CountArray2(name, "chars");
		for (int k = 0; k < num; k++)
		{
			int num2 = int.Parse(configure.GetArray2(name, "chars", k, 0));
			Rect rect = new Rect(0f, 0f, 0f, 0f)
			{
				x = float.Parse(configure.GetArray2(name, "chars", k, 2)),
				y = float.Parse(configure.GetArray2(name, "chars", k, 3)),
				width = float.Parse(configure.GetArray2(name, "chars", k, 4)),
				height = float.Parse(configure.GetArray2(name, "chars", k, 5))
			};
			m_Width[num2] = rect.width;
			m_UVRect[num2] = rect;
		}
	}

	public Material GetMaterial()
	{
		return m_Material;
	}

	public TextAsset GetConf()
	{
		return m_Conf;
	}

	public Rect GetUVRect(char ch)
	{
		int num = ch;
		if (num >= m_UVRect.Length)
		{
			num = 0;
		}
		return m_UVRect[num];
	}

	public float GetHeight()
	{
		return m_Height;
	}

	public float GetWidth(char ch)
	{
		int num = ch;
		if (num >= m_Width.Length)
		{
			num = 0;
		}
		return m_Width[num];
	}

	public int GetTextWidth(string text)
	{
		int num = 0;
		foreach (char ch in text)
		{
			num += (int)GetWidth(ch);
		}
		return num;
	}
}
