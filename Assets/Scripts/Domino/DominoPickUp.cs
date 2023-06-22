using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoPickUp : MonoBehaviour
{
    [SerializeField] private int dominoCount;
    private bool isPickedup = false;

    public void ChangeIsPickedup()
    {
        if (!isPickedup)
        {
            isPickedup = true;
            foreach (Material material in transform.GetChild(0).GetComponent<Renderer>().materials)
            {
                Color color = material.color;
                color.a = 0.5f;
                material.color = color;
            }
            GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
            isPickedup = false;
            foreach (Material material in transform.GetChild(0).GetComponent<Renderer>().materials)
            {
                Color color = material.color;
                color.a = 1.0f;
                material.color = color;
            }
            GetComponent<SphereCollider>().enabled = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Camera.main.GetComponent<PlaceDominoChain>().AddDominoToSupply(dominoCount);
            ChangeIsPickedup();
        }
    }
}
