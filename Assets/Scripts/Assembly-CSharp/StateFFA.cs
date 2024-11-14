using UnityEngine;

public class StateFFA : MonoBehaviour
{
	[SerializeField]
	private UILabel playerScore;

	[SerializeField]
	private UISprite playerIcon;

	[SerializeField]
	private UILabel topScore;

	[SerializeField]
	private UISprite topSeat;

	[SerializeField]
	private UISprite[] otherTopIcon;

	private void Update()
	{
		playerScore.text = string.Empty + UserStateUI.GetInstance().GetPlayerScore();
		int playerSeatID = UserStateUI.GetInstance().GetPlayerSeatID();
		playerScore.color = UIConstant.COLOR_PLAYER_ICONS[playerSeatID];
		playerIcon.color = UIConstant.COLOR_PLAYER_ICONS[playerSeatID];
		topScore.text = string.Empty + UserStateUI.GetInstance().GetTopScore();
		int topScoreSeat = UserStateUI.GetInstance().GetTopScoreSeat();
		topSeat.spriteName = UserStateUI.GetInstance().GetSeatSpriteName(topScoreSeat);
		topSeat.MakePixelPerfect();
		topSeat.color = UIConstant.COLOR_PLAYER_ICONS[topScoreSeat];
		topScore.color = UIConstant.COLOR_PLAYER_ICONS[topScoreSeat];
		UISprite[] array = otherTopIcon;
		foreach (UISprite uISprite in array)
		{
			uISprite.color = UIConstant.COLOR_PLAYER_ICONS[topScoreSeat];
		}
	}
}
