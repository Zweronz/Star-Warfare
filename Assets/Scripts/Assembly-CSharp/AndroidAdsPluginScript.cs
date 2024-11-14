using UnityEngine;

public class AndroidAdsPluginScript
{
	public static void CallFlurryAds()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallFlurryAds");
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
			CurrentActivity.getInstance().JavaObject.Call("CallTapjoyOfferWall");
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
			CurrentActivity.getInstance().JavaObject.Call("GetTapJoyPoints");
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
			CurrentActivity.getInstance().JavaObject.Call("CallSponsorPayOfferWall");
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
			CurrentActivity.getInstance().JavaObject.Call("CallAdcolonyVideo");
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
			CurrentActivity.getInstance().JavaObject.Call("ChartboostAds");
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
			CurrentActivity.getInstance().JavaObject.Call("InitAds", userID);
		}
		else
		{
			Debug.Log("InitAds :" + userID);
		}
	}

	public static bool OpenAds()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<bool>("OpenAds", new object[0]);
		}
		Debug.Log("OpenAds");
		return true;
	}

	public static void ShowTapjoyDialog(int num)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("ShowTapjoyDialog", num);
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
