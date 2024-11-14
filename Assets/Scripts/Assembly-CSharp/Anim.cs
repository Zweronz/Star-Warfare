using System.IO;
using UnityEngine;

public class Anim
{
	private short[][] sData;

	private Vector2[][] pos;

	public void Load(BinaryReader br)
	{
		try
		{
			sData = new short[1][];
			pos = new Vector2[1][];
			for (int i = 0; i < 1; i++)
			{
				int num = br.ReadInt16();
				sData[i] = new short[num];
				pos[i] = new Vector2[num];
				for (int j = 0; j < num; j++)
				{
					sData[i][j] = br.ReadInt16();
					pos[i][j].x = br.ReadInt16();
					pos[i][j].y = br.ReadInt16();
				}
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

	public int GetIndex(int aIndex, int fIndex)
	{
		return (sData[0][fIndex] >> 4) & 0xFFF;
	}

	public int GetFreq(int aIndex, int fIndex)
	{
		return sData[0][fIndex] & 0xF;
	}

	public Vector2 GetPosition(int aIndex, int fIndex)
	{
		return pos[0][fIndex];
	}

	public int GetLength(int aIndex)
	{
		return sData[0].Length;
	}
}
