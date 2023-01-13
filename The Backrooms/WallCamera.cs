using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCamera : MonoBehaviour
{
    public bool x;
    public float lockedPosition;

    public Transform player;
    
    void Update()
    {
        GetComponent<Camera>().enabled = (Vector3.Distance(transform.position, player.transform.position) <= 100);

        if (!x)
            transform.position = new Vector3(player.position.x, player.position.y, lockedPosition);
        else
            transform.position = new Vector3(lockedPosition, player.position.y, player.position.z);
    }
}
