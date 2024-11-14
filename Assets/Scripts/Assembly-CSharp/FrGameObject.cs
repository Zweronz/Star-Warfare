using System.Collections;
using UnityEngine;

public class FrGameObject
{
	private GameObject m_gameObject;

	public PrefabDependencys m_pd;

	public GameObject mGameObject
	{
		get
		{
			return m_gameObject;
		}
	}

	public void Instantiate(bool unloadAssetBundle)
	{
		m_gameObject = Object.Instantiate(m_pd.m_prefab.m_assetBundle.mainAsset) as GameObject;
		if (unloadAssetBundle)
		{
			m_pd.m_prefab.m_assetBundle.Unload(false);
		}
	}

	public void Unload(bool immediately)
	{
		Object.Destroy(m_gameObject);
		FrAssetBundleManager.GetInstance().UnloadAssetBundle(m_pd, immediately);
	}

	public IEnumerator WaitForDone()
	{
		while (!m_pd.m_prefab.m_isDone)
		{
			yield return 1;
		}
	}
}
