using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mouseSensitivity;
    public float rotationDrag;
    public bool invertMouse;
    public bool autoLockCursor;
    public float zoom;

    private Vector2 momentum;

    public Transform player;

    void Awake()
    {
        Cursor.lockState = (autoLockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        transform.position = player.position;

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            zoom -= Mathf.Pow(Input.mouseScrollDelta.y * 2, 4) * Mathf.Sign(Input.mouseScrollDelta.y);
            zoom = Mathf.Clamp(zoom, 1, 100);

            momentum += new Vector2(Input.GetAxis("Mouse Y") * mouseSensitivity * (zoom / 50) * ((invertMouse) ? 1 : -1), Input.GetAxis("Mouse X") * mouseSensitivity * (zoom / 50) * ((invertMouse) ? -1 : 1));

            gameObject.transform.Rotate(momentum);
            //gameObject.transform.localEulerAngles = new Vector3(this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, 0);
            momentum *= rotationDrag;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if(Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}