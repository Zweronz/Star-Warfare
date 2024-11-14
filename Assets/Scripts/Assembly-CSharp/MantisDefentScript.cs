using UnityEngine;

public class MantisDefentScript : MonoBehaviour
{
	protected FadeInAlphaAnimationScript[] defentScript = new FadeInAlphaAnimationScript[3];

	protected DefentOneByOneScript[] defentOnebyOneScript = new DefentOneByOneScript[3];

	protected Timer showDefentTimer = new Timer();

	protected bool display;

	public void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			defentScript[i] = base.transform.GetChild(i).GetComponent<FadeInAlphaAnimationScript>();
			defentOnebyOneScript[i] = base.transform.GetChild(i).GetComponent<DefentOneByOneScript>();
		}
		showDefentTimer.SetTimer(2f, false);
	}

	public void Update()
	{
		if (display)
		{
			ShowDefent();
		}
	}

	public void Enable()
	{
		display = true;
		for (int i = 0; i < 3; i++)
		{
			defentScript[i].minAlpha = 0.1f;
		}
	}

	public void Disable()
	{
		display = false;
		for (int i = 0; i < 3; i++)
		{
			defentScript[i].minAlpha = 0f;
		}
	}

	public void ShowDefent()
	{
		if (showDefentTimer.Ready())
		{
			for (int i = 0; i < 3; i++)
			{
				defentOnebyOneScript[i].appearTime = Time.time + (float)i * 0.05f;
				defentScript[i].FadeIn();
			}
			showDefentTimer.Do();
		}
	}
}
