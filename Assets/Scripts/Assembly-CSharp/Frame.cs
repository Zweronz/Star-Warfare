using System.IO;
using UnityEngine;

public class Frame
{
	private short[][] sData;

	private Vector2[][] pos;

	private Vector2[] size;

	public void Load(BinaryReader br)
	{
		try
		{
			int num = br.ReadInt16();
			sData = new short[num][];
			pos = new Vector2[num][];
			size = new Vector2[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = br.ReadInt16();
				sData[i] = new short[num2];
				pos[i] = new Vector2[num2];
				for (int j = 0; j < num2; j++)
				{
					sData[i][j] = br.ReadInt16();
					pos[i][j].x = br.ReadInt16();
					pos[i][j].y = br.ReadInt16();
				}
				size[i] = default(Vector2);
				size[i].x = br.ReadInt16();
				size[i].y = br.ReadInt16();
			}
		}
		catch
		{
		}
	}

	public void Free()
	{
		sData = null;
		size = null;
	}

	public int GetCount(int fIndex)
	{
		return sData[fIndex].Length;
	}

	public int GetIndex(int fIndex, int mIndex)
	{
		return sData[fIndex][mIndex] & 0xFFF;
	}

	public int GetType(int fIndex, int mIndex)
	{
		return (sData[fIndex][mIndex] >> 12) & 0xF;
	}

	public Vector2 GetPosition(int fIndex, int mIndex)
	{
		return pos[fIndex][mIndex];
	}

	public Vector2 GetSize(int fIndex)
	{
		return size[fIndex];
	}
}
