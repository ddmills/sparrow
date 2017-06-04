using UnityEngine;

namespace Sparrow.Map {
  [RequireComponent(typeof(Grid))]
  public class Query : MonoBehaviour {
    private Grid grid;

    void Start() {
      grid = GetComponent<Grid>();
    }

    public Chunk ChunkForTile(int tileX, int tileZ) {
      int chunkX = tileX % grid.tileSize;
      int chunkZ = tileZ % grid.tileSize;
      return grid.GetChunk(chunkX, chunkZ);
    }

    public float Height(int tileX, int tileZ) {
      float perlinX = (grid.seed * 1000 + tileX) / grid.scale;
      float perlinZ = (grid.seed * 1000 + tileZ) / grid.scale;
      return Mathf.PerlinNoise(perlinX, perlinZ);
    }
  }
}
