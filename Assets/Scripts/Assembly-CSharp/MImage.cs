using System.IO;
using UnityEngine;

public class MImage : IModule
{
	private short[] sData;

	private Rect[] rect;

	public void Load(BinaryReader br)
	{
		try
		{
			short num = br.ReadInt16();
			rect = new Rect[num];
			sData = new short[num];
			for (int i = 0; i < num; i++)
			{
				sData[i] = br.ReadInt16();
				rect[i] = new Rect(br.ReadInt16(), br.ReadInt16(), br.ReadInt16(), br.ReadInt16());
			}
		}
		catch
		{
		}
	}

	public void Free()
	{
		sData = null;
	}

	public int GetMaterial(int index)
	{
		return sData[index] >> 4;
	}

	public int GetRotate(int index)
	{
		return sData[index] & 0xF;
	}

	public Rect GetRect(int index)
	{
		return rect[index];
	}

	public Vector2 GetSize(int index)
	{
		return new Vector2(rect[index].width, rect[index].height);
	}
}
