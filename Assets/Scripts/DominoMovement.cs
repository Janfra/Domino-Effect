using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DominoMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody dominoRigidbody;
    private Transform dominoTransform;

    [Header("Config")]
    [SerializeField]
    private bool isInputEnable;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;

    private Vector3 movementDirection;
    private bool isMoving;

    private void Start()
    {
        if(dominoRigidbody == null)
        {
            Debug.LogError("No domino set for movement");
        }
        else
        {
            dominoTransform = dominoRigidbody.transform;
        }
    }

    private void Update()
    {
        UpdateInput();
        RotateToMovementDirection();
        transform.LookAt(dominoTransform);
    }

    private void FixedUpdate()
    {
        TryMoveDomino();
    }

    private void UpdateInput()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        float zDirection = Input.GetAxisRaw("Vertical");
        
        if(zDirection != 0 && isInputEnable)
        {
            movementDirection = new(xDirection, 0, zDirection);
            isMoving = true;
        }
        else
        {
            movementDirection = new();
            isMoving = false;
        }
    }

    private void TryMoveDomino()
    {
        if (isMoving)
        {
            Vector3 velocity = new();
            if(movementDirection.z > 0)
            {
                velocity = dominoTransform.forward.normalized * Time.fixedDeltaTime * speed;
            }
            else
            {
                velocity = -dominoTransform.forward.normalized * Time.fixedDeltaTime * speed;
            }

            dominoRigidbody.velocity = new(velocity.x, dominoRigidbody.velocity.y, velocity.z);
        }
    }

    private void RotateToMovementDirection()
    {
        if (isMoving)
        {
            dominoTransform.rotation = Quaternion.RotateTowards(dominoTransform.rotation, GetFacingDirection(), rotationSpeed * Time.deltaTime);
        }
    }

    private Quaternion GetFacingDirection()
    {
        return Quaternion.LookRotation(GetMovementDirection().normalized);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 dominoPosition = dominoTransform.position;

        // Get direction of the movement based on position
        Vector3 vectorPointingToDirection = new();
        vectorPointingToDirection = (new Vector3(movementDirection.normalized.x, 0, 1) + dominoPosition) - dominoPosition;

        // Change it from global direction to match the object rotation
        vectorPointingToDirection = dominoTransform.rotation * vectorPointingToDirection;

        return vectorPointingToDirection;
    }

    private void OnDrawGizmos()
    {
        if (movementDirection != null && dominoTransform != null)
        {
            Vector3 dominoPosition = dominoTransform.position;

            // Get direction of the movement based on position
            Vector3 directionToMovement = (movementDirection.normalized + dominoPosition) - dominoPosition;

            // Change it from global direction to match the object rotation
            directionToMovement = dominoTransform.rotation * directionToMovement;

            Gizmos.DrawLine(dominoTransform.position, directionToMovement.normalized * 10);
        }
    }
}
