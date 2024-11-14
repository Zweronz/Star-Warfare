using UnityEngine;

public class AnimationScript : MonoBehaviour
{
	public static string Idle = "run";

	public static string Run = "run";

	public static string Attack = "attack";

	public static string RunAttack = "run";

	public static string Hurt = "hurt";

	protected bool attacked;

	protected float lastAttackTime;

	private void Start()
	{
	}

	public bool CouldMakeNextAttack()
	{
		if (Time.time - lastAttackTime > 1.5f)
		{
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (base.animation.IsPlaying(Hurt) && base.animation[Hurt].time >= base.animation[Hurt].clip.length)
		{
			base.animation.CrossFade(Idle);
		}
		if (base.animation.IsPlaying(Attack) && base.animation[Attack].time >= base.animation[Attack].clip.length)
		{
			base.animation.CrossFade(Idle);
		}
		if (!base.animation.IsPlaying(Hurt))
		{
		}
	}
}
