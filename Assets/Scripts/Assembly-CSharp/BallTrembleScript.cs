using UnityEngine;

public class BallTrembleScript : MonoBehaviour
{
	public float minScale = 1000f;

	public float maxScale = 4000f;

	public float scaleSpeed = 0.1f;

	protected bool increasing;

	private void Start()
	{
		increasing = true;
	}

	private void Update()
	{
		if (increasing)
		{
			if (base.transform.localScale.x < maxScale)
			{
				base.transform.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
			}
			else
			{
				increasing = false;
			}
		}
		else if (base.transform.localScale.x > minScale)
		{
			base.transform.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
		}
		else
		{
			increasing = true;
		}
	}
}
