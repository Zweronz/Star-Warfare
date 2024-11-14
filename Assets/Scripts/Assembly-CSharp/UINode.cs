using UnityEngine;

public class UINode
{
	private int mVisibleFlag = -1;

	public Transform trans;

	public UIWidget widget;

	private bool mLastActive;

	private Vector3 mLastPos;

	private Quaternion mLastRot;

	private Vector3 mLastScale;

	private GameObject mGo;

	private float mLastAlpha;

	public int changeFlag = -1;

	public int visibleFlag
	{
		get
		{
			return (!(widget != null)) ? mVisibleFlag : widget.visibleFlag;
		}
		set
		{
			if (widget != null)
			{
				widget.visibleFlag = value;
			}
			else
			{
				mVisibleFlag = value;
			}
		}
	}

	public UINode(Transform t)
	{
		trans = t;
		mLastPos = trans.localPosition;
		mLastRot = trans.localRotation;
		mLastScale = trans.localScale;
		mGo = t.gameObject;
	}

	public bool HasChanged()
	{
		bool flag = NGUITools.GetActive(mGo) && (widget == null || (widget.enabled && widget.isVisible));
		bool flag2 = mLastActive != flag;
		if (widget != null)
		{
			float finalAlpha = widget.finalAlpha;
			if (finalAlpha != mLastAlpha)
			{
				mLastAlpha = finalAlpha;
				flag2 = true;
			}
		}
		if (!flag2 && !trans.hasChanged)
		{
			return false;
		}
		trans.hasChanged = false;
		flag2 = true;
		if (flag2 || (flag && (mLastPos != trans.localPosition || mLastRot != trans.localRotation || mLastScale != trans.localScale)))
		{
			mLastActive = flag;
			mLastPos = trans.localPosition;
			mLastRot = trans.localRotation;
			mLastScale = trans.localScale;
			return true;
		}
		return flag2;
	}
}
