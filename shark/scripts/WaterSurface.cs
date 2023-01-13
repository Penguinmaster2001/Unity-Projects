using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    public Transform shark;
    public float maxRenderDepth;

    private MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        transform.position = new Vector3(shark.position.x, UniversalVariables.waterHeight, shark.position.z);

        if(shark.position.y < maxRenderDepth)
        {
            mr.forceRenderingOff = true;
        }
        else
        {
            mr.forceRenderingOff = false;
        }
    }
}