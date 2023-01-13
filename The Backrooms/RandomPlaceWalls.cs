using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPlaceWalls : MonoBehaviour
{
    public int boxNum;
    public int artiNum;
    public int enemyNum;

    public GameObject[] rooms;
    public GameObject artifactBox;
    public GameObject artifact;
    public GameObject enemy;
    public GameObject player;
    public GameObject door;
    public GameObject desk;

    void Start()
    {

        for (float x = -4.5f; x < 5.5f; x++)
        {
            for (float y = -4.5f; y < 5.5f; y++)
            {
                GameObject newRoom = Instantiate(rooms[Random.Range(0, rooms.Length)],
                    new Vector3(25 * x, 2, 25 * y),
                    new Quaternion(0, 0, 0, 0));
                newRoom.GetComponentInChildren<LightFlicker>().player = player;
            }
        }

        InstantiateNewObject(desk, 0, 5, 25, 5, 0);

        InstantiateNewObject(artifactBox, boxNum, 5, 12, 6, 2);

        InstantiateNewObject(artifact, artiNum, 5, 12, 5, 0);

        InstantiateNewObject(enemy, enemyNum, 5, 12, 5, 1);
    }

    void InstantiateNewObject(GameObject objectToInstantiate, int amount, float areaMultiplyer, float area, float offset, int extra)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject newObject = Instantiate(objectToInstantiate,
                    new Vector3(areaMultiplyer * Mathf.RoundToInt(Random.Range(-area, area)) + offset,
                        1,
                        areaMultiplyer * Mathf.RoundToInt(Random.Range(-area, area)) + offset),
                    new Quaternion(0, 0, 0, 0));

            switch (extra)
            {
                case 1:
                    newObject.GetComponent<EnemyScript>().target = player;
                    break;

                case 2:
                    newObject.GetComponent<BoxScript>().door = door;
                    break;

                default:
                    break;
            }
        }
    }
}