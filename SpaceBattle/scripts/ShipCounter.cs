using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCounter : MonoBehaviour
{
    public int collideramount;

    public static List<Collider> colliders = new List<Collider>();
    private static List<Collider> collidersatoAdd = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        collideramount = 0;
        collidersatoAdd.Clear();
        foreach (var addEnemy in FindObjectsOfType<Collider>())
        {
            collidersatoAdd.Add(addEnemy);
            collideramount++;
        }

        colliders = collidersatoAdd;
    }
}
