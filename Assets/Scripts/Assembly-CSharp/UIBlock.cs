public class UIBlock : UIControl
{
	public override bool HandleInput(UITouchInner touch)
	{
		if (PtInRect(touch.position))
		{
			return true;
		}
		return false;
	}
}
