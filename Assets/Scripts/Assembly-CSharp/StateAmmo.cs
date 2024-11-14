using UnityEngine;

public class StateAmmo : MonoBehaviour
{
	[SerializeField]
	private UISprite ammoSprite;

	[SerializeField]
	private UILabel ammoLabel;

	private void Update()
	{
		int num = Mathf.Max(UserStateUI.GetInstance().GetAmmo(), UserStateUI.GetInstance().GetMaxAmmo());
		ammoSprite.fillAmount = (float)UserStateUI.GetInstance().GetAmmo() / (float)num;
		ammoLabel.text = string.Empty + UserStateUI.GetInstance().GetAmmo() + "/" + num;
	}
}
