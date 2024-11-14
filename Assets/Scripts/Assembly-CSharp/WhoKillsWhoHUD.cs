using UnityEngine;

public class WhoKillsWhoHUD : UIPanelX
{
	private const float START_X = 5f;

	private const float SPACE_X = 0f;

	private const float SPACE_Y = 5f;

	private const float NAME_START_WIDTH = 40f;

	private const float WEAPON_START_WIDTH = 40f;

	private const float ICON_START_HEIGHT = 40f;

	private const float SCALE_RATIO = 0.8f;

	private const float MIN_ALPHA = 0.4f;

	private KillInfoLineHUD[] mKillInfoArray;

	private Rect[] mKillerImageRectArray;

	private Rect[] mWeaponImageRectArray;

	private Rect[] mKilledImageRectArray;

	private int mMaxLineNumber;

	private float mFadeOutSpeed;

	private int mVsFrameIndex;

	private bool mIsTeamMode;

	public WhoKillsWhoHUD(int maxLineNumber, float fadeOutSpeed, int frameIndex, int vsFrameIndex)
	{
		mMaxLineNumber = maxLineNumber;
		mFadeOutSpeed = fadeOutSpeed;
		mVsFrameIndex = vsFrameIndex;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		float num = unitUI.GetModulePositionRect(0, mVsFrameIndex, 5).top;
		mKillInfoArray = new KillInfoLineHUD[mMaxLineNumber];
		mKillerImageRectArray = new Rect[mMaxLineNumber];
		mWeaponImageRectArray = new Rect[mMaxLineNumber];
		mKilledImageRectArray = new Rect[mMaxLineNumber];
		float num2 = 0f;
		float num3 = 40f;
		float num4 = 40f;
		float num5 = 40f;
		for (int i = 0; i < mMaxLineNumber; i++)
		{
			mKillInfoArray[i] = null;
			mKillerImageRectArray[i] = new Rect(5f, num, num3, num5);
			mWeaponImageRectArray[i] = new Rect(5f + num3 + num2, num, num4, num5);
			mKilledImageRectArray[i] = new Rect(5f + num3 + num2 + num4 + num2, num, num3, num5);
			num += 4f + num5;
			num2 *= 0.8f;
			num3 *= 0.8f;
			num4 *= 0.8f;
			num5 *= 0.8f;
		}
		mIsTeamMode = GameApp.GetInstance().GetGameMode().IsTeamMode();
		base.Show();
	}

	public override void Draw()
	{
		for (int i = 0; i < mMaxLineNumber; i++)
		{
			if (mKillInfoArray[i] != null)
			{
				mKillInfoArray[i].Draw();
			}
		}
	}

