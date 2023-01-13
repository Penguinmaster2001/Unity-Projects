using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boltScript : MonoBehaviour
{
    public int frames;
    public float speed;
    private int i;
    
    void Update()
    {
        i++;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (i >= frames && (
            transform.position.x >  100 ||
            transform.position.y >  100 ||
            transform.position.x < -100 ||
            transform.position.y < -100 ))
        {
            Destroy(gameObject);
        }

        if (transform.position.x > 100)
            transform.position = new Vector2(-100, transform.position.y);
        if (transform.position.y > 100)
            transform.position = new Vector2(transform.position.x, -100);
        if (transform.position.x < -100)
            transform.position = new Vector2(100, transform.position.y);
        if (transform.position.y < -100)
            transform.position = new Vector2(transform.position.x, 100);
    }
}