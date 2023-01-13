using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public bool active = false;

    public GameObject door;

    void Entered()
    {
        if (!active)
        {
            active = true;
            door.SendMessage("boxActivated");
            GetComponent<AudioSource>().Stop();
        }
    }

    void Update()
    {
        if (active == true)
        {
            transform.position -= new Vector3(0, .25f * Time.deltaTime, 0);
            if (transform.position.y < -5)
                Destroy(gameObject);
        }

        transform.Rotate(new Vector3(15, 15, 15) * Time.deltaTime);
    }
}