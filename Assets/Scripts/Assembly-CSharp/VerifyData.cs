public class VerifyData
{
	private short mId;

	private byte mType;

	private float mRealValue;

	private float mVerifyValue;

	public short ID
	{
		get
		{
			return mId;
		}
	}

	public byte Type
	{
		get
		{
			return mType;
		}
	}

	public float RealValue
	{
		get
		{
			return mRealValue;
		}
	}

	public float VerifyValue
	{
		get
		{
			return mVerifyValue;
		}
	}

	public VerifyData(BodyType bodyType, byte armorId, SkillsType skillsType, float realValue, float verifyValue)
	{
		if (bodyType == BodyType.Bag)
		{
			mId = (short)(100 + armorId);
		}
		else
		{
			mId = (short)(4 * armorId + bodyType);
		}
		mType = (byte)skillsType;
		mRealValue = realValue;
		mVerifyValue = verifyValue;
	}

	public VerifyData(int armorRewardId, SkillsType skillsType, float realValue, float verifyValue)
	{
		mId = (short)(200 + armorRewardId);
		mType = (byte)skillsType;
		mRealValue = realValue;
		mVerifyValue = verifyValue;
	}

	public VerifyData(int gunId, int gunDataId, float realValue, float verifyValue)
	{
		mId = (short)(300 + gunId);
		mType = (byte)gunDataId;
		mRealValue = realValue;
		mVerifyValue = verifyValue;
	}

	public VerifyData(int upgradeLevel, int realValue, int verifyValue)
	{
		mId = (short)(400 + upgradeLevel);
		mType = 0;
		mRealValue = realValue;
		mVerifyValue = verifyValue;
	}
}
