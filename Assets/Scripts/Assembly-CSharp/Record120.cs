using System.Collections.Generic;
using System.IO;

public class Record120 : IRecordset
{
	private UserState user;

	public static string version = "120";

	public Record120(UserState user)
	{
		this.user = user;
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(user.GetFirstLunchApp());
		bw.Write(user.GetPlaySound());
		bw.Write(user.GetPlayMusic());
		bw.Write((int)(user.GetSoundVolume() * 100f));
		bw.Write((int)(user.GetMusicVolume() * 100f));
		bw.Write(user.GetMithril());
		bw.Write(user.GetCash());
		byte bagNum = user.GetBagNum();
		bw.Write(bagNum);
		bw.Write(user.Enegy);
		bw.Write(user.GetExp());
		bw.Write(user.GetSaveNum());
		bw.Write(user.GetStage());
		bw.Write(user.GetSubStage());
		bw.Write(user.GetNetStage());
		bw.Write(user.GetTimeSpan());
		bw.Write((byte)user.TouchInputSensitivity);
		bw.Write(user.GetBossDate());
		bw.Write(user.GetSuccBossStage());
		bw.Write(user.GetSuccBossStageGetMithril());
		bw.Write(user.GetCompletedLevelId());
		bw.Write(user.GetTwitter());
		bw.Write(user.GetFacebook());
		for (int i = 0; i < bagNum; i++)
		{
			bw.Write((byte)user.GetBagPosition()[i]);
		}
		bw.Write(user.GetAvatar().Length);
		for (int j = 0; j < user.GetAvatar().Length; j++)
		{
			bw.Write((byte)user.GetAvatar()[j]);
		}
		bw.Write(user.GetWeapons().Count);
		for (int k = 0; k < user.GetWeapons().Count; k++)
		{
			Weapon weapon = user.GetWeapons()[k];
			bw.Write(weapon.Level);
		}
		List<List<Armor>> armor = user.GetArmor();
		bw.Write(armor.Count);
		for (int l = 0; l < armor.Count; l++)
		{
			List<Armor> list = armor[l];
			bw.Write(list.Count);
			for (int m = 0; m < list.Count; m++)
			{
				Armor armor2 = list[m];
				bw.Write(armor2.Level);
			}
		}
		bw.Write(user.GetStageState().GetLength(0));
		bw.Write(user.GetStageState().GetLength(1));
		for (int n = 0; n < user.GetStageState().GetLength(0); n++)
		{
			for (int num = 0; num < user.GetStageState().GetLength(1); num++)
			{
				bw.Write(user.GetStageState(n, num));
			}
		}
		bw.Write(user.GetStorageInfo().GetLength(0));
		bw.Write(user.GetStorageInfo().GetLength(1));
		byte[,] storageInfo = user.GetStorageInfo();
		for (int num2 = 0; num2 < storageInfo.GetLength(0); num2++)
		{
			for (int num3 = 0; num3 < storageInfo.GetLength(1); num3++)
			{
				bw.Write(storageInfo[num2, num3]);
			}
		}
		BattleRec120 battleRec = new BattleRec120(user);
		battleRec.SaveData(bw);
		OperRec121 operRec = new OperRec121(user.OperInfo);
		operRec.SaveData(bw);
		user.Achievement.Save(bw);
	}

