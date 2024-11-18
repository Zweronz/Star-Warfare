using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Panel")]
[ExecuteInEditMode]
public class UIPanel : MonoBehaviour
{
	public enum DebugInfo
	{
		None = 0,
		Gizmos = 1,
		Geometry = 2
	}

	public bool showInPanelTool = true;

	public bool generateNormals;

	public bool depthPass;

	public bool widgetsAreStatic;

	[HideInInspector]
	[SerializeField]
	private float mAlpha = 1f;

	[SerializeField]
	[HideInInspector]
	private DebugInfo mDebugInfo = DebugInfo.Gizmos;

	[SerializeField]
	[HideInInspector]
	private UIDrawCall.Clipping mClipping;

	[SerializeField]
	[HideInInspector]
	private Vector4 mClipRange = Vector4.zero;

	[HideInInspector]
	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(40f, 40f);

	private OrderedDictionary mChildren = new OrderedDictionary();

	private BetterList<UIWidget> mWidgets = new BetterList<UIWidget>();

	private BetterList<Material> mChanged = new BetterList<Material>();

	private BetterList<UIDrawCall> mDrawCalls = new BetterList<UIDrawCall>();

	private BetterList<Vector3> mVerts = new BetterList<Vector3>();

	private BetterList<Vector3> mNorms = new BetterList<Vector3>();

	private BetterList<Vector4> mTans = new BetterList<Vector4>();

	private BetterList<Vector2> mUvs = new BetterList<Vector2>();

	private BetterList<Color32> mCols = new BetterList<Color32>();

	private Transform mTrans;

	private Camera mCam;

	private int mLayer = -1;

	private bool mDepthChanged;

	private bool mRebuildAll;

	private bool mChangedLastFrame;

	private bool mWidgetsAdded;

	private float mUpdateTime;

	private float mMatrixTime;

	private Matrix4x4 mWorldToLocal = Matrix4x4.identity;

	private static float[] mTemp = new float[4];

	private Vector2 mMin = Vector2.zero;

	private Vector2 mMax = Vector2.zero;

	private List<Transform> mRemoved = new List<Transform>();

	private UIPanel[] mChildPanels;

	private bool mCheckVisibility;

	private float mCullTime;

	private bool mCulled;

	private static BetterList<UINode> mHierarchy = new BetterList<UINode>();

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public bool changedLastFrame
	{
		get
		{
			return mChangedLastFrame;
		}
	}

