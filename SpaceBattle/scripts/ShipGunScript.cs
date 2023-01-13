using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGunScript : MonoBehaviour
{
    public GameObject explosionPS;
    public string targetTag;
    public GameObject target;
    private GameObject[] enemies;
    public float rotSpeed;

    public GameObject boltObject;
    public float rate;
    public float rateLeft;
    public float shotStartDistance;
    public float hull;

    // Start is called before the first frame update
    void Start()
    {
        rateLeft += Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        rateLeft -= Time.deltaTime + Random.Range(0, 0.05f);

        GetTarget();

        if(target != null)
            TrackTarget();

        if (rateLeft <= 0 && target != null)
        {
            Shoot();
        }
    }

    void GetTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag(targetTag);

        //float currentDistance;
        //float closestDistance = 100000;
        //GameObject closestEn = enemies[1];
        //foreach (GameObject en in enemies)
        //{
        //    currentDistance = Vector3.Distance(transform.position, en.transform.position);
        //    if (currentDistance < closestDistance)
        //    {
        //        closestDistance = currentDistance;
        //        closestEn = en;
        //    }
        //}

        if(enemies != null)
        {
            if(enemies.Length > 1)
            {
                if(enemies[1] != null)
                    target = enemies[1];
            }
        }
    }

    void TrackTarget()
    {
        //face direction
        // The step size is equal to speed times frame time.
        float singleStep = rotSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, (target.transform.position - transform.position), singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void Shoot()
    {
        Vector3 rand = new Vector3(Random.Range(-0.5f, .5f), 0, Random.Range(-0.5f, .5f));

        Ray ray = new Ray(transform.position + (transform.right * shotStartDistance), transform.forward); //check if going to acually hit enemy
        Physics.Raycast(ray, out RaycastHit hit);
        if(hit.collider == null)
        {
            return;
        }
        else
        {
            if(hit.collider != target)
                return;
        }

        rateLeft = rate;
        GameObject newBolt = Instantiate(boltObject);
        newBolt.transform.position = transform.position + rand + (transform.forward * shotStartDistance);
        newBolt.transform.rotation = transform.rotation;
        //newBolt.GetComponent<BoltScript>().shotFrom = gameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bolt")
        {
            Destroy(collision.gameObject);

            hull--;

            if (hull <= 0)
            {
                Instantiate(explosionPS, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
            }
        }
    }
}