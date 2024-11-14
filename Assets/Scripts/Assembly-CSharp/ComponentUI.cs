using System;
using UnityEngine;

public class ComponentUI : MonoBehaviour
{
	private GameUI mGameUI;

	private void Awake()
	{
		Transform transform = base.transform;
		while (transform != null)
		{
			GameUI component = transform.GetComponent<GameUI>();
			if (component != null)
			{
				mGameUI = component;
				mGameUI.AddComponent(this);
				transform = null;
			}
			else
			{
				transform = transform.parent;
			}
		}
		if (mGameUI == null)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void AddButtonListener(GameObject button)
	{
		UIEventListener uIEventListener = UIEventListener.Get(button);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
		uIEventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onDoubleClick, new UIEventListener.VoidDelegate(OnDoubleClickThumb));
		uIEventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onHover, new UIEventListener.BoolDelegate(OnHoverThumb));
		uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
	}

	public void RemoveButtonListener(GameObject button)
	{
		UIEventListener uIEventListener = UIEventListener.Get(button);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
		uIEventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onDoubleClick, new UIEventListener.VoidDelegate(OnDoubleClickThumb));
		uIEventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onHover, new UIEventListener.BoolDelegate(OnHoverThumb));
		uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
	}

	protected virtual void OnClickThumb(GameObject go)
	{
	}

	protected virtual void OnDoubleClickThumb(GameObject go)
	{
	}

	protected virtual void OnHoverThumb(GameObject go, bool isOver)
	{
	}

	protected virtual void OnDragThumb(GameObject go, Vector2 delta)
	{
	}

	protected virtual void OnPressThumb(GameObject go, bool isPressed)
	{
	}

	protected void AddTouchEventToGameUI(GameUITouchEvent touchEvent)
	{
		mGameUI.AddTouchEvent(touchEvent);
	}

	public void Init()
	{
		OnInit();
	}

	protected virtual void OnInit()
	{
	}
}
