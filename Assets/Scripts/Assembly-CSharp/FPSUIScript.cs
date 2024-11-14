using UnityEngine;

public class FPSUIScript : MonoBehaviour
{
	protected float frames;

	protected float updateInterval = 2f;

	protected float timeLeft;

	protected string fpsStr;

	protected float accum;

	private void Start()
	{
	}

	private void Update()
	{
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames += 1f;
		if (timeLeft <= 0f)
		{
			fpsStr = "FPS:" + accum / frames;
			frames = 0f;
			accum = 0f;
			timeLeft = updateInterval;
		}
	}

	public void OnGUI()
	{
		GUI.Label(new Rect(10f, 130f, 200f, 100f), fpsStr);
	}
}
