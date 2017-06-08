using UnityEngine;
using Sparrow.Actor.Task;
using Sparrow.Map;

namespace Sparrow {
  public class Systems : MonoBehaviour {
    public static Systems instance;
    public Queue taskQueue;
    public World world;
    public Pathing pathing;

    protected Systems() {}

    void Awake() {
      if (Systems.instance == null) {
        Systems.instance = this;
      } else if (Systems.instance != this) {
        Destroy(gameObject);
      }

      DontDestroyOnLoad(gameObject);
      taskQueue = GetComponent<Queue>();
    }

    public Object Spawn(Object original, Vector3 position, Quaternion rotation) {
      return Instantiate(original, position, rotation);
    }
  }
}
