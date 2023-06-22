using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonState : MonoBehaviour
{
    private bool isPressed = false;
    private int objectsOnButton = 0;

    private void OnCollisionEnter(Collision collision)
    {
        objectsOnButton++;

        if (objectsOnButton > 0)
        {
            isPressed = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        objectsOnButton--;

        if (objectsOnButton <= 0)
        {
            isPressed = false;
        }
    }

    public bool GetIsPressed()
    {
        return isPressed;
    }
}
