using System.Collections;
using UnityEngine;

internal class ResourceLoad
{
	protected Hashtable resources = new Hashtable();

	protected static ResourceLoad instance = new ResourceLoad();

	public static ResourceLoad GetInstance()
	{
		return instance;
	}

	public GameObject Load(string resName)
	{
		if (!resources.Contains(resName))
		{
			Debug.Log("new resource load:" + resName);
			GameObject value = Resources.Load(resName) as GameObject;
			resources.Add(resName, value);
		}
		return resources[resName] as GameObject;
	}
}
