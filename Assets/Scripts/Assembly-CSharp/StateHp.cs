using UnityEngine;

public class StateHp : MonoBehaviour
{
	[SerializeField]
	private UISprite hpSprite;

	[SerializeField]
	private UILabel hpLabel;

	private void Update()
	{
		hpSprite.fillAmount = (float)UserStateUI.GetInstance().GetHp() / (float)UserStateUI.GetInstance().GetMaxHp();
		hpLabel.text = string.Empty + UserStateUI.GetInstance().GetHp() + "/" + UserStateUI.GetInstance().GetMaxHp();
	}
}
