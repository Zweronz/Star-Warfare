using UnityEngine;

[ExecuteInEditMode]
public class SetRenderQueue : MonoBehaviour
{
	public int mRendererQueue;

	private Renderer[] mRd;

	private void Start()
	{
		mRd = GetComponentsInChildren<Renderer>();
	}

	private void Update()
	{
		if (mRd == null || mRd.Length <= 0)
		{
			return;
		}
		Renderer[] array = mRd;
		foreach (Renderer renderer in array)
		{
			if (renderer != null && renderer.sharedMaterials != null && renderer.sharedMaterials.Length > 0)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				foreach (Material material in sharedMaterials)
				{
					material.renderQueue = material.shader.renderQueue + mRendererQueue;
				}
			}
		}
	}
}