	public void LoadData(BinaryReader br)
	{
		bool firstLunchApp = br.ReadBoolean();
		bool playSound = br.ReadBoolean();
		bool playMusic = br.ReadBoolean();
		float soundVolume = (float)br.ReadInt32() / 100f;
		float musicVolume = (float)br.ReadInt32() / 100f;
		int mithril = br.ReadInt32();
		int cash = br.ReadInt32();
		byte b = br.ReadByte();
		int enegy = br.ReadInt32();
		int exp = br.ReadInt32();
		int saveNum = br.ReadInt32();
		byte stage = br.ReadByte();
		byte subStage = br.ReadByte();
		byte netStage = br.ReadByte();
		byte timeSpan = br.ReadByte();
		byte touchInputSensitivity = br.ReadByte();
		long bossDate = br.ReadInt64();
		short succBossStage = br.ReadInt16();
		short succBossStageGetMithril = br.ReadInt16();
		short completedLevelId = br.ReadInt16();
		bool twitter = br.ReadBoolean();
		bool facebook = br.ReadBoolean();
		byte[] array = new byte[Global.BAG_MAX_NUM];
		for (int i = 0; i < b; i++)
		{
			array[i] = br.ReadByte();
		}
		byte[] array2 = new byte[Global.AVATAR_PART_NUM];
		int num = br.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			array2[j] = br.ReadByte();
		}
		byte[] array3 = new byte[47];
		num = br.ReadInt32();
		for (int k = 0; k < num; k++)
		{
			array3[k] = br.ReadByte();
		}
		byte[] array4 = new byte[Global.TOTAL_ARMOR_HEAD_NUM];
		byte[] array5 = new byte[Global.TOTAL_ARMOR_BODY_NUM];
		byte[] array6 = new byte[Global.TOTAL_ARMOR_ARM_NUM];
		byte[] array7 = new byte[Global.TOTAL_ARMOR_FOOT_NUM];
		byte[] array8 = new byte[Global.TOTAL_ARMOR_BAG_NUM];
		num = br.ReadInt32();
		int num2 = br.ReadInt32();
		for (int l = 0; l < num2; l++)
		{
			array4[l] = br.ReadByte();
		}
		num2 = br.ReadInt32();
		for (int m = 0; m < num2; m++)
		{
			array5[m] = br.ReadByte();
		}
		num2 = br.ReadInt32();
		for (int n = 0; n < num2; n++)
		{
			array6[n] = br.ReadByte();
		}
		num2 = br.ReadInt32();
		for (int num3 = 0; num3 < num2; num3++)
		{
			array7[num3] = br.ReadByte();
		}
		num2 = br.ReadInt32();
		for (int num4 = 0; num4 < num2; num4++)
		{
			array8[num4] = br.ReadByte();
		}
		byte[,] array9 = new byte[Global.TOTAL_STAGE, Global.TOTAL_SUB_STAGE];
		int num5 = br.ReadInt32();
		int num6 = br.ReadInt32();
		for (int num7 = 0; num7 < num5; num7++)
		{
			for (int num8 = 0; num8 < num6; num8++)
			{
				array9[num7, num8] = br.ReadByte();
			}
		}
		byte[,] array10 = new byte[Global.STORAGE_MAX_NUM, 2];
		int num9 = br.ReadInt32();
		int num10 = br.ReadInt32();
		for (int num11 = 0; num11 < num9; num11++)
		{
			for (int num12 = 0; num12 < num10; num12++)
			{
				array10[num11, num12] = br.ReadByte();
			}
		}
		user.InitBattleStates();
		BattleRec120 battleRec = new BattleRec120(user);
		battleRec.LoadData(br);
		OperRec121 operRec = new OperRec121(user.OperInfo);
		operRec.LoadData(br);
		user.Achievement = new AchievementState();
		user.Achievement.Load(br);
		user.SetFirstLunchApp(firstLunchApp);
		user.SetPlaySound(playSound);
		user.SetPlayMusic(playMusic);
		user.SetSoundVolume(soundVolume);
		user.SetMusicVolume(musicVolume);
		user.SetMithril(mithril);
		user.SetCash(cash);
		user.SetBagNum(b);
		user.Enegy = enegy;
		user.SetExp(exp);
		user.SetSaveNum(saveNum);
		user.SetStage(stage);
		user.SetBossDate(bossDate);
		user.SetSubStage(subStage);
		user.SetNetStage(netStage);
		user.SetTimeSpan(timeSpan);
		user.TouchInputSensitivity = (InputSensitivity)touchInputSensitivity;
		user.SetSuccBossStage(succBossStage);
		user.SetSuccBossStageGetMithril(succBossStageGetMithril);
		user.SetCompletedLevelId(completedLevelId);
		user.SetTwitter(twitter);
		user.SetFacebook(facebook);
		user.SetBagPosition(array);
		user.SetAvatar(array2);
		user.InitWeapons(array3);
		user.InitArmors(array4, array5, array6, array7, array8);
		user.SetStageState(array9);
		user.SetStorageInfo(array10);
		user.ReviseData();
	}
}
