using System.Runtime.InteropServices;

public class FlurryScript
{
	[DllImport("__Internal")]
	protected static extern void _InitAds();

	public static void InitAds()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _PlayFlurryVideo();

	public static void PlayFlurryVideo()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _Tapjoy();

	public static void Tapjoy()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _MoreGame();

	public static void MoreGame()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _PlayAdcolonyVideo();

	public static void PlayAdcolonyVideo()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _PlayAppLovin();

	public static void PlayAppLovin()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _PlayAppLovinVideo();

	public static void PlayAppLovinVideo()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendAppStatus(byte status);

	public static void SendAppStatus(byte status)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _SendAdScale(byte scale);

	public static void SendAdScale(byte scale)
	{
	}

	[DllImport("__Internal")]
	protected static extern void _ShowNotification();

	public static void ShowNotification()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _CloseNotification();

	public static void CloseNotification()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _PairBladePad();

	public static void PairBladePad()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _PlayChartBoost();

	public static void PlayChartBoost()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _InitChartBoost();

	public static void InitChartBoost()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _Fyber();

	public static void PlayFyber()
	{
	}

	[DllImport("__Internal")]
	protected static extern void _CheckAdsRewards();

	public static void CheckAdsRewards()
	{
	}

	[DllImport("__Internal")]
	protected static extern string _GetCFUUID();

	public static string GetCFUUID()
	{
		return string.Empty;
	}

	[DllImport("__Internal")]
	protected static extern void _PlayNativeX();

	public static void PlayNativeX()
	{
	}
}
