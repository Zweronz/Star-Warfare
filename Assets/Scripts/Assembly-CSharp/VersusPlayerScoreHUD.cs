using UnityEngine;

public class VersusPlayerScoreHUD : UIPanelX
{
	protected UIImage mPlayerImage;

	protected UIImage mHitImage;

	protected UINumeric mScoreNumeric;

	protected int mFrameIndex;

	protected Rect mStartRect;

	protected Color mScoreColor;

	protected bool mShowAimImage;

	public VersusPlayerScoreHUD(Rect startRect, int seatID, Color scoreColor, bool showAimImage)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[27];
		if (unitUI != null && unitUI2 != null)
		{
			mStartRect = startRect;
			mScoreColor = scoreColor;
			base.Show();
		}
	}

	public override void Draw()
	{
		if (mPlayerImage != null)
		{
			mPlayerImage.Draw();
		}
		if (mHitImage != null && mShowAimImage)
		{
			mHitImage.Draw();
		}
		if (mScoreNumeric != null)
		{
			mScoreNumeric.Draw();
		}
	}

	public void Destroy()
	{
		if (mPlayerImage != null)
		{
			mPlayerImage.Free();
			mPlayerImage = null;
		}
		if (mHitImage != null)
		{
			mHitImage.Free();
			mHitImage = null;
		}
		if (mScoreNumeric != null)
		{
			mScoreNumeric.Free();
			mScoreNumeric = null;
		}
	}

	public void SetScore(int score)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI != null && mScoreNumeric != null)
		{
			mScoreNumeric.SetNumeric(unitUI, 4, "x" + score);
			mScoreNumeric.SetColor(mScoreColor);
		}
	}

	public void SetScore(int score, int winScore)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI != null && mScoreNumeric != null)
		{
			mScoreNumeric.SetNumeric(unitUI, 4, score + "/" + winScore);
			mScoreNumeric.SetColor(mScoreColor);
		}
	}

	public void SetIconID(int seatID)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[27];
		if (unitUI != null && mPlayerImage != null)
		{
			mPlayerImage.SetTexture(unitUI, 1, seatID);
			mPlayerImage.Rect = mStartRect;
		}
	}

	public void SetIconColor(Color color)
	{
		mPlayerImage.SetColor(color);
	}
}
