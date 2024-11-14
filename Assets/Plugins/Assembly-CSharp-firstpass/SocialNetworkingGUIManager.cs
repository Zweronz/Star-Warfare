using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialNetworkingGUIManager : MonoBehaviour
{
	public bool useTweetSheet;

	private string screenshotFilename = "someScreeny.png";

	private void Start()
	{
		SocialNetworkingManager.twitterHomeTimelineReceived += delegate(ArrayList result)
		{
			ResultLogger.logArraylist(result);
		};
		SocialNetworkingManager.facebookReceivedCustomRequest += delegate(object result)
		{
			ResultLogger.logObject(result);
		};
		Application.CaptureScreenshot(screenshotFilename);
	}

	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float num2 = ((Screen.width < 960 && Screen.height < 960) ? 160 : 320);
		float num3 = ((Screen.width < 960 && Screen.height < 960) ? 30 : 70);
		float num4 = num3 + 10f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Initialize"))
		{
			FacebookBinding.init("YOUR_APP_ID_HERE");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Is Logged In?"))
		{
			bool flag = FacebookBinding.isLoggedIn();
			Debug.Log("Facebook is logged in: " + flag);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Login"))
		{
			FacebookBinding.login();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Logout"))
		{
			FacebookBinding.logout();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get User's Name"))
		{
			FacebookBinding.getLoggedinUsersName();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Image"))
		{
			string pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
			FacebookBinding.postImage(pathToImage, "im an image posted from iOS");
		}
		if (GUI.Button(new Rect(left, num += num4 * 2f, num2, num3), "More Facebook..."))
		{
			Application.LoadLevel("SocialNetworkingtestSceneTwo");
		}
		left = (float)Screen.width - num2 - 5f;
		num = 5f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Initialize"))
		{
			TwitterBinding.init("INSERT_YOUR_INFO_HERE", "INSERT_YOUR_INFO_HERE");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Is Logged In?"))
		{
			bool flag2 = TwitterBinding.isLoggedIn();
			Debug.Log("Twitter is logged in: " + flag2);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Logged in Username"))
		{
			string text = TwitterBinding.loggedInUsername();
			Debug.Log("Twitter username: " + text);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Login with Oauth"))
		{
			TwitterBinding.showOauthLoginDialog();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Logout"))
		{
			TwitterBinding.logout();
		}
		if (!useTweetSheet)
		{
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Status Update"))
			{
				TwitterBinding.postStatusUpdate("im posting this from Unity: " + Time.deltaTime);
			}
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Status Update + Image"))
			{
				string pathToImage2 = Application.persistentDataPath + "/" + screenshotFilename;
				TwitterBinding.postStatusUpdate("I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage2);
			}
		}
		else
		{
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "Can User Tweet?"))
			{
				Debug.Log("Can the user tweet using the tweet sheet? " + TwitterBinding.canUserTweet());
			}
			if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Status Update + Image"))
			{
				string pathToImage3 = Application.persistentDataPath + "/" + screenshotFilename;
				TwitterBinding.showTweetComposer("I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage3);
			}
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom Request"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("status", "word up with a boogie boogie update");
			TwitterBinding.performRequest("POST", "/statuses/update.json", dictionary);
		}
	}
}
