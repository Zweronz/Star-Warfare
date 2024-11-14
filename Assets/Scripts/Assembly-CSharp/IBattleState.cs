using System.IO;

public interface IBattleState
{
	void Init();

	void Save(BinaryWriter bw);

	void Load(BinaryReader br);

	void WriteToBuffer(BytesBuffer buffer);

	void ReadFromBuffer(BytesBuffer buffer);

	void SetState(IBattleState state);
}
