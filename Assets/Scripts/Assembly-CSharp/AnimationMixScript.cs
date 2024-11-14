using UnityEngine;

public class AnimationMixScript : MonoBehaviour
{
	private void Start()
	{
		base.animation.AddClip(base.animation["shoot"].clip, "shootUpperBody");
		Transform transform = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		if (transform == null)
		{
		}
		base.animation["shootUpperBody"].AddMixingTransform(transform);
		base.animation["run"].layer = -1;
		base.animation.Play("run");
	}

	private void Update()
	{
		base.animation.CrossFade("shootUpperBody");
	}
}
