using System.IO;
using UnityEngine;

public class CoopState : IBattleState
{
	public int maxTotalKills;

	public int maxKills;

	public void Init()
	{
		maxKills = 0;
		maxTotalKills = 0;
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(maxKills);
		bw.Write(maxTotalKills);
	}

	public void Load(BinaryReader br)
	{
		maxKills = br.ReadInt32();
		maxTotalKills = br.ReadInt32();
	}

	public void SetState(IBattleState state)
	{
		CoopState coopState = (CoopState)state;
		maxKills = coopState.maxKills;
		maxTotalKills = coopState.maxTotalKills;
	}

	public void WriteToBuffer(BytesBuffer buffer)
	{
		buffer.AddInt(maxKills);
		buffer.AddInt(maxTotalKills);
	}

	public void ReadFromBuffer(BytesBuffer buffer)
	{
		maxKills = buffer.ReadInt();
		maxTotalKills = buffer.ReadInt();
	}

	public void SetMaxKills(int kills)
	{
		kills = Mathf.Min(int.MaxValue, kills);
		maxKills = Mathf.Max(maxKills, kills);
	}

	public void AddTotalKills(int kills)
	{
		int b = maxTotalKills + kills;
		b = Mathf.Min(int.MaxValue, b);
		maxTotalKills = Mathf.Max(maxTotalKills, b);
	}
}
