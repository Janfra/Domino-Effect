using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoChain : MonoBehaviour
{
    private float percent;
    public float percentGiveBetweenDomino
    {
        set { percent = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLinkage();
    }

    private void CheckLinkage()
    {
        for (int i = 0; i < (transform.childCount - 1); i++)
        {
            if ((transform.GetChild(i).position - transform.GetChild(i + 1).position).magnitude
                > (Camera.main.GetComponent<PlaceDominoChain>().spaceBetweenDomino * (1.0f + (percent / 100.0f))))
            {
                Camera.main.GetComponent<PlaceDominoChain>().AddDominoToSupply(transform.childCount);
                Destroy(gameObject);
            }
        }
    }
}
