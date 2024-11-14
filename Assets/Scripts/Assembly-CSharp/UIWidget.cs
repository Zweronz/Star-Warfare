using System;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
	public enum Pivot
	{
		TopLeft = 0,
		Top = 1,
		TopRight = 2,
		Left = 3,
		Center = 4,
		Right = 5,
		BottomLeft = 6,
		Bottom = 7,
		BottomRight = 8
	}

	[SerializeField]
	[HideInInspector]
	protected Material mMat;

	[HideInInspector]
	[SerializeField]
	protected Texture mTex;

	[SerializeField]
	[HideInInspector]
	private Color mColor = Color.white;

	[SerializeField]
	[HideInInspector]
	private Pivot mPivot = Pivot.Center;

	[HideInInspector]
	[SerializeField]
	private int mDepth;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected bool mChanged = true;

	protected bool mPlayMode = true;

	private GameObject mGo;

	private Vector3 mDiffPos;

	private Quaternion mDiffRot;

	private Vector3 mDiffScale;

	private int mVisibleFlag = -1;

	private UIGeometry mGeom = new UIGeometry();

	public bool isVisible
	{
		get
		{
			return finalAlpha > 0.001f;
		}
	}

	public Color color
	{
		get
		{
			return mColor;
		}
		set
		{
			if (!mColor.Equals(value))
			{
				mColor = value;
				mChanged = true;
			}
		}
	}

	public float alpha
	{
		get
		{
			return mColor.a;
		}
		set
		{
			Color color = mColor;
			color.a = value;
			this.color = color;
		}
	}

	public float finalAlpha
	{
		get
		{
			if (mPanel == null)
			{
				CreatePanel();
			}
			return (!(mPanel != null)) ? mColor.a : (mColor.a * mPanel.alpha);
		}
	}

	public Pivot pivot
	{
		get
		{
			return mPivot;
		}
		set
		{
			if (mPivot != value)
			{
				Vector3 vector = NGUIMath.CalculateWidgetCorners(this)[0];
				mPivot = value;
				mChanged = true;
				Vector3 vector2 = NGUIMath.CalculateWidgetCorners(this)[0];
				Transform transform = cachedTransform;
				Vector3 position = transform.position;
				float z = transform.localPosition.z;
				position.x += vector.x - vector2.x;
				position.y += vector.y - vector2.y;
				cachedTransform.position = position;
				position = cachedTransform.localPosition;
				position.x = Mathf.Round(position.x);
				position.y = Mathf.Round(position.y);
				position.z = z;
				cachedTransform.localPosition = position;
			}
		}
	}

	public int depth
	{
		get
		{
			return mDepth;
		}
		set
		{
			if (mDepth != value)
			{
				mDepth = value;
				if (mPanel != null)
				{
					mPanel.MarkMaterialAsChanged(material, true);
				}
			}
		}
	}

	public Vector2 pivotOffset
	{
		get
		{
			Vector2 zero = Vector2.zero;
			Vector4 vector = relativePadding;
			Pivot pivot = this.pivot;
			switch (pivot)
			{
			case Pivot.Top:
			case Pivot.Center:
			case Pivot.Bottom:
				zero.x = (vector.x - vector.z - 1f) * 0.5f;
				break;
			case Pivot.TopRight:
			case Pivot.Right:
			case Pivot.BottomRight:
				zero.x = -1f - vector.z;
				break;
			default:
				zero.x = vector.x;
				break;
			}
			switch (pivot)
			{
			case Pivot.Left:
			case Pivot.Center:
			case Pivot.Right:
				zero.y = (vector.w - vector.y + 1f) * 0.5f;
				break;
			case Pivot.BottomLeft:
			case Pivot.Bottom:
			case Pivot.BottomRight:
				zero.y = 1f + vector.w;
				break;
			default:
				zero.y = 0f - vector.y;
				break;
			}
			return zero;
		}
	}

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

	public virtual Material material
	{
		get
		{
			return mMat;
		}
		set
		{
			if (mMat != value)
			{
				if (mMat != null && mPanel != null)
				{
					mPanel.RemoveWidget(this);
				}
				mPanel = null;
				mMat = value;
				mTex = null;
				if (mMat != null)
				{
					CreatePanel();
				}
			}
		}
	}

	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			if (material != null)
			{
				if (material.mainTexture != null)
				{
					mTex = material.mainTexture;
				}
				else if (mTex != null)
				{
					if (mPanel != null)
					{
						mPanel.RemoveWidget(this);
					}
					mPanel = null;
					mMat.mainTexture = mTex;
					if (base.enabled)
					{
						CreatePanel();
					}
				}
			}
			return mTex;
		}
		set
		{
			Material material = this.material;
			if (!(material == null) && !(material.mainTexture != value))
			{
				return;
			}
			if (mPanel != null)
			{
				mPanel.RemoveWidget(this);
			}
			mPanel = null;
			mTex = value;
			material = this.material;
			if (material != null)
			{
				material.mainTexture = value;
				if (base.enabled)
				{
					CreatePanel();
				}
			}
		}
	}

	public UIPanel panel
	{
		get
		{
			if (mPanel == null)
			{
				CreatePanel();
			}
			return mPanel;
		}
		set
		{
			mPanel = value;
		}
	}

	public int visibleFlag
	{
		get
		{
			return mVisibleFlag;
		}
		set
		{
			mVisibleFlag = value;
		}
	}

	public virtual Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	public virtual Vector4 relativePadding
	{
		get
		{
			return Vector4.zero;
		}
	}

	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
	}

	public virtual bool keepMaterial
	{
		get
		{
			return false;
		}
	}

	public virtual bool pixelPerfectAfterResize
	{
		get
		{
			return false;
		}
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		return 0;
	}

	public void MarkAsChangedLite()
	{
		mChanged = true;
	}

	public virtual void MarkAsChanged()
	{
		mChanged = true;
		if (mPanel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !Application.isPlaying && material != null)
		{
			mPanel.AddWidget(this);
			CheckLayer();
		}
	}

	public void CreatePanel()
	{
		if (mPanel == null && base.enabled && NGUITools.GetActive(base.gameObject) && material != null)
		{
			mPanel = UIPanel.Find(cachedTransform);
			if (mPanel != null)
			{
				CheckLayer();
				mPanel.AddWidget(this);
				mChanged = true;
			}
		}
	}

	public void CheckLayer()
	{
		if (mPanel != null && mPanel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = mPanel.gameObject.layer;
		}
	}

	[Obsolete("Use ParentHasChanged() instead")]
	public void CheckParent()
	{
		ParentHasChanged();
	}

	public void ParentHasChanged()
	{
		if (!(mPanel != null))
		{
			return;
		}
		bool flag = true;
		Transform parent = cachedTransform.parent;
		while (parent != null && !(parent == mPanel.cachedTransform))
		{
			if (!mPanel.WatchesTransform(parent))
			{
				flag = false;
				break;
			}
			parent = parent.parent;
		}
		if (!flag)
		{
			if (!keepMaterial || Application.isPlaying)
			{
				material = null;
			}
			mPanel = null;
			CreatePanel();
		}
	}

	protected virtual void Awake()
	{
		mGo = base.gameObject;
		mPlayMode = Application.isPlaying;
	}

	protected virtual void OnEnable()
	{
		mChanged = true;
		if (!keepMaterial)
		{
			mMat = null;
			mTex = null;
		}
		mPanel = null;
	}

	private void Start()
	{
		OnStart();
		CreatePanel();
	}

	public void Update()
	{
		CheckLayer();
		if (mPanel == null)
		{
			CreatePanel();
		}
		Vector3 localScale = cachedTransform.localScale;
		if (localScale.z != 1f)
		{
			localScale.z = 1f;
			mTrans.localScale = localScale;
		}
	}

	private void OnDisable()
	{
		if (!keepMaterial)
		{
			material = null;
		}
		else if (mPanel != null)
		{
			mPanel.RemoveWidget(this);
		}
		mPanel = null;
	}

	private void OnDestroy()
	{
		if (mPanel != null)
		{
			mPanel.RemoveWidget(this);
			mPanel = null;
		}
	}

	public bool UpdateGeometry(UIPanel p, ref Matrix4x4 worldToPanel, bool parentMoved, bool generateNormals)
	{
		if (material == null)
		{
			return false;
		}
		if (OnUpdate() || mChanged)
		{
			mChanged = false;
			if (NGUITools.GetActive(mGo))
			{
				mPanel = p;
				mGeom.Clear();
				OnFill(mGeom.verts, mGeom.uvs, mGeom.cols);
				if (mGeom.hasVertices)
				{
					Vector3 vector = pivotOffset;
					Vector2 vector2 = relativeSize;
					vector.x *= vector2.x;
					vector.y *= vector2.y;
					mGeom.ApplyOffset(vector);
					mGeom.ApplyTransform(worldToPanel * cachedTransform.localToWorldMatrix, generateNormals);
				}
				return true;
			}
			if (mGeom.hasVertices)
			{
				mGeom.Clear();
				return true;
			}
			return false;
		}
		if (mGeom.hasVertices && parentMoved)
		{
			mGeom.ApplyTransform(worldToPanel * cachedTransform.localToWorldMatrix, generateNormals);
		}
		return false;
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		mGeom.WriteToBuffers(v, u, c, n, t);
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 localScale = cachedTransform.localScale;
		int num = Mathf.RoundToInt(localScale.x);
		int num2 = Mathf.RoundToInt(localScale.y);
		localScale.x = num;
		localScale.y = num2;
		localScale.z = 1f;
		Vector3 localPosition = cachedTransform.localPosition;
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		if (num % 2 == 1 && (pivot == Pivot.Top || pivot == Pivot.Center || pivot == Pivot.Bottom))
		{
			localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
		}
		else
		{
			localPosition.x = Mathf.Round(localPosition.x);
		}
		if (num2 % 2 == 1 && (pivot == Pivot.Left || pivot == Pivot.Center || pivot == Pivot.Right))
		{
			localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
		}
		else
		{
			localPosition.y = Mathf.Round(localPosition.y);
		}
		cachedTransform.localPosition = localPosition;
		cachedTransform.localScale = localScale;
	}

	protected virtual void OnStart()
	{
	}

	public virtual bool OnUpdate()
	{
		return false;
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}
}
