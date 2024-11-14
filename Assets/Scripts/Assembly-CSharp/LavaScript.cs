using UnityEngine;

public class LavaScript : MonoBehaviour
{
	protected GameObject redLavaPrefab;

	protected GameObject blackLavaPrefab;

	protected Timer timer = new Timer();

	private void Start()
	{
		redLavaPrefab = Resources.Load("Effect/dargon_smoke1") as GameObject;
		blackLavaPrefab = Resources.Load("Effect/dargon_smoke2") as GameObject;
		timer.SetTimer(2f, false);
	}

	private void Update()
	{
		if (timer.Ready())
		{
			float x = Random.Range((0f - base.transform.localScale.x) / 2f, base.transform.localScale.x / 2f);
			float z = Random.Range((0f - base.transform.localScale.z) / 2f, base.transform.localScale.z / 2f);
			Vector3 position = base.transform.position + new Vector3(x, 0f, z);
			int num = Random.Range(0, 100);
			if (num < 50)
			{
				Object.Instantiate(redLavaPrefab, position, Quaternion.identity);
			}
			else
			{
				Object.Instantiate(blackLavaPrefab, position, Quaternion.identity);
			}
			timer.Do();
		}
	}
}
