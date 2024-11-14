using System.Collections.Generic;

public class FrAssetBundleConfig
{
	public GlobalDependency m_globalDependency = new GlobalDependency();

	public Dictionary<string, PrefabDependencys> m_prefabDependencys = new Dictionary<string, PrefabDependencys>();

	public bool m_loading;

	public void LoadAssetBundle(PrefabDependencys pd)
	{
		UpdatRrefCount(pd, true);
	}

	public void UnloadAssetBundle(PrefabDependencys pd)
	{
		UpdatRrefCount(pd, false);
	}

	public void ClearRrefCount()
	{
		foreach (KeyValuePair<PrefabDependencyType, List<PrefabDependency>> item in m_globalDependency.m_dependency)
		{
			foreach (PrefabDependency item2 in item.Value)
			{
				item2.m_refCount = 0;
			}
		}
		foreach (KeyValuePair<string, PrefabDependencys> prefabDependency in m_prefabDependencys)
		{
			prefabDependency.Value.m_prefab.m_refCount = 0;
		}
	}

	private void UpdatRrefCount(PrefabDependencys pd, bool positive)
	{
		pd.m_prefab.m_refCount = ((!positive) ? (pd.m_prefab.m_refCount - 1) : (pd.m_prefab.m_refCount + 1));
		foreach (KeyValuePair<PrefabDependencyType, List<short>> item in pd.m_dependency)
		{
			PrefabDependencyType key = item.Key;
			List<short> value = item.Value;
			List<PrefabDependency> list = m_globalDependency.m_dependency[key];
			foreach (short item2 in value)
			{
				if (positive)
				{
					list[item2].m_refCount++;
				}
				else
				{
					list[item2].m_refCount--;
				}
			}
		}
	}

	public void LoadRes()
	{
		m_loading = true;
	}

	public void UnloadRes()
	{
		GlobalDependency globalDependency = m_globalDependency;
		foreach (KeyValuePair<PrefabDependencyType, List<PrefabDependency>> item in globalDependency.m_dependency)
		{
			foreach (PrefabDependency item2 in item.Value)
			{
				if (item2.m_refCount <= 0 && item2.m_assetBundle != null && item2.m_assetBundle != null)
				{
					item2.m_assetBundle.Unload(true);
					item2.m_assetBundle = null;
					item2.m_isDone = false;
				}
			}
		}
		foreach (KeyValuePair<string, PrefabDependencys> prefabDependency in m_prefabDependencys)
		{
			PrefabDependency prefab = prefabDependency.Value.m_prefab;
			if (prefab.m_refCount <= 0 && prefab.m_assetBundle != null && prefab.m_assetBundle != null)
			{
				prefab.m_assetBundle.Unload(true);
				prefab.m_assetBundle = null;
				prefab.m_isDone = false;
			}
		}
	}
}
