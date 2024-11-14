using System.Collections.Generic;
using UnityEngine;

public class StatePopulation : MonoBehaviour
{
	[SerializeField]
	private GameObject[] blueTeam;

	[SerializeField]
	private GameObject[] redTeam;

	private void Awake()
	{
		GameObject[] array = blueTeam;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		GameObject[] array2 = redTeam;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(false);
		}
	}

	private void Update()
	{
		List<UserStateUI.RemotePlayerUI> remotePlayerList = UserStateUI.GetInstance().GetRemotePlayerList();
		int num = 0;
		int num2 = 0;
		foreach (UserStateUI.RemotePlayerUI item in remotePlayerList)
		{
			if (item.TeamName == TeamName.Blue)
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		if (UserStateUI.GetInstance().GetTeam() == TeamName.Blue)
		{
			num++;
		}
		else
		{
			num2++;
		}
		UpdateTeam(num, blueTeam);
		UpdateTeam(num2, redTeam);
	}

	private void UpdateTeam(int teamNum, GameObject[] team)
	{
		for (int i = 0; i < team.Length; i++)
		{
			if (i < teamNum)
			{
				if (!team[i].activeSelf)
				{
					team[i].SetActive(true);
				}
			}
			else if (team[i].activeSelf)
			{
				team[i].SetActive(false);
			}
		}
	}
}
