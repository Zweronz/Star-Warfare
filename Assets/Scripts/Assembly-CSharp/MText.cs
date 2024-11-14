using System.IO;

public class MText : IModule
{
	private string[] strData;

	private int[] nColor;

	public void Load(BinaryReader br)
	{
		try
		{
			short num = br.ReadInt16();
			if (num > 0)
			{
				strData = new string[num];
				nColor = new int[num];
				for (int i = 0; i < num; i++)
				{
					strData[i] = Res2DManager.ReadUncode(br);
					nColor[i] = br.ReadInt32();
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
		strData = null;
	}
}
