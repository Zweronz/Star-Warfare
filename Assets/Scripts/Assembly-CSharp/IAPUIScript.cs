using System.Collections.Generic;
using UnityEngine;

public class IAPUIScript : MonoBehaviour
{
	private IAPName iapProcessing = IAPName.None;

	private List<IAPItem> itemList;

	private void Start()
	{
		Shop shop = new Shop();
		shop.CreateIAPShopData();
		itemList = shop.GetIAPList();
	}

	private void Update()
	{
		GetPurchaseStatus();
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(300f, 200f, 400f, 60f), "Mithril:" + GameApp.GetInstance().GetUserState().GetMithril());
		GUI.Label(new Rect(300f, 250f, 400f, 60f), itemList[0].Desc);
		if (GUI.Button(new Rect(300f, 300f, 400f, 60f), "Buy $500,000 Mithril for $0.99 dollar!"))
		{
			AndroidIAPPluginScript.CallPurchaseProduct(itemList[0].ID);
			iapProcessing = IAPName.ROOKIE;
		}
		if (GUI.Button(new Rect(300f, 100f, 400f, 60f), "Free Mithril!"))
		{
			AndroidIAPPluginScript.CallPurchaseProduct(itemList[0].ID);
			iapProcessing = IAPName.SERGEANT;
		}
		if (GUI.Button(new Rect(30f, 560f, 100f, 60f), "Back"))
		{
			Application.LoadLevel("StartMenu");
		}
	}

	public void GetPurchaseStatus()
	{
		if (iapProcessing != IAPName.None)
		{
			switch (AndroidIAPPluginScript.GetPurchaseStatus())
			{
			case 0:
				break;
			case 1:
				GameApp.GetInstance().GetUserState().DeliverIAPItem(iapProcessing);
				iapProcessing = IAPName.None;
				break;
			default:
				iapProcessing = IAPName.None;
				break;
			}
		}
	}
}
