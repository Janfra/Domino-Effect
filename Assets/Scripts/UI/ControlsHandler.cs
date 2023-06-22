using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject controlsContainer;

    public void ShowControlsMenu()
    {
        controlsContainer.SetActive(true);
    }

    public void HideControlsMenu()
    {
        controlsContainer.SetActive(false);
    }
}
