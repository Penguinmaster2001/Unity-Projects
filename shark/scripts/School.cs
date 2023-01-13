//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School
{
    List<Fish> fishes = new List<Fish>();
    Vector3 target = new Vector3();
    FishContainer fc;

    float size = Random.Range(0.5f, 2.0f);
    float spawnDistance = 100;

    public void InitalizeSchool(int fishAmount, Material fishMaterial, Mesh fishMesh, GameObject baseFish, Vector3 sharkPos, FishContainer fc)
    {
        Vector3 SpawnPos;
        SpawnPos = (Random.insideUnitSphere * spawnDistance) + sharkPos;
        if (SpawnPos.y > UniversalVariables.waterHeight)
        {
            SpawnPos.y = UniversalVariables.waterHeight - 5;
        }

        this.fc = fc;
        for(int i = 0; i < fishAmount; i++)
        {
            Fish newFish = new Fish();

            newFish.InitalizeFish(fishMaterial, fishMesh, baseFish, 2f, size, this);

            newFish.position = SpawnPos + (Random.insideUnitSphere * 2);

            fishes.Add(newFish);
        }

        SetNewTarget(SpawnPos);
    }

    public void UpdateSchool(GameObject shark, Vector3 mouthPos)
    {
        Vector3 avePos = new Vector3();
        Vector3 aveVel = new Vector3();

        //get average position and velocity of whole school
        foreach(Fish fish in fishes)
        {
            avePos += fish.position;
            aveVel += fish.velocity;
        }
        avePos /= fishes.Count;
        aveVel /= fishes.Count;

        if(Vector3.Distance(shark.transform.position, avePos) > spawnDistance)
        {
            for(int i = 0; i < fishes.Count; i++)
            {
                fishes[i].alive = false;
                GameObject.Destroy(fishes[i].thisFish);
                KillFish(fishes[i]);
            }
        }
        
        if(Vector3.Distance(target, avePos) < 2)
        {
            SetNewTarget(avePos);
        }

        Debug.DrawLine(avePos, target, Color.yellow);

        for(int i = 0; i < fishes.Count; i++)
        {
            if(fishes[i].alive)
            {
                Vector3 aveDist = new Vector3();

                foreach(Fish otherFish in fishes)
                {
                    if(fishes[i] != otherFish)
                    {
                        //for separation only
                        float distance = Vector3.Distance(fishes[i].position, otherFish.position);
                        Vector3 difference = fishes[i].position - otherFish.position;
                        difference /= Mathf.Pow(distance, 3) * 2;
                        aveDist += difference;
                    }
                }
                aveDist /= fishes.Count - 1;

                fishes[i].UpdateFish(Time.deltaTime, avePos, aveVel, aveDist, target, shark, mouthPos, 1, 1, 1, 1, 5, 5);
            }
            else
            {
                KillFish(fishes[i]);
            }
        }
    }

    public void KillFish(Fish toKill)
    {
        fishes.Remove(toKill);
        if(fishes.Count == 0)
        {
            fc.RemoveSchool(this);
        }
    }

    void SetNewTarget(Vector3 avePos)
    {
        target = avePos + (Random.insideUnitSphere * 20);
        while(target.y > 30 || Physics.Linecast(avePos, target))
        {
            Debug.DrawLine(avePos, target, Color.blue, 10);

            target = avePos + (Random.insideUnitSphere * 20);
        }
    }
}