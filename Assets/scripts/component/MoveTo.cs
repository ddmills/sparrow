using UnityEngine;
using EpPathFinding.cs;
using System.Collections.Generic;
using Sparrow.Map;

namespace Sparrow.Component {
  public class MoveTo : MonoBehaviour {
    public World world;
    public float speed;
    public float rotSpeed;
    public float epsilon;
    public Transform target;
    private int step;
    List<Vector3> path;
    public GameObject pathMarker;

    void Start () {
      step = -1;

      path = world.GetPath(transform.position, target.transform.position);

      if (path.Count > 0) {
        step = 0;
      }

      path.ForEach((pos) => {
        Instantiate(pathMarker, pos, Quaternion.identity);
      });
    }

    void Update() {
      if (step >= 0) {
        Vector3 waypoint = path[step];
        if (transform.Distance(waypoint) > epsilon) {
          transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
          transform.forward = Vector3.RotateTowards(transform.forward, waypoint - transform.position, rotSpeed * Time.deltaTime, .0f);
        } else {
          step++;
          if (step >= path.Count) {
            step = -1;
          }
        }
      }
    }
  }
}
