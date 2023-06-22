using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovablePlatform : MonoBehaviour, IMovablePlatform
{
    public virtual event Action<Vector3> OnCollision;
    public virtual event Action OnEndCollision;

    public abstract Rigidbody GetRigidbody();

    public abstract Transform GetTransform();

    protected void CallOnCollision(Vector3 collisionPosition)
    {
        OnCollision?.Invoke(collisionPosition);
    }

    protected void CallOnEndCollision()
    {
        OnEndCollision?.Invoke();
    }
}
