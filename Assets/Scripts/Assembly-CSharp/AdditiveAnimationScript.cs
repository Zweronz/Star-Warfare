using UnityEngine;

public class AdditiveAnimationScript : MonoBehaviour
{
	public float weight = 0.5f;

	private void Start()
	{
		string text = "shoot45";
		string text2 = "stand_shoot";
		AnimationState animationState = base.GetComponent<Animation>()[text];
		animationState.blendMode = AnimationBlendMode.Additive;
		animationState.layer = 10;
		animationState.wrapMode = WrapMode.Loop;
		animationState.enabled = true;
		animationState.weight = 1f;
		AnimationState animationState2 = base.GetComponent<Animation>()[text2];
		animationState2.layer = 10;
		animationState2.wrapMode = WrapMode.Loop;
		animationState2.weight = 1f;
		base.GetComponent<Animation>().Play(text);
		base.GetComponent<Animation>().Play(text2);
	}

	private void Update()
	{
	}
}
