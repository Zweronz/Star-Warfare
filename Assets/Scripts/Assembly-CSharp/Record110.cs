using System.Collections.Generic;
using System.IO;

public class Record110 : IRecordset
{
	private UserState user;

	public static string version = "110";

	public static string versionE = "111";

	public Record110(UserState user)
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
		bw.Write(user.GetSuccBossStage());
		bw.Write(user.GetSuccBossStageGetMithril());
		bw.Write(user.GetCompletedLevelId());
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
		int num = br.ReadInt32();
		int num2 = br.ReadInt32();
		short completedLevelId = br.ReadInt16();
		byte[] array = new byte[Global.BAG_MAX_NUM];
		for (int i = 0; i < b; i++)
		{
			array[i] = br.ReadByte();
		}
		byte[] array2 = new byte[Global.AVATAR_PART_NUM];
		int num3 = br.ReadInt32();
		for (int j = 0; j < num3; j++)
		{
			array2[j] = br.ReadByte();
		}
		byte[] array3 = new byte[47];
		num3 = br.ReadInt32();
		for (int k = 0; k < num3; k++)
		{
			array3[k] = br.ReadByte();
		}
		byte[] array4 = new byte[Global.TOTAL_ARMOR_HEAD_NUM];
		byte[] array5 = new byte[Global.TOTAL_ARMOR_BODY_NUM];
		byte[] array6 = new byte[Global.TOTAL_ARMOR_ARM_NUM];
		byte[] array7 = new byte[Global.TOTAL_ARMOR_FOOT_NUM];
		byte[] array8 = new byte[Global.TOTAL_ARMOR_BAG_NUM];
		num3 = br.ReadInt32();
		int num4 = br.ReadInt32();
		for (int l = 0; l < num4; l++)
		{
			array4[l] = br.ReadByte();
		}
		num4 = br.ReadInt32();
		for (int m = 0; m < num4; m++)
		{
			array5[m] = br.ReadByte();
		}
		num4 = br.ReadInt32();
		for (int n = 0; n < num4; n++)
		{
			array6[n] = br.ReadByte();
		}
		num4 = br.ReadInt32();
		for (int num5 = 0; num5 < num4; num5++)
		{
			array7[num5] = br.ReadByte();
		}
		num4 = br.ReadInt32();
		for (int num6 = 0; num6 < num4; num6++)
		{
			array8[num6] = br.ReadByte();
		}
		byte[,] array9 = new byte[Global.TOTAL_STAGE, Global.TOTAL_SUB_STAGE];
		int num7 = br.ReadInt32();
		int num8 = br.ReadInt32();
		for (int num9 = 0; num9 < num7; num9++)
		{
			for (int num10 = 0; num10 < num8; num10++)
			{
				array9[num9, num10] = br.ReadByte();
			}
		}
		byte[,] array10 = new byte[Global.STORAGE_MAX_NUM, 2];
		int num11 = br.ReadInt32();
		int num12 = br.ReadInt32();
		for (int num13 = 0; num13 < num11; num13++)
		{
			for (int num14 = 0; num14 < num12; num14++)
			{
				array10[num13, num14] = br.ReadByte();
			}
		}
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
		user.SetSubStage(subStage);
		user.SetNetStage(netStage);
		user.SetTimeSpan(timeSpan);
		user.TouchInputSensitivity = (InputSensitivity)touchInputSensitivity;
		user.SetSuccBossStage((short)num);
		user.SetSuccBossStageGetMithril((short)num2);
		user.SetCompletedLevelId(completedLevelId);
		user.SetBagPosition(array);
		user.SetAvatar(array2);
		user.InitWeapons(array3);
		user.InitArmors(array4, array5, array6, array7, array8);
		user.SetStageState(array9);
		user.SetStorageInfo(array10);
		user.ReviseData();
	}
}
