using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceDominoChain : MonoBehaviour
{
    private bool IsUIEnabled = false;
    private int numberOfDomino = 1;
    private GameObject DominoChain;
    [SerializeField]private TMP_Text dominoSupplyText;
    [SerializeField] private int dominoSupply;
    [SerializeField] public float spaceBetweenDomino;
    [SerializeField] private GameObject dominoToPlace;

    private void Start()
    {
        if (dominoSupply > 0)
        {
            dominoSupplyText.text = ("Domino Supply : " + dominoSupply);
        }
    }

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
        if (DominoChain.transform.childCount > 0)
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
            DominoChain.AddComponent<DominoChain>().percentGiveBetweenDomino = 5.0f;
            SubtractDominoFromSupply(DominoChain.transform.childCount);
            DominoChain = null;
        }
    }

    private void ChangeDominoLength()
    {
        numberOfDomino += Mathf.RoundToInt(Input.mouseScrollDelta.y);
        numberOfDomino = Mathf.Clamp(numberOfDomino, 0, dominoSupply);

        Transform playerTransform = Camera.main.GetComponent<DominoMovement>().GetPlayerTransform();

        if (DominoChain.transform.childCount < numberOfDomino && 
            !Physics.Raycast(playerTransform.position, playerTransform.forward, (DominoChain.transform.childCount + 1) * spaceBetweenDomino,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
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

            DominoChain = new GameObject("DominoChain");
            DominoChain.transform.tag = "Domino Chain";

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

    public void AddDominoToSupply(int domino)
    {
        dominoSupply += domino;
        dominoSupplyText.text = "Domino Supply : " + dominoSupply;
    }

    public void SubtractDominoFromSupply(int domino)
    {
        dominoSupply -= domino;
        if (dominoSupply < 0) dominoSupply = 0;
        dominoSupplyText.text = "Domino Supply : " + dominoSupply;
    }
}
