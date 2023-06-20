using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RespawnPoint : MonoBehaviour
{
    private static Transform objectToRespawn;
    private static RespawnPoint currentRespawnPoint;

    [SerializeField]
    private Vector3 respawnPosition;

    private void Awake()
    {
        respawnPosition += transform.position;
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("New respawn point");
            objectToRespawn = other.transform;
            currentRespawnPoint = this;
        }
    }

    public static void RespawnPlayer()
    {
        if (objectToRespawn == null || currentRespawnPoint == null) return;

        objectToRespawn.position = currentRespawnPoint.respawnPosition;
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.4f;
        Gizmos.color = color;

        if (Application.isPlaying)
        {
            Gizmos.DrawCube(respawnPosition, Vector3.one / 2);
        }
        else
        {
            Gizmos.DrawCube(respawnPosition + transform.position, Vector3.one / 2);
        }
    }
}
