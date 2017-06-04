using UnityEngine;

namespace Sparrow.Map {
  [RequireComponent(typeof(MeshFilter))]
  [RequireComponent(typeof(MeshRenderer))]
  public class Chunk : MonoBehaviour {
    public float textureBleedEpsilon;
    public int x;
    public int z;

    public void Generate(int chunkX, int chunkZ, int chunkSize, int tileSize, int seed, float scaleFactor, float amplitude) {
      Mesh mesh = new Mesh();

      Vector3[] vertices = new Vector3[chunkSize * chunkSize * 4];

      int[] triangles = new int[chunkSize * chunkSize * 6];
      Vector2[] uv = new Vector2[vertices.Length];

      for (int i = 0, v = 0, t = 0; i < chunkSize; i++) {
        for (int j = 0; j < chunkSize; j++, v += 4, t += 6) {
          float tileX = chunkX * chunkSize + i;
          float tileZ = chunkZ * chunkSize + j;

          float tl = Mathf.PerlinNoise((seed * 1000 + tileX) / scaleFactor, (seed * 1000 + tileZ + 1) / scaleFactor) * amplitude;
          float tr = Mathf.PerlinNoise((seed * 1000 + tileX + 1) / scaleFactor, (seed * 1000 + tileZ + 1) / scaleFactor) * amplitude;
          float br = Mathf.PerlinNoise((seed * 1000 + tileX) / scaleFactor, (seed * 1000 + tileZ) / scaleFactor) * amplitude;
          float bl = Mathf.PerlinNoise((seed * 1000 + tileX + 1) / scaleFactor, (seed * 1000 + tileZ) / scaleFactor) * amplitude;

          vertices[v] = new Vector3(tileSize * i, tl, tileSize * (j + 1));
          vertices[v + 1] = new Vector3(tileSize * (i + 1), tr, tileSize * (j + 1));
          vertices[v + 2] = new Vector3(tileSize * (i + 1), bl, tileSize * j);
          vertices[v + 3] = new Vector3(tileSize * i, br, tileSize * j);

          triangles[t] = triangles[t + 3] = v;
          triangles[t + 1] = v + 1;
          triangles[t + 2] = triangles[t + 4] = v + 2;
          triangles[t + 5] = v + 3;

          float uvX = 0f;
          float uvZ = 0f;

          float height = Mathf.PerlinNoise((seed * 1000 + tileX) / scaleFactor, (seed * 1000 + tileZ) / scaleFactor);

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
