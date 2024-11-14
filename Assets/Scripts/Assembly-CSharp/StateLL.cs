using UnityEngine;

public class StateLL : MonoBehaviour
{
	[SerializeField]
	private UILabel playerScore;

	[SerializeField]
	private UILabel topScore;

	[SerializeField]
	private UISprite topSeat;

	private void Update()
	{
		playerScore.text = string.Empty + UserStateUI.GetInstance().GetPlayerScore() + "/" + UserStateUI.GetInstance().GetTotalScore();
		topScore.text = string.Empty + UserStateUI.GetInstance().GetTopScore() + "/" + UserStateUI.GetInstance().GetTotalScore();
		int topScoreSeat = UserStateUI.GetInstance().GetTopScoreSeat();
		topSeat.spriteName = UserStateUI.GetInstance().GetSeatSpriteName(topScoreSeat);
		topSeat.MakePixelPerfect();
		playerScore.color = UIConstant.COLOR_PLAYER_ICONS[UserStateUI.GetInstance().GetPlayerSeatID()];
		topSeat.color = UIConstant.COLOR_PLAYER_ICONS[topScoreSeat];
		topScore.color = UIConstant.COLOR_PLAYER_ICONS[topScoreSeat];
	}
}
