using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(PressingButton))]
public class SwapColliderStateOnButtonPress : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PressingButton caller;
    [SerializeField]
    private List<Collider> collidersToSwapState;

    private void Awake()
    {
        if(caller == null)
        {
            Debug.LogWarning($"No caller set for {gameObject.name} in the disable collider");
            caller = GetComponent<PressingButton>();
        }
    }

    private void Start()
    {
        caller.OnPressed += SwapCurrentState;
        caller.OnReleased += SwapCurrentState;
    }

    private void OnDisable()
    {
        caller.OnPressed -= SwapCurrentState;
        caller.OnReleased -= SwapCurrentState;
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
