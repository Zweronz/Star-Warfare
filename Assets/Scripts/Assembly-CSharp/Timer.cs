using UnityEngine;

public class Timer
{
	protected TimerInfo info = new TimerInfo();

	protected bool start;

	public string Name { get; set; }

	public void SetTimer(float interval, bool doAtStart)
	{
		if (doAtStart)
		{
			info.lastDoTime = -9999f;
		}
		else
		{
			info.lastDoTime = Time.time;
		}
		info.interval = interval;
		start = true;
	}

	public void Do()
	{
		info.lastDoTime = Time.time;
	}

	public bool Ready()
	{
		if (start && Time.time - info.lastDoTime > info.interval)
		{
			return true;
		}
		return false;
	}

	public float GetInterval()
	{
		return info.interval;
	}

	public float GetTimeSpan()
	{
		if (start)
		{
			return Mathf.Min(Time.time - info.lastDoTime, info.interval);
		}
		return 0f;
	}
}
