using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileMap : MonoBehaviour {
  public int chunkSize;

  void Start () {
    GenerateMesh();
  }

  public void GenerateMesh() {
    Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[chunkSize * chunkSize * 4];

    int[] triangles = new int[chunkSize * chunkSize * 6];
    Vector2[] uv = new Vector2[vertices.Length];

    for (int i = 0, v = 0, t = 0; i < chunkSize; i++) {
      for (int j = 0; j < chunkSize; j++, v += 4, t += 6) {
        vertices[v] = new Vector3(i, 0, j + 1);
        vertices[v + 1] = new Vector3(i + 1, 0, j + 1);
        vertices[v + 2] = new Vector3(i + 1, 0, j);
        vertices[v + 3] = new Vector3(i, 0, j);

        triangles[t] = triangles[t + 3] = v;
        triangles[t + 1] = v + 1;
        triangles[t + 2] = triangles[t + 4] = v + 2;
        triangles[t + 5] = v + 3;

        float col = Random.Range(0, 2) * .5f;
        float row = Random.Range(0, 2) * .5f;

        uv[v] = new Vector2(col + 0f, row + .5f);
        uv[v + 1] = new Vector2(col + .5f, row + .5f);
        uv[v + 2] = new Vector2(col + .5f, row + 0f);
        uv[v + 3] = new Vector2(col + 0f, row + 0f);
      }
    }


    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.triangles = triangles;

    mesh.RecalculateNormals();

    GetComponent<MeshFilter>().mesh = mesh;
  }
}
