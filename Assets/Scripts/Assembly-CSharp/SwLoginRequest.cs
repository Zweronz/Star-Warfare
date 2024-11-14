using UnityEngine;

public class SwLoginRequest : WWWRequest
{
	protected const string REGIST_SERVER_PORT = "10265";

	protected string REGIST_SERVER_ADDRESS
	{
		get
		{
			string text = "208.43.93.162";
			return text + ":10265";
		}
	}

	protected string Device
	{
		get
		{
			return SystemInfo.deviceModel;
		}
	}

	protected string OS
	{
		get
		{
			return SystemInfo.operatingSystem;
		}
	}

	protected EDevicePlatform Platform
	{
		get
		{
			return EDevicePlatform.ANDROID;
		}
	}

	public override void OnTimeOut()
	{
	}

	public override void OnResponseError()
	{
	}
}
