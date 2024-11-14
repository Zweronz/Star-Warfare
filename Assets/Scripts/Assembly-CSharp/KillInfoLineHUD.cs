using UnityEngine;

public class KillInfoLineHUD : UIPanelX
{
	public UIImage mKillerImage;

	public UIImage mKilledImage;

	public UIImage mWeaponImage;

	public float mAlpha = 1f;

	public Color mKillerColor = default(Color);

	public Color mKilledColor = default(Color);

	public override void Draw()
	{
		if (mKillerImage != null)
		{
			mKillerImage.Draw();
		}
		if (mWeaponImage != null)
		{
			mWeaponImage.Draw();
		}
		if (mKilledImage != null)
		{
			mKilledImage.Draw();
		}
	}

	public void Destroy()
	{
		if (mKillerImage != null)
		{
			mKillerImage.Free();
			mKillerImage = null;
		}
		if (mWeaponImage != null)
		{
			mWeaponImage.Free();
			mWeaponImage = null;
		}
		if (mKilledImage != null)
		{
			mKilledImage.Free();
			mKilledImage = null;
		}
	}
}
