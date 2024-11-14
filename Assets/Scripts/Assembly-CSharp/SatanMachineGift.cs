using UnityEngine;

public class SatanMachineGift : SatanMachineProjectile
{
	public float dropRate = 1f;

	public int amount;

	public LootType item;

	protected override void OnExplode()
	{
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_box02") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", base.transform.position);
		OnLoot();
	}

	protected override void OnTouchGround()
	{
		AudioManager.GetInstance().PlaySound("Audio/enemy/SatanMachine/liwuhe");
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_box01") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
	}

	private void SpawnItem(LootType itemType)
	{
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			ItemSpawnRequest request = new ItemSpawnRequest((byte)itemType, base.transform.position, amount);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void OnLoot()
	{
		float value = Random.value;
		float num = dropRate;
		if (value < num)
		{
			SpawnItem(item);
		}
	}
}
