using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetInputs
{
    public static KeyCode forwardKey = KeyCode.W;
    public static KeyCode leftwardKey = KeyCode.A;
    public static KeyCode rightwardKey = KeyCode.D;
    public static KeyCode backwardKey = KeyCode.S;
    public static KeyCode upwardKey = KeyCode.E;
    public static KeyCode downwardKey = KeyCode.Q;
    public static KeyCode shoot = KeyCode.Mouse0;

    public static KeyCode GetKey()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
            {
                Debug.Log("KeyCode down: " + kcode);
                return kcode;
            }
        }

        //return no input
        return KeyCode.Joystick8Button9;
    }
}