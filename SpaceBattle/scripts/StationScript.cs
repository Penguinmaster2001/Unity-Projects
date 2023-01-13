using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationScript : MonoBehaviour
{
    public GameObject ally;

    public float speed;
    public float turn;

    public float spawnTime;
    public float spawnTimeLeft;
    public int spawnNum;

    // Update is called once per frame
    void Update()
    {
        transform.position += -transform.right * speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, turn, 0) * Time.deltaTime);

        spawnTimeLeft -= Time.deltaTime;

        if (spawnTimeLeft <= 0 && spawnNum > 0)
        {
            spawnNum--;
            spawnTimeLeft = spawnTime;
            GameObject nAlly = Instantiate(ally, transform.position + new Vector3(0, 1000, 0), Quaternion.Euler(0, 0, 0));
            nAlly.GetComponent<AIScript>().home = gameObject;
        }
    }
}
