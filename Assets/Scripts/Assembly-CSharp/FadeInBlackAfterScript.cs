using UnityEngine;

public class FadeInBlackAfterScript : MonoBehaviour
{
	public float afterSeconds = 3f;

	protected float startTime;

	protected bool startFade;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - startTime > afterSeconds && !startFade)
		{
			FadeAnimationScript.GetInstance().FadeInBlack();
			Application.LoadLevel("StartMenu");
		}
	}
}
