using UnityEngine;

public class FadeAnimationScript : MonoBehaviour
{
	public Color startColor = Color.black;

	public Color endColor = new Color(0f, 0f, 0f, 0f);

	public float animationSpeed = 0.5f;

	public bool enableAlphaAnimation;

	public string colorPropertyName = "_TintColor";

	protected float deltaTime;

	private void Start()
	{
		base.renderer.material.SetColor(colorPropertyName, startColor);
	}

	public void StartFade(Color startColor, Color endColor)
	{
		this.startColor = startColor;
		this.endColor = endColor;
		base.renderer.material.SetColor(colorPropertyName, startColor);
		enableAlphaAnimation = true;
		base.renderer.enabled = true;
	}

	public void StartFade(Color startColor, Color endColor, float time)
	{
		StartFade(startColor, endColor);
		if (time != 0f)
		{
			animationSpeed = 1f / time;
		}
	}

	public bool FadeOutComplete()
	{
		return base.renderer.material.GetColor(colorPropertyName).a == 0f;
	}

	public bool FadeInComplete()
	{
		return base.renderer.material.GetColor(colorPropertyName).a == 1f;
	}

	public static FadeAnimationScript GetInstance()
	{
		return GameObject.Find("CameraFade").GetComponent<FadeAnimationScript>();
	}

	public void FadeInBlack()
	{
		StartFade(new Color(0f, 0f, 0f, 0f), Color.black);
	}

	public void FadeOutBlack()
	{
		StartFade(Color.black, new Color(0f, 0f, 0f, 0f));
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime < 0.02f)
		{
			return;
		}
		if (enableAlphaAnimation)
		{
			float a = startColor.a;
			float a2 = endColor.a;
			float num = Mathf.Sign(a2 - a);
			Color color = base.renderer.material.GetColor(colorPropertyName);
			color.a += num * animationSpeed * deltaTime;
			if (Mathf.Sign(a2 - color.a) != num)
			{
				color.a = a2;
				enableAlphaAnimation = false;
				base.renderer.enabled = false;
			}
			base.renderer.material.SetColor(colorPropertyName, color);
		}
		deltaTime = 0f;
	}
}
