using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TwitterBinding
{
	[DllImport("__Internal")]
	private static extern void _twitterInit(string consumerKey, string consumerSecret);

	public static void init(string consumerKey, string consumerSecret)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterInit(consumerKey, consumerSecret);
		}
	}

	[DllImport("__Internal")]
	private static extern bool _twitterIsLoggedIn();

	public static bool isLoggedIn()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterIsLoggedIn();
		}
		return false;
	}

	[DllImport("__Internal")]
	private static extern string _twitterLoggedInUsername();

	public static string loggedInUsername()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterLoggedInUsername();
		}
		return string.Empty;
	}

	[DllImport("__Internal")]
	private static extern void _twitterLogin(string username, string password);

	public static void login(string username, string password)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterLogin(username, password);
		}
	}

	[DllImport("__Internal")]
	private static extern void _twitterShowOauthLoginDialog();

	public static void showOauthLoginDialog()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterShowOauthLoginDialog();
		}
	}

	[DllImport("__Internal")]
	private static extern void _twitterLogout();

	public static void logout()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterLogout();
		}
	}

	[DllImport("__Internal")]
	private static extern void _twitterPostStatusUpdate(string status);

	public static void postStatusUpdate(string status)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterPostStatusUpdate(status);
		}
	}

	[DllImport("__Internal")]
	private static extern void _twitterPostStatusUpdateWithImage(string status, string imagePath);

	public static void postStatusUpdate(string status, string pathToImage)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterPostStatusUpdateWithImage(status, pathToImage);
		}
	}

	[DllImport("__Internal")]
	private static extern void _twitterGetHomeTimeline();

	public static void getHomeTimeline()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterGetHomeTimeline();
		}
	}

	[DllImport("__Internal")]
	private static extern void _twitterPerformRequest(string methodType, string path, string parameters);

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterPerformRequest(methodType, path, (parameters == null) ? null : parameters.toJson());
		}
	}

	[DllImport("__Internal")]
	private static extern bool _twitterIsTweetSheetSupported();

	public static bool isTweetSheetSupported()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterIsTweetSheetSupported();
		}
		return false;
	}

	[DllImport("__Internal")]
	private static extern bool _twitterCanUserTweet();

	public static bool canUserTweet()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterCanUserTweet();
		}
		return false;
	}

	[DllImport("__Internal")]
	private static extern void _twitterShowTweetComposer(string status, string imagePath);

	public static void showTweetComposer(string status, string pathToImage)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterShowTweetComposer(status, pathToImage);
		}
	}
}
