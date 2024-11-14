using UnityEngine;

public class GiftBombScript : MonoBehaviour
{
	public float dropRate = 1f;

	public LootType[] itemTables = new LootType[10];

	public float[] rateTables = new float[10];

	public Player targetPlayer;

	public int damage = 5;

	public float explodeRadius = 1f;

	public GiftBombPhysicsScript giftBombPhysicsScript;

	public float duration = 4f;

	private float startTime;

	private float startJumpSoundTime;

	private bool lastOnGround;

	private void Update()
	{
		if (lastOnGround)
		{
			if (Time.time - startTime > duration)
			{
				Explode();
				OnLoot();
			}
			else if (Time.time - startJumpSoundTime > 0.5f)
			{
				AudioManager.GetInstance().PlaySound("Audio/enemy/SatanMachine/liwuhe");
				startJumpSoundTime = Time.time;
			}
		}
		else if (giftBombPhysicsScript.IsOnGround)
		{
			lastOnGround = true;
			startTime = Time.time;
			startJumpSoundTime = 0f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.PLAYER || other.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			Explode();
		}
	}

	private void Explode()
	{
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_box02") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", base.transform.position);
		Object.Destroy(giftBombPhysicsScript.gameObject);
		Collider[] array = Physics.OverlapSphere(base.transform.position, explodeRadius, (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER));
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				targetPlayer.OnHit(damage);
			}
		}
	}

	private void SpawnItem(LootType itemType)
	{
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			ItemSpawnRequest request = new ItemSpawnRequest((byte)itemType, base.transform.position, 1);
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
