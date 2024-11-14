public abstract class UIDragListState
{
	private UIDragList mUIDragList;

	public UIDragListState(UIDragList list)
	{
		mUIDragList = list;
	}

	protected UIDragList GetUIDragList()
	{
		return mUIDragList;
	}

	public virtual void Active()
	{
	}

	public virtual void Update()
	{
	}
}
