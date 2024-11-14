using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialNetworkingGUIManagerTwo : MonoBehaviour
{
	private void Start()
	{
		SocialNetworkingManager.facebookReceivedFriends += delegate(ArrayList result)
		{
			ResultLogger.logArraylist(result);
		};
		SocialNetworkingManager.facebookReceivedCustomRequest += delegate(object result)
		{
			ResultLogger.logObject(result);
		};
	}

	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float num2 = ((Screen.width < 960 && Screen.height < 960) ? 160 : 320);
		float num3 = ((Screen.width < 960 && Screen.height < 960) ? 30 : 70);
		float num4 = num3 + 10f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Post Message"))
		{
			FacebookBinding.postMessage("im posting this from Unity: " + Time.deltaTime);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Message & More"))
		{
			FacebookBinding.postMessageWithLinkAndLinkToImage("link post from Unity: " + Time.deltaTime, "http://prime31.com", "Prime31 Studios", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get Friends"))
		{
			FacebookBinding.getFriends();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Dialog With Options"))
		{
			FacebookBinding.showPostMessageDialogWithOptions("http://prime31.com", "Prime31 Studios", string.Empty, string.Empty);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom Feed Dialog"))
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("link", "http://prime31.com");
			hashtable.Add("picture", "http://prime31.com/assets/images/prime31logo.png");
			hashtable.Add("name", "Name of the link");
			hashtable.Add("caption", "Im the prime31 logo");
			hashtable.Add("description", "some text telling what this is all about");
			FacebookBinding.showPostMessageDialogWithOptions(hashtable);
		}
		if (GUI.Button(new Rect(left, num += num4 * 2f, num2, num3), "Back"))
		{
			Application.LoadLevel("SocialNetworkingtestScene");
		}
		left = (float)Screen.width - num2 - 5f;
		num = 5f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Graph Request (me)"))
		{
			FacebookBinding.graphRequest("me", "GET", new Hashtable());
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom Graph Request"))
		{
			FacebookBinding.graphRequest("platform/posts", "GET", new Hashtable());
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom REST Request"))
		{
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("query", "SELECT uid,name FROM user WHERE uid=4");
			FacebookBinding.restRequest("fql.query", "POST", hashtable2);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom Dialog"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("message", "Check out this great app!");
			Dictionary<string, string> options = dictionary;
			FacebookBinding.showDialog("apprequests", options);
		}
	}
}
