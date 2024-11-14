using UnityEngine;

public class ScreenBloodScript : MonoBehaviour
{
	public float scrollSpeed = 1f;

	protected float alpha;

	protected float startTime;

	protected float deltaTime;

	public string alphaPropertyName = "_Alpha";

	protected GameObject blood;

	protected GameObject deadblood;

	private void Start()
	{
		alpha = base.renderer.material.GetFloat(alphaPropertyName);
		startTime = Time.time;
		if (UIConstant.Is16By9())
		{
			blood = GameObject.Find("Screen_Blood");
			deadblood = GameObject.Find("Screen_DeadBlood");
			blood.transform.localScale = new Vector3(4.16f, 2.4f, 1f);
			deadblood.transform.localScale = new Vector3(4.16f, 2.4f, 1f);
		}
	}

	public void NewBlood(float damage)
	{
		base.renderer.enabled = true;
		alpha = damage;
		alpha = Mathf.Clamp(alpha, 0f, 1f);
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			alpha -= 0.5f * deltaTime;
			if (alpha <= 0f)
			{
				base.renderer.enabled = false;
			}
			base.renderer.material.SetFloat(alphaPropertyName, alpha);
			deltaTime = 0f;
		}
	}
}
