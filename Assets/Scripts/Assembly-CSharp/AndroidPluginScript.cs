using UnityEngine;

public class AndroidPluginScript
{
	public static int GetLanguage()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("GetLanguage", new object[0]);
		}
		return 6;
	}

	public static string GetMacAddress()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetMacAddress", new object[0]);
		}
		return string.Empty;
	}

	public static string GetAndroidId()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetAndroidId", new object[0]);
		}
		return "freyrtest1";
	}

	public static void ShowFreyrGames(string bundleId, int type)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("ShowFreyrGames", bundleId, type);
		}
		else
		{
			Debug.Log("ShowFreyrGames = " + bundleId);
		}
	}

	public static void DoStartMethod()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("DoStartMethod");
		}
		else
		{
			Debug.Log("DoStartAdsMethod");
		}
	}

	public static int GetRandomCount()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("GetRandomCount", new object[0]);
		}
		return 3;
	}

	public static void isStart()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("isStart");
		}
		else
		{
			Debug.Log("isStart");
		}
	}

	public static void ShowToast(string str)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("ShowToast", str);
		}
		else
		{
			Debug.Log("ShowToast " + str);
		}
	}

	public static string GetUDID(string str)
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetUDID", new object[1] { str });
		}
		Debug.Log("send Eclipse url = " + str);
		return string.Empty;
	}

	public static string GetCountry()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetCountry", new object[0]);
		}
		return Application.systemLanguage.ToString();
	}

	public static string GetAdvisterID()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetAdvisterID", new object[0]);
		}
		return string.Empty;
	}

	public static void isGoogleServiceReady()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("isGoogleServiceReady");
		}
		else
		{
			Debug.Log("isGoogleServiceReady");
		}
	}

	public static void CloseGame()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CloseGame");
		}
		else
		{
			Debug.Log("CloseGame");
		}
	}

	public static void DoStart()
	{
	}

	public static void OpenURL()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("OpenURL");
		}
		else
		{
			Debug.Log("OpenURL");
		}
	}

	public static int GetVersionType()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("GetVersionType", new object[0]);
		}
		return 0;
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}

	public static int GetRandom(int randomMax)
	{
		int num = Random.Range(0, randomMax);
		Debug.Log("Random Count = " + num);
		return num;
	}
}
