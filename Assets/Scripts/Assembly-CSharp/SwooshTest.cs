using UnityEngine;

public class SwooshTest : MonoBehaviour
{
	[SerializeField]
	private AnimationClip _animation;

	private AnimationState _animationState;

	[SerializeField]
	private int _start;

	[SerializeField]
	private int _end;

	private float _startN;

	private float _endN;

	private float _time;

	private float _prevTime;

	private float _prevAnimTime;

	[SerializeField]
	private MeleeWeaponTrail _trail;

	private bool _firstFrame = true;

	private void Start()
	{
		float num = _animation.frameRate * _animation.length;
		_startN = (float)_start / num;
		_endN = (float)_end / num;
		_animationState = base.GetComponent<Animation>()[_animation.name];
		_trail.Emit = false;
	}

	private void Update()
	{
		_time += _animationState.normalizedTime - _prevAnimTime;
		if (_time > 1f || _firstFrame)
		{
			if (!_firstFrame)
			{
				_time -= 1f;
			}
			_firstFrame = false;
		}
		if (_prevTime < _startN && _time >= _startN)
		{
			_trail.Emit = true;
		}
		else if (_prevTime < _endN && _time >= _endN)
		{
			_trail.Emit = false;
		}
		_prevTime = _time;
		_prevAnimTime = _animationState.normalizedTime;
	}
}
