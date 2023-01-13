/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controller object
public class FishController : MonoBehaviour
{
    public GameObject toAvoid;
    public GameObject toAttract;
    public GameObject toEat;
    public float eatDistance;

    //Placeholder for meshes and materials for future fish
    public Mesh fishMesh;
    public Material fishMat;
    public GameObject bloodSystem;

    //number of fish to create
    private static int fishAmount = 200;

    //master list of fishes
    public OldFish[] fishes = new OldFish[fishAmount];

    //boid settings
    public float effectDist = 2; //maximum distance fish can interact

    public float separateFactor = 1.0f; //effect multiplyers
    public float alignFactor    = 1.0f;
    public float cohesionFactor = 1.0f;
    public float targetFactor   = 1.0f;
    public float avoidFactor    = 1.0f;

    public float maxForce = 0.1f; //max effect
    public float speed    = 5.0f; //speed of fish in m/s

    void Start()
    {
        Random.InitState(42);

        for (int i = 0; i < fishAmount; i++)
        {
            float maxP = 10;
            fishes[i] = new OldFish()
            {
                size = 1f,
                fish = new GameObject()
                {
                    name = ("Fish" + i).ToString()
                },
                m = fishMesh,
                mat = fishMat,
                position = (Random.insideUnitSphere * maxP) + new Vector3(10, 0, 0),
                maxForce = this.maxForce,
                speed = this.speed,
                ps = bloodSystem
            };
            fishes[i].Initialize();
        }
    }
    
    void FixedUpdate()
    {
        //go to every fish
        int i = 0;
        foreach(OldFish thisFish in fishes)
        {
            if(thisFish != null)
            {
                //make list for fish within effect distance
                List<OldFish> closeFishes = new List<OldFish>();

                //make variables to hold average positions and velocities
                Vector3 averagePos = new Vector3();
                Vector3 averageVelocity = new Vector3();
                Vector3 averageDifference = new Vector3();

                //look at all other fish and add velocity and position to averages if they are close enough to this fish
                foreach(OldFish otherFish in fishes)
                {
                    //check that other fish is not this fish and that other fish is close enough
                    if(otherFish != null)
                    {
                        float distance = Vector3.Distance(thisFish.position, otherFish.position);
                        if(thisFish != otherFish && distance < effectDist)
                        {
                            //for all
                            averagePos += otherFish.position;
                            averageVelocity += otherFish.velocity;

                            //for separation only
                            Vector3 difference = thisFish.position - otherFish.position;
                            difference /= Mathf.Pow(distance, 2);
                            averageDifference += difference;

                            //add other fish to list
                            closeFishes.Add(otherFish);
                        }
                    }
                }

                if(closeFishes.Count == 0)
                {
                    //if no fish are close enough, use this fishes's own velocity and position
                    thisFish.UpdateFish(Time.fixedDeltaTime, thisFish.position, thisFish.velocity, thisFish.position, toAttract.transform.position, toAvoid.transform.position, toEat, eatDistance * Movement.points,
                        alignFactor, cohesionFactor, separateFactor, targetFactor, avoidFactor);
                }
                else
                {
                    //make totals averages
                    averagePos /= closeFishes.Count;
                    averageVelocity /= closeFishes.Count;
                    averageDifference /= closeFishes.Count;

                    //call function to update fish with average positions and velocities of relevant other fish and delta time
                    thisFish.UpdateFish(Time.fixedDeltaTime, averagePos, averageVelocity, averageDifference, toAttract.transform.position, toAvoid.transform.position, toEat, eatDistance * Movement.points,
                        alignFactor, cohesionFactor, separateFactor, targetFactor, avoidFactor);
                }
            }
            else
            {
                fishes[i] = null;
            }
            i++;
        }
    }
}

//fish object
public class OldFish
{
    public bool eaten = false;

    public float size;

    //gameobject setup
    public GameObject fish;
    public Mesh m;
    private MeshFilter mf;
    private MeshRenderer mr;
    public Material mat;
    public GameObject ps;

    //motion vectors
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public float maxForce; //max effect
    public float speed; //speed of fish in m/s

    public void Initialize()
    {
        fish.transform.localScale = new Vector3(size, size, size);
        fish.transform.position = position;

        mf = fish.AddComponent<MeshFilter>();
        mf.mesh = m;

        mr = fish.AddComponent<MeshRenderer>();
        mr.material = mat;
        float maxInitV = 5;
        velocity = Random.insideUnitSphere * maxInitV;
    }

