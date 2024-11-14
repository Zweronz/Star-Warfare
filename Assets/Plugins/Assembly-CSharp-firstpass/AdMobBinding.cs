using System.Runtime.InteropServices;
using UnityEngine;

public class AdMobBinding
{
	[DllImport("__Internal")]
	private static extern void _adMobInit(string publisherId);

	public static void init(string publisherId)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobInit(publisherId);
		}
	}

	[DllImport("__Internal")]
	private static extern void _adMobCreateBanner(int bannerType, float xPos, float yPos);

	public static void createBanner(AdMobBannerType bannerType, float xPos, float yPos)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobCreateBanner((int)bannerType, xPos, yPos);
		}
	}

	[DllImport("__Internal")]
	private static extern void _adMobDestroyBanner();

	public static void destroyBanner()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobDestroyBanner();
		}
	}

	[DllImport("__Internal")]
	private static extern void _adMobRotateToOrientation(int orientation);

	public static void rotateToOrientation(DeviceOrientation orientation)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobRotateToOrientation((int)orientation);
		}
	}

	[DllImport("__Internal")]
	private static extern void _adMobRequestInterstitalAd(string interstitialUnitId);

	public static void requestInterstitalAd(string interstitialUnitId)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobRequestInterstitalAd(interstitialUnitId);
		}
	}

	[DllImport("__Internal")]
	private static extern bool _adMobIsInterstitialAdReady();

	public static bool isInterstitialAdReady()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _adMobIsInterstitialAdReady();
		}
		return false;
	}

	[DllImport("__Internal")]
	private static extern void _adMobShowInterstitialAd();

	public static void showInterstitialAd()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobShowInterstitialAd();
		}
	}

	[DllImport("__Internal")]
	private static extern void _adMobRegisterAppDownloadWithiTunesAppId(string iTunesAppId);

	public static void registerAppDownloadWithiTunesAppId(string iTunesAppId)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobRegisterAppDownloadWithiTunesAppId(iTunesAppId);
		}
	}

	[DllImport("__Internal")]
	private static extern void _adMobRegisterAppDownloadWithAdMobSiteId();

	public static void registerAppDownloadWithAdMobSiteId()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_adMobRegisterAppDownloadWithAdMobSiteId();
		}
	}
}
