using System.Collections;
using UnityEngine;

public class CameraAnimationScript : MonoBehaviour
{
	public float speed = 0.5f;

	public Transform[] trackTrans;

	protected int currentTarget;

	public bool lookRotate;

	protected Transform lastTarget;

	private IEnumerator Start()
	{
		int count = base.transform.GetChildCount();
		trackTrans = new Transform[count];
		for (int i = 0; i < count; i++)
		{
			trackTrans[i] = base.transform.GetChild(i);
		}
		if (lookRotate)
		{
			for (int j = 0; j < count; j++)
			{
				if (j == count - 1)
				{
					trackTrans[j].LookAt(trackTrans[0]);
				}
				else
				{
					trackTrans[j].LookAt(trackTrans[j + 1]);
				}
			}
		}
		Transform ct = GameObject.Find("MenuCamera").transform;
		ct.position = trackTrans[0].position;
		ct.rotation = GameObject.Find("StartMenuScene/CameraTransform").transform.rotation;
		while (true)
		{
			Vector3 dir = (trackTrans[currentTarget].position - ct.position).normalized;
			ct.Translate(dir * speed * Time.deltaTime, Space.World);
			if (Vector3.Distance(ct.position, trackTrans[currentTarget].position) < 0.1f)
			{
				lastTarget = trackTrans[currentTarget];
				currentTarget++;
				if (currentTarget == trackTrans.Length)
				{
					currentTarget = 0;
				}
			}
			if (lookRotate)
			{
				float d1 = Vector3.Distance(ct.position, lastTarget.position);
				float d2 = Vector3.Distance(lastTarget.position, trackTrans[currentTarget].position);
				Quaternion q1 = lastTarget.rotation;
				Quaternion q2 = trackTrans[currentTarget].rotation;
				float r1 = d1 / d2;
				float r2 = 1f - r1;
				Vector3 e2 = q1.eulerAngles;
				Vector3 e3 = q2.eulerAngles;
				Vector3 e = e2 * r2 + e3 * r1;
				if (Mathf.Abs(e2.y - e3.y) > 180f)
				{
					if (e2.y > e3.y)
					{
						e2.y -= 360f;
					}
					else
					{
						e3.y -= 360f;
					}
					e = e2 * r2 + e3 * r1;
				}
				ct.rotation = Quaternion.Euler(e);
			}
			yield return 1;
		}
	}

	private void Update()
	{
	}
}
