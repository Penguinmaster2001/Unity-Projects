using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipScript : MonoBehaviour
{
    public Transform shipCamera;
    public Transform sight;
    private Vector3 forward;
    public GameObject boltObject;
    public GameObject[] Guns;
    private int gunNum;
    public float rate;
    private float rateLeft;
    Rigidbody rb;

    public float rotSpeed;
    public float speed;

    [Space]
    [Header("Texts")]
    public Text lockText;
    public Text energyText;
    public Text hullText;

    [Space]
    [Header("Shield")]
    public float hull;
    public GameObject shield;
    private CapsuleCollider shieldCollider;
    private MeshRenderer shieldMesh;
    public bool shieldOn = false;

    [Space]
    [Header("Energy")]
    public float energy;
    public float rechargeRate;
    public float maxEnergy;
    private float timeSinceUse;
    public float cooldownTime;

    void Start()
    {
        /*
        shieldCollider = shield.GetComponent<CapsuleCollider>();
        shieldMesh = shield.GetComponent<MeshRenderer>();
        */      
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (!Input.GetKey(KeyCode.Space))
            FaceCamera();
        float maxBounds = 10000;
        Vector3 bounds = transform.position;
        if(transform.position.x > maxBounds)
            bounds.x = -maxBounds;
        if(transform.position.y > maxBounds)
            bounds.y = -maxBounds;
        if(transform.position.z > maxBounds)
            bounds.z = -maxBounds;
        if(transform.position.x < -maxBounds)
            bounds.x = maxBounds;
        if(transform.position.y < -maxBounds)
            bounds.y = maxBounds;
        if(transform.position.z < -maxBounds)
            bounds.z = maxBounds;
        transform.position = bounds;

        Controls();

        //Shoot();

        //DoShield();

        //DoEnergy();

        energyText.text = ((energy / maxEnergy) * 100).ToString() + "%";
        hullText.text = hull.ToString();
    }

    void FaceCamera()
    {
        //// Determine which direction to rotate towards
        //Vector3 targetDirection = -(shipCamera.position - transform.position);

        //// The step size is equal to speed times frame time.
        //float singleStep = rotSpeed * Time.deltaTime;

        //// Rotate the forward vector towards the target direction by one step
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        //// Draw a ray pointing at our target in
        //Debug.DrawRay(transform.position, newDirection, Color.red);

        //// Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = shipCamera.rotation; //Quaternion.LookRotation(newDirection);
    }

    void Controls()
    {
        forward = transform.forward;

        sight.gameObject.transform.localPosition += new Vector3(0, 0, Input.mouseScrollDelta.y * 10);

        float throttle = 1;//Input.GetKey(GetInputs.forwardKey) ? 1:0;//Input.GetAxis("Throttle");
        if (energy > 0)
        {
            rb.AddForce(forward * speed * Time.deltaTime * throttle, ForceMode.Force);
            energy -= Time.deltaTime * throttle;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            shieldOn = !shieldOn;

        if (Input.GetKey(KeyCode.Space) && energy > 0)
        {
            energy -= Time.deltaTime * 1000;
            rb.velocity += forward * speed * 100 * Time.deltaTime;
            timeSinceUse = 0;
        }
    }

    void Shoot()
    {
        Vector3 aim;
        aim = sight.position;
        lockText.text = ("Lock: " + Vector3.Distance(transform.position, sight.position) + " meters ahead");

        if (Input.GetAxis("Shoot") != 0 && !Input.GetKey(KeyCode.Space) && energy > 0)
        {
            if (rateLeft > 0)
            {
                rateLeft -= Time.deltaTime;
                return;
            }

            rateLeft = 1 / rate;

            if (gunNum < Guns.Length - 1)
                gunNum++;
            else
                gunNum = 0;

            Vector3 rand = new Vector3(Random.Range(-0.5f, .5f), 0, Random.Range(-0.5f, .5f));

            timeSinceUse = 0;
            energy -= .1f;
            GameObject newBolt = Instantiate(boltObject);
            newBolt.transform.position = Guns[gunNum].transform.position + rand;
            newBolt.transform.LookAt(aim);
            newBolt.transform.Rotate(0, 0, 0);
            //newBolt.GetComponent<BoltScript>().speed += rb.velocity.magnitude;
        }
    }

    void DoShield()
    {
        if (energy > 0 && shieldOn)
        {
            shieldCollider.enabled = true;
            shieldMesh.enabled = true;
            energy -= Time.deltaTime * .5f;
        }
        else
        {
            shieldCollider.enabled = false;
            shieldMesh.enabled = false;
        }
    }

    void DoEnergy()
    {
        timeSinceUse += Time.deltaTime;

        if (timeSinceUse > cooldownTime && energy < maxEnergy)
            if (energy + (Time.deltaTime * rechargeRate) > maxEnergy)
                energy = maxEnergy;
            else
                energy += Time.deltaTime * rechargeRate;
        if (energy < 0)
            energy = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bolt")
        {
            Destroy(collision.gameObject);

            timeSinceUse = 0;

            if(!shieldOn || energy <= 0)
            {
                hull--;
            }
            else
                energy--;
        }
        else if(collision.gameObject.tag == "BigBolt")
        {
            Destroy(collision.gameObject);

            timeSinceUse = 0;

            if(!shieldOn || energy <= 0)
            {
                hull -= 250;
            }
            else
                energy = 0;
        }
        else
        {
            if(!shieldOn || energy <= 0)
            {
                hull -= (collision.rigidbody.velocity.magnitude + rb.velocity.magnitude) / 5;
            }
            else
                energy -= (collision.rigidbody.velocity.magnitude + rb.velocity.magnitude) / 5;
        }

        if(hull <= 0)
        {
            Destroy(gameObject);
        }
    }
}