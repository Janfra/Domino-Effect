using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DominoMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody dominoRigidbody;
    [SerializeField]
    private Transform dominoTransform;
    [SerializeField]
    private Transform dominoBodyTransform;

    [Header("Config")]
    [SerializeField]
    public bool isInputEnable = true;
    [SerializeField]
    private float speed = 100;
    [SerializeField]
    private float rotationSpeed = 100;
    [SerializeField]
    private float dominoRotationMultiplier = 1.6f;

    #region Movement

    private Vector3 movementDirection;
    private float lastNonZeroMovementDirection = 1;
    private bool isMoving;

    #endregion

    #region Domino Body Rotation

    private bool isRotating;
    private Vector3 rotationEuler;
    private float xRotationTarget;
    private float xRotationStart;
    private float timer;

    #endregion

    private void Start()
    {
        if(dominoRigidbody == null)
        {
            Debug.LogError("No domino set for movement");
        }
        else
        {
            SetControlledDomino(dominoRigidbody);
        }
        xRotationStart = 0;
        xRotationTarget = 90;
    }

    private void Update()
    {
        UpdateInput();
        RotateDominoBody();
        RotateToFaceMovementDirection();
        transform.LookAt(dominoTransform);
    }

    private void FixedUpdate()
    {
        TryMoveDomino();
    }

    /// <summary>
    /// Updates the domino being moved
    /// </summary>
    /// <param name="dominoRigidbody"></param>
    public void SetControlledDomino(Rigidbody dominoRigidbody)
    {
        this.dominoRigidbody = dominoRigidbody;
        this.dominoTransform = dominoRigidbody.transform;
        this.dominoTransform.gameObject.tag = "Player";
    }

    /// <summary>
    /// Updates the movement to match inputs
    /// </summary>
    private void UpdateInput()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        float zDirection = Input.GetAxisRaw("Vertical");

        if (isInputEnable)
        {
            if(zDirection != 0)
            {
                if (!isRotating)
                {
                    lastNonZeroMovementDirection = zDirection;
                }
                movementDirection = new(xDirection, 0, zDirection);
                isMoving = true;
            }
            else if (xDirection != 0 && isRotating)
            {
                movementDirection = new(xDirection, 0, lastNonZeroMovementDirection);
                isMoving = false;
            }
            else
            {
                movementDirection = new();
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Attempts to move the domino to the given input
    /// </summary>
    private void TryMoveDomino()
    {
        if (isRotating)
        {
            Vector3 velocity = new();
            if(lastNonZeroMovementDirection > 0)
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

    /// <summary>
    /// Rotate the domino to face the movement direction at rotation speed
    /// </summary>
    private void RotateToFaceMovementDirection()
    {
        if (isRotating)
        {
            dominoTransform.rotation = Quaternion.RotateTowards(dominoTransform.rotation, GetFacingDirection(), rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Get a quaternion with the forward direction to rotate to
    /// </summary>
    /// <returns></returns>
    private Quaternion GetFacingDirection()
    {
        return Quaternion.LookRotation(GetMovementDirection().normalized);
    }

    /// <summary>
    /// Get the movement direction based on the domino rotation
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Rotates the domino body for movement and matching forward rotation
    /// </summary>
    private void RotateDominoBody()
    {
        if (isMoving || isRotating)
        {
            timer += Time.deltaTime * dominoRotationMultiplier;
            timer = Mathf.Clamp01(timer);
            rotationEuler.x = Mathf.Lerp(xRotationStart, xRotationTarget, timer);
            isRotating = true;
            if(timer == 1)
            {
                isRotating = false;
                xRotationStart = xRotationTarget;
                xRotationTarget += 90 * lastNonZeroMovementDirection;
                timer = 0.0f;
            }
        }

        rotationEuler.y = dominoTransform.rotation.eulerAngles.y;
        rotationEuler.z = dominoTransform.rotation.eulerAngles.z;
        Quaternion rotationToSet = Quaternion.Euler(rotationEuler);
        dominoBodyTransform.rotation = rotationToSet;
    }

    #region Getters & Setters

    public Rigidbody GetPlayerRigidbody()
    {
        return dominoRigidbody;
    }

    public Transform GetPlayerTransform()
    {
        return dominoTransform;
    }

    #endregion
}
