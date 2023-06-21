using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SpawnBus : MonoBehaviour
{
    public GameObject bus;
    public Transform busSpawnPoint;
    public bool isActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Instantiate(bus, busSpawnPoint.transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (isActive == true)
        {
            Instantiate(bus, busSpawnPoint.transform.position, new Quaternion(0, 180, 0, 0));
            isActive = false;
        }
    }
}
