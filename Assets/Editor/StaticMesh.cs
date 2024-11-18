using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using System.Runtime.InteropServices;
using System.IO;
using UnityEditor.SceneManagement;

public class StaticMesh : Editor
{
    [MenuItem("Static Mesh Separation/Split Scene")]
    public static void SplitScene()
    {
        Mesh target = null;

        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();
        MeshFilter[] filters = renderers.Select(x => x.GetComponent<MeshFilter>()).ToArray();
        
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].isPartOfStaticBatch)
            {
                target = filters[i].sharedMesh;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("no combined mesh in scene");
            return;
        }

        Mesh[] splitMeshes = MeshManager.SplitMesh(target);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].isPartOfStaticBatch)
            {
                MeshCollider collider = renderers[i].GetComponent<MeshCollider>();

                if (collider != null && collider.sharedMesh != null)
                {
                    filters[i].sharedMesh = collider.sharedMesh;
                }
                else
                {
                    //thanks, unity.
                    short subMeshIndex = Marshal.ReadInt16(new IntPtr(renderers[i].GetPointer().ToInt64() + 0x134));

                    if (renderers[i].sharedMaterials.Length > 1)
                    {
                        Mesh[] subMeshes = new Mesh[renderers[i].sharedMaterials.Length];

                        for (int j = 0; j < subMeshes.Length; j++)
                        {
                            subMeshes[j] = splitMeshes[subMeshIndex + j];
                        }

                        Mesh mesh = MeshManager.CombineMesh(subMeshes);

                        mesh.vertices = OffsetVertices(renderers[i].transform, mesh.vertices);
                        mesh.name = renderers[i].name;

                        filters[i].sharedMesh = mesh;

                        if (!Directory.Exists("Assets/StaticMesh"))
                        { 
                            Directory.CreateDirectory("Assets/StaticMesh");
                        }

                        if (!Directory.Exists("Assets/StaticMesh/" + EditorSceneManager.GetActiveScene().name))
                        {
                            Directory.CreateDirectory("Assets/StaticMesh/" + EditorSceneManager.GetActiveScene().name);
                        }

                        AssetDatabase.CreateAsset(mesh, "Assets/StaticMesh/" + EditorSceneManager.GetActiveScene().name + "/" + mesh.name + ".asset");
                    }
                    else
                    {
                        Mesh mesh = CopyMesh(splitMeshes[subMeshIndex]);

                        mesh.vertices = OffsetVertices(renderers[i].transform, mesh.vertices);
                        mesh.name = renderers[i].name;

                        filters[i].sharedMesh = mesh;

                        if (!Directory.Exists("Assets/StaticMesh"))
                        { 
                            Directory.CreateDirectory("Assets/StaticMesh");
                        }

                        if (!Directory.Exists("Assets/StaticMesh/" + EditorSceneManager.GetActiveScene().name))
                        {
                            Directory.CreateDirectory("Assets/StaticMesh/" + EditorSceneManager.GetActiveScene().name);
                        }

                        AssetDatabase.CreateAsset(mesh, "Assets/StaticMesh/" + EditorSceneManager.GetActiveScene().name + "/" + mesh.name + ".asset");
                    }
                }
            }
        }
    }

    private static Mesh CopyMesh(Mesh source)
    {
        Mesh mesh = new Mesh();

        if (source.vertices != null && source.vertices.Length > 0) mesh.vertices = source.vertices;
        if (source.normals != null && source.normals.Length > 0) mesh.normals = source.normals;
        if (source.tangents != null && source.tangents.Length > 0) mesh.tangents = source.tangents;

        if (source.colors != null && source.colors.Length > 0) mesh.colors = source.colors;
        if (source.colors32 != null && source.colors32.Length > 0) mesh.colors32 = source.colors32;

        if (source.uv != null && source.uv.Length > 0) mesh.uv = source.uv;
        if (source.uv2 != null && source.uv2.Length > 0) mesh.uv2 = source.uv2;
        if (source.uv3 != null && source.uv3.Length > 0) mesh.uv3 = source.uv3;
        if (source.uv4 != null && source.uv4.Length > 0) mesh.uv4 = source.uv4;
        
        mesh.triangles = source.triangles;

        return mesh;
    }

    private static Vector3[] OffsetVertices(Transform target, Vector3[] inputVertices)
    {
        Vector3[] vertices = new Vector3[inputVertices.Length];
        Matrix4x4 inverse = target.localToWorldMatrix.inverse;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = inverse.MultiplyPoint3x4(inputVertices[i]);
        }

        return vertices;
    }
}

