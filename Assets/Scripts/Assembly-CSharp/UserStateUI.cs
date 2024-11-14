using System.Collections.Generic;
using UnityEngine;

public class UserStateUI
{
	public class SkillUI
	{
		public int SkillId { get; set; }

		public bool IsUse { get; set; }

		public float PercentOfUsingTime { get; set; }

		public bool IsCoolDown { get; set; }

		public float PercentOfCoolDownTime { get; set; }

		public bool IsJustCoolDownFinish { get; set; }

		public static SkillUI CreateSkill(int skillId)
		{
			SkillUI skillUI = new SkillUI();
			skillUI.SkillId = skillId;
			skillUI.IsUse = false;
			skillUI.PercentOfUsingTime = 0f;
			skillUI.IsCoolDown = false;
			skillUI.PercentOfCoolDownTime = 0f;
			skillUI.IsJustCoolDownFinish = false;
			return skillUI;
		}
	}

	public class RemotePlayerUI
	{
		public RemotePlayer RemotePlayer { get; set; }

		public TeamName TeamName
		{
			get
			{
				if (RemotePlayer == null)
				{
					return TeamName.Blue;
				}
				return RemotePlayer.Team;
			}
		}

		public int SeatID
		{
			get
			{
				if (RemotePlayer == null)
				{
					return 0;
				}
				return RemotePlayer.GetSeatID();
			}
		}

		public float PercentOfHp
		{
			get
			{
				if (RemotePlayer == null)
				{
					return 0f;
				}
				return (float)RemotePlayer.Hp / (float)RemotePlayer.MaxHp;
			}
		}
	}

	public class KillInfoUI
	{
		public int KillerID { get; set; }

		public HUDAction Action { get; set; }

		public int KilledID { get; set; }
	}

	public class TimeInfoUI
	{
		public enum TimeType
		{
			None = 0,
			Clock = 1,
			Flag = 2,
			VIP = 3
		}

		public TimeType Type { get; set; }

		public int Time1 { get; set; }

		public int Time2 { get; set; }

		public int Team { get; set; }

		public float IconFillAmount { get; set; }

		public bool IsIconFillAmountOn { get; set; }

		public TimeInfoUI()
		{
			Type = TimeType.Clock;
			Time1 = 0;
			Time2 = 0;
			Team = -1;
			IconFillAmount = 0f;
			IsIconFillAmountOn = true;
		}
	}

	public class JoyStickUI
	{
		public bool InitOK;

		public Vector2 MoveJoyStickCenter { get; set; }

		public Vector2 ShootJoyStickCenter { get; set; }

		public float Radius { get; set; }
	}

	private static UserStateUI Instance;

	private Vector2 mMoveJoyStickPos = Vector2.zero;

	private Vector2 mShootJoyStickPos = Vector2.zero;

	private float mWaveProcess;

	private int mHp;

	private int mMaxHp;

	private int mAmmo;

	private int mMaxAmmo;

	private int mWeaponIndex;

	private List<Weapon> mWeaponList = new List<Weapon>();

	private int[] mItemPosition;

	private int mAimId;

	private bool mIsAim;

	private bool mIsItemListPop;

	private bool mIsItemListUpForever;

	private int mComboNum;

	private List<SkillUI> mSkillList = new List<SkillUI>();

	private int mSurvivalKills;

	private float mBossHpPercent;

	private float m2ndBossHpPercent;

	private List<RemotePlayerUI> mRemotePlayerList = new List<RemotePlayerUI>();

	private List<KillInfoUI> mKillInfoList = new List<KillInfoUI>();

	private bool mIsTeamMode;

	private TeamName mTeam;

	private int mTotalScore;

	private int mTotalTime;

	private int mSeatId;

	private IWaitVSRebirth mIWaitVSRebirth = new EmptyWaitVSRebirth();

	private TimeInfoUI mTimeInfoUI = new TimeInfoUI();

	private VSBattleInformation mVSBattleInformation = new VSBattleInformation();

	private int mPlayerScore;

	private JoyStickUI mJoyStickUI = new JoyStickUI();

	private int mRebirthTime;

	private int mUsedRiviveTimes;

	private UserStateUI()
	{
	}

	public static UserStateUI GetInstance()
	{
		if (Instance == null)
		{
			Instance = new UserStateUI();
		}
		return Instance;
	}

