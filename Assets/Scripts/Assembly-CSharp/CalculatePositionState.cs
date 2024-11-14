public class CalculatePositionState : UIDragListState
{
	private float moveSpeed;

	public CalculatePositionState(UIDragList list, float moveSpeed)
		: base(list)
	{
		this.moveSpeed = moveSpeed;
	}

	public override void Active()
	{
		base.Active();
		GetUIDragList().Reposition();
		GetUIDragList().Format(false);
		GetUIDragList().SwitchState(GetUIDragList().UpdatePositionState);
	}
}
