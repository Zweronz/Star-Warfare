using UnityEngine;

public class UVOffsetScript : MonoBehaviour
{
	public float scrollSpeed = 1f;

	protected float alpha;

	protected float startTime;

	public string texturePropertyName = "_tex2";

	public int materialIndex;

	public int cols = 3;

	public int rows = 3;

	protected float xStep;

	protected float yStep;

	private void Start()
	{
		startTime = Time.time;
		xStep = 1f / (float)cols;
		yStep = 1f / (float)rows;
	}

	private void Update()
	{
		int num = (int)(Time.time * scrollSpeed) % (cols * rows);
		base.GetComponent<Renderer>().materials[materialIndex].SetTextureOffset(texturePropertyName, new Vector2(xStep * (float)(num % cols), 1f - yStep * (float)(num / cols)));
	}
}
