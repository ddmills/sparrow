using UnityEditor;
using UnityEngine;
using Sparrow.Map;

namespace Sparrow.Inspector {

  [CustomEditor(typeof(Grid))]
  public class TileMapInspector : Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      if (GUILayout.Button("Clear")) {
        Grid map = (Grid) target;
        map.Clear();
      }

      if (GUILayout.Button("Generate")) {
        Grid map = (Grid) target;
        map.Clear();

        for (int i = 0; i < 8; i++) {
          for (int j = 0; j < 8; j++) {
            map.GenerateChunk(i, j);
          }
        }
      }
    }
  }
}
