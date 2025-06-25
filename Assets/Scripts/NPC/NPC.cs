using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private float rotationThreshold = 1f; // How close (in degrees) before we consider rotation done

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GenerateAnimationLookupFromAnimator();

        Invoke(nameof(MoveToInitialTarget), 1f);
    }

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

    private void MoveToInitialTarget()
    {
        MoveTo(initialTarget);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.enabled = true;
        agent.SetDestination(targetPosition);
        isMoving = true;
        isRotating = false;
        PlayAnimation(AnimationState.Run);
        currentTargetRotation = null;
    }

    public void MoveTo(Transform target)
    {
        if (target != null)
        {
            MoveTo(target.position);
            currentTargetRotation = target;
        }
    }

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
