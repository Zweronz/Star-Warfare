using UnityEngine;

public class StateTeamScore : MonoBehaviour
{
	[SerializeField]
	private UILabel blueScore;

	[SerializeField]
	private UILabel redScore;

	private void Update()
	{
		blueScore.text = string.Empty + UserStateUI.GetInstance().GetBlueScore();
		redScore.text = string.Empty + UserStateUI.GetInstance().GetRedScore();
	}
}
