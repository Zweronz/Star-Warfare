using UnityEngine;

public class AnimationTestScript : MonoBehaviour
{
	private GameObject playerObject;

	private void Start()
	{
	}

	private void Update()
	{
		playerObject.GetComponent<Animation>().CrossFade(AnimationString.Attack + "_rifle");
	}
}
