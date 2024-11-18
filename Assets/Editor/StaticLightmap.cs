using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using System.Linq;

public class StaticLightmap : Editor
{
	private static Dictionary<string, string> ShaderMap
	{
		get
		{
			return new Dictionary<string, string>()
			{
				{"Advanced/Lightmap", "iPhone/LightMap"}
			};
		}
	}

	private static Dictionary<string, string> TextureMap
	{
		get
		{
			return new Dictionary<string, string>
			{
				{"iPhone/LightMap", "_texBase,_texLightmap"}
			};
		}
	}

	private static void FixTiling()
	{
		List<int> tiledMeshes = GetTiledMeshes();

		string[] sceneYaml = File.ReadAllLines(EditorSceneManager.GetActiveScene().path);
		int state = 0, gameObjectID = 0;

		Vector4 scaleOffset = default(Vector4);

		for (int i = 0; i < sceneYaml.Length; i++)
		{
			int indentationLevel = GetIndentationLevel(sceneYaml[i]);
			string unIndented = sceneYaml[i].Substring(indentationLevel);

			if (indentationLevel == 0 && sceneYaml[i] == "MeshRenderer:")
			{
				state = 1;
				gameObjectID = 0;

				continue;
			}
			
			if (state > 0)
			{
				if (state == 1 && unIndented.StartsWith("m_GameObject:"))
				{
					string id = unIndented.Substring(unIndented.IndexOf("fileID: ") + 8);
					gameObjectID = int.Parse(id.Substring(0, id.IndexOf('}')));

					if (tiledMeshes.Contains(gameObjectID))
					{
						state = 2;
					}
				}
				else if (state == 2 && unIndented.StartsWith("m_LightmapTilingOffset:"))
				{
					string vector = unIndented.Substring(unIndented.IndexOf('{') + 1);
					
					string x = vector.Substring(vector.IndexOf("x: ") + 3),
					y = vector.Substring(vector.IndexOf("y: ") + 3),
					z = vector.Substring(vector.IndexOf("z: ") + 3),
					w = vector.Substring(vector.IndexOf("w: ") + 3);

					scaleOffset = new Vector4
					(
						float.Parse(x.Substring(0, x.IndexOf(","))),
						float.Parse(y.Substring(0, y.IndexOf(","))),
						float.Parse(z.Substring(0, z.IndexOf(","))),
						float.Parse(w.Substring(0, w.IndexOf("}")))
					);

					state = 3;
				}
				else if (state == 3 && unIndented.StartsWith("m_Materials:"))
				{
					state = 4;
				}
				else if (state == 4)
				{
					if (unIndented.StartsWith("- "))
					{
                        string guid;
                        try
						{
							guid = unIndented.Substring(unIndented.IndexOf("guid: ") + 6);
							guid = guid.Substring(0, guid.IndexOf(','));
						}
						catch
						{
							continue;
						}

						if (!Directory.Exists("Assets/StaticLightmap"))
						{
							Directory.CreateDirectory("Assets/StaticLightmap");
						}

						if (!Directory.Exists("Assets/StaticLightmap/" + EditorSceneManager.GetActiveScene().name))
						{
							Directory.CreateDirectory("Assets/StaticLightmap/" + EditorSceneManager.GetActiveScene().name);
						}

						Material original = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));

						if (original == null)
						{
							continue;
						}

						string lightmap = GetLightmap(original.shader.name);

						if (lightmap == null)
						{
							continue;
						}

						Material copy = new Material(original);

						copy.SetTextureScale(lightmap, new Vector2(scaleOffset.x, scaleOffset.y));
						copy.SetTextureOffset(lightmap, new Vector2(scaleOffset.z, scaleOffset.w));

						AssetDatabase.CreateAsset(copy, "Assets/StaticLightmap/" + EditorSceneManager.GetActiveScene().name + "/" + original.name + "_Tiled_" + gameObjectID + ".mat");
						sceneYaml[i] = sceneYaml[i].Replace(guid, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(copy)));
					}
					else
					{
						state = 0;
					}
				}
			}
		}

		File.WriteAllLines(EditorSceneManager.GetActiveScene().path, sceneYaml);
	}

	private static List<int> GetTiledMeshes()
	{
		string combinedMeshGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(FindObjectsOfType<MeshRenderer>().ToList().Find(x => x.isPartOfStaticBatch).GetComponent<MeshFilter>().sharedMesh));

		string[] sceneYaml = File.ReadAllLines(EditorSceneManager.GetActiveScene().path);
		bool inMeshFilter = false;

		int gameObject = 0;
		List<int> tiledMeshes = new List<int>();

		for (int i = 0; i < sceneYaml.Length; i++)
		{
			int indentationLevel = GetIndentationLevel(sceneYaml[i]);
			string unIndented = sceneYaml[i].Substring(indentationLevel);

			if (indentationLevel == 0 && sceneYaml[i] == "MeshFilter:")
			{
				gameObject = 0;

				inMeshFilter = true;
				continue;
			}

			if (inMeshFilter)
			{
				if (unIndented.StartsWith("m_GameObject:"))
				{
					string id = unIndented.Substring(unIndented.IndexOf("fileID: ") + 8);
					gameObject = int.Parse(id.Substring(0, id.IndexOf('}')));
				}
				else if (unIndented.StartsWith("m_Mesh:"))
				{
					string guid = unIndented.Substring(unIndented.IndexOf("guid: ") + 6);

					if (guid.Substring(0, guid.IndexOf(',')) != combinedMeshGUID)
					{
						tiledMeshes.Add(gameObject);
					}

					inMeshFilter = false;
				}
			}
		}

		return tiledMeshes;
	}

	[MenuItem("Static Lightmaps/Optimize Lightmaps")]
	public static void Optimize()
	{
		Dictionary<int, Dictionary<string, string>> materials = MaterialMap(MaterialsToCreate());

		string[] sceneYaml = File.ReadAllLines(EditorSceneManager.GetActiveScene().path);
		int state = 0;

		int lightmapIndex = 255;

		for (int i = 0 ; i < sceneYaml.Length; i++)
		{
			int indentationLevel = GetIndentationLevel(sceneYaml[i]);
			string unIndented = sceneYaml[i].Substring(indentationLevel);

			if (state > 0 && indentationLevel == 0 && sceneYaml[i] != "MeshRenderer:")
			{
				state = 0;
				lightmapIndex = 25;

				continue;
			}

			switch (state)
			{
				case 0:
					if (indentationLevel == 0 && sceneYaml[i] == "MeshRenderer:")
					{
						state = 1;
					}
					break;

				case 1:
					if (unIndented.StartsWith("m_LightmapIndex:"))
					{
						lightmapIndex = int.Parse(unIndented.Split(' ')[1]);
					}
					else if (unIndented.StartsWith("m_Materials:"))
					{
						state = 2;
					}
					break;

				case 2:
					if (unIndented.StartsWith("- "))
					{
						if (lightmapIndex < 255)
						{
							string guid = unIndented.Substring(unIndented.IndexOf("guid: ") + 6);
							guid = guid.Substring(0, guid.IndexOf(','));

							sceneYaml[i] = sceneYaml[i].Replace(guid, materials[lightmapIndex][guid]);
						}
					}
					else
					{
						state = 1;
					}
					break;
			}
		}

		File.WriteAllLines(EditorSceneManager.GetActiveScene().path, sceneYaml);
		FixTiling();
	}

	private static string GetShader(string original)
	{
		if (!ShaderMap.ContainsKey(original))
		{
			return original;
		}

		return ShaderMap[original];
	}

	private static string GetTexture(string shader)
	{
		if (!TextureMap.ContainsKey(shader))
		{
			return "_MainTex";
		}

		return TextureMap[shader].Split(',')[0];
	}

	private static string GetLightmap(string shader)
	{
		if (!TextureMap.ContainsKey(shader))
		{
			return null;
		}

		return TextureMap[shader].Split(',')[1];
	}

	private static Dictionary<int, Dictionary<string, string>> MaterialMap(Dictionary<int, List<string>> materials)
	{
		Dictionary<int, Dictionary<string, string>> materialMap = new Dictionary<int, Dictionary<string, string>>();
		
		foreach (int index in materials.Keys)
		{
			materialMap.Add(index, new Dictionary<string, string>());

			foreach (string guid in materials[index])
			{
				if (!Directory.Exists("Assets/StaticLightmap"))
				{
					Directory.CreateDirectory("Assets/StaticLightmap");
				}

				if (!Directory.Exists("Assets/StaticLightmap/" + EditorSceneManager.GetActiveScene().name))
				{
					Directory.CreateDirectory("Assets/StaticLightmap/" + EditorSceneManager.GetActiveScene().name);
				}

				Material original = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));
				Material material = new Material(Shader.Find(GetShader(original.shader.name)));

				material.SetTexture(GetTexture(material.shader.name), original.GetTexture(GetTexture(original.shader.name)));
				string lightmap = GetLightmap(material.shader.name);

				if (lightmap == null)
				{
					materialMap[index].Add(guid, guid);
					continue;
				}
				
				material.SetTexture(lightmap, AssetDatabase.LoadAssetAtPath<Texture2D>(EditorSceneManager.GetActiveScene().path.Replace(".unity", "") + "/LightmapNear-" + index + ".png"));

				AssetDatabase.CreateAsset(material, "Assets/StaticLightmap/" + EditorSceneManager.GetActiveScene().name + "/" + original.name + "_" + index + ".mat");
				materialMap[index].Add(guid, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(material)));
			}
		}

		return materialMap;
	}

	private static Dictionary<int, List<string>> MaterialsToCreate()
	{
		string[] sceneYaml = File.ReadAllLines(EditorSceneManager.GetActiveScene().path);
		int state = 0;

		int lightmapIndex = 255;
		List<string> materials = new List<string>();

		List<RendererInstance> renderers = new List<RendererInstance>();

		for (int i = 0 ; i < sceneYaml.Length; i++)
		{
			int indentationLevel = GetIndentationLevel(sceneYaml[i]);
			string unIndented = sceneYaml[i].Substring(indentationLevel);

			if (state > 0 && indentationLevel == 0 && sceneYaml[i] != "MeshRenderer:")
			{
				state = 0;
				
				if (lightmapIndex < 255)
				{
					List<string> takenMaterials = new List<string>();

					foreach (RendererInstance renderer in renderers.FindAll(x => x.lightmapIndex == lightmapIndex))
					{
						foreach (string material in renderer.materials)
						{
							if (!takenMaterials.Contains(material))
							{
								takenMaterials.Add(material);
							}
						}
					}

					List<string> freeMaterials = materials.FindAll(x => !takenMaterials.Contains(x));

					if (freeMaterials.Count > 0)
					{
						renderers.Add(new RendererInstance
						{
							lightmapIndex = lightmapIndex,
							materials = freeMaterials
						});
					}
				}

				lightmapIndex = 255;
				materials.Clear();

				continue;
			}

			switch (state)
			{
				case 0:
					if (indentationLevel == 0 && sceneYaml[i] == "MeshRenderer:")
					{
						state = 1;
					}
					break;

				case 1:
					if (unIndented.StartsWith("m_LightmapIndex:"))
					{
						lightmapIndex = int.Parse(unIndented.Split(' ')[1]);
					}
					else if (unIndented.StartsWith("m_Materials:"))
					{
						state = 2;
					}
					break;

				case 2:
					if (unIndented.StartsWith("- "))
					{
						try
						{
							string guid = unIndented.Substring(unIndented.IndexOf("guid: ") + 6);

							materials.Add(guid.Substring(0, guid.IndexOf(',')));
						}
						catch {}
					}
					else
					{
						state = 1;
					}
					break;
			}
		}

		Dictionary<int, List<string>> materialLists = new Dictionary<int, List<string>>();

		foreach (RendererInstance renderer in renderers)
		{
			if (!materialLists.ContainsKey(renderer.lightmapIndex))
			{
				materialLists.Add(renderer.lightmapIndex, new List<string>());
			}

			foreach (string material in renderer.materials)
			{
				if (!materialLists[renderer.lightmapIndex].Contains(material))
				{
					materialLists[renderer.lightmapIndex].Add(material);
				}
			}
		}

		return materialLists;
	}

	private class RendererInstance
	{
		public int lightmapIndex;

		public List<string> materials;
	}

	private static int GetIndentationLevel(string line)
	{
		int count = 0;

		for (int i = 0; i < line.Length; i++)
		{
			switch (line[i])
			{
				case ' ': count++; break;
				//case '\t': count += 4; break;

				default: return count;
			}
		}

		return count;
	}
}