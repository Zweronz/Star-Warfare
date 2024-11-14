using UnityEngine;

public class AndroidIAPPluginScript
{
	public static void CallPurchaseProduct(string productId)
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CallPurchaseProduct", productId);
		}
		else
		{
			Debug.Log("CallPurchaseProduct = " + productId);
		}
	}

	public static int GetPurchaseStatus()
	{
		if (!IsPC())
		{
			return CurrentActivity.getInstance().JavaObject.Call<int>("GetPurchaseStatus", new object[0]);
		}
		return 0;
	}

	public static void InitPurchase()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("InitPurchase");
		}
		else
		{
			Debug.Log("InitIAP");
		}
	}

	public static void DoPurchase()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("DoPurchase");
		}
		else
		{
			Debug.Log("DoPurchase");
		}
	}

	public static void FailPurchase()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("FailPurchase");
		}
		else
		{
			Debug.Log("FailPurchase");
		}
	}

	public static void CheckIap()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("CheckIap");
		}
		else
		{
			Debug.Log("CheckIap");
		}
	}

	public static void GetRestorePurchse()
	{
		if (!IsPC())
		{
			CurrentActivity.getInstance().JavaObject.Call("GetRestorePurchse");
		}
		else
		{
			Debug.Log("GetRestorePurchse");
		}
	}

	public static bool IsPC()
	{
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
	}
}
