using UnityEngine;
// using King.Component;

namespace Sparrow.Actor.Task {
  public class Worker : MonoBehaviour {
    public Queue queue;
    public Task task;

    void Start() {
      queue = Systems.instance.taskQueue;
    }

    private void Update () {
      queue.AssignTask(this);
      if (task != null) {
        task.Process(this);
      }
    }

    public void BeginTask(Task task) {
      // Debug.Log(this.GetComponent<Selectable>().label + " is going to " + task.rootVerb);
      // Debug.Log(this.GetComponent<Selectable>().label + " is currently " + task.presentVerb);
    }

    public void StopTask() {
      // Debug.Log(this.GetComponent<Selectable>().label + " stopped " + task.presentVerb);
    }

    public void OnTaskCompleted(Task task) {
      // Debug.Log(this.GetComponent<Selectable>().label + " has " + task.pastVerb);
    }

    public void OnTaskCancelled(Task task) {
      // Debug.Log(this.GetComponent<Selectable>().label + " stopped " + task.presentVerb + " because it was cancelled");
    }

    public Task GetDefaultTask(Queue queue) {
      return new Wander(queue);
    }

    public float Weight(Task task) {
      return 1;
    }
  }
}
