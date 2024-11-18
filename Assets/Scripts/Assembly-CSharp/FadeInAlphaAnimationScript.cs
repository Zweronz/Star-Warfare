using UnityEngine;

public class FadeInAlphaAnimationScript : MonoBehaviour
{
	public float maxAlpha = 1f;

	public float minAlpha;

	public float animationSpeed = 5.5f;

	public float maxBright = 1f;

	public float minBright;

	public bool enableAlphaAnimation;

	public bool enableBrightAnimation;

	public string colorPropertyName = "_TintColor";

	protected float alpha;

	protected float startTime;

	protected bool increasing = true;

	public Color startColor = Color.yellow;

	protected float lastUpdateTime;

	protected float deltaTime;

	private void Start()
	{
		startTime = Time.time;
	}

	public void FadeIn()
	{
		increasing = true;
		Material[] materials = base.GetComponent<Renderer>().materials;
		Color color = new Color(1f, 1f, 1f, 1f);
		Material[] array = materials;
		foreach (Material material in array)
		{
			material.SetColor(colorPropertyName, color);
		}
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.02f))
		{
			Color white = Color.white;
			white = base.GetComponent<Renderer>().material.GetColor(colorPropertyName);
			if (increasing)
			{
				white.a -= animationSpeed * deltaTime;
				white.a = Mathf.Clamp(white.a, minAlpha, maxAlpha);
			}
			Material[] materials = base.GetComponent<Renderer>().materials;
			Material[] array = materials;
			foreach (Material material in array)
			{
				material.SetColor(colorPropertyName, white);
			}
			deltaTime = 0f;
		}
	}
}
