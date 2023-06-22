using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlatformActive : MonoBehaviour
{
    public GameObject platform1;
    public GameObject vent1;

    // Update is called once per frame
    void Update()
    {
        platform1.SetActive(true);
        vent1.SetActive(true);
    }
}
