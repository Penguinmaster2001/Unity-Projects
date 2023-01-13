using UnityEngine;

public class LargeShipScript : MonoBehaviour
{
    public GameObject explosionPS;
    public GameObject warpGameobject;
    public GameObject brokeFront;
    public GameObject brokeBack;
    public float hull;
    public Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Warp()
    {
        Instantiate(warpGameobject, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }

    void Explode()
    {
        GameObject newPS = Instantiate(explosionPS, transform.position + (transform.right * 600), Quaternion.Euler(0, 0, 0));
        ParticleSystem ps = newPS.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startSize = 500;
        Instantiate(brokeFront, transform.position, transform.rotation);
        Instantiate(brokeBack, transform.position, transform.rotation);

        Rigidbody[] rbs = FindObjectsOfType<Rigidbody>();

        float limitDistance = 1000;
        foreach(Rigidbody r in rbs)
        {
            if(r.tag != "Bolt")
                r.AddExplosionForce(1000, transform.position + (transform.right * 600), limitDistance);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bolt")
        {
            GameObject newPS = Instantiate(explosionPS, transform.position + (transform.right * 600), Quaternion.Euler(0, 0, 0));
            ParticleSystem ps = newPS.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startSize = .1f;

            Destroy(collision.gameObject);
            hull -= 1;
        }
        else if (collision.gameObject.tag == "BigBolt")
        {
            Instantiate(explosionPS, collision.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(collision.gameObject);
            
            hull -= 100;
        }
        else
        {
            hull -= (collision.rigidbody.velocity.magnitude + rb.velocity.magnitude) / 5;
        }

        if (hull <= 0)
        {
            Explode();
        }
    }
}