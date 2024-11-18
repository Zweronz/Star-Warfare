using UnityEngine;

public class AdvancedShotgunAnimationScript : MonoBehaviour
{
	protected Timer aniTimer = new Timer();

	private void Start()
	{
		aniTimer.SetTimer(0.5f, false);
	}

	private void Update()
	{
		if (aniTimer.Ready())
		{
			base.GetComponent<Animation>()["fire"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().Play("fire");
			aniTimer.Do();
		}
	}
}
