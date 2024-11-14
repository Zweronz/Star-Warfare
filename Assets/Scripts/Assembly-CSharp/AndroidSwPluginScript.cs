using UnityEngine;

public class AndroidSwPluginScript
{
	public static string GetVersionUrl()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetVersionUrl", new object[0]);
		}
		Debug.Log("GetVersionUrl");
		return string.Empty;
	}

	public static void SetRoleName(int type)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("SetRoleName", type);
		}
		else
		{
			Debug.Log("SetRoleName");
		}
	}

	public static void SendFreyrAdsStatus(byte status)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("SendFreyrAdsStatus", status);
		}
		else
		{
			Debug.Log("SendFreyrAdsStatus");
		}
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}
