using System.IO;
using UnityEngine;

public class VIPState : IBattleState
{
	public int maxKills;

	public int maxTotalKills;

	public int wins;

	public int loses;

	public int score;

	public void Init()
	{
		maxKills = 0;
		maxTotalKills = 0;
		wins = 0;
		loses = 0;
		score = 0;
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(maxKills);
		bw.Write(maxTotalKills);
		bw.Write(wins);
		bw.Write(loses);
		bw.Write(score);
	}

	public void Load(BinaryReader br)
	{
		maxKills = br.ReadInt32();
		maxTotalKills = br.ReadInt32();
		wins = br.ReadInt32();
		loses = br.ReadInt32();
		score = br.ReadInt32();
	}

	public void WriteToBuffer(BytesBuffer buffer)
	{
		buffer.AddInt(maxKills);
		buffer.AddInt(maxTotalKills);
		buffer.AddInt(wins);
		buffer.AddInt(loses);
		buffer.AddInt(score);
	}

	public void ReadFromBuffer(BytesBuffer buffer)
	{
		maxKills = buffer.ReadInt();
		maxTotalKills = buffer.ReadInt();
		wins = buffer.ReadInt();
		loses = buffer.ReadInt();
		score = buffer.ReadInt();
	}

	public void SetState(IBattleState state)
	{
		VIPState vIPState = (VIPState)state;
		maxKills = vIPState.maxKills;
		maxTotalKills = vIPState.maxTotalKills;
		wins = vIPState.wins;
		loses = vIPState.loses;
		score = vIPState.score;
	}

	public void SetMaxKills(int kills)
	{
		maxKills = Mathf.Max(maxKills, kills);
	}

	public void AddTotalKills(int kills)
	{
		maxTotalKills += kills;
	}

	public void AtomicWins()
	{
		wins++;
	}

	public void AtomicLoses()
	{
		loses++;
	}

	public void AddTotalScore(int score)
	{
		this.score += score;
	}
}
