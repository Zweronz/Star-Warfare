using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (base.animation.IsPlaying(AnimationScript.Hurt) && base.animation[AnimationScript.Hurt].time >= base.animation[AnimationScript.Hurt].clip.length)
		{
			base.animation.CrossFade(AnimationScript.Run);
		}
		if (base.animation.IsPlaying(AnimationScript.Attack) && base.animation[AnimationScript.Attack].time >= base.animation[AnimationScript.Attack].clip.length)
		{
			base.animation.CrossFade(AnimationScript.Idle);
		}
	}
}
