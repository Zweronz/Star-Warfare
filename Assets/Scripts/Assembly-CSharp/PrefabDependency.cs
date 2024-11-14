using UnityEngine;

public class PrefabDependency
{
	public string m_name = string.Empty;

	public int m_refCount;

	public AssetBundle m_assetBundle;

	public bool m_isDone;
}
