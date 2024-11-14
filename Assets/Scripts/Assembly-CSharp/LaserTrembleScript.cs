using UnityEngine;

public class LaserTrembleScript : MonoBehaviour
{
	public float minScaleX = 0.03f;

	public float maxScaleX = 0.06f;

	public float scaleSpeed = 0.1f;

	protected bool increasing;

	private void Start()
	{
	}

	private void Update()
	{
		if (increasing)
		{
			if (base.transform.localScale.x < maxScaleX)
			{
				base.transform.localScale += Vector3.right * Time.deltaTime * scaleSpeed;
			}
			else
			{
				increasing = false;
			}
		}
		else if (base.transform.localScale.x > minScaleX)
		{
			base.transform.localScale -= Vector3.right * Time.deltaTime * scaleSpeed;
		}
		else
		{
			increasing = true;
		}
	}
}
