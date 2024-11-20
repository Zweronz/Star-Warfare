using UnityEngine;

public class AndroidIAPPluginScript
{
	public static void CallPurchaseProduct(string productId)
	{
		if (!IsPC())
		{
		}
		else
		{
			Debug.Log("CallPurchaseProduct = " + productId);
		}
	}

	public static int GetPurchaseStatus()
	{
		return 0;
	}

	public static void InitPurchase()
	{
		if (!IsPC())
		{
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
