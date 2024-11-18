using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (base.GetComponent<Animation>().IsPlaying(AnimationScript.Hurt) && base.GetComponent<Animation>()[AnimationScript.Hurt].time >= base.GetComponent<Animation>()[AnimationScript.Hurt].clip.length)
		{
			base.GetComponent<Animation>().CrossFade(AnimationScript.Run);
		}
		if (base.GetComponent<Animation>().IsPlaying(AnimationScript.Attack) && base.GetComponent<Animation>()[AnimationScript.Attack].time >= base.GetComponent<Animation>()[AnimationScript.Attack].clip.length)
		{
			base.GetComponent<Animation>().CrossFade(AnimationScript.Idle);
		}
	}
}
