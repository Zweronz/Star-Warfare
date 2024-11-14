using System;

public class SwBackgroundLoginRequest : SwLoginRequest
{
	protected long mReceiveNextLoginInterval;

	protected override string GetUrl()
	{
		string uUID = GameApp.GetInstance().UUID;
		int num = (GameApp.GetInstance().GetUserState().GetFirstLunchApp() ? 1 : 0);
		mSign = Hash(uUID);
		return "http://" + base.REGIST_SERVER_ADDRESS + "/Sw1/SwBackgroundLogin?udid=" + ToBase64String(uUID) + "&platform=" + ToBase64String(((int)base.Platform).ToString()) + "&device=" + ToBase64String(base.Device) + "&os=" + ToBase64String(base.OS) + "&recruit=" + num + "&test=" + mSign;
	}

	protected override void ReadData(BytesBuffer bb)
	{
		mReceiveErrorCode = bb.ReadInt();
		if (mReceiveErrorCode == SwErrorCode.SUCCESS)
		{
			mReceiveNextLoginInterval = bb.ReadLong();
		}
	}

	public override void ProcessLogic()
	{
		if (mReceiveErrorCode == SwErrorCode.SUCCESS)
		{
			GameApp.GetInstance().GetUserState().SetLastLoginTicks(DateTime.Now.Ticks);
			GameApp.GetInstance().GetUserState().SetNextLoginInterval(mReceiveNextLoginInterval * 10000);
		}
	}
}
