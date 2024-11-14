using AnimationOrTween;
using UnityEngine;

public class UITweenX : MonoBehaviour
{
	public enum TimeToPlay
	{
		None = 0,
		Start = 1,
		OnEnable = 2
	}

	public delegate void VoidAction();

	public GameObject tweenTarget;

	public int tweenGroup;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public bool includeChildren;

	public TimeToPlay timeToPlay;

	private VoidAction act;

	private bool forward;

	private UITweener[] mTweens;

	private void Start()
	{
		if (tweenTarget == null)
		{
			tweenTarget = base.gameObject;
		}
		if (timeToPlay == TimeToPlay.Start)
		{
			Play();
		}
	}

	private void OnEnable()
	{
		if (timeToPlay == TimeToPlay.OnEnable)
		{
			Play();
		}
	}

	private void Update()
	{
		if (mTweens == null)
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		int i = 0;
		for (int num = mTweens.Length; i < num; i++)
		{
			UITweener uITweener = mTweens[i];
			if (uITweener.enabled)
			{
				flag = false;
				break;
			}
			if (uITweener.direction != (Direction)disableWhenFinished)
			{
				flag2 = false;
			}
		}
		if (flag)
		{
			mTweens = null;
			if (flag2)
			{
				tweenTarget.SetActive(false);
			}
			if (act != null)
			{
				act();
			}
		}
	}

	public void PlayForward(int group, VoidAction doAfterFinish)
	{
		tweenGroup = group;
		playDirection = Direction.Forward;
		Play(doAfterFinish);
	}

	public void PlayReverse(int group, VoidAction doAfterFinish)
	{
		tweenGroup = group;
		playDirection = Direction.Reverse;
		Play(doAfterFinish);
	}

	public void PlayForward()
	{
		PlayForward(tweenGroup, null);
	}

	public void PlayReverse()
	{
		PlayReverse(tweenGroup, null);
	}

	public void PlayForward(VoidAction doAfterFinish)
	{
		PlayForward(tweenGroup, doAfterFinish);
	}

	public void PlayReverse(VoidAction doAfterFinish)
	{
		PlayReverse(tweenGroup, doAfterFinish);
	}

	public void Play()
	{
		Play(null);
	}

	public void Play(VoidAction doAfterFinish)
	{
		act = doAfterFinish;
		forward = true;
		GameObject gameObject = ((!(tweenTarget == null)) ? tweenTarget : base.gameObject);
		if (!gameObject.activeSelf)
		{
			if (ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			gameObject.SetActive(true);
		}
		mTweens = ((!includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (mTweens.Length == 0)
		{
			if (disableWhenFinished != 0)
			{
				tweenTarget.SetActive(false);
			}
			return;
		}
		bool flag = false;
		if (playDirection == Direction.Reverse)
		{
			forward = !forward;
		}
		int i = 0;
		for (int num = mTweens.Length; i < num; i++)
		{
			UITweener uITweener = mTweens[i];
			if (uITweener.tweenGroup == tweenGroup)
			{
				if (!flag && !gameObject.activeSelf)
				{
					flag = true;
					gameObject.SetActive(true);
				}
				if (playDirection == Direction.Toggle)
				{
					uITweener.Toggle();
				}
				else
				{
					uITweener.Play(forward);
				}
				if (resetOnPlay)
				{
					uITweener.Reset();
				}
			}
		}
	}

	public void Reset()
	{
		GameObject gameObject = ((!(tweenTarget == null)) ? tweenTarget : base.gameObject);
		mTweens = ((!includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		int i = 0;
		for (int num = mTweens.Length; i < num; i++)
		{
			UITweener uITweener = mTweens[i];
			if (uITweener.tweenGroup == tweenGroup)
			{
				uITweener.Reset();
			}
		}
		mTweens = null;
	}
}
