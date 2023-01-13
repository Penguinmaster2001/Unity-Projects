using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiShip : MonoBehaviour
{
    public GameObject Main;

    public Transform target;

    public float speed = 5f;
    public float rotateSpeed = 200f;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GameObject newBolt = Instantiate(Main);
        newBolt.transform.position = transform.position;

        newBolt.transform.LookAt(Vector3.forward);

        newBolt.GetComponent<boltScript>().speed += speed + 500;

        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;
    }

    //void OnTriggerEnter2D()
    //{
    //    Put a particle effect here
    //    Destroy(gameObject);
    //}
}
