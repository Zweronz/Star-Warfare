using System;
using System.IO;
using System.Text;
using UnityEngine;

public class Res2DManager
{
	private const string RES_PNG_FILE_NAME = "res";

	private const string RES_DATASETS_FILE_NAME = "resDataSets";

	private const string RES_FILES_FILE_NAME = "resFiles";

	private const string RES_UI_FILE_NAME = "resUI";

	private const byte LOADING_RES_TYPE_UI = 0;

	private const byte LOADING_RES_TYPE_DATABASE = 1;

	private const byte LOADING_RES_TYPE_FILES = 2;

	private const byte LOADING_RES_TYPE_MATERIAL = 3;

	private const byte LOADING_RES_TYPE_CLOSE = 4;

	public const byte DATA_STRING = 0;

	public const byte DATA_INT = 1;

	public const byte DATA_SHORT = 2;

	public const byte DATA_BYTE = 3;

	public const byte DATA_TWO_DIMENSIONAL_STRING = 4;

	public const byte DATA_TWO_DIMENSIONAL_INT = 5;

	public const byte DATA_TWO_DIMENSIONAL_SHORT = 6;

	public const byte DATA_TWO_DIMENSIONAL_BYTE = 7;

	public const byte LOADMODE_STEPBY = 0;

	public const byte LOADMODE_ONEPASS = 1;

	public static Res2DManager instance;

	public int nVersion;

	private Stream stream;

	private BinaryReader binReader;

	private int[][] nResPoses;

	private string[] strGameDatas;

	public UnitUI[] vUI;

	private byte[] byUICounts;

	public UnitDataTable[] vDataTable;

	private byte[] byDTCounts;

	public UnitFile[] vFile;

	private byte[] byFDCounts;

	public UnitMaterial[] vMaterial;

	private byte[] byMDCounts;

	public byte byLoadMode;

	private byte byLoadphase;

	private byte bySubLoadPhase;

	private short nLoadDataCount;

	private short nLoadMaterialCount;

	private short nDataMaxPercent;

	private short nMaterialMaxPercent;

	public byte byLoadPercent;

	private short nLoadMaterialIdx;

	private short nLoadDataIdx;

	private short nCurUIIdx;

	private short nCurFileIdx;

	private short nCurDatabaseIdx;

	private short nMaterialCount;

	private short nCurMaterialIdx;

	private bool bLoadUI;

	private bool bLoadDataTable;

	private bool bLoadFiles;

	private bool bInit;

	public static Res2DManager GetInstance()
	{
		if (instance == null)
		{
			instance = new Res2DManager();
		}
		return instance;
	}

	public void Init()
	{
		LoadConfig();
	}

	public void Init(int UI_index)
	{
		if (!bInit)
		{
			LoadConfig();
			bInit = true;
		}
		SetResUI(UI_index);
		LoadResInit(1, -1);
		LoadResPro();
	}

	public void LoadImmediately(int UI_index)
	{
		SetResUI(UI_index);
		LoadResInit(1, -1);
		LoadResPro();
	}

	private Stream GetInputStream(string name)
	{
		string path = "UI/" + name;
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		byte[] bytes = textAsset.bytes;
		stream = new MemoryStream(bytes);
		return stream;
	}

	private void OpenDataInputStream(string name)
	{
		try
		{
			binReader = new BinaryReader(GetInputStream(name));
		}
		catch
		{
		}
	}

	private void CloseDataInputStream()
	{
		if (binReader == null)
		{
			return;
		}
		try
		{
			binReader.Close();
			stream.Close();
			binReader = null;
			stream = null;
		}
		catch
		{
		}
	}

	public static object LoadData(BinaryReader br, int nc, int nClass)
	{
		if (nc <= 0)
		{
			nc = br.ReadInt16();
		}
		if (nc <= 0)
		{
			return null;
		}
		switch (nClass)
		{
		default:
			return null;
		case 0:
		{
			string[] array8 = new string[nc];
			for (int num = 0; num < nc; num++)
			{
				array8[num] = ReadUncode(br);
			}
			return array8;
		}
		case 1:
		{
			int[] array4 = new int[nc];
			for (int l = 0; l < nc; l++)
			{
				array4[l] = br.ReadInt32();
			}
			return array4;
		}
		case 2:
		{
			short[] array7 = new short[nc];
			for (int n = 0; n < nc; n++)
			{
				array7[n] = br.ReadInt16();
			}
			return array7;
		}
		case 3:
		{
			byte[] array5 = new byte[nc];
			return br.ReadBytes(nc);
		}
		case 4:
		{
			string[][] array2 = new string[nc][];
			for (int j = 0; j < nc; j++)
			{
				array2[j] = (string[])LoadData(br, 0, 0);
			}
			return array2;
		}
		case 5:
		{
			int[][] array6 = new int[nc][];
			for (int m = 0; m < nc; m++)
			{
				array6[m] = (int[])LoadData(br, 0, 1);
			}
			return array6;
		}
		case 6:
		{
			short[][] array3 = new short[nc][];
			for (int k = 0; k < nc; k++)
			{
				array3[k] = (short[])LoadData(br, 0, 2);
			}
			return array3;
		}
		case 7:
		{
			byte[][] array = new byte[nc][];
			for (int i = 0; i < nc; i++)
			{
				array[i] = (byte[])LoadData(br, 0, 3);
			}
			return array;
		}
		}
	}

