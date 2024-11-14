using UnityEngine;

public class ButtonUI : ComponentUI
{
	[SerializeField]
	private TouchEventID eventID;

	private void Start()
	{
		AddButtonListener(base.gameObject);
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		GameUITouchEvent gameUITouchEvent = new GameUITouchEvent();
		gameUITouchEvent.EventID = eventID;
		gameUITouchEvent.EventAction = TouchEventAction.Click;
		AddTouchEventToGameUI(gameUITouchEvent);
	}
}
