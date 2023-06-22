using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlatform2Active : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject platform2;
    public GameObject vent2;

    // Update is called once per frame
    void Update()
    {
        platform2.SetActive(true);
        vent2.SetActive(true);
    }
}
