using System;
using UnityEngine;

public class WaitVSRebirthHUD : UIPanelX
{
	protected UIImage mBackgroundImage;

	protected UIImage mRespawnImage;

	protected UIImage mAutoBalanceImage;

	protected UIImage mNoMithrilImage;

	protected UIImage mSpendMithrilImage;

	protected UIImage mUseUpImage;

	protected UIClickButton mReviveButton;

	protected UINumeric mCountdownNumeric;

	protected UINumeric mRemainingTimesNumeric;

	protected int mMaxRiviveTimes;

	protected int mUsedRiviveTimes;

	protected UserState mUserState;

	protected static int[] MAX_RIVIVE_TIMES = new int[3] { 5, 10, 15 };

	protected bool mUseMithril;

	protected bool mIsAutoBalanced;

	public WaitVSRebirthHUD()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI != null)
		{
			mUserState = GameApp.GetInstance().GetUserState();
			mMaxRiviveTimes = 0;
			mUsedRiviveTimes = 0;
			mUseMithril = false;
			mIsAutoBalanced = false;
			int num = -1;
			num = ((Lobby.GetInstance().WinCondition != 0) ? Array.IndexOf(UIConstant.WIN_VALUE_FOR_SCORE, Lobby.GetInstance().WinValue.ToString()) : Array.IndexOf(UIConstant.WIN_VALUE_FOR_TIMER, Lobby.GetInstance().WinValue.ToString()));
			if (num >= 0 && num < MAX_RIVIVE_TIMES.Length)
			{
				mMaxRiviveTimes = MAX_RIVIVE_TIMES[num];
			}
			mBackgroundImage = new UIImage();
			mBackgroundImage.AddObject(unitUI, 10, 0);
			mBackgroundImage.Rect = mBackgroundImage.GetObjectRect();
			mBackgroundImage.SetParent(this);
			mRespawnImage = new UIImage();
			mRespawnImage.AddObject(unitUI, 10, 1);
			mRespawnImage.Rect = mRespawnImage.GetObjectRect();
			mRespawnImage.SetParent(this);
			mAutoBalanceImage = new UIImage();
			mAutoBalanceImage.AddObject(unitUI, 10, 10);
			mAutoBalanceImage.Rect = mAutoBalanceImage.GetObjectRect();
			mAutoBalanceImage.SetParent(this);
			mNoMithrilImage = new UIImage();
			mNoMithrilImage.AddObject(unitUI, 10, 2);
			mNoMithrilImage.Rect = mNoMithrilImage.GetObjectRect();
			mNoMithrilImage.SetParent(this);
			mSpendMithrilImage = new UIImage();
			mSpendMithrilImage.AddObject(unitUI, 10, 3);
			mSpendMithrilImage.Rect = mSpendMithrilImage.GetObjectRect();
			mSpendMithrilImage.SetParent(this);
			mUseUpImage = new UIImage();
			mUseUpImage.AddObject(unitUI, 10, 4);
			mUseUpImage.Rect = mUseUpImage.GetObjectRect();
			mUseUpImage.SetParent(this);
			byte[] module = new byte[2] { 6, 7 };
			byte[] module2 = new byte[2] { 5, 7 };
			mReviveButton = new UIClickButton();
			mReviveButton.AddObject(UIButtonBase.State.Normal, unitUI, 10, module);
			mReviveButton.AddObject(UIButtonBase.State.Pressed, unitUI, 10, module2);
			mReviveButton.Rect = unitUI.GetModulePositionRect(0, 10, 6);
			mReviveButton.SetParent(this);
			mCountdownNumeric = new UINumeric();
			mCountdownNumeric.AlignStyle = UINumeric.enAlignStyle.center;
			mCountdownNumeric.SpacingOffsetX = 1f;
			mCountdownNumeric.SetNumeric(unitUI, 1, Convert.ToString(11f));
			mCountdownNumeric.Rect = unitUI.GetModulePositionRect(0, 10, 9);
			mCountdownNumeric.SetParent(this);
			mRemainingTimesNumeric = new UINumeric();
			mRemainingTimesNumeric.AlignStyle = UINumeric.enAlignStyle.center;
			mRemainingTimesNumeric.SpacingOffsetX = -6f;
			mRemainingTimesNumeric.Rect = unitUI.GetModulePositionRect(0, 10, 8);
			mRemainingTimesNumeric.SetParent(this);
			Hide();
		}
	}

	public override void Draw()
	{
		if (mBackgroundImage != null && mBackgroundImage.Visible)
		{
			mBackgroundImage.Draw();
		}
		if (mRespawnImage != null && mRespawnImage.Visible)
		{
			mRespawnImage.Draw();
		}
		if (mAutoBalanceImage != null && mAutoBalanceImage.Visible)
		{
			mAutoBalanceImage.Draw();
		}
		if (mCountdownNumeric != null && mCountdownNumeric.Visible)
		{
			mCountdownNumeric.Draw();
		}
		if (mNoMithrilImage != null && mNoMithrilImage.Visible)
		{
			mNoMithrilImage.Draw();
		}
		if (mSpendMithrilImage != null && mSpendMithrilImage.Visible)
		{
			mSpendMithrilImage.Draw();
		}
		if (mUseUpImage != null && mUseUpImage.Visible)
		{
			mUseUpImage.Draw();
		}
		if (mReviveButton != null && mReviveButton.Visible)
		{
			mReviveButton.Draw();
		}
		if (mRemainingTimesNumeric != null && mRemainingTimesNumeric.Visible)
		{
			mRemainingTimesNumeric.Draw();
		}
	}

	public void Destroy()
	{
		if (mBackgroundImage != null)
		{
			mBackgroundImage.Free();
			mBackgroundImage = null;
		}
		if (mRespawnImage != null)
		{
			mRespawnImage.Free();
			mRespawnImage = null;
		}
		if (mAutoBalanceImage != null)
		{
			mAutoBalanceImage.Free();
			mAutoBalanceImage = null;
		}
		if (mNoMithrilImage != null)
		{
			mNoMithrilImage.Free();
			mNoMithrilImage = null;
		}
		if (mSpendMithrilImage != null)
		{
			mSpendMithrilImage.Free();
			mSpendMithrilImage = null;
		}
		if (mUseUpImage != null)
		{
			mUseUpImage.Free();
			mUseUpImage = null;
		}
		if (mCountdownNumeric != null)
		{
			mCountdownNumeric.Free();
			mCountdownNumeric = null;
		}
		if (mRemainingTimesNumeric != null)
		{
			mRemainingTimesNumeric.Free();
			mRemainingTimesNumeric = null;
		}
	}

	public override void Show()
	{
		base.Show();
		if (mBackgroundImage != null)
		{
			mBackgroundImage.Visible = true;
		}
		if (mIsAutoBalanced)
		{
			if (mRespawnImage != null)
			{
				mRespawnImage.Visible = false;
			}
			if (mAutoBalanceImage != null)
			{
				mAutoBalanceImage.Visible = true;
			}
		}
		else
		{
			if (mRespawnImage != null)
			{
				mRespawnImage.Visible = true;
			}
			if (mAutoBalanceImage != null)
			{
				mAutoBalanceImage.Visible = false;
			}
		}
		if (mCountdownNumeric != null)
		{
			mCountdownNumeric.Visible = true;
		}
		if (mUserState.GetMithril() > 0)
		{
			if (mUsedRiviveTimes < mMaxRiviveTimes)
			{
				if (mSpendMithrilImage != null)
				{
					mSpendMithrilImage.Visible = true;
				}
				if (mUseUpImage != null)
				{
					mUseUpImage.Visible = false;
				}
				if (mReviveButton != null)
				{
					mReviveButton.Visible = true;
					mReviveButton.Enable = true;
				}
				if (mRemainingTimesNumeric != null)
				{
					mRemainingTimesNumeric.Visible = true;
				}
			}
			else
			{
				if (mUseUpImage != null)
				{
					mUseUpImage.Visible = true;
				}
				if (mSpendMithrilImage != null)
				{
					mSpendMithrilImage.Visible = false;
				}
				if (mReviveButton != null)
				{
					mReviveButton.Visible = false;
					mReviveButton.Enable = false;
				}
				if (mRemainingTimesNumeric != null)
				{
					mRemainingTimesNumeric.Visible = false;
				}
			}
			if (mNoMithrilImage != null)
			{
				mNoMithrilImage.Visible = false;
			}
		}
		else
		{
			if (mNoMithrilImage != null)
			{
				mNoMithrilImage.Visible = true;
			}
			if (mSpendMithrilImage != null)
			{
				mSpendMithrilImage.Visible = false;
			}
			if (mUseUpImage != null)
			{
				mUseUpImage.Visible = false;
			}
			if (mReviveButton != null)
			{
				mReviveButton.Visible = false;
				mReviveButton.Enable = false;
			}
			if (mRemainingTimesNumeric != null)
			{
				mRemainingTimesNumeric.Visible = false;
			}
		}
		SetRiviveRemainingTimes();
	}

	public new void Hide()
	{
		mIsAutoBalanced = false;
		if (mBackgroundImage != null)
		{
			mBackgroundImage.Visible = false;
		}
		if (mRespawnImage != null)
		{
			mRespawnImage.Visible = false;
		}
		if (mAutoBalanceImage != null)
		{
			mAutoBalanceImage.Visible = false;
		}
		if (mCountdownNumeric != null)
		{
			mCountdownNumeric.Visible = false;
		}
		if (mNoMithrilImage != null)
		{
			mNoMithrilImage.Visible = false;
		}
		if (mSpendMithrilImage != null)
		{
			mSpendMithrilImage.Visible = false;
		}
		if (mUseUpImage != null)
		{
			mUseUpImage.Visible = false;
		}
		if (mReviveButton != null)
		{
			mReviveButton.Visible = false;
			mReviveButton.Enable = false;
		}
		if (mRemainingTimesNumeric != null)
		{
			mRemainingTimesNumeric.Visible = false;
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (mReviveButton != null && mReviveButton.Enable && mReviveButton.HandleInput(touch))
		{
			return true;
		}
		return false;
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
				mUsedRiviveTimes++;
				mUserState.OperInfo.MithrilRebirthTime++;
			}
		}
	}

	public void SetCountdownValue(string time)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI != null)
		{
			mCountdownNumeric.SetNumeric(unitUI, 1, time);
		}
	}

	protected void SetRiviveRemainingTimes()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[17];
		if (unitUI != null && mUserState.GetMithril() > 0 && mUsedRiviveTimes < mMaxRiviveTimes)
		{
			string numeric = Mathf.Min(mUserState.GetMithril(), mMaxRiviveTimes - mUsedRiviveTimes) + "/" + mMaxRiviveTimes;
			mRemainingTimesNumeric.SetNumeric(unitUI, 0, numeric);
		}
	}
}
