using System.Collections;
using UnityEngine;

public class FADScript : MonoBehaviour
{
	private Texture2D texture;

	private IEnumerator Start()
	{
		WWW www = new WWW("http://unity3d.com/images/fig/documentation.jpg");
		yield return www;
		texture = www.texture;
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if (texture != null && GUI.Button(new Rect(0f, 0f, texture.width, texture.height), texture, GUIStyle.none))
		{
			Application.OpenURL(AndroidSwPluginScript.GetVersionUrl());
		}
	}
}
