using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int boxesLeft;

    private int playSound = 0;

    public GameObject wallPlacer;

    public AudioClip chimeShort;
    public AudioClip chimeLong;

    void Start()
    {
        boxesLeft = wallPlacer.GetComponent<RandomPlaceWalls>().boxNum;
    }
    
    void Update()
    {
        if (playSound > 1 && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = chimeShort;
            GetComponent<AudioSource>().Play();

            playSound--;
        }

        if (playSound == 1 && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = chimeLong;
            GetComponent<AudioSource>().Play();

            playSound--;
        }
    }

    void boxActivated()
    {
        boxesLeft--;
        playSound += boxesLeft;
    }

    void Open()
    {
        if (boxesLeft <= 0)
            Destroy(gameObject);
    }
}