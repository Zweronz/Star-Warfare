using System.Collections.Generic;
using UnityEngine;

public class StateKillInfoManager : MonoBehaviour
{
	[SerializeField]
	private StateKillInfo killInfoTemplete;

	[SerializeField]
	private GameObject killInfoGrid;

	private List<StateKillInfo> mKillInfoList = new List<StateKillInfo>();

	private int mHeight = 50;

	private void Update()
	{
		bool flag = false;
		for (UserStateUI.KillInfoUI killInfoUI = UserStateUI.GetInstance().PopKillInfo(); killInfoUI != null; killInfoUI = UserStateUI.GetInstance().PopKillInfo())
		{
			flag = true;
			GameObject gameObject = Object.Instantiate(killInfoTemplete.gameObject) as GameObject;
			gameObject.transform.parent = killInfoGrid.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(true);
			StateKillInfo component = gameObject.GetComponent<StateKillInfo>();
			component.Set(killInfoUI.KillerID, killInfoUI.Action, killInfoUI.KilledID);
			mKillInfoList.Add(component);
			if (mKillInfoList.Count > 3)
			{
				Object.Destroy(mKillInfoList[0].gameObject);
				mKillInfoList.RemoveAt(0);
			}
		}
		List<StateKillInfo> list = new List<StateKillInfo>();
		foreach (StateKillInfo mKillInfo in mKillInfoList)
		{
			if (!mKillInfo.FadeOut())
			{
				list.Add(mKillInfo);
			}
		}
		foreach (StateKillInfo item in list)
		{
			flag = true;
			mKillInfoList.Remove(item);
			Object.Destroy(item.gameObject);
		}
		if (flag)
		{
			for (int i = 0; i < mKillInfoList.Count; i++)
			{
				mKillInfoList[i].transform.localPosition = new Vector3(0f, -i * mHeight, 0f);
			}
		}
	}

	private void OnDisable()
	{
		foreach (StateKillInfo mKillInfo in mKillInfoList)
		{
			Object.Destroy(mKillInfo.gameObject);
		}
		mKillInfoList.Clear();
	}
}
