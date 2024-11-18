using System.Collections.Generic;
using UnityEngine;

internal class AvatarBuilder
{
	protected static AvatarBuilder instance = new AvatarBuilder();

	protected UserState lastUserState;

	public static AvatarBuilder GetInstance()
	{
		return instance;
	}

	public void BindBones(SkinnedMeshRenderer smr, List<Transform> bones, GameObject parentObj)
	{
		Transform[] array = new Transform[smr.bones.Length];
		for (int i = 0; i < smr.bones.Length; i++)
		{
			string name = smr.bones[i].name;
			for (int j = 0; j < bones.Count; j++)
			{
				if (name == bones[j].name)
				{
					array[i] = bones[j];
					break;
				}
			}
		}
		smr.bones = array;
		smr.transform.parent = parentObj.transform;
	}

	public void TraverseBones(Transform t, List<Transform> bones)
	{
		bones.Add(t);
		if (t.childCount > 0)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				TraverseBones(t.GetChild(i), bones);
			}
		}
	}

	public void AddAnimation(string name, GameObject playerObject)
	{
		GameObject gameObject = Resources.Load("Avatar/animation/" + name) as GameObject;
		playerObject.GetComponent<Animation>().AddClip(gameObject.GetComponent<Animation>()[name].clip, name);
	}

	public void AddAnimations(GameObject player, Player p)
	{
		List<Weapon> battleWeapons = lastUserState.GetBattleWeapons();
		HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
		foreach (Weapon item in battleWeapons)
		{
			hashSet.Add(item.GetWeaponType());
		}
		bool flag = p.GetSkills().GetSkill(SkillsType.FLY) > 0f;
		AddAnimation("attacked", player);
		AddAnimation("attacked_back", player);
		AddAnimation("dead", player);
		if (flag)
		{
			AddAnimation("fly_idle", player);
			AddAnimation("fly_front", player);
			AddAnimation("fly_back", player);
			AddAnimation("fly_left", player);
			AddAnimation("fly_right", player);
		}
		AddAnimation("idle_rifle", player);
		AddAnimation("run_rifle", player);
		if (hashSet.Contains(WeaponType.AssaultRifle) || hashSet.Contains(WeaponType.AdvancedAssaultRifle) || hashSet.Contains(WeaponType.LaserGun) || hashSet.Contains(WeaponType.LaserRifle) || hashSet.Contains(WeaponType.PlasmaNeo))
		{
			AddAnimation("run_shoot_rifle", player);
			if (flag)
			{
				AddAnimation("fly_idle_rifle", player);
				AddAnimation("fly_rifle", player);
				AddAnimation("fly_stand_shoot_rifle", player);
			}
			else
			{
				AddAnimation("stand_shoot_rifle", player);
			}
		}
		if (hashSet.Contains(WeaponType.ShotGun) || hashSet.Contains(WeaponType.GrenadeLauncher) || hashSet.Contains(WeaponType.AdvancedGrenadeLauncher) || hashSet.Contains(WeaponType.AdvancedShotGun) || hashSet.Contains(WeaponType.PingPongLauncher) || hashSet.Contains(WeaponType.FlyGrenadeLauncher))
		{
			AddAnimation("run_shoot_shotgun", player);
			if (flag)
			{
				AddAnimation("fly_shotgun", player);
				AddAnimation("fly_idle_shotgun", player);
				AddAnimation("fly_stand_shoot_shotgun", player);
			}
			else
			{
				AddAnimation("idle_shotgun", player);
				AddAnimation("run_shotgun", player);
				AddAnimation("stand_shoot_shotgun", player);
			}
		}
		if (hashSet.Contains(WeaponType.RocketLauncher) || hashSet.Contains(WeaponType.AutoRocketLauncher))
		{
			AddAnimation("run_shoot_bazinga", player);
			if (flag)
			{
				AddAnimation("fly_bazinga", player);
				AddAnimation("fly_idle_bazinga", player);
				AddAnimation("fly_stand_shoot_bazinga", player);
			}
			else
			{
				AddAnimation("idle_bazinga", player);
				AddAnimation("run_bazinga", player);
				AddAnimation("stand_shoot_bazinga", player);
			}
		}
		if (hashSet.Contains(WeaponType.Sword) || hashSet.Contains(WeaponType.AdvancedSword))
		{
			AddAnimation("run_shoot_jian", player);
			if (flag)
			{
				AddAnimation("fly_jian", player);
				AddAnimation("fly_idle_jian", player);
				AddAnimation("fly_stand_shoot_jian", player);
				AddAnimation("fly_runshoot_jian", player);
				AddAnimation("fly_stand_shoot_jian_lower", player);
			}
			else
			{
				AddAnimation("idle_jian", player);
				AddAnimation("run_jian", player);
				AddAnimation("stand_shoot_jian", player);
			}
		}
		if (hashSet.Contains(WeaponType.GrenadeLauncher) || hashSet.Contains(WeaponType.AdvancedGrenadeLauncher) || hashSet.Contains(WeaponType.AdvancedShotGun) || hashSet.Contains(WeaponType.PingPongLauncher) || hashSet.Contains(WeaponType.FlyGrenadeLauncher))
		{
			AddAnimation("run_shoot_grenade_launcher", player);
			if (flag)
			{
				AddAnimation("fly_stand_shoot_grenade_launcher", player);
			}
			else
			{
				AddAnimation("stand_shoot_grenade_launcher", player);
			}
		}
		if (hashSet.Contains(WeaponType.AutoRocketLauncher))
		{
			AddAnimation("run_shoot_BLACKSTARS", player);
			if (flag)
			{
				AddAnimation("fly_stand_shoot_BLACKSTARS", player);
			}
			else
			{
				AddAnimation("stand_shoot_BLACKSTARS", player);
			}
		}
		if (hashSet.Contains(WeaponType.LaserGun))
		{
			AddAnimation("run_shoot_laser", player);
			if (flag)
			{
				AddAnimation("fly_stand_shoot_laser", player);
			}
			else
			{
				AddAnimation("stand_shoot_laser", player);
			}
		}
		if (hashSet.Contains(WeaponType.LightBow) || hashSet.Contains(WeaponType.AutoBow) || hashSet.Contains(WeaponType.TheArrow))
		{
			AddAnimation("run_shoot_bow", player);
			if (flag)
			{
				AddAnimation("fly_bow", player);
				AddAnimation("fly_idle_bow", player);
				AddAnimation("fly_stand_shoot_bow", player);
			}
			else
			{
				AddAnimation("idle_bow", player);
				AddAnimation("run_bow", player);
				AddAnimation("stand_shoot_bow", player);
			}
		}
		if (hashSet.Contains(WeaponType.LightFist) || hashSet.Contains(WeaponType.TrackingGun))
		{
			AddAnimation("run_shoot_fist", player);
			if (flag)
			{
				AddAnimation("fly_fist", player);
				AddAnimation("fly_idle_fist", player);
				AddAnimation("fly_stand_shoot_fist", player);
			}
			else
			{
				AddAnimation("idle_fist", player);
				AddAnimation("run_fist", player);
				AddAnimation("stand_shoot_fist", player);
			}
		}
		if (hashSet.Contains(WeaponType.MachineGun) || hashSet.Contains(WeaponType.AdvancedMachineGun))
		{
			AddAnimation("run_shoot_machinegun", player);
			if (flag)
			{
				AddAnimation("fly_machinegun", player);
				AddAnimation("fly_idle_machinegun", player);
				AddAnimation("fly_stand_shoot_machinegun", player);
				AddAnimation("fly_runshoot_machinegun", player);
			}
			else
			{
				AddAnimation("idle_machinegun", player);
				AddAnimation("run_machinegun", player);
				AddAnimation("stand_shoot_machinegun", player);
			}
		}
		if (hashSet.Contains(WeaponType.Sniper) || hashSet.Contains(WeaponType.AdvancedSniper) || hashSet.Contains(WeaponType.RelectionSniper))
		{
			AddAnimation("run_shoot_Sniper", player);
			if (flag)
			{
				AddAnimation("fly_stand_shoot_Sniper", player);
				AddAnimation("fly_Sniper", player);
				AddAnimation("fly_idle_Sniper", player);
			}
			else
			{
				AddAnimation("idle_Sniper", player);
				AddAnimation("run_Sniper", player);
				AddAnimation("stand_shoot_Sniper", player);
			}
		}
		AddAnimation("win", player);
		AddAnimation("win01", player);
		AddAnimation("win_idle", player);
		AddAnimation("idle01_machinegun", player);
	}

	public void AddAnimationsForUI(GameObject player)
	{
		lastUserState = null;
		AddAnimation("fly_idle", player);
		AddAnimation("idle_rifle", player);
		AddAnimation("fly_idle_rifle", player);
		AddAnimation("idle_shotgun", player);
		AddAnimation("fly_idle_shotgun", player);
		AddAnimation("idle_bazinga", player);
		AddAnimation("fly_idle_bazinga", player);
		AddAnimation("idle_jian", player);
		AddAnimation("fly_idle_jian", player);
		AddAnimation("idle_bow", player);
		AddAnimation("fly_idle_bow", player);
		AddAnimation("idle_fist", player);
		AddAnimation("fly_idle_fist", player);
		AddAnimation("idle_machinegun", player);
		AddAnimation("fly_idle_machinegun", player);
		AddAnimation("idle_Sniper", player);
		AddAnimation("fly_idle_Sniper", player);
		AddAnimation("idle01", player);
		AddAnimation("fly_idle01", player);
		AddAnimation("idle01_machinegun", player);
		AddAnimation("fly_idle01_machinegun", player);
	}

	public GameObject ReBuildAvatar(UserState userState, Player p)
	{
		int[] array = new int[5] { 1, 1, 1, 1, 1 };
		if (userState != null)
		{
			array = userState.GetAvatar();
		}
		lastUserState = userState;
		Object original = Resources.Load("Avatar/Bone");
		Object original2 = Resources.Load(string.Format("Avatar/{0:D2}/Head", array[0] + 1));
		Object original3 = Resources.Load(string.Format("Avatar/{0:D2}/Body", array[1] + 1));
		Object original4 = Resources.Load(string.Format("Avatar/{0:D2}/Hand", array[2] + 1));
		Object original5 = Resources.Load(string.Format("Avatar/{0:D2}/Foot", array[3] + 1));
		Object original6 = Resources.Load(string.Format("Avatar/{0:D2}/Bag", array[4] + 1));
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		GameObject gameObject2 = Object.Instantiate(original3) as GameObject;
		GameObject gameObject3 = Object.Instantiate(original2) as GameObject;
		GameObject gameObject4 = Object.Instantiate(original4) as GameObject;
		GameObject gameObject5 = Object.Instantiate(original5) as GameObject;
		GameObject gameObject6 = Object.Instantiate(original6) as GameObject;
		if (array[1] == 5)
		{
			gameObject6.transform.localScale = Vector3.one * WeaponResourceConfig.GetBagSize(array[4]);
		}
		else
		{
			gameObject6.transform.localScale = Vector3.one * WeaponResourceConfig.GetBagSize(array[4]) * 0.8f;
		}
		if (array[4] == 15 || array[4] == 23)
		{
			FlyBagAnimationScript component = gameObject6.GetComponent<FlyBagAnimationScript>();
		}
		List<Transform> list = new List<Transform>();
		Transform transform = gameObject.transform;
		TraverseBones(transform.GetChild(0), list);
		for (int i = 0; i < list.Count; i++)
		{
		}
		SkinnedMeshRenderer component2 = gameObject2.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component3 = gameObject3.transform.Find("head").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component4 = gameObject4.transform.Find("hand").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component5 = gameObject5.transform.Find("foot").GetComponent<SkinnedMeshRenderer>();
		BindBones(component2, list, gameObject);
		BindBones(component3, list, gameObject);
		BindBones(component4, list, gameObject);
		BindBones(component5, list, gameObject);
		gameObject6.transform.position = gameObject.transform.Find(BoneName.Bag).position;
		if (array[1] == 5)
		{
			gameObject6.transform.position = gameObject.transform.Find(BoneName.Bag).position + Vector3.forward * -0.05f + Vector3.up * 0.03f;
		}
		gameObject6.transform.parent = gameObject.transform.Find(BoneName.Bag);
		gameObject6.name = "Bag";
		Object.Destroy(gameObject2);
		Object.Destroy(gameObject3);
		Object.Destroy(gameObject4);
		Object.Destroy(gameObject5);
		gameObject.transform.position = new Vector3(4f, 0f, -5f);
		if (array[1] == 20)
		{
			Transform transform2 = transform.Find(BoneName.ClavicleL);
			if (transform2 != null)
			{
				GameObject original7 = Resources.Load("SW3_Effect/C12_Low_jianjia_L") as GameObject;
				GameObject gameObject7 = Object.Instantiate(original7) as GameObject;
				gameObject7.transform.parent = transform2;
				gameObject7.transform.localPosition = Vector3.zero;
				gameObject7.transform.localRotation = Quaternion.identity;
			}
			Transform transform3 = transform.Find(BoneName.ClavicleR);
			if (transform3 != null)
			{
				GameObject original8 = Resources.Load("SW3_Effect/C12_Low_jianjia_R") as GameObject;
				GameObject gameObject8 = Object.Instantiate(original8) as GameObject;
				gameObject8.transform.parent = transform3;
				gameObject8.transform.localPosition = Vector3.zero;
				gameObject8.transform.localRotation = Quaternion.identity;
			}
		}
		AddAnimations(gameObject, p);
		gameObject.GetComponent<Animation>().Play(AnimationString.Idle + "_rifle");
		return gameObject;
	}

	public GameObject CreateUIAvatar(int[] avatar)
	{
		Object original = Resources.Load("Avatar/Bone");
		Object original2 = Resources.Load(string.Format("NoMipMapRes/Avatar/{0:D2}/Head", avatar[0] + 1));
		Object original3 = Resources.Load(string.Format("NoMipMapRes/Avatar/{0:D2}/Body", avatar[1] + 1));
		Object original4 = Resources.Load(string.Format("NoMipMapRes/Avatar/{0:D2}/Hand", avatar[2] + 1));
		Object original5 = Resources.Load(string.Format("NoMipMapRes/Avatar/{0:D2}/Foot", avatar[3] + 1));
		Object original6 = Resources.Load(string.Format("NoMipMapRes/Avatar/{0:D2}/Bag", avatar[4] + 1));
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		GameObject gameObject2 = Object.Instantiate(original3) as GameObject;
		GameObject gameObject3 = Object.Instantiate(original2) as GameObject;
		GameObject gameObject4 = Object.Instantiate(original4) as GameObject;
		GameObject gameObject5 = Object.Instantiate(original5) as GameObject;
		GameObject gameObject6 = Object.Instantiate(original6) as GameObject;
		if (avatar[1] == 5)
		{
			gameObject6.transform.localScale = Vector3.one * WeaponResourceConfig.GetBagSize(avatar[4]);
		}
		else
		{
			gameObject6.transform.localScale = Vector3.one * WeaponResourceConfig.GetBagSize(avatar[4]) * 0.8f;
		}
		if (avatar[4] == 15 || avatar[4] == 23)
		{
			FlyBagAnimationScript component = gameObject6.GetComponent<FlyBagAnimationScript>();
		}
		List<Transform> list = new List<Transform>();
		Transform transform = gameObject.transform;
		TraverseBones(transform.GetChild(0), list);
		for (int i = 0; i < list.Count; i++)
		{
		}
		SkinnedMeshRenderer component2 = gameObject2.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component3 = gameObject3.transform.Find("head").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component4 = gameObject4.transform.Find("hand").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component5 = gameObject5.transform.Find("foot").GetComponent<SkinnedMeshRenderer>();
		BindBones(component2, list, gameObject);
		BindBones(component3, list, gameObject);
		BindBones(component4, list, gameObject);
		BindBones(component5, list, gameObject);
		gameObject6.transform.position = gameObject.transform.Find(BoneName.Bag).position;
		if (avatar[1] == 5)
		{
			gameObject6.transform.position = gameObject.transform.Find(BoneName.Bag).position + Vector3.forward * -0.05f + Vector3.up * 0.03f;
		}
		gameObject6.transform.parent = gameObject.transform.Find(BoneName.Bag);
		gameObject6.name = "Bag";
		Object.Destroy(gameObject2);
		Object.Destroy(gameObject3);
		Object.Destroy(gameObject4);
		Object.Destroy(gameObject5);
		if (avatar[1] == 20)
		{
			Transform transform2 = transform.Find(BoneName.ClavicleL);
			if (transform2 != null)
			{
				GameObject original7 = Resources.Load("SW3_Effect/C12_Low_jianjia_L") as GameObject;
				GameObject gameObject7 = Object.Instantiate(original7) as GameObject;
				gameObject7.transform.parent = transform2;
				gameObject7.transform.localPosition = Vector3.zero;
				gameObject7.transform.localRotation = Quaternion.identity;
			}
			Transform transform3 = transform.Find(BoneName.ClavicleR);
			if (transform3 != null)
			{
				GameObject original8 = Resources.Load("SW3_Effect/C12_Low_jianjia_R") as GameObject;
				GameObject gameObject8 = Object.Instantiate(original8) as GameObject;
				gameObject8.transform.parent = transform3;
				gameObject8.transform.localPosition = Vector3.zero;
				gameObject8.transform.localRotation = Quaternion.identity;
			}
		}
		AddAnimationsForUI(gameObject);
		return gameObject;
	}

	public GameObject ReBuildPlayerAvatar(UserState userState, Player p)
	{
		GameObject gameObject = ReBuildAvatar(userState, p);
		gameObject.AddComponent<CharacterController>().center = new Vector3(0f, 1f, 0f);
		GameObject original = Resources.Load("Effect/Shadow") as GameObject;
		GameObject gameObject2 = Object.Instantiate(original) as GameObject;
		gameObject2.transform.position = gameObject.transform.position;
		gameObject2.transform.localScale = Vector3.one * 0.15f;
		gameObject2.transform.parent = gameObject.transform;
		return gameObject;
	}
}
