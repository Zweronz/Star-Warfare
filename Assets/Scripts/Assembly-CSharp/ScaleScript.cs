using UnityEngine;

public class ScaleScript : MonoBehaviour
{
	public float scaleSpeed = 2f;

	public float maxScale = 100f;

	public bool enableMaxScale;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.localScale = base.transform.localScale + Vector3.one * Time.deltaTime * scaleSpeed;
		if (enableMaxScale && base.transform.localScale.x >= maxScale)
		{
			base.transform.localScale = Vector3.one * maxScale;
		}
	}
}
