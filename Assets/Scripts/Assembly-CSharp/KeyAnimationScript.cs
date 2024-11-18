using UnityEngine;

public class KeyAnimationScript : MonoBehaviour
{
	public float frameRate = 0.02f;

	protected int currentIndex;

	public Texture2D[] textures;

	public string texturePropertyName = "_MainTex";

	protected float deltaTime;

	private void Start()
	{
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime > frameRate)
		{
			deltaTime = 0f;
			int num = textures.Length;
			currentIndex++;
			if (currentIndex >= textures.Length)
			{
				currentIndex = 0;
			}
			base.GetComponent<Renderer>().material.SetTexture(texturePropertyName, textures[currentIndex]);
		}
	}
}
