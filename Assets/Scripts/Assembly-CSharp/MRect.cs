using System.IO;
using UnityEngine;

public class MRect : IModule
{
	private short[] sWidth;

	private short[] sHeight;

	private bool[] bHasBGColor;

	private int[] nBGColor;

	private byte[] byBorderThick;

	private int[] nBorderColor;

	private short[] sArcLen;

	public void Load(BinaryReader br)
	{
		try
		{
			short num = br.ReadInt16();
			if (num <= 0)
			{
				return;
			}
			sWidth = new short[num];
			sHeight = new short[num];
			bHasBGColor = new bool[num];
			nBGColor = new int[num];
			byBorderThick = new byte[num];
			nBorderColor = new int[num];
			sArcLen = new short[num];
			for (int i = 0; i < num; i++)
			{
				sWidth[i] = br.ReadInt16();
				sHeight[i] = br.ReadInt16();
				if (br.ReadByte() == 1)
				{
					bHasBGColor[i] = true;
				}
				nBGColor[i] = br.ReadInt32();
				byBorderThick[i] = br.ReadByte();
				nBorderColor[i] = br.ReadInt32();
				sArcLen[i] = br.ReadInt16();
			}
		}
		catch
		{
		}
	}

	public void Free()
	{
		sWidth = null;
		sHeight = null;
		bHasBGColor = null;
		nBGColor = null;
		byBorderThick = null;
		nBorderColor = null;
		sArcLen = null;
	}

	public Vector2 GetSize(int index)
	{
		return new Vector2(sWidth[index], sHeight[index]);
	}
}
