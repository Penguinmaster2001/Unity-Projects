using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    [Space]
    [Header("AI Settings")]
    public float maxRange;
    public GameObject home;
    public string targetTag;
    public float shotCooldown;
    private float shotCooldownLeft;
    public float targetDistance;
    public float safeZone;
    public bool attack;
    public Material boltMat;

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

    //public GameObject explosionGameobject;
    //private ParticleSystem explosionPS;
    //public GameObject warpGameobject;
    //private ParticleSystem warpPS;
    public Rigidbody rb;
    public GameObject enemy;
    private List<GameObject> enemies = new List<GameObject>();

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

    public CapsuleCollider cc;
    public BoltRenderer boltRenderer;

    // Start is called before the first frame update
    void Start()
    {

        boltRenderer = new BoltRenderer
        {
            boltMat = this.boltMat
        };

        //explosionPS = explosionGameobject.GetComponent<ParticleSystem>();
        //warpPS = warpGameobject.GetComponent<ParticleSystem>();
        //warpPS.Play(true);

        //shieldCollider = shield.GetComponent<CapsuleCollider>();
        //shieldMesh = shield.GetComponent<MeshRenderer>();

        //cc = gameObject.GetComponent<CapsuleCollider>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        boltRenderer.UpdateBolts();

        Shoot();

        //GetHit();

        //DoAI();

        //DoShield();

        //DoEnergy();
    }

    void ChooseTarget()
    {
        foreach (var addEnemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            enemies.Add(addEnemy);
        }

        float currentDistance;
        float closestDistance = 10000;
        GameObject closestEn = enemies[1];
        foreach (GameObject en in enemies)
        {
            currentDistance = Vector3.Distance(transform.position, en.transform.position);
            if (currentDistance < closestDistance && Vector3.Distance(home.transform.position, en.transform.position) <= maxRange)
            {
                closestDistance = currentDistance;
                closestEn = en;
            }
        }

        enemy = closestEn;
    }

    void DoAI()
    {

        if (enemy == null) //choose enemy
        {
            attack = false;
            ChooseTarget();
            Move(speed, (home.transform.position - transform.position));
        }
        else if(Vector3.Distance (transform.position, home.transform.position) >= maxRange) //goto home if too far away
        {
            attack = false;
            if(Vector3.Distance(home.transform.position, enemy.transform.position) <= maxRange) //if enemy is too far, then lose it
                enemy = null;
            Move(speed, (home.transform.position - transform.position));
        }
        else
        {
            attack = true;
        }

        if(energy < 25 || shotCooldownLeft <= 0)
            shieldOn = false;

        if(hull <= 25)
            Warp();

        if(attack)
            Attack();
        else
            GetAway();



        /*
        if(Vector3.Distance(transform.position, enemy.transform.position) < 50) //fly away from close player
        {
            if(energy > 25) //fly away from close player if energy allows
            {
                shieldOn = true;
                energy -= Time.deltaTime;
                timeSinceUse = 0;
                Move(speed, -(Vector3.zero - transform.position));
            }
            //else if (hull > 25) //quick escape that damages hull
            //{
            //    energy -= 100;
            //    hull -= 25;
            //    Move(speed * 1000, new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), 50));
            //}
            else //last ditch effort to destroy player
            {
                rate = 20;
                Shoot();
                Move(speed, Vector3.zero - transform.position);
            }
        }
        else if(Vector3.Distance(transform.position, enemy.transform.position) > safeZone) //get closer to far away player quickly
        {
            if(Vector3.Distance(transform.position, Vector3.zero) > 1000) //go back to middle if not doing anything else
            {
                Move(speed, Vector3.zero - transform.position);
            }
            else if(energy > 75) //get to player if energy allows
            {
                energy -= Time.deltaTime * 10;
                timeSinceUse = 0;
                Move(speed * 50, target - transform.position);
            }
            else //wait to recharge if not in immdiate danger
            {
                if(shotCooldownLeft <= 0)
                    shieldOn = false;
                Move(speed, target - transform.position);
            }
        }
        else if(Vector3.Distance(transform.position, enemy.transform.position) > targetDistance + 100) //keep player at correct distance
        {
            Shoot();
            Move(speed, target - transform.position);
        }
        else if(Vector3.Distance(transform.position, enemy.transform.position) < targetDistance - 100) //keep player at correct distance
        {
            shieldOn = true;
            Move(speed, -(target - transform.position));
        }
        else //if player is at correct distance, attempt to get behind and chase
        {
            Shoot();
            Move(speed, (target + new Vector3(0, 0, -500)) - transform.position);
        }
        */
        
    }

    void Move(float moveSpeed, Vector3 targetDirection)
    {
        //face direction
        // The step size is equal to speed times frame time.
        float singleStep = rotSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);

        //transform.position += speed * transform.forward;
        //rb.AddForce(speed * transform.forward, ForceMode.Force);

        rb.AddForce(transform.forward * speed, ForceMode.Force);

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

            boltRenderer.CreateNewBolt(Guns[gunNum].transform.position, Guns[gunNum].transform.position + (Guns[gunNum].transform.forward * 100), 10, 10, Guns[gunNum].transform.forward);
        }
    }

    void Attack()
    {
        Vector3 Compute_first_order_correction(Vector3 target_position, Vector3 target_velocity, float projectile_speed) //track moving target
        {
            float t = Vector3.Distance(transform.position, target_position) / projectile_speed;
            return target_position + t * target_velocity;
        }

        Move(speed, Compute_first_order_correction(enemy.transform.position, enemy.GetComponent<Rigidbody>().velocity, 1000) - transform.position);
        Shoot();
    }

    void GetAway()
    {
        Move(speed, home.transform.position - transform.position);
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

    public void Warp()
    {
      //  Instantiate(warpGameobject, transform.position, transform.rotation);
        Destroy(gameObject);
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
            attack = false;
            //if(!collision.gameObject.GetComponent<BoltScript>().shotFrom.tag.StartsWith(tag[0].ToString()));
            //    enemy = collision.gameObject.GetComponent<BoltScript>().shotFrom;
            shotCooldownLeft = shotCooldown;
        }
        else if (collision.gameObject.tag == "BigBolt")
        {
            Destroy(collision.gameObject);

            timeSinceUse = 0;

            if (!shieldOn || energy <= 0)
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

        if (hull <= 0)
        {
        //    Instantiate(explosionPS, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }

    public void GetHit()
    {
        Debug.Log("Hit");
        //Destroy(gameObject);
    }
}
