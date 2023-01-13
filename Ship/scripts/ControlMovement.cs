using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ControlMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Xvel;
    public float Yvel;
    public float fastSpeed;
    public float fineSpeed;
    private float appliedSpeed;
    public float maxSpeed;
    public float[] boltspeed;
    private int a;
    private bool moveType;

    public GameObject Main;
    public GameObject Flack;
    public Camera MainCamera;
    public Text movementText;
    public Text speedText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Inputs();
        Move();
        UpdateText();
    }

    void FixedUpdate()
    {
        if (a == 24)
        {
            GameObject newBolt = Instantiate(Main);
            newBolt.transform.position = transform.position;

            RaycastHit hit;

            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit);

            newBolt.transform.LookAt(new Vector3(hit.point.x, hit.point.y, -5));

            newBolt.GetComponent<boltScript>().speed += new Vector3(Xvel, Yvel, 0).magnitude + boltspeed[0];

            gameObject.GetComponent<AudioSource>().Play();
            a = 0;
        }
        else if(a == 12)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject newBolt = Instantiate(Flack);
                newBolt.transform.position = transform.position;

                RaycastHit hit;

                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hit);

                newBolt.transform.LookAt(new Vector3(100, 100, 0));

                newBolt.transform.Rotate(new Vector3(i * 60, 0, 0));

                newBolt.GetComponent<boltScript>().speed += new Vector3(Xvel, Yvel, 0).magnitude + boltspeed[1];
            }
        }
        a++;
    }

    void Move()
    {
        transform.position += new Vector3(Xvel, Yvel, 0) * Time.deltaTime;

        if (transform.position.x > 100) //this keeps the ship within the box
            transform.position = new Vector2(-100, transform.position.y);
        if (transform.position.y > 100)
            transform.position = new Vector2(transform.position.x, -100);
        if (transform.position.x < -100)
            transform.position = new Vector2(100, transform.position.y);
        if (transform.position.y < -100)
            transform.position = new Vector2(transform.position.x, 100);
    }

    void Inputs()
    {
        if (moveType)
            appliedSpeed = fastSpeed;
        else
            appliedSpeed = fineSpeed;

        if (Input.GetKey(KeyCode.W))
            Yvel += appliedSpeed;
        if (Input.GetKey(KeyCode.S))
            Yvel -= appliedSpeed;
        if (Input.GetKey(KeyCode.D))
            Xvel += appliedSpeed;
        if (Input.GetKey(KeyCode.A))
            Xvel -= appliedSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
            moveType = !moveType;

        if (Xvel >= maxSpeed) //Mathf.Clamp wouldn't work
            Xvel = maxSpeed;
        if (Xvel <= -maxSpeed)
            Xvel = -maxSpeed;
        if (Yvel >= maxSpeed)
            Yvel = maxSpeed;
        if (Yvel <= -maxSpeed)
            Yvel = -maxSpeed;
    }

    void UpdateText()
    {
        speedText.text = "Speed: " + new Vector2(Xvel, Yvel).magnitude.ToString() + " X: " + Mathf.Round(Xvel).ToString() + " Y: " + Mathf.Round(Yvel).ToString();
        if (moveType)
            movementText.text = "Fast";
        else
            movementText.text = "Fine";
    }
}