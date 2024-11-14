using System.IO;

public class AchievementState
{
	protected const int ACHIEVEMENT_COUNT = 23;

	protected int score;

	protected int suitCollected;

	protected int enemyKills;

	protected int fullUpgrade;

	protected int wasteMoney;

	protected int teamkill;

	protected int rebirthTimes;

	protected int combo;

	protected int levelComplete;

	protected int neverGotHit;

	protected int[] bossKills = new int[3];

	protected AchievementInfo[] acheivements = new AchievementInfo[23];

	protected ScoreInfo scoreInfo = new ScoreInfo();

	protected int coop;

	public AchievementState()
	{
		for (int i = 0; i < 23; i++)
		{
			acheivements[i] = new AchievementInfo();
			acheivements[i].id = "com.ifreyr.starwarfare.a" + (i + 1);
		}
	}

	public void SubmitScore(int score)
	{
		scoreInfo.score = score;
	}

	public void SubmitAllToGameCenter()
	{
		for (int i = 0; i < 23; i++)
		{
			if (acheivements[i].submitting)
			{
				acheivements[i].submitting = false;
			}
		}
		TDMState tDMState = (TDMState)GameApp.GetInstance().GetUserState().GetBattleStates()[1];
		FFAState fFAState = (FFAState)GameApp.GetInstance().GetUserState().GetBattleStates()[2];
		VIPState vIPState = (VIPState)GameApp.GetInstance().GetUserState().GetBattleStates()[3];
		CMIState cMIState = (CMIState)GameApp.GetInstance().GetUserState().GetBattleStates()[4];
		int num = tDMState.maxTotalKills + fFAState.maxTotalKills + vIPState.maxTotalKills + cMIState.maxTotalKills;
		int num2 = tDMState.score + fFAState.score + vIPState.score + cMIState.score;
		scoreInfo.score = 0;
	}

	public void GotNewAvatar(int ownedSuit)
	{
		suitCollected = ownedSuit;
		CheckAchievement_OneSuit();
		CheckAchievement_TenSuit();
	}

	public void OnceCoop()
	{
		coop = 1;
		CheckAchievement_OnceCoop();
	}

	public void FullUpgrades()
	{
		fullUpgrade++;
		CheckAchievement_Blacksmith();
		CheckAchievement_WeaponSpecialist();
	}

	public void KillEnemy()
	{
		enemyKills++;
		CheckAchievement_KillFirstEnemy();
		CheckAchievement_Debugger();
	}

	public void TeamKillEnemy()
	{
		teamkill++;
		CheckAchievement_TeamWork();
		CheckAchievement_SealTeamVI();
	}

	public void Rebirth()
	{
		rebirthTimes++;
		CheckAchievement_Rebirth();
		CheckAchievement_Phoenix();
	}

	public void MaxCombo(int combo)
	{
		if (this.combo < combo)
		{
			this.combo = combo;
		}
		CheckAchievement_Combo();
		CheckAchievement_OverHeated();
		CheckAchievement_MonsterKiller();
	}

	public void WasteMoney(int cash)
	{
		wasteMoney += cash;
		CheckAchievement_WasteOneMillion();
		CheckAchievement_WasteTenMillion();
	}

	public void KillBoss(int bossType)
	{
		bossKills[bossType]++;
		CheckAchievement_BossKiller();
		CheckAchievement_KillBoss10();
		CheckAchievement_KillBoss100();
	}

	public void AddScore(int scoreAdd)
	{
		score += scoreAdd;
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(score);
		bw.Write(enemyKills);
		bw.Write(suitCollected);
		bw.Write(fullUpgrade);
		for (int i = 0; i < 3; i++)
		{
			bw.Write(bossKills[i]);
		}
		for (int j = 0; j < 23; j++)
		{
			bw.Write(acheivements[j].submitting);
			bw.Write(acheivements[j].complete);
		}
	}

	public void Load(BinaryReader br)
	{
		score = br.ReadInt32();
		enemyKills = br.ReadInt32();
		suitCollected = br.ReadInt32();
		fullUpgrade = br.ReadInt32();
		for (int i = 0; i < 3; i++)
		{
			bossKills[i] = br.ReadInt32();
		}
		for (int j = 0; j < 23; j++)
		{
			acheivements[j].submitting = br.ReadBoolean();
			acheivements[j].complete = br.ReadBoolean();
		}
	}

	public void CheckAchievement_KillFirstEnemy()
	{
		if (!acheivements[0].complete && enemyKills == 1)
		{
			acheivements[0].submitting = true;
			acheivements[0].complete = true;
		}
	}

