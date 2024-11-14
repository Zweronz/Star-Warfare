using UnityEngine;

public class UIDialogButton : UIPanelX, UIHandler
{
	public enum Command
	{
		BUTTON_1 = 9,
		BUTTON_2 = 10
	}

	protected UITextButton[] mButtons;

	protected int mButtonNum;

	public UIDialogButton()
	{
	}

	public UIDialogButton(int buttonNum)
	{
		mButtonNum = buttonNum;
		mButtons = new UITextButton[mButtonNum];
	}

	public void CreateButtons()
	{
		for (int i = 0; i < mButtonNum; i++)
		{
			mButtons[i] = new UITextButton();
			Add(mButtons[i]);
		}
		SetUIHandler(this);
	}

	public override void Destory()
	{
		base.Destory();
		if (mButtons == null)
		{
			return;
		}
		UITextButton[] array = mButtons;
		foreach (UITextButton uITextButton in array)
		{
			if (uITextButton != null)
			{
				uITextButton.Destory();
			}
		}
	}

	public void SetEnable(int buttonId, bool enable)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].Enable = enable;
		}
	}

	public void SetVisible(int buttonId, bool bVisible)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].Visible = bVisible;
		}
	}

	public void SetButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, int module)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetTexture(state, ui, frame, module);
			mButtons[buttonId].Rect = mButtons[buttonId].GetObjectRect(UIButtonBase.State.Normal);
		}
	}

	public void SetButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, byte[] module)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetTexture(state, ui, frame, module);
			mButtons[buttonId].Rect = mButtons[buttonId].GetObjectRect(UIButtonBase.State.Normal);
		}
	}

	public void AddButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, int module)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].AddObject(state, ui, frame, module);
			mButtons[buttonId].Rect = mButtons[buttonId].GetObjectRect(UIButtonBase.State.Normal);
		}
	}

	public void AddButton(int buttonId, UIButtonBase.State state, UnitUI ui, int frame, byte[] module)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].AddObject(state, ui, frame, module);
			mButtons[buttonId].Rect = mButtons[buttonId].GetObjectRect(UIButtonBase.State.Normal);
		}
	}

	public void AddButton(int buttonId, UnitUI ui, int normalFrame, int normalModule, int pressedFrame, int pressedModule)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].AddObject(UIButtonBase.State.Normal, ui, normalFrame, normalModule);
			mButtons[buttonId].AddObject(UIButtonBase.State.Pressed, ui, pressedFrame, pressedModule);
			mButtons[buttonId].Rect = mButtons[buttonId].GetObjectRect(UIButtonBase.State.Normal);
		}
	}

	public void AddButton(int buttonId, UnitUI ui, int normalFrame, int normalModule, int pressedFrame, int pressedModule, int disableFrame, int disableModule)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].AddObject(UIButtonBase.State.Normal, ui, normalFrame, normalModule);
			mButtons[buttonId].AddObject(UIButtonBase.State.Pressed, ui, pressedFrame, pressedModule);
			mButtons[buttonId].AddObject(UIButtonBase.State.Disabled, ui, disableFrame, disableModule);
			mButtons[buttonId].Rect = mButtons[buttonId].GetObjectRect(UIButtonBase.State.Normal);
		}
	}

	public void SetText(int buttonId, string text)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetText(text);
		}
	}

	public void SetText(int buttonId, string font, string text, Color color)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetText(font, text, color);
		}
	}

	public void SetText(int buttonId, string font, string text, Color color, float width)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetText(font, text, color, width);
		}
	}

	public void SetTextColor(int buttonId, Color normalColor, Color pressedColor)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetTextColor(normalColor, pressedColor);
		}
	}

	public void SetTextRect(int buttonId, Rect rect)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetTextRect(rect);
		}
	}

	public void SetTextFont(int buttonId, string font)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetTextFont(font);
		}
	}

	public void SetTextAlignment(int buttonId, FrUIText.enAlignStyle type)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].SetTextAlignment(type);
		}
	}

	public float GetButtonTop()
	{
		float num = 0f;
		UITextButton[] array = mButtons;
		foreach (UIClickButton uIClickButton in array)
		{
			float num2 = uIClickButton.Rect.y + uIClickButton.Rect.height;
			if (num2 > num)
			{
				num = num2;
			}
		}
		return num;
	}

	public void SetButtonPosition(int buttonId, Rect pos)
	{
		if (buttonId < mButtonNum)
		{
			mButtons[buttonId].Rect = pos;
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		for (int i = 0; i < mButtonNum; i++)
		{
			if (control == mButtons[i])
			{
				m_Parent.SendEvent(this, 9 + i, wparam, lparam);
				break;
			}
		}
	}
}
