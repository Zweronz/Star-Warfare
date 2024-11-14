using System;
using UnityEngine;

public class PlayerLoginGameServerResponse : Response
{
	private bool loginSuccess;

	private int channelID;

	private byte[] weaponIDs = new byte[Global.BAG_MAX_NUM];

	private byte[] armorIDs = new byte[Global.AVATAR_PART_NUM];

	private byte[] ownedWeapons = new byte[47];

	private byte[] ownedArmorsHeads = new byte[Global.TOTAL_ARMOR_HEAD_NUM];

	private byte[] ownedArmorsBodies = new byte[Global.TOTAL_ARMOR_BODY_NUM];

	private byte[] ownedArmorsArms = new byte[Global.TOTAL_ARMOR_ARM_NUM];

	private byte[] ownedArmorsFeet = new byte[Global.TOTAL_ARMOR_FOOT_NUM];

	private byte[] ownedArmorsBags = new byte[Global.TOTAL_ARMOR_BAG_NUM];

	private byte[] ownedProps = new byte[Global.TOTAL_ITEM_NUM];

	private byte bagNum = Global.BAG_DEFAULT_NUM;

	private int exp;

	private int enegy;

	private int cash;

	private int saveNum;

	private short levelId;

	private long bossDate;

	private short bossKillTime;

	private short bossDropMithrilTime;

	private bool bTwitter;

	private bool bFacebook;

	private byte[] soloBossKilledTime = new byte[Global.TOTAL_SOLO_BOSS_NUM];

	private byte[] coopBossKilledTime = new byte[Global.TOTAL_COOP_BOSS_NUM];

	private byte[] coopBossDropMithrilTime = new byte[Global.TOTAL_COOP_BOSS_NUM];

	private string soloBossDate;

	private byte showRewardRunningStatus;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		byte b = bytesBuffer.ReadByte();
		if (b == 1)
		{
			loginSuccess = true;
		}
		else
		{
			loginSuccess = false;
		}
		channelID = bytesBuffer.ReadInt();
		if (loginSuccess)
		{
			cash = bytesBuffer.ReadInt();
			bagNum = bytesBuffer.ReadByte();
			enegy = bytesBuffer.ReadInt();
			exp = bytesBuffer.ReadInt();
			saveNum = bytesBuffer.ReadInt();
			levelId = bytesBuffer.ReadShort();
			bossDate = bytesBuffer.ReadLong();
			bossKillTime = bytesBuffer.ReadShort();
			bossDropMithrilTime = bytesBuffer.ReadShort();
			bTwitter = bytesBuffer.ReadBool();
			bFacebook = bytesBuffer.ReadBool();
			showRewardRunningStatus = bytesBuffer.ReadByte();
			for (int i = 0; i < Global.BAG_MAX_NUM; i++)
			{
				weaponIDs[i] = bytesBuffer.ReadByte();
			}
			for (int j = 0; j < Global.AVATAR_PART_NUM; j++)
			{
				armorIDs[j] = bytesBuffer.ReadByte();
			}
			for (int k = 0; k < 47; k++)
			{
				ownedWeapons[k] = bytesBuffer.ReadByte();
			}
			for (int l = 0; l < Global.TOTAL_ARMOR_HEAD_NUM; l++)
			{
				ownedArmorsHeads[l] = bytesBuffer.ReadByte();
			}
			for (int m = 0; m < Global.TOTAL_ARMOR_BODY_NUM; m++)
			{
				ownedArmorsBodies[m] = bytesBuffer.ReadByte();
			}
			for (int n = 0; n < Global.TOTAL_ARMOR_ARM_NUM; n++)
			{
				ownedArmorsArms[n] = bytesBuffer.ReadByte();
			}
			for (int num = 0; num < Global.TOTAL_ARMOR_FOOT_NUM; num++)
			{
				ownedArmorsFeet[num] = bytesBuffer.ReadByte();
			}
			for (int num2 = 0; num2 < Global.TOTAL_ARMOR_BAG_NUM; num2++)
			{
				ownedArmorsBags[num2] = bytesBuffer.ReadByte();
			}
			for (int num3 = 0; num3 < Global.TOTAL_ITEM_NUM; num3++)
			{
				ownedProps[num3] = bytesBuffer.ReadByte();
			}
			for (int num4 = 0; num4 < Global.TOTAL_SOLO_BOSS_NUM; num4++)
			{
				soloBossKilledTime[num4] = bytesBuffer.ReadByte();
			}
			for (int num5 = 0; num5 < Global.TOTAL_COOP_BOSS_NUM; num5++)
			{
				coopBossKilledTime[num5] = bytesBuffer.ReadByte();
			}
			for (int num6 = 0; num6 < Global.TOTAL_COOP_BOSS_NUM; num6++)
			{
				coopBossDropMithrilTime[num6] = bytesBuffer.ReadByte();
			}
			soloBossDate = bytesBuffer.ReadString();
		}
	}

	public override void ProcessLogic()
	{
		if (!loginSuccess)
		{
			return;
		}
		TimeManager.GetInstance().LastSynTime = Time.time;
		Lobby.GetInstance().SetChannelID(channelID);
		GameObject gameObject = GameObject.Find("StartMenu");
		if (gameObject != null)
		{
			StartMenuScript component = gameObject.GetComponent<StartMenuScript>();
			if (component != null)
			{
				NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
				UserState userState = GameApp.GetInstance().GetUserState();
				UploadBattleStateRequest request = new UploadBattleStateRequest(userState);
				networkManager.SendRequest(request);
				UploadOperatingInfo request2 = new UploadOperatingInfo(userState.OperInfo.MithrilRebirthTime, userState.OperInfo.payDollars, userState.OperInfo.UDID);
				networkManager.SendRequest(request2);
				userState.OperInfo.MithrilRebirthTime = 0;
				userState.OperInfo.payDollars = 0;
				component.CorrectLogin();
				UserData userData = component.GetUserData();
				userData.playerLogin = this;
				userData.saveNum = saveNum;
				component.PopupGift();
			}
		}
	}

	public void ResetUserStateFromNet()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		userState.SetCash(cash);
		userState.SetBagNum(bagNum);
		userState.Enegy = enegy;
		userState.SetExp(exp);
		userState.SetSaveNum(saveNum);
		userState.InitWeapons(ownedWeapons);
		userState.InitArmors(ownedArmorsHeads, ownedArmorsBodies, ownedArmorsArms, ownedArmorsFeet, ownedArmorsBags);
		userState.SetBagPosition(weaponIDs);
		userState.SetAvatar(armorIDs);
		userState.InitStorages(ownedWeapons, ownedProps);
		userState.UnlockLevelFromCompletedLevel(levelId);
		userState.UnLockEquip();
		userState.SetBossDate(bossDate);
		userState.SetSuccBossStage(bossKillTime);
		userState.SetSuccBossStageGetMithril(bossDropMithrilTime);
		userState.SetTwitter(bTwitter);
		userState.SetFacebook(bFacebook);
		userState.SetRewardStatus(showRewardRunningStatus);
		userState.SetSuccSoloBoss(soloBossKilledTime);
		userState.SetSuccCoopBoss(coopBossKilledTime);
		userState.SetSuccCoopBossGetMithril(coopBossDropMithrilTime);
		userState.SetSoloSuccBossTime(DateTime.Parse(soloBossDate));
		userState.ReviseData();
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		if (loginSuccess)
		{
			Debug.Log("Robot " + robot.userName + " login gameserver");
			robot.lobby.SetChannelID(channelID);
		}
	}
}
