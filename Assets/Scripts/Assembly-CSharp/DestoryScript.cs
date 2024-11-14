using UnityEngine;

public class DestoryScript : MonoBehaviour
{
	public void Start()
	{
	}

	public void Update()
	{
	}

	public void OnDestroy()
	{
		if (GameApp.SpringBulletCount > 0)
		{
			GameApp.SpringBulletCount--;
		}
	}
}
