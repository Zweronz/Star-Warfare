using System.Collections;
using UnityEngine;

public class UIMesh : MonoBehaviour
{
	private int m_Layer;

	private MeshFilter m_MeshFilter;

	private MeshRenderer m_MeshRenderer;

	private ArrayList m_Sprites;

	private ArrayList m_DrawGroups;

	private int m_SubMeshCount = -1;

	public void Initialize(int layer)
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
		m_DrawGroups = new ArrayList();
	}

	public void DoLateUpdate()
	{
		m_MeshFilter.mesh.Clear();
		if (m_Sprites.Count <= 0)
		{
			return;
		}
		int num = 0;
		m_DrawGroups.Clear();
		ArrayList arrayList = null;
		for (int i = 0; i < m_Sprites.Count; i++)
		{
			Sprite sprite = (Sprite)m_Sprites[i];
			if (sprite.LoadMateMode)
			{
				if (sprite.MaterialID == -1)
				{
					continue;
				}
			}
			else if (sprite.Material == null)
			{
				continue;
			}
			num++;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				m_DrawGroups.Add(arrayList);
				arrayList.Add(sprite);
				continue;
			}
			Material material = null;
			Material material2 = null;
			material = ((!((Sprite)arrayList[0]).LoadMateMode) ? ((Sprite)arrayList[0]).Material : Res2DManager.GetInstance().vMaterial[((Sprite)arrayList[0]).MaterialID].GetMaterial());
			material2 = ((!sprite.LoadMateMode) ? sprite.Material : Res2DManager.GetInstance().vMaterial[sprite.MaterialID].GetMaterial());
			if (material != material2)
			{
				arrayList = new ArrayList();
				m_DrawGroups.Add(arrayList);
				arrayList.Add(sprite);
			}
			else
			{
				arrayList.Add(sprite);
			}
		}
		int count = m_DrawGroups.Count;
		Vector3[] array = new Vector3[num * 4];
		Vector2[] array2 = new Vector2[num * 4];
		Color[] array3 = new Color[num * 4];
		int num2 = 0;
		int[][] array4 = new int[count][];
		Material[] array5 = new Material[count];
		int num3 = 0;
		for (int j = 0; j < m_DrawGroups.Count; j++)
		{
			ArrayList arrayList2 = (ArrayList)m_DrawGroups[j];
			Material material3 = null;
			material3 = ((!((Sprite)arrayList2[0]).LoadMateMode) ? ((Sprite)arrayList2[0]).Material : Res2DManager.GetInstance().vMaterial[((Sprite)arrayList2[0]).MaterialID].GetMaterial());
			array4[num3] = new int[arrayList2.Count * 6];
			for (int k = 0; k < arrayList2.Count; k++)
			{
				Sprite sprite2 = (Sprite)arrayList2[k];
				array[num2] = sprite2.Vertices[0];
				array[num2 + 1] = sprite2.Vertices[1];
				array[num2 + 2] = sprite2.Vertices[2];
				array[num2 + 3] = sprite2.Vertices[3];
				array2[num2] = sprite2.UV[0];
				array2[num2 + 1] = sprite2.UV[1];
				array2[num2 + 2] = sprite2.UV[2];
				array2[num2 + 3] = sprite2.UV[3];
				array3[num2] = sprite2.Color;
				array3[num2 + 1] = sprite2.Color;
				array3[num2 + 2] = sprite2.Color;
				array3[num2 + 3] = sprite2.Color;
				int num4 = k * 6;
				array4[num3][num4] = num2 + Sprite.Triangles[0];
				array4[num3][num4 + 1] = num2 + Sprite.Triangles[1];
				array4[num3][num4 + 2] = num2 + Sprite.Triangles[2];
				array4[num3][num4 + 3] = num2 + Sprite.Triangles[3];
				array4[num3][num4 + 4] = num2 + Sprite.Triangles[4];
				array4[num3][num4 + 5] = num2 + Sprite.Triangles[5];
				num2 += 4;
			}
			array5[num3] = material3;
			num3++;
		}
		if (m_SubMeshCount != count)
		{
			m_SubMeshCount = count;
			Object.DestroyImmediate(m_MeshRenderer);
			m_MeshRenderer = base.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		}
		m_MeshFilter.mesh.subMeshCount = count;
		m_MeshFilter.mesh.vertices = array;
		m_MeshFilter.mesh.uv = array2;
		m_MeshFilter.mesh.colors = array3;
		for (int l = 0; l < count; l++)
		{
			m_MeshFilter.mesh.SetTriangles(array4[l], l);
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

	public void Destory()
	{
		m_Sprites.Clear();
		m_MeshFilter.mesh.Clear();
	}
}
