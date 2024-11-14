public abstract class Item
{
	public ItemEffect itemEffect = new ItemEffect();

	protected int price;

	protected int mithril;

	protected int time;

	protected byte type;

	protected string name;

	public ItemID ItemID { get; set; }

	public int Time
	{
		get
		{
			return time;
		}
		set
		{
			time = value;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	public int Duration
	{
		get
		{
			return time;
		}
	}

	public int Price
	{
		get
		{
			return price;
		}
	}

	public int Mithril
	{
		get
		{
			return mithril;
		}
	}

	public abstract bool Use(Player player, byte bagIndex);

	public abstract void TakeEffect(Player player, float hpRate);

	public void LoadConfig()
	{
		itemEffect.CreateEffects();
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[16];
		int iRow = (int)(ItemID - 81);
		if (unitDataTable != null)
		{
			name = unitDataTable.GetData(iRow, 0, string.Empty, false);
			type = (byte)unitDataTable.GetData(iRow, 1, 0, false);
			float val = unitDataTable.GetData(iRow, 4, 0, false);
			AddEffect(EffectsType.HP_BOOTH, val);
			val = (float)(sbyte)unitDataTable.GetData(iRow, 2, 0, false) / 100f;
			AddEffect(EffectsType.ATTACK_BOOTH, val);
			val = (sbyte)unitDataTable.GetData(iRow, 3, 0, false);
			AddEffect(EffectsType.SPEED_BOOTH, val);
			val = (sbyte)unitDataTable.GetData(iRow, 5, 0, false);
			AddEffect(EffectsType.REVIVAL_BOOTH, val);
			val = (sbyte)unitDataTable.GetData(iRow, 6, 0, false);
			AddEffect(EffectsType.CALL_BOOTH, val);
			price = unitDataTable.GetData(iRow, 11, 0, false);
			mithril = unitDataTable.GetData(iRow, 12, 0, false);
			val = (sbyte)unitDataTable.GetData(iRow, 7, 0, false);
			AddEffect(EffectsType.DAMAGE_BOOTH, val);
			time = (sbyte)unitDataTable.GetData(iRow, 8, 0, false);
			val = (float)(sbyte)unitDataTable.GetData(iRow, 9, 0, false) / 100f;
			AddEffect(EffectsType.HP_RECOVERY, val);
			val = (float)(sbyte)unitDataTable.GetData(iRow, 10, 0, false) / 100f;
			AddEffect(EffectsType.DAMAGE_REDUCE, val);
		}
	}

	public void CreateEmptyItem()
	{
		itemEffect.CreateEffects();
	}

	public void AddEffect(EffectsType type, float val)
	{
		if (val != 0f)
		{
			Effect effect = new Effect();
			effect.effectType = type;
			effect.data = val;
			itemEffect.AddEffect(effect);
		}
	}
}
