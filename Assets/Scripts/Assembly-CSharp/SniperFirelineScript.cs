using UnityEngine;

public class SniperFirelineScript : MonoBehaviour
{
	public Vector3 beginPos;

	public Vector3 endPos;

	private float dis;

	protected bool growing = true;

	protected float grownTime;

	public float destroyTime = 0.5f;

	public void Start()
	{
		base.transform.position = beginPos;
		base.transform.rotation = Quaternion.LookRotation(endPos - beginPos);
		base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, 0.01f);
		dis = Vector3.Distance(beginPos, endPos);
	}

	public void Update()
	{
		float num = dis / 49f;
		if (growing && base.transform.localScale.z > num)
		{
			growing = false;
			grownTime = Time.time;
		}
		if (growing)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z + 8f * Time.deltaTime);
			return;
		}
		Color color = base.GetComponent<Renderer>().material.GetColor("_TintColor");
		float num2 = Time.time - grownTime;
		color.a = 1f - num2 / destroyTime * (num2 / destroyTime);
		base.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		if (Time.time - grownTime > destroyTime)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
