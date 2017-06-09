using System.Collections;
using UnityEngine;
using Sparrow.Component;

namespace Sparrow.Actor.Behavior {
  public class Wanderable : MonoBehaviour {
    public MoveTo moveTo;
    public int wanderRadius;
    public float wanderDelayMin;
    public float wanderDelayMax;
    private IEnumerator coroutine;

    public void Wander() {
      coroutine = GoToRandom();
      StartCoroutine(coroutine);
    }

    public void Stop() {
      StopCoroutine(coroutine);
    }

    private IEnumerator GoToRandom() {
      while(true) {
        Vector3 target = Systems.instance.pathing.walkableWithinRadius(transform.position, wanderRadius);
        moveTo.SetGoal(target);
        yield return new WaitForSeconds(Random.Range(wanderDelayMin, wanderDelayMax));
      }
    }
  }
}
