using UnityEngine;

public class Fish
{
    public GameObject thisFish;
    public Material mat;
    public Mesh mesh;
    public School school;

    public float size;
    public float speed = 4;
    public float maxForce = 0.2f;
    public bool alive = true;

    //motion vectors
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public void InitalizeFish(Material fishMaterial, Mesh fishMesh, GameObject baseFish, float speed, float size, School school)
    {
        this.school = school;
        mat = fishMaterial;
        mesh = fishMesh;
        thisFish = new GameObject()
        {
            name = "Fish"
        };

        thisFish.AddComponent<MeshFilter>();
        thisFish.AddComponent<MeshRenderer>();

        thisFish.GetComponent<MeshFilter>().mesh = mesh;
        thisFish.GetComponent<MeshRenderer>().material = mat;

        this.speed = speed;
        this.size = size;
        velocity = Random.insideUnitSphere * this.speed;
        thisFish.transform.localScale *= size;
    }

    public void UpdateFish(float dTime, Vector3 aveLocPos, Vector3 aveLocVel, Vector3 aveDist, Vector3 target, GameObject avoid, Vector3 mouth,
        float alignFactor, float cohesionFactor, float separateFactor, float targetFactor, float avoidFactor, float AvoidObsticlesFactor)
    {
        //call interact functions with average position
        acceleration = Vector3.zero; //reset
        acceleration += Align(aveLocVel, velocity) * alignFactor; //add alignment
        acceleration += Cohere(aveLocPos, velocity, position) * cohesionFactor; //add cohesion
        acceleration += Separate(aveDist, velocity, position) * separateFactor; //add cohesion
        acceleration += Target(target, velocity, position) * targetFactor; //add target
        acceleration += Avoid(avoid.transform.position, velocity, position) * avoidFactor; //add avoid
        acceleration += AvoidObsticles(thisFish.transform.forward, velocity, position, 5) * AvoidObsticlesFactor;
        //acceleration += RandomChange(); //TODO make this effect last over multiple frames in the same direction to reduce jittering

        if(position.y > UniversalVariables.waterHeight)
        {
            acceleration += UniversalVariables.gravity;
        }

        //change velocity
        velocity += acceleration; //add acceleration
        velocity = Vector3.ClampMagnitude(velocity, speed); //keep velocity at constant magnitude
        position += velocity * dTime; //change position with frame time

        //look forward (easy)
        thisFish.transform.LookAt(position);

        //move fish
        thisFish.transform.position = position;

        if(Vector3.Distance(position, mouth) < avoid.GetComponent<Movement>().mouthsize)
        {
            GetEaten(avoid);
        }
    }

    public void GetEaten(GameObject Shark)
    {
        Shark.GetComponent<Movement>().Eat(size);
        alive = false;
        GameObject.Destroy(thisFish);
    }

    #region
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
        if(!Physics.Linecast(pos, toAvoid))
        {
            return Vector3.zero;
        }

        float distance = Vector3.Distance(pos, toAvoid);
        toAvoid -= pos;
        toAvoid /= Mathf.Pow(distance, 8) * 5;
        toAvoid = toAvoid.normalized * speed;
        toAvoid -= vel;
        return -Vector3.ClampMagnitude(toAvoid, maxForce);
    }

    Vector3 Align(Vector3 steering, Vector3 vel)
    {
        return Vector3.ClampMagnitude((steering.normalized * speed) - vel, maxForce);
    }

    Vector3 Cohere(Vector3 avePos, Vector3 vel, Vector3 pos)
    {
        avePos -= pos;
        avePos = avePos.normalized * speed;
        avePos -= vel;
        return Vector3.ClampMagnitude(avePos, maxForce);
    }

    Vector3 Separate(Vector3 aveDist, Vector3 vel, Vector3 pos)
    {
        aveDist = aveDist.normalized * speed;
        aveDist -= vel;
        return Vector3.ClampMagnitude(aveDist, maxForce);
    }

    Vector3 AvoidObsticles(Vector3 forwardDirection, Vector3 vel, Vector3 pos, float closeDistance)
    {
        Ray ray = new Ray(pos, forwardDirection);
        Physics.Raycast(ray, out RaycastHit hit, closeDistance);
        if(hit.collider != null)
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
            //Debug.DrawRay(ray.origin, obsticle, Color.yellow);
            return Vector3.ClampMagnitude(obsticle, maxForce);

        }
        else
        {
            //Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            return Vector3.zero;
        }
    }

    Vector3 RandomChange()
    {
        return Vector3.ClampMagnitude(Random.insideUnitSphere, maxForce);
    }
    #endregion
}
