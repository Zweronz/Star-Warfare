using System.IO;

public class UnitDataTable
{
	public short sCols;

	public short sRows;

	private string[] strLabel;

	private int[] nFormat;

	private string[][] strData;

	private int[][] nData;

	public bool Load(BinaryReader br)
	{
		bool flag = false;
		sCols = br.ReadInt16();
		sRows = br.ReadInt16();
		strLabel = (string[])Res2DManager.LoadData(br, sCols, 0);
		nFormat = (int[])Res2DManager.LoadData(br, sCols, 1);
		short num = br.ReadInt16();
		short num2 = br.ReadInt16();
		strData = new string[sRows][];
		nData = new int[sRows][];
		for (int i = 0; i < sRows; i++)
		{
			if (num > 0)
			{
				strData[i] = (string[])Res2DManager.LoadData(br, num, 0);
			}
			if (num2 > 0)
			{
				nData[i] = (int[])Res2DManager.LoadData(br, num2, 1);
			}
		}
		return true;
	}

	public void Free()
	{
		strLabel = null;
		nFormat = null;
		strData = null;
		nData = null;
	}

	public int GetData(int iRow, int iCol, int nValue, bool bSet)
	{
		if (iRow >= nData.Length)
		{
			return 0;
		}
		if (iCol >= nFormat.Length)
		{
			return 0;
		}
		int num = nFormat[iCol];
		if ((num & 0xFF) == 0)
		{
			return 0;
		}
		int num2 = (num >> 8) & 0xFF;
		int num3 = (num >> 16) & 0xFF;
		int num4 = (num >> 24) & 0xFF;
		if (bSet)
		{
			if (num4 >= 32)
			{
				nData[iRow][num2] = nValue;
			}
			else
			{
				int num5 = (1 << num4) - 1;
				nValue &= num5;
				nData[iRow][num2] &= ~(num5 << num3);
				nData[iRow][num2] |= nValue << num3;
			}
			return 0;
		}
		int num6 = nData[iRow][num2] >> num3;
		if (num4 < 32)
		{
			num6 &= (1 << (num4 & 0x1F)) - 1;
		}
		return num6;
	}

	public string GetData(int iRow, int iCol, string strValue, bool bSet)
	{
		if (iRow >= nData.Length)
		{
			return string.Empty;
		}
		if (iCol >= nFormat.Length)
		{
			return string.Empty;
		}
		int num = nFormat[iCol];
		int num2 = num & 0xFF;
		if (num2 > 0)
		{
			return string.Empty;
		}
		int num3 = (num >> 8) & 0xFF;
		int num4 = (num >> 16) & 0xFF;
		int num5 = (num >> 24) & 0xFF;
		if (bSet)
		{
			strData[iRow][num3] = strValue;
			return string.Empty;
		}
		return strData[iRow][num3];
	}
}
