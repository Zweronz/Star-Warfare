public class SwErrorCode
{
	public static int SUCCESS;

	public static int GLOBAL_DATABASE_EXCEPTION = 1;

	public static int GLOBAL_HANDLE_EXCEPTION = 2;

	public static int GLOBAL_ACCOUNT_NOT_VERIFIED = 3;

	public static int GLOBAL_MD5_FAIL = 4;

	public static int GLOBAL_INVALID_USER_ID = 51;

	public static int ACCOUNT_EMAIL_INVALID = 101;

	public static int ACCOUNT_NAME_INVALID = 102;

	public static int ACCOUNT_PASSWORD_INVALID = 103;

	public static int ACCOUNT_UUID_INVALID = 104;

	public static int LOGIN_ACCOUNT_NOT_EXIST = 111;

	public static int LOGIN_PASSWORD_INCORRECT = 112;

	public static int LOGIN_GAMESERVER_BUSY = 113;

	public static int LOGIN_ACCOUNT_LOCK = 114;

	public static int LOGIN_VERSION_MISMATCH = 115;

	public static int LOGIN_GAMESERVER_MAINTENANCE = 116;

	public static int ACCOUNT_EMAIL_EXIST = 131;

	public static int ACCOUNT_NAME_EXIST = 132;

	public static int RENAME_NAME_INVALID = 141;

	public static int RENAME_NAME_EXIST = 142;

	public static int RENAME_TIME_LIMITATION = 143;

	public static int FRIEND_INVALID_FRIEND_ID = 151;

	public static int FRIEND_INVALID_FRIEND_NAME = 152;

	public static int FRIEND_SEARCH_USER_NOT_EXIST = 161;

	public static int FRIEND_SEARCH_ALREADY_FRIEND = 162;

	public static int FRIEND_SEARCH_SELF = 163;

	public static int FRIEND_SEARCH_MAX_REQUEST = 164;

	public static int FRIEND_ACCEPT_NO_INFO = 171;

	public static int FRIEND_ACCEPT_SELF_MAX = 172;

	public static int FRIEND_ACCEPT_OTHER_MAX = 173;

	public static int FRIEND_REMOVE_DEFAULT_FRIEND = 181;

	public static int VOID_UNKNOWN_ERROR = 201;

	public static int VOID_MATERIAL_NOT_ENOUGH = 202;

	public static int VOID_NOT_EXIST = 203;

	public static int VOID_NOT_AVAILABLE = 204;

	public static int VOID_REACH_COUNT_LIMIT = 205;

	public static int VOID_SLOT_ERROR = 206;

	public static int VOID_SLOT_REACH_MAX = 207;

	public static int VOID_HAS_BEEN_RESOLVED = 208;

	public static int VOID_ADD_VERIFIED_FAILED = 209;

	public static int VOID_CACHE_NOT_FOUND = 210;

	public static int VOID_ADD_SLOT_COUNT_WRONG = 211;

	public static int VOID_CREATE_TYPE_IS_WRONG = 212;

	public static int VOID_GET_RANDOM_VALUE_WRONG = 213;

	public static int VOID_GET_REWARD_VALUE_WRONG = 214;

	public static int VOID_IAP_CHEST_LEVEL_WRONG = 215;

	public static int VOID_RESOLVE_ID_WRONG = 216;

	public static int VOID_UPGRADE_ID_WRONG = 217;

	public static int DAILY_LOGIN_REWARD_UNKNOWN_ERROR = 251;

	public static int DAILY_BONUS_PLAYER_IS_NULL = 252;

	public static int DAILY_LOGIN_REWARD_ALREADY_RECEIVED = 253;

	public static int DAILY_QUEST_NOT_FINISH = 254;

	public static int DAILY_QUEST_REWARD_ALREADY_RECEIVED = 255;

	public static int DAILY_QUEST_FINAL_REWARD_ALREADY_RECEIVED = 256;

	public static int PRESTIGE_DATA_ERROR = 301;

	public static int PRESTIGE_RANK_NOT_ENOUGH = 302;

	public static int PRESTIGE_MAX = 303;
}
