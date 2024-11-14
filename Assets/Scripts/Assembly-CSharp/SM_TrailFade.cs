using UnityEngine;

public class SM_TrailFade : MonoBehaviour
{
	public float fadeInTime = 0.1f;

	public float stayTime = 0.1f;

	public float fadeOutTime = 0.1f;

	public TrailRenderer thisTrail;

	private float timeElapsed;

	private float timeElapsedLast;

	private float percent;

	private void Start()
	{
		thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		if ((double)fadeInTime < 0.01)
		{
			fadeInTime = 0.01f;
		}
		percent = timeElapsed / fadeInTime;
	}

	private void Update()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed <= fadeInTime)
		{
			percent = timeElapsed / fadeInTime;
			thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, percent));
		}
		if (timeElapsed > fadeInTime && timeElapsed < fadeInTime + stayTime)
		{
			thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		}
		if (timeElapsed >= fadeInTime + stayTime && timeElapsed < fadeInTime + stayTime + fadeOutTime)
		{
			timeElapsedLast += Time.deltaTime;
			percent = 1f - timeElapsedLast / fadeOutTime;
			thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, percent));
		}
	}
}
