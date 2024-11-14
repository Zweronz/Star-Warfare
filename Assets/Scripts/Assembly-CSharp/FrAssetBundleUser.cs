using UnityEngine;

public class FrAssetBundleUser : MonoBehaviour
{
	private static FrAssetBundleUser mInstance;

	private void Start()
	{
		mInstance = this;
	}

	private void Destroy()
	{
		mInstance = null;
	}

	public static FrAssetBundleUser GetInstance()
	{
		return mInstance;
	}

	public void Create(FrGameObject frObject, string path, bool immediately)
	{
	}
}
