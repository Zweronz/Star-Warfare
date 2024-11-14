using UnityEngine;

public class AStarPathFinding
{
	private WayPoint lastWayPoint;

	private WayPoint targetWayPoint;

	private float lastFindPathTime = Time.time;

	public WayPoint Target
	{
		get
		{
			return targetWayPoint;
		}
	}

	public void Reset()
	{
		targetWayPoint = null;
		lastWayPoint = null;
		lastFindPathTime = Time.time;
	}

	public bool GetNextWayPoint(Vector3 enemyPos, Player target, Enemy enemy)
	{
		float num = 99999f;
		bool result = false;
		if (Time.time - lastFindPathTime > 10f || targetWayPoint == null)
		{
			Reset();
			foreach (WayPoint wayPoint2 in GameApp.GetInstance().GetGameWorld().GetWayPointList())
			{
				float magnitude = (enemyPos - wayPoint2.Position).magnitude;
				float num2 = wayPoint2.GetDistanceTo(target.NearestPoint) + magnitude;
				if (num2 < num)
				{
					Ray ray = new Ray(enemyPos + new Vector3(0f, 0.5f, 0f), wayPoint2.Position - enemyPos);
					RaycastHit hitInfo;
					if (!Physics.Raycast(ray, out hitInfo, magnitude, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL)))
					{
						targetWayPoint = wayPoint2;
						num = num2;
					}
				}
			}
			if (targetWayPoint == null)
			{
				targetWayPoint = GameApp.GetInstance().GetGameWorld().GetWayPointList()[0];
			}
			result = true;
		}
		Vector3 vector = enemyPos - targetWayPoint.Position;
		vector.y = 0f;
		if (vector.sqrMagnitude < enemy.WayPointSqrDistance)
		{
			num = 99999f;
			WayPoint wayPoint = null;
			foreach (WayPoint adjacentWayPoint in targetWayPoint.AdjacentWayPoints)
			{
				if (adjacentWayPoint != lastWayPoint)
				{
					float magnitude2 = (enemyPos - adjacentWayPoint.Position).magnitude;
					float num3 = adjacentWayPoint.GetDistanceTo(target.NearestPoint) + magnitude2;
					if (num3 < num)
					{
						wayPoint = adjacentWayPoint;
						num = num3;
					}
				}
			}
			result = true;
			lastWayPoint = targetWayPoint;
			targetWayPoint = wayPoint;
		}
		return result;
	}
}
