using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && Camera.main.GetComponent<DominoMovement>().isInputEnable)
        {
            Camera.main.GetComponent<DominoMovement>().isInputEnable = false;
            transform.GetChild(0).GetComponent<PipeTrajectoryCalc>().SetObjectToProject(collider.transform);
            collider.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
