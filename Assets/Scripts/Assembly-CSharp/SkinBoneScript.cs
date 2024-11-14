using System.Collections.Generic;
using UnityEngine;

public class SkinBoneScript : MonoBehaviour
{
	public int[] AvatarSelection;

	public string aniName = "run";

	protected bool binded;

	protected GameObject player;

	public void BindBones(SkinnedMeshRenderer smr, List<Transform> bones, GameObject parentObj)
	{
		Transform[] array = new Transform[smr.bones.Length];
		for (int i = 0; i < smr.bones.Length; i++)
		{
			string text = smr.bones[i].name;
			for (int j = 0; j < bones.Count; j++)
			{
				if (text == bones[j].name)
				{
					array[i] = bones[j];
					break;
				}
			}
		}
		smr.bones = array;
		smr.transform.parent = parentObj.transform;
	}

	public void TraverseBones(Transform t, List<Transform> bones)
	{
		bones.Add(t);
		if (t.childCount > 0)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				TraverseBones(t.GetChild(i), bones);
			}
		}
	}

	private void Start()
	{
	}

	public void BindAvatar()
	{
	}

	private void Update()
	{
		if (!binded)
		{
		}
	}
}
