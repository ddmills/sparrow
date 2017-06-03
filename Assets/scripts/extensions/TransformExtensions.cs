using UnityEngine;
using System.Linq;

namespace Sparrow {
  public static class TransformExtensions {
    public static GameObject[] Children(this Transform parent) {
      return parent
        .GetComponentsInChildren<Transform>()
        .Where(t => !t.Equals(parent))
        .Select(t => t.gameObject)
        .ToArray();
    }

    public static Transform DestroyAllChildren(this Transform transform) {
      foreach (GameObject child in transform.Children()) {
        Object.Destroy(child);
      }
      return transform;
    }

    public static Transform SafeDestroyAllChildren(this Transform transform) {
      foreach (GameObject child in transform.Children()) {
        Utility.SafeDestroy(child);
      }
      return transform;
    }
  }
}
