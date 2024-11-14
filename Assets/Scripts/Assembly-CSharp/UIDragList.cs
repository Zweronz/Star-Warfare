using System.Collections.Generic;
using UnityEngine;

public class UIDragList : MonoBehaviour
{
	public enum Dir
	{
		Horizontal = 0,
		Vertical = 1
	}

	[SerializeField]
	private Dir dir = Dir.Vertical;

	[SerializeField]
	private float width = 100f;

	[SerializeField]
	private float height = 100f;

	[SerializeField]
	private float itemWidth = 10f;

	[SerializeField]
	private float itemHeight = 10f;

	[SerializeField]
	private float moveStep = 1f;

	private List<IUIDragListPlugin> mPluginList = new List<IUIDragListPlugin>();

	private List<UIDraggableListItem> mItemAddList = new List<UIDraggableListItem>();

	private List<UIDraggableListItem> mItemList = new List<UIDraggableListItem>();

	private int mCenterIndex;

	private CalculatePositionState mCalculatePositionState;

	private UpdatePositionState mUpdatePositionState;

	private DragState mDragState;

	private UIDragListState mNextState;

	private UIDragListState mCurState;

	private int mNextIndex;

	private Vector3 mLastPos;

	public float ListWidth
	{
		get
		{
			return width;
		}
	}

	public float ListHeight
	{
		get
		{
			return height;
		}
	}

	public CalculatePositionState CalculatePositionState
	{
		get
		{
			return mCalculatePositionState;
		}
	}

	public UpdatePositionState UpdatePositionState
	{
		get
		{
			return mUpdatePositionState;
		}
	}

	public DragState DragState
	{
		get
		{
			return mDragState;
		}
	}

	private void Awake()
	{
		SetupPanel();
		SetupCollider();
		mNextIndex = -1;
		mCenterIndex = 0;
		mCalculatePositionState = new CalculatePositionState(this, moveStep);
		mUpdatePositionState = new UpdatePositionState(this);
		mDragState = new DragState(this);
	}

	private void SetupPanel()
	{
		UIPanel uIPanel = GetComponent<UIPanel>();
		if (uIPanel == null)
		{
			uIPanel = base.gameObject.AddComponent<UIPanel>();
		}
		uIPanel.clipping = UIDrawCall.Clipping.HardClip;
		uIPanel.clipRange = new Vector4(0f - base.transform.localPosition.x, 0f - base.transform.localPosition.y, width, height);
	}

