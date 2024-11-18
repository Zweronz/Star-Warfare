using System.Collections.Generic;
using UnityEngine;

internal class WeaponResourceConfig
{
	public static Renderer[] GetWeaponRenderers(GameObject weaponObj, int gunID)
	{
		List<Renderer> list = new List<Renderer>();
		switch (gunID)
		{
		case 24:
		case 25:
			list.Add(weaponObj.transform.GetChild(1).GetComponent<Renderer>());
			break;
		case 39:
			list.Add(weaponObj.transform.GetChild(1).GetComponent<Renderer>());
			list.Add(weaponObj.transform.GetChild(1).GetChild(0).GetComponent<Renderer>());
			break;
		case 28:
			list.Add(weaponObj.transform.GetChild(0).GetComponent<Renderer>());
			list.Add(weaponObj.transform.GetChild(1).GetComponent<Renderer>());
			break;
		case 31:
			list.Add(weaponObj.transform.Find("CRAB").GetComponent<Renderer>());
			break;
		case 32:
			list.Add(weaponObj.transform.Find("Morpheus").GetComponent<Renderer>());
			break;
		case 36:
			list.Add(weaponObj.GetComponent<Renderer>());
			list.Add(weaponObj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>());
			break;
		case 37:
			list.Add(weaponObj.GetComponent<Renderer>());
			list.Add(weaponObj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>());
			list.Add(weaponObj.transform.GetChild(0).GetChild(1).GetComponent<Renderer>());
			break;
		case 41:
			list.Add(weaponObj.transform.Find("Gun").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Gun01").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("halo").GetComponent<Renderer>());
			break;
		case 43:
			list.Add(weaponObj.GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Point02").GetChild(0).GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Point03").GetChild(0).GetComponent<Renderer>());
			break;
		case 45:
			list.Add(weaponObj.transform.Find("Object02").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object02/Point02/Object01").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object02/Point03/Object03").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object02/Point04/Object08").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object02/Point05/Object05").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object02/Point06/Object06").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object02/Point07/Object07").GetComponent<Renderer>());
			break;
		case 46:
			list.Add(weaponObj.transform.Find("Object025").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object026").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object025/Point002/Object029").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object025/Point003/Object030").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object025/Point004/Object028").GetComponent<Renderer>());
			list.Add(weaponObj.transform.Find("Object025/Point005/Object027").GetComponent<Renderer>());
			break;
		default:
			list.Add(weaponObj.GetComponent<Renderer>());
			break;
		}
		return list.ToArray();
	}

	public static float GetBagSize(int bagID)
	{
		float result = 1f;
		switch (bagID)
		{
		case 14:
			result = 1.2f;
			break;
		case 15:
			result = 1.2f;
			break;
		case 16:
			result = 1.2f;
			break;
		case 17:
			result = 1.2f;
			break;
		case 18:
			result = 1.1f;
			break;
		case 19:
			result = 1.2f;
			break;
		case 20:
			result = 1.2f;
			break;
		case 42:
			result = 1.5f;
			break;
		}
		return result;
	}

	public static void RotateGun(GameObject weaponObj, int gunID)
	{
		switch (gunID)
		{
		case 22:
		case 23:
		case 24:
		case 25:
		case 28:
		case 31:
		case 32:
		case 39:
		case 41:
		case 45:
		case 46:
			break;
		case 36:
			weaponObj.transform.Rotate(new Vector3(0f, 90f, -90f));
			break;
		case 44:
			weaponObj.transform.Rotate(new Vector3(90f, 0f, 0f));
			break;
		default:
			weaponObj.transform.Rotate(new Vector3(270f, 0f, 0f));
			break;
		}
	}

	public static void RotateGunInUI(GameObject weaponObj, int gunID)
	{
		switch (gunID)
		{
		case 22:
		case 23:
		case 24:
		case 25:
		case 31:
		case 32:
		case 41:
		case 45:
		case 46:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(-30f, 270f, 0f));
			break;
		case 39:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(-30f, 310f, 0f));
			break;
		case 27:
		case 33:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(210f, 90f, 0f));
			break;
		case 28:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(-60f, 90f, 0f));
			break;
		case 36:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 120f));
			break;
		case 42:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 120f));
			break;
		case 44:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(60f, 270f, 0f));
			break;
		default:
			weaponObj.transform.rotation = Quaternion.Euler(new Vector3(240f, 270f, 0f));
			break;
		}
	}

	public static Transform GetWeaponGunFire(GameObject weaponObj, int gunID)
	{
		Transform result = weaponObj.transform.Find("Point01");
		switch (gunID)
		{
		case 22:
			result = weaponObj.transform.GetChild(2).Find("Point01");
			break;
		case 23:
			result = weaponObj.transform.GetChild(1).Find("Point01");
			break;
		case 24:
		case 25:
		case 39:
			result = weaponObj.transform.GetChild(1).Find("Point01");
			break;
		}
		return result;
	}
}
