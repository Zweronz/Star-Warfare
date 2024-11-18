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
		if (base.GetComponent<Animation>().IsPlaying(Hurt) && base.GetComponent<Animation>()[Hurt].time >= base.GetComponent<Animation>()[Hurt].clip.length)
		{
			base.GetComponent<Animation>().CrossFade(Idle);
		}
		if (base.GetComponent<Animation>().IsPlaying(Attack) && base.GetComponent<Animation>()[Attack].time >= base.GetComponent<Animation>()[Attack].clip.length)
		{
			base.GetComponent<Animation>().CrossFade(Idle);
		}
		if (!base.GetComponent<Animation>().IsPlaying(Hurt))
		{
		}
	}
}
