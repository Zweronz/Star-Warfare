public class Global
{
	public const int AVATAR_PART_HEAD = 0;

	public const int AVATAR_PART_BODY = 1;

	public const int AVATAR_PART_ARM = 2;

	public const int AVATAR_PART_FOOT = 3;

	public const int AVATAR_PART_BAG = 4;

	public const int TOTAL_WEAPON_NUM = 47;

	public const byte STAGE_STATE_LOCK = 0;

	public const byte STAGE_STATE_UNLOCK = 1;

	public const byte EQUIP_STATE_LOCKED = 0;

	public const byte EQUIP_STATE_UNLOCKED = 15;

	public const int TEAM_SIZE = 4;

	public const float MIN_BLOOD_EFFECT_INTERVAL = 0.3f;

	public const float VS_WAIT_REBIRTH_TIME = 11f;

	public const string SCREEN_SHOT_FILENAME = "tempscreens.png";

	public const float CAMERA_RAYCAST_MOVE_DISTANCE = 1.8f;

	public const float DEFENCE_UP_VALUE = 0.85f;

	public const float ANDROMEDA_DEFENCE_UP_VALUE = 0.3f;

	public const int FLAG_TIME = 40;

	public const int VIP_TIME = 50;

	public const float STEAL_HEALTH_FACTOR = 1f;

	public const float DAMAGE_FACTOR_WHEN_STEAL_HEALTH = 0.65f;

	public const float SPEED_UP_WHEN_GOT_HIT_BUFF_DURATION = 2f;

	public const int MIN_CTF_FFA_RESUME_TIME = 10;

	public const float THUNDER_ALL_UP_VALUE = 1.5f;

	public const float IMPACT_WAVE_STEAL_HEALTH = 0.5f;

	public const float MACHINE_GUN_RPG_DAMAGE = 3f;

	public const float HURT_HEALTH_DAMAGE_TO_HP = 0.6f;

	public static byte BAG_DEFAULT_NUM = 4;

	public static int BAG_MAX_NUM = 8;

	public static int AVATAR_PART_NUM = 5;

	public static int TOTAL_ITEM_NUM = 11;

	public static int TOTAL_ITEM_CATEGORY_NUM = 3;

	public static int TOTAL_ITEM_CATEGORY_HP = 6;

	public static int TOTAL_ITEM_CATEGORY_REVIVAL = 2;

	public static int TOTAL_ITEM_CATEGORY_ASSIST = 3;

	public static int[] TOTAL_ITEM_CATEGORY = new int[3] { 6, 2, 3 };

	public static int[] ITEM_HP = new int[6] { 0, 1, 2, 3, 10, 4 };

	public static int[] ITEM_REVIVAL = new int[2] { 5, 6 };

	public static int[] ITEM_ASSIST = new int[3] { 7, 8, 9 };

	public static int TOTAL_ARMOR_NUM = 21;

	public static int TOTAL_ARMOR_HEAD_NUM = 21;

	public static int TOTAL_ARMOR_BODY_NUM = 21;

	public static int TOTAL_ARMOR_ARM_NUM = 21;

	public static int TOTAL_ARMOR_FOOT_NUM = 21;

	public static int TOTAL_ARMOR_BAG_NUM = 25;

	public static int CAMERA_ZOOM_SPEED = 10;

	public static int FLOORHEIGHT = 0;

	public static int TOTAL_STAGE = 8;

	public static int TOTAL_SUB_STAGE = 6;

	public static int TOTAL_SOLO_STAGE = 11;

	public static int TOTAL_VS_STAGE = 9;

	public static int TOTAL_BOSS_STAGE = 6;

	public static int TOTAL_SURVIVAL_STAGE = 8;

	public static byte[] VS_CMI_UNLOCK_SCENE = new byte[1] { 22 };

	public static int TOTAL_SOLO_BOSS_NUM = 5;

	public static int TOTAL_COOP_BOSS_NUM = 6;

	public static byte STORAGE_MAX_PANEL = 8;

	public static byte STORAGE_MAX_NUM = 72;

	public static byte DOUBLE_MANTIS_STAGE = 11;

	public static int MAX_CASH = 999999999;

	public static int MAX_ENEGY = 999999999;

	public static int MAX_MITHRIL = 999999999;

	public static int MAX_LEVEL_WEAPONW = 8;

	public static int TOTAL_IAP_CATEGORY_NUM = 2;

	public static int TOTAL_IAP_PURCHASE = 9;

	public static int TOTAL_IAP_EXCHANGE = 6;

	public static int IAP_CATEGORY_PURCHASE = 0;

	public static int IAP_CATEGORY_EXCHANGE = 1;

	public static int[] TOTAL_IAP_CATEGORY = new int[2] { 9, 6 };

	public static int SURVIVAL_DIFFICULTY_UP_EVERY_ROUND = 13;
}
