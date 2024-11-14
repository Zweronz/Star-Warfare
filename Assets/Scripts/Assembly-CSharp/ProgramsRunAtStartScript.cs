using System;
using System.Collections;
using UnityEngine;

public class ProgramsRunAtStartScript : MonoBehaviour
{
	public bool tokenSent;

	private IEnumerator Start()
	{
		if (!GameApp.GetInstance().AdsInit)
		{
			GameObject prefab = Resources.Load("Ads&Social/AdsManager") as GameObject;
			GameObject go = UnityEngine.Object.Instantiate(prefab) as GameObject;
			UnityEngine.Object.DontDestroyOnLoad(go);
			UnityEngine.Object.Instantiate(prefab);
			GameApp.GetInstance().AdsInit = true;
		}
		GameApp.GetInstance().UUID = AndroidPluginScript.GetAndroidId();
		GameApp.GetInstance().MacAddress = AndroidPluginScript.GetMacAddress();
		if (GameApp.GetInstance().MacAddress != null)
		{
			Debug.Log("MacAddress: " + GameApp.GetInstance().MacAddress);
		}
		else
		{
			Debug.Log("MacAddress is null");
		}
		if (AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			GameApp.GetInstance().AppStatus = 0;
		}
		else
		{
			GameApp.GetInstance().AppStatus = 1;
		}
		GameApp.GetInstance().AppSNSStatus = 0;
		if (!GameApp.GetInstance().IsConnectedToInternet())
		{
			yield break;
		}
		WWW getAppStatusWWW = new WWW("http://sw1.freyrgames.com:8088/AdvertServer/GetAppStatus?appcode=ag001&v=" + GameApp.GetInstance().GetUserState().version);
		while (!getAppStatusWWW.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			byte[] appStatusBytes = getAppStatusWWW.bytes;
			BytesBuffer bb = new BytesBuffer(appStatusBytes);
			if (bb.ReadShort() == 0)
			{
				byte status = bb.ReadByte();
				GameApp.GetInstance().AppSNSStatus = status;
				Debug.Log("AppSNSStatus:" + status);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	private void Update()
	{
	}
}
