using UnityEngine;
using Sparrow.Collections;

namespace Sparrow.Map {
  [ExecuteInEditMode]
  public class World : MonoBehaviour {
    public int chunkSize;
    public int tileSize;
    public int seed;
    public float amplitude;
    public float scale;
    public float floor;
    public Chunk chunkPrefab;
    public MultiKeyDictionary<int, int, Chunk> chunks = new MultiKeyDictionary<int, int, Chunk>();

    public void Clear() {
      transform.SafeDestroyAllChildren();
      chunks.Clear();
    }

    public float Height(float x, float z) {
      float baseHeight = floor * amplitude;
      float height = NormalizedHeight(x, z);

      if (height < floor) {
        return floor * amplitude - baseHeight;// * height;
      }

      return (height * amplitude) - baseHeight;
    }

    public float NormalizedHeight(float x, float z) {
      float perlinX = (seed * 1000 + x) / scale;
      float perlinZ = (seed * 1000 + z) / scale;
      return Mathf.PerlinNoise(perlinX, perlinZ);
    }

    public Chunk GetChunk(int chunkX, int chunkZ) {
      if (chunks.ContainsKey(chunkX, chunkZ)) {
        return chunks[chunkX][chunkZ];
      }

      Chunk chunk = (Chunk) Instantiate(chunkPrefab);
      chunk.transform.SetParent(transform);
      chunk.transform.position = getChunkOffset(chunkX, chunkZ);
      chunk.Generate(this, chunkX, chunkZ);
      chunks.Add(chunkX, chunkZ, chunk);
      return chunk;
    }

    private Vector3 getChunkOffset(int x, int z) {
      float offset = (chunkSize * tileSize);
      return new Vector3(x * offset, 0, z * offset);
    }
  }
}
