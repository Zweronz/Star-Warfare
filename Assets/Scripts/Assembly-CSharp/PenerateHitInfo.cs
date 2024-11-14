using UnityEngine;

public class PenerateHitInfo
{
	public int enemyID;

	public float lastHitTime;

	public PenerateHitInfo(int id, float time)
	{
		enemyID = id;
		lastHitTime = time;
	}

	public bool CouldHit()
	{
		if (Time.time - lastHitTime > 0.3f)
		{
			lastHitTime = Time.time;
			return true;
		}
		return false;
	}
}
