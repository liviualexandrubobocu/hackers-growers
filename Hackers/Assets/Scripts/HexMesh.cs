using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{

    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;
    List<Vector3> normals;
    List<Color> colors;

    MeshCollider meshCollider;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        normals = new List<Vector3>();
        colors = new List<Color>();
    }

    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        normals.Clear();
        colors.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();

        hexMesh.uv = uvs.ToArray();
        hexMesh.normals = normals.ToArray();
        //hexMesh.RecalculateNormals();
        //hexMesh.colors = colors.ToArray();
        meshCollider.sharedMesh = hexMesh;
    }

    void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        AddSquare(center + HexMetrics.squareCorners[0],
                  center + HexMetrics.squareCorners[1],
                  center + HexMetrics.squareCorners[2],
                  center + HexMetrics.squareCorners[3]
                );
    }

    void Triangulate_Old(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;

        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    AddTriangle(
                        center + HexMetrics.corners[4],
                        center,
                        center + HexMetrics.corners[3]
                    );
                    break;
                case 1:
                    AddTriangle(
                        center,
                        center + HexMetrics.corners[4],
                        center + HexMetrics.corners[5]);
                    break;
                case 2:
                    AddTriangle(
                        center + HexMetrics.corners[0],
                        center,
                        center + HexMetrics.corners[5]);
                    break;
                case 3:
                    AddTriangle(
                        center + HexMetrics.corners[1],
                        center,
                        center + HexMetrics.corners[0]);
                    break;
                case 4:
                    AddTriangle(
                        center + HexMetrics.corners[2],
                        center,
                        center + HexMetrics.corners[1]);
                    break;
                case 5:
                    AddTriangle(
                        center + HexMetrics.corners[3],
                        center,
                        center + HexMetrics.corners[2]);
                    break;
            }
            //AddTriangleColor(cell.color);
            AddTriangleTexture(i);
        }
    }

    void AddSquare(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        AddTriangle(v1, v2, v3);
        AddTriangle(v1, v3, v4);
        AddSquareTexture();
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    void AddSquareTexture()
    {
        int textureNo = UnityEngine.Random.Range(1, 4);
        float hOffset = textureNo * 0.25f;
        float vOffset = textureNo * 0.25f;

        uvs.Add(new Vector2(0.0f + hOffset, 0.0f));
        uvs.Add(new Vector2(0.0f + hOffset, 0.25f));
        uvs.Add(new Vector3(0.25f + hOffset, 0.25f));
        uvs.Add(new Vector2(0.0f + hOffset, 0.0f));
        uvs.Add(new Vector2(0.25f + hOffset, 0.25f));
        uvs.Add(new Vector2(0.25f + hOffset, 0.0f));

    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void AddTriangleTexture(int i)
    {
        int textureNo = 1;
        float hOffset = textureNo * 0.25f;
        float vOffset = textureNo * 0.125f;

        //uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f)); -- center
        //uvs.Add(new Vector2(0.0625f + hOffset, 0.125f)); -- up
        //uvs.Add(new Vector2(0.125f + hOffset, 0.075f)); -- up right
        //uvs.Add(new Vector2(0.125f + hOffset, 0.0375f)); -- down right
        //uvs.Add(new Vector2(0.0625f + hOffset, 0.0f)); -- down
        //uvs.Add(new Vector2(0.0f + hOffset, 0.0375f)); -- down left
        //uvs.Add(new Vector2(0.0f + hOffset, 0.075f)); -- up left
        switch (i)
        {
            case 0:
                uvs.Add(new Vector2(0.0f + hOffset, 0.0375f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0f));
                break;
            case 1:
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f));
                uvs.Add(new Vector2(0.0f + hOffset, 0.0375f));
                uvs.Add(new Vector2(0.0f + hOffset, 0.075f));
                break;
            case 2:
                uvs.Add(new Vector2(0.0625f + hOffset, 0.125f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f));
                uvs.Add(new Vector2(0.0f + hOffset, 0.075f));
                break;
            case 3:
                uvs.Add(new Vector2(0.125f + hOffset, 0.075f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.125f));
                break;
            case 4:
                uvs.Add(new Vector2(0.125f + hOffset, 0.0375f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f));
                uvs.Add(new Vector2(0.125f + hOffset, 0.075f));
                break;
            case 5:
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0f));
                uvs.Add(new Vector2(0.0625f + hOffset, 0.0625f));
                uvs.Add(new Vector2(0.125f + hOffset, 0.0375f));
                break;

        }
    }

}