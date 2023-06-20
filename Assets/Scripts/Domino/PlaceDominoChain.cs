using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceDominoChain : MonoBehaviour
{
    private bool IsUIEnabled = false;
    [SerializeField]private int numberOfDomino = 1;
    private GameObject DominoChain;
    [SerializeField] private float spaceBetweenDomino;
    [SerializeField] private GameObject dominoToPlace;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeUIShowing();
        }
        if (IsUIEnabled)
        {
            ChangeDominoLength();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceDomino();
                ChangeUIShowing();
            }
        }
    }

    private void PlaceDomino()
    {
        for (int i = 0; i < DominoChain.transform.childCount; i++)
        {
            Transform domino = DominoChain.transform.GetChild(i);
            domino.GetComponent<Rigidbody>().isKinematic = false;
            domino.GetComponent<BoxCollider>().isTrigger = false;
            foreach (Material material in domino.GetComponent<Renderer>().materials)
            {
                Color color = material.color;
                color.a = 1.0f;
                material.color = color;
            }
        }
        DominoChain = null;
    }

    private void ChangeDominoLength()
    {
        numberOfDomino += Mathf.RoundToInt(Input.mouseScrollDelta.y);
        numberOfDomino = Mathf.Clamp(numberOfDomino, 0, 100);

        if (DominoChain.transform.childCount < numberOfDomino)
        {
            InstatiateUIDomino();
        }
        else if (DominoChain.transform.childCount > numberOfDomino)
        {
            Destroy(DominoChain.transform.GetChild(DominoChain.transform.childCount - 1).gameObject);
        }
    }

    private void ChangeUIShowing()
    {
        if (IsUIEnabled)
        {
            IsUIEnabled = false;
            Camera.main.GetComponent<DominoMovement>().isInputEnable = true;
            numberOfDomino = 1;
            Destroy(DominoChain);
        }
        else
        {
            IsUIEnabled = true;
            Camera.main.GetComponent<DominoMovement>().isInputEnable = false;

            DominoChain = new GameObject("DominoChainUI");

            Transform playerTransform = Camera.main.GetComponent<DominoMovement>().GetPlayerTransform();
            for (int i = 0; i < numberOfDomino; i++)
            {
                InstatiateUIDomino();
            }
        }
    }

    private void InstatiateUIDomino()
    {
        Transform playerTransform = Camera.main.GetComponent<DominoMovement>().GetPlayerTransform();
        GameObject newDomino = Instantiate(
                 dominoToPlace,
                 playerTransform.position + playerTransform.forward * (DominoChain.transform.childCount + 1) * spaceBetweenDomino,
                 playerTransform.transform.rotation,
                 DominoChain.transform
                 );
        newDomino.GetComponent<Rigidbody>().isKinematic = true;
        newDomino.GetComponent<BoxCollider>().isTrigger = true;
        foreach (Material material in newDomino.GetComponent<Renderer>().materials)
        {
            Color color = material.color;
            color.a = 0.5f;
            material.color = color;
        }
    }
}
