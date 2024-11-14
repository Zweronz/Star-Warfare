using UnityEngine;

public class WeaponFactory
{
	protected static WeaponFactory instance;

	protected static WeaponType[] weaponIDToWeaponType = new WeaponType[48];

	public static WeaponFactory GetInstance()
	{
		if (instance == null)
		{
			instance = new WeaponFactory();
		}
		return instance;
	}

	public Weapon CreateWeapon(byte weaponID)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[13];
		Weapon result = null;
		if (unitDataTable != null)
		{
			byte wType = (byte)unitDataTable.GetData(weaponID, 11, 0, false);
			result = CreateWeapon((WeaponType)wType, weaponID);
		}
		return result;
	}

	public Weapon CreateWeapon(WeaponType wType, byte wID)
	{
		Weapon weapon = null;
		switch (wType)
		{
		case WeaponType.AssaultRifle:
			weapon = new AssaultRifle();
			break;
		case WeaponType.ShotGun:
			weapon = new ShotGun();
			break;
		case WeaponType.RocketLauncher:
			weapon = new RocketLauncher();
			break;
		case WeaponType.GrenadeLauncher:
			weapon = new GrenadeLauncher();
			break;
		case WeaponType.PlasmaNeo:
			weapon = new PlasmaNeo();
			break;
		case WeaponType.LaserGun:
			weapon = ((wID != 38) ? ((Weapon)new LaserCannon()) : ((Weapon)new SnowGun()));
			break;
		case WeaponType.LightBow:
			weapon = new LightBow();
			break;
		case WeaponType.LightFist:
			weapon = new LightFist();
			break;
		case WeaponType.MachineGun:
			weapon = new MachineGun();
			break;
		case WeaponType.LaserRifle:
			weapon = new LaserRifle();
			break;
		case WeaponType.AutoBow:
			weapon = new AutoBow();
			break;
		case WeaponType.AutoRocketLauncher:
			weapon = new AutoRocketLauncher();
			break;
		case WeaponType.Sword:
			weapon = new Sword();
			break;
		case WeaponType.AdvancedShotGun:
			weapon = new AdvancedShotGun();
			break;
		case WeaponType.Sniper:
			weapon = new Sniper();
			break;
		case WeaponType.AdvancedSword:
			weapon = new AdvancedSword();
			break;
		case WeaponType.AdvancedSniper:
			weapon = new AdvancedSniper();
			break;
		case WeaponType.TrackingGun:
			weapon = new TrackingGun();
			break;
		case WeaponType.PingPongLauncher:
			weapon = new PingPongLauncher();
			break;
		case WeaponType.AdvancedMachineGun:
			weapon = new AdvancedMachineGun();
			break;
		case WeaponType.AdvancedAssaultRifle:
			weapon = new AdvancedAssaultRifle();
			break;
		case WeaponType.AdvancedGrenadeLauncher:
			weapon = new AdvancedGrenadeLauncher();
			break;
		case WeaponType.Spring:
			weapon = new Spring();
			break;
		case WeaponType.RelectionSniper:
			weapon = new ReflectionSniper();
			break;
		case WeaponType.TheArrow:
			weapon = new WeaponTheArrow();
			Debug.Log("case wID = " + wID);
			break;
		case WeaponType.FlyGrenadeLauncher:
			weapon = new FlyGrenadeLauncher();
			break;
		}
		weapon.GunID = wID;
		return weapon;
	}
}
