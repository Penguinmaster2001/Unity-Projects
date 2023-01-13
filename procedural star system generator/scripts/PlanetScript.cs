using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public Vector3 rotation;
    public float altitude;
    public float speedScale;
    public float speed;
    public Rigidbody rb;
    public GameObject OrbitBody;

    public string SystemName;

    public string alienName;
    public int alienTrade;

    void Awake()
    {
        Random.InitState(SystemName.GetHashCode());

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = 0;
        rb.AddTorque(rotation, ForceMode.VelocityChange);

        CreateAliens();
    }

    void Update()
    {
        speedScale = (2 * Mathf.PI) / (altitude / 100);

        transform.Rotate(rotation);

        Vector3 focus = OrbitBody.transform.position;

        var angle = Time.time * speedScale;
        transform.position = new Vector3((Mathf.Sin(angle) * altitude) + focus.x, focus.y, (Mathf.Cos(angle) * altitude) + focus.z);
    }

    void CreateAliens()
    {
        alienName = NameLists.alienNames[Random.Range(0, NameLists.alienNames.Length)];
    }
}