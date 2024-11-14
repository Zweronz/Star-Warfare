using UnityEngine;

public class VSClock
{
	protected float currentTime;

	protected bool clockRunning;

	protected int totalGameSeconds;

	public bool sentTimeUpRequest;

	public VSClock()
	{
		totalGameSeconds = 1500;
	}

	public void Restart()
	{
		currentTime = 0f;
		clockRunning = true;
		sentTimeUpRequest = false;
	}

	public void Reset()
	{
		currentTime = 0f;
	}

	public void SetTimeLeft(int seconds)
	{
		currentTime = totalGameSeconds - seconds;
	}

	public float GetTimeLeft()
	{
		return (float)totalGameSeconds - currentTime;
	}

	public void SetTotalGameSeconds(int seconds)
	{
		totalGameSeconds = seconds;
	}

	public int GetCurrentTimeSeconds()
	{
		if (currentTime < 0.5f)
		{
			return 0;
		}
		return (int)currentTime + 1;
	}

	public void SetCurrentTime(float currentTime)
	{
		this.currentTime = currentTime;
	}

	public float GetCurrentTime()
	{
		return currentTime;
	}

	public void StopTime()
	{
		clockRunning = false;
	}

	public void StopAtEnd()
	{
		currentTime = totalGameSeconds;
		clockRunning = false;
	}

	public void ResumeTime()
	{
		clockRunning = true;
	}

	public void Update()
	{
		if (clockRunning)
		{
			currentTime += Time.deltaTime;
		}
		if (currentTime > (float)totalGameSeconds * 1f)
		{
			currentTime = (float)totalGameSeconds * 1f;
		}
	}

	public bool IsGameOver()
	{
		if (currentTime >= (float)totalGameSeconds)
		{
			return true;
		}
		return false;
	}
}
