public class UpdatePositionState : UIDragListState
{
	public UpdatePositionState(UIDragList list)
		: base(list)
	{
	}

	public override void Update()
	{
		base.Update();
		GetUIDragList().UpdatePosition();
	}
}
