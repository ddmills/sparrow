using UnityEditor;
using UnityEngine;
using Sparrow.Map;

namespace Sparrow.Inspector {

  [CustomEditor(typeof(World))]
  public class WorldInspector : Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      if (GUILayout.Button("Clear")) {
        World map = (World) target;
        map.Clear();
      }

      if (GUILayout.Button("Generate")) {
        World map = (World) target;
        map.Clear();

        for (int i = 0; i < 8; i++) {
          for (int j = 0; j < 8; j++) {
            map.GetChunk(i, j);
          }
        }
      }
    }
  }
}
