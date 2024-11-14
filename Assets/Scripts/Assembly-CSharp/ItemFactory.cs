public class ItemFactory
{
	protected static ItemFactory instance;

	public static ItemFactory GetInstance()
	{
		if (instance == null)
		{
			instance = new ItemFactory();
		}
		return instance;
	}

	public Item CreateItem(byte itemID)
	{
		Item result = null;
		switch ((ItemID)itemID)
		{
		case ItemID.HP_MINOR:
		case ItemID.HP_SMALL:
		case ItemID.HP_MEDIUM:
		case ItemID.HP_GREAT:
		case ItemID.HP_FULL:
		case ItemID.HP_GIANT:
			result = new HpItem((ItemID)itemID);
			break;
		case ItemID.REVIVAL_SMALL:
		case ItemID.REVIVAL_BIG:
			result = new RevivalItem((ItemID)itemID);
			break;
		case ItemID.ASSIST_BOOSTER:
		case ItemID.ASSIST_FORCE_SHIELD:
		case ItemID.ASSIST_HYPER_CLIP:
			result = new AssistItem((ItemID)itemID);
			break;
		}
		return result;
	}
}
