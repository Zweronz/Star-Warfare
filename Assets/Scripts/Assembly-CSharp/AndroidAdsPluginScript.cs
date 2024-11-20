using UnityEngine;

public class AndroidAdsPluginScript
{
	public static void CallFlurryAds()
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("CallFlurryAds");
		}
	}

	public static void CallTapjoyOfferWall()
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("CallTapjoyOfferWall");
		}
	}

	public static void GetTapJoyPoints()
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("GetTapJoyPoints");
		}
	}

	public static void CallSponsorPayOfferWall()
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("CallSponsorPayOfferWall");
		}
	}

	public static void CallAdcolonyVideo()
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("CallAdcolonyVideo");
		}
	}

	public static void CallChartboostAds()
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("ChartboostAds");
		}
	}

	public static void InitAds(string userID)
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("InitAds :" + userID);
		}
	}

	public static bool OpenAds()
	{
		return true;
	}

	public static void ShowTapjoyDialog(int num)
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("ShowTapjoyDialog");
		}
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}
