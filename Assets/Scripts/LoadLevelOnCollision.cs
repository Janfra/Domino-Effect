using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LoadLevelOnCollision : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }
    }
}
