using System.Collections.Generic;
using UnityEngine;

public class StateRemotePlayer : MonoBehaviour
{
	[SerializeField]
	private UISprite hpSprite;

	[SerializeField]
	private UISprite hpBGSprite;

	[SerializeField]
	private UISprite iconSprite;

	[SerializeField]
	private int playerIndex;

	private bool mIsActive = true;

	private void Awake()
	{
		InactiveSprite();
	}

	private void ActiveSprite(int seatID)
	{
		if (!mIsActive)
		{
			hpSprite.gameObject.SetActive(true);
			hpBGSprite.gameObject.SetActive(true);
			iconSprite.gameObject.SetActive(true);
			mIsActive = true;
			iconSprite.spriteName = UserStateUI.GetInstance().GetSeatSpriteName(seatID);
			iconSprite.color = UIConstant.COLOR_PLAYER_ICONS[seatID];
		}
	}

	private void InactiveSprite()
	{
		if (mIsActive)
		{
			hpSprite.gameObject.SetActive(false);
			hpBGSprite.gameObject.SetActive(false);
			iconSprite.gameObject.SetActive(false);
			mIsActive = false;
		}
	}

	private void Update()
	{
		List<UserStateUI.RemotePlayerUI> remotePlayerList = UserStateUI.GetInstance().GetRemotePlayerList();
		if (remotePlayerList.Count > playerIndex)
		{
			ActiveSprite(remotePlayerList[playerIndex].SeatID);
			hpSprite.fillAmount = remotePlayerList[playerIndex].PercentOfHp;
		}
		else
		{
			InactiveSprite();
		}
	}
}
