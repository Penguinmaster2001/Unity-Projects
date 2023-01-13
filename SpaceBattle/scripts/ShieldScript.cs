using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public Transform objToShield;
    public float energy;
    public float rechargeRate;
    public float maxEnergy;
    private CapsuleCollider shield;
    private MeshRenderer shieldMesh;
    private float timeSinceShot;
    public float cooldownTime;

    void Start()
    {
        shield = GetComponent<CapsuleCollider>();
        shieldMesh = GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        timeSinceShot += Time.deltaTime;

        if (timeSinceShot > cooldownTime && energy < maxEnergy)
            energy += Time.deltaTime * rechargeRate;

        if(energy > 0)
        {
            shield.enabled = true;
            shieldMesh.enabled = true;
        }
    }

    void LateUpdate()
    {
        //if (objToShield.gameObject == null)
        //    Destroy(gameObject);

        //transform.SetPositionAndRotation(objToShield.position, objToShield.rotation);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bolt")
        {
            Destroy(collision.gameObject);

            timeSinceShot = 0;
            energy--;
            
            if (energy <= 0)
            {
                shield.enabled = false;
                shieldMesh.enabled = false;
            }
        }
    }
}