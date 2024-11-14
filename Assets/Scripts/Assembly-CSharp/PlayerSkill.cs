using System.Collections.Generic;

public class PlayerSkill
{
	protected List<Skill> skills = new List<Skill>();

	public void CreateSkills()
	{
		for (int i = 0; i < 58; i++)
		{
			Skill skill = new Skill();
			skill.skillType = (SkillsType)i;
			skill.data = 0f;
			skills.Add(skill);
		}
	}

	public void AddSkill(Skill skill)
	{
		skills[(int)skill.skillType].data += skill.data;
	}

	public float GetSkill(SkillsType sType)
	{
		return skills[(int)sType].data;
	}

	public override string ToString()
	{
		string text = string.Empty;
		for (int i = 0; i < 58; i++)
		{
			string text2 = text;
			text = string.Concat(text2, skills[i].skillType, ":", skills[i].data, "   ");
		}
		return text;
	}

	public bool HasSign(SkillsType type)
	{
		return type == SkillsType.HP_BOOTH || type == SkillsType.ATTACK_BOOTH || type == SkillsType.SPEED_BOOTH || type == SkillsType.MONEY_BOOTH || type == SkillsType.EXP_BOOTH || type == SkillsType.SAVE_ENEGY || type == SkillsType.DAMAGE_REDUCE || type == SkillsType.TEAM_HP_RECOVERY || type == SkillsType.HP_AUTO_RECOVERY || type == SkillsType.HP_RECOVERY_WHEN_MAKE_KILL || type == SkillsType.TEAM_ATTACK_BOOTH || type == SkillsType.LASER_BOOTH || type == SkillsType.LASER_CANNON_BOOTH || type == SkillsType.SHOTGUN_BOOTH || SkillsType.SPEEDUP_WHEN_GOT_HIT == type;
	}

	public bool IsPercetage(SkillsType type)
	{
		return type == SkillsType.ATTACK_BOOTH || type == SkillsType.MONEY_BOOTH || type == SkillsType.EXP_BOOTH || type == SkillsType.SAVE_ENEGY || type == SkillsType.RECOVERY_BOOTH || type == SkillsType.DAMAGE_REDUCE || type == SkillsType.TEAM_ATTACK_BOOTH || type == SkillsType.BLOCK_AT_A_RATE || type == SkillsType.LASER_BOOTH || type == SkillsType.LASER_CANNON_BOOTH || type == SkillsType.SHOTGUN_BOOTH || type == SkillsType.ASSAULT_BOOTH || type == SkillsType.TEAM_DAMAGE_REDUCE || type == SkillsType.SWORD_BOOTH || type == SkillsType.RPG_BOOTH || type == SkillsType.SWORD_DEFENCE || SkillsType.ATTACK_FRENQUENCY == type;
	}
}