    public void UpdateFish(float dTime, Vector3 aveLocPos, Vector3 aveLocVel, Vector3 aveDist, Vector3 target, Vector3 avoid, GameObject eat, float eatDist,
        float alignFactor, float cohesionFactor, float separateFactor, float targetFactor, float avoidFactor)
    {
        if(Vector3.Distance(position, eat.transform.position) < eatDist && eaten == false)
        {
            GetEaten(eat);
        }

        if(eaten == false)
        {
            //call interact functions with average position
            acceleration = Vector3.zero; //reset
            acceleration += Align(aveLocVel, velocity) * alignFactor; //add alignment
            acceleration += Cohese(aveLocVel, velocity, position) * cohesionFactor; //add cohesion
            acceleration += Separate(aveDist, velocity, position) * separateFactor; //add cohesion
            acceleration += Target(target, velocity, position) * targetFactor; //add target
            acceleration += Avoid(avoid, velocity, position) * avoidFactor; //add avoid
            acceleration += AvoidObsticles(fish.transform.forward, velocity, position, 5) * 10;

            if(position.y > UniversalVariables.waterHeight)
            {
                acceleration += UniversalVariables.gravity;
            }

            //change velocity
            velocity += acceleration; //add acceleration
            velocity = Vector3.ClampMagnitude(velocity, speed); //keep velocity at constant magnitude
            position += velocity * dTime; //change position with frame time

            //confine where the fish can be
            float maxDistance = 100;

            if(position.x > maxDistance)
                position.x = -maxDistance;
            if(position.x < -maxDistance)
                position.x = maxDistance;

            if(position.y > maxDistance)
                position.y = -maxDistance;
            if(position.y < -maxDistance)
                position.y = maxDistance;

            if(position.z > maxDistance)
                position.z = -maxDistance;
            if(position.z < -maxDistance)
                position.z = maxDistance;

            //look forward (easy)
            fish.transform.LookAt(position);

            //move fish
            fish.transform.position = position;
        }
    }

    public void GetEaten(GameObject eater)
    {
        eaten = true;
        GameObject.Instantiate(ps, fish.transform.position, fish.transform.rotation);
        eater.GetComponentInParent<Movement>().Eat();
        MeshRenderer.Destroy(mr);
        MeshFilter.Destroy(mf);
        GameObject.Destroy(fish);
    }

    //Interact functions
    Vector3 Target(Vector3 steering, Vector3 vel, Vector3 pos)
    {
        steering -= pos;
        steering = steering.normalized * speed;
        steering -= vel;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    Vector3 Avoid(Vector3 toAvoid, Vector3 vel, Vector3 pos)
    {
        float distance = Vector3.Distance(pos, toAvoid);
        toAvoid -= pos;
        toAvoid /= Mathf.Pow(distance, 8);
        toAvoid = toAvoid.normalized * speed;
        toAvoid -= vel;
        return -Vector3.ClampMagnitude(toAvoid, maxForce);
    }

    Vector3 Align(Vector3 steering, Vector3 vel)
    {
        return Vector3.ClampMagnitude((steering.normalized * speed) - vel, maxForce);
    }

    Vector3 Cohese(Vector3 steering, Vector3 vel , Vector3 pos)
    {
        steering -= pos;
        steering = steering.normalized * speed;
        steering -= vel;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    Vector3 Separate(Vector3 steering, Vector3 vel, Vector3 pos)
    {
        steering = steering.normalized * speed;
        steering -= vel;
        return Vector3.ClampMagnitude(steering, maxForce);
    }

    Vector3 AvoidObsticles(Vector3 forwardDirection, Vector3 vel, Vector3 pos, float closeDistance)
    {
        Ray ray = new Ray(pos, forwardDirection);
        Physics.Raycast(ray, out RaycastHit hit, closeDistance);
        if (hit.collider != null)
        {
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            //Debug.DrawRay(ray.origin, hit.normal, Color.magenta);

            Vector3 obsticle = (hit.normal - pos).normalized;
            obsticle = Vector3.Cross(obsticle, forwardDirection.normalized);
            //Debug.DrawRay(ray.origin, obsticle, Color.green);
            obsticle = -Vector3.Cross(obsticle, forwardDirection.normalized);
            //Debug.DrawRay(ray.origin, obsticle, Color.cyan);
            obsticle /= Mathf.Pow(hit.distance, 2);
            obsticle = obsticle.normalized * speed;
            obsticle -= vel;
            Debug.DrawRay(ray.origin, obsticle, Color.yellow);
            return Vector3.ClampMagnitude(obsticle, maxForce);
            
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            return Vector3.zero;
        }
    }
}
*/