using UnityEngine;

public class FadeOutScript : MonoBehaviour
{
	public float fadeOutStartTime;

	public float fadeOutEndTime = 5f;

	public string colorPropertyName = "_TintColor";

	private float startTime;

	private float fadeOutSpeed = 0.1f;

	private bool isComplete;

	public void Start()
	{
		startTime = Time.time;
		fadeOutSpeed = 1f / (fadeOutEndTime - fadeOutStartTime);
	}

	public void Update()
	{
		if (!isComplete && Time.time - startTime > fadeOutStartTime)
		{
			Color color = base.GetComponent<Renderer>().material.GetColor(colorPropertyName);
			color.a -= Time.deltaTime * fadeOutSpeed;
			if (color.a < 0f)
			{
				color.a = 0f;
				isComplete = true;
			}
			base.GetComponent<Renderer>().material.SetColor(colorPropertyName, color);
		}
	}
}