	public float alpha
	{
		get
		{
			return mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mAlpha != num)
			{
				mAlpha = num;
				mCheckVisibility = true;
				for (int i = 0; i < mDrawCalls.size; i++)
				{
					UIDrawCall uIDrawCall = mDrawCalls[i];
					MarkMaterialAsChanged(uIDrawCall.material, false);
				}
				for (int j = 0; j < mWidgets.size; j++)
				{
					mWidgets[j].MarkAsChangedLite();
				}
			}
		}
	}

	public DebugInfo debugInfo
	{
		get
		{
			return mDebugInfo;
		}
		set
		{
			if (mDebugInfo != value)
			{
				mDebugInfo = value;
				BetterList<UIDrawCall> betterList = drawCalls;
				HideFlags hideFlags = ((mDebugInfo != DebugInfo.Geometry) ? HideFlags.HideAndDontSave : (HideFlags.DontSave | HideFlags.NotEditable));
				int i = 0;
				for (int size = betterList.size; i < size; i++)
				{
					UIDrawCall uIDrawCall = betterList[i];
					GameObject gameObject = uIDrawCall.gameObject;
					NGUITools.SetActiveSelf(gameObject, false);
					gameObject.hideFlags = hideFlags;
					NGUITools.SetActiveSelf(gameObject, true);
				}
			}
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return mClipping;
		}
		set
		{
			if (mClipping != value)
			{
				mCheckVisibility = true;
				mClipping = value;
				mMatrixTime = 0f;
				UpdateDrawcalls();
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return mClipRange;
		}
		set
		{
			if (mClipRange != value)
			{
				mCullTime = ((mCullTime != 0f) ? (Time.realtimeSinceStartup + 0.15f) : 0.001f);
				mCheckVisibility = true;
				mClipRange = value;
				mMatrixTime = 0f;
				UpdateDrawcalls();
			}
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return mClipSoftness;
		}
		set
		{
			if (mClipSoftness != value)
			{
				mClipSoftness = value;
				UpdateDrawcalls();
			}
		}
	}

	public BetterList<UIWidget> widgets
	{
		get
		{
			return mWidgets;
		}
	}

	public BetterList<UIDrawCall> drawCalls
	{
		get
		{
			int num = mDrawCalls.size;
			while (num > 0)
			{
				UIDrawCall uIDrawCall = mDrawCalls[--num];
				if (uIDrawCall == null)
				{
					mDrawCalls.RemoveAt(num);
				}
			}
			return mDrawCalls;
		}
	}

	public void SetAlphaRecursive(float val, bool rebuildList)
	{
		if (rebuildList || mChildPanels == null)
		{
			mChildPanels = GetComponentsInChildren<UIPanel>(true);
		}
		int i = 0;
		for (int num = mChildPanels.Length; i < num; i++)
		{
			mChildPanels[i].alpha = val;
		}
	}

	private UINode GetNode(Transform t)
	{
		UINode result = null;
		if (t != null && mChildren.Contains(t))
		{
			result = (UINode)mChildren[t];
		}
		return result;
	}

	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		UpdateTransformMatrix();
		a = mWorldToLocal.MultiplyPoint3x4(a);
		b = mWorldToLocal.MultiplyPoint3x4(b);
		c = mWorldToLocal.MultiplyPoint3x4(c);
		d = mWorldToLocal.MultiplyPoint3x4(d);
		mTemp[0] = a.x;
		mTemp[1] = b.x;
		mTemp[2] = c.x;
		mTemp[3] = d.x;
		float num = Mathf.Min(mTemp);
		float num2 = Mathf.Max(mTemp);
		mTemp[0] = a.y;
		mTemp[1] = b.y;
		mTemp[2] = c.y;
		mTemp[3] = d.y;
		float num3 = Mathf.Min(mTemp);
		float num4 = Mathf.Max(mTemp);
		if (num2 < mMin.x)
		{
			return false;
		}
		if (num4 < mMin.y)
		{
			return false;
		}
		if (num > mMax.x)
		{
			return false;
		}
		if (num3 > mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(Vector3 worldPos)
	{
		if (mAlpha < 0.001f)
		{
			return false;
		}
		if (mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		UpdateTransformMatrix();
		Vector3 vector = mWorldToLocal.MultiplyPoint3x4(worldPos);
		if (vector.x < mMin.x)
		{
			return false;
		}
		if (vector.y < mMin.y)
		{
			return false;
		}
		if (vector.x > mMax.x)
		{
			return false;
		}
		if (vector.y > mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(UIWidget w)
	{
		if (mAlpha < 0.001f)
		{
			return false;
		}
		if (!w.enabled || !NGUITools.GetActive(w.gameObject) || w.alpha < 0.001f)
		{
			return false;
		}
		if (mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		Vector2 relativeSize = w.relativeSize;
		Vector2 vector = Vector2.Scale(w.pivotOffset, relativeSize);
		Vector2 vector2 = vector;
		vector.x += relativeSize.x;
		vector.y -= relativeSize.y;
		Transform transform = w.cachedTransform;
		Vector3 a = transform.TransformPoint(vector);
		Vector3 b = transform.TransformPoint(new Vector2(vector.x, vector2.y));
		Vector3 c = transform.TransformPoint(new Vector2(vector2.x, vector.y));
		Vector3 d = transform.TransformPoint(vector2);
		return IsVisible(a, b, c, d);
	}

	public void MarkMaterialAsChanged(Material mat, bool sort)
	{
		if (mat != null)
		{
			if (sort)
			{
				mDepthChanged = true;
			}
			if (!mChanged.Contains(mat))
			{
				mChanged.Add(mat);
				mChangedLastFrame = true;
			}
		}
	}

	public bool WatchesTransform(Transform t)
	{
		return t == cachedTransform || mChildren.Contains(t);
	}

	private UINode AddTransform(Transform t)
	{
		UINode uINode = null;
		UINode uINode2 = null;
		while (t != null && t != cachedTransform)
		{
			if (mChildren.Contains(t))
			{
				if (uINode2 == null)
				{
					uINode2 = (UINode)mChildren[t];
				}
			}
			else
			{
				uINode = new UINode(t);
				if (uINode2 == null)
				{
					uINode2 = uINode;
				}
				mChildren.Add(t, uINode);
			}
			t = t.parent;
		}
		return uINode2;
	}

	private void RemoveTransform(Transform t)
	{
		if (!(t != null))
		{
			return;
		}
		while (mChildren.Contains(t))
		{
			mChildren.Remove(t);
			t = t.parent;
			if (t == null || t == mTrans || t.childCount > 1)
			{
				break;
			}
		}
	}

	public void AddWidget(UIWidget w)
	{
		if (!(w != null))
		{
			return;
		}
		UINode uINode = AddTransform(w.cachedTransform);
		if (uINode != null)
		{
			uINode.widget = w;
			w.visibleFlag = 1;
			if (!mWidgets.Contains(w))
			{
				mWidgets.Add(w);
				if (!mChanged.Contains(w.material))
				{
					mChanged.Add(w.material);
					mChangedLastFrame = true;
				}
				mDepthChanged = true;
				mWidgetsAdded = true;
			}
		}
		else
		{
			Debug.LogError("Unable to find an appropriate root for " + NGUITools.GetHierarchy(w.gameObject) + "\nPlease make sure that there is at least one game object above this widget!", w.gameObject);
		}
	}

	public void RemoveWidget(UIWidget w)
	{
		if (!(w != null))
		{
			return;
		}
		UINode node = GetNode(w.cachedTransform);
		if (node != null)
		{
			if (node.visibleFlag == 1 && !mChanged.Contains(w.material))
			{
				mChanged.Add(w.material);
				mChangedLastFrame = true;
			}
			RemoveTransform(w.cachedTransform);
		}
		mWidgets.Remove(w);
	}

	private UIDrawCall GetDrawCall(Material mat, bool createIfMissing)
	{
		int i = 0;
		for (int size = drawCalls.size; i < size; i++)
		{
			UIDrawCall uIDrawCall = drawCalls.buffer[i];
			if (uIDrawCall.material == mat)
			{
				return uIDrawCall;
			}
		}
		UIDrawCall uIDrawCall2 = null;
		if (createIfMissing)
		{
			GameObject gameObject = new GameObject("_UIDrawCall [" + mat.name + "]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = base.gameObject.layer;
			uIDrawCall2 = gameObject.AddComponent<UIDrawCall>();
			uIDrawCall2.material = mat;
			mDrawCalls.Add(uIDrawCall2);
		}
		return uIDrawCall2;
	}

	private void Start()
	{
		mLayer = base.gameObject.layer;
		UICamera uICamera = UICamera.FindCameraForLayer(mLayer);
		mCam = ((!(uICamera != null)) ? NGUITools.FindCameraForLayer(mLayer) : uICamera.cachedCamera);
	}

	private void OnEnable()
	{
		int i = 0;
		for (int size = mWidgets.size; i < size; i++)
		{
			AddWidget(mWidgets.buffer[i]);
		}
		mRebuildAll = true;
	}

	private void OnDisable()
	{
		int num = mDrawCalls.size;
		while (num > 0)
		{
			UIDrawCall uIDrawCall = mDrawCalls.buffer[--num];
			if (uIDrawCall != null)
			{
				NGUITools.DestroyImmediate(uIDrawCall.gameObject);
			}
		}
		mDrawCalls.Clear();
		mChanged.Clear();
		mChildren.Clear();
	}

	private int GetChangeFlag(UINode start)
	{
		int num = start.changeFlag;
		if (num == -1)
		{
			Transform parent = start.trans.parent;
			while (true)
			{
				if (parent != null && mChildren.Contains(parent))
				{
					UINode uINode = (UINode)mChildren[parent];
					num = uINode.changeFlag;
					parent = parent.parent;
					if (num == -1)
					{
						mHierarchy.Add(uINode);
						continue;
					}
					break;
				}
				num = 0;
				break;
			}
			int i = 0;
			for (int size = mHierarchy.size; i < size; i++)
			{
				UINode uINode2 = mHierarchy.buffer[i];
				uINode2.changeFlag = num;
			}
			mHierarchy.Clear();
		}
		return num;
	}

	private void UpdateTransformMatrix()
	{
		if (mUpdateTime != 0f && mMatrixTime == mUpdateTime)
		{
			return;
		}
		mMatrixTime = mUpdateTime;
		mWorldToLocal = cachedTransform.worldToLocalMatrix;
		if (mClipping != 0)
		{
			Vector2 vector = new Vector2(mClipRange.z, mClipRange.w);
			if (vector.x == 0f)
			{
				vector.x = ((!(mCam == null)) ? mCam.pixelWidth : ((float)Screen.width));
			}
			if (vector.y == 0f)
			{
				vector.y = ((!(mCam == null)) ? mCam.pixelHeight : ((float)Screen.height));
			}
			vector *= 0.5f;
			mMin.x = mClipRange.x - vector.x;
			mMin.y = mClipRange.y - vector.y;
			mMax.x = mClipRange.x + vector.x;
			mMax.y = mClipRange.y + vector.y;
		}
	}

	private void UpdateTransforms()
	{
		mChangedLastFrame = false;
		bool flag = false;
		bool flag2 = false;
		flag2 = clipping != 0 && mUpdateTime > mCullTime;
		if (!widgetsAreStatic || mWidgetsAdded || flag2 != mCulled)
		{
			int i = 0;
			for (int count = mChildren.Count; i < count; i++)
			{
				UINode uINode = (UINode)mChildren[i];
				if (uINode.trans == null)
				{
					mRemoved.Add(uINode.trans);
				}
				else if (uINode.HasChanged())
				{
					uINode.changeFlag = 1;
					flag = true;
				}
				else
				{
					uINode.changeFlag = -1;
				}
			}
			int j = 0;
			for (int count2 = mRemoved.Count; j < count2; j++)
			{
				mChildren.Remove(mRemoved[j]);
			}
			mRemoved.Clear();
		}
		if (!mCulled && flag2)
		{
			mCheckVisibility = true;
		}
		if (mCheckVisibility || flag || mRebuildAll)
		{
			int k = 0;
			for (int count3 = mChildren.Count; k < count3; k++)
			{
				UINode uINode2 = (UINode)mChildren[k];
				if (!(uINode2.widget != null))
				{
					continue;
				}
				int num = 1;
				if (flag2 || flag)
				{
					if (uINode2.changeFlag == -1)
					{
						uINode2.changeFlag = GetChangeFlag(uINode2);
					}
					if (flag2)
					{
						num = ((!mCheckVisibility && uINode2.changeFlag != 1) ? uINode2.visibleFlag : (IsVisible(uINode2.widget) ? 1 : 0));
					}
				}
				if (uINode2.visibleFlag != num)
				{
					uINode2.changeFlag = 1;
				}
				if (uINode2.changeFlag == 1 && (num == 1 || uINode2.visibleFlag != 0))
				{
					uINode2.visibleFlag = num;
					Material material = uINode2.widget.material;
					if (!mChanged.Contains(material))
					{
						mChanged.Add(material);
						mChangedLastFrame = true;
					}
				}
			}
		}
		mCulled = flag2;
		mCheckVisibility = false;
		mWidgetsAdded = false;
	}

	private void UpdateWidgets()
	{
		int i = 0;
		for (int count = mChildren.Count; i < count; i++)
		{
			UINode uINode = (UINode)mChildren[i];
			UIWidget widget = uINode.widget;
			if (uINode.visibleFlag == 1 && widget != null && widget.UpdateGeometry(this, ref mWorldToLocal, uINode.changeFlag == 1, generateNormals) && !mChanged.Contains(widget.material))
			{
				mChanged.Add(widget.material);
				mChangedLastFrame = true;
			}
			uINode.changeFlag = 0;
		}
	}

	public void UpdateDrawcalls()
	{
		Vector4 vector = Vector4.zero;
		if (mClipping != 0)
		{
			vector = new Vector4(mClipRange.x, mClipRange.y, mClipRange.z * 0.5f, mClipRange.w * 0.5f);
		}
		if (vector.z == 0f)
		{
			vector.z = (float)Screen.width * 0.5f;
		}
		if (vector.w == 0f)
		{
			vector.w = (float)Screen.height * 0.5f;
		}
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
		{
			vector.x -= 0.5f;
			vector.y += 0.5f;
		}
		Transform transform = cachedTransform;
		int i = 0;
		for (int size = mDrawCalls.size; i < size; i++)
		{
			UIDrawCall uIDrawCall = mDrawCalls.buffer[i];
			uIDrawCall.clipping = mClipping;
			uIDrawCall.clipRange = vector;
			uIDrawCall.clipSoftness = mClipSoftness;
			uIDrawCall.depthPass = depthPass && mClipping == UIDrawCall.Clipping.None;
			Transform transform2 = uIDrawCall.transform;
			transform2.position = transform.position;
			transform2.rotation = transform.rotation;
			transform2.localScale = transform.lossyScale;
		}
	}

	private void Fill(Material mat)
	{
		int num = mWidgets.size;
		while (num > 0)
		{
			if (mWidgets[--num] == null)
			{
				mWidgets.RemoveAt(num);
			}
		}
		int i = 0;
		for (int size = mWidgets.size; i < size; i++)
		{
			UIWidget uIWidget = mWidgets.buffer[i];
			if (uIWidget.visibleFlag != 1 || !(uIWidget.material == mat))
			{
				continue;
			}
			UINode node = GetNode(uIWidget.cachedTransform);
			if (node != null)
			{
				if (generateNormals)
				{
					uIWidget.WriteToBuffers(mVerts, mUvs, mCols, mNorms, mTans);
				}
				else
				{
					uIWidget.WriteToBuffers(mVerts, mUvs, mCols, null, null);
				}
			}
			else
			{
				Debug.LogError("No transform found for " + NGUITools.GetHierarchy(uIWidget.gameObject), this);
			}
		}
		if (mVerts.size > 0)
		{
			UIDrawCall drawCall = GetDrawCall(mat, true);
			drawCall.depthPass = depthPass && mClipping == UIDrawCall.Clipping.None;
			drawCall.Set(mVerts, (!generateNormals) ? null : mNorms, (!generateNormals) ? null : mTans, mUvs, mCols);
		}
		else
		{
			UIDrawCall drawCall2 = GetDrawCall(mat, false);
			if (drawCall2 != null)
			{
				mDrawCalls.Remove(drawCall2);
				NGUITools.DestroyImmediate(drawCall2.gameObject);
			}
		}
		mVerts.Clear();
		mNorms.Clear();
		mTans.Clear();
		mUvs.Clear();
		mCols.Clear();
	}

	private void LateUpdate()
	{
		mUpdateTime = Time.realtimeSinceStartup;
		UpdateTransformMatrix();
		UpdateTransforms();
		if (mLayer != base.gameObject.layer)
		{
			mLayer = base.gameObject.layer;
			UICamera uICamera = UICamera.FindCameraForLayer(mLayer);
			mCam = ((!(uICamera != null)) ? NGUITools.FindCameraForLayer(mLayer) : uICamera.cachedCamera);
			SetChildLayer(cachedTransform, mLayer);
			int i = 0;
			for (int size = drawCalls.size; i < size; i++)
			{
				mDrawCalls.buffer[i].gameObject.layer = mLayer;
			}
		}
		UpdateWidgets();
		if (mDepthChanged)
		{
			mDepthChanged = false;
			mWidgets.Sort(UIWidget.CompareFunc);
		}
		int j = 0;
		for (int size2 = mChanged.size; j < size2; j++)
		{
			Fill(mChanged.buffer[j]);
		}
		UpdateDrawcalls();
		mChanged.Clear();
		mRebuildAll = false;
	}

	public void Refresh()
	{
		UIWidget[] componentsInChildren = GetComponentsInChildren<UIWidget>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].Update();
		}
		LateUpdate();
	}

	public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(clipRange.x - num, clipRange.y - num2);
		Vector2 maxArea = new Vector2(clipRange.x + num, clipRange.y + num2);
		if (clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += clipSoftness.x;
			minArea.y += clipSoftness.y;
			maxArea.x -= clipSoftness.x;
			maxArea.y -= clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = CalculateConstrainOffset(targetBounds.min, targetBounds.max);
		if (vector.magnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += vector;
				targetBounds.center += vector;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(cachedTransform, target);
		return ConstrainTargetToBounds(target, ref targetBounds, immediate);
	}

	private static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.GetComponent<UIPanel>() == null)
			{
				child.gameObject.layer = layer;
				SetChildLayer(child, layer);
			}
		}
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		Transform transform = trans;
		UIPanel uIPanel = null;
		while (uIPanel == null && trans != null)
		{
			uIPanel = trans.GetComponent<UIPanel>();
			if (uIPanel != null || trans.parent == null)
			{
				break;
			}
			trans = trans.parent;
		}
		if (createIfMissing && uIPanel == null && trans != transform)
		{
			uIPanel = trans.gameObject.AddComponent<UIPanel>();
			SetChildLayer(uIPanel.cachedTransform, uIPanel.gameObject.layer);
		}
		return uIPanel;
	}

	public static UIPanel Find(Transform trans)
	{
		return Find(trans, true);
	}
}
