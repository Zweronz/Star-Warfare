using UnityEngine;

public class DownloadObbExample : MonoBehaviour
{
	private static string expPath;

	private static bool downloadButton;

	private static string errorMsg = string.Empty;

	private void Start()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			errorMsg = "Use GooglePlayDownloader only on Android device!";
			return;
		}
		expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (expPath == null)
		{
			errorMsg = "External storage is not available!";
			return;
		}
		string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expPath);
		if (mainOBBPath == null)
		{
			downloadButton = true;
		}
	}

	private void Update()
	{
		if (expPath != null)
		{
			string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			if (mainOBBPath != null)
			{
				Application.LoadLevel("StartMenu");
			}
		}
	}

	private void OnGUI()
	{
		if (!string.IsNullOrEmpty(errorMsg))
		{
			GUI.Label(new Rect(10f, Screen.height - 50, Screen.width - 10, 50f), errorMsg);
		}
		if (downloadButton && GUI.Button(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 150, 600f, 300f), "Fetch OBBs"))
		{
			GooglePlayDownloader.FetchOBB();
		}
	}
}
