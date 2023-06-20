using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<TempMovement>().enabled)
        {
            collider.gameObject.GetComponent<TempMovement>().enabled = false;
            transform.GetChild(0).GetComponent<PipeTrajectoryCalc>().SetObjectToProject(collider.transform);
            collider.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
