public class AndroidConstant
{
	public enum Version
	{
		GooglePlay = 0,
		Kindle = 1
	}

	public static string BUNDLEID_CALLOFARENA_KINDLE = "com.ifreyr.spartacus";

	public static string BUNDLEID_STARWARFARE_KINDLE = "com.ifreyrgames.starwarfareamazon";

	public static string BUNDLEID_BLACKDAWN_KINDLE = "com.ifreyrgames.blackdawnamazon";

	public static string BUNDLEID_CALLOFARENA = "com.ifreyr.spartacus";

	public static string BUNDLEID_STARWARFARE = "com.ifreyrgames.starwarfare";

	public static string BUNDLEID_BLACKDAWN = "com.ifreyrgames.blackdawn";

	public static string BUNDLEID_LORDOFZOMBIES = "com.ifreyr.zombies";

	public static string URL_CALLOFARENA_GOOGLEPLAY = "https://play.google.com/store/apps/details?id=com.ifreyr.spartacus";

	public static string URL_STARWARFARE_GOOGLEPLAY = "https://play.google.com/store/apps/details?id=com.ifreyrgames.starwarfarehd";

	public static string URL_BLACKDAWN_GOOGLEPLAY = "https://play.google.com/store/apps/details?id=com.ifreyrgames.blackdawnamazon";

	public static string URL_LORDOFZOMBIES_GOOGLEPLAY = "https://play.google.com/store/apps/details?id=com.ifreyr.zombies";

	public static int SHOW_START_DIALOG_START = 0;

	public static int SHOW_CLOSE_DIALOG_START = 1;

	public static Version version = (Version)AndroidPluginScript.GetVersionType();
}
