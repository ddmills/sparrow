using UnityEngine;
using Sparrow.Collections;

namespace Sparrow.Map {
  [RequireComponent(typeof(MeshFilter))]
  [RequireComponent(typeof(MeshRenderer))]
  public class Chunk : MonoBehaviour {
    public float textureBleedEpsilon;
    public int x;
    public int z;
    public Mountain mountainPrefab;
    public MultiKeyDictionary<int, int, Mountain> mountains = new MultiKeyDictionary<int, int, Mountain>();

    public void Generate(World world, int chunkX, int chunkZ) {
      Mesh mesh = new Mesh();

      Vector3[] vertices = new Vector3[world.chunkSize * world.chunkSize * 4];

      int[] triangles = new int[world.chunkSize * world.chunkSize * 6];
      Vector2[] uv = new Vector2[vertices.Length];

      for (int i = 0, v = 0, t = 0; i < world.chunkSize; i++) {
        for (int j = 0; j < world.chunkSize; j++, v += 4, t += 6) {
          int tileX = chunkX * world.chunkSize + i;
          int tileZ = chunkZ * world.chunkSize + j;

          float tl = world.Height(tileX, tileZ + 1);
          float tr = world.Height(tileX + 1, tileZ + 1);
          float br = world.Height(tileX, tileZ);
          float bl = world.Height(tileX + 1, tileZ);

          vertices[v] = new Vector3(world.tileSize * i, tl, world.tileSize * (j + 1));
          vertices[v + 1] = new Vector3(world.tileSize * (i + 1), tr, world.tileSize * (j + 1));
          vertices[v + 2] = new Vector3(world.tileSize * (i + 1), bl, world.tileSize * j);
          vertices[v + 3] = new Vector3(world.tileSize * i, br, world.tileSize * j);

          triangles[t] = triangles[t + 3] = v;
          triangles[t + 1] = v + 1;
          triangles[t + 2] = triangles[t + 4] = v + 2;
          triangles[t + 5] = v + 3;

          float uvX = 0f;
          float uvZ = 0f;

          float height = world.NormalizedHeight(tileX, tileZ);

          if (height < .25f) {
            uvX = 0;
            uvZ = .5f;
          } else if (height < .4f) {
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


          if (height >= .5f) {
            Mountain mountain = (Mountain) Instantiate(mountainPrefab);
            mountain.transform.position = new Vector3(world.tileSize * tileX + .5f, world.floor, world.tileSize * tileZ + .5f);
            mountain.transform.SetParent(transform);
            // mountains.Add(tileX, tileZ, mountain);
          }
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
