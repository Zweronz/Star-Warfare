using UnityEngine;

public class StateSuvival : MonoBehaviour
{
	[SerializeField]
	private UILabel kills;

	private void Update()
	{
		kills.text = string.Empty + UserStateUI.GetInstance().GetSurvivalKills();
	}
}
