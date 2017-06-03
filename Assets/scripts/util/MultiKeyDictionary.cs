using System.Collections.Generic;
using System.Linq;

namespace Sparrow.Utility {
  public class MultiKeyDictionary<K1, K2, V> : Dictionary<K1, Dictionary<K2, V>> {

    public V this[K1 key1, K2 key2] {
      get {
        return base[key1][key2];
      }
      set {
        if (!ContainsKey(key1)) {
          this[key1] = new Dictionary<K2, V>();
        }
        this[key1][key2] = value;
      }
    }

    public void Add(K1 key1, K2 key2, V value) {
      if (!ContainsKey(key1)) {
       this[key1] = new Dictionary<K2, V>();
      }
      this[key1][key2] = value;
    }

    public V Remove(K1 key1, K2 key2) {
      if (ContainsKey(key1, key2)) {
        V v = base[key1][key2];
        base[key1].Remove(key2);
        if (base[key1].Count <= 0) {
          base.Remove(key1);
        }
        return v;
      }

      return default(V);
    }

    public bool ContainsKey(K1 key1, K2 key2) {
      return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
    }

    public new IEnumerable<V> Values {
      get {
        return
          from d in base.Values
          from baseKey in d.Keys
          select d[baseKey];
      }
    }
  }
}
