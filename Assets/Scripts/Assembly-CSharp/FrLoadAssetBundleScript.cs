using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class FrLoadAssetBundleScript : MonoBehaviour
{
	private bool m_loading = true;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		Debug.Log("LoadAssetbundleScript start....");
		StartCoroutine(LoadGlobalConfig());
	}

	private void Update()
	{
		if (m_loading)
		{
			StartCoroutine(LoadRes());
		}
	}

	private IEnumerator LoadRes()
	{
		m_loading = false;
		FrAssetBundleManager abMgr = FrAssetBundleManager.GetInstance();
		foreach (KeyValuePair<string, FrAssetBundleConfig> assetbundleConfig in abMgr.AssetbundleConfigs)
		{
			FrAssetBundleConfig abConf = assetbundleConfig.Value;
			if (!abConf.m_loading)
			{
				continue;
			}
			abConf.UnloadRes();
			bool isLoadPrefab = true;
			bool isLoadingOver = true;
			GlobalDependency gd = abConf.m_globalDependency;
			foreach (KeyValuePair<PrefabDependencyType, List<PrefabDependency>> item in gd.m_dependency)
			{
				foreach (PrefabDependency pd2 in item.Value)
				{
					if (pd2.m_refCount > 0 && !pd2.m_isDone)
					{
						isLoadPrefab = false;
						string strPath2 = "jar:file://" + Application.dataPath + "!/assets/" + pd2.m_name;
						Debug.Log("LoadRes strPath: " + strPath2);
						WWW www2 = new WWW(strPath2)
						{
							threadPriority = gd.Priority
						};
						yield return www2;
						pd2.m_assetBundle = www2.assetBundle;
						pd2.m_isDone = true;
					}
				}
			}
			if (!isLoadPrefab)
			{
				continue;
			}
			foreach (KeyValuePair<string, PrefabDependencys> prefabDependency in abConf.m_prefabDependencys)
			{
				PrefabDependencys pd = prefabDependency.Value;
				if (pd.m_prefab.m_refCount > 0 && pd.m_prefab.m_assetBundle == null)
				{
					isLoadingOver = false;
					string strPath = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + pd.m_prefab.m_name + ".assetbundle";
					Debug.Log("LoadPrefab strPath: " + strPath);
					WWW www = new WWW(strPath)
					{
						threadPriority = gd.Priority
					};
					yield return www;
					pd.m_prefab.m_assetBundle = www.assetBundle;
					pd.m_prefab.m_isDone = true;
				}
			}
			if (isLoadingOver)
			{
				abConf.m_loading = false;
				Debug.Log("isLoading Over.....");
			}
		}
		m_loading = true;
	}

	public IEnumerator LoadGlobalConfig()
	{
		string strPath = "jar:file://" + Application.dataPath + "!/assets/assetbundles_android/" + FrAssetBundleManager.GLOBAL_CONFIG_NAME;
		Debug.Log("LoadConfigMgr strPath: " + strPath);
		WWW www = new WWW(strPath)
		{
			threadPriority = ThreadPriority.High
		};
		yield return www;
		XmlDocument xmlDoc = new XmlDocument();
		StringReader stringReader = new StringReader(www.text);
		stringReader.Read();
		XmlReader reader = XmlReader.Create(stringReader);
		xmlDoc.Load(reader);
		XmlElement root = xmlDoc.DocumentElement;
		foreach (XmlNode node in root)
		{
			FrAssetBundleConfig abConfig = new FrAssetBundleConfig();
			string path = node.Attributes["file"].Value;
			StartCoroutine(LoadConfig(path, abConfig));
			FrAssetBundleManager.GetInstance().AssetbundleConfigs.Add(path, abConfig);
			Debug.Log("LoadConfig: " + node.Name);
		}
		if (stringReader != null)
		{
			stringReader.Close();
		}
		if (reader != null)
		{
			reader.Close();
		}
	}

	private IEnumerator LoadConfig(string path, FrAssetBundleConfig abConfig)
	{
		string strPath = "jar:file://" + Application.dataPath + "!/assets/" + path;
		Debug.Log("LoadConfig strPath: " + strPath);
		WWW www = new WWW(strPath)
		{
			threadPriority = ThreadPriority.High
		};
		yield return www;
		XmlDocument xmlDoc = new XmlDocument();
		StringReader stringReader = new StringReader(www.text);
		stringReader.Read();
		XmlReader reader = XmlReader.Create(stringReader);
		xmlDoc.Load(reader);
		XmlNode node = xmlDoc.SelectSingleNode("Config/Global");
		LoadGlobalDependencys(node, abConfig);
		XmlNode setNode = xmlDoc.SelectSingleNode("Config/Global/Setting");
		abConfig.m_globalDependency.Priority = (ThreadPriority)(int)Enum.Parse(typeof(ThreadPriority), setNode.Attributes["LoadPriority"].Value);
		XmlNode prefabNode = xmlDoc.SelectSingleNode("Config/Prefabs");
		LoadPrefabDependencys(prefabNode, abConfig);
		if (stringReader != null)
		{
			stringReader.Close();
		}
		if (reader != null)
		{
			reader.Close();
		}
	}

	private void LoadGlobalDependencys(XmlNode subNode, FrAssetBundleConfig abConfig)
	{
		foreach (XmlNode item in subNode)
		{
			PrefabDependencyType prefabDependencyType = PrefabDependencyType.NONE;
			if (item.Name == "Texture")
			{
				prefabDependencyType = PrefabDependencyType.TEXTURE;
			}
			else if (item.Name == "Shader")
			{
				prefabDependencyType = PrefabDependencyType.SHADER;
			}
			else if (item.Name == "Material")
			{
				prefabDependencyType = PrefabDependencyType.MATERIAL;
			}
			Debug.Log("LoadGlobalDependencys type " + prefabDependencyType);
			if (prefabDependencyType != 0)
			{
				abConfig.m_globalDependency.m_dependency.Add(prefabDependencyType, null);
				abConfig.m_globalDependency.m_dependency[prefabDependencyType] = new List<PrefabDependency>();
				LoadGlabalDependency(item, prefabDependencyType, abConfig);
			}
		}
	}

	private void LoadGlabalDependency(XmlNode subNode, PrefabDependencyType type, FrAssetBundleConfig abConfig)
	{
		foreach (XmlNode item in subNode)
		{
			PrefabDependency prefabDependency = new PrefabDependency();
			prefabDependency.m_name = item.Attributes["file"].Value;
			prefabDependency.m_refCount = 0;
			abConfig.m_globalDependency.m_dependency[type].Add(prefabDependency);
		}
	}

	private void LoadPrefabDependencys(XmlNode subNode, FrAssetBundleConfig abConfig)
	{
		foreach (XmlNode item in subNode)
		{
			PrefabDependencys prefabDependencys = new PrefabDependencys();
			prefabDependencys.m_prefab.m_name = item.Attributes["name"].Value;
			LoadPrefabDependency(item, prefabDependencys);
			abConfig.m_prefabDependencys.Add(prefabDependencys.m_prefab.m_name, prefabDependencys);
		}
	}

	private void LoadPrefabDependency(XmlNode subNode, PrefabDependencys pd)
	{
		foreach (XmlNode item in subNode)
		{
			PrefabDependencyType prefabDependencyType = PrefabDependencyType.NONE;
			if (item.Name == "Texture")
			{
				prefabDependencyType = PrefabDependencyType.TEXTURE;
			}
			else if (item.Name == "Shader")
			{
				prefabDependencyType = PrefabDependencyType.SHADER;
			}
			else if (item.Name == "Material")
			{
				prefabDependencyType = PrefabDependencyType.MATERIAL;
			}
			pd.m_dependency.Add(prefabDependencyType, null);
			pd.m_dependency[prefabDependencyType] = new List<short>();
			LoadPrefabDependency(item, pd, prefabDependencyType);
		}
	}

	private void LoadPrefabDependency(XmlNode subNode, PrefabDependencys pd, PrefabDependencyType type)
	{
		foreach (XmlNode item in subNode)
		{
			int num = int.Parse(item.Attributes["fileIndex"].Value);
			pd.m_dependency[type].Add((short)num);
		}
	}
}
