using UnityEngine;

public class ScaleFullScreenScript : MonoBehaviour
{
	private UIRoot uiRoot;

	private void Start()
	{
		uiRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		float num = uiRoot.activeHeight;
		base.transform.localScale = new Vector3(num * (float)Screen.width / (float)Screen.height, num, base.transform.localScale.z);
	}
}
