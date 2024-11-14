using UnityEngine;

internal class VSMath
{
	protected static float HP_POWER = 0.65f;

	protected static float DAMAGE_POWER = 0.3f;

	public static float CHARGE_CONSUME_FOR_LASERCANNON = 20f;

	public static float RPG_FLY_BOOTH = 2f;

	public static float GL_FLY_BOOTH = 1.5f;

	public static float GLOVE_GROW_SPPED_BOOTH = 0.5f;

	public static float IMPACT_WAVE_SPEED_BOOTH = 0.05f;

	public static float GL_EXP_TIME = 1.2f;

	public static float ASSAULT_RIFLE_FREQUENCY_BOOTH = 0.75f;

	public static float LASER_RIFLE_FREQUENCY_BOOTH = 0.65f;

	public static float GL_DAMAGE_BOOTH = 2.4f;

	public static float MACHINE_GUN_DAMAGE_BOOTH = 1.37f;

	public static float RPG_DAMAGE_BOOTH = 1.3f;

	public static float BOW_DAMAGE_BOOTH = 1.2f;

	public static float SWORD_DAMAGE_BOOTH = 2f;

	public static float SNIPER_DAMAGE_BOOTH = 4f;

	public static float TRACKINGGUN_DAMAGE_BOOTH = 1.3f;

	public static float PINGPONGLAUNCHER_DAMAGE_BOOTH = 1.9f;

	public static float FLY_GRENADE_DAMAGE_BOOTH = 0.6f;

	public static float IMPACT_WAVE_DAMAGE_BOOTH = 2.5f;

	public static float GetDamageInVS(float damage)
	{
		return Mathf.Pow(damage, DAMAGE_POWER) * 10f;
	}

	public static int GetHpInVS(int hp)
	{
		return (int)(Mathf.Pow((float)hp * 1f, HP_POWER) / 2f + 300f);
	}

	public static float GetRecoverInVS(float recover)
	{
		return recover / 12f;
	}

	public static float GetAutoRecoverInVS(float autoRecover)
	{
		return autoRecover / 8f;
	}

	public static float GetKillRecoverInVS(float killRecover)
	{
		return killRecover * 4f;
	}

	public static float GetExplodeRadiusInVS(float radius)
	{
		return radius * 0.67f;
	}

	public static float GetFlyGrenadeRadiusInVS(float radius)
	{
		return radius * 0.5f;
	}
}
