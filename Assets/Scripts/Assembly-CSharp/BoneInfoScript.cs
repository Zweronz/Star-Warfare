using UnityEngine;

public class BoneInfoScript : MonoBehaviour
{
	private void Start()
	{
		Transform transform = base.transform.Find(base.name);
		SkinnedMeshRenderer component = transform.GetComponent<SkinnedMeshRenderer>();
		Transform[] bones = component.bones;
		for (int i = 0; i < component.bones.Length; i++)
		{
		}
	}

	private void Update()
	{
	}
}
