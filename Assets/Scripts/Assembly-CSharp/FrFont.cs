using System.Collections;
using UnityEngine;

public class FrFont
{
	public Material _Material;

	private int _texWidth;

	private int _texHeight;

	private int _cellWidth;

	private int _cellHeight;

	private int _cellxOffset;

	private int _cellyOffset;

	public ArrayList _widths = new ArrayList();

	public int TextureWidth
	{
		get
		{
			return _texWidth;
		}
		set
		{
			_texWidth = value;
		}
	}

	public int TextureHeight
	{
		get
		{
			return _texHeight;
		}
		set
		{
			_texHeight = value;
		}
	}

	public int CellWidth
	{
		get
		{
			return _cellWidth;
		}
		set
		{
			_cellWidth = value;
		}
	}

	public int CellHeight
	{
		get
		{
			return _cellHeight;
		}
		set
		{
			_cellHeight = value;
		}
	}

	public int OffsetX
	{
		get
		{
			return _cellxOffset;
		}
		set
		{
			_cellxOffset = value;
		}
	}

	public int OffsetY
	{
		get
		{
			return _cellyOffset;
		}
		set
		{
			_cellyOffset = value;
		}
	}

	public Material getTexture()
	{
		return _Material;
	}

	public float getCharWidth(char c)
	{
		int index = c - 32;
		return ((float)_widths[index] + (float)(_cellxOffset * 2)) * ResolutionConstant.R;
	}

	public float GetTextWidth(string text)
	{
		float num = 0f;
		foreach (char c in text)
		{
			num += getCharWidth(c);
		}
		return num;
	}

	public float GetTextWidth(string text, float CharacterSpacing)
	{
		float num = 0f;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			num += getCharWidth(c);
			if (i < text.Length - 1)
			{
				num += CharacterSpacing;
			}
		}
		return num;
	}
}
