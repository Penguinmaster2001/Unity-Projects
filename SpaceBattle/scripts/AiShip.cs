using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiShip : MonoBehaviour
{
    public float    heath;
    public Text     heathText;
    public Image    heathbar;

    public GameObject Main;

    public Transform target;

    public float speed = 5f;
    public float rotateSpeed = 200f;

    private Rigidbody2D rb;

    private int a = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (a == 100)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;

            GameObject newBolt = Instantiate(Main);
            newBolt.transform.position = transform.position;

            newBolt.transform.LookAt(Vector3.forward);

            //newBolt.GetComponent<BoltScript>().speed += speed + 50;

            Vector2 direction = (Vector2)target.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;
            a = 0;
        }
        a++;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("GBolt"))
        {
            //heath -= other.GetComponent<BoltScript>().damage;
            if (heath <= 0)
            {
                Destroy(gameObject);
                SpawnScript.currentSpawned--;
            }   
        }

        heathbar.fillAmount = heath / 200;
    }
}