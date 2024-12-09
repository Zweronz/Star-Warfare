using UnityEngine;

public class AndroidSwPluginScript
{
	public static string GetVersionUrl()
	{
		return "https://github.com/Zweronz/Star-Warfare";
	}

	public static void SetRoleName(int type)
	{
		if (!IsPC())
		{
			if (KeyboardListener.current != null)
			{
				Object.Destroy(KeyboardListener.current.gameObject);
			}
	
			TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(GameApp.GetInstance().GetUserState().GetRoleName());

			KeyboardListener listener = KeyboardListener.GetOrCreate((name, succes) =>
            {
                if (!succes) return;
                GameApp.GetInstance().GetUserState().SetRoleName(name);
				GameApp.GetInstance().isChangeName = true;
			});

			listener.keyboard = keyboard;
		}
		else
		{
			KeyboardListener listener = KeyboardListener.GetOrCreate((name, succes) =>
			{
				if (!succes) return;
				GameApp.GetInstance().GetUserState().SetRoleName(name);
				GameApp.GetInstance().isChangeName = true;
			});

			listener.pcString = GameApp.GetInstance().GetUserState().GetRoleName();
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
