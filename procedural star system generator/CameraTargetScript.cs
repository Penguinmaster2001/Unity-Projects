using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetScript : MonoBehaviour
{
    //public Camera camera;
    private Vector3 velocity;
    //private Vector3 randPos;
    private Vector3 targetPos = Vector3.zero;
    private GameObject targetPlanet;
    public GameObject[] planets;

    void Update()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");

        if (targetPlanet == null || Vector3.Distance(transform.position, targetPos) < targetPlanet.transform.lossyScale.x + 1)
        {
            targetPlanet = planets[Random.Range(0, planets.Length)];
        }

        targetPos = targetPlanet.transform.position;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, Vector3.Distance(transform.position, targetPos) / 1000);
        transform.LookAt(targetPlanet.transform.position);
    }
}
