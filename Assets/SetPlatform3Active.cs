using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlatform3Active : MonoBehaviour
{
    public GameObject platform3;
    public GameObject vent3;

    // Update is called once per frame
    void Update()
    {
        platform3.SetActive(true);
        vent3.SetActive(true);
    }
}
