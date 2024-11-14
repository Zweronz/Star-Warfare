using System.Collections;
using UnityEngine;

public class SpriteMesh : MonoBehaviour
{
	private int m_Layer;

	private MeshFilter m_MeshFilter;

	private MeshRenderer m_MeshRenderer;

	private ArrayList m_Sprites;

	private int m_MaxSpriteLayer;

	private Hashtable[] m_SpritesGroup;

	public void Initialize(int layer, int max_sprite_layer)
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		m_Layer = layer;
		base.gameObject.layer = m_Layer;
		m_MeshFilter = (MeshFilter)base.gameObject.AddComponent(typeof(MeshFilter));
		m_MeshRenderer = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		m_MeshRenderer.castShadows = false;
		m_MeshRenderer.receiveShadows = false;
		m_Sprites = new ArrayList();
		m_MaxSpriteLayer = max_sprite_layer;
		m_SpritesGroup = new Hashtable[m_MaxSpriteLayer];
		for (int i = 0; i < m_MaxSpriteLayer; i++)
		{
			m_SpritesGroup[i] = new Hashtable();
		}
	}

	public void LateUpdate()
	{
		m_MeshFilter.mesh.Clear();
		if (m_Sprites.Count <= 0)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < m_MaxSpriteLayer; i++)
		{
			m_SpritesGroup[i].Clear();
		}
		for (int j = 0; j < m_Sprites.Count; j++)
		{
			Sprite sprite = (Sprite)m_Sprites[j];
			int layer = sprite.Layer;
			if (layer < 0 || layer >= m_MaxSpriteLayer)
			{
				continue;
			}
			Material material = null;
			if (sprite.LoadMateMode)
			{
				material = Res2DManager.GetInstance().vMaterial[sprite.MaterialID].GetMaterial();
			}
			if (!(material == null))
			{
				num++;
				if (m_SpritesGroup[layer].Contains(material))
				{
					ArrayList arrayList = (ArrayList)m_SpritesGroup[layer][material];
					arrayList.Add(sprite);
				}
				else
				{
					ArrayList arrayList2 = new ArrayList();
					arrayList2.Add(sprite);
					m_SpritesGroup[layer].Add(material, arrayList2);
				}
			}
		}
		int num2 = 0;
		for (int k = 0; k < m_MaxSpriteLayer; k++)
		{
			num2 += m_SpritesGroup[k].Count;
		}
		Vector3[] array = new Vector3[num * 4];
		Vector2[] array2 = new Vector2[num * 4];
		Color[] array3 = new Color[num * 4];
		int num3 = 0;
		int[][] array4 = new int[num2][];
		Material[] array5 = new Material[num2];
		int num4 = 0;
		for (int l = 0; l < m_MaxSpriteLayer; l++)
		{
			if (m_SpritesGroup[l].Count <= 0)
			{
				continue;
			}
			Material[] array6 = new Material[m_SpritesGroup[l].Count];
			m_SpritesGroup[l].Keys.CopyTo(array6, 0);
			foreach (Material material2 in array6)
			{
				ArrayList arrayList3 = (ArrayList)m_SpritesGroup[l][material2];
				array4[num4] = new int[arrayList3.Count * 6];
				for (int n = 0; n < arrayList3.Count; n++)
				{
					Sprite sprite2 = (Sprite)arrayList3[n];
					array[num3] = sprite2.Vertices[0];
					array[num3 + 1] = sprite2.Vertices[1];
					array[num3 + 2] = sprite2.Vertices[2];
					array[num3 + 3] = sprite2.Vertices[3];
					array2[num3] = sprite2.UV[0];
					array2[num3 + 1] = sprite2.UV[1];
					array2[num3 + 2] = sprite2.UV[2];
					array2[num3 + 3] = sprite2.UV[3];
					array3[num3] = sprite2.Color;
					array3[num3 + 1] = sprite2.Color;
					array3[num3 + 2] = sprite2.Color;
					array3[num3 + 3] = sprite2.Color;
					int num5 = n * 6;
					array4[num4][num5] = num3 + Sprite.Triangles[0];
					array4[num4][num5 + 1] = num3 + Sprite.Triangles[1];
					array4[num4][num5 + 2] = num3 + Sprite.Triangles[2];
					array4[num4][num5 + 3] = num3 + Sprite.Triangles[3];
					array4[num4][num5 + 4] = num3 + Sprite.Triangles[4];
					array4[num4][num5 + 5] = num3 + Sprite.Triangles[5];
					num3 += 4;
				}
				array5[num4] = material2;
				num4++;
			}
		}
		m_MeshFilter.mesh.subMeshCount = num2;
		m_MeshFilter.mesh.vertices = array;
		m_MeshFilter.mesh.uv = array2;
		m_MeshFilter.mesh.colors = array3;
		for (int num6 = 0; num6 < num2; num6++)
		{
			m_MeshFilter.mesh.SetTriangles(array4[num6], num6);
		}
		m_MeshRenderer.materials = array5;
	}

	public void Add(Sprite sprite)
	{
		m_Sprites.Add(sprite);
	}

	public void Remove(Sprite sprite)
	{
		m_Sprites.Remove(sprite);
	}

	public void RemoveAll()
	{
		m_Sprites.Clear();
	}
}
