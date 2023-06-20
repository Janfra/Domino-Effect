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
                GameObject newDomino = Instantiate(
                     dominoToPlace,
                     playerTransform.position + playerTransform.forward * (1 + i) * spaceBetweenDomino,
                     playerTransform.transform.rotation,
                     DominoChain.transform
                     );
                newDomino.GetComponent<Rigidbody>().isKinematic = true;
                newDomino.GetComponent<BoxCollider>().isTrigger = true;
                for (int j = 0; j < newDomino.GetComponent<Renderer>().materials.Length; j++)
                {
                    Color color = newDomino.GetComponent<Renderer>().materials[j].color;
                    color.a = 0.5f;
                    newDomino.GetComponent<Renderer>().materials[j].color = color;
                }
            }
        }
    }
}
