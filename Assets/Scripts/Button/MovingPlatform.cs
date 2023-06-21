using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MovingPlatform : MonoBehaviour
{
    public event Action<Vector3> OnPlatformCollision;
    public event Action OnPlatformCollisionEnded;

    private Rigidbody thisRigidbody;
    public Rigidbody Rigidbody => thisRigidbody;
    private List<Transform> blockingPlatforms = new();
    [SerializeField]
    private MoveOrganizer organizer;

    [System.Serializable]
    protected class MoveOrganizer
    {
        [SerializeField]
        private List<MovingConfiguration> configurations;
        private List<MovingConfiguration> activeMoves;
        private List<MovingConfiguration> readyForResetMoves;
        private bool isActive = false;
        private float moveTime = 0.0f;
        private Vector3 startPosition;
        private MovingConfiguration currentHighestPriority;

        public List<MovingConfiguration> ConfigurationsData => configurations;

        public void AddToMoving(MovingConfiguration configuration, MovingPlatform caller)
        {
            if (activeMoves.Contains(configuration))
            {
                return;
            }
            activeMoves.Add(configuration);
            configuration.OnStartMove(moveTime);
            if (!isActive)
            {
                caller.StartCoroutine(MoveActiveConfigurations());
            }
        }

        private void RemoveFromMoving(MovingConfiguration configuration)
        {
            activeMoves.Remove(configuration);
            readyForResetMoves.Remove(configuration);
        }

        public void AddToReadyToReset(MovingConfiguration configuration)
        {
            if (!readyForResetMoves.Contains(configuration))
            {
                readyForResetMoves.Add(configuration);
            }
        }

        private IEnumerator MoveActiveConfigurations()
        {
            yield return null;
            while(activeMoves.Count > 0)
            {
                // Sorting them by priority
                List<MoveSortData> orderOfUpdate;
                GetOrderOfUpdate(out orderOfUpdate);

                // Move platforms with active configs
                Vector3 startPositionOffset = MovePlatform(orderOfUpdate.ToArray());

                // Check for objects that can be resetted by priority
                if(readyForResetMoves.Count > 0)
                {
                    CheckForReset(orderOfUpdate, startPosition + startPositionOffset);
                }
                yield return null;
            }

            isActive = false;
            yield return null;
        }

        private Vector3 MovePlatform(MoveSortData[] orderOfUpdate)
        {
            moveTime += Time.deltaTime;
            Vector3 startPositionOffset = Vector3.zero;
            foreach (MoveSortData sortedData in orderOfUpdate)
            {
                MovingConfiguration config = activeMoves[sortedData.index];
                if (config.IsComplete)
                {
                    startPositionOffset += config.GetPositionToAdd(startPosition + startPositionOffset, moveTime);
                    continue;
                }
                startPositionOffset += config.MovePlatform(startPosition + startPositionOffset, moveTime);
            }

            return startPositionOffset;
        }

        private void CheckForReset(List<MoveSortData> priorityOrderData, Vector3 positionToMoveTo)
        {
            SetHighestPriority(activeMoves[priorityOrderData[0].index]);
            // If the lowest priority can be reset, start resetting, otherwise it cannot be reset yet.
            for (int i = priorityOrderData.Count - 1; i > 0; i--)
            {
                MoveSortData sortData = priorityOrderData[i];
                MovingConfiguration currentLowestPriorityConfiguration = activeMoves[sortData.index];
                if (readyForResetMoves.Contains(currentLowestPriorityConfiguration))
                {
                    currentLowestPriorityConfiguration.StartReset(positionToMoveTo, moveTime);
                    positionToMoveTo -= currentLowestPriorityConfiguration.ResetOffset;
                    if (!currentLowestPriorityConfiguration.IsResetting)
                    {
                        RemoveFromMoving(currentLowestPriorityConfiguration);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void GetOrderOfUpdate(out List<MoveSortData> priorityOrderData)
        {
            priorityOrderData = new();
            for (int index = 0; index < activeMoves.Count; index++)
            {
                priorityOrderData.Add(new(activeMoves[index].priority, index));
            }
            priorityOrderData.Sort();
        }

        private void SetHighestPriority(MovingConfiguration firstHighPriority)
        {
            if(currentHighestPriority != null)
            {
                currentHighestPriority.SetResetToHighesPriority(false);
            }

            currentHighestPriority = firstHighPriority;
            currentHighestPriority.SetResetToHighesPriority(true);
        }
    }

    [System.Serializable]
    protected class MovingConfiguration
    {
        private const float BLOCK_DISTANCE = 0.05f;
        private Action<MovingConfiguration> OnAddToActive;
        private Action<MovingConfiguration> OnAddToReset;

        [Header("Dependencies")]
        [SerializeField]
        private PressingButton caller;
        private MovingPlatform owner;

        [Header("Configuration")]
        [Tooltip("Lower value gives a higher priority")]
        public int priority;
        [SerializeField]
        public Vector3 target;
        [SerializeField]
        private float targetDuration;
        [SerializeField]
        private float resetDuration;
        [SerializeField]
        private bool isPingPong;

        private Action<Vector3, float> resetTypeFunction;
        private Vector3 resetOffset;
        public Vector3 ResetOffset => resetOffset;
        private Vector3 positionAdded;

        private bool isResetting;
        public bool IsResetting => isResetting;
        private bool isComplete;
        public bool IsComplete => isComplete;

        private bool isBlocked;
        private float timeOffset;

        public void Init(MovingPlatform owner)
        {
            this.owner = owner;
            SetResetToHighesPriority(false);
            OnAddToActive = this.owner.AddMoveConfigurationToActive;
            OnAddToReset = this.owner.AddMoveToReset;
            caller.OnPressed += TryAddToActive;
            caller.OnReleased += TryAddToReset;
        }

        public void OnStartMove(float timeOffset)
        {
            this.timeOffset = timeOffset;
            owner.OnPlatformCollision += OnCollision;
            owner.OnPlatformCollisionEnded += CancelBlock;
        }

        public Vector3 MovePlatform(Vector3 startingPoint, float time)
        {
            Vector3 movement = GetPositionToAdd(startingPoint, time);
            if (!isBlocked)
            {
                owner.Rigidbody.MovePosition(movement + startingPoint);
            }
            else
            {
                timeOffset += Time.deltaTime;
                isComplete = false;
            }
            return movement;
        }

        public Vector3 GetPositionToAdd(Vector3 startPoint, float time)
        {
            time -= timeOffset;
            Vector3 position = new();
            float progress;

            if (isPingPong)
            {
                GetLerpProgress(out progress, time, false);
                position = Vector3.Lerp(startPoint, (target - resetOffset) + startPoint, Mathf.PingPong(progress, 1));
            }
            else
            {
                isComplete = GetLerpProgress(out progress, time);
                position = Vector3.Lerp(startPoint, (target - resetOffset) + startPoint, progress);
            }

            positionAdded = position;
            return position;
        }

        public void SetResetToHighesPriority(bool isHighestPriority)
        {
            if (isHighestPriority)
            {
                resetTypeFunction = ResetBackToStart;
            }
            else
            {
                resetTypeFunction = ResetBackToRelativeStart;
            }
        }

        public void StartReset(Vector3 relativeStartPoint, float time)
        {
            resetTypeFunction.Invoke(relativeStartPoint, time);
        }
        
        private void TryAddToActive()
        {
            OnAddToActive?.Invoke(this);
        }

        private void TryAddToReset()
        {
            OnAddToActive?.Invoke(this);
        }

        private void ResetBackToRelativeStart(Vector3 relativeStartPoint, float time)
        {
            float progress;
            isResetting = GetLerpProgress(out progress, time);
            owner.Rigidbody.MovePosition(Vector3.Lerp(owner.transform.position, relativeStartPoint - positionAdded, progress));
            resetOffset = owner.transform.position - relativeStartPoint - positionAdded;
        }

        private void ResetBackToStart(Vector3 startPoint, float time)
        {
            float progress;
            isResetting = GetLerpProgress(out progress, time);
            owner.Rigidbody.MovePosition(Vector3.Lerp(owner.transform.position, startPoint, progress));
            resetOffset = owner.transform.position - startPoint;
        }

        private bool GetLerpProgress(out float progress, float time, bool isClamped = true)
        {
            progress = time / targetDuration;
            if (isClamped)
            {
                progress = Mathf.Clamp01(progress);
                return progress == 1;
            }
            else
            {
                float tempValue = Mathf.Clamp01(progress % 1);
                return tempValue == 1;
            }
        }
        
        private void OnCollision(Vector3 collisionPosition)
        {
            Debug.Log("Collided with platform");

            if (CheckForBlock(collisionPosition))
            {
                isBlocked = true;
                timeOffset += Time.deltaTime;
                isComplete = false;
            }
            else
            {
                isBlocked = false;
            }
        }

        private bool CheckForBlock(Vector3 position)
        {
            Vector3 currentPosition = owner.transform.position;
            Vector3 closestPoint = owner.Rigidbody.ClosestPointOnBounds(position);
            Vector3 directionToCollision = closestPoint - currentPosition;
            float distanceFromClosestBound = BLOCK_DISTANCE + directionToCollision.magnitude;
            return Physics.Raycast(currentPosition, directionToCollision, distanceFromClosestBound);
        }

        private void CancelBlock()
        {
            isBlocked = false;
        }
    }

    protected struct MoveSortData : IComparable<MoveSortData>
    {
        public int priority;
        public int index;

        public MoveSortData(int priority, int index)
        {
            this.priority = priority;
            this.index = index;
        }

        public int CompareTo(MoveSortData other)
        {
            return this.priority.CompareTo(other.priority);
        }
    }

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out MovingPlatform isPlatform))
        {
            blockingPlatforms.Add(isPlatform.transform);
            OnPlatformCollision?.Invoke(isPlatform.transform.position);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(blockingPlatforms.Count > 0)
        {
            // Temporary list to avoid editing while updating
            List<Transform> temporaryList = new(blockingPlatforms);
            foreach (Transform transform in temporaryList)
            {
                OnPlatformCollision?.Invoke(transform.position);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out MovingPlatform isPlatform))
        {
            blockingPlatforms.Remove(isPlatform.transform);
            if(blockingPlatforms.Count == 0)
            {
                OnPlatformCollisionEnded?.Invoke();
            }
        }
    }

    private void AddMoveConfigurationToActive(MovingConfiguration configuration)
    {
        organizer.AddToMoving(configuration, this);
    }

    private void AddMoveToReset(MovingConfiguration configuration)
    {
        organizer.AddToReadyToReset(configuration);
    }

    private void OnDrawGizmos()
    {
        if (organizer == null || organizer.ConfigurationsData.Count == 0) return;

        foreach (MovingConfiguration configuration in organizer.ConfigurationsData)
        {
            if (configuration == null || configuration.target == null) return;
            Gizmos.DrawLine(transform.position, configuration.target + transform.position);
        }
    }
}
