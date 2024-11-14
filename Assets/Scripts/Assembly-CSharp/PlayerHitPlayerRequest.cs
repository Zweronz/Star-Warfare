using UnityEngine;

public class PlayerHitPlayerRequest : Request
{
	protected short damage;

	protected int playerID;

	protected bool criticalDamage;

	protected byte weaponType;

	protected bool stealHeath;

	public PlayerHitPlayerRequest(short damage, int playerID, bool criticalDamage, byte weaponType)
	{
		TeamSkill otherTeamSkill = GameApp.GetInstance().GetGameWorld().OtherTeamSkill;
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		PlayerSkill skills = remotePlayerByUserID.GetSkills();
		WeaponType wType = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetWeapon()
			.GetWeaponType();
		float defenceSkillByWeaponType = remotePlayerByUserID.GetDefenceSkillByWeaponType(wType);
		float num = (1f + skills.GetSkill(SkillsType.DAMAGE_REDUCE)) * (1f + remotePlayerByUserID.GetDamageReduceItemAssist()) * (1f + otherTeamSkill.teamDamageReduce) * (1f + defenceSkillByWeaponType);
		if (remotePlayerByUserID.IsPowerUp(2))
		{
			num *= 0.14999998f;
		}
		if (remotePlayerByUserID.IsPowerUp(3))
		{
			num *= 0.7f;
		}
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (player.GetAttackItemAssist() > 0f)
		{
			num *= 1f + player.GetAttackItemAssist();
		}
		if (player.IsPowerUp(4))
		{
			num *= 0.65f;
			stealHeath = true;
		}
		if (remotePlayerByUserID.IsPowerUp(8))
		{
			num *= -0.6f;
		}
		else if (weaponType == 22)
		{
			stealHeath = true;
		}
		else
		{
			stealHeath = false;
		}
		if (player.IsPowerUp(0))
		{
			num *= 1.5f;
		}
		damage = (short)((float)damage * num * 1f);
		if (skills.GetSkill(SkillsType.BLOCK_AT_A_RATE) > 0f)
		{
			float num2 = Random.Range(0f, 1f);
			if (num2 < skills.GetSkill(SkillsType.BLOCK_AT_A_RATE))
			{
				damage = 0;
			}
		}
		this.damage = damage;
		this.playerID = playerID;
		this.criticalDamage = criticalDamage;
		this.weaponType = weaponType;
	}

	public PlayerHitPlayerRequest(short damage, int playerID, byte weaponType)
	{
		WeaponType weaponType2 = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetWeapon()
			.GetWeaponType();
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		this.damage = damage;
		this.playerID = playerID;
		criticalDamage = false;
		this.weaponType = weaponType;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(11);
		bytesBuffer.AddByte(115);
		bytesBuffer.AddByte(9);
		bytesBuffer.AddShort(damage);
		bytesBuffer.AddInt(playerID);
		bytesBuffer.AddBool(criticalDamage);
		bytesBuffer.AddByte(weaponType);
		bytesBuffer.AddBool(stealHeath);
		return bytesBuffer.GetBytes();
	}
}
