using System.Collections.Generic;
using UnityEngine;

public class StatePersonalPopulation : MonoBehaviour
{
	[SerializeField]
	private GameObject[] player;

	private void Update()
	{
		List<UserStateUI.RemotePlayerUI> remotePlayerList = UserStateUI.GetInstance().GetRemotePlayerList();
		for (int i = 0; i < player.Length; i++)
		{
			if (i < remotePlayerList.Count)
			{
				if (!player[i].activeSelf)
				{
					player[i].SetActive(true);
				}
			}
			else if (player[i].activeSelf)
			{
				player[i].SetActive(false);
			}
		}
	}
}
