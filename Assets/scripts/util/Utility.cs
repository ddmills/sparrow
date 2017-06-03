using UnityEngine;

namespace Sparrow {
  public static class Utility {
    public static void SafeDestroy(Object gameObject) {
      if (Application.isEditor) {
        Object.DestroyImmediate(gameObject);
      } else {
        Object.Destroy(gameObject);
      }
    }
  }
}
