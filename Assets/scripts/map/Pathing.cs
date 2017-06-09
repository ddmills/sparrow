using UnityEngine;
using EpPathFinding.cs;
using System.Collections.Generic;

namespace Sparrow.Map {
  public class Pathing : MonoBehaviour {
    private BaseGrid grid;
    private JumpPointParam jpParam;

    void Awake() {
      grid = new StaticGrid(Systems.instance.world.mapSize * Systems.instance.world.chunkSize, Systems.instance.world.mapSize * Systems.instance.world.chunkSize);
      jpParam = new JumpPointParam(grid, true, DiagonalMovement.OnlyWhenNoObstacles);
    }

    public void SetWalkable(int tileX, int tileZ) {
      grid.SetWalkableAt(new GridPos(tileX, tileZ), true);
    }

    public void SetUnWalkable(int tileX, int tileZ) {
      grid.SetWalkableAt(new GridPos(tileX, tileZ), false);
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 end) {
      return GetPath(
        Systems.instance.world.GetTileCoordinate(start.x),
        Systems.instance.world.GetTileCoordinate(start.z),
        Systems.instance.world.GetTileCoordinate(end.x),
        Systems.instance.world.GetTileCoordinate(end.z)
      );
    }

    public List<Vector3> GetPath(int startX, int startZ, int endX, int endZ) {
      GridPos start = new GridPos(startX, startZ);
      GridPos end = new GridPos(endX, endZ);
      jpParam.Reset(start, end);
      List<GridPos> path = JumpPointFinder.FindPath(jpParam);
      return PathToRealCoordinates(path);
    }

    private List<Vector3> PathToRealCoordinates(List<GridPos> path) {
      List<Vector3> realPath = new List<Vector3>();

      path.ForEach((position) => {
        realPath.Add(
          new Vector3(
            Systems.instance.world.GetTileCoordinate(position.x) + (float) Systems.instance.world.tileSize / 2,
            0,
            Systems.instance.world.GetTileCoordinate(position.y) + (float) Systems.instance.world.tileSize / 2
          )
        );
      });

      return realPath;
    }

    private bool isOutOfBounds(int x, int y) {
      return x >= grid.width
        || y >= grid.height
        || x < 0
        || y < 0;
    }

    public Vector3 walkableWithinRadius(Vector3 position, int radius) {
      int x = (int) (position.x / Systems.instance.world.tileSize);
      int y = (int) (position.z / Systems.instance.world.tileSize);
      return walkableWithinRadius(x, y, radius);
    }

    public Vector3 walkableWithinRadius(int x, int y, int radius) {
      List<Node> nodes = new List<Node>();
      float radiusSquared = radius * radius;

      for (int relX = -radius; relX <= radius; relX++) {
        for (int relY = -radius; relY <= radius; relY++) {

          int tileX = x + relX;
          int tileY = y + relY;

          if (isOutOfBounds(tileX, tileY)) {
            continue;
          }

          Node node = grid.GetNodeAt(tileX, tileY);

          if (node.walkable) {
            float dx = node.x - x;
            float dy = node.y - y;

            float distSquared = dx * dx + dy * dy;

            if (distSquared <= radiusSquared) {
              nodes.Add(node);
            }
          }
        }
      }

      if (nodes.Empty()) {
        return new Vector3(x * Systems.instance.world.tileSize, 0, y * Systems.instance.world.tileSize);
      }

      int randIndex = Random.Range(0, nodes.Count);
      Node randNode = nodes[randIndex];
      return new Vector3(randNode.x * Systems.instance.world.tileSize, 0, randNode.y * Systems.instance.world.tileSize);
    }
  }
}