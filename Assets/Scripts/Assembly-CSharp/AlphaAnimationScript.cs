using UnityEngine;

public class AlphaAnimationScript : MonoBehaviour
{
	public float maxAlpha = 1f;

	public float minAlpha;

	public float animationSpeed = 3f;

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

	public bool oneShot;

	protected bool animating;

	protected bool hasColorProperty;

	private void Start()
	{
		startTime = Time.time;
		startColor = Color.black;
		if (enableAlphaAnimation || enableBrightAnimation)
		{
			if (!base.GetComponent<Renderer>().material.HasProperty(colorPropertyName))
			{
				hasColorProperty = false;
				return;
			}
			startColor = base.GetComponent<Renderer>().material.GetColor(colorPropertyName);
			hasColorProperty = true;
		}
		if (enableAlphaAnimation)
		{
			startColor.a = minAlpha;
		}
		float num = Mathf.Min(startColor.r - minBright, startColor.r - minBright, startColor.r - minBright);
		startColor.r -= num;
		startColor.g -= num;
		startColor.b -= num;
		Material[] materials = base.GetComponent<Renderer>().materials;
		Material[] array = materials;
		foreach (Material material in array)
		{
			material.SetColor(colorPropertyName, startColor);
		}
	}

	private void Update()
	{
		if (!hasColorProperty)
		{
			return;
		}
		deltaTime += Time.deltaTime;
		if (deltaTime < 0.02f)
		{
			return;
		}
		if (animating || !oneShot)
		{
			Color color = Color.white;
			if (enableAlphaAnimation || enableBrightAnimation)
			{
				color = base.GetComponent<Renderer>().material.GetColor(colorPropertyName);
			}
			if (enableAlphaAnimation)
			{
				if (increasing)
				{
					color.a += animationSpeed * deltaTime;
					color.a = Mathf.Clamp(color.a, minAlpha, maxAlpha);
					if (color.a == maxAlpha)
					{
						increasing = false;
					}
				}
				else
				{
					color.a -= animationSpeed * deltaTime;
					color.a = Mathf.Clamp(color.a, minAlpha, maxAlpha);
					if (color.a == minAlpha)
					{
						increasing = true;
					}
				}
			}
			if (enableBrightAnimation)
			{
				if (increasing)
				{
					color.r += animationSpeed * deltaTime;
					color.g += animationSpeed * deltaTime;
					color.b += animationSpeed * deltaTime;
					if (color.r >= maxBright || color.g >= maxBright || color.b >= maxBright)
					{
						float num = Mathf.Max(color.r, color.g, color.b);
						float num2 = num - maxBright;
						color.r -= num2;
						color.g -= num2;
						color.b -= num2;
						increasing = false;
					}
				}
				else
				{
					color.r -= animationSpeed * deltaTime;
					color.g -= animationSpeed * deltaTime;
					color.b -= animationSpeed * deltaTime;
					if (color.r <= minBright || color.g <= minBright || color.b <= minBright)
					{
						float num3 = Mathf.Min(color.r, color.g, color.b);
						float num4 = minBright - num3;
						color.r += num4;
						color.g += num4;
						color.b += num4;
						increasing = true;
						animating = false;
					}
				}
			}
			Material[] materials = base.GetComponent<Renderer>().materials;
			Material[] array = materials;
			foreach (Material material in array)
			{
				material.SetColor(colorPropertyName, color);
			}
		}
		deltaTime = 0f;
	}

	public void StartAnimation()
	{
		animating = true;
		increasing = true;
		Material[] materials = base.GetComponent<Renderer>().materials;
		Material[] array = materials;
		foreach (Material material in array)
		{
			material.SetColor(colorPropertyName, startColor);
		}
	}
}
