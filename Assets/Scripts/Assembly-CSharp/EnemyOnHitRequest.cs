public class EnemyOnHitRequest : Request
{
	protected short enemyID;

	protected short damage;

	protected byte weaponType;

	protected bool criticalAttack;

	protected bool stealHealth;

	public EnemyOnHitRequest(short enemyID, short damage, byte weaponType, bool criticalAttack, int weaponid)
	{
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (player.IsPowerUp(4))
		{
			damage = (short)((float)damage * 0.65f);
			stealHealth = true;
		}
		else if (weaponType == 22)
		{
			stealHealth = true;
		}
		else
		{
			stealHealth = false;
		}
		this.enemyID = enemyID;
		this.damage = damage;
		if (weaponid == 38)
		{
			this.weaponType = 38;
		}
		else
		{
			this.weaponType = weaponType;
		}
		this.criticalAttack = criticalAttack;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(9);
		bytesBuffer.AddByte(108);
		bytesBuffer.AddByte(7);
		bytesBuffer.AddShort(enemyID);
		bytesBuffer.AddShort(damage);
		bytesBuffer.AddByte(weaponType);
		bytesBuffer.AddBool(criticalAttack);
		bytesBuffer.AddBool(stealHealth);
		return bytesBuffer.GetBytes();
	}
}
