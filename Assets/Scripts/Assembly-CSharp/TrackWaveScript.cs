using System.Collections.Generic;
using UnityEngine;

public class TrackWaveScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	public Vector3 targetPos;

	protected Transform proTransform;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public WeaponType gunType;

	public float life = 2f;

	public int damage;

	public bool isLocal = true;

	public bool isPenerating;

	public bool isReflecting;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	protected float initAngel = 50f;

	protected HashSet<Enemy> enemyhitSet = new HashSet<Enemy>();

	public bool destroyed;

	public void Start()
	{
		Debug.Log("Start");
		proTransform = base.transform;
		createdTime = Time.time;
		ScaleScript component = base.transform.GetComponent<ScaleScript>();
		if (component != null)
		{
			component.scaleSpeed *= VSMath.IMPACT_WAVE_SPEED_BOOTH;
			component.maxScale = 0.65f;
			component.enableMaxScale = true;
		}
	}

	public void Update()
	{
		deltaTime = Time.deltaTime;
		proTransform.Translate(flySpeed * dir * deltaTime, Space.World);
		if (Time.time - createdTime > life)
		{
			Object.DestroyObject(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter");
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		Player player = gameWorld.GetPlayer();
		if (isLocal && other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			Debug.Log("this.isLocal && other.gameObject.layer == PhysicsLayer.PLAYER");
			return;
		}
		string path = "Effect/update_effect/effect_explosion_001";
		GameObject original = Resources.Load("Effect/LaserHit") as GameObject;
		GameObject gameObject = Object.Instantiate(original, proTransform.position, Quaternion.identity) as GameObject;
		if (!isPenerating)
		{
			GameObject original2 = Resources.Load(path) as GameObject;
			Object.Instantiate(original2, proTransform.position, Quaternion.identity);
			Object.DestroyObject(base.gameObject);
		}
		bool criticalAttack = false;
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = damage;
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.isLocal = isLocal;
		damageProperty.wType = gunType;
		Collider[] array = Physics.OverlapSphere(proTransform.position, explodeRadius, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.REMOTE_PLAYER));
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			Ray ray = new Ray(base.transform.position, collider.transform.position - base.transform.position);
			float num = Mathf.Sqrt((base.transform.position - collider.transform.position).sqrMagnitude);
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (enemyByID != null && !enemyhitSet.Contains(enemyByID))
				{
					enemyByID.HitEnemy(damageProperty);
					break;
				}
			}
			if (enemyByCollider.layer != PhysicsLayer.REMOTE_PLAYER || !GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsTeamMode())
			{
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(int.Parse(enemyByCollider.name));
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, int.Parse(enemyByCollider.name), false, 39);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			break;
		}
	}
}
