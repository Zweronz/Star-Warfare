using System.Collections.Generic;
using UnityEngine;

public class TwitterUIManager : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float num2 = ((Screen.width < 800 && Screen.height < 800) ? 160 : 320);
		float num3 = ((Screen.width < 800 && Screen.height < 800) ? 30 : 70);
		float num4 = num3 + 10f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Initialize Twitter"))
		{
			TwitterAndroid.init("INSERT_YOUR_INFO_HERE", "INSERT_YOUR_INFO_HERE");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Login"))
		{
			TwitterAndroid.showLoginDialog();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Is Logged In?"))
		{
			bool flag = TwitterAndroid.isLoggedIn();
			Debug.Log("Is logged in?: " + flag);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get Username"))
		{
			string username = TwitterAndroid.getUsername();
			Debug.Log("username: " + username);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Update with Image"))
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
			JPGEncoder jPGEncoder = new JPGEncoder(texture2D, 75f);
			jPGEncoder.doEncoding();
			byte[] bytes = jPGEncoder.GetBytes();
			TwitterAndroid.postUpdateWithImage("test update from Unity!", bytes);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Facebook Scene"))
		{
			Application.LoadLevel("FacebookTestScene");
		}
		left = (float)Screen.width - num2 - 5f;
		num = 5f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Logout"))
		{
			TwitterAndroid.logout();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Post Update"))
		{
			TwitterAndroid.postUpdate("im an update from the Twitter Android Plugin");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get Home Timeline"))
		{
			TwitterAndroid.getHomeTimeline();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get Followers"))
		{
			TwitterAndroid.getFollowers();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Custom Request"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("screen_name", "prime_31");
			TwitterAndroid.performRequest("get", "/1/users/show.json", dictionary);
		}
	}
}
