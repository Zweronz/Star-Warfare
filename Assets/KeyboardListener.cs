using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardListener : MonoBehaviour
{
	public TouchScreenKeyboard keyboard;

	public Action<string> onFinish;

	public string pcString;

	public static KeyboardListener current;
	
	private static Dictionary<KeyCode, string> keyMap = new Dictionary<KeyCode, string>
	{
		{KeyCode.A, "a"},
		{KeyCode.B, "b"},
		{KeyCode.C, "c"},
		{KeyCode.D, "d"},
		{KeyCode.E, "e"},
		{KeyCode.F, "f"},
		{KeyCode.G, "g"},
		{KeyCode.H, "h"},
		{KeyCode.I, "i"},
		{KeyCode.J, "j"},
		{KeyCode.K, "k"},
		{KeyCode.L, "l"},
		{KeyCode.M, "m"},
		{KeyCode.N, "n"},
		{KeyCode.O, "o"},
		{KeyCode.P, "p"},
		{KeyCode.Q, "q"},
		{KeyCode.R, "r"},
		{KeyCode.S, "s"},
		{KeyCode.T, "t"},
		{KeyCode.U, "u"},
		{KeyCode.V, "v"},
		{KeyCode.W, "w"},
		{KeyCode.X, "x"},
		{KeyCode.Y, "y"},
		{KeyCode.Z, "z"},
		{KeyCode.Space, " "},
		{KeyCode.Alpha0, "0"},
		{KeyCode.Alpha1, "1"},
		{KeyCode.Alpha2, "2"},
		{KeyCode.Alpha3, "3"},
		{KeyCode.Alpha4, "4"},
		{KeyCode.Alpha5, "5"},
		{KeyCode.Alpha6, "6"},
		{KeyCode.Alpha7, "7"},
		{KeyCode.Alpha8, "8"},
		{KeyCode.Alpha9, "9"},
		{KeyCode.Comma, ","},
		{KeyCode.Period, "."},
		{KeyCode.Slash, "/"},
		{KeyCode.Semicolon, ";"},
		{KeyCode.Quote, "'"},
		{KeyCode.LeftBracket, "["},
		{KeyCode.RightBracket, "]"},
		{KeyCode.Backslash, "\\"},
		{KeyCode.Minus, "-"},
		{KeyCode.Equals, "="},
		{KeyCode.BackQuote, "`"},
	};

	private static Dictionary<KeyCode, string> upperKeyMap = new Dictionary<KeyCode, string>
	{
		{KeyCode.Alpha0, "!"},
		{KeyCode.Alpha1, "@"},
		{KeyCode.Alpha2, "#"},
		{KeyCode.Alpha3, "$"},
		{KeyCode.Alpha4, "%"},
		{KeyCode.Alpha5, "^"},
		{KeyCode.Alpha6, "&"},
		{KeyCode.Alpha7, "*"},
		{KeyCode.Alpha8, "("},
		{KeyCode.Alpha9, ")"},
		{KeyCode.Comma, "<"},
		{KeyCode.Period, ">"},
		{KeyCode.Slash, "?"},
		{KeyCode.Semicolon, ":"},
		{KeyCode.Quote, "\""},
		{KeyCode.LeftBracket, "{"},
		{KeyCode.RightBracket, "}"},
		{KeyCode.Backslash, "\\"},
		{KeyCode.Minus, "_"},
		{KeyCode.Equals, "+"},
		{KeyCode.BackQuote, "~"},
	};

	void Update()
	{
		if (Application.isMobilePlatform)
		{
			if (keyboard.status == TouchScreenKeyboard.Status.Done)
			{
				onFinish(keyboard.text);

				Destroy(gameObject);
				current = null;

				return;
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				onFinish(pcString);

				Destroy(gameObject);
				current = null;
				
				return;
			}

			ListenForKeys();
		}
	}

	void OnGUI()
	{
		//I can improve this later or something I really don't feel like it
		GUI.Label(new Rect(Screen.width / 2f, Screen.height / 2f, Screen.width, Screen.height), pcString);
	}

	//GOOD ENOUGH!!
	private void ListenForKeys()
	{
		bool upper = Input.GetKey(KeyCode.LeftShift);

		foreach (KeyCode key in keyMap.Keys)
		{
			if (Input.GetKeyDown(key))
			{
				if (upper)
				{
					pcString += upperKeyMap.ContainsKey(key) ? upperKeyMap[key] : keyMap[key].ToUpper();
				}
				else
				{
					pcString += keyMap[key];
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Backspace) && pcString.Length > 0)
		{
			pcString = pcString.Substring(0, pcString.Length - 1);
		}
	}
}
