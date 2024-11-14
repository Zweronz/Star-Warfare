using UnityEngine;

[ExecuteInEditMode]
public class UIRatio : MonoBehaviour
{
	public enum Type
	{
		None = 0,
		AutoOnlyGreaterThanBase = 1,
		AutoOnlyLessThanBase = 2,
		Auto = 3
	}

	public static int BASE_SCREEN_WIDTH = 960;

	public static int BASE_SCREEN_HEIGHT = 640;

	private UIRoot root;

	public Type fixedType;

	public bool refresh;

	public bool autoRefresh = true;

	private int screenWidth;

	private int screenHeight;

	private void Awake()
	{
		CalculateRatio();
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	private void Update()
	{
		if (refresh)
		{
			CalculateRatio();
			refresh = false;
		}
	}

	private void CalculateRatio()
	{
		root = GetComponent<UIRoot>();
		if (root == null)
		{
			Debug.Log("it hasn't UIRoot");
			return;
		}
		root.automatic = false;
		int width = Screen.width;
		int height = Screen.height;
		float num = (float)width / (float)height;
		float num2 = (float)BASE_SCREEN_WIDTH / (float)BASE_SCREEN_HEIGHT;
		switch (fixedType)
		{
		case Type.None:
			root.manualHeight = BASE_SCREEN_HEIGHT;
			break;
		case Type.AutoOnlyGreaterThanBase:
			if (num > num2)
			{
				root.manualHeight = BASE_SCREEN_WIDTH * height / width;
			}
			else
			{
				root.manualHeight = BASE_SCREEN_HEIGHT;
			}
			break;
		case Type.AutoOnlyLessThanBase:
			if (num < num2)
			{
				root.manualHeight = BASE_SCREEN_WIDTH * height / width;
			}
			else
			{
				root.manualHeight = BASE_SCREEN_HEIGHT;
			}
			break;
		case Type.Auto:
			root.manualHeight = BASE_SCREEN_WIDTH * height / width;
			break;
		}
	}
}
