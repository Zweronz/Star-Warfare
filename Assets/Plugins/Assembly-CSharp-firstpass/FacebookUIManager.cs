using System.Collections.Generic;
using UnityEngine;

public class FacebookUIManager : MonoBehaviour
{
	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		else
		{
			ResultLogger.logObject(result);
		}
	}

	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float num2 = ((Screen.width < 800 && Screen.height < 800) ? 160 : 320);
		float num3 = ((Screen.width < 800 && Screen.height < 800) ? 30 : 70);
		float num4 = num3 + 10f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Initialize Facebook"))
		{
			FacebookAndroid.init("YOUR_APP_ID");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Login"))
		{
			FacebookAndroid.loginWithRequestedPermissions(new string[3] { "publish_stream", "email", "user_birthday" });
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get Session Token"))
		{
			string accessToken = FacebookAndroid.getAccessToken();
			Debug.Log("session token: " + accessToken);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Is Session Valid?"))
		{
			bool flag = FacebookAndroid.isSessionValid();
			Debug.Log("Is session valid?: " + flag);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Graph Request"))
		{
			Facebook.instance.graphRequest("me", completionHandler);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Twitter Scene"))
		{
			Application.LoadLevel("TwitterTestScene");
		}
		left = (float)Screen.width - num2 - 5f;
		num = 5f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Logout"))
		{
			FacebookAndroid.logout();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Show Post Dialog"))
		{
			FacebookAndroid.showPostMessageDialog();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Show Post Dialog++"))
		{
			FacebookAndroid.showPostMessageDialogWithOptions("http://prime31.com", "prime31 studios", "http://prime31.com/assets/images/banners/tweetsBannerLogo.png", "image caption here");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Image"))
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
			JPGEncoder jPGEncoder = new JPGEncoder(texture2D, 75f);
			jPGEncoder.doEncoding();
			byte[] bytes = jPGEncoder.GetBytes();
			Object.Destroy(texture2D);
			Facebook.instance.postImage(bytes, "im an image posted from Android", completionHandler);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom Dialog"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("message", "check out my cool app");
			FacebookAndroid.showDialog("apprequests", dictionary);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Rest Request"))
		{
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2.Add("query", "SELECT uid,name FROM user WHERE uid=4");
			FacebookAndroid.restRequest("fql.query", "POST", dictionary2);
		}
	}
}
