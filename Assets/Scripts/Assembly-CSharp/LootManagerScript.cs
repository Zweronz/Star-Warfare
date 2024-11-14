using UnityEngine;

internal class LootManagerScript : MonoBehaviour
{
	public float dropRate = 1f;

	public LootType[] itemTables = new LootType[10];

	public float[] rateTables = new float[10];

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void SpawnItem(LootType itemType)
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(base.gameObject.name);
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			GameObject original = Resources.Load("Loot/Enegy") as GameObject;
			GameObject original2 = Resources.Load("Loot/Money") as GameObject;
			GameObject gameObject = null;
			enemyByID.GetGround();
			float floorHeight = enemyByID.GetFloorHeight();
			switch (itemType)
			{
			case LootType.Enegy:
				gameObject = Object.Instantiate(original, new Vector3(base.transform.position.x, floorHeight + 1f, base.transform.position.z), Quaternion.identity) as GameObject;
				gameObject.GetComponent<ItemScript>().itemType = itemType;
				gameObject.GetComponent<ItemScript>().LowPos = floorHeight + 1.2f;
				gameObject.GetComponent<ItemScript>().HighPos = floorHeight + 1.5f;
				gameObject.GetComponent<ItemScript>().Amount = enemyByID.GetLoot(itemType);
				break;
			case LootType.Money:
				gameObject = Object.Instantiate(original2, new Vector3(base.transform.position.x, floorHeight + 1f, base.transform.position.z), Quaternion.identity) as GameObject;
				gameObject.GetComponent<ItemScript>().itemType = itemType;
				gameObject.GetComponent<ItemScript>().LowPos = floorHeight + 1.2f;
				gameObject.GetComponent<ItemScript>().HighPos = floorHeight + 1.5f;
				gameObject.GetComponent<ItemScript>().Amount = enemyByID.GetLoot(itemType);
				break;
			}
			GameObject original3 = Resources.Load("Loot/Halo") as GameObject;
			GameObject gameObject2 = Object.Instantiate(original3, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			gameObject2.AddComponent<LookAtCameraScript>();
			gameObject2.transform.localScale = Vector3.one * 0.4f;
			gameObject2.transform.parent = gameObject.transform;
			gameObject.tag = TagName.ITEM;
		}
		else
		{
			ItemSpawnRequest request = new ItemSpawnRequest((byte)itemType, base.transform.position, enemyByID.GetLoot(itemType));
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void OnLoot()
	{
		float value = Random.value;
		float num = dropRate;
		if (!(value < num))
		{
			return;
		}
		value = Random.value;
		float num2 = 0f;
		for (int i = 0; i < itemTables.Length; i++)
		{
			if (rateTables[i] > 0f && value <= num2 + rateTables[i])
			{
				SpawnItem(itemTables[i]);
				break;
			}
			num2 += rateTables[i];
		}
	}
}
