using System;
using System.Runtime.CompilerServices;
using Prime31;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action loginSucceededEvent;

	[method: MethodImpl(32)]
	public static event Action<string> loginFailedEvent;

	[method: MethodImpl(32)]
	public static event Action loggedOutEvent;

	[method: MethodImpl(32)]
	public static event Action<DateTime> accessTokenExtendedEvent;

	[method: MethodImpl(32)]
	public static event Action failedToExtendTokenEvent;

	[method: MethodImpl(32)]
	public static event Action sessionInvalidatedEvent;

	[method: MethodImpl(32)]
	public static event Action dialogCompletedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> dialogFailedEvent;

	[method: MethodImpl(32)]
	public static event Action dialogDidNotCompleteEvent;

	[method: MethodImpl(32)]
	public static event Action<string> dialogCompletedWithUrlEvent;

	[method: MethodImpl(32)]
	public static event Action<object> customRequestReceivedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> customRequestFailedEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void facebookLoginSucceeded(string empty)
	{
		if (FacebookManager.loginSucceededEvent != null)
		{
			FacebookManager.loginSucceededEvent();
		}
	}

	public void facebookLoginDidFail(string error)
	{
		if (FacebookManager.loginFailedEvent != null)
		{
			FacebookManager.loginFailedEvent(error);
		}
	}

	public void facebookDidLogout(string empty)
	{
		if (FacebookManager.loggedOutEvent != null)
		{
			FacebookManager.loggedOutEvent();
		}
	}

	public void facebookDidExtendToken(string secondsSinceEpoch)
	{
		if (FacebookManager.accessTokenExtendedEvent != null)
		{
			double value = double.Parse(secondsSinceEpoch);
			DateTime obj = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
			FacebookManager.accessTokenExtendedEvent(obj);
		}
	}

	public void facebookFailedToExtendToken(string empty)
	{
		if (FacebookManager.failedToExtendTokenEvent != null)
		{
			FacebookManager.failedToExtendTokenEvent();
		}
	}

	public void facebookSessionInvalidated(string empty)
	{
		if (FacebookManager.sessionInvalidatedEvent != null)
		{
			FacebookManager.sessionInvalidatedEvent();
		}
	}

	public void facebookDialogDidComplete(string empty)
	{
		if (FacebookManager.dialogCompletedEvent != null)
		{
			FacebookManager.dialogCompletedEvent();
		}
	}

	public void facebookDialogDidCompleteWithUrl(string url)
	{
		if (FacebookManager.dialogCompletedWithUrlEvent != null)
		{
			FacebookManager.dialogCompletedWithUrlEvent(url);
		}
	}

	public void facebookDialogDidNotComplete(string empty)
	{
		if (FacebookManager.dialogDidNotCompleteEvent != null)
		{
			FacebookManager.dialogDidNotCompleteEvent();
		}
	}

	public void facebookDialogDidFailWithError(string error)
	{
		if (FacebookManager.dialogFailedEvent != null)
		{
			FacebookManager.dialogFailedEvent(error);
		}
	}

	public void facebookDidReceiveCustomRequest(string result)
	{
		if (FacebookManager.customRequestReceivedEvent != null)
		{
			object obj = Prime31.MiniJSON.jsonDecode(result);
			FacebookManager.customRequestReceivedEvent(obj);
		}
	}

	public void facebookCustomRequestDidFail(string error)
	{
		if (FacebookManager.customRequestFailedEvent != null)
		{
			FacebookManager.customRequestFailedEvent(error);
		}
	}
}
