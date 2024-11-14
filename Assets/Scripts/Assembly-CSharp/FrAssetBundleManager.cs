using System.Collections.Generic;
using UnityEngine;

public class FrAssetBundleManager
{
	private static FrAssetBundleManager instance;

	public static string GLOBAL_CONFIG_NAME = "GlobalConfig.xml";

	public static string CONFIG_NAME = "Config.xml";

	public Dictionary<string, FrAssetBundleConfig> AssetbundleConfigs = new Dictionary<string, FrAssetBundleConfig>();

	public static bool m_bInit;

	public static FrAssetBundleManager GetInstance()
	{
		if (instance == null)
		{
			instance = new FrAssetBundleManager();
		}
		return instance;
	}

	public FrGameObject LoadAssetBundle(string path, bool immediately)
	{
		FrAssetBundleConfig config = GetConfig(path);
		config.LoadAssetBundle(config.m_prefabDependencys[path]);
		if (immediately)
		{
			config.LoadRes();
		}
		FrGameObject frGameObject = new FrGameObject();
		frGameObject.m_pd = config.m_prefabDependencys[path];
		return frGameObject;
	}

	public void UnloadAssetBundle(string path, bool immediately)
	{
		FrAssetBundleConfig config = GetConfig(path);
		config.UnloadAssetBundle(config.m_prefabDependencys[path]);
		if (immediately)
		{
			config.UnloadRes();
		}
	}

	public void UnloadAssetBundle(PrefabDependencys pd, bool immediately)
	{
		FrAssetBundleConfig config = GetConfig(pd.m_prefab.m_name);
		config.UnloadAssetBundle(pd);
		if (immediately)
		{
			config.UnloadRes();
		}
	}

	public void UnloadAllAssetBundle(FrAssetBundleConfig config)
	{
		config.ClearRrefCount();
		config.UnloadRes();
	}

	private FrAssetBundleConfig GetConfig(string path)
	{
		string text = "assetbundles_android/" + path.Substring(0, path.LastIndexOf('/') + 1) + CONFIG_NAME;
		Debug.Log("GetConfig strPath : " + text);
		return AssetbundleConfigs[text];
	}

	public AssetBundle GetAssetbundle(string key)
	{
		FrAssetBundleConfig config = GetConfig(key);
		return config.m_prefabDependencys[key].m_prefab.m_assetBundle;
	}
}
