using System.IO;

public class MLine : IModule
{
	private int[] nColor;

	private byte[] byLineThick;

	private short[] sPx;

	private short[] sPy;

	private short[] sWidth;

	private short[] sHeight;

	public void Load(BinaryReader br)
	{
		try
		{
			short num = br.ReadInt16();
			if (num > 0)
			{
				nColor = new int[num];
				byLineThick = new byte[num];
				sPx = new short[num];
				sPy = new short[num];
				sWidth = new short[num];
				sHeight = new short[num];
				for (int i = 0; i < num; i++)
				{
					nColor[i] = br.ReadInt32();
					byLineThick[i] = br.ReadByte();
					sPx[i] = br.ReadInt16();
					sPy[i] = br.ReadInt16();
					sWidth[i] = br.ReadInt16();
					sHeight[i] = br.ReadInt16();
				}
			}
		}
		catch
		{
		}
	}

	public void Free()
	{
		nColor = null;
		byLineThick = null;
		sPx = null;
		sPy = null;
		sWidth = null;
		sHeight = null;
	}
}
