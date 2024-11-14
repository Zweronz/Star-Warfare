public class PlayerInfo
{
	public int channelID;

	public byte seatID;

	public byte bagIdOfWeapon;

	public byte[] bags = new byte[Global.BAG_MAX_NUM];

	public byte[] armors = new byte[Global.AVATAR_PART_NUM];
}
