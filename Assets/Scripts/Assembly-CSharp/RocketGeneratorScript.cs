using UnityEngine;

public class RocketGeneratorScript : MonoBehaviour
{
	protected Timer timer = new Timer();

	protected Transform A1;

	protected Transform A2;

	protected Vector3 dir;

	protected GameObject rocketPrefab;

	private void Start()
	{
		timer.SetTimer(5f, false);
		A1 = GameObject.Find("A1").transform;
		A2 = GameObject.Find("A2").transform;
		dir = (A2.position - A1.position).normalized;
		rocketPrefab = Resources.Load("Level/Rocket") as GameObject;
	}

	private void Update()
	{
		if (timer.Ready())
		{
			GameObject gameObject = Object.Instantiate(rocketPrefab, A1.position, Quaternion.identity) as GameObject;
			gameObject.transform.LookAt(A2);
			RocketScript component = gameObject.GetComponent<RocketScript>();
			component.beginPos = A1;
			component.endPos = A2;
			timer.Do();
		}
	}
}
