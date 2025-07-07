using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles NPC movement, rotation, and animation control using NavMeshAgent and Animator.
/// </summary>
public class NPC : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform initialTarget;

    private NavMeshAgent agent;
    private bool isMoving = false;
    private bool isRotating = false;

    private Dictionary<AnimationState, string> animationLookup = new();
    private Transform currentTargetRotation;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationThreshold = 1f;

    /// <summary>
    /// Initializes components and starts movement to the initial target.
    /// </summary>
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GenerateAnimationLookupFromAnimator();
        Invoke(nameof(MoveToInitialTarget), 1f);
    }

    /// <summary>
    /// Updates movement and rotation each frame.
    /// </summary>
    void Update()
    {
        if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoving = false;
                agent.enabled = false;
                isRotating = currentTargetRotation != null;

                if (!isRotating)
                {
                    PlayAnimation(AnimationState.Idle);
                    Debug.Log("Reached destination and turned off");
                }
            }
        }

        if (isRotating)
        {
            RotateTowardsTargetRotation();
        }
    }

    /// <summary>
    /// Smoothly rotates the NPC towards the target rotation.
    /// </summary>
    private void RotateTowardsTargetRotation()
    {
        if (currentTargetRotation == null) return;

        Quaternion targetRotation = currentTargetRotation.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
        if (angleDifference <= rotationThreshold)
        {
            transform.rotation = targetRotation;
            isRotating = false;
            PlayAnimation(AnimationState.Idle);
            Debug.Log("Finished smooth rotation");
        }
    }

    /// <summary>
    /// Commands the NPC to move to the initial target.
    /// </summary>
    private void MoveToInitialTarget()
    {
        MoveTo(initialTarget);
    }

    /// <summary>
    /// Commands the NPC to move to a world position.
    /// </summary>
    /// <param name="targetPosition">The target position to move to.</param>
    public void MoveTo(Vector3 targetPosition)
    {
        agent.enabled = true;
        agent.SetDestination(targetPosition);
        isMoving = true;
        isRotating = false;
        PlayAnimation(AnimationState.Run);
        currentTargetRotation = null;
    }

    /// <summary>
    /// Commands the NPC to move to a transform's position and face it upon arrival.
    /// </summary>
    /// <param name="target">The transform to move to.</param>
    public void MoveTo(Transform target)
    {
        if (target != null)
        {
            MoveTo(target.position);
            currentTargetRotation = target;
        }
    }

    /// <summary>
    /// Generates a lookup table from AnimationState to Animator parameter names.
    /// </summary>
    private void GenerateAnimationLookupFromAnimator()
    {
        if (animator == null) return;

        animationLookup.Clear();

        var parameters = animator.parameters;

        foreach (AnimationState state in System.Enum.GetValues(typeof(AnimationState)))
        {
            foreach (var param in parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool && param.name == state.ToString())
                {
                    animationLookup[state] = param.name;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Plays the specified animation state by setting the corresponding Animator bool.
    /// </summary>
    /// <param name="state">The animation state to play.</param>
    public void PlayAnimation(AnimationState state)
    {
        foreach (var boolName in animationLookup.Values)
        {
            animator.SetBool(boolName, false);
        }

        if (animationLookup.TryGetValue(state, out string boolToPlay))
        {
            animator.SetBool(boolToPlay, true);
        }
        else
        {
            Debug.LogWarning($"No matching animator bool found for animation state: {state}");
        }
    }

    /// <summary>
    /// Plays an animation state by its name as a string.
    /// </summary>
    /// <param name="stateName">The name of the animation state to play.</param>
    public void PlayAnimationByName(string stateName)
    {
        if (System.Enum.TryParse(stateName, true, out AnimationState parsedState))
        {
            PlayAnimation(parsedState);
        }
        else
        {
            Debug.LogWarning($"Invalid animation state string: {stateName}");
        }
    }
}
