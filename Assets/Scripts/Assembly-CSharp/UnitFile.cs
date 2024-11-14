using System.IO;

public class UnitFile
{
	private byte[] byData;

	public bool Load(BinaryReader br)
	{
		bool flag = false;
		byData = (byte[])Res2DManager.LoadData(br, 0, 3);
		return true;
	}

	public void Free()
	{
		byData = null;
	}
}
