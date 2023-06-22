using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraMaxDistance;

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.parent.position, transform.position - transform.parent.position, out hit, cameraMaxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point;
        }
    }
}
