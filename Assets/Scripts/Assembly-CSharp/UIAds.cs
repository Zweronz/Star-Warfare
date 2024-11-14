using UnityEngine;

public class UIAds : GameUI, GameUIListener
{
	public string url;

	protected override void OnInit(GameUIBundle bundle)
	{
		base.OnInit(bundle);
		SetListener(this);
	}

	public void OnTouch(GameUITouchEvent touchEvent)
	{
		switch (touchEvent.EventID)
		{
		case TouchEventID.UI_Ads_BH_Link:
			AndroidPluginScript.OpenURL();
			GameUIManager.GetInstance().RemoveUI(this);
			break;
		case TouchEventID.UI_Ads_Close:
			Debug.Log("UI_Ads_Close");
			GameUIManager.GetInstance().RemoveUI(this);
			AndroidPluginScript.CloseGame();
			break;
		}
	}
}
