using UnityEngine;

public class AdMobGUIManager : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float num2 = ((Screen.width < 960 && Screen.height < 960) ? 160 : 320);
		float num3 = ((Screen.width < 960 && Screen.height < 960) ? 40 : 80);
		float num4 = num3 + 10f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Initialize AdMob"))
		{
			AdMobBinding.init("YOUR_PUBLISHER_ID");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Destroy Banner"))
		{
			AdMobBinding.destroyBanner();
		}
		num += num4;
		if (iPhoneSettings.generation != iPhoneGeneration.iPad1Gen)
		{
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "320x50 Banner"))
			{
				AdMobBinding.createBanner(AdMobBannerType.iPhone_320x50, 0f, 0f);
			}
		}
		else
		{
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "320x250 Banner"))
			{
				AdMobBinding.createBanner(AdMobBannerType.iPad_320x250, (Screen.width - 320) / 2, 0f);
			}
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "468x60 Banner"))
			{
				AdMobBinding.createBanner(AdMobBannerType.iPad_468x60, (Screen.width - 488) / 2, 0f);
			}
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "728x90 Banner"))
			{
				AdMobBinding.createBanner(AdMobBannerType.iPad_728x90, (Screen.width - 748) / 2, 0f);
			}
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Report App Download"))
		{
			AdMobBinding.registerAppDownloadWithiTunesAppId("ITUNES_APP_ID");
			AdMobBinding.registerAppDownloadWithAdMobSiteId();
		}
		left = (float)Screen.width - num2 - 5f;
		num = 5f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Force Portrait Banner"))
		{
			AdMobBinding.rotateToOrientation(DeviceOrientation.Portrait);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Force LandcapeLeft Banner"))
		{
			AdMobBinding.rotateToOrientation(DeviceOrientation.LandscapeLeft);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Force LandcapeRight Banner"))
		{
			AdMobBinding.rotateToOrientation(DeviceOrientation.LandscapeRight);
		}
		if (GUI.Button(new Rect(left, num += num4 + 20f, num2, num3), "Request Interstitial"))
		{
			AdMobBinding.requestInterstitalAd("a14d3e67dfeb7ba");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Is Interstial Loaded?"))
		{
			Debug.Log("is interstitial loaded: " + AdMobBinding.isInterstitialAdReady());
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Show Interstitial"))
		{
			AdMobBinding.showInterstitialAd();
		}
	}
}