	private void SetupCollider()
	{
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = base.gameObject.AddComponent<BoxCollider>();
		}
		boxCollider.center = Vector3.zero;
		boxCollider.size = new Vector3(width, height, 0f);
	}

	private void OnPress(bool pressed)
	{
		if (mItemList.Count <= 1)
		{
			return;
		}
		if (pressed)
		{
			mLastPos = mItemList[0].transform.localPosition;
			SwitchState(mDragState);
			{
				foreach (IUIDragListPlugin mPlugin in mPluginList)
				{
					mPlugin.TouchPress();
				}
				return;
			}
		}
		bool flag = false;
		if ((mItemList[0].transform.localPosition - mLastPos).sqrMagnitude > 5f)
		{
			flag = true;
		}
		if (flag)
		{
			SwitchState(mCalculatePositionState);
			{
				foreach (IUIDragListPlugin mPlugin2 in mPluginList)
				{
					mPlugin2.TouchRelease(GetIndex());
				}
				return;
			}
		}
		if (dir != Dir.Vertical)
		{
			return;
		}
		foreach (UIDraggableListItem mItem in mItemList)
		{
			mItem.transform.localPosition += new Vector3(0f, itemHeight, 0f);
		}
		SwitchState(mCalculatePositionState);
		foreach (IUIDragListPlugin mPlugin3 in mPluginList)
		{
			mPlugin3.TouchClick(GetIndex());
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (mItemList.Count <= 1 || dir != Dir.Vertical)
		{
			return;
		}
		foreach (UIDraggableListItem mItem in mItemList)
		{
			mItem.transform.localPosition += new Vector3(0f, delta.y, 0f);
		}
	}

	private void Update()
	{
		if (RefreshList())
		{
			SwitchState(mCalculatePositionState);
		}
		if (mNextState != null)
		{
			mCurState = mNextState;
			mNextState = null;
			mCurState.Active();
		}
		if (mCurState != null)
		{
			mCurState.Update();
		}
		foreach (IUIDragListPlugin mPlugin in mPluginList)
		{
			mPlugin.UpdatePlugin();
		}
		if (mNextIndex <= -1)
		{
			return;
		}
		int num = mNextIndex - GetIndex();
		if (dir == Dir.Vertical)
		{
			foreach (UIDraggableListItem mItem in mItemList)
			{
				mItem.transform.localPosition += new Vector3(0f, (float)num * itemHeight, 0f);
			}
		}
		SwitchState(mCalculatePositionState);
		mNextIndex = -1;
	}

	private bool RefreshList()
	{
		if (mItemAddList.Count > 0)
		{
			foreach (UIDraggableListItem mItemAdd in mItemAddList)
			{
				mItemAdd.transform.parent = base.transform;
				mItemAdd.transform.localPosition = Vector3.zero;
				mItemAdd.transform.localEulerAngles = Vector3.zero;
				mItemAdd.transform.localScale = Vector3.one;
				mItemAdd.SetSize(itemWidth, itemHeight);
				mItemList.Add(mItemAdd);
				mItemAdd.gameObject.SetActive(true);
			}
			mItemAddList.Clear();
			foreach (IUIDragListPlugin mPlugin in mPluginList)
			{
				mPlugin.RefreshItemList(mItemList, GetIndex());
			}
			return true;
		}
		return false;
	}

	public void AddItem(UIDraggableListItem item)
	{
		mItemAddList.Add(item);
	}

	public void SwitchState(UIDragListState state)
	{
		mNextState = state;
	}

	public void UpdatePosition()
	{
		foreach (UIDraggableListItem mItem in mItemList)
		{
			mItem.UpdatePosition();
		}
	}

	public void Reposition()
	{
		if (mItemList.Count == 0 || dir != Dir.Vertical)
		{
			return;
		}
		int index = GetIndex();
		int num = index;
		int num2 = index;
		int num3 = (num2 + 1) % mItemList.Count;
		Vector2 step = new Vector2(0f, moveStep);
		bool flag = true;
		while (num3 != num)
		{
			if (flag)
			{
				SetHigherThan(mItemList[num3], mItemList[num2], true, step);
				if (mItemList[num3].IsHigherThan(height / 2f))
				{
					num = num2;
					num2 = index;
					num3 = (num2 - 1 + mItemList.Count) % mItemList.Count;
					flag = false;
				}
				else
				{
					num2 = num3;
					num3 = (num2 + 1) % mItemList.Count;
				}
			}
			else
			{
				SetLowerThan(mItemList[num3], mItemList[num2], true, step);
				num2 = num3;
				num3 = (num2 - 1 + mItemList.Count) % mItemList.Count;
			}
		}
	}

	public void Format(bool immediately)
	{
		if (mItemList.Count == 0 || dir != Dir.Vertical)
		{
			return;
		}
		int index = GetIndex();
		int num = index;
		int num2 = index;
		int num3 = (num2 + 1) % mItemList.Count;
		Vector2 step = new Vector2(0f, moveStep);
		bool flag = true;
		Vector3 targetVector = Vector3.zero;
		SetZeroCenter(mItemList[index], immediately, step);
		while (num3 != num)
		{
			if (mItemList[num3].transform.localPosition.y > 0f)
			{
				targetVector = SetHigherThan(mItemList[num3], targetVector, immediately, step);
				num2 = num3;
				num3 = (num2 + 1) % mItemList.Count;
				continue;
			}
			if (flag)
			{
				num = num2;
				num2 = index;
				num3 = (num2 - 1 + mItemList.Count) % mItemList.Count;
				flag = false;
				targetVector = Vector3.zero;
			}
			targetVector = SetLowerThan(mItemList[num3], targetVector, immediately, step);
			num2 = num3;
			num3 = (num2 - 1 + mItemList.Count) % mItemList.Count;
		}
	}

	public int GetIndex()
	{
		if (dir == Dir.Vertical)
		{
			float num = 9999f;
			int result = 0;
			for (int i = 0; i < mItemList.Count; i++)
			{
				float y = mItemList[i].transform.localPosition.y;
				float num2 = Mathf.Abs(y);
				if (num2 < num)
				{
					result = i;
					num = num2;
				}
			}
			return result;
		}
		return 0;
	}

	private void SetPosition(UIDraggableListItem item, bool immediately, Vector3 pos, Vector2 step)
	{
		item.SetTargetPosition(pos, step);
		if (immediately)
		{
			item.MoveToTarget();
		}
	}

	public void SetZeroCenter(UIDraggableListItem item, bool immediately, Vector2 step)
	{
		SetPosition(item, immediately, Vector3.zero, step);
	}

	public Vector3 SetHigherThan(UIDraggableListItem item, Vector3 targetVector, bool immediately, Vector2 step)
	{
		Vector3 vector = targetVector + new Vector3(0f, itemHeight, 0f);
		SetPosition(item, immediately, vector, step);
		return vector;
	}

	public Vector3 SetLowerThan(UIDraggableListItem item, Vector3 targetVector, bool immediately, Vector2 step)
	{
		Vector3 vector = targetVector - new Vector3(0f, itemHeight, 0f);
		SetPosition(item, immediately, vector, step);
		return vector;
	}

	public void SetHigherThan(UIDraggableListItem item, UIDraggableListItem targetItem, bool immediately, Vector2 step)
	{
		SetHigherThan(item, targetItem.transform.localPosition, immediately, step);
	}

	public void SetLowerThan(UIDraggableListItem item, UIDraggableListItem targetItem, bool immediately, Vector2 step)
	{
		SetLowerThan(item, targetItem.transform.localPosition, immediately, step);
	}

	public void AddPlugin(IUIDragListPlugin plugin)
	{
		mPluginList.Add(plugin);
		plugin.RefreshItemList(mItemList, GetIndex());
	}

	public void SetIndex(int index)
	{
		mNextIndex = index;
	}
}
