using UnityEngine;
using Sparrow.Utility;

namespace Sparrow.Map {
  [ExecuteInEditMode]
  public class Grid : MonoBehaviour {
    public int chunkSize;
    public int tileSize;
    public Chunk chunkPrefab;
    private MultiKeyDictionary<int, int, Chunk> chunks = new MultiKeyDictionary<int, int, Chunk>();

    public void Clear() {
      foreach (Chunk chunk in chunks.Values) {
        if (Application.isEditor) {
          Object.DestroyImmediate(chunk.gameObject);
        } else {
          Object.Destroy(chunk.gameObject);
        }
      }
      chunks.Clear();
    }

    public void GenerateChunk(int x, int z) {
      if (!chunks.ContainsKey(x, z)) {
        Chunk chunk = (Chunk) Instantiate(chunkPrefab);
        chunk.transform.SetParent(transform);
        chunk.transform.position = getChunkOffset(x, z);
        chunk.Generate(chunkSize, tileSize);
        chunk.x = x;
        chunk.z = z;
        chunks.Add(x, z, chunk);
      }
    }

    private Vector3 getChunkOffset(int x, int z) {
      float offset = (chunkSize * tileSize);
      return new Vector3(x * offset, 0, z * offset);
    }
  }
}
