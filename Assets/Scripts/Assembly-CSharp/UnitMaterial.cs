using UnityEngine;

public class UnitMaterial
{
	private string strName;

	private Material material;

	public bool Load(string name)
	{
		bool flag = false;
		string path = "UI/" + name;
		strName = name;
		material = Resources.Load(path) as Material;
		if (!(material == null))
		{
			Texture mainTexture = material.mainTexture;
		}
		return true;
	}

	public Material GetMaterial()
	{
		return material;
	}

	public void Free()
	{
		material = null;
		strName = string.Empty;
	}
}
