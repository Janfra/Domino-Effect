using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carSpawnLocation, carPrefab;

    private void Start()
    {
        Instantiate(carPrefab, carSpawnLocation.transform.position, Quaternion.identity);
    }
}
