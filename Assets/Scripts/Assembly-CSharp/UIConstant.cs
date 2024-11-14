using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConstant
{
	public const byte UI_LOADING_INDEX = 22;

	public const byte UI_INGAME_INDEX = 0;

	public const byte UI_MAIN_MENU = 1;

	public const byte UI_LOGO_INDEX = 2;

	public const byte UI_STAGE_CHOISE = 3;

	public const byte UI_MULTI_PLAYER = 4;

	public const byte UI_CREATE_ROOM = 5;

	public const byte UI_READY_GAME = 6;

	public const byte UI_EXTRA = 7;

	public const byte UI_OPTION_INDEX = 8;

	public const byte UI_INGAME_OPTION_INDEX = 9;

	public const byte UI_CUSTOMIZE_INDEX = 10;

	public const byte UI_STORE_INDEX = 11;

	public const byte UI_MAKE_PACKAGE_INDEX = 12;

	public const byte UI_PROPS_STORE_INDEX = 13;

	public const byte UI_NAVIGATION_MENU_INDEX = 14;

	public const byte UI_STATISTICS_INDEX = 15;

	public const byte UI_PAUSE_INDEX = 16;

	public const byte UI_NUMERIC_INDEX = 17;

	public const byte UI_REFUEL_INDEX = 18;

	public const byte UI_NAVIGATION_BAR_INDEX = 19;

	public const byte UI_ICON_INDEX = 20;

	public const byte UI_IAP_INDEX = 21;

	public const byte UI_MESSAGE_BOX_INDEX = 23;

	public const byte UI_GIFT_INDEX = 24;

	public const byte UI_TUTORIAL_INDEX = 25;

	public const byte UI_IPAD_INDEX = 26;

	public const byte UI_PLAYER_ICON_INDEX = 27;

	public const byte UI_ADS_LIST = 28;

	public const byte DATATABLE_WEAPON = 13;

	public const byte DATATABLE_ARMOR = 14;

	public const byte DATATABLE_ARMOR_REWARDS = 15;

	public const byte DATATABLE_ITEM = 16;

	public const byte DATATABLE_RANK = 17;

	public const byte DATATABLE_WEAPON_UPGRADE = 18;

	public const byte DATATABLE_SKILL = 73;

	public const byte DATATABLE_WEAPON_ENHANCE = 74;

	public const byte DATATABLE_WEAPON_DEFENCE = 75;

	public const byte DATATABLE_ENEMY_SPAWN = 19;

	public const byte DATATABLE_ENEMY_CONFIG = 0;

	public const byte DATATABLE_ENEMY_ATTACK = 1;

	public const byte DATATABLE_ENEMY_ATTACK_COUNT = 12;

	public const byte UI_TEXT_CATEGORY_WEAPON = 0;

	public const byte UI_TEXT_CATEGORY_PACK = 2;

	public const byte UI_TEXT_CATEGORY_BONUS = 1;

	public const byte UI_TEXT_CATEGORY_PROPS = 3;

	public const byte UI_TEXT_CATEGORY_MESSAGE = 4;

	public const byte UI_TEXT_CATEGORY_MAP_NAME = 5;

	public const byte UI_TEXT_CATEGORY_TIPS = 6;

	public const byte UI_TEXT_MESSAGE_QUIT_GAME = 0;

	public const byte UI_TEXT_MESSAGE_NET_DONOT_ACCESS = 2;

	public const byte UI_TEXT_MESSAGE_LOST_CONNECTION = 8;

	public const byte UI_TEXT_MESSAGE_PURCHASE_RANK = 17;

	public const byte UI_TEXT_MESSAGE_PURCHASE_MITHRIL = 11;

	public const byte UI_TEXT_MESSAGE_PURCHASE_CASH = 12;

	public const byte UI_TEXT_MESSAGE_UPGRADE = 15;

	public const byte UI_TEXT_MESSAGE_UPLOAD = 16;

	public const byte UI_TEXT_MESSAGE_GAME_CENTER_ERROR = 18;

	public const byte UI_TEXT_MESSAGE_DEAD_TIPS = 19;

	public const byte UI_TEXT_MESSAGE_DEAD_TIPS_1 = 20;

	public const byte UI_TEXT_MESSAGE_MAKE_PACKAGE = 21;

	public const byte UI_TEXT_MESSAGE_TERMINAL_TIMEOUT = 22;

	public const byte UI_TEXT_MESSAGE_ACCOUNT_LOCKED = 23;

	public const byte UI_TEXT_MESSAGE_SERVER_MIANTENANCE = 24;

	public const byte UI_TEXT_MESSAGE_VERSION_MISMATCH = 25;

	public const byte UI_TEXT_MESSAGE_UPLOAD_CONFIRM = 26;

	public const byte UI_TEXT_MESSAGE_DOWNLOAD_CONFIRM = 27;

	public const byte UI_TEXT_MESSAGE_LITE_VERSION = 28;

	public const byte UI_TEXT_MESSAGE_LOGIN_COOP = 29;

	public const byte UI_TEXT_MESSAGE_GUEST_INFO = 30;

	public const byte UI_TEXT_MESSAGE_TOO_MANY_IN_STORE = 31;

	public const byte UI_TEXT_MESSAGE_PASSWORD_ERROR_JOINROOM = 32;

	public const byte UI_TEXT_MESSAGE_RANK_LOW_JOINROOM = 33;

	public const byte UI_TEXT_MESSAGE_RANK_HIGH_JOINROOM = 34;

	public const byte UI_TEXT_MESSAGE_PASSWORD_INPUT = 35;

	public const byte UI_TEXT_MESSAGE_LEAVE_INGAME_VS_MODE = 36;

	public const byte UI_TEXT_MESSAGE_BOSS_MITHRIL_TIPS = 37;

	public const byte UI_TEXT_MESSAGE_SEND_FACEBOOK_GEARS = 38;

	public const byte UI_TEXT_MESSAGE_SEND_FACEBOOK_BATTLE = 39;

	public const byte UI_TEXT_MESSAGE_SEND_TWITTER_GEARS = 41;

	public const byte UI_TEXT_MESSAGE_SEND_TWITTER_BATTLE = 42;

	public const byte UI_TEXT_MESSAGE_CAN_NOT_ACCESS_INTERNET = 40;

	public const byte UI_TEXT_MESSAGE_RECORD_ERROR = 43;

	public const byte UI_TEXT_MESSAGE_IAP_TIPS = 44;

	public const string EMPTY_STRING = "[EMPTY]";

	public const byte SCENE_STARTMENU = 0;

	public const byte SCENE_SOLOMENU = 1;

	public const byte SCENE_MULTIMENU = 2;

	public static Vector2 ScreenAdaptived;

	public static float ScreenLocalWidth = 960f;

	public static float ScreenLocalHeight = 640f;

	public static int[] IAP_MITHRIL = new int[9] { 0, 0, 10, 24, 72, 168, 666, 1430, 4000 };

	public static float[] IAP_PRICE = new float[9] { 0.99f, 2.99f, 0.99f, 1.99f, 4.99f, 9.99f, 29.99f, 49.99f, 99.99f };

	public static float[] IAP_PRICE_KINDLE = new float[9] { 0.99f, 0.99f, 0.99f, 1.99f, 4.99f, 9.99f, 29.99f, 49.99f, 99.99f };

	public static string[] IAP_ORIGIONAL_PRICE = new string[9] { "0.99", "1.99", "0.99", "2.34", "7.13", "16.65", "66.64", "142.83", "399.96" };

	public static string[] IAP_SAVING_PRICE = new string[9]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		"0.35",
		"2.14",
		"6.66",
		"36.65",
		"92.84",
		"299.97"
	};

	public static int[] IAP_EXCHANGE_MITHRIL = new int[6] { 1, 5, 10, 50, 100, 500 };

	public static int[] IAP_EXCHANGE_CASH = new int[6] { 10000, 60000, 140000, 900000, 2200000, 15000000 };

	public static int[] SUCCESS_STAGE_AWARD = new int[8] { 20000, 50000, 80000, 120000, 200000, 300000, 500000, 700000 };

	public static int[] GIFT_CASH = new int[5] { 8000, 16000, 24000, 2, 3 };

	public static string[] ITEM_CATEGORY = new string[3] { "HEALTH", "AID-KIT", "ASSIST" };

	public static string[] OPTION_CATEGORY = new string[3] { "SOUND", "CONTROLS", "ADVANCED" };

	public static byte[] NET_STAGE_LEVEL = new byte[12]
	{
		0, 1, 2, 3, 4, 5, 5, 2, 4, 6,
		6, 6
	};

	public static string SHOW_EQUIP_STR = "Hey! Did you see me? My GEARS are soooooo cool. Come and play with me in the world of StarWarfare!";

	public static string SHOW_SCORE_STR = "I got a high score and can't wait to share it with you. And your friends need you, fight with me!";

	public static string HEY = "My Gears! ";

	public static string FULL_VERSION_URL = "https://play.google.com/store/apps/details?id=com.ifreyrgames.starwarfare";

	public static string FACEBOOK_HOME = "http://www.facebook.com/pages/Freyr-Games/159454344141173";

	public static string TWITTER_HOME = "https://twitter.com/#!/FreyrGames";

	public static string RPG_VERSION_URL = "https://play.google.com/store/apps/details?id=com.ifreyrgames.blackdawn";

	public static string RUSH_VERSION_URL = "http://itunes.apple.com/us/app/id547135385";

	public static string[] avatar = new string[6] { "Helmet", "Body", "Arms", "Legs", "Pack", "Gun" };

	public static string[] SUB_AVATAR = new string[5] { "head", "body", "hand", "foot", "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/fly_bag/Bag" };

	public static string[] AVATAR_DIR = new string[5] { "/Head", "/Body", "/Hand", "/Foot", "/Bag" };

	public static string[] AVATAR_DIR_FR = new string[5] { "_Head", "_Body", "_Hand", "_Foot", "_Bag" };

	public static string[] GAME_MODE_COOP = new string[2] { "BOSS", "SURVIVAL" };

	public static string[] GAME_MODE_VS = new string[6] { "CATCH ME", "VERY INSANE PARTY", "CAPTURE THE FLAG", "ALL FOR ONE", "TEAM DEATHMATCH", "FREE FOR ALL" };

	public static byte GAME_MODE_TOTAL = 2;

	public static string GUN_COST = "COST:";

	public static string GUN_AMMO = "AMMO:";

	public static byte TOTAL_SEARCH_ROOM_CATEGORY = 3;

	public static string[] PLAYER_NUM_COOP = new string[3] { "1 PLAYER", "2 PLAYERS", "3 PLAYERS" };

	public static string[] PLAYER_NUM_VS_TDM = new string[1] { "4 VS 4 PLAYERS" };

	public static string[] PLAYER_NUM_VS_FFA = new string[1] { "8 PLAYERS" };

	public static string[] WIN_CONDITION = new string[2] { "SCORE", "TIMER" };

	public static string[] WIN_VALUE_FOR_SCORE = new string[3] { "500", "1000", "1500" };

	public static string[] WIN_VALUE_FOR_TIMER = new string[3] { "5", "10", "15" };

	public static string[] STR_SEARCH_ROOM_GAME_MODE_COOP = new string[3] { "ANY", "SURVIVAL", "BOSS" };

	public static string[] STR_SEARCH_ROOM_GAME_MODE_VS = new string[7] { "ANY", "CAPTURE THE FLAG", "ALL FOR ONE", "TEAM DEATHMATCH", "FREE FOR ALL", "VIP", "CATCH ME" };

	public static string[] STR_SEARCH_ROOM_PLAYER_NUM_COOP = new string[3] { "ANY", "2 PLAYERS", "3 PLAYERS" };

	public static string[] STR_SEARCH_ROOM_WIN_CONDITION = new string[3] { "ANY", "SCORE", "TIMER" };

	public static string[] STR_SEARCH_ROOM_WIN_VALUE_FOR_SCORE = new string[4] { "ANY", "500", "1000", "1500" };

	public static string[] STR_SEARCH_ROOM_WIN_VALUE_FOR_TIMER = new string[4] { "ANY", "5", "10", "15" };

	public static byte[] VS_CMI_SPECIAL_SCENE = new byte[1] { 8 };

	public static byte[] SEARCH_ROOM_PLAYER_NUM_COOP = new byte[3] { 0, 2, 3 };

	public static byte[] SEARCH_ROOM_WIN_CONDITION = new byte[3] { 100, 1, 0 };

	public static short[] SEARCH_ROOM_WIN_VALUE_FOR_SCORE = new short[4] { 0, 500, 1000, 1500 };

	public static short[] SEARCH_ROOM_WIN_VALUE_FOR_TIMER = new short[4] { 0, 5, 10, 15 };

	public static Color COLOR_DISABLE_COMBOBOX = new Color(0f, 0f, 0f, 1f);

	public static float[,] AVATAR_SCALE = new float[25, 5]
	{
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 1.1f },
		{ 1.1f, 1.1f, 0.9f, 1.1f, 0.85f },
		{ 1f, 1f, 1f, 1f, 1f }
	};

	public static float[,] BAG_OFFSET;

	public static float[] WEAPON_SCALE;

	public static float[,] WEAPON_OFFSET;

	public static float[] PRICE_SCALE;

	public static float[,] PRICE_OFFSET;

	public static Color fontColor_cyan;

	public static Color fontColor_black;

	public static Color fontColor_white;

	public static Color fontColor_gray;

	public static Color FONT_COLOR_WEAPON_LEVEL;

	public static Color FONT_COLOR_EQUIP_NAME;

	public static Color FONT_COLOR_DESCRIPTION;

	public static Color FONT_COLOR_DESCRIPTION_LOCK;

	public static Color FONT_COLOR_TIPS;

	public static Color FONT_COLOR_WIN_INFO_READY_GAME;

	public static Color COLOR_AIM;

	public static Color COLOR_COMBO;

	public static Color COLOR_COMBO_KILL;

	public static Color COLOR_COMBO_RED;

	public static Color COLOR_REBIRTH_TIMER;

	public static Color COLOR_ENERGY_HUD;

	public static Color COLOR_HP_HUD;

	public static Color COLOR_POWERUP;

	public static Color COLOR_STATISTICS_PLAYER_NAME;

	public static Color COLOR_MAINMENU_TITLE;

	public static Color COLOR_MANTIS_BOSS;

	public static Color COLOR_MANTIS_OTHER_BOSS;

	public static Color[] COLOR_PLAYER_ICONS;

	public static Color[] COLOR_TEAM_PLAYER_ICONS;

	public static Color[] COLOR_TEAM_VIP_PLAYER_ICONS;

	public static Color COLOR_WIN_VALUE;

	public static Color COLOR_PLAYER_SCORE;

	public static Color COLOR_POPULATION;

	public static string[] PROPERTY_NAME;

	public static string[] PROPERTY_POWER;

	public static Color[] PROPERTY_COLOR;

	public static string BAG_SLOT_STRING;

	public static string[] KEY_WORD;

	public static string[] PROPS_KEY_WORD;

	public static string KEEP_TIME_STRING;

	public static string UPGRADE_STRING;

	private static string[] mMessageDescriptions;

	private static byte initPhase;

	private static byte currentScene;

	static UIConstant()
	{
		float[,] array = new float[25, 3];
		array[15, 1] = 0.1f;
		array[16, 1] = 0.1f;
		array[17, 1] = 0.1f;
		array[18, 1] = 0.1f;
		array[19, 1] = 0.1f;
		array[20, 1] = 0.1f;
		array[21, 1] = 0.1f;
		array[22, 1] = 0.1f;
		array[23, 1] = 0.2f;
		array[24, 1] = 0.1f;
		BAG_OFFSET = array;
		WEAPON_SCALE = new float[47]
		{
			1.1f, 1.1f, 1.1f, 1.1f, 1.1f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f,
			1.2f, 0.85f, 0.85f, 0.85f, 1f, 1f, 1f, 1.2f, 1.2f, 1.2f,
			1.2f, 1.2f, 1f, 1.2f, 0.9f, 0.9f, 1.2f, 0.7f, 0.7f, 1f,
			0.85f, 1.1f, 1f, 0.75f, 1f, 0.9f, 1f, 0.9f, 0.9f, 0.9f,
			1.1f, 1f, 1f, 1f, 1f, 0.9f, 1f
		};
		WEAPON_OFFSET = new float[47, 2]
		{
			{ -0.1f, 0f },
			{ -0.1f, 0f },
			{ -0.1f, 0f },
			{ -0.1f, 0f },
			{ 0f, 0f },
			{ -0.15f, 0f },
			{ -0.15f, 0f },
			{ -0.15f, 0f },
			{ -0.15f, 0f },
			{ -0.15f, 0f },
			{ -0.15f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ -0.1f, 0f },
			{ -0.15f, 0f },
			{ 0f, 0f },
			{ 0f, -0.15f },
			{ 0f, -0.15f },
			{ 0f, 0f },
			{ 0.3f, 0.3f },
			{ 0.3f, -0.15f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0.2f, -0.15f },
			{ 0f, 0f },
			{ 0.15f, 0.35f },
			{ -0.2f, 0f },
			{ -0.2f, 0f },
			{ 0.07f, 0.2f },
			{ -0.2f, 0f },
			{ -0.2f, 0f },
			{ 0f, -0.15f },
			{ -0.1f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0f, 0f },
			{ 0.3f, 0f },
			{ 0.15f, 0f }
		};
		PRICE_SCALE = new float[47]
		{
			0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.02f, 0.025f, 0.02f, 0.02f, 0.02f,
			0.02f, 0.03f, 0.03f, 0.03f, 0.025f, 0.025f, 0.025f, 0.02f, 0.02f, 0.02f,
			0.02f, 0.02f, 0.02f, 0.02f, 0.03f, 0.03f, 0.02f, 0.045f, 0.04f, 0.025f,
			0.03f, 0.025f, 0.025f, 0.035f, 0.025f, 0.03f, 0.03f, 0.03f, 0.03f, 0.03f,
			0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f, 0.025f
		};
		float[,] array2 = new float[47, 3];
		array2[22, 0] = 0.35f;
		array2[24, 0] = 0.2f;
		array2[24, 1] = 0.4f;
		array2[25, 0] = 0.2f;
		array2[25, 1] = 0.4f;
		array2[27, 0] = -0.4f;
		array2[27, 1] = -0.5f;
		array2[28, 0] = -0.4f;
		array2[28, 1] = 0.25f;
		array2[31, 0] = -0.3f;
		array2[31, 1] = 0.2f;
		array2[33, 0] = -0.3f;
		array2[33, 1] = -0.6f;
		array2[34, 1] = 0.1f;
		array2[35, 0] = 0.2f;
		array2[35, 1] = 0.2f;
		array2[36, 0] = -0.1f;
		array2[36, 1] = -0.2f;
		array2[37, 0] = 0.2f;
		array2[37, 1] = 0.2f;
		array2[38, 0] = 0.2f;
		array2[38, 1] = 0.2f;
		array2[39, 0] = 0.2f;
		array2[39, 1] = 0.4f;
		array2[45, 0] = -0.2f;
		array2[46, 0] = -0.1f;
		PRICE_OFFSET = array2;
		fontColor_cyan = new Color(0.4f, 1f, 1f, 1f);
		fontColor_black = new Color(0.23529412f, 0.23529412f, 0.23529412f, 1f);
		fontColor_white = new Color(1f, 1f, 1f, 1f);
		fontColor_gray = new Color(0.23529412f, 0.23529412f, 0.23529412f, 1f);
		FONT_COLOR_WEAPON_LEVEL = new Color(1f, 1f, 1f, 1f);
		FONT_COLOR_EQUIP_NAME = new Color(0.12156863f, 0.6784314f, 0.7372549f, 1f);
		FONT_COLOR_DESCRIPTION = new Color(0.11372549f, 62f / 85f, 56f / 85f, 1f);
		FONT_COLOR_DESCRIPTION_LOCK = new Color(0.4f, 0.4f, 0.4f, 1f);
		FONT_COLOR_TIPS = new Color(1f, 1f, 1f, 1f);
		FONT_COLOR_WIN_INFO_READY_GAME = new Color(0.11372549f, 62f / 85f, 56f / 85f, 1f);
		COLOR_AIM = new Color(0f, 1f, 1f, 0.8f);
		COLOR_COMBO = new Color(1f, 0.8901961f, 0.57254905f, 1f);
		COLOR_COMBO_KILL = new Color(1f, 59f / 85f, 4f / 85f, 1f);
		COLOR_COMBO_RED = new Color(1f, 0f, 0f, 1f);
		COLOR_REBIRTH_TIMER = new Color(1f, 1f, 1f, 1f);
		COLOR_ENERGY_HUD = new Color(1f, 0.64705884f, 0f, 1f);
		COLOR_HP_HUD = new Color(0f, 0.6156863f, 1f, 1f);
		COLOR_POWERUP = new Color(1f, 1f, 1f, 1f);
		COLOR_STATISTICS_PLAYER_NAME = new Color(1f, 1f, 1f, 1f);
		COLOR_MAINMENU_TITLE = new Color(0.23529412f, 0.23529412f, 0.23529412f, 1f);
		COLOR_MANTIS_BOSS = new Color(0f, 1f, 1f, 1f);
		COLOR_MANTIS_OTHER_BOSS = new Color(1f, 26f / 51f, 26f / 51f, 1f);
		COLOR_PLAYER_ICONS = new Color[8]
		{
			new Color(1f, 0.2f, 0f, 1f),
			new Color(1f, 0.6f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(0.2f, 1f, 0f, 1f),
			new Color(0f, 1f, 1f, 1f),
			new Color(0f, 0.6f, 1f, 1f),
			new Color(0f, 0.2f, 1f, 1f),
			new Color(0.6f, 0f, 1f, 1f)
		};
		COLOR_TEAM_PLAYER_ICONS = new Color[2]
		{
			new Color(0f, 0.7019608f, 1f, 1f),
			new Color(1f, 0f, 0.5294118f, 1f)
		};
		COLOR_TEAM_VIP_PLAYER_ICONS = new Color[2]
		{
			Color.blue,
			Color.red
		};
		COLOR_WIN_VALUE = new Color(1f, 1f, 1f, 1f);
		COLOR_PLAYER_SCORE = new Color(1f, 0f, 0f, 1f);
		COLOR_POPULATION = new Color(0f, 0.7019608f, 1f, 1f);
		PROPERTY_NAME = new string[4] { "HP", "POW", "SPD", "GOLD" };
		PROPERTY_POWER = new string[47]
		{
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			" x3",
			" x3",
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			" x4",
			string.Empty
		};
		PROPERTY_COLOR = new Color[4]
		{
			new Color(0f, 53f / 85f, 1f, 1f),
			new Color(1f, 0.64705884f, 0f, 1f),
			new Color(2f / 51f, 0.5019608f, 0f, 1f),
			new Color(1f, 1f, 4f / 85f, 1f)
		};
		BAG_SLOT_STRING = "[BAG_SLOT]";
		KEY_WORD = new string[58]
		{
			"[HP_BOOTH]", "[ATTACK_BOOTH]", "[SPEED_BOOTH]", "[MONEY_BOOTH]", "[EXP_BOOTH]", "[SAVE_ENEGY]", "[RECOVERY_BOOTH]", "[HP_AUTO_RECOVERY]", "[DAMAGE_REDUCE]", "[HP_RECOVERY_WHEN_MAKE_KILL]",
			"[BLOCK_AT_A_RATE]", "[TEAM_HP_RECOVERY]", "[TEAM_ATTACK_BOOTH]", "[ASSAULT_BOOTH]", "[SHOTGUN_BOOTH]", "[RPG_BOOTH]", "[GRENADE_BOOTH]", "[LASER_BOOTH]", "[LASER_CANNON_BOOTH]", "[PLASMA_BOOTH]",
			"[MACHINE_BOOTH]", "[BOW_BOOTH]", "[IMPULSE_BOOTH]", "[GLOVE_BOOTH]", "[POWER_UP]", "[SPEED_UP]", "[UNLIMITED_ENEGY]", "[FLY]", "[TEAM_DAMAGE_REDUCE]", "[SWORD_BOOTH]",
			"[DEFENCE_UP]", "[ANDROMEDA_UP]", "[SNIPER_BOOTH]", "[ASSAULT_DEFENCE]", "[SHOTGUN_DEFENCE]", "[RPG_DEFENCE]", "[GRENADE_DEFENCE]", "[LASER_DEFENCE]", "[LASER_CANNON_DEFENCE]", "[PLASMA_DEFENCE]",
			"[MACHINE_DEFENCE]", "[BOW_DEFENCE]", "[IMPULSE_DEFENCE]", "[GLOVE_DEFENCE]", "[SWORD_DEFENCE]", "[SNIPER_DEFENCE]", "[SPEEDUP_WHEN_GOT_HIT]", "[HEALTH_STEAL]", "[ATTACK_SHIELD]", "[TRACKINGGUN_BOOTH]",
			"[PINGPONG_BOOTH]", "[TRACKINGGUN_DEFENCE]", "[PINGPONG_DEFENCE]", "[ATTACK_FRENQUENCY]", "[IMPACT_WAVE]", "[BAG_SLOT]", "[HURT_HEALTH]", "[GRAVITY_FORCE]"
		};
		PROPS_KEY_WORD = new string[10] { "[HP_BOOTH]", "[ATTACK_BOOTH]", "[SPEED_BOOTH]", "[REVIVAL_BOOTH]", "[CALL_BOOTH]", "[DAMAGE_BOOTH]", "[DAMAGE_REDUCE]", "[HP_RECOVERY]", "[SPEED_BOOTH_WHEN_GOT_HIT]", "[ATTACK_BOOTH_WHEN_SECURE_FLAG]" };
		KEEP_TIME_STRING = "[KEEP_TIME]";
		UPGRADE_STRING = "[UPGRADE]";
	}

	public static string FormatNum(int val)
	{
		return string.Format("{0:N0}", val);
	}

	public static void GotoShopAndCustomize(byte curScene, byte phase)
	{
		currentScene = curScene;
		initPhase = phase;
	}

	public static byte GetInitPhase()
	{
		return initPhase;
	}

	public static byte GetCurrentScene()
	{
		return currentScene;
	}

	public static void ExitShopAndCustomize()
	{
		switch (currentScene)
		{
		case 0:
			Application.LoadLevel("StartMenu");
			break;
		case 1:
			Application.LoadLevel("SoloMenu");
			break;
		case 2:
			Application.LoadLevel("MultiMenu");
			break;
		}
	}

	public static void InitMessage()
	{
		string[] gameText = Res2DManager.GetInstance().GetGameText();
		if (gameText.Length > 4)
		{
			mMessageDescriptions = Res2DManager.GetInstance().SplitString(gameText[4]);
		}
	}

	public static string GetMessage(int index)
	{
		return mMessageDescriptions[index];
	}

	public static void InitScreenInfo()
	{
		ScreenLocalWidth = 960f;
		ScreenLocalHeight = 640f;
		float num = (float)Screen.width / (float)Screen.height - ScreenLocalWidth / ScreenLocalHeight;
		if (num > 0f)
		{
			ScreenAdaptived.y = (ScreenAdaptived.x = (float)Screen.height / ScreenLocalHeight);
			return;
		}
		if (num < 0f)
		{
			ScreenAdaptived.y = (ScreenAdaptived.x = (float)Screen.width / ScreenLocalWidth);
			return;
		}
		ScreenAdaptived.x = (float)Screen.width / ScreenLocalWidth;
		ScreenAdaptived.y = (float)Screen.height / ScreenLocalHeight;
	}

	public static Rect GetRectForScreenAdaptived(Rect rct)
	{
		int num = (int)((double)ScreenLocalWidth * 0.5);
		int num2 = (int)((double)ScreenLocalHeight * 0.5);
		int num3 = (int)((double)Screen.width * 0.5 + (double)((rct.xMin - (float)num) * ScreenAdaptived.x));
		int num4 = (int)((double)Screen.width * 0.5 + (double)((rct.xMax - (float)num) * ScreenAdaptived.x));
		int num5 = (int)((double)Screen.height * 0.5 + (double)((rct.yMin - (float)num2) * ScreenAdaptived.y));
		int num6 = (int)((double)Screen.height * 0.5 + (double)((rct.yMax - (float)num2) * ScreenAdaptived.y));
		return new Rect(num3, num5, num4 - num3, num6 - num5);
	}

	public static int GetWidthForScreenAdaptived(int v)
	{
		return (int)((float)v * ScreenAdaptived.x);
	}

	public static int GetHeightForScreenAdaptived(int v)
	{
		return (int)((float)v * ScreenAdaptived.y);
	}

	public static Vector2 GetPosForScreenAdaptived(Vector2 v)
	{
		int num = (int)((double)ScreenLocalWidth * 0.5);
		int num2 = (int)((double)ScreenLocalHeight * 0.5);
		int num3 = (int)((double)Screen.width * 0.5 + (double)((v.x - (float)num) * ScreenAdaptived.x));
		int num4 = (int)((double)Screen.height * 0.5 + (double)((v.y - (float)num2) * ScreenAdaptived.y));
		return new Vector2(num3, num4);
	}

	public static Vector2 GetReversePosForScreenAdaptived(Vector2 v)
	{
		int num = (int)((double)Screen.width * 0.5);
		int num2 = (int)((double)Screen.height * 0.5);
		int num3 = (int)((double)ScreenLocalWidth * 0.5 + (double)((v.x - (float)num) * (1f / ScreenAdaptived.x)));
		int num4 = (int)((double)ScreenLocalHeight * 0.5 + (double)((v.y - (float)num2) * (1f / ScreenAdaptived.y)));
		return new Vector2(num3, num4);
	}

	public static Rect GetRectForScreenAdaptived2(Rect rct)
	{
		float num = ScreenLocalWidth * 0.5f;
		float left = ((float)(Screen.width / 2) - (num - rct.x * ScreenLocalWidth) * ScreenAdaptived.x) / (float)Screen.width;
		float width = rct.width * ScreenLocalWidth * ScreenAdaptived.x / (float)Screen.width;
		return new Rect(left, rct.y, width, rct.height);
	}

	public static string GetRankName(int exp)
	{
		string result = null;
		switch (exp)
		{
		case 0:
			result = "Private";
			break;
		case 1:
			result = "Private E-2(PV2)";
			break;
		case 2:
			result = "Private FirstClass(PFC)";
			break;
		case 3:
			result = "Corporal(CPL)";
			break;
		case 4:
			result = "Sergeant(SGT)";
			break;
		case 5:
			result = "Staff Sergeant(SSG)";
			break;
		case 6:
			result = "Sergeant First Class(SFC)";
			break;
		case 7:
			result = "Master Sergeant(MSG)";
			break;
		case 8:
			result = "Sergeant Major(SGM)";
			break;
		case 9:
			result = "Command Sergeant Major";
			break;
		case 10:
			result = "Sergeant Major of the Army";
			break;
		case 11:
			result = "Second Lieutenant";
			break;
		}
		return result;
	}

	public static List<Weapon> GetRankWeapon(int rank)
	{
		string text = ((rank >= 0 && rank <= 3) ? "2,5,8,10,23,28" : ((rank != 4 && rank != 5) ? "13,21,23,26,28,29,30,35,36,38" : "13,16,21,23,28,29,30,33,35,38"));
		string[] array = text.Split(',');
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < array.Length; i++)
		{
			arrayList.Add(int.Parse(array[i]));
		}
		List<Weapon> list = new List<Weapon>();
		List<Weapon> weapons = GameApp.GetInstance().GetUserState().GetWeapons();
		for (int j = 0; j < weapons.Count; j++)
		{
			if (arrayList.Contains(weapons[j].GunID))
			{
				list.Add(weapons[j]);
			}
		}
		return list;
	}

	public static int GetRankNeedTime(int rank)
	{
		if (rank >= 0 && rank <= 3)
		{
			return 3600;
		}
		switch (rank)
		{
		case 4:
		case 5:
			return 14400;
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
			return 43200;
		default:
			return 0;
		}
	}

	public static int GetRankNeedMithril(int rank)
	{
		if (rank >= 0 && rank <= 3)
		{
			return 10;
		}
		switch (rank)
		{
		case 4:
		case 5:
			return 85;
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
			return 100;
		default:
			return 0;
		}
	}

	public static float GetRankDisCount(int rank)
	{
		if (rank >= 0 && rank <= 3)
		{
			return 0.5f;
		}
		switch (rank)
		{
		case 4:
		case 5:
			return 0.75f;
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
			return 0.75f;
		default:
			return 0f;
		}
	}

	public static bool Is16By9()
	{
		return Screen.width == 1136 || Screen.width == 1334 || Screen.width == 1920;
	}
}
