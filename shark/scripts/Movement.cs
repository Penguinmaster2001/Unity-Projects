using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float points = 0;
    public float mouthsize = 2;

    [Header("Camera")]
    public float cameraBackDistance;
    public float cameraUpDistance;
    public float cameraSideDistance;
    public float cameraFOV;
    public float rotationSpeed;
    public Camera mainCamera;
    public GameObject cameraContainer;

    [Space]
    [Header("Shark")]
    public GameObject shark;
    public ParticleSystem bloodTrail;

    [Space]
    [Header("Movement")]
    private Rigidbody rb;
    public Vector3 acceleration;
    public float speed;
    public float slowestSpeed;
    public float dragMultiplyer;

    [Space]
    public KeyCode forwardKey;
    public KeyCode leftwardKey;
    public KeyCode rightwardKey;
    public KeyCode backwardKey;
    public KeyCode upwardKey;
    public KeyCode downwardKey;

    [Space]
    [Header("Boost")]
    //boost
    public float boost;
    public float boostCharge;
    public float boostChargeRate;

    [Space]
    public KeyCode boostKey;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        mainCamera.transform.localPosition = new Vector3(cameraSideDistance, cameraUpDistance, cameraBackDistance);;
        mainCamera.fieldOfView = cameraFOV;

        Move();

        Rotate();
    }

    public void Eat(float fishSize)
    {
        points += fishSize;
        transform.localScale += Vector3.one * fishSize * 0.01f;
        mouthsize += fishSize * 0.01f;
        speed += Mathf.Pow(fishSize, 1/3) * 0.01f;
        cameraBackDistance -= Mathf.Pow(fishSize, 3) * 0.01f;
        bloodTrail.Play();
    }

    void GetInput()
    {
        if(transform.position.y < UniversalVariables.waterHeight)
        {
            if(Input.GetKey(forwardKey))
            {
                acceleration.z += speed;
            }

            if(Input.GetKey(backwardKey))
            {
                acceleration.z -= speed;
            }

            if(Input.GetKey(rightwardKey))
            {
                acceleration.x += speed;
            }

            if(Input.GetKey(leftwardKey))
            {
                acceleration.x -= speed;
            }

            if(Input.GetKey(upwardKey))
            {
                acceleration.y += speed;
            }

            if(Input.GetKey(downwardKey))
            {
                acceleration.y -= speed;
            }


            if(Input.GetKeyDown(boostKey) && boostCharge >= 100)
            {
                acceleration.z += boost;
                boostCharge = 0;
            }
        }
    }

    void Move()
    {
        GetInput();

        if(transform.position.y > UniversalVariables.waterHeight)
        {
            rb.useGravity = true;
            rb.drag = 0.0f;
            //acceleration += UniversalVariables.gravity * Time.fixedDeltaTime;
            //rb.AddForce(UniversalVariables.gravity, ForceMode.Acceleration);
            //transform.position += velocity * Time.fixedDeltaTime;
        }
        else
        {
            rb.useGravity = false;
            rb.drag = 4.0f;

            rb.AddForce(acceleration.z * transform.forward, ForceMode.Force);
            rb.AddForce(acceleration.x * transform.right, ForceMode.Force);
            rb.AddForce(acceleration.y * transform.up, ForceMode.Force);

            //transform.position += acceleration.z * transform.forward * Time.fixedDeltaTime; //front and back
            //transform.position += acceleration.x * transform.right * Time.fixedDeltaTime; //left and right
            //transform.position += acceleration.y * transform.up * Time.fixedDeltaTime; //up and down

            //acceleration *= dragMultiplyer;
            if(acceleration.magnitude < slowestSpeed)
            {
                acceleration = Vector3.zero;
            }
        }

        if (boostCharge < 100)
        {
            boostCharge += boostChargeRate * Time.fixedDeltaTime;
        }

        acceleration = Vector3.zero;
    }

    void Rotate()
    {
        float multiplyer = Quaternion.Angle(transform.rotation, cameraContainer.transform.rotation) * 0.02f;
        rb.rotation = (Quaternion.RotateTowards(transform.rotation, cameraContainer.transform.rotation, rotationSpeed * multiplyer));
    }
}