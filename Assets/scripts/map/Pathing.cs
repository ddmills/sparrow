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
  }
}