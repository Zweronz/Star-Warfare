using System;
using UnityEngine;

public class NetworkTransformInterpolation
{
	private float extrapolationForwardTime;

	protected NetworkTransform[] bufferedTransforms = new NetworkTransform[50];

	protected Transform transform;

	protected int transCount;

	protected Vector3 diff = Vector3.zero;

	protected RemotePlayer remotePlayer;

	protected int enterExpolation;

	protected bool lastExpolation;

	protected int enterInterpolation;

	protected bool lastInterpolation;

	public NetworkTransformInterpolation(RemotePlayer remotePlayer)
	{
		this.remotePlayer = remotePlayer;
	}

	public void SetTransform(Transform transform)
	{
		this.transform = transform;
	}

	public void ReceiveTransform(NetworkTransform nTrans)
	{
		for (int num = bufferedTransforms.Length - 1; num >= 1; num--)
		{
			bufferedTransforms[num] = bufferedTransforms[num - 1];
		}
		bufferedTransforms[0] = nTrans;
		transCount = Mathf.Min(transCount + 1, bufferedTransforms.Length);
		for (int i = 0; i < transCount - 1; i++)
		{
			if (bufferedTransforms[i].TimeStamp < bufferedTransforms[i + 1].TimeStamp)
			{
			}
		}
	}

	public void Clear()
	{
		transCount = 0;
		bufferedTransforms = new NetworkTransform[50];
	}

	public void Loop()
	{
		if (transCount == 0)
		{
			return;
		}
		int networkTime = TimeManager.GetInstance().NetworkTime;
		int num = networkTime - TimeManager.GetInstance().InterplolationBackTime;
		if (bufferedTransforms[0].TimeStamp > num)
		{
			for (int i = 0; i < transCount; i++)
			{
				if (bufferedTransforms[i].TimeStamp > num && i != transCount - 1)
				{
					continue;
				}
				NetworkTransform networkTransform = bufferedTransforms[Mathf.Max(i - 1, 0)];
				NetworkTransform networkTransform2 = bufferedTransforms[i];
				double num2 = networkTransform.TimeStamp - networkTransform2.TimeStamp;
				float t = 0f;
				if (num2 > 0.0001)
				{
					t = (float)((double)(num - networkTransform2.TimeStamp) / num2);
				}
				Vector3 vector = Vector3.Lerp(networkTransform2.Pos, networkTransform.Pos, t);
				diff = transform.position - vector;
				float num3 = Vector3.Distance(transform.position, vector);
				if (num3 > 0.5f && num3 < 8f && transCount > 40)
				{
					float num4 = 8f;
					if (num3 > 4f)
					{
						num4 = 20f;
					}
					transform.position -= diff.normalized * num4 * Time.deltaTime;
				}
				else
				{
					transform.position = vector;
				}
				float y = Mathf.LerpAngle(networkTransform2.Angle, networkTransform.Angle, t);
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z);
				return;
			}
			lastExpolation = false;
			return;
		}
		enterInterpolation = 0;
		lastInterpolation = false;
		float num5 = Convert.ToSingle(networkTime - bufferedTransforms[0].TimeStamp) / 1000f;
		if (num5 < extrapolationForwardTime && transCount > 1)
		{
			Vector3 vector2 = bufferedTransforms[0].Pos - bufferedTransforms[1].Pos;
			float num6 = Vector3.Distance(bufferedTransforms[0].Pos, bufferedTransforms[1].Pos);
			float num7 = Convert.ToSingle(bufferedTransforms[0].TimeStamp - bufferedTransforms[1].TimeStamp) / 1000f;
			if (Mathf.Approximately(num6, 0f) || Mathf.Approximately(num7, 0f))
			{
				transform.position = bufferedTransforms[0].Pos;
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, bufferedTransforms[0].Angle, transform.rotation.eulerAngles.z);
				return;
			}
			float num8 = num6 / num7;
			vector2 = vector2.normalized;
			Vector3 to = bufferedTransforms[0].Pos + vector2 * num5 * num8;
			transform.position = Vector3.Lerp(transform.position, to, Time.deltaTime * num8);
		}
		else
		{
			Vector3 pos = bufferedTransforms[0].Pos;
			diff = transform.position - pos;
			float num9 = Vector3.Distance(transform.position, pos);
			if (num9 > 0.5f && num9 < 8f && transCount > 40)
			{
				float num10 = 8f;
				if (num9 > 4f)
				{
					num10 = 20f;
				}
				transform.position -= diff.normalized * num10 * Time.deltaTime;
			}
			else
			{
				transform.position = pos;
			}
		}
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, bufferedTransforms[0].Angle, transform.rotation.eulerAngles.z);
	}
}
