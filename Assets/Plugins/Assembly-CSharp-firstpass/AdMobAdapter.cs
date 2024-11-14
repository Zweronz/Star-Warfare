using UnityEngine;

public class AdMobAdapter : MonoBehaviour
{
	public bool autorotateAds = true;

	public bool isLandscape = true;

	public string publisherId = "a14d3e67dfeb7ba";

	private void Start()
	{
		AdMobBinding.init(publisherId);
		if (isLandscape)
		{
			if (iPhoneSettings.generation == iPhoneGeneration.iPad1Gen)
			{
				AdMobBinding.createBanner(AdMobBannerType.iPad_728x90, (Screen.width - 728) / 2, 0f);
			}
			else
			{
				AdMobBinding.createBanner(AdMobBannerType.iPhone_320x50, (Screen.width - 320) / 2, 0f);
			}
			AdMobBinding.rotateToOrientation(DeviceOrientation.LandscapeLeft);
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		else if (iPhoneSettings.generation == iPhoneGeneration.iPad1Gen)
		{
			AdMobBinding.createBanner(AdMobBannerType.iPad_728x90, 0f, 0f);
		}
		else
		{
			AdMobBinding.createBanner(AdMobBannerType.iPhone_320x50, 0f, 0f);
		}
		if (!autorotateAds)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (Screen.orientation == (ScreenOrientation)Input.deviceOrientation)
		{
			return;
		}
		if (isLandscape)
		{
			if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
			{
				AdMobBinding.rotateToOrientation(Input.deviceOrientation);
				Screen.orientation = (ScreenOrientation)Input.deviceOrientation;
			}
		}
		else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
		{
			AdMobBinding.rotateToOrientation(Input.deviceOrientation);
			Screen.orientation = (ScreenOrientation)Input.deviceOrientation;
		}
	}
}
