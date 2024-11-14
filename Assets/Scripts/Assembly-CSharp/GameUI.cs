using System.Collections.Generic;
using UnityEngine;

public abstract class GameUI : MonoBehaviour
{
	private GameUIStatus mUIStatus;

	private GameUIBundle mGameUIBundle;

	private GameSubUI mGameSubUI = EmptyGameSubUI.Self;

	private GameSubUI mNextGameSubUI;

	private bool mIsRemoveSubUI;

	private GameUIListener mGameUIListener = EmptyGameUIlistener.Self;

	private List<GameUITouchEvent> mTouchList = new List<GameUITouchEvent>();

	private List<ComponentUI> mComponentList = new List<ComponentUI>();

	private bool mIsStart;

	protected bool mIsHide;

	public GameUIStatus UIStatus
	{
		get
		{
			return mUIStatus;
		}
	}

	public void InitUI(GameUIStatus uiStatus)
	{
		mIsRemoveSubUI = false;
		mUIStatus = uiStatus;
	}

	public void SetListener(GameUIListener listener)
	{
		if (listener == null)
		{
			mGameUIListener = EmptyGameUIlistener.Self;
		}
		else
		{
			mGameUIListener = listener;
		}
	}

	public void SetGameUIBundle(GameUIBundle bundle)
	{
		mGameUIBundle = bundle;
	}

	protected virtual void OnInit(GameUIBundle bundle)
	{
	}

	public void LoadSubUI(GameSubUI gameSubUI)
	{
		if (gameSubUI != null)
		{
			mNextGameSubUI = gameSubUI;
		}
	}

	public void RemoveSubUI()
	{
		mIsRemoveSubUI = true;
	}

	private void Update()
	{
		if (!mIsStart)
		{
			if (!IsCanInit())
			{
				return;
			}
			OnInit(mGameUIBundle);
			mGameUIBundle = null;
			mIsStart = true;
			{
				foreach (ComponentUI mComponent in mComponentList)
				{
					mComponent.Init();
				}
				return;
			}
		}
		OnUIUpdate();
		if (mIsRemoveSubUI)
		{
			mGameSubUI.Destroy();
			mGameSubUI = EmptyGameSubUI.Self;
			mIsRemoveSubUI = false;
		}
		if (mNextGameSubUI != null)
		{
			mGameSubUI.Destroy();
			mGameSubUI = mNextGameSubUI;
			mGameSubUI.Create(this);
			mNextGameSubUI = null;
		}
		else
		{
			mGameSubUI.Update();
		}
		foreach (GameUITouchEvent mTouch in mTouchList)
		{
			mGameUIListener.OnTouch(mTouch);
			mGameSubUI.GetListener().OnTouch(mTouch);
		}
		mTouchList.Clear();
	}

	protected virtual void OnUIUpdate()
	{
	}

	private void OnDestroy()
	{
		mTouchList.Clear();
		mComponentList.Clear();
		mGameSubUI.Destroy();
		OnUIDestroy();
		mGameSubUI = null;
		mGameUIListener = null;
	}

	protected virtual void OnUIDestroy()
	{
	}

	public void Show()
	{
		if (mIsStart)
		{
			mGameSubUI.ResumeFromHide();
			OnShow();
			mIsHide = false;
		}
	}

	protected virtual void OnShow()
	{
	}

	public void Hide()
	{
		if (mIsStart)
		{
			mGameSubUI.Hide();
			OnHide();
			mIsHide = true;
		}
	}

	public bool IsHide()
	{
		return mIsHide;
	}

	protected virtual void OnHide()
	{
	}

	public void AddTouchEvent(GameUITouchEvent touchEvent)
	{
		mTouchList.Add(touchEvent);
	}

	protected virtual bool IsCanInit()
	{
		return true;
	}

	public void AddComponent(ComponentUI componentUI)
	{
		if (componentUI != null && !mComponentList.Contains(componentUI))
		{
			mComponentList.Add(componentUI);
		}
	}
}
