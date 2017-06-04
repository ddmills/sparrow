using UnityEngine;

namespace Sparrow.UI {
  public class RTSCamera : MonoBehaviour {

    public float rotationSpeed = .15f;
    public bool invertRotation = true;
    public float zoomSpeed = 2.5f;
    public float panSpeed = 4f;
    public bool invertPan = false;

    public float ROTATION_CLAMP_MIN = 25f;
    public float ROTATION_CLAMP_MAX = 70f;
    public float ZOOM_CLAMP_MIN = -40f;
    public float ZOOM_CLAMP_MAX = -8f;
    private float previousMouseX = 0f;
    private float previousMouseY = 0f;

    public GameObject cam;

    void Update () {
      if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftControl)) {
        Rotate();
      } else if (Input.GetMouseButton(2)) {
        Pan();
      }

      if (Input.GetAxis("Mouse ScrollWheel") < 0) {
        ZoomOut();
      }

      if (Input.GetAxis("Mouse ScrollWheel") > 0) {
        ZoomIn();
      }

      previousMouseX = Input.mousePosition.x;
      previousMouseY = Input.mousePosition.y;
    }

    private void ZoomOut() {
      float z = Mathf.Clamp(
        cam.transform.localPosition.z - zoomSpeed,
        ZOOM_CLAMP_MIN,
        ZOOM_CLAMP_MAX
      );

      cam.transform.localPosition = new Vector3(0, 0, z);
    }

    private void ZoomIn() {
      float z = Mathf.Clamp(
        cam.transform.localPosition.z + zoomSpeed,
        ZOOM_CLAMP_MIN,
        ZOOM_CLAMP_MAX
      );

      cam.transform.localPosition = new Vector3(0, 0, z);
    }

    private void Rotate() {
      float mouseOffsetX = Input.mousePosition.x - previousMouseX;
      float mouseOffsetY = Input.mousePosition.y - previousMouseY;

      mouseOffsetX *= invertRotation ? -1 : 1;
      mouseOffsetY *= invertRotation ? 1 : -1;

      float rotationX = transform.localEulerAngles.x - mouseOffsetY * rotationSpeed;
      float rotationY = transform.eulerAngles.y - mouseOffsetX * rotationSpeed;

      transform.localEulerAngles = new Vector3(
        Mathf.Clamp(rotationX, ROTATION_CLAMP_MIN, ROTATION_CLAMP_MAX),
        transform.localEulerAngles.y,
        transform.localEulerAngles.z
      );

      transform.eulerAngles = new Vector3(
        transform.eulerAngles.x,
        rotationY,
        transform.eulerAngles.z
      );
    }

    private void Pan() {
      float mouseOffsetX = Input.mousePosition.x - previousMouseX;
      float mouseOffsetY = Input.mousePosition.y - previousMouseY;

      mouseOffsetX *= invertPan ? -1 : 1;
      mouseOffsetY *= invertPan ? -1 : 1;

      Vector3 direction = new Vector3(transform.forward.x, 0, transform.forward.z);

      direction.Normalize();

      Vector3 forward = direction * Time.deltaTime * panSpeed * mouseOffsetY;
      Vector3 right = Vector3.right * Time.deltaTime * panSpeed * mouseOffsetX;

      transform.Translate(right, Space.Self);
      transform.Translate(forward, Space.World);

      previousMouseX = Input.mousePosition.x;
      previousMouseY = Input.mousePosition.y;
    }
  }
}