	public void Clear()
	{
		mMoveJoyStickPos = Vector2.zero;
		mShootJoyStickPos = Vector2.zero;
		mWaveProcess = 0f;
		mHp = 0;
		mMaxHp = 0;
		mAmmo = 0;
		mMaxAmmo = 0;
		mWeaponIndex = 0;
		mAimId = 0;
		mIsAim = false;
		mIsItemListPop = false;
		mIsItemListUpForever = false;
		mItemPosition = null;
		mComboNum = 0;
		mBossHpPercent = 0f;
		mSurvivalKills = 0;
		mRebirthTime = 0;
		mIsTeamMode = false;
		mTeam = TeamName.Blue;
		mIWaitVSRebirth = new EmptyWaitVSRebirth();
		mPlayerScore = 0;
		mUsedRiviveTimes = 0;
		mTimeInfoUI = new TimeInfoUI();
		mVSBattleInformation = new VSBattleInformation();
		mWeaponList.Clear();
		mSkillList.Clear();
		mRemotePlayerList.Clear();
		mKillInfoList.Clear();
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player player = gameWorld.GetPlayer();
			if (player != null)
			{
				player.InputController.BlockCamera = false;
			}
		}
		Debug.Log("HUDClear");
	}

	public void SetMoveJoyStickPos(Vector2 pos)
	{
		mMoveJoyStickPos = pos;
	}

	public void SetShootJoyStickPos(Vector2 pos)
	{
		mShootJoyStickPos = pos;
	}

	public Vector2 GetMoveJoyStickPos()
	{
		return mMoveJoyStickPos;
	}

	public Vector2 GetShootJoyStickPos()
	{
		return mShootJoyStickPos;
	}

	public void SetWaveProcess(float process)
	{
		mWaveProcess = process;
	}

	public float GetWaveProcess()
	{
		return mWaveProcess;
	}

	public void SetHpandMaxHp(int hp, int maxHp)
	{
		mHp = hp;
		mMaxHp = maxHp;
	}

	public void UpdateHp(int hp)
	{
		mHp = hp;
	}

	public void SetAmmoandMaxAmmo(int ammo, int maxAmmo)
	{
		mAmmo = ammo;
		mMaxAmmo = maxAmmo;
	}

	public void UpdateAmmo(int ammo)
	{
		mAmmo = ammo;
	}

	public int GetMaxHp()
	{
		return mMaxHp;
	}

	public int GetHp()
	{
		return mHp;
	}

	public int GetMaxAmmo()
	{
		return mMaxAmmo;
	}

	public int GetAmmo()
	{
		return mAmmo;
	}

	public void SetWeaponList(List<Weapon> list)
	{
		mWeaponList = list;
	}

	public void SetWeaponIndex(int index)
	{
		mWeaponIndex = index;
	}

	public List<Weapon> GetWeaponList()
	{
		return mWeaponList;
	}

	public int GetWeaponIndex()
	{
		return mWeaponIndex;
	}

	public void SetItemPosition(int[] position)
	{
		mItemPosition = position;
	}

	public int[] GetItemPosition()
	{
		return mItemPosition;
	}

	public void SetAimId(int id)
	{
		mAimId = id;
	}

	public int GetAimId()
	{
		return mAimId;
	}

	public void SetAim(bool aim)
	{
		mIsAim = aim;
	}

	public bool IsAim()
	{
		return mIsAim;
	}

	public void PopItemList(bool upForever)
	{
		mIsItemListPop = true;
		mIsItemListUpForever = upForever;
	}

	public bool IsItemListUpForever()
	{
		return mIsItemListUpForever;
	}

	public bool IsItemListPop()
	{
		bool result = mIsItemListPop;
		mIsItemListPop = false;
		return result;
	}

	public void PushCombo(int num)
	{
		mComboNum = num;
	}

	public int PopComboNum()
	{
		int result = mComboNum;
		mComboNum = 0;
		return result;
	}

	public void SetSkillList(List<SkillUI> skillLst)
	{
		mSkillList = skillLst;
	}

	public List<SkillUI> GetSkillList()
	{
		return mSkillList;
	}

	public float GetBossHpPercent()
	{
		return mBossHpPercent;
	}

	public void UpdateBossHpPercent(float percent)
	{
		mBossHpPercent = percent;
	}

	public float Get2ndBossHpPercent()
	{
		return m2ndBossHpPercent;
	}

	public void Update2ndBossHpPercent(float percent)
	{
		m2ndBossHpPercent = percent;
	}

	public void UpdateSurvivalKills(int num)
	{
		mSurvivalKills = num;
	}

	public int GetSurvivalKills()
	{
		return mSurvivalKills;
	}

	public List<RemotePlayerUI> GetRemotePlayerList()
	{
		return mRemotePlayerList;
	}

	public void RemoveRemotePlayer(RemotePlayerUI remotePlayer)
	{
		mRemotePlayerList.Remove(remotePlayer);
	}

	public void AddRemotePlayerIfNotExist(RemotePlayer remotePlayer)
	{
		foreach (RemotePlayerUI mRemotePlayer in mRemotePlayerList)
		{
			if (mRemotePlayer.RemotePlayer.Equals(remotePlayer))
			{
				return;
			}
		}
		RemotePlayerUI remotePlayerUI = new RemotePlayerUI();
		remotePlayerUI.RemotePlayer = remotePlayer;
		mRemotePlayerList.Add(remotePlayerUI);
	}

	public void PushKillInfo(int killerID, HUDAction action, int killedID)
	{
		KillInfoUI killInfoUI = new KillInfoUI();
		killInfoUI.KillerID = killerID;
		killInfoUI.Action = action;
		killInfoUI.KilledID = killedID;
		mKillInfoList.Add(killInfoUI);
	}

	public KillInfoUI PopKillInfo()
	{
		if (mKillInfoList.Count > 0)
		{
			KillInfoUI result = mKillInfoList[0];
			mKillInfoList.RemoveAt(0);
			return result;
		}
		return null;
	}

	public void SetTeamMode(bool isTeamMode)
	{
		mIsTeamMode = isTeamMode;
	}

	public bool IsTeamMode()
	{
		return mIsTeamMode;
	}

	public void SetTeam(TeamName team)
	{
		mTeam = team;
	}

	public TeamName GetTeam()
	{
		return mTeam;
	}

	public void SetTotalScore(int score)
	{
		mTotalScore = score;
	}

	public void SetVSBattleInformation(VSBattleInformation vsBattleInformation)
	{
		mVSBattleInformation = vsBattleInformation;
	}

	public int GetBlueScore()
	{
		return mVSBattleInformation.TeamScores[0];
	}

	public int GetRedScore()
	{
		return mVSBattleInformation.TeamScores[1];
	}

	public int GetTotalScore()
	{
		return mTotalScore;
	}

	public int GetTopScore()
	{
		return mVSBattleInformation.TopScore.score;
	}

	public int GetTopScoreSeat()
	{
		return mVSBattleInformation.TopScore.seatID;
	}

	public void SetPlayerSeatID(int id)
	{
		mSeatId = id;
	}

	public int GetPlayerSeatID()
	{
		return mSeatId;
	}

	public string GetSeatSpriteName(int seatID)
	{
		return "sp" + (1 + seatID);
	}

	public IWaitVSRebirth GetWaitVSRebirth()
	{
		return mIWaitVSRebirth;
	}

	public void SetWaitVSRebirth(IWaitVSRebirth waitVSRebirth)
	{
		mIWaitVSRebirth = waitVSRebirth;
	}

	public void SetTimerType(TimeInfoUI.TimeType timeType)
	{
		SetTimerType(timeType, false);
	}

	public void SetTimerType(TimeInfoUI.TimeType timeType, bool fillOn)
	{
		mTimeInfoUI.Type = timeType;
		mTimeInfoUI.IsIconFillAmountOn = fillOn;
	}

	public void UpdateTimerInfo(int team, int value)
	{
		UpdateTimerInfo(team, value, 0f);
	}

	public void UpdateTimerInfo(int team, int value, float fillAmount)
	{
		mTimeInfoUI.Team = team;
		mTimeInfoUI.Time1 = value;
		mTimeInfoUI.IconFillAmount = fillAmount;
	}

	public TimeInfoUI GetTimeInfo()
	{
		return mTimeInfoUI;
	}

	public int GetPlayerScore()
	{
		return mPlayerScore;
	}

	public void SetPlayerScore(int score)
	{
		mPlayerScore = score;
	}

	public JoyStickUI GetJoyStick()
	{
		return mJoyStickUI;
	}

	public void UpdateRebirthTime(int time)
	{
		mRebirthTime = time;
	}

	public int GetRebirthTime()
	{
		return mRebirthTime;
	}

	public int GetUsedRiviveTimes()
	{
		return mUsedRiviveTimes;
	}

	public void SetUsedRiviveTimes(int times)
	{
		mUsedRiviveTimes = times;
	}
}
