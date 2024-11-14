using UnityEngine;

public class NetworkTransform
{
	public Vector3 Pos { get; set; }

	public float Angle { get; set; }

	public int TimeStamp { get; set; }

	public bool Run { get; set; }

	public NetworkTransform(Vector3 pos, float angle, int timeStamp, bool run)
	{
		Pos = pos;
		Angle = angle;
		TimeStamp = timeStamp;
		Run = run;
	}
}
