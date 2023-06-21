using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RotatePickUp : MonoBehaviour
{
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        if (rotateSpeed > 0)
        {
            transform.Rotate(0, rotateSpeed, 0, Space.World);
        }
        else
        {
            Debug.LogError("RotateSpeed is zero");
        }
    }
}
