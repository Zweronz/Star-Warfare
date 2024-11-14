using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SocialNetworkingManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action twitterLogin;

	[method: MethodImpl(32)]
	public static event Action<string> twitterLoginFailed;

	[method: MethodImpl(32)]
	public static event Action twitterPost;

	[method: MethodImpl(32)]
	public static event Action<string> twitterPostFailed;

	[method: MethodImpl(32)]
	public static event Action<ArrayList> twitterHomeTimelineReceived;

	[method: MethodImpl(32)]
	public static event Action<string> twitterHomeTimelineFailed;

	[method: MethodImpl(32)]
	public static event Action<object> twitterRequestDidFinishEvent;

	[method: MethodImpl(32)]
	public static event Action<string> twitterRequestDidFailEvent;

	[method: MethodImpl(32)]
	public static event Action facebookLogin;

	[method: MethodImpl(32)]
	public static event Action<string> facebookLoginFailed;

	[method: MethodImpl(32)]
	public static event Action facebookDidLogoutEvent;

	[method: MethodImpl(32)]
	public static event Action<DateTime> facebookDidExtendTokenEvent;

	[method: MethodImpl(32)]
	public static event Action facebookSessionInvalidatedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> facebookReceivedUsername;

	[method: MethodImpl(32)]
	public static event Action<string> facebookUsernameRequestFailed;

	[method: MethodImpl(32)]
	public static event Action facebookPost;

	[method: MethodImpl(32)]
	public static event Action<string> facebookPostFailed;

	[method: MethodImpl(32)]
	public static event Action<ArrayList> facebookReceivedFriends;

	[method: MethodImpl(32)]
	public static event Action<string> facebookFriendRequestFailed;

	[method: MethodImpl(32)]
	public static event Action facebookDialogCompleted;

	[method: MethodImpl(32)]
	public static event Action<string> facebookDialogFailed;

	[method: MethodImpl(32)]
	public static event Action facebookDialogDidntComplete;

	[method: MethodImpl(32)]
	public static event Action<string> facebookDialogCompletedWithUrl;

	[method: MethodImpl(32)]
	public static event Action<object> facebookReceivedCustomRequest;

	[method: MethodImpl(32)]
	public static event Action<string> facebookCustomRequestFailed;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void twitterLoginSucceeded(string empty)
	{
		if (SocialNetworkingManager.twitterLogin != null)
		{
			SocialNetworkingManager.twitterLogin();
		}
	}

	public void twitterLoginDidFail(string error)
	{
		if (SocialNetworkingManager.twitterLoginFailed != null)
		{
			SocialNetworkingManager.twitterLoginFailed(error);
		}
	}

	public void twitterPostSucceeded(string empty)
	{
		if (SocialNetworkingManager.twitterPost != null)
		{
			SocialNetworkingManager.twitterPost();
		}
	}

	public void twitterPostDidFail(string error)
	{
		if (SocialNetworkingManager.twitterPostFailed != null)
		{
			SocialNetworkingManager.twitterPostFailed(error);
		}
	}

	public void twitterHomeTimelineDidFail(string error)
	{
		if (SocialNetworkingManager.twitterHomeTimelineFailed != null)
		{
			SocialNetworkingManager.twitterHomeTimelineFailed(error);
		}
	}

	public void twitterHomeTimelineDidFinish(string results)
	{
		if (SocialNetworkingManager.twitterHomeTimelineReceived != null)
		{
			ArrayList obj = (ArrayList)MiniJSON.jsonDecode(results);
			SocialNetworkingManager.twitterHomeTimelineReceived(obj);
		}
	}

	public void twitterRequestDidFinish(string results)
	{
		if (SocialNetworkingManager.twitterRequestDidFinishEvent != null)
		{
			SocialNetworkingManager.twitterRequestDidFinishEvent(MiniJSON.jsonDecode(results));
		}
	}

	public void twitterRequestDidFail(string error)
	{
		if (SocialNetworkingManager.twitterRequestDidFailEvent != null)
		{
			SocialNetworkingManager.twitterRequestDidFailEvent(error);
		}
	}

	public void facebookLoginSucceeded(string empty)
	{
		if (SocialNetworkingManager.facebookLogin != null)
		{
			SocialNetworkingManager.facebookLogin();
		}
	}

	public void facebookLoginDidFail(string error)
	{
		if (SocialNetworkingManager.facebookLoginFailed != null)
		{
			SocialNetworkingManager.facebookLoginFailed(error);
		}
	}

	public void facebookDidLogout(string empty)
	{
		if (SocialNetworkingManager.facebookDidLogoutEvent != null)
		{
			SocialNetworkingManager.facebookDidLogoutEvent();
		}
	}

	public void facebookDidExtendToken(string secondsSinceEpoch)
	{
		if (SocialNetworkingManager.facebookDidExtendTokenEvent != null)
		{
			double value = double.Parse(secondsSinceEpoch);
			DateTime obj = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
			SocialNetworkingManager.facebookDidExtendTokenEvent(obj);
		}
	}

	public void facebookSessionInvalidated(string empty)
	{
		if (SocialNetworkingManager.facebookSessionInvalidatedEvent != null)
		{
			SocialNetworkingManager.facebookSessionInvalidatedEvent();
		}
	}

	public void facebookDidReceiveUsername(string username)
	{
		if (SocialNetworkingManager.facebookReceivedUsername != null)
		{
			SocialNetworkingManager.facebookReceivedUsername(username);
		}
	}

	public void facebookUsernameRequestDidFail(string error)
	{
		if (SocialNetworkingManager.facebookUsernameRequestFailed != null)
		{
			SocialNetworkingManager.facebookUsernameRequestFailed(error);
		}
	}

	public void facebookPostSucceeded(string empty)
	{
		if (SocialNetworkingManager.facebookPost != null)
		{
			SocialNetworkingManager.facebookPost();
		}
	}

	public void facebookPostDidFail(string error)
	{
		if (SocialNetworkingManager.facebookPostFailed != null)
		{
			SocialNetworkingManager.facebookPostFailed(error);
		}
	}

	public void facebookDidReceiveFriends(string jsonResult)
	{
		if (SocialNetworkingManager.facebookReceivedFriends != null)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonResult);
			if (hashtable.Contains("data"))
			{
				SocialNetworkingManager.facebookReceivedFriends((ArrayList)hashtable["data"]);
			}
			else
			{
				SocialNetworkingManager.facebookReceivedFriends(new ArrayList());
			}
		}
	}

	public void facebookFriendRequestDidFail(string error)
	{
		if (SocialNetworkingManager.facebookFriendRequestFailed != null)
		{
			SocialNetworkingManager.facebookFriendRequestFailed(error);
		}
	}

	public void facebookDialogDidComplete(string empty)
	{
		if (SocialNetworkingManager.facebookDialogCompleted != null)
		{
			SocialNetworkingManager.facebookDialogCompleted();
		}
	}

	public void facebookDialogDidCompleteWithUrl(string url)
	{
		if (SocialNetworkingManager.facebookDialogCompletedWithUrl != null)
		{
			SocialNetworkingManager.facebookDialogCompletedWithUrl(url);
		}
	}

	public void facebookDialogDidNotComplete(string empty)
	{
		if (SocialNetworkingManager.facebookDialogDidntComplete != null)
		{
			SocialNetworkingManager.facebookDialogDidntComplete();
		}
	}

	public void facebookDialogDidFailWithError(string error)
	{
		if (SocialNetworkingManager.facebookDialogFailed != null)
		{
			SocialNetworkingManager.facebookDialogFailed(error);
		}
	}

	public void facebookDidReceiveCustomRequest(string result)
	{
		if (SocialNetworkingManager.facebookReceivedCustomRequest != null)
		{
			object obj = MiniJSON.jsonDecode(result);
			SocialNetworkingManager.facebookReceivedCustomRequest(obj);
		}
	}

	public void facebookCustomRequestDidFail(string error)
	{
		if (SocialNetworkingManager.facebookCustomRequestFailed != null)
		{
			SocialNetworkingManager.facebookCustomRequestFailed(error);
		}
	}
}
