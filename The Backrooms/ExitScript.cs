using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    private void OnTriggerExit(Collider collision)
    {
        SceneManager.LoadScene(0);
    }

    //private void OnTriggerExit(Collider other)
    //{
        
    //}
}
