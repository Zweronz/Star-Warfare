using UnityEngine;

public class DefentOneByOneScript : MonoBehaviour
{
	public float startTime;

	public float appearTime;

	private void Start()
	{
		base.renderer.enabled = false;
		appearTime = 9999999f;
	}

	private void Update()
	{
		if (Time.time - appearTime > 0f)
		{
			base.renderer.enabled = true;
		}
		else
		{
			base.renderer.enabled = false;
		}
	}
}
