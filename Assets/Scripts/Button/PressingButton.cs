using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PressingButton : MonoBehaviour
{
    public event Action OnPressed;
    public event Action OnReleased;
    private int numberOfObjectsColliding;

    private void Awake()
    {
        numberOfObjectsColliding = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        numberOfObjectsColliding++;

        // Call only with the first collision
        if(numberOfObjectsColliding == 1)
        {
            OnPressed?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        numberOfObjectsColliding++;

        if(numberOfObjectsColliding == 1)
        {
            OnPressed?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numberOfObjectsColliding--;

        if(numberOfObjectsColliding <= 0)
        {
            OnReleased?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        numberOfObjectsColliding--;
       
        // Call when all objects collided are gone
        if(numberOfObjectsColliding <= 0)
        {
            OnReleased?.Invoke();
        }
    }

    private void OnDisable()
    {
        OnPressed = null;
        OnReleased = null;
    }
}
