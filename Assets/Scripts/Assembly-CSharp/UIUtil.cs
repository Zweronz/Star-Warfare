using System.Collections.Generic;
using UnityEngine;

public class UIUtil
{
	public int armorIndex;

	public int subArmorIndex;

	public int weaponIndex;

	public bool loadNoBreak;

	public UIUtil()
	{
		armorIndex = 0;
		subArmorIndex = 0;
		weaponIndex = 0;
		loadNoBreak = false;
	}

	public bool LoadAllAvatar(int type, bool active, bool isShop)
	{
		List<List<Armor>> armor = GameApp.GetInstance().GetUserState().GetArmor();
		Shader shader = Shader.Find("iPhone/SolidTexture_Color");
		Shader shader2 = Shader.Find("iPhone/AlphaBlend_Color_TwoSides");
		Shader shader3 = Shader.Find("iPhone/SolidAndAlphaTexture");
		Shader shader4 = Shader.Find("iPhone/Additive");
		Object original = Resources.Load("UI/lock");
		Object original2 = Resources.Load("UI/unlock");
		Object original3 = Resources.Load("UI/unlock1");
		if (type < 5)
		{
			List<Armor> list = armor[type];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Equipment != null || (!isShop && (list[i].Level == 0 || list[i].Level == 15)))
				{
					continue;
				}
				list[i].CreateArmor();
				GameObject gameObject = list[i].Equipment;
				if (type == 1 && i == 20)
				{
					Transform transform = gameObject.transform.Find(BoneName.ClavicleL);
					if (transform != null)
					{
						GameObject original4 = Resources.Load("SW3_Effect/C12_Low_jianjia_L") as GameObject;
						GameObject gameObject2 = Object.Instantiate(original4) as GameObject;
						gameObject2.transform.parent = transform;
						gameObject2.transform.localPosition = Vector3.zero;
						gameObject2.transform.localRotation = Quaternion.identity;
					}
					Transform transform2 = gameObject.transform.Find(BoneName.ClavicleR);
					if (transform2 != null)
					{
						GameObject original5 = Resources.Load("SW3_Effect/C12_Low_jianjia_R") as GameObject;
						GameObject gameObject3 = Object.Instantiate(original5) as GameObject;
						gameObject3.transform.parent = transform2;
						gameObject3.transform.localPosition = Vector3.zero;
						gameObject3.transform.localRotation = Quaternion.identity;
					}
				}
				Transform transform3 = gameObject.transform.Find(UIConstant.SUB_AVATAR[type]);
				GameObject gameObject4 = Object.Instantiate(original) as GameObject;
				gameObject4.transform.parent = gameObject.transform;
				GameObject gameObject5 = Object.Instantiate(original2) as GameObject;
				gameObject5.transform.parent = gameObject.transform;
				GameObject gameObject6 = Object.Instantiate(original3) as GameObject;
				gameObject6.transform.parent = gameObject.transform;
				if (transform3 != null)
				{
					gameObject = transform3.gameObject;
				}
				if (type == Global.AVATAR_PART_NUM - 1 || i >= 4)
				{
				}
				BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
				list[i].Center = boxCollider.center;
				list[i].Equipment.SetActive(active);
			}
		}
		else
		{
			List<Weapon> weapons = GameApp.GetInstance().GetUserState().GetWeapons();
			for (int j = weaponIndex; j < weapons.Count; j++)
			{
				Weapon weapon = weapons[j];
				if (!(weapon.Gun != null) && (isShop || (weapon.Level != 0 && weapon.Level != 15)))
				{
					weapon.CreateGunForUI();
					GameObject gameObject7 = weapon.Gun;
					switch (j)
					{
					case 23:
						gameObject7 = weapon.Gun.transform.GetChild(0).gameObject;
						break;
					case 24:
					case 25:
					case 39:
						gameObject7 = weapon.Gun.transform.GetChild(1).gameObject;
						break;
					case 22:
						gameObject7 = weapon.Gun.transform.GetChild(1).gameObject;
						break;
					case 44:
						gameObject7 = weapon.Gun.transform.GetChild(1).gameObject;
						break;
					}
					GameObject gameObject8 = Object.Instantiate(original) as GameObject;
					gameObject8.transform.parent = weapon.Gun.transform;
					GameObject gameObject9 = Object.Instantiate(original2) as GameObject;
					gameObject9.transform.parent = weapon.Gun.transform;
					GameObject gameObject10 = Object.Instantiate(original3) as GameObject;
					gameObject10.transform.parent = weapon.Gun.transform;
					BoxCollider boxCollider2 = gameObject7.AddComponent<BoxCollider>();
					weapon.Center = boxCollider2.center;
					weapon.Gun.SetActive(active);
				}
			}
		}
		return true;
	}

	public void SetShader(GameObject obj, Shader shader)
	{
		Material[] materials = obj.GetComponent<Renderer>().materials;
		foreach (Material material in materials)
		{
			Texture texture = material.mainTexture;
			if (texture == null)
			{
				texture = material.GetTexture("_MainTex");
			}
			material.shader = shader;
			material.SetTexture("_MainTex", texture);
		}
	}

	public void FreeAllAvatar()
	{
		List<List<Armor>> armor = GameApp.GetInstance().GetUserState().GetArmor();
		for (int i = 0; i < armor.Count; i++)
		{
			List<Armor> list = armor[i];
			for (int j = 0; j < list.Count; j++)
			{
				if (!(list[j].Equipment == null))
				{
					Object.Destroy(list[j].Equipment);
					list[j].Equipment = null;
				}
			}
		}
		List<Weapon> weapons = GameApp.GetInstance().GetUserState().GetWeapons();
		for (int k = 0; k < weapons.Count; k++)
		{
			if (!(weapons[k].Gun == null))
			{
				Object.Destroy(weapons[k].Gun);
				weapons[k].Gun = null;
			}
		}
	}
}
