using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{
	private GameObject playerObject;

	private void Start()
	{
	}

	private void Update()
	{
		playerObject.animation.CrossFade(AnimationString.Attack + "_rifle");
	}
}
