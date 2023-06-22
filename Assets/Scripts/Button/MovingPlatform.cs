using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MovingPlatform : MonoBehaviour
{
    public event Action<Vector3> OnPlatformCollision;
    public event Action OnPlatformCollisionEnded;

    private Rigidbody thisRigidbody;
    public Rigidbody Rigidbody => thisRigidbody;
    private List<Transform> blockingPlatforms = new();

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out MovingPlatform isPlatform))
        {
            blockingPlatforms.Add(isPlatform.transform);
            OnPlatformCollision?.Invoke(isPlatform.transform.position);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(blockingPlatforms.Count > 0)
        {
            // Temporary list to avoid editing while updating
            List<Transform> temporaryList = new(blockingPlatforms);
            foreach (Transform transform in temporaryList)
            {
                OnPlatformCollision?.Invoke(transform.position);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out MovingPlatform isPlatform))
        {
            blockingPlatforms.Remove(isPlatform.transform);
            if(blockingPlatforms.Count == 0)
            {
                OnPlatformCollisionEnded?.Invoke();
            }
        }
    }
}
