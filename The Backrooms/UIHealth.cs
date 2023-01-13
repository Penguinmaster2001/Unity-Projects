using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 255 - player.GetComponent<PlayerControl>().health);
    }
}
