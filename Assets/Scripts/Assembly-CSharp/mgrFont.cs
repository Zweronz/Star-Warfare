using System.Collections;
using UnityEngine;

public class mgrFont
{
	protected static mgrFont _mgrFontInstance;

	protected Hashtable _fonts;

	public static mgrFont Instance()
	{
		if (_mgrFontInstance == null)
		{
			_mgrFontInstance = new mgrFont();
		}
		return _mgrFontInstance;
	}

	public FrFont getFont(string fontName)
	{
		if (_fonts == null)
		{
			_fonts = new Hashtable();
		}
		if (_fonts.Contains(fontName))
		{
			return (FrFont)_fonts[fontName];
		}
		FrFont frFont = new FrFont();
		frFont._Material = Resources.Load("font/" + fontName + "_M") as Material;
		if (null == frFont._Material)
		{
			return null;
		}
		TextAsset textAsset = Resources.Load("font/" + fontName + "_cfg") as TextAsset;
		if (null != textAsset && textAsset.text != null)
		{
			string[] array = textAsset.text.Split('\n');
			string[] array2 = array[0].Split(' ');
			frFont.TextureWidth = int.Parse(array2[0]);
			frFont.TextureHeight = int.Parse(array2[1]);
			frFont.CellWidth = int.Parse(array2[2]);
			frFont.CellHeight = int.Parse(array2[3]);
			frFont.OffsetX = int.Parse(array2[4]);
			frFont.OffsetY = int.Parse(array2[5]);
			string[] array3 = array[1].Split(' ');
			for (int i = 0; i < array3.Length; i++)
			{
				frFont._widths.Add(float.Parse(array3[i]));
			}
			_fonts[fontName] = frFont;
			return frFont;
		}
		return null;
	}
}
