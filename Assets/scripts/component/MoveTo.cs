using UnityEngine;
using System.Collections.Generic;
using Sparrow.Map;

namespace Sparrow.Component {
  public class MoveTo : MonoBehaviour {
    public World world;
    public float speed;
    public float rotSpeed;
    private float epsilon;
    public Transform goal;
    private int step;
    private List<Vector3> path;
    public GameObject pathMarker;
    public bool reachedGoal;

    void Start() {
      SetGoal(goal.position);
    }

    public bool SetGoal(Vector3 goal, float epsilon = .02f) {
      this.epsilon = epsilon;
      step = -1;
      path = world.GetPath(transform.position, goal);

      if (path.Count > 0) {
        reachedGoal = false;
        step = 0;
        return true;
      }
      return false;
    }

    void Update() {
      if (step >= 0 && !reachedGoal) {
        Vector3 waypoint = path[step];
        if (transform.Distance(waypoint) > epsilon) {
          transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
          transform.forward = Vector3.RotateTowards(transform.forward, waypoint - transform.position, rotSpeed * Time.deltaTime, .0f);
        } else {
          step++;
          if (step >= path.Count) {
            reachedGoal = true;
            step = -1;
          }
        }
      }
    }
  }
}
