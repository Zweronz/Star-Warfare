using System.Collections.Generic;

public interface IUIDragListPlugin
{
	void RefreshItemList(List<UIDraggableListItem> list, int index);

	void UpdatePlugin();

	void TouchPress();

	void TouchRelease(int index);

	void TouchClick(int index);
}
