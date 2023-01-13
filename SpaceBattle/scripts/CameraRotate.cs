using System.Collections;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float sensitivity;

    public Transform target;
    public float distance = 25.0f;
    public float xSpeed = 1;
    public float ySpeed = 10;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float distanceMin = 10f;
    public float distanceMax = 10f;
    public float smoothTime = 10f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    //float velocityX = 0.0f;
    //float velocityY = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        if(Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;

        //Vector3 r = new Vector3(Input.GetAxis("Pitch") * -sensitivity, 0, Input.GetAxis("Yaw") * -sensitivity);

        //transform.RotateAround(target.position, r, sensitivity * Time.deltaTime);

        transform.Rotate(Input.GetAxis("Pitch") * -sensitivity, Input.GetAxis("Yaw") * -sensitivity, Input.GetAxis("Roll") * -sensitivity);
        transform.position = target.position;

        //if (target)
        //{
        //    velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
        //    velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
        //    velocityX += xSpeed * Input.GetAxis("Yaw") * distance * sensitivity;
        //    velocityY += ySpeed * Input.GetAxis("Pitch") * sensitivity;

        //    rotationYAxis += velocityX;
        //    rotationXAxis -= velocityY;
        //    rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
        //    Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        //    Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
        //    Quaternion rotation = toRotation;

        //    //distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
        //    //RaycastHit hit;
        //    //if (Physics.Linecast(target.position, transform.position, out hit))
        //    //{
        //    //distance -= hit.distance;
        //    //}
        //    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        //    Vector3 position = rotation * negDistance + target.position;

        //    transform.rotation = rotation;
        //    transform.position = position;
        //    velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
        //    velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
        //}
    }
    //public static float ClampAngle(float angle, float min, float max)
    //{
    //    if (angle < -360F)
    //        angle += 360F;
    //    if (angle > 360F)
    //        angle -= 360F;
    //    return Mathf.Clamp(angle, min, max);
    //}
}