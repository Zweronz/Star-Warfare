using UnityEngine;

public class AndroidAWSPluginScript
{
	public static bool IsLoginKindle()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<bool>("IsLoginKindle", new object[0]);
		}
		Debug.Log("IsLoginKindle = false");
		return false;
	}

	public static void OpenAwsUrl(string id)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("OpenAwsUrl", id);
		}
		else
		{
			Debug.Log("OpenAwsUrl = " + id);
		}
	}

	public static string GetCurrentPlayerName()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<string>("GetCurrentPlayerName", new object[0]);
		}
		Debug.Log("GetCurrentPlayerName");
		return string.Empty;
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}