	public void AddWhoKillsWho(int killerID, HUDAction action, int killedID)
	{
		if (mMaxLineNumber <= 0)
		{
			return;
		}
		if (mKillInfoArray[mMaxLineNumber - 1] != null)
		{
			mKillInfoArray[mMaxLineNumber - 1].Destroy();
			mKillInfoArray[mMaxLineNumber - 1] = null;
		}
		for (int num = mMaxLineNumber - 1; num > 0; num--)
		{
			mKillInfoArray[num] = mKillInfoArray[num - 1];
			if (mKillInfoArray[num] != null)
			{
				mKillInfoArray[num].mKillerImage.Rect = mKillerImageRectArray[num];
				mKillInfoArray[num].mKillerImage.SetSize(new Vector2(mKillerImageRectArray[num].width, mKillerImageRectArray[num].height));
				mKillInfoArray[num].mWeaponImage.Rect = mWeaponImageRectArray[num];
				mKillInfoArray[num].mWeaponImage.SetSize(new Vector2(mWeaponImageRectArray[num].width, mWeaponImageRectArray[num].height));
				mKillInfoArray[num].mKilledImage.Rect = mKilledImageRectArray[num];
				mKillInfoArray[num].mKilledImage.SetSize(new Vector2(mKilledImageRectArray[num].width, mKilledImageRectArray[num].height));
			}
		}
		UnitUI ui = Res2DManager.GetInstance().vUI[0];
		UnitUI ui2 = Res2DManager.GetInstance().vUI[27];
		KillInfoLineHUD killInfoLineHUD = new KillInfoLineHUD();
		killInfoLineHUD.mKillerImage = new UIImage();
		killInfoLineHUD.mKillerImage.SetTexture(ui2, 0, killerID);
		killInfoLineHUD.mKillerImage.Rect = mKillerImageRectArray[0];
		killInfoLineHUD.mKillerImage.SetSize(new Vector2(mKillerImageRectArray[0].width, mKillerImageRectArray[0].height));
		killInfoLineHUD.mKillerImage.SetParent(killInfoLineHUD);
		killInfoLineHUD.mWeaponImage = new UIImage();
		switch (action)
		{
		case HUDAction.KILL:
			killInfoLineHUD.mWeaponImage.SetTexture(ui, mVsFrameIndex, 5);
			break;
		case HUDAction.SECURE_FLAG:
			killInfoLineHUD.mWeaponImage.SetTexture(ui, mVsFrameIndex, 12);
			break;
		case HUDAction.DROP_FLAG:
			killInfoLineHUD.mWeaponImage.SetTexture(ui, mVsFrameIndex, 13);
			break;
		case HUDAction.CATCH_FLAG:
			killInfoLineHUD.mWeaponImage.SetTexture(ui, mVsFrameIndex, 14);
			break;
		}
		killInfoLineHUD.mWeaponImage.Rect = mWeaponImageRectArray[0];
		killInfoLineHUD.mWeaponImage.SetSize(new Vector2(mWeaponImageRectArray[0].width, mWeaponImageRectArray[0].height));
		killInfoLineHUD.mWeaponImage.SetParent(killInfoLineHUD);
		killInfoLineHUD.mKilledImage = new UIImage();
		killInfoLineHUD.mKilledImage.SetTexture(ui2, 0, killedID);
		killInfoLineHUD.mKilledImage.Rect = mKilledImageRectArray[0];
		killInfoLineHUD.mKilledImage.SetSize(new Vector2(mKilledImageRectArray[0].width, mKilledImageRectArray[0].height));
		killInfoLineHUD.mKilledImage.SetParent(killInfoLineHUD);
		if (mIsTeamMode)
		{
			killInfoLineHUD.mKillerColor = UIConstant.COLOR_TEAM_PLAYER_ICONS[killerID / 4];
			if (action == HUDAction.KILL)
			{
				killInfoLineHUD.mKilledColor = UIConstant.COLOR_TEAM_PLAYER_ICONS[killedID / 4];
			}
			else
			{
				killInfoLineHUD.mKilledColor = killInfoLineHUD.mKillerColor;
			}
		}
		else
		{
			killInfoLineHUD.mKillerColor = UIConstant.COLOR_PLAYER_ICONS[killerID];
			if (action == HUDAction.KILL)
			{
				killInfoLineHUD.mKilledColor = UIConstant.COLOR_PLAYER_ICONS[killedID];
			}
			else
			{
				killInfoLineHUD.mKilledColor = killInfoLineHUD.mKillerColor;
			}
		}
		killInfoLineHUD.mKillerImage.SetColor(killInfoLineHUD.mKillerColor);
		killInfoLineHUD.mKilledImage.SetColor(killInfoLineHUD.mKilledColor);
		killInfoLineHUD.mAlpha = 1f;
		killInfoLineHUD.SetParent(this);
		killInfoLineHUD.Show();
		mKillInfoArray[0] = killInfoLineHUD;
	}

	public void UpdateAlpha()
	{
		for (int i = 0; i < mMaxLineNumber; i++)
		{
			if (mKillInfoArray[i] != null)
			{
				if (mKillInfoArray[i].mAlpha > 0f)
				{
					mKillInfoArray[i].mKillerImage.SetColor(new Color(mKillInfoArray[i].mKillerColor.r, mKillInfoArray[i].mKillerColor.g, mKillInfoArray[i].mKillerColor.b, mKillInfoArray[i].mAlpha));
					mKillInfoArray[i].mKilledImage.SetColor(new Color(mKillInfoArray[i].mKilledColor.r, mKillInfoArray[i].mKilledColor.g, mKillInfoArray[i].mKilledColor.b, mKillInfoArray[i].mAlpha));
					mKillInfoArray[i].mWeaponImage.SetColor(new Color(1f, 1f, 1f, mKillInfoArray[i].mAlpha));
					mKillInfoArray[i].mAlpha -= mFadeOutSpeed * Time.deltaTime;
				}
				else
				{
					mKillInfoArray[i].Destroy();
					mKillInfoArray[i] = null;
				}
			}
		}
	}

	public void Destroy()
	{
		for (int i = 0; i < mMaxLineNumber; i++)
		{
			if (mKillInfoArray[i] != null)
			{
				mKillInfoArray[i].Destroy();
				mKillInfoArray[i] = null;
			}
		}
	}
}
