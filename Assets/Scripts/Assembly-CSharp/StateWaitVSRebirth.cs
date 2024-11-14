using System;
using UnityEngine;

public class StateWaitVSRebirth : ComponentUI
{
	public UISprite mBackgroundImage;

	public UISprite mRespawnImage;

	public UISprite mAutoBalanceImage;

	public UISprite mNoMithrilImage;

	public UISprite mSpendMithrilImage;

	public UISprite mUseUpImage;

	public UILabel mCountdownNumeric;

	public UILabel mRemainingTimesNumeric;

	public GameObject mReviveButton;

	protected int mMaxRiviveTimes;

	protected UserState mUserState;

	protected static int[] MAX_RIVIVE_TIMES = new int[3] { 5, 10, 15 };

	protected bool mUseMithril;

	protected bool mIsAutoBalanced;

	private void Start()
	{
		mUserState = GameApp.GetInstance().GetUserState();
		mMaxRiviveTimes = 0;
		mUseMithril = false;
		mIsAutoBalanced = false;
		int num = -1;
		num = ((Lobby.GetInstance().WinCondition != 0) ? Array.IndexOf(UIConstant.WIN_VALUE_FOR_SCORE, Lobby.GetInstance().WinValue.ToString()) : Array.IndexOf(UIConstant.WIN_VALUE_FOR_TIMER, Lobby.GetInstance().WinValue.ToString()));
		if (num >= 0 && num < MAX_RIVIVE_TIMES.Length)
		{
			mMaxRiviveTimes = MAX_RIVIVE_TIMES[num];
		}
		AddButtonListener(mReviveButton);
		Hide();
	}

	public void Show()
	{
		mBackgroundImage.gameObject.SetActive(true);
		if (mIsAutoBalanced)
		{
			mRespawnImage.gameObject.SetActive(false);
			mAutoBalanceImage.gameObject.SetActive(true);
		}
		else
		{
			mRespawnImage.gameObject.SetActive(true);
			mAutoBalanceImage.gameObject.SetActive(false);
		}
		mCountdownNumeric.gameObject.SetActive(true);
		if (mUserState.GetMithril() > 0)
		{
			if (UserStateUI.GetInstance().GetUsedRiviveTimes() < mMaxRiviveTimes)
			{
				mSpendMithrilImage.gameObject.SetActive(true);
				mUseUpImage.gameObject.SetActive(false);
				mReviveButton.gameObject.SetActive(true);
				mRemainingTimesNumeric.gameObject.SetActive(true);
			}
			else
			{
				mUseUpImage.gameObject.SetActive(true);
				mSpendMithrilImage.gameObject.SetActive(false);
				mReviveButton.gameObject.SetActive(false);
				mRemainingTimesNumeric.gameObject.SetActive(false);
			}
			mNoMithrilImage.gameObject.SetActive(false);
		}
		else
		{
			mNoMithrilImage.gameObject.SetActive(true);
			mSpendMithrilImage.gameObject.SetActive(false);
			mUseUpImage.gameObject.SetActive(false);
			mReviveButton.gameObject.SetActive(false);
			mRemainingTimesNumeric.gameObject.SetActive(false);
		}
		SetRiviveRemainingTimes();
	}

	public void Hide()
	{
		mIsAutoBalanced = false;
		mBackgroundImage.gameObject.SetActive(false);
		mRespawnImage.gameObject.SetActive(false);
		mAutoBalanceImage.gameObject.SetActive(false);
		mNoMithrilImage.gameObject.SetActive(false);
		mSpendMithrilImage.gameObject.SetActive(false);
		mUseUpImage.gameObject.SetActive(false);
		mCountdownNumeric.gameObject.SetActive(false);
		mRemainingTimesNumeric.gameObject.SetActive(false);
		mReviveButton.gameObject.SetActive(false);
	}

	public void DoAutoBalance()
	{
		mIsAutoBalanced = true;
	}

	public void DoRebirth()
	{
		mUseMithril = true;
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.SendRebirthRequest();
	}

	public void OnRebirth()
	{
		if (mUseMithril)
		{
			mUseMithril = false;
			if (mUserState.GetMithril() > 0)
			{
				mUserState.BuyWithMithril(1);
				UserStateUI.GetInstance().SetUsedRiviveTimes(UserStateUI.GetInstance().GetUsedRiviveTimes() + 1);
				mUserState.OperInfo.MithrilRebirthTime++;
			}
		}
	}

	public void SetCountdownValue(string time)
	{
		mCountdownNumeric.text = time;
	}

	protected void SetRiviveRemainingTimes()
	{
		if (mUserState.GetMithril() > 0 && UserStateUI.GetInstance().GetUsedRiviveTimes() < mMaxRiviveTimes)
		{
			string text = Mathf.Min(mUserState.GetMithril(), mMaxRiviveTimes - UserStateUI.GetInstance().GetUsedRiviveTimes()) + "/" + mMaxRiviveTimes;
			mRemainingTimesNumeric.text = text;
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		GameUITouchEvent gameUITouchEvent = new GameUITouchEvent();
		gameUITouchEvent.EventID = TouchEventID.HUD_Rebirth;
		gameUITouchEvent.EventAction = TouchEventAction.Click;
		AddTouchEventToGameUI(gameUITouchEvent);
	}
}
