using UnityEngine;

public class PlayerVSStatistics
{
	public const int SCORE_PER_KILL_IN_FFA = 50;

	public const int SCORE_PER_KILL_IN_TDM = 40;

	public const int SCORE_PER_KILL_VIP_IN_VIP = 150;

	public const int SCORE_PER_SECURE_VIP_IN_VIP = 100;

	public const int SCORE_PER_KILL_IN_CTF_TDM = 50;

	public const int SCORE_PER_FLAGS_IN_CTF_TDM = 100;

	public const int SCORE_PER_ASSIST_FLAGS_IN_CTF_TDM = 16;

	public const int SCORE_PER_KILL_IN_CTF_FFA = 50;

	public const int SCORE_PER_FLAGS_IN_CTF_FFA = 100;

	public const int SCORE_PER_ASSIST_FLAGS_IN_CTF_FFA = 20;

	protected float lastKillTime;

	protected float COMBO_INTERVAL = 3f;

	public int Kills { get; set; }

	public int Death { get; set; }

	public int Assist { get; set; }

	public int Score { get; set; }

	public int Bonus { get; set; }

	public int Combo { get; set; }

	public int TotalCombo { get; set; }

	public int PlayerHit { get; set; }

	public int CashReward { get; set; }

	public int SecureFlags { get; set; }

	public int AssistFlags { get; set; }

	public int VIPAssist { get; set; }

	public int CMIGiftHit { get; set; }

	public void MakeCombo()
	{
		Combo++;
		TotalCombo++;
		lastKillTime = Time.time;
	}

	public void AddKillScore()
	{
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA)
		{
			Score += 50;
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM)
		{
			Score += 40;
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay != Mode.VS_CTF_TDM && GameApp.GetInstance().GetGameMode().ModePlay != Mode.VS_CTF_FFA)
		{
		}
	}

	public void AddSecureFlagScore()
	{
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
		{
			Score += 100;
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
		{
			Score += 100;
		}
	}

	public void AddKillVIPScore()
	{
		Score += 150;
	}

	public void AddSecureVIPScore()
	{
		Score += 100;
	}

	public void AddHitItemScore(int score)
	{
		Score += score;
	}

	public void UpdateCashReward()
	{
		PVPRewardInfo pVPReward = GameApp.GetInstance().GetGameWorld().PVPReward;
		CashReward = Kills * pVPReward.cashPerKill + PlayerHit * pVPReward.cashPerAssist + CMIGiftHit * pVPReward.cashPerAssist;
		Debug.Log("cash rewards:" + CashReward + "/" + Kills + "/" + PlayerHit);
	}

	public void CalculateBonus(Mode mode, bool win)
	{
		int num = Death;
		if (num == 0)
		{
			num = 1;
		}
		switch (mode)
		{
		case Mode.VS_TDM:
			Bonus = Assist * 8 + Kills * 10 + Death * 5 + (int)((float)Kills * 1f / ((float)num * 1f) * 14f) + ((!win) ? 5 : 10);
			break;
		case Mode.VS_FFA:
			Bonus = Kills * 12 + Death * 6 + (int)((float)Kills * 1f / ((float)num * 1f) * 20f);
			break;
		case Mode.VS_CTF_TDM:
			Bonus = Assist * 16 + Kills * 50 + Death * 5 + (int)((float)Kills * 1f / ((float)num * 1f) * 14f) + ((!win) ? 5 : 10);
			break;
		case Mode.VS_CTF_FFA:
			Bonus = Assist * 20 + Kills * 50 + Death * 6 + (int)((float)Kills * 1f / ((float)num * 1f) * 20f);
			break;
		case Mode.VS_VIP:
			Bonus = Assist * 8 + Kills * 10 + Death * 5 + (int)((float)Kills * 1f / ((float)num * 1f) * 14f) + ((!win) ? 5 : 10);
			break;
		case Mode.VS_CMI:
			Bonus = Assist * 8 + Kills * 10 + Death * 5 + (int)((float)Kills * 1f / ((float)num * 1f) * 14f) + ((!win) ? 5 : 10);
			break;
		}
	}

	public void ClearAll()
	{
		Kills = 0;
		Death = 0;
		Assist = 0;
		Score = 0;
		Bonus = 0;
		Combo = 0;
		TotalCombo = 0;
		PlayerHit = 0;
		CashReward = 0;
		SecureFlags = 0;
		AssistFlags = 0;
		VIPAssist = 0;
		CMIGiftHit = 0;
	}

	public void UpdateCombo()
	{
		if (Time.time - lastKillTime > COMBO_INTERVAL)
		{
			Combo = 0;
		}
	}
}
