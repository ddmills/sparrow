using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileMap : MonoBehaviour {
  public int sizeX;
  public int sizeZ;

  void Start () {
    GenerateMesh();
  }

  public void GenerateMesh() {
    Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];
    int[] triangles = new int[sizeX * sizeZ * 6];

    for (int i = 0, z = 0; z <= sizeZ; z++) {
      for (int x = 0; x <= sizeX; x++, i++) {
        vertices[i] = new Vector3(x, Random.Range(-.2f, .2f), z);
      }
    }

    for (int i = 0, v = 0, y = 0; y < sizeZ; y++, v++) {
      for (int x = 0; x < sizeX; x++, i += 6, v++) {
        triangles[i] = v;
        triangles[i + 3] = triangles[i + 2] = v + 1;
        triangles[i + 4] = triangles[i + 1] = v + sizeX + 1;
        triangles[i + 5] = v + sizeX + 2;
      }
    }

    mesh.vertices = vertices;
    mesh.triangles = triangles;

    mesh.RecalculateNormals();

    GetComponent<MeshFilter>().mesh = mesh;
  }
}
