using UnityEngine;

public class StateWinScore : MonoBehaviour
{
	[SerializeField]
	private UILabel totalScore;

	private void Update()
	{
		totalScore.text = string.Empty + UserStateUI.GetInstance().GetTopScore() + "/" + UserStateUI.GetInstance().GetTotalScore();
	}
}
