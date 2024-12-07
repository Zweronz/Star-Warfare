using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Screen.lockCursor = !Screen.lockCursor;
		}
	}

	void OnApplicationFocus(bool focus)
	{
		if (!Application.isEditor && focus)
		{
			HUDBattle hud = FindObjectOfType<HUDBattle>();

			if (hud != null && hud.StateManager.FrGetCurrentPhase() == 6)
			{
				Screen.lockCursor = true;
			}
		}
	}
}