	public static string ReadUTF8(BinaryReader br)
	{
		int num = br.ReadInt16();
		byte[] array = new byte[num];
		br.Read(array, 0, num);
		string @string = Encoding.UTF8.GetString(array);
		array = null;
		return @string;
	}

	public static string ReadUncode(BinaryReader br)
	{
		int num = br.ReadInt16();
		byte[] array = new byte[num];
		br.Read(array, 0, num);
		string @string = Encoding.Unicode.GetString(array);
		array = null;
		return @string;
	}

	private bool LoadConfig()
	{
		int num = 0;
		OpenDataInputStream("res");
		num = binReader.ReadInt32();
		nVersion = binReader.ReadInt16();
		nResPoses = new int[num >> 24][];
		nResPoses = (int[][])LoadData(binReader, nResPoses.Length, 5);
		if (nResPoses[3] != null)
		{
			vMaterial = new UnitMaterial[nResPoses[3].Length];
			byMDCounts = new byte[nResPoses[3].Length];
		}
		if (nResPoses[4] != null)
		{
			vUI = new UnitUI[nResPoses[4].Length];
			byUICounts = new byte[nResPoses[4].Length];
		}
		if (nResPoses[1] != null)
		{
			vDataTable = new UnitDataTable[nResPoses[1].Length];
			byDTCounts = new byte[nResPoses[1].Length];
		}
		if (nResPoses[2] != null)
		{
			vFile = new UnitFile[nResPoses[2].Length];
			byFDCounts = new byte[nResPoses[2].Length];
		}
		string text = ReadUncode(binReader);
		strGameDatas = (string[])LoadData(binReader, 0, 0);
		CloseDataInputStream();
		return true;
	}

	public bool LoadResPro()
	{
		do
		{
			if (byLoadphase == 0)
			{
				if (LoadProcUI(byLoadMode))
				{
					LoadGoToPhase(1);
				}
			}
			else if (byLoadphase == 1)
			{
				if (LoadProDatabase(byLoadMode))
				{
					LoadGoToPhase(2);
				}
			}
			else if (byLoadphase == 2)
			{
				if (LoadProFiles(byLoadMode))
				{
					LoadGoToPhase(3);
				}
			}
			else if (byLoadphase == 3)
			{
				if (LoadProMaterial(byLoadMode))
				{
					LoadGoToPhase(4);
				}
			}
			else if (byLoadphase == 4)
			{
				LoadResClose();
				return true;
			}
			byLoadPercent = (byte)(nDataMaxPercent * nLoadDataIdx / ((nLoadDataCount != 0) ? nLoadDataCount : (nLoadDataCount + 1)) + ((nLoadMaterialCount > 0) ? (nMaterialMaxPercent * nLoadMaterialIdx / nLoadMaterialCount) : 0));
		}
		while (byLoadMode != 0);
		return false;
	}

	private void LoadGoToPhase(byte phase)
	{
		byLoadphase = phase;
		bySubLoadPhase = 0;
	}

	public void LoadResInit(byte mode, int nMax)
	{
		byLoadMode = mode;
		if (bLoadUI)
		{
			nMaterialMaxPercent = (nDataMaxPercent = (short)(nMax / 2));
		}
		else
		{
			nMaterialMaxPercent = 1;
			nDataMaxPercent = (short)nMax;
		}
		byLoadPercent = 0;
		byLoadphase = 0;
		bySubLoadPhase = 0;
		nMaterialCount = 0;
		nCurMaterialIdx = 0;
		nLoadMaterialIdx = 0;
		nLoadDataIdx = 0;
		nCurUIIdx = 0;
		nCurFileIdx = 0;
		nCurDatabaseIdx = 0;
	}