	public void CheckAchievement_Debugger()
	{
		if (!acheivements[1].complete && enemyKills == 100)
		{
			acheivements[1].submitting = true;
			acheivements[1].complete = true;
		}
	}

	public void CheckAchievement_BossKiller()
	{
		if (!acheivements[2].complete && (bossKills[0] == 1 || bossKills[1] == 1 || bossKills[2] == 1))
		{
			acheivements[2].submitting = true;
			acheivements[2].complete = true;
		}
	}

	public void CheckAchievement_WasteOneMillion()
	{
		if (!acheivements[3].complete && wasteMoney >= 1000000)
		{
			acheivements[3].submitting = true;
			acheivements[3].complete = true;
		}
	}

	public void CheckAchievement_WasteTenMillion()
	{
		if (!acheivements[4].complete && wasteMoney >= 10000000)
		{
			acheivements[4].submitting = true;
			acheivements[4].complete = true;
		}
	}

	public void CheckAchievement_Blacksmith()
	{
		if (!acheivements[5].complete && fullUpgrade == 1)
		{
			acheivements[5].submitting = true;
			acheivements[5].complete = true;
		}
	}

	public void CheckAchievement_WeaponSpecialist()
	{
		if (!acheivements[6].complete && fullUpgrade == 5)
		{
			acheivements[6].submitting = true;
			acheivements[6].complete = true;
		}
	}

	public void CheckAchievement_OneSuit()
	{
		if (!acheivements[7].complete && suitCollected >= 1)
		{
			acheivements[7].submitting = true;
			acheivements[7].complete = true;
		}
	}

	public void CheckAchievement_TenSuit()
	{
		if (!acheivements[8].complete && suitCollected >= 7)
		{
			acheivements[8].submitting = true;
			acheivements[8].complete = true;
		}
	}

	public void CheckAchievement_OnceCoop()
	{
		if (!acheivements[9].complete && coop == 1)
		{
			acheivements[9].submitting = true;
			acheivements[9].complete = true;
		}
	}

	public void CheckAchievement_TeamWork()
	{
		if (!acheivements[10].complete && teamkill == 50)
		{
			acheivements[10].submitting = true;
			acheivements[10].complete = true;
		}
	}

	public void CheckAchievement_SealTeamVI()
	{
		if (!acheivements[11].complete && teamkill == 500)
		{
			acheivements[11].submitting = true;
			acheivements[11].complete = true;
		}
	}

	public void CheckAchievement_Combo()
	{
		if (!acheivements[12].complete && combo == 1)
		{
			acheivements[12].submitting = true;
			acheivements[12].complete = true;
		}
	}

	public void CheckAchievement_FirstLevel()
	{
		if (!acheivements[13].complete)
		{
			acheivements[13].submitting = true;
			acheivements[13].complete = true;
		}
	}

	public void CheckAchievement_ThreeLevels()
	{
		if (!acheivements[14].complete)
		{
			acheivements[14].submitting = true;
			acheivements[14].complete = true;
		}
	}

	public void CheckAchievement_FiveLevels()
	{
		if (!acheivements[15].complete)
		{
			acheivements[15].submitting = true;
			acheivements[15].complete = true;
		}
	}

	public void CheckAchievement_Ghost()
	{
		if (!acheivements[16].complete)
		{
			acheivements[16].submitting = true;
			acheivements[16].complete = true;
		}
	}

	public void CheckAchievement_Rebirth()
	{
		if (!acheivements[17].complete && rebirthTimes == 1)
		{
			acheivements[17].submitting = true;
			acheivements[17].complete = true;
		}
	}

	public void CheckAchievement_Phoenix()
	{
		if (!acheivements[18].complete && rebirthTimes == 100)
		{
			acheivements[18].submitting = true;
			acheivements[18].complete = true;
		}
	}

	public void CheckAchievement_MonsterKiller()
	{
		if (!acheivements[19].complete && combo == 10)
		{
			acheivements[19].submitting = true;
			acheivements[19].complete = true;
		}
	}

	public void CheckAchievement_OverHeated()
	{
		if (!acheivements[20].complete && combo == 50)
		{
			acheivements[20].submitting = true;
			acheivements[20].complete = true;
		}
	}

	public void CheckAchievement_KillBoss10()
	{
		if (!acheivements[21].complete && (bossKills[0] == 10 || bossKills[1] == 10 || bossKills[2] == 10))
		{
			acheivements[21].submitting = true;
			acheivements[21].complete = true;
		}
	}

	public void CheckAchievement_KillBoss100()
	{
		if (!acheivements[22].complete && (bossKills[0] == 100 || bossKills[1] == 100 || bossKills[2] == 100))
		{
			acheivements[22].submitting = true;
			acheivements[22].complete = true;
		}
	}
}
