using System.Collections.Generic;
using UnityEngine;

public class GameUIBundle
{
	private Dictionary<string, object> mDictionary = new Dictionary<string, object>();

	public bool Add(string key, object value)
	{
		if (mDictionary.ContainsKey(key))
		{
			Debug.LogError("GameUIBundle has contained key '" + key + "'");
			return false;
		}
		if (value == null)
		{
			Debug.LogError(string.Concat("The value '", value, "' you want to add to GameUIBundle is null"));
			return false;
		}
		mDictionary.Add(key, value);
		return true;
	}

	public object Get(string key)
	{
		if (mDictionary.ContainsKey(key))
		{
			return mDictionary[key];
		}
		Debug.LogError("The key '" + key + "' does not exist in GameUIBundle.");
		return null;
	}
}
