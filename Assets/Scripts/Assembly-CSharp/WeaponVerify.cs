public class WeaponVerify
{
	public float range;

	public float damageInit;

	public float splashDamageInit;

	public float splashDuration;

	public float bombRangeInit;

	public float attackFrenquencyInit;

	public float speedDrag;

	public WeaponVerify(int gunID)
	{
		LoadConfig(gunID);
	}

	public virtual void LoadConfig(int gunID)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[13];
		if (unitDataTable != null)
		{
			damageInit = unitDataTable.GetData(gunID, 1, 0, false);
			attackFrenquencyInit = (float)(short)unitDataTable.GetData(gunID, 2, 0, false) / 100f;
			range = unitDataTable.GetData(gunID, 3, 0, false);
			bombRangeInit = (float)(sbyte)unitDataTable.GetData(gunID, 4, 0, false) / 10f;
			splashDamageInit = unitDataTable.GetData(gunID, 5, 0, false);
			splashDuration = (float)(sbyte)unitDataTable.GetData(gunID, 6, 0, false) / 10f;
			speedDrag = (float)(sbyte)unitDataTable.GetData(gunID, 7, 0, false) / 10f;
		}
	}
}
