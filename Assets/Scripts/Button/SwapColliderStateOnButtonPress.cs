using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SwapColliderStateOnButtonPress : OnPressedEvent
{
    [Header("Dependencies")]
    [SerializeField]
    private List<Collider> collidersToSwapState;

    private void Start()
    {
        buttonCaller.OnPressed += SwapCurrentState;
        buttonCaller.OnReleased += SwapCurrentState;
    }

    private void OnDisable()
    {
        buttonCaller.OnPressed -= SwapCurrentState;
        buttonCaller.OnReleased -= SwapCurrentState;
    }

    private void SwapCurrentState() 
    {
        Debug.Log($"{gameObject.name} has swap all given colliders state, either from on to off or off to on");
        foreach (Collider collider in collidersToSwapState)
        {
            if(collider == null)
            {
                Debug.LogError($"One of the colliders inside {gameObject.name} is null, check colliders to swap");
                continue;
            }

            collider.enabled = !collider.enabled;
        }
    }
}
