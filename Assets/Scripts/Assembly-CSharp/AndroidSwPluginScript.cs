using UnityEngine;

public class AndroidSwPluginScript
{
	public static string GetVersionUrl()
	{
		return "https://github.com/Zweronz/Star-Warfare";
	}

	public static void SetRoleName(int type)
	{
		if (type == 1)
		{
			if (!IsPC())
			{
				if (KeyboardListener.current != null)
				{
					Object.Destroy(KeyboardListener.current.gameObject);
				}
	
				TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(GameApp.GetInstance().GetUserState().GetRoleName());

				KeyboardListener listener = KeyboardListener.CreateNew();
				listener.keyboard = keyboard;
	
				listener.onFinish = GameApp.GetInstance().GetUserState().SetRoleName;
			}
			else
			{
				KeyboardListener listener = KeyboardListener.CreateNew();

				listener.pcString = GameApp.GetInstance().GetUserState().GetRoleName();
	
				listener.onFinish = GameApp.GetInstance().GetUserState().SetRoleName;
			}
		}
	}

	public static void SendFreyrAdsStatus(byte status)
	{
		if (!IsPC())
		{
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
