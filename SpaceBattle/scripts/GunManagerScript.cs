using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManagerScript : MonoBehaviour
{
    //public Vector3 offset;
    public Vector3 rotation;
    public float maxRange;
    public string targetTag;
    public GameObject bolt;
    public int gunAmount;
    public float radius;
    public float shootRate;
    private float shootRateLeft;
    private int gunNum;

    private Vector3[] guns;
    private GameObject[] gunEnemies;
    private LineRenderer ln;
    
    private void Start()
    {
        ln = GetComponent<LineRenderer>();
    }

    void Update()
    {
        guns = new Vector3[gunAmount];
        gunEnemies = new GameObject[gunAmount];

        shootRateLeft -= Time.deltaTime;
        if (shootRateLeft <= 0)
        {
            shootRateLeft = 1 / shootRate;

            for(int i = 0; i < guns.Length; i++)
            {
                guns[i] = (transform.position) + (Quaternion.Euler(rotation + transform.rotation.eulerAngles) * (Quaternion.AngleAxis((360 / gunAmount) * i, Vector3.up) * (Vector3.forward * radius)));
            }

            if(gunNum < guns.Length - 1)
                gunNum++;
            else
                gunNum = 0;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

            float currentDistance;
            float closestDistance = maxRange;
            GameObject closestEn = enemies[1];
            foreach(GameObject en in enemies)
            {
                currentDistance = Vector3.Distance(guns[gunNum], en.transform.position);
                if(currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestEn = en;
                }
            }

            ShootGun(guns[gunNum], closestEn);

            ln.positionCount = guns.Length;
            ln.SetPositions(guns);
        }
    }

    void ShootGun(Vector3 gunPos, GameObject target)
    {
        Vector3 rand = new Vector3(Random.Range(-0.5f, .5f), 0, Random.Range(-0.5f, .5f)); //make all bolt star pos inconmsistant

        Vector3 Compute_first_order_correction(Vector3 target_position, Vector3 target_velocity, float projectile_speed) //track moving target
        {
            float t = Vector3.Distance(transform.position, target_position) / projectile_speed;
            return target_position + t * target_velocity;
        }

        Vector3 aim = Compute_first_order_correction(target.transform.position, target.GetComponent<Rigidbody>().velocity, 1000);

        //Ray ray = new Ray(gunPos, aim); //check if going to acually hit enemy
        //Physics.Raycast(ray, out RaycastHit hit);
        //if(hit.collider.gameObject != target)
        //    return;

        GameObject newBolt = Instantiate(bolt);
        newBolt.transform.position = gunPos + rand;
        newBolt.transform.LookAt(aim);
        //newBolt.GetComponent<BoltScript>().shotFrom = gameObject;
    }
}