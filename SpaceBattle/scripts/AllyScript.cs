using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AllyScript : MonoBehaviour
{
    [Space]
    [Header("AI Settings")]
    public float shotCooldown;
    private float shotCooldownLeft;
    public float targetDistance;
    public float safeZone;

    [Space]
    [Header("Guns")]
    public GameObject boltObject;
    public GameObject[] Guns;
    private int gunNum;
    public float rate;
    private float rateLeft;


    [Space]
    [Header("Movement")]
    public float rotSpeed;
    public float speed;
    public float useSpeed;
    public bool move;

    public GameObject explosionGameobject;
    private ParticleSystem explosionPS;
    public GameObject warpGameobject;
    private ParticleSystem warpPS;
    private Rigidbody rb;
    public GameObject enemy;
    private List<GameObject> enemies = new List<GameObject>();
    private Vector3 target;

    [Space]
    [Header("Shield")]
    public GameObject shield;
    public bool shieldOn = false;
    private CapsuleCollider shieldCollider;
    private MeshRenderer shieldMesh;
    public float hull;

    [Space]
    [Header("Energy")]
    public float energy;
    public float rechargeRate;
    public float maxEnergy;
    public float cooldownTime;
    private float timeSinceUse;

    // Start is called before the first frame update
    void Start()
    {
        explosionPS = explosionGameobject.GetComponent<ParticleSystem>();
        warpPS = warpGameobject.GetComponent<ParticleSystem>();
        warpPS.Play(true);

        shieldCollider = shield.GetComponent<CapsuleCollider>();
        shieldMesh = shield.GetComponent<MeshRenderer>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        DoShield();

        DoEnergy();
    }

    void Move()
    {
        Vector3 targetDirection;
        //AI
        foreach (var addEnemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(addEnemy);
        }

        float currentDistance;
        float closestDistance = 10000;
        GameObject closestEn = enemies[1];
        foreach (GameObject en in enemies)
        {
            currentDistance = Vector3.Distance(transform.position, en.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestEn = en;
            }
        }

        enemy = closestEn;
        target = enemy.transform.position;

        targetDirection = transform.rotation.eulerAngles;
        move = false;
        useSpeed = speed;
        if (Vector3.Distance(transform.position, enemy.transform.position) < 50) //fly away from close player
        {
            if (energy > 25) //fly away from close player if energy allows
            {
                shieldOn = true;
                energy -= Time.deltaTime * 10;
                timeSinceUse = 0;
                targetDirection = -(target - transform.position);
                move = true;
                useSpeed = speed * 50;
            }
            else if (hull > 25) //quick escape that damages hull
            {
                energy -= 100;
                hull -= 25;
                targetDirection = new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), 50);
                move = true;
                useSpeed = speed * 1000;
            }
            else //last ditch effort to destroy player
            {
                targetDirection = (target - transform.position);
                move = false;
                useSpeed = speed;
                rate = 20;
                Shoot();
            }
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) > safeZone) //get closer to far away player quickly
        {
            if (Vector3.Distance(transform.position, Vector3.zero) > 1000) //go back to middle if not doing anything else
            {
                targetDirection = (Vector3.zero - transform.position);
                move = true;
                useSpeed = speed;
            }
            else if (energy > 75) //get to player if energy allows
            {
                energy -= Time.deltaTime * 10;
                timeSinceUse = 0;
                targetDirection = (target - transform.position);
                move = true;
                useSpeed = speed * 50;
            }
            else //wait to recharge if not in immediate danger
            {
                shieldOn &= shotCooldownLeft > 0;
                targetDirection = (target - transform.position);
                move = false;
                useSpeed = speed;
            }
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) > targetDistance + 100) //keep player at correct distance
        {
            Shoot();
            targetDirection = (target - transform.position);
            move = true;
            useSpeed = speed;
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) < targetDistance - 100) //keep player at correct distance
        {
            shieldOn = true;
            targetDirection = -(target - transform.position);
            move = true;
            useSpeed = speed;
        }
        else //if player is at correct distance, attempt to get behind and chase
        {
            Shoot();
            move = true;
            targetDirection = ((target + new Vector3(0, 0, -500)) - transform.position);
            useSpeed = speed;
        }

        //face direction
        // The step size is equal to speed times frame time.
        float singleStep = rotSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (move) //move
        {
            transform.position += useSpeed * transform.forward;
            //rb.AddForce(useSpeed * transform.forward, ForceMode.Force);
            //rb.AddForce(acceleration.x * transform.right, ForceMode.Force);
            //rb.AddForce(acceleration.y * transform.up, ForceMode.Force);
        }
    }

    void Shoot()
    {
        if (energy > 0)
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
            newBolt.transform.LookAt(enemy.transform);
            newBolt.transform.Rotate(0, 0, 0);
        }
    }

    void DoShield()
    {
        shotCooldownLeft -= Time.deltaTime;

        if (shotCooldownLeft <= 0)
            shieldOn = false;
        else
            shieldOn = true;

        if (energy <= 25)
            shieldOn = false;

        if (energy > 0 && shieldOn)
        {
            shieldCollider.enabled = true;
            shieldMesh.enabled = true;
            energy -= Time.deltaTime * .05f;
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
        if (collision.gameObject.tag == "bolt")
        {
            Destroy(collision.gameObject);

            timeSinceUse = 0;

            if (!shieldOn || energy <= 0)
            {
                hull--;
            }
            else
                energy--;

            shieldOn = true;
            shotCooldownLeft = shotCooldown;

            if (hull <= 0)
            {
                explosionPS.Play(true);
                //Destroy(model);
                if (!explosionPS.isPlaying)
                Destroy(gameObject);
            }
        }
    }
}