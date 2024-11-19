using System.Collections.Generic;
using UnityEngine;

public class StateItem : ComponentUI
{
	private const byte STATE_NONE = 0;

	private const byte STATE_UP = 1;

	private const byte STATE_WAIT = 2;

	private const byte STATE_DOWN = 3;

	[SerializeField]
	private UIGrid uiGrid;

	[SerializeField]
	private BoxCollider boxCollider;

	[SerializeField]
	private GameObject itemTemple;

	[SerializeField]
	private float moveTime = 2f;

	[SerializeField]
	private float waitTime = 2.5f;

	[SerializeField]
	private float initY = -30f;

	[SerializeField]
	private float targetY = 50f;

	private bool mIsItemPop;

	private List<SWItem> mItemList = new List<SWItem>();

	private byte mNextState;

	private float mUpdateTime;

	protected override void OnInit()
	{
		base.OnInit();
		int[] itemPosition = UserStateUI.GetInstance().GetItemPosition();
		if (itemPosition != null && itemPosition.Length > 0)
		{
			int num = 0;
			for (int i = 0; i < itemPosition.Length; i++)
			{
				if (itemPosition[i] <= 80)
				{
					continue;
				}
				GameObject gameObject = Object.Instantiate(itemTemple) as GameObject;
				UISprite[] componentsInChildren = gameObject.GetComponentsInChildren<UISprite>(true);
				UISprite[] array = componentsInChildren;
				foreach (UISprite uISprite in array)
				{
					if (uISprite.gameObject.name.Equals("ItemIcon"))
					{
						uISprite.spriteName = "item_" + (itemPosition[i] - 82);
					}
				}
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localEulerAngles = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				num++;
				SWItem sWItem = new SWItem();
				sWItem.Item = gameObject;
				sWItem.indexInBag = i;
				mItemList.Add(sWItem);
				AddButtonListener(gameObject);
			}
			base.transform.localPosition -= new Vector3((float)(num - 1) * uiGrid.cellWidth / 2f, 0f, 0f);
			uiGrid.repositionNow = true;
		}
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, initY, base.transform.localPosition.z);
		boxCollider.center = new Vector3(0f - base.transform.localPosition.x, boxCollider.center.y, boxCollider.center.z);
		AddButtonListener(base.gameObject);
	}

	private void Update()
	{
		if (UserStateUI.GetInstance().IsItemListPop() && !mIsItemPop)
		{
			SwitchState(1);
		}
		if (mNextState != 0)
		{
			switch (mNextState)
			{
			case 1:
				Up(UserStateUI.GetInstance().IsItemListUpForever());
				break;
			case 2:
				Wait();
				break;
			case 3:
				Down();
				break;
			}
		}
		int[] itemPosition = UserStateUI.GetInstance().GetItemPosition();
		if (itemPosition == null || itemPosition.Length <= 0)
		{
			return;
		}
		foreach (SWItem mItem in mItemList)
		{
			if (itemPosition[mItem.indexInBag] >= 81)
			{
				continue;
			}
			UISprite[] componentsInChildren = mItem.Item.GetComponentsInChildren<UISprite>(true);
			UISprite[] array = componentsInChildren;
			foreach (UISprite uISprite in array)
			{
				if (uISprite.gameObject.name.Equals("ItemIcon"))
				{
					uISprite.gameObject.SetActive(false);
				}
			}
		}
		if (!Application.isMobilePlatform)
		{
			//again idk how many items you can have so just in case
			KeyCode[] itemKeys = new KeyCode[8]
			{
				KeyCode.E,
				KeyCode.R,
				KeyCode.T,
				KeyCode.Y,
				KeyCode.U,
				KeyCode.I,
				KeyCode.O,
				KeyCode.P
			};
			for (int i = 0; i < itemKeys.Length; i++)
			{
				if (Input.GetKeyDown(itemKeys[i]) && mItemList.Count > i)
				{
					OnClickThumb(mItemList[i].Item);
				}
			}
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (mIsItemPop)
		{
			for (int i = 0; i < mItemList.Count; i++)
			{
				if (go.Equals(mItemList[i].Item))
				{
					GameUITouchEvent gameUITouchEvent = new GameUITouchEvent();
					gameUITouchEvent.EventID = TouchEventID.HUD_Item;
					gameUITouchEvent.EventAction = TouchEventAction.Click;
					gameUITouchEvent.ArgObject = mItemList[i].indexInBag;
					AddTouchEventToGameUI(gameUITouchEvent);
				}
			}
		}
		else
		{
			GameUITouchEvent gameUITouchEvent2 = new GameUITouchEvent();
			gameUITouchEvent2.EventID = TouchEventID.HUD_Item_Pop;
			gameUITouchEvent2.EventAction = TouchEventAction.Click;
			AddTouchEventToGameUI(gameUITouchEvent2);
		}
	}

	private void Up(bool forever)
	{
		mIsItemPop = true;
		base.transform.localPosition += new Vector3(0f, (targetY - initY) * Time.deltaTime / moveTime, 0f);
		if (base.transform.localPosition.y >= targetY)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, targetY, base.transform.localPosition.z);
			if (!forever)
			{
				SwitchState(2);
			}
		}
	}

	private void Wait()
	{
		if (Time.time - mUpdateTime > waitTime)
		{
			SwitchState(3);
		}
	}

	private void Down()
	{
		base.transform.localPosition -= new Vector3(0f, (targetY - initY) * Time.deltaTime / moveTime, 0f);
		if (base.transform.localPosition.y <= initY)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, initY, base.transform.localPosition.z);
			SwitchState(0);
			mIsItemPop = false;
		}
	}

	private void SwitchState(byte state)
	{
		mNextState = state;
		mUpdateTime = Time.time;
	}
}
