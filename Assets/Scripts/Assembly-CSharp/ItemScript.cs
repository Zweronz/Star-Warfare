using UnityEngine;

public class ItemScript : MonoBehaviour
{
	public LootType itemType;

	private bool moveUp;

	public Vector3 rotationSpeed = new Vector3(0f, 45f, 0f);

	public bool enableUpandDown = true;

	protected float deltaTime;

	public float moveSpeed = 0.2f;

	public float HighPos = 1.2f;

	public float LowPos = 1f;

	public int Amount;

	public short sequenceID;

	protected Timer lastPickUpRequestTimer = new Timer();

	private void Start()
	{
		lastPickUpRequestTimer.SetTimer(3f, true);
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime < 0.03f)
		{
			return;
		}
		base.transform.Rotate(rotationSpeed * deltaTime);
		if (enableUpandDown)
		{
			if (!moveUp)
			{
				float num = Mathf.MoveTowards(base.transform.position.y, LowPos, moveSpeed * deltaTime);
				base.transform.position = new Vector3(base.transform.position.x, num, base.transform.position.z);
				if (num <= LowPos)
				{
					moveUp = true;
				}
			}
			else
			{
				float num2 = Mathf.MoveTowards(base.transform.position.y, HighPos, moveSpeed * deltaTime);
				base.transform.position = new Vector3(base.transform.position.x, num2, base.transform.position.z);
				if (num2 >= HighPos)
				{
					moveUp = false;
				}
			}
		}
		deltaTime = 0f;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (lastPickUpRequestTimer.Ready() && c.collider.gameObject.layer == PhysicsLayer.PLAYER)
		{
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
				player.OnPickUp(itemType, Amount);
				Object.Destroy(base.gameObject);
			}
			else
			{
				PickUpItemRequest request = new PickUpItemRequest(sequenceID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			lastPickUpRequestTimer.Do();
		}
	}
}
