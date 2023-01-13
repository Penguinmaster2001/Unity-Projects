using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltRenderer
{
    public List<Bolt> bolts = new List<Bolt>();
    public Material boltMat;

    public void CreateNewBolt(Vector3 startPos, Vector3 target, float length, float speed, Vector3 forward)
    {
        Bolt addBolt = new Bolt()
        {
            position = startPos,
            target = target,
            length = length,
            speed = speed,
            forward = forward,
            lineMat = boltMat
        };

        addBolt.Initialize();

        bolts.Add(addBolt);
    }

    public void UpdateBolts()
    {
        /*
        for (int i = 0; i < bolts.Count; i++)
        {
            Debug.Log("updating bolt1");

            bolts[i].UpdateBolt();
        }
        */

        foreach (Bolt bolt in bolts)
        {
            if (bolt.updates >= 300)
            {
                GameObject.Destroy(bolt.boltObject);
                bolts.Remove(bolt);
            }

            bolt.UpdateBolt();
        }
    }
}

public class Bolt
{
    public GameObject boltObject = new GameObject
    {
        tag = "Bolt"
    };
    public LineRenderer line;

    public Vector3 position;
    public Vector3 target;
    public Vector3 forward;
    public float length;
    public float speed;
    public float updates;
    public Material lineMat;

    public void Initialize()
    {
        line = boltObject.AddComponent<LineRenderer>();
        line.material = lineMat;
        line.startWidth = 0.1f;
        line.positionCount = 2;
    }

    public void UpdateBolt()
    {
        position += speed * forward;
        target = position + (speed * forward);

        line.SetPositions(new Vector3[] { position, target });
        updates ++;

        foreach (var testCollider in ShipCounter.colliders)
        {
            if (testCollider.bounds.Contains(boltObject.transform.position))
            {
                //Debug.Log("SentHit");
                testCollider.gameObject.SendMessageUpwards("GetHit", SendMessageOptions.DontRequireReceiver);
                testCollider.gameObject.SendMessage("GetHit", SendMessageOptions.DontRequireReceiver);

                if(testCollider.gameObject.TryGetComponent<AIScript>(out AIScript ais))
                    ais.GetHit();
            }
        }
    }
}

/*
public class BoltScript : MonoBehaviour
{
    public GameObject shotFrom;
    public int      frames;
    public float    speed;
    public float    damage;
    private int     i;
    
    void Update()
    {
        i++;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        if (i >= frames && (
            transform.position.x >  1000 ||
            transform.position.y >  1000 ||
            transform.position.x < -1000 ||
            transform.position.y < -1000 ))
        {
            Destroy(gameObject);
        }

        if (transform.position.x > 1000)
            transform.position = new Vector2(-1000, transform.position.y);
        if (transform.position.y > 1000)
            transform.position = new Vector2(transform.position.x, -1000);
        if (transform.position.x < -1000)
            transform.position = new Vector2(1000, transform.position.y);
        if (transform.position.y < -1000)
            transform.position = new Vector2(transform.position.x, 1000);
    }
}
*/