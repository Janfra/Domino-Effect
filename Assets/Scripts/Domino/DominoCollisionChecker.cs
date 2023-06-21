using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DominoCollisionChecker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.CompareTag("Domino Chain"))
        {
            Transform parent = collision.transform.parent;
            if (parent.GetChild(0) == collision.transform)
            {
                Transform child = parent.GetChild(parent.childCount - 1);
                child.parent = null;
                child.AddComponent<DominoCollisionChecker>();
                transform.parent = parent;
                Camera.main.GetComponent<DominoMovement>().SetControlledDomino(child);
                Camera.main.GetComponent<PlaceDominoChain>().AddDominoToSupply(parent.childCount);
                Destroy(parent.gameObject);
            }
        }
    }
}
