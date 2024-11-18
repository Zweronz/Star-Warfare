using UnityEngine;

public class TwoUVAnimationScript : MonoBehaviour
{
	public bool u1 = true;

	public bool v1 = true;

	public float scrollSpeed1 = 1f;

	public bool u2 = true;

	public bool v2 = true;

	public float scrollSpeed2 = 1f;

	protected float alpha;

	protected float startTime;

	public string alphaPropertyName = "_Alpha";

	public string texturePropertyName = "_MainTex";

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		Material[] materials = base.GetComponent<Renderer>().materials;
		if (materials.Length > 0)
		{
			float num = Time.time * scrollSpeed1 % 1f;
			if (u1 && v1)
			{
				materials[0].SetTextureOffset(texturePropertyName, new Vector2(num, num));
			}
			else if (u1)
			{
				materials[0].SetTextureOffset(texturePropertyName, new Vector2(num, 0f));
			}
			else if (v1)
			{
				materials[0].SetTextureOffset(texturePropertyName, new Vector2(0f, num));
			}
		}
		if (materials.Length > 1)
		{
			float num2 = Time.time * scrollSpeed2 % 1f;
			if (u2 && v2)
			{
				materials[1].SetTextureOffset(texturePropertyName, new Vector2(num2, num2));
			}
			else if (u2)
			{
				materials[1].SetTextureOffset(texturePropertyName, new Vector2(num2, 0f));
			}
			else if (v2)
			{
				materials[1].SetTextureOffset(texturePropertyName, new Vector2(0f, num2));
			}
		}
	}
}