	public void LoadResClose()
	{
		bLoadUI = false;
		bLoadDataTable = false;
		bLoadFiles = false;
		byLoadphase = 0;
		bySubLoadPhase = 0;
		nMaterialCount = 0;
		nCurMaterialIdx = 0;
		nMaterialMaxPercent = 0;
		nDataMaxPercent = 0;
		nLoadDataCount = 0;
		nLoadMaterialCount = 0;
		nLoadDataIdx = 0;
		nLoadMaterialIdx = 0;
		nCurUIIdx = 0;
		nCurFileIdx = 0;
		nCurDatabaseIdx = 0;
	}

	private bool LoadProMaterial(byte mode)
	{
		do
		{
			if (bySubLoadPhase == 0)
			{
				CalcMaterial();
				bySubLoadPhase = 1;
			}
			else
			{
				if (bySubLoadPhase != 1)
				{
					continue;
				}
				if (nMaterialCount <= 0)
				{
					return true;
				}
				bool flag = true;
				try
				{
					for (int i = nCurMaterialIdx; i < vMaterial.Length; i++)
					{
						if (vMaterial[i] == null && byMDCounts[i] > 0)
						{
							flag = false;
							vMaterial[i] = new UnitMaterial();
							vMaterial[i].Load(Convert.ToString(i));
							nMaterialCount--;
							nLoadMaterialIdx++;
							nCurMaterialIdx = (short)(i + 1);
							if (mode == 0)
							{
								break;
							}
						}
					}
				}
				catch
				{
				}
				if (flag)
				{
					return true;
				}
			}
		}
		while (mode != 0);
		return false;
	}

	private void UpdateMaterialCount(short[] ni, bool isload)
	{
		if (ni == null)
		{
			return;
		}
		foreach (int num in ni)
		{
			if (num >= 0 && num < vMaterial.Length)
			{
				if (isload)
				{
					byMDCounts[num]++;
				}
				else
				{
					byMDCounts[num]--;
				}
			}
		}
	}

	private void CalcMaterial()
	{
		nMaterialCount = 0;
		for (int i = 0; i < byMDCounts.Length; i++)
		{
			if (byMDCounts[i] <= 0 && vMaterial[i] != null)
			{
				vMaterial[i].Free();
				vMaterial[i] = null;
			}
			else if (vMaterial[i] == null)
			{
				nMaterialCount++;
			}
		}
		nLoadMaterialCount = nMaterialCount;
	}

	private void UpdateMaterial()
	{
		for (int i = 0; i < byMDCounts.Length; i++)
		{
			if (byMDCounts[i] <= 0 && vMaterial[i] != null)
			{
				vMaterial[i].Free();
				vMaterial[i] = null;
			}
		}
	}

	public byte[] GetMaterialCounts()
	{
		return byMDCounts;
	}

	private bool LoadProcUI(byte mode)
	{
		if (!bLoadUI)
		{
			return true;
		}
		while (bLoadUI)
		{
			if (bySubLoadPhase == 0)
			{
				OpenDataInputStream("resUI");
				bySubLoadPhase = 1;
				nLoadDataIdx++;
			}
			else if (bySubLoadPhase == 1)
			{
				bool flag = true;
				try
				{
					for (int i = nCurUIIdx; i < vUI.Length; i++)
					{
						if (vUI[i] == null && byUICounts[i] > 0)
						{
							vUI[i] = new UnitUI();
							vUI[i].Load(binReader);
							UpdateMaterialCount(vUI[i].GetDibInfo(), true);
							nLoadDataIdx++;
							flag = false;
							nCurUIIdx = (short)(i + 1);
							if (mode == 0)
							{
								break;
							}
						}
						else if (i + 1 < vUI.Length)
						{
							binReader.BaseStream.Seek(nResPoses[4][i + 1] - nResPoses[4][i], SeekOrigin.Current);
						}
					}
				}
				catch
				{
				}
				if (flag)
				{
					bySubLoadPhase = 2;
				}
			}
			else if (bySubLoadPhase == 2)
			{
				nLoadDataIdx++;
				bLoadUI = false;
				CloseDataInputStream();
				return true;
			}
			if (mode == 0)
			{
				break;
			}
		}
		return false;
	}

	public void SetResUI(int begin, int end)
	{
		for (int i = begin; i <= end; i++)
		{
			SetResUI(i);
		}
	}

	public void SetResUI(int index)
	{
		if (index < vUI.Length && vUI[index] == null)
		{
			if (!bLoadUI)
			{
				nLoadDataCount += 2;
			}
			nLoadDataCount++;
			bLoadUI = true;
			byUICounts[index] = 1;
		}
	}