public static class MeshManager
{
    public static Mesh CombineMesh(Mesh[] subMeshes)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>(), normals = new List<Vector3>();
        List<Vector4> tangents = new List<Vector4>();

        List<Color> colors = new List<Color>();
        List<Color32> colors32 = new List<Color32>();

        List<Vector2> uv = new List<Vector2>(), uv2 = new List<Vector2>(),
        uv3 = new List<Vector2>(), uv4 = new List<Vector2>();

        for (int i = 0; i < subMeshes.Length; i++)
        {
            vertices.AddRange(subMeshes[i].vertices);
            normals.AddRange(subMeshes[i].normals);
            tangents.AddRange(subMeshes[i].tangents);

            colors.AddRange(subMeshes[i].colors);
            colors32.AddRange(subMeshes[i].colors32);

            uv.AddRange(subMeshes[i].uv);
            uv2.AddRange(subMeshes[i].uv2);
            uv3.AddRange(subMeshes[i].uv3);
            uv4.AddRange(subMeshes[i].uv4);
        }

        if (vertices.Count > 0) mesh.vertices = vertices.ToArray();
        if (normals.Count > 0) mesh.normals = normals.ToArray();
        if (tangents.Count > 0) mesh.tangents = tangents.ToArray();

        if (colors.Count > 0) mesh.colors = colors.ToArray();
        if (colors32.Count > 0) mesh.colors32 = colors32.ToArray();

        if (uv.Count > 0) mesh.uv = uv.ToArray();
        if (uv2.Count > 0) mesh.uv2 = uv2.ToArray();
        if (uv3.Count > 0) mesh.uv3 = uv3.ToArray();
        if (uv4.Count > 0) mesh.uv4 = uv4.ToArray();

        mesh.subMeshCount = subMeshes.Length;

        for (int i = 0; i < subMeshes.Length; i++)
        {
            mesh.SetTriangles(subMeshes[i].triangles, i, false, i == 0 ? 0 : subMeshes[i - 1].vertexCount);
        }

        return mesh;
    }

    public static Mesh[] SplitMesh(Mesh input)
    {
        Mesh[] meshes = new Mesh[input.subMeshCount];

        for (int i = 0; i < meshes.Length; i++)
        {
            uint indexStart = input.GetIndexStart(i), indexCount = input.GetIndexCount(i),
            baseVertex = input.GetBaseVertex(i);

            int[] triangles = input.GetTriangles(i, true);
            meshes[i] = new Mesh();

            int vertexCount = triangles.Max() + 1;

            if (input.vertices != null && input.vertices.Length > 0)
            {
                meshes[i].vertices = input.vertices.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }

            if (input.normals != null && input.normals.Length > 0)
            {
                meshes[i].normals = input.normals.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }

            if (input.tangents != null && input.tangents.Length > 0)
            {
                meshes[i].tangents = input.tangents.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }

            if (input.uv != null && input.uv.Length > 0)
            {
                meshes[i].uv = input.uv.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }
            if (input.uv2 != null && input.uv2.Length > 0)
            {
                meshes[i].uv2 = input.uv2.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }
            if (input.uv3 != null && input.uv3.Length > 0)
            {
                meshes[i].uv3 = input.uv3.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }
            if (input.uv4 != null && input.uv4.Length > 0)
            {
                meshes[i].uv4 = input.uv4.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }

            if (input.colors != null && input.colors.Length > 0)
            {
                meshes[i].colors = input.colors.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }
            if (input.colors32 != null && input.colors32.Length > 0)
            {
                meshes[i].colors32 = input.colors32.Skip((int)baseVertex).Take(vertexCount).ToArray();
            }

            meshes[i].triangles = triangles;
        }

        return meshes;
    }
}