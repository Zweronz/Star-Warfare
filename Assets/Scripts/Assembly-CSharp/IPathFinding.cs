using UnityEngine;

public interface IPathFinding
{
	Transform GetNextWayPoint(Vector3 enemyPos, Vector3 playerPos, Player target);

	void ClearPath();

	bool HavePath();

	void PopNode();
}
