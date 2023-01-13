using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent agent;
    private bool lighton;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        agent.destination = target.transform.position;

        lighton = target.GetComponent<PlayerControl>().on;

        if (!lighton)
        {
            GetComponent<ParticleSystem>().Play(!GetComponent<ParticleSystem>().isPlaying);

            if(!GetComponent<ParticleSystem>().isPlaying && Random.Range(0, Mathf.RoundToInt(distance / 2)) == 0 || distance <= 10)
                transform.position = new Vector3Int(5 * Mathf.RoundToInt(Random.Range(-50, 50)) + 5, 1, 5 * Mathf.RoundToInt(Random.Range(-50, 50)));
        }

        if (distance <= 10)
            target.GetComponent<PlayerControl>().health -= distance / 10;

        //if (distance > 100)
        //    target.GetComponent<PlayerControl>().health -= distance / 10;
    }
}