	public void FreeUI(int index, bool bFreeMaterial)
	{
		if (vUI != null && vUI[index] != null)
		{
			UpdateMaterialCount(vUI[index].GetDibInfo(), false);
			vUI[index].Free();
			vUI[index] = null;
			if (bFreeMaterial)
			{
				UpdateMaterial();
			}
			byUICounts[index] = 0;
		}
	}

	private bool LoadProDatabase(byte mode)
	{
		if (!bLoadDataTable)
		{
			return true;
		}
		while (bLoadDataTable)
		{
			if (bySubLoadPhase == 0)
			{
				OpenDataInputStream("resDataSets");
				bySubLoadPhase = 1;
				nLoadDataIdx++;
			}
			else if (bySubLoadPhase == 1)
			{
				bool flag = true;
				try
				{
					for (int i = nCurDatabaseIdx; i < vDataTable.Length; i++)
					{
						if (vDataTable[i] == null && byDTCounts[i] > 0)
						{
							vDataTable[i] = new UnitDataTable();
							vDataTable[i].Load(binReader);
							nLoadDataIdx++;
							flag = false;
							nCurDatabaseIdx = (short)(i + 1);
							if (mode == 0)
							{
								break;
							}
						}
						else if (i + 1 < vDataTable.Length)
						{
							binReader.BaseStream.Seek(nResPoses[1][i + 1] - nResPoses[1][i], SeekOrigin.Current);
						}
					}
				}
				catch
				{
				}
				if (flag)
				{
					bySubLoadPhase = 2;
				}
			}
			else if (bySubLoadPhase == 2)
			{
				bLoadDataTable = false;
				CloseDataInputStream();
				nLoadDataIdx++;
				return true;
			}
			if (mode == 0)
			{
				break;
			}
		}
		return false;
	}

	public void SetResData(int start, int end)
	{
		for (int i = start; i <= end; i++)
		{
			SetResData(i);
		}
	}

	public void SetResData(int index)
	{
		if (index < vDataTable.Length && vDataTable[index] == null)
		{
			if (!bLoadDataTable)
			{
				nLoadDataCount += 2;
			}
			nLoadDataCount++;
			bLoadDataTable = true;
			byDTCounts[index] = 1;
		}
	}

	public void FreeDataTable(int index)
	{
		if (vDataTable != null && vDataTable[index] != null)
		{
			vDataTable[index].Free();
			vDataTable[index] = null;
			byDTCounts[index] = 0;
		}
	}

	private bool LoadProFiles(byte mode)
	{
		if (!bLoadFiles)
		{
			return true;
		}
		while (bLoadFiles)
		{
			if (bySubLoadPhase == 0)
			{
				OpenDataInputStream("resFiles");
				bySubLoadPhase = 1;
				nLoadDataIdx++;
			}
			else if (bySubLoadPhase == 1)
			{
				bool flag = true;
				try
				{
					for (int i = nCurFileIdx; i < vFile.Length; i++)
					{
						if (vFile[i] == null && byFDCounts[i] > 0)
						{
							vFile[i] = new UnitFile();
							vFile[i].Load(binReader);
							nLoadDataIdx++;
							flag = false;
							nCurFileIdx = (short)(i + 1);
							if (mode == 0)
							{
								break;
							}
						}
						else if (i + 1 < vFile.Length)
						{
							binReader.BaseStream.Seek(nResPoses[2][i + 1] - nResPoses[2][i], SeekOrigin.Current);
						}
					}
				}
				catch
				{
				}
				if (flag)
				{
					bySubLoadPhase = 2;
				}
			}
			else if (bySubLoadPhase == 2)
			{
				bLoadFiles = false;
				CloseDataInputStream();
				nLoadDataIdx++;
				return true;
			}
			if (mode == 0)
			{
				break;
			}
		}
		return false;
	}

	public void SetResFile(int start, int end)
	{
		for (int i = start; i <= end; i++)
		{
			SetResFile(i);
		}
	}

	public void SetResFile(int index)
	{
		if (index < vFile.Length && vFile[index] == null)
		{
			if (!bLoadFiles)
			{
				nLoadDataCount += 2;
			}
			nLoadDataCount++;
			bLoadFiles = true;
			byFDCounts[index] = 1;
		}
	}

	public void FreeFile(int index)
	{
		if (vFile != null && vFile[index] != null)
		{
			vFile[index].Free();
			vFile[index] = null;
			byFDCounts[index] = 0;
		}
	}

	public string[] GetGameText()
	{
		return strGameDatas;
	}

	public string[] SplitString(string str)
	{
		return str.Split('\n');
	}

	public string[] SplitString(string str, char sn)
	{
		return str.Split(sn);
	}
}
