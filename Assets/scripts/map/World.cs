using UnityEngine;
using EpPathFinding.cs;
using System.Collections.Generic;
using Sparrow.Collections;

namespace Sparrow.Map {
  public class World : MonoBehaviour {
    public int mapSize;
    public int chunkSize;
    public int tileSize;
    public int seed;
    public float amplitude;
    public float scale;
    public float floor;
    public Chunk chunkPrefab;
    public MultiKeyDictionary<int, int, Chunk> chunks;
    private BaseGrid pathing;
    JumpPointParam jpParam;

    void Awake() {
      chunks = new MultiKeyDictionary<int, int, Chunk>();
      pathing = new StaticGrid(mapSize * chunkSize, mapSize * chunkSize);
      jpParam = new JumpPointParam(pathing, true, DiagonalMovement.OnlyWhenNoObstacles);

      for (int i = 0; i < mapSize; i++) {
        for (int j = 0; j < mapSize; j++) {
          GetChunk(i, j);
        }
      }
    }

    public void Clear() {
      transform.SafeDestroyAllChildren();
      chunks.Clear();
    }

    public float Height(float x, float z) {
      float baseHeight = floor * amplitude;
      float height = NormalizedHeight(x, z);

      if (height < floor) {
        return floor * amplitude - baseHeight;
      }

      return (height * amplitude) - baseHeight;
    }

    public float NormalizedHeight(float x, float z) {
      float perlinX = (seed * 1000 + x) / scale;
      float perlinZ = (seed * 1000 + z) / scale;
      return Mathf.PerlinNoise(perlinX, perlinZ);
    }

    public void SetWalkable(int tileX, int tileZ) {
      pathing.SetWalkableAt(new GridPos(tileX, tileZ), true);
    }

    public void SetUnWalkable(int tileX, int tileZ) {
      pathing.SetWalkableAt(new GridPos(tileX, tileZ), false);
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 end) {
      return GetPath(
        GetTileCoordinate(start.x),
        GetTileCoordinate(start.z),
        GetTileCoordinate(end.x),
        GetTileCoordinate(end.z)
      );
    }

    public List<Vector3> GetPath(int startX, int startZ, int endX, int endZ) {
      GridPos start = new GridPos(startX, startZ);
      GridPos end = new GridPos(endX, endZ);
      jpParam.Reset(start, end);
      List<GridPos> path = JumpPointFinder.FindPath(jpParam);
      return PathToRealCoordinates(path);
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

    private List<Vector3> PathToRealCoordinates(List<GridPos> path) {
      List<Vector3> realPath = new List<Vector3>();

      path.ForEach((position) => {
        Debug.Log(position);
        realPath.Add(
          new Vector3(
            GetTileCoordinate(position.x) + .5f,
            0,
            GetTileCoordinate(position.y) + .5f
          )
        );
      });

      return realPath;
    }

    public int GetTileCoordinate(float coordinate) {
      return Mathf.FloorToInt(coordinate / tileSize);
    }
  }
}
