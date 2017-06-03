using UnityEditor;
using UnityEngine;
using Sparrow.Map;

namespace Sparrow.Inspector {

  [CustomEditor(typeof(Grid))]
  public class TileMapInspector : Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      if (GUILayout.Button("Generate")) {
        Grid map = (Grid) target;
        map.Clear();
        map.GenerateChunk(0, 0);
        map.GenerateChunk(1, 0);
        map.GenerateChunk(1, 1);
        map.GenerateChunk(0, 1);
      }

      if (GUILayout.Button("Clear")) {
        Grid map = (Grid) target;
        map.Clear();
      }
    }
  }
}
