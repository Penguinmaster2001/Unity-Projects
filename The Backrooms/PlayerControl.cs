using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject artifact;
    public GameObject book;
    public GameObject MainCamera;
    public GameObject flashlight;
    private GameObject obj;
    private Rigidbody rb;
    public float maxHealth;
    public float health;
    public Text healthText;

    public int holdingObject;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -80F;
    public float maximumY = 80F;
    private float rotationY = 0F;
    public float speed = 5;
    public float fallMult = 5f;
    public float lowJumpMult = 5f;
    
    //controls lights to align them properly
    private readonly float frequency = 1;
    private float switchTimeLeft;
    public bool on;

    public AudioClip[] music;
    private int lastSong;
    private int lastLastSong;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();


        health = maxHealth;
        lastSong = 0;
        lastLastSong = 0;
    }

    void Update()
    {
        CameraControl();
        MouseInputs();
        Move();
        PlayMusic();
        ControlLights();

        if (health < maxHealth)
            health += Time.deltaTime * 0.1f;
        healthText.text = health.ToString();
    }

    void CameraControl()
    {
        //MainCamera.GetComponent<Camera>().fieldOfView = health / 4;

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = MainCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            MainCamera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            MainCamera.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            MainCamera.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void MouseInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(MainCamera.transform.position, MainCamera.transform.forward);
            Physics.Raycast(ray, out hit);

            if (hit.collider == null)
                return;

            switch (hit.collider.tag)
            {
                case "Artifact":
                    if (holdingObject == 0)
                    {
                        holdingObject = 1;
                        hit.collider.SendMessage("Destroy");
                        //destroy artifact
                    }
                    break;
                case "Book":
                    if (holdingObject == 0)
                    {
                        holdingObject = 2;
                        hit.collider.SendMessage("Destroy");
                        //destroy book
                    }
                    break;
                case "Box":
                    if (holdingObject == 1 && !hit.collider.gameObject.GetComponent<BoxScript>().active)
                    {
                        holdingObject = 0;
                        hit.collider.SendMessage("Entered");
                    }
                    break;
                case "Door":
                    hit.collider.SendMessage("Open");
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1) && holdingObject != 0)
        {
            
            if (holdingObject == 1)
            {
                    obj = artifact;
            }

            if (holdingObject == 2)
                {
                    obj = book;
            }

            GameObject newObject = Instantiate(obj, transform.position + MainCamera.transform.forward, new Quaternion(0, 0, 0, 0));
            newObject.GetComponent<Rigidbody>().AddForce(MainCamera.transform.forward * 25, ForceMode.VelocityChange);
            newObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(10, 0, 0);

            holdingObject = 0;
        }

        if (Input.GetMouseButtonDown(3))
            flashlight.GetComponent<Light>().enabled = !flashlight.GetComponent<Light>().enabled;
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //sprint
            speed = 6 * (health / maxHealth);
        else
            speed = 5 * (health / maxHealth);

        transform.position += (MainCamera.transform.forward - new Vector3(0, MainCamera.transform.forward.y, 0)) * Input.GetAxis("Vertical") * speed * Time.deltaTime; //walk forward
        transform.position += MainCamera.transform.right * Input.GetAxis("Horizontal") * 4 * Time.deltaTime;     //strafe
    }

    void PlayMusic()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            lastLastSong = lastSong;

            int s = Random.Range(0, music.Length);

            while (s == lastSong || s == lastLastSong)
            {
                s = Random.Range(0, music.Length);
            }

            lastSong = s;

            GetComponent<AudioSource>().clip = music[Random.Range(0, music.Length)];
            GetComponent<AudioSource>().Play();
        }
    }

    void ControlLights()
    {
        //switches all lights at the same time
        switchTimeLeft -= Time.deltaTime;

        if (switchTimeLeft <= 0)
        {
            switchTimeLeft = frequency;
            on = !on;
        }
    }
}