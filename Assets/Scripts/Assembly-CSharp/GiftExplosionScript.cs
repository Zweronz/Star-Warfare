using System.Collections.Generic;
using UnityEngine;

public class GiftExplosionScript : MonoBehaviour
{
	protected float createdTime;

	public float explodeRadius = 5f;

	public int damage;

	public bool isLocal;

	public Transform proTransform;

	protected bool isDestory;

	private void Start()
	{
		createdTime = Time.time;
		proTransform = base.transform;
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", base.transform.position);
	}

	private void Update()
	{
		if (Time.time - createdTime > 2f)
		{
			Object.Destroy(base.gameObject, 1f);
		}
		if (!isDestory)
		{
			Explode();
			isDestory = true;
		}
	}

	private void Explode()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
		{
			return;
		}
		List<Player> players = GameApp.GetInstance().GetGameWorld().GetPlayers();
		foreach (Player item in players)
		{
			if (item == null || !(item.GetTransform() != null))
			{
				continue;
			}
			Vector3 vector = item.GetTransform().position + new Vector3(0f, 1f, 0f);
			float sqrMagnitude = (vector - proTransform.position).sqrMagnitude;
			float num = explodeRadius * explodeRadius;
			if (sqrMagnitude < num)
			{
				Ray ray = new Ray(proTransform.position, vector - proTransform.position);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, explodeRadius, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.PLAYER)) && (hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER || hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER))
				{
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, item.GetUserID(), 27);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
	}
}
