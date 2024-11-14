using System.IO;

public interface IRecordset
{
	void SaveData(BinaryWriter bw);

	void LoadData(BinaryReader br);
}
