using UnityEngine;

public class Sprite2DManager : MonoBehaviour
{
	public int LAYER;

	public int MAX_SPRITE_LAYER = 16;

	private SpriteMesh m_SpriteMesh;

	private SpriteCamera m_SpriteCamera;

	~Sprite2DManager()
	{
	}

	public void Awake()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void Start()
	{
	}

	public void Add(Sprite2D sprite)
	{
		m_SpriteMesh.Add(sprite);
	}

	public void Remove(Sprite2D sprite)
	{
		m_SpriteMesh.Remove(sprite);
	}

	public void RemoveAll()
	{
		m_SpriteMesh.RemoveAll();
	}

	public SpriteMesh GetSpriteMesh()
	{
		return m_SpriteMesh;
	}

	public SpriteCamera GetSpriteCamera()
	{
		return m_SpriteCamera;
	}

	public void SetViewport(Rect range)
	{
		m_SpriteCamera.SetViewport(range);
	}

	public void SetViewport(Vector2 position, Vector2 size)
	{
		Rect viewport = new Rect(position.x - size.x / 2f, position.y - size.y / 2f, size.x, size.y);
		m_SpriteCamera.SetViewport(viewport);
	}

	public Vector2 ScreenToWorld(Vector2 point)
	{
		return m_SpriteCamera.ScreenToWorld(point);
	}

	private void Initialize()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject gameObject = new GameObject("SpriteMesh");
		gameObject.transform.parent = base.gameObject.transform;
		m_SpriteMesh = (SpriteMesh)gameObject.AddComponent(typeof(SpriteMesh));
		m_SpriteMesh.Initialize(LAYER, MAX_SPRITE_LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject gameObject = new GameObject("SpriteCamera");
		gameObject.transform.parent = base.gameObject.transform;
		m_SpriteCamera = (SpriteCamera)gameObject.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(true);
		m_SpriteCamera.SetDepth(0f);
	}
}
