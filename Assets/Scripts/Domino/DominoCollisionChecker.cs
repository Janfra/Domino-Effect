using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DominoCollisionChecker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.CompareTag("Domino Chain"))
        {
            Transform parent = collision.transform.parent;
            if (parent.GetChild(0) == collision.transform)
            {
                Transform player = collision.contacts[0].thisCollider.transform;
                Transform child = parent.GetChild(parent.childCount - 1);
                child.parent = transform;
                child.AddComponent<DominoCollisionChecker>();
                player.parent = parent;
                Camera.main.GetComponent<DominoMovement>().SetControlledDomino(child);
                Camera.main.GetComponent<PlaceDominoChain>().AddDominoToSupply(parent.childCount);
                Destroy(parent.gameObject);
            }
        }
    }
}
