using UnityEngine;

public class ButtonUI : ComponentUI
{
	[SerializeField]
	private TouchEventID eventID;

	public KeyCode keyCode;

	private void Start()
	{
		AddButtonListener(base.gameObject);
	}

	private void Update()
	{
		if (keyCode != KeyCode.None)
		{
			if (Input.GetKeyDown(keyCode))
			{
				OnClickThumb(null);
			}
		}
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
