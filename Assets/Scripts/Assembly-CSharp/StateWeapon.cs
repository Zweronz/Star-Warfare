using System.Collections.Generic;
using UnityEngine;

public class StateWeapon : ComponentUI, IUIDragListPlugin
{
	[SerializeField]
	private UIDragList uiDragList;

	[SerializeField]
	private GameObject weaponItem;

	[SerializeField]
	private float rate = 10f;

	private List<SWUIDraggableItem> mItemList = new List<SWUIDraggableItem>();

	private Dictionary<int, Weapon> mWeaponDic = new Dictionary<int, Weapon>();

	private int mWeaponIndex;

	protected override void OnInit()
	{
		base.OnInit();
		int weaponIndex = UserStateUI.GetInstance().GetWeaponIndex();
		List<Weapon> weaponList = UserStateUI.GetInstance().GetWeaponList();
		AddWeapon(weaponList);
		if (weaponList.Count == 2 || weaponList.Count == 3)
		{
			AddWeapon(weaponList);
		}
		uiDragList.SetIndex(weaponIndex);
		uiDragList.AddPlugin(this);
	}

	private void AddWeapon(List<Weapon> weaponList)
	{
		foreach (Weapon weapon in weaponList)
		{
			mWeaponDic.Add(mWeaponIndex, weapon);
			GameObject gameObject = Object.Instantiate(weaponItem) as GameObject;
			gameObject.name = weapon.Name;
			uiDragList.AddItem(gameObject.GetComponent<UIDraggableListItem>());
			UISprite[] componentsInChildren = gameObject.GetComponentsInChildren<UISprite>(true);
			UISprite[] array = componentsInChildren;
			foreach (UISprite uISprite in array)
			{
				if (weapon.GunID != 21)
				{
					if (uISprite.gameObject.name.Equals("Icon") || uISprite.gameObject.name.Equals("IconFill"))
					{
						uISprite.spriteName = ((weapon.GunID < 39) ? ("weapons_" + weapon.GunID) : ("weapons_" + (weapon.GunID + 1)));
					}
				}
				else if (uISprite.gameObject.name.Equals("Icon"))
				{
					uISprite.spriteName = "weapons_39";
				}
				else if (uISprite.gameObject.name.Equals("IconFill"))
				{
					uISprite.spriteName = "weapons_21";
					uISprite.fillAmount = 0f;
				}
			}
			mWeaponIndex++;
		}
	}

	public void RefreshItemList(List<UIDraggableListItem> list, int index)
	{
		mItemList = SWUIDraggableItem.CreateList(list);
		HideItemWithout(index);
	}

	public void UpdatePlugin()
	{
		int num = 0;
		foreach (SWUIDraggableItem mItem in mItemList)
		{
			float num2 = 1f - Mathf.Abs(mItem.UIDraggableListItem.transform.localPosition.y) / rate;
			num2 = ((!(num2 < 0.5f)) ? num2 : 0.5f);
			mItem.UIDraggableListItem.transform.localScale = Vector3.one * num2;
			float x = (uiDragList.ListWidth - num2 * mItem.SpriteWidth) / 2f;
			mItem.UIDraggableListItem.transform.localPosition = new Vector3(x, mItem.UIDraggableListItem.transform.localPosition.y, mItem.UIDraggableListItem.transform.localPosition.z);
			mItem.SetSpriteAlpha(num2);
			mItem.SetDepth((int)(num2 * 10f));
			if (mWeaponDic[num].GunID == 21)
			{
				LaserCannon laserCannon = (LaserCannon)mWeaponDic[num];
				float chargeEnegy = laserCannon.GetChargeEnegy();
				mItem.SetAmount(chargeEnegy / 100f);
			}
			num++;
		}
	}

	public void TouchPress()
	{
		foreach (SWUIDraggableItem mItem in mItemList)
		{
			mItem.UIDraggableListItem.gameObject.SetActive(true);
		}
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.InputController.BlockCamera = true;
	}

	public void TouchRelease(int index)
	{
		GameUITouchEvent gameUITouchEvent = new GameUITouchEvent();
		gameUITouchEvent.EventID = TouchEventID.HUD_Switch_Weapon;
		gameUITouchEvent.EventAction = TouchEventAction.Click;
		gameUITouchEvent.ArgObject = GameApp.GetInstance().GetUserState().GetWeaponBagIndex(mWeaponDic[index]);
		AddTouchEventToGameUI(gameUITouchEvent);
		HideItemWithout(index);
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.InputController.BlockCamera = false;
	}

	public void TouchClick(int index)
	{
		GameUITouchEvent gameUITouchEvent = new GameUITouchEvent();
		gameUITouchEvent.EventID = TouchEventID.HUD_Switch_Weapon;
		gameUITouchEvent.EventAction = TouchEventAction.Click;
		gameUITouchEvent.ArgObject = GameApp.GetInstance().GetUserState().GetWeaponBagIndex(mWeaponDic[index]);
		AddTouchEventToGameUI(gameUITouchEvent);
		HideItemWithout(index);
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.InputController.BlockCamera = false;
	}

	private void HideItemWithout(int index)
	{
		if (mItemList.Count > 0)
		{
			for (int num = (index + 1) % mItemList.Count; num != index; num = (num + 1) % mItemList.Count)
			{
				mItemList[num].UIDraggableListItem.gameObject.SetActive(false);
			}
		}
	}

	private void OnDisable()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player player = gameWorld.GetPlayer();
			if (player != null)
			{
				player.InputController.BlockCamera = false;
			}
		}
	}
}
