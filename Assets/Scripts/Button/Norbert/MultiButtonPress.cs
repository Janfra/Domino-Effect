using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiButtonPress : MonoBehaviour
{
    [SerializeField] private List<ButtonState> buttonStates;

    private void Update()
    {
        bool isAllButtonsPressed = true;
        foreach (ButtonState state in buttonStates)
        {
            if (!state.GetIsPressed())
            {
                isAllButtonsPressed = false;
            }
        }
        if (isAllButtonsPressed)
        {
            transform.GetComponent<Collider>().enabled = true;
        }
        else
        {
            transform.GetComponent<Collider>().enabled = false;
        }
    }
}
