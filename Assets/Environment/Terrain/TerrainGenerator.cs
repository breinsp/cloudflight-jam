using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainGenerator : MonoBehaviour
{
    public float size;
    public int vertexCount;
    public float amplitude;
    public float frequency;
    public GameObject terrain;

    private Vector3 offset;

    private Vector3 noiseOffset;

    void Awake()
    {
        offset = new Vector3(size, 0, size) * -0.5f;
        noiseOffset = Random.insideUnitSphere * Random.Range(1000f, 100000f);
        Mesh mesh = GenerateMesh();
        terrain.GetComponent<MeshFilter>().mesh = mesh;
        terrain.GetComponent<MeshCollider>().sharedMesh = mesh;
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
        Vector3 POffset = P + noiseOffset; //offset perlin

        float y1 = Mathf.PerlinNoise(POffset.x * frequency, POffset.z * frequency);
        float y2 = Mathf.PerlinNoise(POffset.x * frequency * 2, POffset.z * frequency * 2) * 0.5f;
        float y3 = Mathf.PerlinNoise(POffset.x * frequency * 4, POffset.z * frequency * 4) * 0.25f;
        float y4 = Mathf.PerlinNoise(POffset.x * frequency * 8, POffset.z * frequency * 8) * 0.125f;

        float y = (y1 + y2 + y3 + y4) / 2f;
        y = Mathf.Pow(y, 2);

        P.y = y * mag * amplitude;

        return P;
    }
}
