using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PressingButton))]
public class OnPressedEvent : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    protected PressingButton buttonCaller;

    protected virtual void Awake()
    {
        if (buttonCaller == null)
        {
            Debug.LogWarning($"No caller set for {gameObject.name} in the pressed event");
            buttonCaller = GetComponent<PressingButton>();
        }
    }
}
