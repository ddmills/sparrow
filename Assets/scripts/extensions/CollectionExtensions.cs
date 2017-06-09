using System.Collections.Generic;

namespace Sparrow {
  public static class CollectionExtensions {
    public static bool Empty<T>(this ICollection<T> collection) {
      return collection.Count <= 0;
    }
  }
}