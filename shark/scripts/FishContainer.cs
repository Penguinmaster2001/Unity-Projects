using System.Collections.Generic;
using UnityEngine;

public class FishContainer : MonoBehaviour
{
    [Header("Shark")]
    public GameObject shark;
    public GameObject mouth;

    [Space]
    [Header("Schools")]
    public int schoolAmount;
    public int maxFishPerSchool;
    public int minFishPerSchool;

    public List<School> schools = new List<School>();

    [Space]
    [Header("Fish")]
    public Material[] materials;
    public Mesh[] meshes;
    public GameObject baseFish;

    void Start()
    {
        //CreateSchools();
    }

    void FixedUpdate()
    {
        CreateSchools();

        UpdateSchools();
    }

    #region
    void CreateSchools()
    {
        for(int i = schools.Count; i < schoolAmount; i++)
        {
            School newSchool = new School();
            newSchool.InitalizeSchool(Random.Range(minFishPerSchool, maxFishPerSchool), materials[Random.Range(0, materials.Length)], meshes[Random.Range(0, meshes.Length)], baseFish, shark.transform.position, this);
            schools.Add(newSchool);
        }
    }

    void UpdateSchools()
    {
        for(int i = 0; i < schools.Count; i++)
        {
            schools[i].UpdateSchool(shark, mouth.transform.position);
        }
    }

    public void RemoveSchool(School toRemove)
    {
        schools.Remove(toRemove);
    }
    #endregion

    #region
    public void Thread()
    {
        UpdateSchools();
    }
    #endregion
}