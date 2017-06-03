using UnityEngine;
using Sparrow.Collections;

namespace Sparrow.Map {
  public class Grid : MonoBehaviour {
    public int chunkSize;
    public int tileSize;
    public int seed;
    public float scaleFactor;
    public Chunk chunkPrefab;
    private MultiKeyDictionary<int, int, Chunk> chunks = new MultiKeyDictionary<int, int, Chunk>();

    public void Clear() {
      transform.SafeDestroyAllChildren();
      chunks.Clear();
    }

    public void GenerateChunk(int x, int z) {
      if (!chunks.ContainsKey(x, z)) {
        Chunk chunk = (Chunk) Instantiate(chunkPrefab);
        chunk.transform.SetParent(transform);
        chunk.transform.position = getChunkOffset(x, z);
        chunk.Generate(x, z, chunkSize, tileSize, seed, scaleFactor);
        chunks.Add(x, z, chunk);
      }
    }

    private Vector3 getChunkOffset(int x, int z) {
      float offset = (chunkSize * tileSize);
      return new Vector3(x * offset, 0, z * offset);
    }
  }
}
