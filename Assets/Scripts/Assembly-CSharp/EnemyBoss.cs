using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBoss : Enemy
{
	public enum MindState
	{
		NORMAL = 0,
		RAGE = 1
	}

	public static EnemyState INIT_STATE;

	protected bool enableGravity = true;

	protected GameObject leftArmTrail;

	protected GameObject rightArmTrail;

	protected Collider bodyCollider;

	protected Collider flyBodyCollider;

	protected GameObject shadow;

	protected GameObject[] trails;

	protected GameObject bipObject;

	protected Vector3 targetToLookAt;

	protected Vector3 deltaPosition;

	protected int maxDeltaCount = 10;

	protected int currentDeltaCount;

	protected float maxTurnRadian;

	protected bool canShot = true;

	protected bool hasShadow = true;

	protected Vector3 areaCenter;

	protected float areaRadius;

	protected float maxOverRushDistance;

	protected Timer walkAudioTimer = new Timer();

	protected Timer touchtimer = new Timer();

	protected Timer firetimer = new Timer();

	protected Timer criticalTimer = new Timer();

	protected string walkAudioName = string.Empty;

	protected MindState mindState;

	protected float[] maxCatchingTime = new float[2];

	protected float lastCatchingTime;

	protected float hpPercentagePerHit;

	protected int hitTimesForRage;

	protected int currentHitTime = 1;

	protected float attackDetectionAngle;

	protected float turnSpeed;

	protected float upSpeed;

	protected float downSpeed;

	protected float groupAttackDistance;

	protected float touchKnockSpeed;

	protected int attackCount;

	public bool CanShot
	{
		get
		{
			return canShot;
		}
		set
		{
			canShot = value;
		}
	}

	public float MaxCatchingTime
	{
		get
		{
			return maxCatchingTime[(int)mindState];
		}
	}

	public float GroupAttackDistance
	{
		get
		{
			return groupAttackDistance;
		}
	}

	public void SetCatchingTimeNow()
	{
		lastCatchingTime = Time.time;
	}

	public float GetCatchingration()
	{
		return Time.time - lastCatchingTime;
	}

	public void IncreaseAttackCount()
	{
		attackCount++;
	}

	public int GetAttackCount()
	{
		return attackCount;
	}

	public void ResetAttackCount()
	{
		attackCount = 0;
	}

	protected virtual bool GetHitPoint(out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		Camera mainCamera = Camera.mainCamera;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Ray ray = new Ray(transform.position, vector - transform.position);
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
		{
			hitPoint = hitInfo.point;
			return true;
		}
		ray = new Ray(player.GetTransform().position + Vector3.up * 0.5f, bipObject.transform.position + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f) - player.GetTransform().position - Vector3.up * 0.5f);
		hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
		{
			hitPoint = hitInfo.point;
			return true;
		}
		return false;
	}

	protected bool GetHitResponsePoint(out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		if (!player.InPlayingState())
		{
			return false;
		}
		bool flag = true;
		if (gameWorld.GetPlayingPlayerCount() > 1)
		{
			if (player.State != Player.ATTACK_STATE)
			{
				flag = false;
			}
			Weapon weapon = player.GetWeapon();
			if (weapon != null && weapon.GetWeaponType() == WeaponType.LaserGun)
			{
				LaserCannon laserCannon = weapon as LaserCannon;
				if (laserCannon != null && laserCannon.IsOverHeat)
				{
					flag = false;
				}
			}
		}
		if (flag)
		{
			return GetHitPoint(out hitPoint);
		}
		RemotePlayer remotePlayer = null;
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState() && item.State == Player.ATTACK_STATE)
			{
				remotePlayer = item;
				break;
			}
		}
		if (remotePlayer != null)
		{
			Ray ray = new Ray(remotePlayer.GetTransform().position + Vector3.up * 0.5f, bipObject.transform.position + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f) - remotePlayer.GetTransform().position - Vector3.up * 0.5f);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
			{
				hitPoint = hitInfo.point;
				return true;
			}
		}
		return false;
	}

	public virtual void InitBossLevelTime()
	{
		lastCatchingTime = Time.time;
	}

	public bool NeedRage()
	{
		if (mindState == MindState.RAGE)
		{
			return false;
		}
		if ((float)(maxHp - hp) > (float)maxHp * hpPercentagePerHit * (float)hitTimesForRage)
		{
			mindState = MindState.RAGE;
			return true;
		}
		return false;
	}

	public float GetPlayerHorizontalAbsAngle()
	{
		Vector3 to = new Vector3(enemyTransform.forward.x, 0f, enemyTransform.forward.z);
		Vector3 from = player.GetTransform().position - enemyTransform.position;
		from.y = 0f;
		float f = Vector3.Angle(from, to);
		return Mathf.Abs(f);
	}

	public float GetPlayerHorizontalAngle()
	{
		Vector3 vector = new Vector3(enemyTransform.forward.x, 0f, enemyTransform.forward.z);
		Vector3 vector2 = player.GetTransform().position - enemyTransform.position;
		vector2.y = 0f;
		float num = Vector3.Angle(vector2, vector);
		if (Vector3.Cross(vector, vector2).y < 0f)
		{
			num = 0f - num;
		}
		return num;
	}

	public float GetTargetHorizontalAngle()
	{
		Vector3 vector = new Vector3(enemyTransform.forward.x, 0f, enemyTransform.forward.z);
		Vector3 vector2 = target.position - enemyTransform.position;
		vector2.y = 0f;
		float num = Vector3.Angle(vector2, vector);
		if (Vector3.Cross(vector, vector2).y < 0f)
		{
			num = 0f - num;
		}
		return num;
	}

	public void EnableLeftArmTrail(bool bEnable)
	{
		if (bEnable)
		{
			leftArmTrail.GetComponent<TrailRenderer>().enabled = true;
		}
		else
		{
			leftArmTrail.GetComponent<TrailRenderer>().enabled = false;
		}
	}

	public void EnableRightArmTrail(bool bEnable)
	{
		if (bEnable)
		{
			rightArmTrail.GetComponent<TrailRenderer>().enabled = true;
		}
		else
		{
			rightArmTrail.GetComponent<TrailRenderer>().enabled = false;
		}
	}

	protected virtual void loadParameters()
	{
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		loadParameters();
		state = INIT_STATE;
		targetPlayer = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			switch (gameWorld.GetPlayingPlayerCount())
			{
			case 2:
				hp = (int)((float)hp * 2f);
				break;
			case 3:
				hp = (int)((float)hp * 3f);
				break;
			}
		}
		maxHp = hp;
		deltaPosition = Vector3.zero;
		if (hasShadow)
		{
			GameObject original = Resources.Load("Effect/Spider/shadow") as GameObject;
			Vector3 position = new Vector3(enemyTransform.position.x, 0.05f, enemyTransform.position.z);
			shadow = UnityEngine.Object.Instantiate(original, position, Quaternion.AngleAxis(270f, Vector3.right)) as GameObject;
		}
		InitBossLevelTime();
		EnableGravity(true);
	}

	public virtual bool NearGround()
	{
		return false;
	}

	public virtual bool isFlying()
	{
		return false;
	}

	public void SetDir(Vector3 dir)
	{
		base.dir = dir;
	}

	public Vector3 GetDir()
	{
		return dir;
	}

	public void PlaySound(string name)
	{
		AudioManager.GetInstance().PlaySoundAt(name, enemyTransform.position);
	}

	public void PlaySoundSingle(string name)
	{
		AudioManager.GetInstance().PlaySoundSingleAt(name, enemyTransform.position);
	}

	public void StopSound(string name)
	{
		AudioManager.GetInstance().StopSound(name);
	}

	public virtual void TouchPlayer()
	{
		if (state == Enemy.DEAD_STATE || !touchtimer.Ready())
		{
			return;
		}
		if (isFlying())
		{
			if (flyBodyCollider.bounds.Intersects(player.GetCollider().bounds))
			{
				player.OnHit(touchDamage);
				CheckKnocked(touchKnockSpeed);
				touchtimer.Do();
			}
		}
		else if (bodyCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(touchDamage);
			CheckKnocked(touchKnockSpeed);
			touchtimer.Do();
		}
	}

	public virtual bool CanTurn()
	{
		return true;
	}

	public virtual bool NeedMoveDown()
	{
		return true;
	}

	public override void FindNewTarget()
	{
		int nearestPlayer = GetNearestPlayer();
		ChangeTargetPlayer(nearestPlayer);
	}

	public override void DoShoutAudio()
	{
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (hasShadow)
		{
			float num = 1f + enemyTransform.position.y / 40f;
			shadow.transform.position = new Vector3(enemyTransform.position.x, 0.05f, enemyTransform.position.z);
			shadow.transform.localScale = new Vector3(num, num, 1f);
		}
		if (trails != null)
		{
			GameObject[] array = trails;
			foreach (GameObject gameObject in array)
			{
				gameObject.transform.LookAt(gameObject.transform.position - 10f * enemyTransform.forward);
			}
		}
		TouchPlayer();
		if (enableGravity && NeedMoveDown())
		{
			CharacterController characterController = enemyObject.collider as CharacterController;
			if (characterController != null)
			{
				characterController.Move(Vector3.down * downSpeed * deltaTime);
			}
			if (enemyTransform.position.y < (float)Global.FLOORHEIGHT + 0.1f)
			{
				enemyTransform.position = new Vector3(enemyTransform.position.x, (float)Global.FLOORHEIGHT + 0.1f, enemyTransform.position.z);
			}
		}
		if (!Lobby.GetInstance().IsMasterPlayer && currentDeltaCount < maxDeltaCount)
		{
			enemyTransform.position += deltaPosition / maxDeltaCount;
			currentDeltaCount++;
		}
		if (CanTurn())
		{
			Vector3 vector = targetToLookAt - enemyTransform.position;
			vector.y = 0f;
			Vector3 vector2 = Vector3.RotateTowards(enemyTransform.forward, vector, maxTurnRadian, 100f);
			enemyTransform.LookAt(enemyTransform.position + vector2);
		}
	}

	public override void FindPath()
	{
	}

	public override void DoMove(float deltaTime)
	{
		LookAtTarget();
		CharacterController characterController = enemyObject.collider as CharacterController;
		if (characterController != null)
		{
			Vector3 vector = enemyTransform.forward * runSpeed;
			characterController.Move(vector * deltaTime);
		}
		if (walkAudioTimer.Ready())
		{
			PlaySound(walkAudioName);
			walkAudioTimer.Do();
		}
	}

	public override void LookAtPoint(Vector3 targetPoint)
	{
		targetToLookAt = targetPoint;
		Vector3 to = new Vector3(enemyTransform.forward.x, 0f, enemyTransform.forward.z);
		Vector3 from = targetToLookAt - enemyTransform.position;
		from.y = 0f;
		float f = Vector3.Angle(from, to);
		float num = Mathf.Abs(f) * ((float)Math.PI / 180f);
		maxTurnRadian = num * turnSpeed;
	}

	public override bool CouldEnterAttackState()
	{
		return false;
	}

	public override void UpdatePosition(Vector3 position)
	{
		if (!Lobby.GetInstance().IsMasterPlayer)
		{
			deltaPosition = position - GetTransform().position;
			currentDeltaCount = 0;
		}
	}

	public virtual void EnableTrailEffect(bool bEnable)
	{
		if (bEnable)
		{
			GameObject[] array = trails;
			foreach (GameObject gameObject in array)
			{
				gameObject.transform.GetChild(0).GetComponent<ParticleEmitter>().emit = true;
			}
		}
		else
		{
			GameObject[] array2 = trails;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.transform.GetChild(0).GetComponent<ParticleEmitter>().emit = false;
			}
		}
	}

	protected virtual void StopSoundOnHit()
	{
	}

	public virtual void EnableGravity(bool bEnable)
	{
		enableGravity = bEnable;
	}

	public override Vector3 GetColliderCenterPosition()
	{
		return bodyCollider.transform.position;
	}
}
