using UnityEngine;

public class StateAim : MonoBehaviour
{
	[SerializeField]
	private UISprite aimSprite;

	private void Update()
	{
		string text = "hud" + UserStateUI.GetInstance().GetAimId();
		if (!aimSprite.spriteName.Equals(text))
		{
			aimSprite.spriteName = text;
			aimSprite.MakePixelPerfect();
		}
		if (UserStateUI.GetInstance().IsAim())
		{
			if (!aimSprite.color.Equals(Color.red))
			{
				aimSprite.color = Color.red;
			}
		}
		else if (!aimSprite.color.Equals(UIConstant.COLOR_AIM))
		{
			aimSprite.color = UIConstant.COLOR_AIM;
		}
	}
}
