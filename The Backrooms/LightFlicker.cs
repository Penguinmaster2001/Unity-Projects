using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private bool on;
    public GameObject player;

    void Update()
    {
        //on = player.GetComponent<PlayerControl>().on;

        if (Vector3.Distance(player.transform.position, transform.position) >= 100)
        {
                GetComponent<Light>().enabled = false;
                GetComponent<AudioSource>().enabled = false;
                return;
        }

        on = true;

        if (Random.Range(0, 5) == 0)
            on = false;

        GetComponent<Light>().enabled = on;
        //GetComponent<AudioSource>().enabled = on;
    }
}