using System.Collections.Generic;
using UnityEngine;

public class WayPoint
{
	private GameObject wayPointObject;

	private Dictionary<WayPoint, float> distances = new Dictionary<WayPoint, float>();

	private List<WayPoint> adjacentWayPoints = new List<WayPoint>();

	public float HelpDistance { get; set; }

	public GameObject WayPointObject
	{
		get
		{
			return wayPointObject;
		}
	}

	public Vector3 Position
	{
		get
		{
			return wayPointObject.transform.position;
		}
	}

	public List<WayPoint> AdjacentWayPoints
	{
		get
		{
			return adjacentWayPoints;
		}
	}

	public WayPoint(GameObject obj)
	{
		wayPointObject = obj;
	}

	public float GetDistanceTo(WayPoint point)
	{
		return distances[point];
	}

	public void AddAdjacentWayPoint(WayPoint point)
	{
		if (!adjacentWayPoints.Contains(point))
		{
			adjacentWayPoints.Add(point);
		}
	}

	public void AddWayPointDistance(WayPoint targetPoint)
	{
		if (distances.ContainsKey(targetPoint))
		{
			return;
		}
		List<WayPoint> list = new List<WayPoint>();
		List<WayPoint> list2 = new List<WayPoint>();
		HelpDistance = 0f;
		list.Add(this);
		WayPoint wayPoint = list[0];
		while (list.Count > 0)
		{
			wayPoint = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].HelpDistance < wayPoint.HelpDistance)
				{
					wayPoint = list[i];
				}
			}
			if (wayPoint == targetPoint)
			{
				distances.Add(targetPoint, wayPoint.HelpDistance);
				break;
			}
			list.Remove(wayPoint);
			list2.Add(wayPoint);
			foreach (WayPoint adjacentWayPoint in wayPoint.AdjacentWayPoints)
			{
				if (!list.Contains(adjacentWayPoint) && !list2.Contains(adjacentWayPoint))
				{
					float num = Vector3.Distance(adjacentWayPoint.Position, wayPoint.Position);
					adjacentWayPoint.HelpDistance = wayPoint.HelpDistance + num;
					list.Add(adjacentWayPoint);
				}
			}
		}
		targetPoint.AddWayPointDistance(this, wayPoint.HelpDistance);
	}

	public void AddWayPointDistance(WayPoint targetPoint, float distance)
	{
		if (!distances.ContainsKey(targetPoint))
		{
			distances.Add(targetPoint, distance);
		}
	}
}
