using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class GameConfig
{
	public GlobalConfig globalConf;

	public PlayerConfig playerConf;

	public ArrayList avatarConfTable = new ArrayList();

	public Hashtable monsterConfTable = new Hashtable();

	public ArrayList weaponConfTable = new ArrayList();

	public Hashtable equipConfTable = new Hashtable();

	public MonsterConfig GetMonsterConfig(string name)
	{
		return monsterConfTable[name] as MonsterConfig;
	}

	public WeaponConfig GetWeaponConfig(string name)
	{
		foreach (WeaponConfig item in weaponConfTable)
		{
			if (item.name == name)
			{
				return item;
			}
		}
		return null;
	}

	public AvatarConfig GetAvatarConfig(int index)
	{
		return avatarConfTable[index - 1] as AvatarConfig;
	}

	public List<WeaponConfig> GetPossibleLootWeapons(int wave)
	{
		List<WeaponConfig> list = new List<WeaponConfig>();
		foreach (WeaponConfig item in weaponConfTable)
		{
			LootConfig lootConf = item.lootConf;
			if (wave >= lootConf.fromWave && wave <= lootConf.toWave)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public WeaponConfig GetUnLockWeapon(int wave)
	{
		foreach (WeaponConfig item in weaponConfTable)
		{
			LootConfig lootConf = item.lootConf;
			if (wave == lootConf.giveAtWave)
			{
				return item;
			}
		}
		return null;
	}

	public List<WeaponConfig> GetWeapons()
	{
		List<WeaponConfig> list = new List<WeaponConfig>();
		foreach (WeaponConfig item in weaponConfTable)
		{
			list.Add(item);
		}
		return list;
	}

	public void LoadFromXML(string path)
	{
		globalConf = new GlobalConfig();
		playerConf = new PlayerConfig();
		XmlReader xmlReader = null;
		StringReader stringReader = null;
		Stream stream = null;
		if (path != null)
		{
			path = Application.dataPath + path;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			stream = File.Open(path + "config.xml", FileMode.Open);
			xmlReader = XmlReader.Create(stream);
		}
		else
		{
			TextAsset textAsset = Resources.Load("Config/config") as TextAsset;
			stringReader = new StringReader(textAsset.text);
			xmlReader = XmlReader.Create(stringReader);
		}
		WeaponConfig weaponConfig = null;
		AvatarConfig avatarConfig = null;
		while (xmlReader.Read())
		{
			switch (xmlReader.NodeType)
			{
			case XmlNodeType.Element:
				if (xmlReader.Name == "Global")
				{
					LoadGlobalConf(xmlReader);
				}
				else if (xmlReader.Name == "Player")
				{
					LoadPlayerConf(xmlReader);
				}
				else if (xmlReader.Name == "Avatar")
				{
					avatarConfig = new AvatarConfig();
					LoadAvatarConf(xmlReader, avatarConfig);
					avatarConfTable.Add(avatarConfig);
				}
				else if (xmlReader.Name == "Monster")
				{
					LoadMonstersConf(xmlReader);
				}
				else if (xmlReader.Name == "Weapon")
				{
					weaponConfig = new WeaponConfig();
					LoadWeaponConf(xmlReader, weaponConfig);
					weaponConfTable.Add(weaponConfig);
				}
				else if (xmlReader.Name == "Damage")
				{
					LoadUpgradeConf(xmlReader, weaponConfig, "Damage");
				}
				else if (xmlReader.Name == "Frequency")
				{
					LoadUpgradeConf(xmlReader, weaponConfig, "Frequency");
				}
				else if (xmlReader.Name == "Accuracy")
				{
					LoadUpgradeConf(xmlReader, weaponConfig, "Accuracy");
				}
				else if (xmlReader.Name == "Loot")
				{
					LoadLootWeapon(xmlReader, weaponConfig);
				}
				break;
			}
		}
		if (xmlReader != null)
		{
			xmlReader.Close();
		}
		if (stringReader != null)
		{
			stringReader.Close();
		}
		if (stream != null)
		{
			stream.Close();
		}
	}

	private void LoadLootWeapon(XmlReader reader, WeaponConfig weaponConf)
	{
		LootConfig lootConfig = new LootConfig();
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "giveAtWave")
				{
					lootConfig.giveAtWave = int.Parse(reader.Value);
				}
				else if (reader.Name == "fromWave")
				{
					lootConfig.fromWave = int.Parse(reader.Value);
				}
				else if (reader.Name == "toWave")
				{
					lootConfig.toWave = int.Parse(reader.Value);
				}
				else if (reader.Name == "lootRate")
				{
					lootConfig.rate = float.Parse(reader.Value);
				}
				else if (reader.Name == "increaseRate")
				{
					lootConfig.increaseRate = float.Parse(reader.Value);
				}
			}
		}
		weaponConf.lootConf = lootConfig;
	}

	private void LoadAvatarConf(XmlReader reader, AvatarConfig avatarConf)
	{
		if (!reader.HasAttributes)
		{
			return;
		}
		while (reader.MoveToNextAttribute())
		{
			if (reader.Name == "price")
			{
				avatarConf.price = int.Parse(reader.Value);
			}
		}
	}

	private void LoadGlobalConf(XmlReader reader)
	{
		if (!reader.HasAttributes)
		{
			return;
		}
		while (reader.MoveToNextAttribute())
		{
			if (reader.Name == "startMoney")
			{
				globalConf.startMoney = int.Parse(reader.Value);
			}
			else if (reader.Name == "enemyLimit")
			{
				globalConf.enemyLimit = int.Parse(reader.Value);
			}
			else if (reader.Name == "resolution")
			{
				globalConf.resolution = float.Parse(reader.Value);
			}
		}
	}

	private void LoadPlayerConf(XmlReader reader)
	{
		if (!reader.HasAttributes)
		{
			return;
		}
		while (reader.MoveToNextAttribute())
		{
			if (reader.Name == "hp")
			{
				playerConf.hp = float.Parse(reader.Value);
			}
			else if (reader.Name == "walkSpeed")
			{
				playerConf.walkSpeed = float.Parse(reader.Value);
			}
			else if (reader.Name == "armorPrice")
			{
				playerConf.upgradeArmorPrice = int.Parse(reader.Value);
			}
			else if (reader.Name == "upPriceFactor")
			{
				playerConf.upPriceFactor = float.Parse(reader.Value);
			}
			else if (reader.Name == "maxArmorLevel")
			{
				playerConf.maxArmorLevel = int.Parse(reader.Value);
			}
		}
	}

	private void LoadMonstersConf(XmlReader reader)
	{
	}

	private void LoadWeaponConf(XmlReader reader, WeaponConfig weaponConf)
	{
		if (!reader.HasAttributes)
		{
			return;
		}
		while (reader.MoveToNextAttribute())
		{
			if (reader.Name == "name")
			{
				weaponConf.name = reader.Value;
			}
			else if (reader.Name == "type")
			{
				switch (reader.Value)
				{
				case "Rifle":
					weaponConf.wType = WeaponType.AssaultRifle;
					break;
				case "ShotGun":
					weaponConf.wType = WeaponType.ShotGun;
					break;
				case "RPG":
					weaponConf.wType = WeaponType.RocketLauncher;
					break;
				case "MachineGun":
					weaponConf.wType = WeaponType.MachineGun;
					break;
				case "Laser":
					weaponConf.wType = WeaponType.LaserGun;
					break;
				}
			}
			else if (reader.Name == "moveSpeedDrag")
			{
				weaponConf.moveSpeedDrag = float.Parse(reader.Value);
			}
			else if (reader.Name == "range")
			{
				weaponConf.range = float.Parse(reader.Value);
			}
			else if (reader.Name == "price")
			{
				weaponConf.price = int.Parse(reader.Value);
			}
			else if (reader.Name == "bulletPrice")
			{
				weaponConf.bulletPrice = int.Parse(reader.Value);
			}
			else if (reader.Name == "initBullet")
			{
				weaponConf.initBullet = int.Parse(reader.Value);
			}
			else if (reader.Name == "bullet")
			{
				weaponConf.bullet = int.Parse(reader.Value);
			}
			else if (reader.Name == "flySpeed")
			{
				weaponConf.flySpeed = float.Parse(reader.Value);
			}
		}
	}

	private void LoadUpgradeConf(XmlReader reader, WeaponConfig weaponConf, string uType)
	{
		UpgradeConfig upgradeConfig = new UpgradeConfig();
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				if (reader.Name == "base")
				{
					upgradeConfig.baseData = float.Parse(reader.Value);
				}
				else if (reader.Name == "upFactor")
				{
					upgradeConfig.upFactor = float.Parse(reader.Value);
				}
				else if (reader.Name == "basePrice")
				{
					upgradeConfig.basePrice = float.Parse(reader.Value);
				}
				else if (reader.Name == "upPriceFactor")
				{
					upgradeConfig.upPriceFactor = float.Parse(reader.Value);
				}
				else if (reader.Name == "maxLevel")
				{
					upgradeConfig.maxLevel = int.Parse(reader.Value);
				}
			}
		}
		switch (uType)
		{
		case "Damage":
			weaponConf.damageConf = upgradeConfig;
			break;
		case "Frequency":
			weaponConf.attackRateConf = upgradeConfig;
			break;
		case "Accuracy":
			weaponConf.accuracyConf = upgradeConfig;
			break;
		}
	}
}
