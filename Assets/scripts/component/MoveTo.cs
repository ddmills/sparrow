using UnityEngine;
using EpPathFinding.cs;
using System.Collections.Generic;
using Sparrow.Map;

namespace Sparrow.Component {
  public class MoveTo : MonoBehaviour {
    public World world;
    public float speed;
    public float epsilon;
    public Transform target;
    private int currentNode;
    private Vector3 currentTarget;
    List<Vector3> path;
    public GameObject pathMarker;

    void Start () {
      currentNode = -1;
      Debug.Log(transform.position);
      Debug.Log(target.transform.position);

      path = world.GetPath(transform.position, target.transform.position);

      if (path.Count > 0) {
        currentNode = 0;
      }

      path.ForEach((pos) => {
        // Debug.Log(pos.x + "+" + pos.z);
        Instantiate(pathMarker, pos, Quaternion.identity);
      });
    }

    void Update() {
      if (currentNode >= 0) {
        Vector3 currentTarget = path[currentNode];
        if (Vector3.Distance(transform.position, currentTarget) > epsilon) {
          transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        } else {
          currentNode++;
          if (currentNode >= path.Count) {
            currentNode = -1;
          }
        }
      }
    }
  }
}
