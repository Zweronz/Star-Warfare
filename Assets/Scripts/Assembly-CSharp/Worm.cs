using System;
using UnityEngine;

public class Worm : Enemy
{
	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	protected GameObject defentObj;

	protected FadeInAlphaAnimationScript[] defentScript = new FadeInAlphaAnimationScript[3];

	protected DefentOneByOneScript[] defentOnebyOneScript = new DefentOneByOneScript[3];

	protected Timer showDefentTimer = new Timer();

	protected void RandomRunAnimation()
	{
		runAnimationName = AnimationString.ENEMY_RUN;
	}

	public override void OnDead()
	{
		base.OnDead();
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		lastTarget = Vector3.zero;
		attackRange = 4.5f;
		localScale = Vector3.one * 0.7f;
		RandomRunAnimation();
		if (base.IsElite)
		{
			hp *= 3;
			runSpeed += 2f;
			attackDamage *= 2;
			animation[runAnimationName].speed = 1.5f;
		}
		shoutAudioName = "Audio/enemy/luntaichong_juansuo";
		CreateDefent();
	}

	public override void CheckHit()
	{
		if (!attacked && AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.5f))
		{
			Vector3 vector = enemyTransform.InverseTransformPoint(player.GetTransform().position);
			if (Vector3.Distance(enemyTransform.position, player.GetTransform().position) < 5f && vector.z > 0f && Mathf.Abs(vector.z / vector.x) > Mathf.Tan((float)Math.PI / 3f))
			{
				player.OnHit(attackDamage);
				attacked = true;
			}
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override void OnAttack()
	{
		base.OnAttack();
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		attacked = false;
		lastAttackTime = Time.time;
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.GRAVEBORN_STATE)
		{
			beWokeUp = true;
			int num = dp.damage;
			if (NeedShowDefent())
			{
				num /= 10;
				ShowDefent();
			}
			else
			{
				gotHitTime = Time.time;
				SetState(Enemy.GOTHIT_STATE);
			}
			hp -= num;
			criticalAttacked = dp.criticalAttack;
		}
	}

	public override void OnHitResponse()
	{
		beWokeUp = true;
		gameWorld.GetHitBloodPool().CreateObject(GetPosition(), Vector3.zero, Quaternion.identity);
		if (NeedShowDefent())
		{
			ShowDefent();
			return;
		}
		SetState(Enemy.GOTHIT_STATE);
		gotHitTime = Time.time;
	}

	public override bool NeedShowDefent()
	{
		return state == Enemy.CATCHING_STATE;
	}

	public void CreateDefent()
	{
		GameObject original = Resources.Load("Effect/worm_defent") as GameObject;
		defentObj = UnityEngine.Object.Instantiate(original, enemyTransform.position, enemyTransform.rotation) as GameObject;
		defentObj.transform.parent = enemyTransform;
		for (int i = 0; i < 3; i++)
		{
			defentScript[i] = defentObj.transform.GetChild(i).GetComponent<FadeInAlphaAnimationScript>();
			defentOnebyOneScript[i] = defentObj.transform.GetChild(i).GetComponent<DefentOneByOneScript>();
		}
		showDefentTimer.SetTimer(0.2f, false);
	}

	public void ShowDefent()
	{
		if (showDefentTimer.Ready())
		{
			int num = Random.Range(-80, 80);
			defentObj.transform.rotation = enemyTransform.rotation;
			defentObj.transform.rotation = Quaternion.AngleAxis(num, Vector3.up) * enemyTransform.rotation;
			Vector3 normalized = defentObj.transform.TransformDirection(Vector3.forward).normalized;
			defentObj.transform.position = enemyTransform.position + Vector3.up * 0.5f;
			for (int i = 0; i < 3; i++)
			{
				defentOnebyOneScript[i].appearTime = Time.time + (float)i * 0.05f;
				defentScript[i].FadeIn();
			}
			showDefentTimer.Do();
		}
	}
}
