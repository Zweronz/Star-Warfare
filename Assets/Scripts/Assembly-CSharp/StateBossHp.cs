using UnityEngine;

public class StateBossHp : MonoBehaviour
{
	[SerializeField]
	private UISprite bossHp;

	private void Update()
	{
		bossHp.fillAmount = UserStateUI.GetInstance().GetBossHpPercent();
	}
}
