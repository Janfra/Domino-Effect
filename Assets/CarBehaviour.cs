using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public Transform carDestroyPoint;
    public Transform carSpawnPointLocation;
    public float carSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        carDestroyPoint = GameObject.FindWithTag("DestroyPoint").GetComponent<Transform>();
        carSpawnPointLocation = GameObject.FindWithTag("CarSpawnPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, carDestroyPoint.transform.position, carSpeed * Time.deltaTime);

        if (transform.position == carDestroyPoint.transform.position)
        {
            transform.position = carSpawnPointLocation.transform.position;
        }
    }
}
