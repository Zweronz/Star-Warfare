using UnityEngine;

public class AndroidAWSPluginScript
{
	public static bool IsLoginKindle()
	{
		return false;
	}

	public static void OpenAwsUrl(string id)
	{
		Application.OpenURL(id);
	}

	public static string GetCurrentPlayerName()
	{
		return string.Empty;
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}
