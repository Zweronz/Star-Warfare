using UnityEngine;

public class StateRebirth : MonoBehaviour
{
	[SerializeField]
	private UILabel timeLabel;

	[SerializeField]
	private TweenScale tweenScale;

	private void OnEnable()
	{
		tweenScale.Reset();
	}

	private void Update()
	{
		timeLabel.text = string.Empty + UserStateUI.GetInstance().GetRebirthTime();
	}
}
