using UnityEngine;

public class AnimationMixScript : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Animation>().AddClip(base.GetComponent<Animation>()["shoot"].clip, "shootUpperBody");
		Transform transform = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		if (transform == null)
		{
		}
		base.GetComponent<Animation>()["shootUpperBody"].AddMixingTransform(transform);
		base.GetComponent<Animation>()["run"].layer = -1;
		base.GetComponent<Animation>().Play("run");
	}

	private void Update()
	{
		base.GetComponent<Animation>().CrossFade("shootUpperBody");
	}
}
