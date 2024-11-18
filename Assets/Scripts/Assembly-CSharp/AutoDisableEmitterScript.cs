using UnityEngine;

public class AutoDisableEmitterScript : MonoBehaviour
{
	protected Timer timer = new Timer();

	private void Start()
	{
		timer.SetTimer(4f, false);
	}

	private void Update()
	{
		if (timer.Ready())
		{
			base.GetComponent<ParticleEmitter>().emit = false;
			timer.Do();
		}
	}
}
