using UnityEngine;

public class DefentOneByOneScript : MonoBehaviour
{
	public float startTime;

	public float appearTime;

	private void Start()
	{
		base.GetComponent<Renderer>().enabled = false;
		appearTime = 9999999f;
	}

	private void Update()
	{
		if (Time.time - appearTime > 0f)
		{
			base.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = false;
		}
	}
}
