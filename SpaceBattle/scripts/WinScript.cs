using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    public GameObject[] destroyObjective;
    int d;
    int p;
    public GameObject[] protectObjective;
    public Text winText;

    private void Update()
    {
        CheckForWin();
        CheckForLose();
    }

    void CheckForWin()
    {
        foreach (GameObject obj in destroyObjective)
        {
            if (obj != null)
                return;
        }

        winText.text = "You Won";

        GameObject[] objs = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objs)
        {
            if(obj.tag.StartsWith("E"))
            {
                if(obj.TryGetComponent(out AIScript ai))
                {
                    ai.Warp();
                }
                else if(obj.TryGetComponent(out LargeShipScript lg))
                {
                    lg.Warp();
                }
            }
        }
    }

    void CheckForLose()
    {
        foreach (GameObject obj in protectObjective)
        {
            if (obj != null)
                return;
        }

        winText.text = "You Lost";

        GameObject[] objs = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objs)
        {
            if(obj.tag.StartsWith("A"))
            {
                if(obj.TryGetComponent(out AIScript ai))
                {
                    ai.Warp();
                }
                else if(obj.TryGetComponent(out LargeShipScript lg))
                {
                    lg.Warp();
                }
            }
        }
    }
}