using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform initialTarget;

    private NavMeshAgent agent;
    private bool isMoving = false;

    private Dictionary<AnimationState, string> animationLookup = new();

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
                PlayAnimation(AnimationState.Idle);
            }
        }
    }

    private void MoveToInitialTarget()
    {
        MoveTo(initialTarget);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        isMoving = true;
        PlayAnimation(AnimationState.Run);
    }

    public void MoveTo(Transform target)
    {
        if (target != null)
            MoveTo(target.position);
    }

    /// <summary>
    /// Auto-fills the animation lookup based on Animator bool parameters that match enum names.
    /// </summary>
    private void GenerateAnimationLookupFromAnimator()
    {
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
    /// Plays the animation by setting the corresponding bool and disabling others.
    /// </summary>
    public void PlayAnimation(AnimationState state)
    {
        // Reset all known bools first
        foreach (var boolName in animationLookup.Values)
        {
            animator.SetBool(boolName, false);
        }

        // Then activate the one for this state
        if (animationLookup.TryGetValue(state, out string boolToPlay))
        {
            animator.SetBool(boolToPlay, true);
        }
        else
        {
            Debug.LogWarning($"No matching animator bool found for animation state: {state}");
        }
    }
}
