using UnityEngine;

public class AutoNeonScript : MonoBehaviour
{
	public GameObject[] neons;

	public float duration;

	private float startTime;

	private int startIndex;

	private int inc;

	private void Start()
	{
		startTime = Time.time;
		for (int i = 0; i < neons.Length; i++)
		{
			if (i == 0)
			{
				neons[i].SetActive(true);
			}
			else
			{
				neons[i].SetActive(false);
			}
		}
		startIndex = 0;
		inc = 1;
	}

	private void Update()
	{
		if (!(Time.time - startTime > duration))
		{
			return;
		}
		startTime = Time.time;
		if (neons.Length > 1)
		{
			neons[startIndex].SetActive(false);
			startIndex += inc;
			if (startIndex > neons.Length - 1 || startIndex < 0)
			{
				inc = -inc;
				startIndex += inc;
			}
			neons[startIndex].SetActive(true);
		}
	}
}
