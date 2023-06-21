using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusBehaviour : MonoBehaviour
{
    public Transform busDestroyPoint;
    public Transform busSpawnPointLocation;
    public float busSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        busDestroyPoint = GameObject.FindWithTag("BusDestroyPoint").GetComponent<Transform>();
        busSpawnPointLocation = GameObject.FindWithTag("BusSpawnPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, busDestroyPoint.transform.position, busSpeed * Time.deltaTime);

        if (transform.position == busDestroyPoint.transform.position)
        {
            Destroy(this.gameObject);
        }
    }
}
