using UnityEngine;

public class MeteorGeneratoerScript : MonoBehaviour
{
	protected Timer timer = new Timer();

	protected float lastTime;

	protected bool enableSecondShot;

	protected Transform A;

	protected Transform B;

	protected Transform C;

	protected Transform D;

	protected GameObject meteorPrefab;

	private void Start()
	{
		timer.SetTimer(5f, false);
		A = base.transform.GetChild(0);
		B = base.transform.GetChild(1);
		C = GameObject.Find("C").transform;
		D = GameObject.Find("D").transform;
		meteorPrefab = Resources.Load("Level/Meteor") as GameObject;
	}

	private void Update()
	{
		if (timer.Ready())
		{
			GameObject gameObject = Object.Instantiate(meteorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			MeteorScript component = gameObject.GetComponent<MeteorScript>();
			component.beginPos = A;
			component.endPos = B;
			component.speed = 300f;
			GameObject gameObject2 = Object.Instantiate(meteorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			component = gameObject2.GetComponent<MeteorScript>();
			component.beginPos = D;
			component.endPos = C;
			component.speed = 300f;
			timer.Do();
			enableSecondShot = true;
			lastTime = Time.time;
		}
		if (Time.time - lastTime > 0.5f && enableSecondShot)
		{
			GameObject gameObject3 = Object.Instantiate(meteorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			MeteorScript component2 = gameObject3.GetComponent<MeteorScript>();
			component2.beginPos = B;
			component2.endPos = A;
			component2.speed = 300f;
			enableSecondShot = false;
		}
	}
}
