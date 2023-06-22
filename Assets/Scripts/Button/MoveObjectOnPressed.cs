using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MoveObjectOnPressed : OnPressedEvent
{
    [SerializeField]
    private List<SimpleMoveConfig> movingConfiguration;
    private bool isActive;
    private bool isResetting;

    [System.Serializable]
    private class SimpleMoveConfig
    {
        private const float BLOCK_DISTANCE = 0.05f;

        [Header("Dependencies")]
        public BaseMovablePlatform platform;

        [Header("Configuration")]
        public Vector3 targetPosition;
        public bool isPingPong;
        [SerializeField]
        private float durationToTarget = 1.0f;
        [SerializeField]
        private float durationToReset = 1.0f;

        [HideInInspector]
        public Vector3 startPosition;
        private float timeBlocked = 0.0f;
        private bool isBlocked;
        private bool isComplete;
        public bool IsComplete => isComplete;

        public void SetStartPosition()
        {
            if (platform == null) return;

            startPosition = platform.GetTransform().position;
            platform.OnCollision += OnCollision;
            platform.OnEndCollision += CancelBlock;
            Debug.Log($"{startPosition}");
        }

        public void SetNewPosition(float time)
        {
            Vector3 newPosition = new();
            time -= timeBlocked;

            if (isPingPong)
            {
                newPosition = Vector3.Lerp(startPosition, targetPosition + startPosition, Mathf.PingPong(time / durationToTarget, 1));
            }
            else
            {
                float progress;
                isComplete = GetLerpProgress(out progress, time);
                newPosition = Vector3.Lerp(startPosition, targetPosition + startPosition, progress);
            }

            if (!isBlocked)
            {
                platform.GetRigidbody().MovePosition(newPosition);
            }
            else
            {
                timeBlocked += Time.deltaTime;
                isComplete = false;
            }
        }

        public void ResetBackToStart(float time)
        {
            float progress;
            isComplete = GetLerpProgress(out progress, time);
            platform.GetRigidbody().MovePosition(Vector3.Lerp(platform.GetTransform().position, startPosition, progress));
        }

        public void RestartCompletedState()
        {
            timeBlocked = 0.0f;
            isComplete = false;
        }

        private bool CheckForBlock(Vector3 position)
        {
            Vector3 currentPosition = platform.GetTransform().position;
            // Closest point, multiplying by 1000 to get closest point outside, otherwise it returns position inside collider. Will not work with objects bigger than that
            Vector3 closestPoint = platform.GetRigidbody().ClosestPointOnBounds(position);
            Vector3 directionToCollision = (closestPoint - currentPosition);
            float distanceFromClosestBound = BLOCK_DISTANCE + directionToCollision.magnitude;
            return Physics.Raycast(currentPosition, directionToCollision, distanceFromClosestBound);
        }

        private void CancelBlock()
        {
            isBlocked = false;
        }

        private void OnCollision(Vector3 collisionPosition)
        {
            Debug.Log("Collided with platform");

            if (CheckForBlock(collisionPosition))
            {
                isBlocked = true;
                timeBlocked += Time.deltaTime;
                isComplete = false;
            }
            else
            {
                isBlocked = false;
            }
        }

        private bool GetLerpProgress(out float progress, float time)
        {
            progress = Mathf.Clamp01(time / durationToReset);
            return progress == 1;
        }
    }

    private void Start()
    {
        buttonCaller.OnPressed += MoveObjects;
        buttonCaller.OnReleased += MoveObjectsBack;

        SetStartPosition();
    }

    private void SetStartPosition()
    {
        foreach (SimpleMoveConfig configuration in movingConfiguration)
        {
            configuration.SetStartPosition();
        }
    }

    private void MoveObjects()
    {
        isActive = true;
        isResetting = false;
        StartCoroutine(PingPongObjectsPosition());
    }

    private void MoveObjectsBack()
    {
        isActive = false;
        StartCoroutine(ResetObjectsPosition());
    }

    private IEnumerator PingPongObjectsPosition()
    {
        float time = 0.0f;
        yield return null;

        while (isActive)
        {
            time += Time.deltaTime;
            foreach (SimpleMoveConfig configuration in movingConfiguration)
            {
                if (configuration.IsComplete) continue;
                configuration.SetNewPosition(time);
            }
            yield return null;
        }

        RestartCompletedState();
        yield return null;
    }

    private IEnumerator ResetObjectsPosition()
    {
        isResetting = true;
        float time = 0.0f;
        List<SimpleMoveConfig> objectThatHaveBeenReset = new();
        yield return null;

        while (movingConfiguration.Count > 0 && isResetting)
        {
            time += Time.deltaTime;
            foreach (SimpleMoveConfig configuration in movingConfiguration)
            {
                if (configuration.IsComplete)
                {
                    objectThatHaveBeenReset.Add(configuration);
                    continue;
                };
                configuration.ResetBackToStart(time);
            }

            yield return null;
            foreach (SimpleMoveConfig configuration in objectThatHaveBeenReset)
            {
                movingConfiguration.Remove(configuration);
            }
            yield return null;
        }

        // If cancel add them back, if not just repopulate it
        if (isResetting != false)
        {
            movingConfiguration = new(objectThatHaveBeenReset);
        }
        else
        {
            foreach (SimpleMoveConfig configuration in objectThatHaveBeenReset)
            {
                movingConfiguration.Add(configuration);
            }
            Debug.Log("All moved back to start");
        }
        isResetting = false;
        RestartCompletedState();
        yield return null;
    }

    private void RestartCompletedState()
    {
        foreach (SimpleMoveConfig configuration in movingConfiguration)
        {
            configuration.RestartCompletedState();
        }
    }

    private void OnDrawGizmos()
    {
        if (movingConfiguration == null || movingConfiguration.Count == 0) return;

        foreach (SimpleMoveConfig configuration in movingConfiguration)
        {
            if (configuration.platform == null) return;

            if (Application.isPlaying)
            {
                Gizmos.DrawLine(configuration.startPosition, configuration.targetPosition + configuration.startPosition);
            }
            else
            {
                Gizmos.DrawLine(configuration.platform.GetTransform().position, configuration.targetPosition + configuration.platform.GetTransform().position);
            }
        }
    }
}

public interface IMovablePlatform
{
    public event Action<Vector3> OnCollision;
    public event Action OnEndCollision;

    Rigidbody GetRigidbody();
    Transform GetTransform();
}
