using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void GetForwardKey()
    {
        KeyCode kcode = GetInputs.GetKey();

        if (kcode != KeyCode.Joystick8Button9)
        {
            GetInputs.forwardKey = kcode;
        }
    }

    public void GetBackwardKey()
    {
        KeyCode kcode = GetInputs.GetKey();

        if (kcode != KeyCode.Joystick8Button9)
        {
            GetInputs.backwardKey = kcode;
        }
    }

    public void GetLeftKey()
    {
        KeyCode kcode = GetInputs.GetKey();

        if (kcode != KeyCode.Joystick8Button9)
        {
            GetInputs.leftwardKey = kcode;
        }
    }

    public void GetRightKey()
    {
        KeyCode kcode = GetInputs.GetKey();

        if (kcode != KeyCode.Joystick8Button9)
        {
            GetInputs.rightwardKey = kcode;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
