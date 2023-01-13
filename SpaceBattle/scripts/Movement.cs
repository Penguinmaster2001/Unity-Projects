using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float points = 0;
    public float mouthsize = 2;
    public int bolts;
    public bool shootThisFrame = false;
    public Material boltMat;
    public float rate;
    private float rateLeft;
    public float zoom;

    [Header("Camera")]
    public float cameraBackDistance;
    public float cameraUpDistance;
    public float cameraSideDistance;
    public float cameraFOV;
    public float rotationSpeed;
    public Camera mainCamera;
    public Camera sightCamera;
    public GameObject cameraContainer;

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


    public BoltRenderer boltRenderer;

    void Start()
    {
        boltRenderer = new BoltRenderer
        {
            boltMat = this.boltMat
        };

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        zoom -= Mathf.Pow(Input.mouseScrollDelta.y * 2, 4) * Mathf.Sign(Input.mouseScrollDelta.y);
        zoom = Mathf.Clamp(zoom, 0.1f, 100);

        mainCamera.transform.localPosition = new Vector3(cameraSideDistance, cameraUpDistance, cameraBackDistance);
        mainCamera.fieldOfView = cameraFOV + zoom;

        sightCamera.transform.localPosition = new Vector3(cameraSideDistance, cameraUpDistance, cameraBackDistance);
        sightCamera.fieldOfView = cameraFOV + zoom;
    }

    void FixedUpdate()
    {
        GetInput();

        Rotate();

        Move();

        bolts = boltRenderer.bolts.Count;

        boltRenderer.UpdateBolts();
    }

    void GetInput()
    {
        if(Input.GetKey(GetInputs.forwardKey))
        {
            acceleration.z += speed;
        }

        if(Input.GetKey(GetInputs.backwardKey))
        {
            acceleration.z -= speed;
        }

        if(Input.GetKey(GetInputs.rightwardKey))
        {
            acceleration.x += speed;
        }

        if(Input.GetKey(GetInputs.leftwardKey))
        {
            acceleration.x -= speed;
        }

        if(Input.GetKey(GetInputs.upwardKey))
        {
            acceleration.y += speed;
        }

        if(Input.GetKey(GetInputs.downwardKey))
        {
            acceleration.y -= speed;
        }

        shootThisFrame = false;
        if (Input.GetKey(GetInputs.shoot))
        {
            shootThisFrame = true;
        }


        if (Input.GetKeyDown(boostKey) && boostCharge >= 100)
        {
            acceleration.z += boost;
            boostCharge = 0;
        }

    }

    void Move()
    {
        rb.useGravity = false;
        rb.drag = 0.1f;

        rb.AddForce(acceleration.z * transform.forward, ForceMode.Force);
        rb.AddForce(acceleration.x * transform.right, ForceMode.Force);
        rb.AddForce(acceleration.y * transform.up, ForceMode.Force);

        //acceleration *= dragMultiplyer;
        if(acceleration.magnitude < slowestSpeed)
        {
            acceleration = Vector3.zero;
        }

        if (boostCharge < 100)
        {
            boostCharge += boostChargeRate * Time.fixedDeltaTime;
        }

        acceleration = Vector3.zero;

        if (shootThisFrame)
        {
            Shoot();
        }
    }

    void Rotate()
    {
        float multiplyer = Quaternion.Angle(transform.rotation, cameraContainer.transform.rotation) * 0.02f;
        rb.rotation = (Quaternion.RotateTowards(transform.rotation, cameraContainer.transform.rotation, rotationSpeed * multiplyer));
    }

    void Shoot()
    {
        if (rateLeft > 0)
        {
            rateLeft -= Time.deltaTime;
            return;
        }

        rateLeft = 1 / rate;

        boltRenderer.CreateNewBolt(transform.position, transform.position + (transform.forward * 100), 10, 10, transform.forward);
    }

    public void GetHit()
    {
        Debug.Log("gothit");
    }
}