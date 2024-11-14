using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HttpRequestThread
{
	public string CryptMD5String(string oriStr)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(oriStr);
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public void DoWork()
	{
		try
		{
			string text = "http://sw1.freyrgames.com:8088/AdvertServer/GetAward";
			string uUID = GameApp.GetInstance().UUID;
			string text2 = CryptMD5String("ag001:ad001:" + uUID + ":B7D02ED9D99A54FBC30FE3CE7659E12EB7D195C6");
			string text3 = "appcode=ag001&adcode=ad001&udid=" + uUID + "&fhash=" + text2;
			string text4 = HttpRequest.Get(text + "?" + text3);
			if (text4.StartsWith("error&") || text4.Equals("ok"))
			{
			}
			string[] array = text4.Split('#');
			int num = 0;
			string[] array2 = array;
			foreach (string text5 in array2)
			{
				string[] array3 = text5.Split('&');
				int num2 = int.Parse(array3[2]);
				string text6 = array3[1];
				if (text6.Length >= 36)
				{
					if (num2 > 20)
					{
						num2 = 20;
					}
					GameApp.GetInstance().GetUserState().AddMithril(num2);
					GameApp.GetInstance().GetUserState().showRewardMsg = true;
					num += num2;
				}
				else
				{
					GameApp.GetInstance().GetUserState().AddMithril(num2);
				}
			}
			GameApp.GetInstance().GetUserState().rewardNumber = num;
			GameApp.GetInstance().GetUserState().rewardAdsName = "Flurry";
			Debug.Log("end");
		}
		catch (Exception ex)
		{
			Debug.Log("HttpRequest Exception:" + ex.Message);
		}
		try
		{
			string text7 = "http://sw1.freyrgames.com:8088/AdvertServer/GetAward";
			string uUID2 = GameApp.GetInstance().UUID;
			string text8 = "ag001:ad002:" + uUID2 + ":42b89fc3b308a25ec6376d29f7d48fec";
			string text9 = "appcode=ag001&adcode=ad002&udid=" + uUID2 + "&fhash=" + text8;
			string text10 = HttpRequest.Get(text7 + "?" + text9);
			if (!text10.StartsWith("error&") && !text10.Equals("ok"))
			{
				string[] array4 = text10.Split('#');
				string[] array5 = array4;
				foreach (string text11 in array5)
				{
					string[] array6 = text11.Split('&');
					int num3 = int.Parse(array6[2]);
					if (num3 > 20)
					{
						num3 = 20;
					}
					GameApp.GetInstance().GetUserState().AddMithril(num3);
					GameApp.GetInstance().GetUserState().rewardNumber = num3;
					GameApp.GetInstance().GetUserState().rewardAdsName = "SponsorPay";
					GameApp.GetInstance().GetUserState().showRewardMsg = true;
				}
			}
		}
		catch (Exception ex2)
		{
			Debug.Log("HttpRequest Exception:" + ex2.Message);
		}
		try
		{
			string text12 = "http://sw1.freyrgames.com:8088/AdvertServer/GetAward";
			string uUID3 = GameApp.GetInstance().UUID;
			string text13 = "g001:ad020:" + uUID3 + ":B7D02ED9D99A54FBC30FE3CE7659E12EB7D195C6";
			string text14 = "appcode=g001&adcode=ad020&udid=" + uUID3 + "&fhash=" + text13;
			string text15 = HttpRequest.Get(text12 + "?" + text14);
			if (text15.StartsWith("error&") || text15.Equals("ok"))
			{
				return;
			}
			string[] array7 = text15.Split('#');
			string[] array8 = array7;
			foreach (string text16 in array8)
			{
				string[] array9 = text16.Split('&');
				int num4 = int.Parse(array9[2]);
				if (num4 > 20)
				{
					num4 = 20;
				}
			}
		}
		catch (Exception ex3)
		{
			Debug.Log("HttpRequest Exception:" + ex3.Message);
		}
	}
}
