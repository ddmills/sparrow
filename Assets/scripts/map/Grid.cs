using UnityEngine;
using Sparrow.Collections;

namespace Sparrow.Map {
  public class Grid : MonoBehaviour {
    public int chunkSize;
    public int tileSize;
    public int seed;
    public float amplitude;
    public float scaleFactor;
    public Chunk chunkPrefab;
    public MultiKeyDictionary<int, int, Chunk> chunks = new MultiKeyDictionary<int, int, Chunk>();

    public void Clear() {
      transform.SafeDestroyAllChildren();
      chunks.Clear();
    }

    public float Height(float x, float y) {
      return Mathf.PerlinNoise((seed * 1000 + x) / scaleFactor, (seed * 1000 + y) / scaleFactor);
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
