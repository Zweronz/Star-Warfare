using System.Collections.Generic;
using UnityEngine;

public class NumberManager
{
	public const int SCORE_COUNT = 1;

	private static NumberManager instance;

	private List<UINumber> uiScoreList;

	private GameObject scorePrefab;

	private NumberManager()
	{
		uiScoreList = new List<UINumber>();
	}

	public static NumberManager GetInstance()
	{
		if (instance == null)
		{
			instance = new NumberManager();
		}
		return instance;
	}

	public void Init()
	{
		GameObject original = Resources.Load("NGUI/Number/Score") as GameObject;
		scorePrefab = Object.Instantiate(original) as GameObject;
		for (int i = 0; i < 1; i++)
		{
			UINumber[] componentsInChildren = scorePrefab.GetComponentsInChildren<UINumber>(true);
			UINumber[] array = componentsInChildren;
			foreach (UINumber item in array)
			{
				uiScoreList.Add(item);
			}
		}
	}

	public void Clear()
	{
		uiScoreList.Clear();
		Object.Destroy(scorePrefab);
	}

	private void Clear(List<UINumber> numberList)
	{
		foreach (UINumber number in numberList)
		{
			Object.Destroy(number.gameObject);
		}
		numberList.Clear();
	}

	public void ShowScore(Vector3 pos, int score)
	{
		Debug.Log("Show Score : " + score);
		for (int i = 0; i < uiScoreList.Count; i++)
		{
			if (!uiScoreList[i].gameObject.activeSelf)
			{
				string str = "[ffff00]" + score + "EXP[-]";
				uiScoreList[i].SetData(pos, str);
				uiScoreList[i].gameObject.SetActive(true);
				break;
			}
		}
	}
}
