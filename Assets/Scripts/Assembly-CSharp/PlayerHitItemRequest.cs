public class PlayerHitItemRequest : Request
{
	protected short damage;

	protected int itemID;

	protected bool criticalDamage;

	protected byte weaponType;

	public PlayerHitItemRequest(short damage, int itemID, bool criticalDamage, byte weaponType)
	{
		WeaponType weaponType2 = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetWeapon()
			.GetWeaponType();
		float num = 1f;
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (player.GetAttackItemAssist() > 0f)
		{
			num *= 1f + player.GetAttackItemAssist();
		}
		if (player.IsPowerUp(0))
		{
			num *= 1.5f;
		}
		damage = (short)((float)damage * num * 1f);
		this.damage = damage;
		this.itemID = itemID;
		this.criticalDamage = criticalDamage;
		this.weaponType = weaponType;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(8);
		bytesBuffer.AddByte(153);
		bytesBuffer.AddByte(6);
		bytesBuffer.AddShort(damage);
		bytesBuffer.AddShort((short)itemID);
		bytesBuffer.AddBool(criticalDamage);
		bytesBuffer.AddByte(weaponType);
		return bytesBuffer.GetBytes();
	}
}
