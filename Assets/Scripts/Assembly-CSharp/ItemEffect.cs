using System.Collections.Generic;

public class ItemEffect
{
	protected List<Effect> effects = new List<Effect>();

	public void CreateEffects()
	{
		effects.Clear();
		for (int i = 0; i < 10; i++)
		{
			Effect effect = new Effect();
			effect.effectType = (EffectsType)i;
			effect.data = 0f;
			effects.Add(effect);
		}
	}

	public void AddEffect(Effect effect)
	{
		effects[(int)effect.effectType].data += effect.data;
	}

	public float GetEffect(EffectsType sType)
	{
		return effects[(int)sType].data;
	}

	public List<Effect> GetEffect()
	{
		return effects;
	}

	public override string ToString()
	{
		string text = string.Empty;
		for (int i = 0; i < 10; i++)
		{
			string text2 = text;
			text = string.Concat(text2, effects[i].effectType, ":", effects[i].data, "   ");
		}
		return text;
	}

	public bool HasSign(EffectsType type)
	{
		return type == EffectsType.HP_BOOTH || type == EffectsType.SPEED_BOOTH || type == EffectsType.REVIVAL_BOOTH || EffectsType.CALL_BOOTH == type;
	}

	public bool IsPercetage(EffectsType type)
	{
		return type == EffectsType.ATTACK_BOOTH || type == EffectsType.DAMAGE_BOOTH || type == EffectsType.DAMAGE_REDUCE || EffectsType.HP_RECOVERY == type;
	}
}
