using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class MithrilRewardsScript : MonoBehaviour
{
	public static bool showDownloadIcon;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void RewardMithril(string msg)
	{
		int value = int.Parse(msg);
		value = Mathf.Clamp(value, 0, 50);
		if (value > 0 && value <= 50)
		{
			GameApp.GetInstance().GetUserState().AddMithril(value);
			GameApp.GetInstance().Save();
			AndroidAdsPluginScript.ShowTapjoyDialog(value);
		}
	}

	public void RewardCash(string msg)
	{
		int value = int.Parse(msg);
		value = Mathf.Clamp(value, 0, 3001);
		if (value > 0 && value <= 3000)
		{
			GameApp.GetInstance().GetUserState().AddCash(value);
			GameApp.GetInstance().Save();
		}
	}

	public void ShowDownloadIcon()
	{
		showDownloadIcon = true;
	}

	public void HideDownloadIcon()
	{
		showDownloadIcon = false;
	}

	public void SetName(string name)
	{
		GameApp.GetInstance().GetUserState().SetRoleName(name);
		GameApp.GetInstance().isChangeName = true;
	}

	public void GetRookiePac(string str)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (!userState.bPurchaseRookie)
		{
			userState.DeliverIAPItem(IAPName.ROOKIE);
		}
	}

	public void RestoreIAPContent(string id)
	{
		Debug.Log("id");
		switch (id)
		{
		case "com.ifreyrgames.starwarfarehd.rookie099cents":
		case "com.ifreyrgames.starwarfare.rookie099cents":
			GameApp.GetInstance().GetUserState().DeliverIAPItem(IAPName.ROOKIE);
			Debug.Log("Deliver Rookie.");
			break;
		case "com.ifreyr.starwarfarehd.sergeant299cents":
		case "com.ifreyr.starwarfare.sergeant299cents":
			GameApp.GetInstance().GetUserState().DeliverIAPItem(IAPName.SERGEANT);
			Debug.Log("Deliver SERGEANT.");
			break;
		}
	}

	public void StartIAPValidation(string base64)
	{
		StartCoroutine(GetIAPValidation(base64));
	}

	private IEnumerator GetIAPValidation(string base64)
	{
		float startTime = Time.realtimeSinceStartup;
		bool timeOut = false;
		WWW iapWWW = null;
		byte[] encodedBytes = Encoding.UTF8.GetBytes(base64);
		string base64EncodedText = Convert.ToBase64String(encodedBytes);
		string base64UrlEncodedText = base64EncodedText.Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			iapWWW = new WWW("http://sw1.freyrgames.com:7671/IapStatServer/VerifyAgPay?receipt=" + base64UrlEncodedText + "&appcode=ag001&udid=" + GameApp.GetInstance().UUID);
			break;
		case AndroidConstant.Version.Kindle:
			iapWWW = new WWW("http://sw1.freyrgames.com:7671/IapStatServer/VerifyAwsPay?receipt=" + base64UrlEncodedText + "&appcode=ag001&udid=" + GameApp.GetInstance().UUID);
			break;
		}
		while (!iapWWW.isDone)
		{
			if (Time.realtimeSinceStartup - startTime > 40f)
			{
				timeOut = true;
				iapWWW.Dispose();
				break;
			}
			yield return new WaitForSeconds(0f);
		}
		if (timeOut)
		{
			Debug.Log("http timeout");
			AndroidIAPPluginScript.FailPurchase();
			yield break;
		}
		if (iapWWW.error != null)
		{
			Debug.Log("http error:" + iapWWW.error);
			if (base64.Length < GameApp.GetInstance().Base64MinLength)
			{
				AndroidIAPPluginScript.FailPurchase();
			}
			else
			{
				AndroidIAPPluginScript.DoPurchase();
			}
			yield break;
		}
		Debug.Log("iapWWW.text = " + iapWWW.text);
		if (iapWWW.text.Equals("succ"))
		{
			Debug.Log("iap validation succ!");
			if (base64.Length < GameApp.GetInstance().Base64MinLength)
			{
				AndroidIAPPluginScript.FailPurchase();
			}
			else
			{
				AndroidIAPPluginScript.DoPurchase();
			}
		}
		else
		{
			Debug.Log("iap validation fail!");
			AndroidIAPPluginScript.FailPurchase();
		}
	}
}
