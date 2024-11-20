using UnityEngine;

public class AndroidPluginScript
{
	public static int GetLanguage()
	{
		return 6;
	}

	public static string GetMacAddress()
	{
		return System.Environment.MachineName;
	}

	public static string GetAndroidId()
	{
		return System.Environment.UserName;
	}

	public static void ShowFreyrGames(string bundleId, int type)
	{
		if (!IsPC())
		{
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
		}
		else
		{
			Debug.Log("DoStartAdsMethod");
		}
	}

	public static int GetRandomCount()
	{
		return 3;
	}

	public static void isStart()
	{
		if (!IsPC())
		{
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
		}
		else
		{
			Debug.Log("ShowToast " + str);
		}
	}

	public static string GetUDID(string str)
	{
		return string.Empty;
	}

	public static string GetCountry()
	{
		return Application.systemLanguage.ToString();
	}

	public static string GetAdvisterID()
	{
		return string.Empty;
	}

	public static void isGoogleServiceReady()
	{
		if (!IsPC())
		{
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
		}
		else
		{
			Debug.Log("OpenURL");
		}
	}

	public static int GetVersionType()
	{
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
