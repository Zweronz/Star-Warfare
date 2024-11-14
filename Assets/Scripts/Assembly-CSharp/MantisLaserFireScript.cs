using UnityEngine;

public class MantisLaserFireScript : MonoBehaviour
{
	public Transform mouthTransform;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = mouthTransform.position;
		base.transform.Translate(-0.3f * mouthTransform.right);
		base.transform.Translate(0.1f * mouthTransform.up);
	}
}
