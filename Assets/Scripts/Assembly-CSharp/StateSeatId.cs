using UnityEngine;

public class StateSeatId : MonoBehaviour
{
	[SerializeField]
	private UISprite seatIcon;

	private int lastSeatId = -1;

	private void Update()
	{
		int playerSeatID = UserStateUI.GetInstance().GetPlayerSeatID();
		if (lastSeatId != playerSeatID)
		{
			lastSeatId = playerSeatID;
			seatIcon.spriteName = UserStateUI.GetInstance().GetSeatSpriteName(playerSeatID);
			if (UserStateUI.GetInstance().IsTeamMode())
			{
				seatIcon.color = UIConstant.COLOR_TEAM_PLAYER_ICONS[playerSeatID / 4];
			}
			else
			{
				seatIcon.color = UIConstant.COLOR_PLAYER_ICONS[playerSeatID];
			}
			seatIcon.MakePixelPerfect();
		}
	}
}
