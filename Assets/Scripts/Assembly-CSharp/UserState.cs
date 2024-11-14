using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class UserState
{
	public string version = "296";

	public string key = "Please quit the app immediately";

	public string md5Key = "1e re sis 02a";

	public bool bInit;

	protected int[] avatar = new int[Global.AVATAR_PART_NUM];

	protected List<Weapon> weaponList;

	protected List<Item> itemList;

	protected int[] bagPosition = new int[Global.BAG_MAX_NUM];

	protected byte bagNum = Global.BAG_DEFAULT_NUM;

	protected string mithril;

	protected string cash;

	protected int exp;

	protected int saveNum;

	protected List<Rank> rankList = new List<Rank>();

	protected byte[,] stageState = new byte[Global.TOTAL_STAGE, Global.TOTAL_SUB_STAGE];

	protected byte stageIdx;

	protected byte subStageIdx;

	protected byte netStageIdx;

	public List<List<Armor>> armorList;

	public List<ArmorRewards> armorRewardList;

	public WeaponUpgrade weaponUpgrade;

	protected byte[,] storageInfo = new byte[Global.STORAGE_MAX_NUM, 2];

	protected bool bPlayMusic = true;

	protected bool bPlaySound = true;

	protected bool bBladePad;

	protected float musicVolume = 1f;

	protected float soundVolume = 1f;

	protected byte timeSpan;

	protected short succBossStage;

	protected short succBossStageGetMithril;

	protected long bossDate;

	protected bool bFirstLunchApp = true;

	protected byte deadTimer = 2;

	protected short completedLevelId = -1;

	protected IBattleState[] battleStates = new IBattleState[5];

	public bool bTwitter;

	public bool bFacebook;

	public bool bGotoIAP;

	protected byte[] succSoloBoss = new byte[Global.TOTAL_SOLO_BOSS_NUM];

	protected byte[] succCoopBoss = new byte[Global.TOTAL_COOP_BOSS_NUM];

	protected byte[] succCoopBossGetMithril = new byte[Global.TOTAL_COOP_BOSS_NUM];

	protected DateTime soloSuccBossTime = DateTime.Now;

	public bool bPurchaseRookie;

	public bool bPurchaseSergeant;

	protected readonly object syncMithrilLock = new object();

	public bool showRewardMsg;

	public string rewardAdsName = string.Empty;

	public int rewardNumber;

	protected byte showRewardRunningStatus;

	protected int usemithrils;

	protected int discountstatus;

	protected string discounttime;

	protected int gametime;

	protected int shownotify;

	protected int discountweapon;

	protected int showmovietime;

	protected string showmoviedate;

	public bool showmovie;

	public Promotion m_promotion = new Promotion();

	public bool showPromotion;

	protected long mLastLoginTicks;

	protected long mNextLoginInterval;

	protected bool isShowTutorial2;

	protected string role_name;

	public int Enegy { get; set; }

	public AchievementState Achievement { get; set; }

	public InputSensitivity TouchInputSensitivity { get; set; }

	public OperatingInfo OperInfo { get; set; }

	public UserState()
	{
		SetSuit(1);
		weaponList = new List<Weapon>();
		itemList = new List<Item>();
		armorList = new List<List<Armor>>();
		rankList = new List<Rank>();
		armorRewardList = new List<ArmorRewards>();
		weaponUpgrade = new WeaponUpgrade();
		battleStates = new IBattleState[5];
		battleStates[0] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_COOP_STATE);
		battleStates[1] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_TDM_STATE);
		battleStates[2] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_FFA_STATE);
		battleStates[3] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_VIP_STATE);
		battleStates[4] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_CMI_STATE);
		OperInfo = new OperatingInfo();
		succSoloBoss = new byte[Global.TOTAL_SOLO_BOSS_NUM];
		succCoopBoss = new byte[Global.TOTAL_COOP_BOSS_NUM];
		succCoopBossGetMithril = new byte[Global.TOTAL_COOP_BOSS_NUM];
	}

	public void Init()
	{
		timeSpan = 0;
		stageIdx = 0;
		subStageIdx = 0;
		SetMithril(0);
		bagNum = Global.BAG_DEFAULT_NUM;
		Enegy = 5000;
		usemithrils = 0;
		discountstatus = 0;
		discounttime = "0000-00-00 00:00";
		gametime = 0;
		shownotify = 0;
		discountweapon = -1;
		showmovietime = 0;
		showmoviedate = "1900-01-01";
		mLastLoginTicks = 0L;
		mNextLoginInterval = 0L;
		role_name = "Player";
		SetCash(0);
		exp = 0;
		saveNum = 0;
		completedLevelId = -1;
		byte[] array = new byte[Global.BAG_MAX_NUM];
		byte[] array2 = new byte[Global.AVATAR_PART_NUM];
		byte[] array3 = new byte[47];
		byte[] array4 = new byte[Global.TOTAL_ARMOR_HEAD_NUM];
		byte[] array5 = new byte[Global.TOTAL_ARMOR_BODY_NUM];
		byte[] array6 = new byte[Global.TOTAL_ARMOR_ARM_NUM];
		byte[] array7 = new byte[Global.TOTAL_ARMOR_FOOT_NUM];
		byte[] array8 = new byte[Global.TOTAL_ARMOR_BAG_NUM];
		byte[,] array9 = new byte[Global.TOTAL_ITEM_NUM, 2];
		byte[,] array10 = new byte[Global.TOTAL_STAGE, Global.TOTAL_SUB_STAGE];
		byte[] array11 = new byte[Global.TOTAL_SOLO_BOSS_NUM];
		byte[] array12 = new byte[Global.TOTAL_COOP_BOSS_NUM];
		byte[] array13 = new byte[Global.TOTAL_COOP_BOSS_NUM];
		array3[0] = 1;
		array4[0] = 1;
		array5[0] = 1;
		array6[0] = 1;
		array7[0] = 1;
		array8[0] = 1;
		array10[stageIdx, subStageIdx] = 1;
		array10[stageIdx, Global.TOTAL_SUB_STAGE - 1] = 1;
		array[0] = 1;
		array[1] = 83;
		array[2] = 88;
		array9[0, 0] = 83;
		array9[0, 1] = 2;
		array9[1, 0] = 87;
		array9[1, 1] = 2;
		array9[2, 0] = 89;
		array9[2, 1] = 2;
		array9[3, 0] = 90;
		array9[3, 1] = 2;
		array9[4, 0] = 91;
		array9[4, 1] = 2;
		for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
		{
			array2[i] = 0;
		}
		array2[4] = 0;
		Achievement = new AchievementState();
		InitWeapons(array3);
		InitArmors(array4, array5, array6, array7, array8);
		SetStorageInfo(array9);
		SetBagPosition(array);
		SetAvatar(array2);
		SetStageState(array10);
		SetSuccSoloBoss(array11);
		SetSuccCoopBoss(array12);
		SetSuccCoopBossGetMithril(array13);
		Armor armor = GetArmor(4, GetAvatar()[4]);
		SetBagNum(armor.BagNum);
		TouchInputSensitivity = InputSensitivity.Normal;
		InitBattleStates();
		timeSpan = 0;
		succBossStage = 0;
		succBossStageGetMithril = 0;
		bossDate = 0L;
		bTwitter = false;
		bFacebook = false;
		showRewardRunningStatus = 0;
		soloSuccBossTime = DateTime.Now;
		bPurchaseRookie = false;
		bPurchaseSergeant = false;
		m_promotion = new Promotion();
		bInit = true;
		isShowTutorial2 = false;
	}

	public void ReviseData()
	{
		for (int i = 0; i < bagPosition.Length; i++)
		{
			int propertyID = GetPropertyID(i);
			if (propertyID != -1 && propertyID < 80 && (weaponList[propertyID].Level == 15 || weaponList[propertyID].Level == 0))
			{
				weaponList[propertyID].Level = 1;
			}
		}
		for (int j = 0; j < storageInfo.GetLength(0); j++)
		{
			int num = storageInfo[j, 0] - 1;
			if (num != -1 && num < 80 && (weaponList[num].Level == 15 || weaponList[num].Level == 0))
			{
				weaponList[num].Level = 1;
			}
		}
	}

	public void InitWeapons(byte[] ownedWeapons)
	{
		InitWeaponUpgrade();
		weaponList.Clear();
		for (int i = 0; i < ownedWeapons.Length; i++)
		{
			Weapon weapon = WeaponFactory.GetInstance().CreateWeapon((byte)i);
			weapon.LoadConfig(ownedWeapons[i]);
			weapon.Level = ownedWeapons[i];
			weaponList.Add(weapon);
		}
	}

	public void InitWeaponUpgrade()
	{
		weaponUpgrade.LoadConfig();
	}

	public void InitStorages(byte[] ownedWeapons, byte[] ownedProps)
	{
		storageInfo = new byte[Global.STORAGE_MAX_NUM, 2];
		for (int i = 0; i < ownedProps.Length; i++)
		{
			if (ownedProps[i] > 0)
			{
				InsertPropsToStorage(81 + i + 1, ownedProps[i]);
			}
		}
		for (int j = 0; j < ownedWeapons.Length; j++)
		{
			if (ownedWeapons[j] > 0 && ownedWeapons[j] < 15 && GetWeaponBagIndex(j) == -1)
			{
				InsertPropsToStorage(j + 1, 1);
			}
		}
	}

	public void InitBattleStates()
	{
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			battleStates[i].Init();
		}
	}

	public void InitProps()
	{
		itemList.Clear();
		for (int i = 0; i < Global.TOTAL_ITEM_NUM; i++)
		{
			Item item = ItemFactory.GetInstance().CreateItem((byte)(i + 1 + 80));
			item.LoadConfig();
			itemList.Add(item);
		}
	}

	public void InitArmors(byte[] ownedHeads, byte[] ownedBodies, byte[] ownedArms, byte[] ownedFeet, byte[] ownedBag)
	{
		ClearArmors();
		List<Armor> list = new List<Armor>();
		for (int i = 0; i < ownedHeads.Length; i++)
		{
			Armor armor = new Armor(BodyType.Head, i);
			int resID = i * 4;
			armor.LoadConfig(ownedHeads[i], resID);
			armor.Level = ownedHeads[i];
			list.Add(armor);
		}
		armorList.Add(list);
		list = new List<Armor>();
		for (int j = 0; j < ownedBodies.Length; j++)
		{
			Armor armor2 = new Armor(BodyType.Body, j);
			int resID2 = j * 4 + 1;
			armor2.LoadConfig(ownedBodies[j], resID2);
			armor2.Level = ownedBodies[j];
			list.Add(armor2);
		}
		armorList.Add(list);
		list = new List<Armor>();
		for (int k = 0; k < ownedArms.Length; k++)
		{
			Armor armor3 = new Armor(BodyType.Arm, k);
			int resID3 = k * 4 + 2;
			armor3.LoadConfig(ownedArms[k], resID3);
			armor3.Level = ownedArms[k];
			list.Add(armor3);
		}
		armorList.Add(list);
		list = new List<Armor>();
		for (int l = 0; l < ownedFeet.Length; l++)
		{
			Armor armor4 = new Armor(BodyType.Foot, l);
			int resID4 = l * 4 + 3;
			armor4.LoadConfig(ownedFeet[l], resID4);
			armor4.Level = ownedFeet[l];
			list.Add(armor4);
		}
		armorList.Add(list);
		list = new List<Armor>();
		for (int m = 0; m < ownedBag.Length; m++)
		{
			Armor armor5 = new Armor(BodyType.Bag, m);
			int resID5 = m + 4 * Global.TOTAL_ARMOR_NUM;
			armor5.LoadConfig(ownedBag[m], resID5);
			armor5.Level = ownedBag[m];
			list.Add(armor5);
		}
		armorList.Add(list);
	}

	public void InitArmorRewards()
	{
		armorRewardList.Clear();
		for (int i = 0; i < Global.TOTAL_ARMOR_NUM; i++)
		{
			ArmorRewards armorRewards = new ArmorRewards(i);
			armorRewards.LoadConfig();
			armorRewardList.Add(armorRewards);
		}
	}

	public void InitRank()
	{
		rankList.Clear();
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[17];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			Rank rank = new Rank(i);
			rank.LoadConfig();
			rankList.Add(rank);
		}
	}

	public void InitCompletedLevel()
	{
		if (completedLevelId == -1)
		{
			completedLevelId = (short)(Global.TOTAL_STAGE * Global.TOTAL_SUB_STAGE - 1);
			bool flag = false;
			for (int i = 0; i < stageState.GetLength(0); i++)
			{
				for (int j = 0; j < stageState.GetLength(1); j++)
				{
					int num = i * Global.TOTAL_SUB_STAGE + j;
					if (stageState[i, j] == 0)
					{
						completedLevelId = (short)(num - 2);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		UnlockLevelFromCompletedLevel(completedLevelId);
	}

	public void UnLockEquip()
	{
		Rank rank = GetRank(exp);
		for (int i = 0; i < armorList.Count; i++)
		{
			List<Armor> list = armorList[i];
			for (int j = 0; j < list.Count; j++)
			{
				Armor armor = list[j];
				if (armor.Level == 0 && armor.UnlockLevel <= rank.rankID)
				{
					armor.Level = 15;
				}
			}
		}
		for (int k = 0; k < weaponList.Count; k++)
		{
			Weapon weapon = weaponList[k];
			if (weapon.Level == 0 && weapon.UnlockLevel <= rank.rankID)
			{
				weapon.Level = 15;
			}
		}
	}

	public void UseEnegy(int count)
	{
		float skill = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetSkills()
			.GetSkill(SkillsType.UNLIMITED_ENEGY);
		if (!(skill > 0f) && !GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			float skill2 = GameApp.GetInstance().GetGameWorld().GetPlayer()
				.GetSkills()
				.GetSkill(SkillsType.SAVE_ENEGY);
			float num = (float)(-count) * skill2;
			if (num < 1f && num > 0f)
			{
				num = 1f;
			}
			count -= (int)num;
			Enegy -= count;
			Enegy = Mathf.Clamp(Enegy, 0, Global.MAX_ENEGY);
		}
	}

	public void UnLockAllLevels()
	{
		for (int i = 0; i < Global.TOTAL_STAGE - 1; i++)
		{
			for (int j = 0; j < Global.TOTAL_SUB_STAGE; j++)
			{
				SetStageState(i, j, 1);
			}
		}
	}

	public void EnterGodMode()
	{
		SetMithril(18000);
		Enegy = 100000000;
		SetCash(100000000);
		exp = 100000000;
		UnLockAllLevels();
	}

	public Weapon GetWeaponInBag(int bagIndex)
	{
		int propertyID = GetPropertyID(bagIndex);
		if (propertyID != -1 && propertyID < 80)
		{
			return weaponList[propertyID];
		}
		return null;
	}

	public void UnlockLevelPatch()
	{
		if (stageState[4, Global.TOTAL_SUB_STAGE - 2] == 1)
		{
			stageState[5, 0] = 1;
			stageState[5, Global.TOTAL_SUB_STAGE - 1] = 1;
		}
	}

	public void UnlockLevelFromCompletedLevel(short completedLevel)
	{
		for (int i = 0; i < stageState.GetLength(0); i++)
		{
			for (int j = 0; j < stageState.GetLength(1); j++)
			{
				int num = i * Global.TOTAL_SUB_STAGE + j;
				if (num <= completedLevel + 1)
				{
					stageState[i, j] = 1;
					stageState[i, Global.TOTAL_SUB_STAGE - 1] = 1;
					if (num <= completedLevel && j == Global.TOTAL_SUB_STAGE - 2)
					{
						int num2 = (i + 1) % Global.TOTAL_STAGE;
						stageState[num2, 0] = 1;
						stageState[num2, Global.TOTAL_SUB_STAGE - 1] = 1;
					}
				}
			}
		}
	}

	public int GetWeaponBagIndex(Weapon w)
	{
		for (int i = 0; i < bagPosition.Length; i++)
		{
			int propertyID = GetPropertyID(i);
			if (propertyID != -1 && propertyID < 80 && propertyID == w.GetGunID())
			{
				return i;
			}
		}
		return -1;
	}

	public int GetWeaponBagIndex(int propsID)
	{
		for (int i = 0; i < bagPosition.Length; i++)
		{
			int propertyID = GetPropertyID(i);
			if (propertyID != -1 && propertyID < 80 && propertyID == propsID)
			{
				return i;
			}
		}
		return -1;
	}

	public List<Weapon> GetBattleWeapons()
	{
		List<Weapon> list = new List<Weapon>();
		for (int i = 0; i < bagPosition.Length; i++)
		{
			int propertyID = GetPropertyID(i);
			if (propertyID >= 0 && propertyID < 80)
			{
				list.Add(weaponList[propertyID]);
			}
		}
		return list;
	}

	private void AddExpendDollar(int dols)
	{
		if (OperInfo != null)
		{
			OperInfo.payDollars += dols;
		}
	}

	public void DeliverIAPItem(IAPName iapName)
	{
		Debug.Log("DeliverIAPItem " + iapName);
		switch (iapName)
		{
		case IAPName.M10:
			AddMithril(10);
			AddExpendDollar(1);
			break;
		case IAPName.M24:
			AddMithril(24);
			AddExpendDollar(2);
			break;
		case IAPName.M72:
			AddMithril(72);
			AddExpendDollar(5);
			break;
		case IAPName.M168:
			AddMithril(168);
			AddExpendDollar(10);
			break;
		case IAPName.M666:
			AddMithril(666);
			AddExpendDollar(30);
			break;
		case IAPName.M1430:
			AddMithril(1430);
			AddExpendDollar(50);
			break;
		case IAPName.M4000:
			AddMithril(4000);
			AddExpendDollar(100);
			break;
		case IAPName.ROOKIE:
		{
			if (bPurchaseRookie)
			{
				break;
			}
			for (int i = 0; i < Global.AVATAR_PART_NUM - 1; i++)
			{
				Armor armor = GetArmor(i, 1);
				armor.Level = 1;
				SetAvatar((BodyType)i, 1);
			}
			Weapon weapon = GetWeapons()[6];
			if (weapon.Level == 15 || weapon.Level == 0)
			{
				Weapon weapon2 = GetBattleWeapons()[0];
				int weaponBagIndex = GetWeaponBagIndex(weapon2);
				InsertPropsToStorage(weapon2.GunID + 1, 1);
				SetBagPosition(weaponBagIndex, 7);
				weapon.Level = 1;
			}
			if (GetNumFromStorage(88) + GetPropsNumFromBag(87) < 99)
			{
				InsertPropsToStorage(88, 2);
			}
			if (Application.loadedLevelName.Equals("ShopAndCustomize"))
			{
				GameObject gameObject = GameObject.Find("ShopAndCustomize");
				if (gameObject != null)
				{
					ShopAndCustomize component = gameObject.GetComponent<ShopAndCustomize>();
					if (component.FrGetCurrentPhase() == 3)
					{
						CustomizeUI customizeUI = component.GetCustomizeUI();
						if (customizeUI != null)
						{
							customizeUI.UpdateAvatar();
						}
					}
				}
			}
			bPurchaseRookie = true;
			AddExpendDollar(1);
			break;
		}
		case IAPName.SERGEANT:
			if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
			{
				if (!bPurchaseSergeant)
				{
					if (exp < 2500000)
					{
						exp = 2500000;
					}
					UnLockEquip();
					AddExpendDollar(3);
					bPurchaseSergeant = true;
				}
			}
			else if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				if (exp < 300)
				{
					exp = 300;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 300 && exp < 3500)
				{
					exp = 3500;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 3500 && exp < 15500)
				{
					exp = 15500;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 15500 && exp < 60000)
				{
					exp = 60000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 60000 && exp < 180000)
				{
					exp = 180000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 180000 && exp < 550000)
				{
					exp = 550000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 550000 && exp < 1300000)
				{
					exp = 1300000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 1300000 && exp < 2500000)
				{
					exp = 2500000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 2500000 && exp < 6000000)
				{
					exp = 6000000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 6000000 && exp < 11000000)
				{
					exp = 11000000;
					UnLockEquip();
					AddExpendDollar(1);
				}
				else if (exp >= 11000000 && exp < 20000000)
				{
					exp = 20000000;
					UnLockEquip();
					AddExpendDollar(1);
				}
			}
			break;
		}
		GameApp.GetInstance().Save();
	}

	public bool ArmorInOneCollection()
	{
		int armorGroupID = GetArmor(0).GetArmorGroupID();
		bool result = true;
		for (int i = 1; i < Global.AVATAR_PART_NUM - 1; i++)
		{
			Armor armor = GetArmor(i);
			if (armor.GetArmorGroupID() != armorGroupID)
			{
				result = false;
			}
		}
		return result;
	}

	public bool ArmorInOneCollection(int[] armors)
	{
		int armorGroupID = GetArmor(0, armors[0]).GetArmorGroupID();
		bool result = true;
		for (int i = 1; i < Global.AVATAR_PART_NUM - 1; i++)
		{
			Armor armor = GetArmor(i, armors[i]);
			if (armor.GetArmorGroupID() != armorGroupID)
			{
				result = false;
			}
		}
		return result;
	}

	public int OwnedSuitCount()
	{
		int num = 0;
		for (int i = 0; i < Global.TOTAL_ARMOR_NUM; i++)
		{
			bool flag = true;
			for (int j = 0; j < Global.AVATAR_PART_NUM - 1; j++)
			{
				if (armorList[j][i].Level == 0 || armorList[j][i].Level == 15)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				num++;
			}
		}
		return num;
	}

	public bool OwnSuit(int suitID)
	{
		bool result = true;
		for (int i = 0; i < Global.AVATAR_PART_NUM - 1; i++)
		{
			if (armorList[i][suitID].Level == 0 || armorList[i][suitID].Level == 15)
			{
				return false;
			}
		}
		return result;
	}

	public string GetRoleName()
	{
		return role_name;
	}

	public void SetRoleName(string name)
	{
		role_name = name;
	}

	public bool GetPlayMusic()
	{
		return bPlayMusic;
	}

	public void SetPlayMusic(bool isPlay)
	{
		bPlayMusic = isPlay;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetMusicMute(!bPlayMusic);
		}
	}

	public bool GetPlaySound()
	{
		return bPlaySound;
	}

	public void SetPlaySound(bool isPlay)
	{
		bPlaySound = isPlay;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetSoundMute(!bPlaySound);
		}
	}

	public float GetMusicVolume()
	{
		return musicVolume;
	}

	public void SetMusicVolume(float volume)
	{
		musicVolume = volume;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetMusicVolume(volume);
		}
	}

	public float GetSoundVolume()
	{
		return soundVolume;
	}

	public void SetSoundVolume(float volume)
	{
		soundVolume = volume;
		if (AudioManager.GetInstance() != null)
		{
			AudioManager.GetInstance().SetSoundVolume(volume);
		}
	}

	public void SetAvatar(BodyType bodyType, int avatarID)
	{
		avatar[(int)bodyType] = avatarID;
	}

	public void SetAvatar(byte[] armorIDs)
	{
		for (int i = 0; i < armorIDs.Length; i++)
		{
			avatar[i] = armorIDs[i];
		}
	}

	public void SetSuit(int avatarID)
	{
		for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
		{
			avatar[i] = avatarID;
		}
	}

	public int[] GetAvatar()
	{
		return avatar;
	}

	public Armor GetArmor(int part)
	{
		return GetArmor()[part][avatar[part]];
	}

	public Armor GetArmor(int part, int id)
	{
		return GetArmor()[part][id];
	}

	public int GetPropertyID(int bagIndex)
	{
		return bagPosition[bagIndex] - 1;
	}

	public Item GetItem(byte bagIndex)
	{
		int propertyID = GetPropertyID(bagIndex);
		if (propertyID > 80)
		{
			return itemList[propertyID - 81];
		}
		return null;
	}

	public void ClearItem(byte bagIndex)
	{
		int propertyID = GetPropertyID(bagIndex);
		if (propertyID > 80)
		{
			bagPosition[bagIndex] = 0;
		}
	}

	public WeaponUpgrade GetWeaponUpgrade()
	{
		return weaponUpgrade;
	}

	public void RemovePropsFromStorage(int propID)
	{
		int indexFromStorage = GetIndexFromStorage(propID);
		if (indexFromStorage != -1)
		{
			storageInfo[indexFromStorage, 1]--;
			if (storageInfo[indexFromStorage, 1] <= 0)
			{
				storageInfo[indexFromStorage, 0] = 0;
			}
		}
	}

	public void SetPropsToStorage(int index, int propID, int num)
	{
		storageInfo[index, 0] = (byte)propID;
		storageInfo[index, 1] = (byte)num;
	}

	public void InsertPropsToStorage(int propID, int num)
	{
		int indexFromStorage = GetIndexFromStorage(propID);
		if (indexFromStorage != -1)
		{
			storageInfo[indexFromStorage, 1] += (byte)num;
			return;
		}
		indexFromStorage = GetIndexAvailableFormStorage();
		storageInfo[indexFromStorage, 0] = (byte)propID;
		storageInfo[indexFromStorage, 1] = (byte)num;
	}

	public byte[,] GetStorageInfo()
	{
		return storageInfo;
	}

	public void SetStorageInfo(byte[,] storage)
	{
		for (int i = 0; i < storage.GetLength(0); i++)
		{
			storageInfo[i, 0] = storage[i, 0];
			storageInfo[i, 1] = storage[i, 1];
		}
	}

	public int GetNumFromStorage(int propID)
	{
		for (int i = 0; i < storageInfo.GetLength(0); i++)
		{
			if (storageInfo[i, 0] == propID)
			{
				return storageInfo[i, 1];
			}
		}
		return 0;
	}

	public int GetIndexFromStorage(int propID)
	{
		for (int i = 0; i < storageInfo.GetLength(0); i++)
		{
			if (storageInfo[i, 0] == propID)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetIndexAvailableFormStorage()
	{
		for (int i = 0; i < storageInfo.GetLength(0); i++)
		{
			if (storageInfo[i, 0] == 0)
			{
				return i;
			}
		}
		return -1;
	}

	public void ClearArmors()
	{
		for (int i = 0; i < armorList.Count; i++)
		{
			armorList[i].Clear();
		}
		armorList.Clear();
	}

	public Rank GetRank(int exp)
	{
		for (int num = rankList.Count - 1; num >= 0; num--)
		{
			if (rankList[num].exp <= exp)
			{
				return rankList[num];
			}
		}
		return null;
	}

	public Rank GetRank()
	{
		return GetRank(GetExp());
	}

	public List<Rank> GetRankList()
	{
		return rankList;
	}

	public List<Armor> GetArmorListForUnLock()
	{
		List<Armor> list = new List<Armor>();
		Rank rank = GetRank(exp);
		for (int i = 0; i < armorList.Count; i++)
		{
			List<Armor> list2 = armorList[i];
			for (int j = 0; j < list2.Count; j++)
			{
				Armor armor = list2[j];
				if (armor.Level == 0 && armor.UnlockLevel <= rank.rankID)
				{
					list.Add(armor);
				}
			}
		}
		return list;
	}

	public List<Weapon> GetWeaponListForUnLock()
	{
		List<Weapon> list = new List<Weapon>();
		Rank rank = GetRank(exp);
		for (int i = 0; i < weaponList.Count; i++)
		{
			Weapon weapon = weaponList[i];
			if (weapon.Level == 0 && weapon.UnlockLevel <= rank.rankID)
			{
				list.Add(weapon);
			}
		}
		return list;
	}

	public byte[,] GetStageState()
	{
		return stageState;
	}

	public void SetStageState(byte[,] stages)
	{
		for (int i = 0; i < stages.GetLength(0); i++)
		{
			for (int j = 0; j < stages.GetLength(1); j++)
			{
				stageState[i, j] = stages[i, j];
			}
		}
	}

	public void SetCompletedLevelId(int levelId)
	{
		completedLevelId = (short)levelId;
	}

	public short GetCompletedLevelId()
	{
		return completedLevelId;
	}

	public void SetTwitter(bool twitter)
	{
		bTwitter = twitter;
	}

	public bool GetTwitter()
	{
		return bTwitter;
	}

	public void SetFacebook(bool facebook)
	{
		bFacebook = facebook;
	}

	public bool GetFacebook()
	{
		return bFacebook;
	}

	public void SetPurchaseRookie(bool purchaseRookie)
	{
		bPurchaseRookie = purchaseRookie;
	}

	public bool GetPurchaseRookie()
	{
		return bPurchaseRookie;
	}

	public void SetPurchaseSergeant(bool purchaseSergeant)
	{
		bPurchaseSergeant = purchaseSergeant;
	}

	public bool GetPurchaseSergeant()
	{
		return bPurchaseSergeant;
	}

	public byte GetDeadTimer()
	{
		return deadTimer;
	}

	public void SetDeadTimer(byte timer)
	{
		deadTimer = timer;
	}

	public byte AtomicDeadTimer()
	{
		deadTimer++;
		deadTimer %= 3;
		return deadTimer;
	}

	public bool GetFirstLunchApp()
	{
		return bFirstLunchApp;
	}

	public void SetFirstLunchApp(bool bFirst)
	{
		bFirstLunchApp = bFirst;
	}

	public byte GetTimeSpan()
	{
		return timeSpan;
	}

	public void SetTimeSpan(byte timeSpan)
	{
		this.timeSpan = timeSpan;
	}

	public void SetStageState(int stage_Idx, int subStage_Idx, int state)
	{
		stageState[stage_Idx, subStage_Idx] = (byte)state;
	}

	public byte GetStageState(int stage_Idx, int subStage_Idx)
	{
		return stageState[stage_Idx, subStage_Idx];
	}

	public byte GetStage()
	{
		return stageIdx;
	}

	public void SetStage(byte stage)
	{
		stageIdx = stage;
	}

	public byte GetSubStage()
	{
		return subStageIdx;
	}

	public void SetSubStage(byte subStage)
	{
		subStageIdx = subStage;
	}

	public byte GetNetStage()
	{
		return netStageIdx;
	}

	public void SetNetStage(byte netStage)
	{
		netStageIdx = netStage;
	}

	public long GetBossDate()
	{
		return bossDate;
	}

	public void SetBossDate(long timer)
	{
		bossDate = timer;
	}

	public short GetSuccBossStage()
	{
		if (succBossStage > 2)
		{
			succBossStage = 2;
		}
		return succBossStage;
	}

	public void SetSuccBossStage(short timer)
	{
		succBossStage = timer;
	}

	public void AddSuccBossStage(short timer)
	{
		succBossStage += timer;
		if (succBossStage > 2)
		{
			succBossStage = 2;
		}
	}

	public short GetSuccBossStageGetMithril()
	{
		return succBossStageGetMithril;
	}

	public void SetSuccBossStageGetMithril(short timer)
	{
		succBossStageGetMithril = timer;
	}

	public void AddSuccBossStageGetMithril(short timer)
	{
		succBossStageGetMithril += timer;
	}

	public byte[] GetSuccSoloBoss()
	{
		return succSoloBoss;
	}

	public int GetSuccSoloBoss(int level)
	{
		int num = 0;
		if (level < succSoloBoss.Length)
		{
			num = succSoloBoss[level];
		}
		if (num > 2)
		{
			num = 2;
		}
		return num;
	}

	public void AddSuccSoloBossStage(int level)
	{
		if (level < succSoloBoss.Length)
		{
			succSoloBoss[level]++;
			if (succSoloBoss[level] > 2)
			{
				succSoloBoss[level] = 2;
			}
		}
	}

	public void SetSuccSoloBoss(byte[] succBoss)
	{
		for (int i = 0; i < succBoss.Length; i++)
		{
			succSoloBoss[i] = succBoss[i];
		}
	}

	public void SetSuccSoloBoss(int levelId, int timer)
	{
		if (levelId < succSoloBoss.Length)
		{
			succSoloBoss[levelId] = (byte)timer;
		}
	}

	public byte[] GetSuccCoopBoss()
	{
		return succCoopBoss;
	}

	public int GetSuccCoopBoss(int level)
	{
		int num = 0;
		if (level < succCoopBoss.Length)
		{
			num = succCoopBoss[level];
		}
		if (num > 2)
		{
			num = 2;
		}
		return num;
	}

	public void SetSuccCoopBoss(byte[] succBoss)
	{
		for (int i = 0; i < succBoss.Length; i++)
		{
			succCoopBoss[i] = succBoss[i];
		}
	}

	public void SetSuccCoopBoss(int levelId, int timer)
	{
		if (levelId < succCoopBoss.Length)
		{
			succCoopBoss[levelId] = (byte)timer;
		}
	}

	public void AddSuccCoopBossStage(int level)
	{
		if (level < succCoopBoss.Length)
		{
			succCoopBoss[level]++;
			if (succCoopBoss[level] > 2)
			{
				succCoopBoss[level] = 2;
			}
		}
	}

	public byte[] GetSuccCoopBossGetMithril()
	{
		return succCoopBossGetMithril;
	}

	public int GetSuccCoopBossGetMithril(int level)
	{
		int num = 0;
		if (level < succCoopBossGetMithril.Length)
		{
			num = succCoopBossGetMithril[level];
		}
		if (num > 5)
		{
			num = 5;
		}
		return num;
	}

	public void SetSuccCoopBossGetMithril(byte[] succBossGetMithril)
	{
		for (int i = 0; i < succBossGetMithril.Length; i++)
		{
			succCoopBossGetMithril[i] = succBossGetMithril[i];
		}
	}

	public void SetSuccCoopBossGetMithril(int levelId, int timer)
	{
		if (levelId < succCoopBossGetMithril.Length)
		{
			succCoopBossGetMithril[levelId] = (byte)timer;
		}
	}

	public void AddSuccCoopBossGetMithril(int level)
	{
		if (level < succCoopBossGetMithril.Length)
		{
			succCoopBossGetMithril[level]++;
			if (succCoopBossGetMithril[level] > 5)
			{
				succCoopBossGetMithril[level] = 5;
			}
		}
	}

	public void SetSoloSuccBossTime(DateTime data)
	{
		soloSuccBossTime = data;
	}

	public DateTime GetSoloSuccBossTime()
	{
		return soloSuccBossTime;
	}

	public int[] GetBagPosition()
	{
		return bagPosition;
	}

	public int[] GetPropsInBag()
	{
		int num = 0;
		int num2 = 0;
		int[] array = null;
		for (int i = 0; i < bagPosition.Length; i++)
		{
			int propertyID = GetPropertyID(i);
			if (propertyID >= 0)
			{
				num++;
			}
		}
		array = new int[num];
		for (int j = 0; j < bagPosition.Length; j++)
		{
			int propertyID2 = GetPropertyID(j);
			if (propertyID2 >= 0)
			{
				array[num2] = propertyID2;
				num2++;
			}
		}
		return array;
	}

	public int GetPropsNumFromBag(int propID)
	{
		int num = 0;
		for (int i = 0; i < bagPosition.Length; i++)
		{
			int propertyID = GetPropertyID(i);
			if (propertyID == propID)
			{
				num++;
			}
		}
		return num;
	}

	public void SetBagPosition(byte[] weaponIDs)
	{
		for (int i = 0; i < weaponIDs.Length; i++)
		{
			bagPosition[i] = weaponIDs[i];
		}
	}

	public void SetBagPosition(int index, int weaponID)
	{
		bagPosition[index] = weaponID;
	}

	public List<Item> GetItem()
	{
		return itemList;
	}

	public List<Weapon> GetWeapons()
	{
		return weaponList;
	}

	public List<List<Armor>> GetArmor()
	{
		return armorList;
	}

	public List<ArmorRewards> GetArmorRewards()
	{
		return armorRewardList;
	}

	public byte GetBagNum()
	{
		return bagNum;
	}

	public void SetBagNum(byte bagNum)
	{
		this.bagNum = bagNum;
	}

	public int GetDiscountWeapon()
	{
		return discountweapon;
	}

	public void SetDiscountWeapon(int id)
	{
		discountweapon = id;
	}

	public int GetUseMithril()
	{
		return usemithrils;
	}

	public void SetUseMithril(int mithril)
	{
		usemithrils = mithril;
	}

	public void AddUseMithril(int addmithril)
	{
		usemithrils += addmithril;
		usemithrils = Mathf.Clamp(usemithrils, 0, Global.MAX_MITHRIL);
	}

	public int GetDiscountStatus()
	{
		return discountstatus;
	}

	public void SetDiscountStatus(int status)
	{
		discountstatus = status;
	}

	public string GetDiscountTime()
	{
		return discounttime;
	}

	public void SetDiscountTime(string time)
	{
		discounttime = time;
	}

	public int GetGameTime()
	{
		return gametime;
	}

	public void SetGameTime(int gtime)
	{
		gametime = gtime;
	}

	public void AddGameTime(int atime)
	{
		gametime += atime;
	}

	public int GetShowNotify()
	{
		return shownotify;
	}

	public void SetShowNotify(int show)
	{
		shownotify = show;
	}

	public void AddShowMovieTime()
	{
		showmovietime++;
	}

	public int GetShowMovieTime()
	{
		return showmovietime;
	}

	public void SetShowMovieTime(int time)
	{
		showmovietime = time;
	}

	public void SetShowMovieDate(string date)
	{
		showmoviedate = date;
	}

	public string GetShowMovieDate()
	{
		return showmoviedate;
	}

	public int GetMithril()
	{
		lock (syncMithrilLock)
		{
			return AntiCracking.DecryptBufferStr(mithril, "sw_mod");
		}
	}

	public void SetMithril(int _mithril)
	{
		lock (syncMithrilLock)
		{
			mithril = AntiCracking.CryptBufferStr(Mathf.Min(Global.MAX_MITHRIL, _mithril), "sw_mod");
		}
	}

	public void AddMithril(int _mithrilGot)
	{
		lock (syncMithrilLock)
		{
			SetMithril(Mathf.Min(Global.MAX_MITHRIL, GetMithril() + _mithrilGot));
		}
	}

	public int GetExp()
	{
		return exp;
	}

	public void SetExp(int exp)
	{
		this.exp = exp;
	}

	public void AddExp(int exp)
	{
		this.exp += exp;
	}

	public int GetCash()
	{
		return AntiCracking.DecryptBufferStr(cash, "sw_acc");
	}

	public void SetCash(int _cash)
	{
		cash = AntiCracking.CryptBufferStr(Mathf.Min(Global.MAX_CASH, _cash), "sw_acc");
	}

	public void AddCash(int _cash)
	{
		SetCash(Mathf.Min(Global.MAX_CASH, GetCash() + _cash));
	}

	public void SetRewardStatus(byte status)
	{
		showRewardRunningStatus = status;
	}

	public byte GetRewardStatus()
	{
		return showRewardRunningStatus;
	}

	public void Buy(int price)
	{
		SetCash(Mathf.Min(Global.MAX_CASH, GetCash() - price));
		Achievement.WasteMoney(price);
	}

	public void BuyWithMithril(int price)
	{
		lock (syncMithrilLock)
		{
			SetMithril(Mathf.Min(Global.MAX_MITHRIL, GetMithril() - price));
		}
	}

	public int GetSaveNum()
	{
		return saveNum;
	}

	public void SetSaveNum(int saveNum)
	{
		this.saveNum = saveNum;
	}

	public void SetBattleStates(IBattleState[] states)
	{
		for (int i = 0; i < states.Length; i++)
		{
			battleStates[i].SetState(states[i]);
		}
	}

	public IBattleState[] GetBattleStates()
	{
		return battleStates;
	}

	public bool GetShowTutorial()
	{
		return isShowTutorial2;
	}

	public void SetShowTutorial(bool status)
	{
		isShowTutorial2 = status;
	}

	public long GetLastLoginTicks()
	{
		return mLastLoginTicks;
	}

	public void SetLastLoginTicks(long ticks)
	{
		mLastLoginTicks = ticks;
	}

	public long GetNextLoginInterval()
	{
		return mNextLoginInterval;
	}

	public void SetNextLoginInterval(long interval)
	{
		mNextLoginInterval = interval;
	}

	public void WriteBagPositionBuffer(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.BAG_MAX_NUM; i++)
		{
			buffer.AddByte((byte)bagPosition[i]);
		}
	}

	public void WriteArmorState(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
		{
			buffer.AddByte((byte)avatar[i]);
		}
	}

	public void WritePropsOwnNum(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_ITEM_NUM; i++)
		{
			buffer.AddByte((byte)GetNumFromStorage(82 + i));
		}
	}

	public void WriteWeaponOwnState(BytesBuffer buffer)
	{
		for (int i = 0; i < 47; i++)
		{
			buffer.AddByte(weaponList[i].Level);
		}
	}

	public void WriteArmorOwnStateHead(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_ARMOR_HEAD_NUM; i++)
		{
			buffer.AddByte(armorList[0][i].Level);
		}
	}

	public void WriteArmorOwnStateBody(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_ARMOR_BODY_NUM; i++)
		{
			buffer.AddByte(armorList[1][i].Level);
		}
	}

	public void WriteArmorOwnStateArm(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_ARMOR_ARM_NUM; i++)
		{
			buffer.AddByte(armorList[2][i].Level);
		}
	}

	public void WriteArmorOwnStateFoot(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_ARMOR_FOOT_NUM; i++)
		{
			buffer.AddByte(armorList[3][i].Level);
		}
	}

	public void WriteArmorOwnStateBag(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_ARMOR_BAG_NUM; i++)
		{
			buffer.AddByte(armorList[4][i].Level);
		}
	}

	public void WriteBagMaxNum(BytesBuffer buffer)
	{
		buffer.AddByte(bagNum);
	}

	public void WriteExp(BytesBuffer buffer)
	{
		buffer.AddInt(exp);
	}

	public void WriteCash(BytesBuffer buffer)
	{
		buffer.AddInt(GetCash());
	}

	public void WriteMithril(BytesBuffer buffer)
	{
		buffer.AddInt(GetMithril());
	}

	public void WriteEnergy(BytesBuffer buffer)
	{
		buffer.AddInt(Enegy);
	}

	public void WriteSaveNum(BytesBuffer buffer)
	{
		buffer.AddInt(saveNum);
	}

	public void WriteLevelId(BytesBuffer buffer)
	{
		short s = GetCompletedLevelId();
		buffer.AddShort(s);
	}

	public void WriteBossDate(BytesBuffer buffer)
	{
		buffer.AddLong(bossDate);
	}

	public void WriteBossKillTime(BytesBuffer buffer)
	{
		buffer.AddShort(succBossStage);
	}

	public void WriteBossDropMithrilTime(BytesBuffer buffer)
	{
		buffer.AddShort(succBossStageGetMithril);
	}

	public void WriteTwitter(BytesBuffer buffer)
	{
		buffer.AddBool(bTwitter);
	}

	public void WriteFacebook(BytesBuffer buffer)
	{
		buffer.AddBool(bFacebook);
	}

	public void WriteRewardStatus(BytesBuffer buffer)
	{
		buffer.AddByte(showRewardRunningStatus);
	}

	public void WriteBattleState(BytesBuffer buffer)
	{
		for (int i = 0; i < battleStates.Length; i++)
		{
			battleStates[i].WriteToBuffer(buffer);
		}
	}

	public void WriteSuccSoloBoss(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_SOLO_BOSS_NUM; i++)
		{
			buffer.AddByte(succSoloBoss[i]);
		}
	}

	public void WriteSuccCoopBoss(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_COOP_BOSS_NUM; i++)
		{
			buffer.AddByte(succCoopBoss[i]);
		}
	}

	public void WriteSuccCoopBossGetMithril(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_COOP_BOSS_NUM; i++)
		{
			buffer.AddByte(succCoopBossGetMithril[i]);
		}
	}

	public void WriteSoloSuccBossTime(BytesBuffer buffer)
	{
		buffer.AddString(soloSuccBossTime.ToShortDateString());
	}

	public void SaveBattleStates(BinaryWriter bw)
	{
		bw.Write((byte)battleStates.Length);
		for (int i = 0; i < battleStates.Length; i++)
		{
			battleStates[i].Save(bw);
		}
	}

	public void LoadBattleStates(BinaryReader br)
	{
		int num = br.ReadByte();
		for (int i = 0; i < num; i++)
		{
			battleStates[i].Load(br);
		}
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(version);
		IRecordset recordset = null;
		if (version.Equals(Record101.version))
		{
			recordset = new Record101(this);
		}
		else if (version.Equals(Record110.version) || version.Equals(Record110.versionE))
		{
			recordset = new Record110(this);
		}
		else if (version.Equals(Record120.version))
		{
			recordset = new Record120(this);
		}
		else if (version.Equals(Record121.version))
		{
			recordset = new Record121(this);
		}
		else if (version.Equals(Record122.version))
		{
			recordset = new Record122(this);
		}
		else if (version.Equals(Record123.version) || version.Equals(Record123.versionE))
		{
			recordset = new Record123(this);
		}
		else if (version.Equals(Record200.version))
		{
			recordset = new Record200(this);
		}
		else if (version.Equals(Record210.version))
		{
			recordset = new Record210(this);
		}
		else if (version.Equals(Record211.version))
		{
			recordset = new Record211(this);
		}
		else if (version.Equals(Record220.version))
		{
			recordset = new Record220(this);
		}
		else if (version.Equals(Record230.version))
		{
			recordset = new Record230(this);
		}
		else if (version.Equals(Record240.version) || version.Equals(Record240.versionE))
		{
			recordset = new Record240(this);
		}
		else if (version.Equals(Record260.version))
		{
			recordset = new Record260(this);
		}
		else if (version.Equals(Record270.version))
		{
			recordset = new Record270(this);
		}
		else if (version.Equals(Record280.version))
		{
			recordset = new Record280(this);
		}
		else if (version.Equals(Record290.version))
		{
			recordset = new Record290(this);
		}
		else if (version.Equals(Record291.version) || version.Equals(Record291.Eversion) || version.Equals(Record291.EEversion) || version.Equals(Record291.EEEversion) || version.Equals(Record291.EEEEversion))
		{
			recordset = new Record291(this);
		}
		SetSaveNum(saveNum + 1);
		recordset.SaveData(bw);
	}

	public string LoadData(BinaryReader br)
	{
		string text = br.ReadString();
		IRecordset recordset = null;
		if (text.Equals(Record101.version))
		{
			recordset = new Record101(this);
		}
		else if (text.Equals(Record110.version) || text.Equals(Record110.versionE))
		{
			recordset = new Record110(this);
		}
		else if (text.Equals(Record120.version))
		{
			recordset = new Record120(this);
		}
		else if (text.Equals(Record121.version))
		{
			recordset = new Record121(this);
		}
		else if (text.Equals(Record122.version))
		{
			recordset = new Record122(this);
		}
		else if (text.Equals(Record123.version) || text.Equals(Record123.versionE))
		{
			recordset = new Record123(this);
		}
		else if (text.Equals(Record200.version))
		{
			recordset = new Record200(this);
		}
		else if (text.Equals(Record210.version))
		{
			recordset = new Record210(this);
		}
		else if (text.Equals(Record211.version))
		{
			recordset = new Record211(this);
		}
		else if (text.Equals(Record220.version))
		{
			recordset = new Record220(this);
		}
		else if (text.Equals(Record230.version))
		{
			recordset = new Record230(this);
		}
		else if (text.Equals(Record240.version) || text.Equals(Record240.versionE))
		{
			recordset = new Record240(this);
		}
		else if (text.Equals(Record260.version))
		{
			recordset = new Record260(this);
		}
		else if (text.Equals(Record270.version))
		{
			recordset = new Record270(this);
		}
		else if (text.Equals(Record280.version))
		{
			recordset = new Record280(this);
		}
		else if (text.Equals(Record290.version))
		{
			recordset = new Record290(this);
		}
		else if (text.Equals(Record291.version) || version.Equals(Record291.Eversion) || version.Equals(Record291.EEversion) || version.Equals(Record291.EEEversion) || version.Equals(Record291.EEEEversion))
		{
			recordset = new Record291(this);
		}
		if (recordset != null)
		{
			recordset.LoadData(br);
			bInit = true;
		}
		return text;
	}

	public byte[] CryptMD5Buffer(byte[] data)
	{
		string s = md5Key + GameApp.GetInstance().UUID;
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		byte[] array = new byte[data.Length];
		data.CopyTo(array, 0);
		for (int i = 0; i < bytes.Length; i++)
		{
			array[i] ^= bytes[i];
		}
		MD5 mD = new MD5CryptoServiceProvider();
		return mD.ComputeHash(array);
	}

	public string CryptMD5String(string oriStr, string key)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(oriStr);
		byte[] bytes2 = Encoding.ASCII.GetBytes(key);
		byte[] array = new byte[bytes.Length];
		bytes.CopyTo(array, 0);
		for (int i = 0; i < bytes2.Length; i++)
		{
			array[i] ^= bytes2[i];
		}
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array2 = mD.ComputeHash(array);
		return array2.ToString();
	}

	public bool VerifyMD5(byte[] original, byte[] md5)
	{
		try
		{
			if (original == null || original.Length == 0)
			{
				return false;
			}
			byte[] array = new byte[original.Length];
			original.CopyTo(array, 0);
			array = CryptMD5Buffer(array);
			if (array == null || md5 == null)
			{
				return false;
			}
			if (array.Length != md5.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != md5[i])
				{
					return false;
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public byte[] GetBytesFromStream(Stream stream)
	{
		byte[] array = new byte[stream.Length];
		int num = (int)stream.Length;
		int num2 = 0;
		while (num > 0)
		{
			int num3 = stream.Read(array, num2, num);
			if (num3 == 0)
			{
				break;
			}
			num2 += num3;
			num -= num3;
		}
		return array;
	}

	public byte[] CryptBuffer(byte[] data)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(key);
		byte[] array = new byte[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			array[i] = (byte)(data[i] ^ bytes[i % bytes.Length]);
		}
		return array;
	}

	public byte[] DecryptBuffer(byte[] buffer)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(key);
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] ^= bytes[i % bytes.Length];
		}
		return buffer;
	}
}
