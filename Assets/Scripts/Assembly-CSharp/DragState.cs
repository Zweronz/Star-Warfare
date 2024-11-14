public class DragState : UIDragListState
{
	public DragState(UIDragList list)
		: base(list)
	{
	}

	public override void Update()
	{
		base.Update();
		GetUIDragList().Reposition();
	}
}
