using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FacebookBinding
{
	[DllImport("__Internal")]
	private static extern void _facebookInit(string applicationId);

	public static void init(string applicationId)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookInit(applicationId);
		}
	}

	[DllImport("__Internal")]
	private static extern bool _facebookIsLoggedIn();

	public static bool isLoggedIn()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _facebookIsLoggedIn();
		}
		return false;
	}

	[DllImport("__Internal")]
	private static extern string _facebookGetFacebookAccessToken();

	public static string getFacebookAccessToken()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _facebookGetFacebookAccessToken();
		}
		return string.Empty;
	}

	[DllImport("__Internal")]
	private static extern void _facebookExtendAccessToken();

	public static void extendAccessToken()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookExtendAccessToken();
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookLogin();

	public static void login()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookLogin();
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookLoginWithRequestedPermissions(string perms);

	public static void loginWithRequestedPermissions(string[] permissions)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string perms = string.Join(",", permissions);
			_facebookLoginWithRequestedPermissions(perms);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookLogout();

	public static void logout()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookLogout();
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookGetLoggedinUsersName();

	public static void getLoggedinUsersName()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookGetLoggedinUsersName();
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookPostMessage(string message);

	public static void postMessage(string message)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookPostMessage(message);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookPostMessageWithLink(string message, string link, string linkName);

	public static void postMessageWithLink(string message, string link, string linkName)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookPostMessageWithLink(message, link, linkName);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookPostMessageWithLinkAndLinkToImage(string message, string link, string linkName, string linkToImage, string caption);

	public static void postMessageWithLinkAndLinkToImage(string message, string link, string linkName, string linkToImage, string caption)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookPostMessageWithLinkAndLinkToImage(message, link, linkName, linkToImage, caption);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookPostImage(string pathToImage, string caption);

	public static void postImage(string pathToImage, string caption)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookPostImage(pathToImage, caption);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookPostImageInAlbum(string pathToImage, string caption, string albumId);

	public static void postImage(string pathToImage, string caption, string albumId)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookPostImageInAlbum(pathToImage, caption, albumId);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookGetFriends();

	public static void getFriends()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookGetFriends();
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookShowPostMessageDialog();

	public static void showPostMessageDialog()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookShowPostMessageDialog();
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookShowPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption);

	public static void showPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookShowPostMessageDialogWithOptions(link, linkName, linkToImage, caption);
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookShowPostMessageDialogWithCustomOptions(string json);

	public static void showPostMessageDialogWithOptions(Hashtable options)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookShowPostMessageDialogWithCustomOptions(MiniJSON.jsonEncode(options));
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookShowDialog(string dialogType, string json);

	public static void showDialog(string dialogType, Dictionary<string, string> options)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_facebookShowDialog(dialogType, MiniJSON.jsonEncode(options));
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookGraphRequest(string graphPath, string httpMethod, string jsonDict);

	public static void graphRequest(string graphPath, string httpMethod, Hashtable keyValueHash)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string text = MiniJSON.jsonEncode(keyValueHash);
			if (text != null)
			{
				_facebookGraphRequest(graphPath, httpMethod, text);
			}
		}
	}

	[DllImport("__Internal")]
	private static extern void _facebookRestRequest(string restMethod, string httpMethod, string jsonDict);

	public static void restRequest(string restMethod, string httpMethod, Hashtable keyValueHash)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string text = MiniJSON.jsonEncode(keyValueHash);
			if (text != null)
			{
				_facebookRestRequest(restMethod, httpMethod, text);
			}
		}
	}
}
