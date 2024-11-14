using UnityEngine;

public class StateDoubleBossHp : MonoBehaviour
{
	[SerializeField]
	private UISprite leftBossHp;

	[SerializeField]
	private UISprite rightBossHp;

	private void Update()
	{
		leftBossHp.fillAmount = UserStateUI.GetInstance().GetBossHpPercent();
		rightBossHp.fillAmount = UserStateUI.GetInstance().Get2ndBossHpPercent();
	}
}
