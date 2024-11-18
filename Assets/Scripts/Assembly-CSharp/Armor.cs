using System.Collections.Generic;
using UnityEngine;

public class Armor
{
	protected BodyType armorType;

	protected byte armorID;

	protected byte level;

	protected GameObject equipment;

	protected string name;

	protected Vector3 center;

	protected List<Skill> skills = new List<Skill>();

	protected byte armorGroupID;

	protected byte unlockLevel;

	protected int price;

	protected int mithril;

	protected byte bagNum;

	public byte Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public GameObject Equipment
	{
		get
		{
			return equipment;
		}
		set
		{
			equipment = value;
		}
	}

	public Vector3 Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
		}
	}

	public byte UnlockLevel
	{
		get
		{
			return unlockLevel;
		}
	}

	public int Price
	{
		get
		{
			return price;
		}
	}

	public int Mithril
	{
		get
		{
			return mithril;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public byte BagNum
	{
		get
		{
			return bagNum;
		}
	}

	public Armor(BodyType type, int id)
	{
		armorID = (byte)id;
		armorType = type;
	}

	public byte GetArmorGroupID()
	{
		return armorGroupID;
	}

	public float GetSkill(SkillsType sType)
	{
		foreach (Skill skill in skills)
		{
			if (skill.skillType == sType)
			{
				return skill.data;
			}
		}
		return 0f;
	}

	public void CreateArmor()
	{
		string text = string.Format("NoMipMapRes/Avatar/{0:D2}", 1 + armorID);
		Object original = Resources.Load(text + UIConstant.AVATAR_DIR[(int)armorType]);
		equipment = Object.Instantiate(original) as GameObject;
	}

	public void LoadConfig(byte ownedState, int resID)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[14];
		if (unitDataTable == null)
		{
			return;
		}
		name = unitDataTable.GetData(resID, 0, string.Empty, false);
		float val = unitDataTable.GetData(resID, 2, 0, false);
		AddSkill(SkillsType.HP_BOOTH, val);
		val = (float)(sbyte)unitDataTable.GetData(resID, 3, 0, false) / 100f;
		AddSkill(SkillsType.ATTACK_BOOTH, val);
		val = (float)(sbyte)unitDataTable.GetData(resID, 4, 0, false) / 10f;
		AddSkill(SkillsType.SPEED_BOOTH, val);
		val = (float)(sbyte)unitDataTable.GetData(resID, 5, 0, false) / 100f;
		AddSkill(SkillsType.MONEY_BOOTH, val);
		val = (float)(sbyte)unitDataTable.GetData(resID, 6, 0, false) / 100f;
		AddSkill(SkillsType.EXP_BOOTH, val);
		armorGroupID = (byte)unitDataTable.GetData(resID, 10, 0, false);
		unlockLevel = (byte)unitDataTable.GetData(resID, 12, 0, false);
		bagNum = (byte)unitDataTable.GetData(resID, 13, 0, false);
		price = unitDataTable.GetData(resID, 14, 0, false);
		mithril = unitDataTable.GetData(resID, 15, 0, false);
		UnitDataTable unitDataTable2 = Res2DManager.GetInstance().vDataTable[73];
		UnitDataTable unitDataTable3 = Res2DManager.GetInstance().vDataTable[74];
		UnitDataTable unitDataTable4 = Res2DManager.GetInstance().vDataTable[75];
		for (int i = 0; i < 3; i++)
		{
			int data = unitDataTable.GetData(resID, 7 + i, 0, false);
			if (data > 0)
			{
				val = (float)(sbyte)unitDataTable2.GetData(data, 0, 0, false) / 100f;
				AddSkill(SkillsType.SAVE_ENEGY, val);
				val = (sbyte)unitDataTable2.GetData(data, 1, 0, false);
				AddSkill(SkillsType.HP_AUTO_RECOVERY, val);
				val = (sbyte)unitDataTable2.GetData(data, 2, 0, false);
				AddSkill(SkillsType.HP_RECOVERY_WHEN_MAKE_KILL, val);
				val = (float)(sbyte)unitDataTable2.GetData(data, 3, 0, false) / 100f;
				AddSkill(SkillsType.RECOVERY_BOOTH, val);
				int data2 = unitDataTable2.GetData(data, 4, 0, false);
				if (data2 > 0)
				{
					val = (float)(sbyte)unitDataTable3.GetData(data2, 0, 0, false) / 100f;
					AddSkill(SkillsType.ASSAULT_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 1, 0, false) / 100f;
					AddSkill(SkillsType.SHOTGUN_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 2, 0, false) / 100f;
					AddSkill(SkillsType.RPG_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 3, 0, false) / 100f;
					AddSkill(SkillsType.GRENADE_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 4, 0, false) / 100f;
					AddSkill(SkillsType.LASER_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 5, 0, false) / 100f;
					AddSkill(SkillsType.LASER_CANNON_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 6, 0, false) / 100f;
					AddSkill(SkillsType.PLASMA_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 7, 0, false) / 100f;
					AddSkill(SkillsType.MACHINE_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 8, 0, false) / 100f;
					AddSkill(SkillsType.BOW_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 9, 0, false) / 100f;
					AddSkill(SkillsType.IMPULSE_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 10, 0, false) / 100f;
					AddSkill(SkillsType.GLOVE_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 11, 0, false) / 100f;
					AddSkill(SkillsType.SWORD_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 12, 0, false) / 100f;
					AddSkill(SkillsType.SNIPER_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 13, 0, false) / 100f;
					AddSkill(SkillsType.TRACKINGGUN_BOOTH, val);
					val = (float)(sbyte)unitDataTable3.GetData(data2, 14, 0, false) / 100f;
					AddSkill(SkillsType.PINGPONG_BOOTH, val);
				}
				val = (float)(sbyte)unitDataTable2.GetData(data, 5, 0, false) / 100f;
				AddSkill(SkillsType.BLOCK_AT_A_RATE, val);
				val = (float)(sbyte)unitDataTable2.GetData(data, 6, 0, false) / 100f;
				AddSkill(SkillsType.DAMAGE_REDUCE, val);
				val = (sbyte)unitDataTable2.GetData(data, 7, 0, false);
				AddSkill(SkillsType.TEAM_HP_RECOVERY, val);
				val = (float)(sbyte)unitDataTable2.GetData(data, 8, 0, false) / 100f;
				AddSkill(SkillsType.TEAM_ATTACK_BOOTH, val);
				val = (int)(byte)unitDataTable2.GetData(data, 9, 0, false);
				if (val == 1f)
				{
					AddSkill(SkillsType.POWER_UP, val);
				}
				else if (val == 2f)
				{
					AddSkill(SkillsType.SPEED_UP, val);
				}
				else if (val == 3f)
				{
					AddSkill(SkillsType.DEFENCE_UP, val);
				}
				else if (val == 4f)
				{
					AddSkill(SkillsType.ANDROMEDA_UP, val);
				}
				else if (val == 5f)
				{
					AddSkill(SkillsType.HEALTH_STEAL, val);
				}
				else if (val == 6f)
				{
					AddSkill(SkillsType.ATTACK_SHIELD, val);
				}
				else if (val == 7f)
				{
					AddSkill(SkillsType.IMPACT_WAVE, val);
				}
				else if (val == 8f)
				{
					AddSkill(SkillsType.TRACK_WAVE, val);
				}
				else if (val == 9f)
				{
					AddSkill(SkillsType.HURT_HEALTH, val);
				}
				else if (val == 10f)
				{
					AddSkill(SkillsType.GRAVITY_FORCE, val);
				}
				val = (int)(byte)unitDataTable2.GetData(data, 10, 0, false);
				AddSkill(SkillsType.UNLIMITED_ENEGY, val);
				val = (int)(byte)unitDataTable2.GetData(data, 11, 0, false);
				AddSkill(SkillsType.FLY, val);
				val = (float)(sbyte)unitDataTable2.GetData(data, 12, 0, false) / 100f;
				AddSkill(SkillsType.TEAM_DAMAGE_REDUCE, val);
				int num = (sbyte)unitDataTable2.GetData(data, 13, 0, false);
				if (num > 0)
				{
					val = (float)(sbyte)unitDataTable4.GetData(num, 0, 0, false) / 100f;
					AddSkill(SkillsType.ASSAULT_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 1, 0, false) / 100f;
					AddSkill(SkillsType.SHOTGUN_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 2, 0, false) / 100f;
					AddSkill(SkillsType.RPG_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 3, 0, false) / 100f;
					AddSkill(SkillsType.GRENADE_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 4, 0, false) / 100f;
					AddSkill(SkillsType.LASER_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 5, 0, false) / 100f;
					AddSkill(SkillsType.LASER_CANNON_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 6, 0, false) / 100f;
					AddSkill(SkillsType.PLASMA_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 7, 0, false) / 100f;
					AddSkill(SkillsType.MACHINE_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 8, 0, false) / 100f;
					AddSkill(SkillsType.BOW_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 9, 0, false) / 100f;
					AddSkill(SkillsType.IMPULSE_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 10, 0, false) / 100f;
					AddSkill(SkillsType.GLOVE_DEFENCE, val);
					val = (float)(sbyte)unitDataTable4.GetData(num, 11, 0, false) / 100f;
					AddSkill(SkillsType.SWORD_DEFENCE, val);
					val = (float)(sbyte)unitDataTable3.GetData(num, 12, 0, false) / 100f;
					AddSkill(SkillsType.SNIPER_DEFENCE, val);
					val = (float)(sbyte)unitDataTable3.GetData(num, 13, 0, false) / 100f;
					AddSkill(SkillsType.TRACKINGGUN_DEFENCE, val);
					val = (float)(sbyte)unitDataTable3.GetData(num, 14, 0, false) / 100f;
					AddSkill(SkillsType.PINGPONG_DEFENCE, val);
				}
				val = (float)(sbyte)unitDataTable2.GetData(data, 14, 0, false) / 10f;
				AddSkill(SkillsType.SPEEDUP_WHEN_GOT_HIT, val);
				val = (float)(sbyte)unitDataTable2.GetData(data, 15, 0, false) / 100f;
				AddSkill(SkillsType.ATTACK_FRENQUENCY, val);
			}
		}
	}

	protected void AddSkill(SkillsType type, float val)
	{
		if (val != 0f)
		{
			Skill skill = new Skill();
			skill.skillType = type;
			skill.data = val;
			skills.Add(skill);
		}
	}

	public byte GetArmorID()
	{
		return armorID;
	}

	public BodyType GetArmorType()
	{
		return armorType;
	}

	public List<Skill> GetSkills()
	{
		return skills;
	}
}