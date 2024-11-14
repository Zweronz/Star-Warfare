using System.Collections.Generic;
using UnityEngine;

public class IAPManager
{
	private static IAPManager m_instance;

	private Stack<IAPName> m_iaps = new Stack<IAPName>();

	private bool m_deliver;

	public static IAPManager GetInstance()
	{
		if (m_instance == null)
		{
			m_instance = new IAPManager();
		}
		return m_instance;
	}

	public bool IsDeliveredImmediately()
	{
		return m_deliver;
	}

	public void Delivered()
	{
		m_deliver = true;
	}

	public void Add(IAPName iap)
	{
		m_iaps.Push(iap);
	}

	public void DeliverIAP()
	{
		while (m_iaps.Count > 0)
		{
			IAPName iAPName = m_iaps.Pop();
			Debug.Log("IAPManager: " + iAPName);
			GameApp.GetInstance().GetUserState().DeliverIAPItem(iAPName);
		}
	}

	public void Test()
	{
		if (!IsDeliveredImmediately())
		{
			Add(IAPName.M168);
		}
	}
}
