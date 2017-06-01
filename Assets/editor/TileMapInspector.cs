using UnityEditor;
using UnityEngine;

namespace Sparrow.Inspector {

  [CustomEditor(typeof(TileMap))]
  public class TileMapInspector : Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      if (GUILayout.Button("Generate")) {
        TileMap tileMap = (TileMap) target;
        tileMap.GenerateMesh();
      }
    }
  }
}
