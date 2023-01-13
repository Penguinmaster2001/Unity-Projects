using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineControl : MonoBehaviour
{
    public GameObject[] engines;
    public GameObject   rudderObj;
    public Vector3[]    relativePos;
    public int          speed;
    public int          rudderAngle;
    private ConstantForce2D rudder;
    private Rigidbody2D rb;

    void Start()
    {
        rudder = rudderObj.GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>();
        int x = 0;
        foreach (GameObject engine in engines) //creates a new joint for each engine
        {
            FixedJoint2D newJoint =     gameObject.AddComponent<FixedJoint2D>();
            newJoint.connectedBody =    engine.GetComponent<Rigidbody2D>();
            newJoint.connectedAnchor =  relativePos[x];
            x++;
        }
    }

    void Update()
    {
        //inputs
        if (Input.GetKey(KeyCode.W))
            speed+= 10;
        if (Input.GetKey(KeyCode.S))
            speed-= 10;
        if (Input.GetKeyDown(KeyCode.A))
            rudderAngle++;
        if (Input.GetKeyDown(KeyCode.D))
            rudderAngle--;

    }

    void FixedUpdate()
    {
        int x = 0;
        foreach (GameObject engine in engines) //applys inputs
        {
            engine.GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, speed);
            x++;
        }

        //applys rudder force
        rudder.relativeForce = new Vector2(rudderAngle*(rb.velocity.magnitude), 0);
    }
}
