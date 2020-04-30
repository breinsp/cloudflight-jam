﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainGenerator : MonoBehaviour
{
    public float size;
    public int vertexCount;
    public float amplitude;
    public float frequency;

    public Material material;

    private GameObject terrain;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(size, 0, size) * -0.5f;
        terrain = new GameObject("Terrain");
        terrain.transform.SetParent(transform);
        terrain.AddComponent<MeshRenderer>().material = material;
        Mesh mesh = GenerateMesh();
        terrain.AddComponent<MeshFilter>().mesh = mesh;
        terrain.AddComponent<MeshCollider>().sharedMesh = mesh;
        terrain.tag = "Terrain";
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;

        List<Vector3> vertices = new List<Vector3>();

        float gapSize = size / (vertexCount - 1);

        for (int i = 0; i < vertexCount; i++)
        {
            for (int j = 0; j < vertexCount; j++)
            {
                if (i != 0 && j != 0)
                {
                    AddQuad(vertices, i, j, gapSize);
                }
            }
        }

        int[] triangles = new int[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            triangles[i] = i;
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void AddQuad(List<Vector3> vertices, int i, int j, float size)
    {
        float x = (i - 1) * size;
        float y = (j - 1) * size;

        float x1 = x;
        float x2 = x + size;
        float y1 = y;
        float y2 = y + size;

        Vector3 topLeft = new Vector3(x1, 0, y1) + offset;
        Vector3 topRight = new Vector3(x2, 0, y1) + offset;
        Vector3 botLeft = new Vector3(x1, 0, y2) + offset;
        Vector3 botRight = new Vector3(x2, 0, y2) + offset;

        topLeft = ApplyNoise(topLeft);
        topRight = ApplyNoise(topRight);
        botLeft = ApplyNoise(botLeft);
        botRight = ApplyNoise(botRight);

        if (Random.Range(0f, 1f) < 0.5f)
        {
            AddTriangle(vertices, botRight, topRight, topLeft);
            AddTriangle(vertices, topLeft, botLeft, botRight);
        }
        else
        {
            AddTriangle(vertices, topRight, topLeft, botLeft);
            AddTriangle(vertices, botLeft, botRight, topRight);
        }
    }

    private void AddTriangle(List<Vector3> vertices, Vector3 A, Vector3 B, Vector3 C)
    {
        vertices.Add(A);
        vertices.Add(B);
        vertices.Add(C);
    }

    private Vector3 ApplyNoise(Vector3 P)
    {
        float mag = P.magnitude / size;
        float y1 = Mathf.PerlinNoise(P.x * frequency, P.z * frequency);
        float y2 = Mathf.PerlinNoise(P.x * frequency * 2, P.z * frequency * 2) * 0.5f;

        P.y = (y1 + y2) * mag * amplitude;

        return P;
    }
}
