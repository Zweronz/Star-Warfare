using System;
using System.Collections;
using UnityEngine;

public class SocialNetworkingEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		SocialNetworkingManager.twitterLogin += twitterLogin;
		SocialNetworkingManager.twitterLoginFailed += twitterLoginFailed;
		SocialNetworkingManager.twitterPost += twitterPost;
		SocialNetworkingManager.twitterPostFailed += twitterPostFailed;
		SocialNetworkingManager.twitterHomeTimelineReceived += twitterHomeTimelineReceived;
		SocialNetworkingManager.twitterHomeTimelineFailed += twitterHomeTimelineFailed;
		SocialNetworkingManager.twitterRequestDidFinishEvent += twitterRequestDidFinishEvent;
		SocialNetworkingManager.twitterRequestDidFailEvent += twitterRequestDidFailEvent;
		SocialNetworkingManager.facebookLogin += facebookLogin;
		SocialNetworkingManager.facebookLoginFailed += facebookLoginFailed;
		SocialNetworkingManager.facebookDidLogoutEvent += facebookDidLogoutEvent;
		SocialNetworkingManager.facebookDidExtendTokenEvent += facebookDidExtendTokenEvent;
		SocialNetworkingManager.facebookSessionInvalidatedEvent += facebookSessionInvalidatedEvent;
		SocialNetworkingManager.facebookReceivedUsername += facebookReceivedUsername;
		SocialNetworkingManager.facebookUsernameRequestFailed += facebookUsernameRequestFailed;
		SocialNetworkingManager.facebookPost += facebookPost;
		SocialNetworkingManager.facebookPostFailed += facebookPostFailed;
		SocialNetworkingManager.facebookReceivedFriends += facebookReceivedFriends;
		SocialNetworkingManager.facebookFriendRequestFailed += facebookFriendRequestFailed;
		SocialNetworkingManager.facebookDialogCompleted += facebokDialogCompleted;
		SocialNetworkingManager.facebookDialogCompletedWithUrl += facebookDialogCompletedWithUrl;
		SocialNetworkingManager.facebookDialogDidntComplete += facebookDialogDidntComplete;
		SocialNetworkingManager.facebookDialogFailed += facebookDialogFailed;
		SocialNetworkingManager.facebookReceivedCustomRequest += facebookReceivedCustomRequest;
		SocialNetworkingManager.facebookCustomRequestFailed += facebookCustomRequestFailed;
	}

	private void OnDisable()
	{
		SocialNetworkingManager.twitterLogin -= twitterLogin;
		SocialNetworkingManager.twitterLoginFailed -= twitterLoginFailed;
		SocialNetworkingManager.twitterPost -= twitterPost;
		SocialNetworkingManager.twitterPostFailed -= twitterPostFailed;
		SocialNetworkingManager.twitterHomeTimelineReceived -= twitterHomeTimelineReceived;
		SocialNetworkingManager.twitterHomeTimelineFailed -= twitterHomeTimelineFailed;
		SocialNetworkingManager.twitterRequestDidFinishEvent -= twitterRequestDidFinishEvent;
		SocialNetworkingManager.twitterRequestDidFailEvent -= twitterRequestDidFailEvent;
		SocialNetworkingManager.facebookLogin -= facebookLogin;
		SocialNetworkingManager.facebookLoginFailed -= facebookLoginFailed;
		SocialNetworkingManager.facebookDidLogoutEvent -= facebookDidLogoutEvent;
		SocialNetworkingManager.facebookDidExtendTokenEvent -= facebookDidExtendTokenEvent;
		SocialNetworkingManager.facebookSessionInvalidatedEvent -= facebookSessionInvalidatedEvent;
		SocialNetworkingManager.facebookReceivedUsername -= facebookReceivedUsername;
		SocialNetworkingManager.facebookUsernameRequestFailed -= facebookUsernameRequestFailed;
		SocialNetworkingManager.facebookPost -= facebookPost;
		SocialNetworkingManager.facebookPostFailed -= facebookPostFailed;
		SocialNetworkingManager.facebookReceivedFriends -= facebookReceivedFriends;
		SocialNetworkingManager.facebookFriendRequestFailed += facebookFriendRequestFailed;
		SocialNetworkingManager.facebookDialogCompleted -= facebokDialogCompleted;
		SocialNetworkingManager.facebookDialogCompletedWithUrl -= facebookDialogCompletedWithUrl;
		SocialNetworkingManager.facebookDialogDidntComplete -= facebookDialogDidntComplete;
		SocialNetworkingManager.facebookDialogFailed -= facebookDialogFailed;
		SocialNetworkingManager.facebookReceivedCustomRequest -= facebookReceivedCustomRequest;
		SocialNetworkingManager.facebookCustomRequestFailed -= facebookCustomRequestFailed;
	}

	private void twitterLogin()
	{
		Debug.Log("Successfully logged in to Twitter");
	}

	private void twitterLoginFailed(string error)
	{
		Debug.Log("Twitter login failed: " + error);
	}

	private void twitterPost()
	{
		Debug.Log("Successfully posted to Twitter");
	}

	private void twitterPostFailed(string error)
	{
		Debug.Log("Twitter post failed: " + error);
	}

	private void twitterHomeTimelineFailed(string error)
	{
		Debug.Log("Twitter HomeTimeline failed: " + error);
	}

	private void twitterHomeTimelineReceived(ArrayList result)
	{
		Debug.Log("received home timeline with tweet count: " + result.Count);
	}

	private void twitterRequestDidFailEvent(string error)
	{
		Debug.Log("twitterRequestDidFailEvent: " + error);
	}

	private void twitterRequestDidFinishEvent(object result)
	{
		if (result != null)
		{
			Debug.Log("twitterRequestDidFinishEvent: " + result.GetType().ToString());
		}
		else
		{
			Debug.Log("twitterRequestDidFinishEvent with no data");
		}
	}

	private void facebookLogin()
	{
		Debug.Log("Successfully logged in to Facebook");
	}

	private void facebookLoginFailed(string error)
	{
		Debug.Log("Facebook login failed: " + error);
	}

	private void facebookDidLogoutEvent()
	{
		Debug.Log("facebookDidLogoutEvent");
	}

	private void facebookDidExtendTokenEvent(DateTime newExpiry)
	{
		Debug.Log("facebookDidExtendTokenEvent: " + newExpiry);
	}

	private void facebookSessionInvalidatedEvent()
	{
		Debug.Log("facebookSessionInvalidatedEvent");
	}

	private void facebookReceivedUsername(string username)
	{
		Debug.Log("Facebook logged in users name: " + username);
	}

	private void facebookUsernameRequestFailed(string error)
	{
		Debug.Log("Facebook failed to receive username: " + error);
	}

	private void facebookPost()
	{
		Debug.Log("Successfully posted to Facebook");
	}

	private void facebookPostFailed(string error)
	{
		Debug.Log("Facebook post failed: " + error);
	}

	private void facebookReceivedFriends(ArrayList result)
	{
		Debug.Log("received total friends: " + result.Count);
	}

	private void facebookFriendRequestFailed(string error)
	{
		Debug.Log("FfacebookFriendRequestFailed: " + error);
	}

	private void facebokDialogCompleted()
	{
		Debug.Log("facebokDialogCompleted");
	}

	private void facebookDialogCompletedWithUrl(string url)
	{
		Debug.Log("facebookDialogCompletedWithUrl: " + url);
	}

	private void facebookDialogDidntComplete()
	{
		Debug.Log("facebookDialogDidntComplete");
	}

	private void facebookDialogFailed(string error)
	{
		Debug.Log("facebookDialogFailed: " + error);
	}

	private void facebookReceivedCustomRequest(object obj)
	{
		Debug.Log("facebookReceivedCustomRequest");
	}

	private void facebookCustomRequestFailed(string error)
	{
		Debug.Log("facebookCustomRequestFailed failed: " + error);
	}
}
