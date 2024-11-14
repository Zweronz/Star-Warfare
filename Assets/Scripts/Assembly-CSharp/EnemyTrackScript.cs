using System.Collections;
using UnityEngine;

public class EnemyTrackScript : MonoBehaviour
{
	public float speed = 0.5f;

	public Transform[] trackTrans;

	protected int currentTarget;

	public string trackName = string.Empty;

	public bool pingpong;

	private IEnumerator Start()
	{
		Transform tt = GameObject.Find(trackName).transform;
		int count = tt.GetChildCount();
		trackTrans = new Transform[count];
		for (int i = 0; i < count; i++)
		{
			trackTrans[i] = tt.GetChild(i);
		}
		Transform ct = base.transform;
		while (true)
		{
			Vector3 dir = (trackTrans[currentTarget].position - ct.position).normalized;
			ct.Translate(dir * speed * Time.deltaTime, Space.World);
			if (Vector3.Distance(ct.position, trackTrans[currentTarget].position) < 0.1f)
			{
				currentTarget++;
				if (currentTarget == trackTrans.Length)
				{
					currentTarget = 0;
					if (!pingpong)
					{
						ct.position = trackTrans[0].position;
					}
				}
				ct.LookAt(trackTrans[currentTarget].position);
			}
			yield return 1;
		}
	}

	private void Update()
	{
	}
}
