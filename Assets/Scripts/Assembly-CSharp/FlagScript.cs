using UnityEngine;

public class FlagScript : MonoBehaviour
{
	private bool moveUp;

	public Vector3 rotationSpeed = new Vector3(0f, 45f, 0f);

	public bool enableUpandDown = true;

	protected float deltaTime;

	public float moveSpeed = 0.2f;

	public float HighPos = 1.2f;

	public float LowPos = 1f;

	public short sequenceID;

	protected Timer lastPickUpRequestTimer = new Timer();

	private void Start()
	{
		lastPickUpRequestTimer.SetTimer(3f, true);
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			base.transform.Rotate(rotationSpeed * deltaTime);
			deltaTime = 0f;
			if (GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode() && GameApp.GetInstance().GetGameWorld().GetPlayer()
				.InPlayingState() && base.GetComponent<Collider>().bounds.Intersects(GameApp.GetInstance().GetGameWorld().GetPlayer()
				.GetCollider()
				.bounds) && lastPickUpRequestTimer.Ready())
				{
					CatchTheFlagRequest request = new CatchTheFlagRequest();
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					lastPickUpRequestTimer.Do();
				}
			}
		}
	}
