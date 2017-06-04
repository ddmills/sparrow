using UnityEngine;

namespace Sparrow.Map {
  [RequireComponent(typeof(MeshFilter))]
  [RequireComponent(typeof(MeshRenderer))]
  public class Chunk : MonoBehaviour {
    public float textureBleedEpsilon;
    public int x;
    public int z;

    public void Generate(Grid grid, int chunkX, int chunkZ) {
      Mesh mesh = new Mesh();

      Vector3[] vertices = new Vector3[grid.chunkSize * grid.chunkSize * 4];

      int[] triangles = new int[grid.chunkSize * grid.chunkSize * 6];
      Vector2[] uv = new Vector2[vertices.Length];

      for (int i = 0, v = 0, t = 0; i < grid.chunkSize; i++) {
        for (int j = 0; j < grid.chunkSize; j++, v += 4, t += 6) {
          float tileX = chunkX * grid.chunkSize + i;
          float tileZ = chunkZ * grid.chunkSize + j;

          float tl = grid.Height(tileX, tileZ + 1);
          float tr = grid.Height(tileX + 1, tileZ + 1);
          float br = grid.Height(tileX, tileZ);
          float bl = grid.Height(tileX + 1, tileZ);

          vertices[v] = new Vector3(grid.tileSize * i, tl, grid.tileSize * (j + 1));
          vertices[v + 1] = new Vector3(grid.tileSize * (i + 1), tr, grid.tileSize * (j + 1));
          vertices[v + 2] = new Vector3(grid.tileSize * (i + 1), bl, grid.tileSize * j);
          vertices[v + 3] = new Vector3(grid.tileSize * i, br, grid.tileSize * j);

          triangles[t] = triangles[t + 3] = v;
          triangles[t + 1] = v + 1;
          triangles[t + 2] = triangles[t + 4] = v + 2;
          triangles[t + 5] = v + 3;

          float uvX = 0f;
          float uvZ = 0f;

          float height = grid.Height(tileX, tileZ);

          if (height < .25f) {
            uvX = 0;
            uvZ = .5f;
          } else if (height < .5f) {
            uvX = (float) .5f;
            uvZ = (float) .5f;
          } else if (height < .75f) {
            uvX = (float) 0f;
            uvZ = (float) 0f;
          } else {
            uvX = (float) .5f;
            uvZ = (float) 0;
          }

          uv[v] = new Vector2(uvX + 0f + textureBleedEpsilon, uvZ + .5f - textureBleedEpsilon);
          uv[v + 1] = new Vector2(uvX + .5f - textureBleedEpsilon, uvZ + .5f - textureBleedEpsilon);
          uv[v + 2] = new Vector2(uvX + .5f - textureBleedEpsilon, uvZ + 0f + textureBleedEpsilon);
          uv[v + 3] = new Vector2(uvX + 0f + textureBleedEpsilon, uvZ + 0f + textureBleedEpsilon);
        }
      }

      mesh.vertices = vertices;
      mesh.uv = uv;
      mesh.triangles = triangles;

      mesh.RecalculateNormals();

      GetComponent<MeshFilter>().mesh = mesh;
    }
  }
}
