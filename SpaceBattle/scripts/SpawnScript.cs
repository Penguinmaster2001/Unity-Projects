using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    //public SpawnBehavior[] spawnBehaviors;
    public float timeUntilStart;
    public float timeUntilSpawns;
    public float timeBetweenSpawns;
    public GameObject commonEnemy;
    public static int currentSpawned;

    void Update()
    {
        if (timeUntilSpawns <= 0 && timeUntilStart <= 0 && currentSpawned <= 5)
        {
            Instantiate(commonEnemy, new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), 0), Quaternion.Euler(0, 0, 0));
            timeUntilSpawns = timeBetweenSpawns;
            currentSpawned++;
        }
        timeUntilStart  -= Time.deltaTime;
        timeUntilSpawns -= Time.deltaTime;
    }
}