using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles movement of the GameObject using Unity's NavMeshAgent and plays animations.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform initialTarget;
    private NavMeshAgent agent;
    private bool isMoving = false;

    /// <summary>
    /// Represents the possible animation states.
    /// </summary>
    public enum AnimationState
    {
        Idle,
        Run
    }
        

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
                animator.SetBool("Run", false);
            }
        }
    }

    /// <summary>
    /// Moves the agent to the target position at the start of the life of the script.
    /// </summary>
    private void MoveToInitialTarget()
    {
        MoveTo(initialTarget);
    }

    /// <summary>
    /// Moves the agent to the target position.
    /// </summary>
    /// <param name="targetPosition">The world position to move toward.</param>
    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        isMoving = true;
        PlayAnimation(AnimationState.Run);
    }

    /// <summary>
    /// Moves the agent to the position of the specified Transform.
    /// </summary>
    /// <param name="target">The Transform to move toward.</param>
    public void MoveTo(Transform target)
    {
        if (target != null)
            MoveTo(target.position);
    }


    /// <summary>
    /// Plays the animation that matches the given animation state.
    /// </summary>
    /// <param name="state">The animation state to play.</param>
    public void PlayAnimation(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.Idle:
                animator.SetTrigger("Idle");
                break;
            case AnimationState.Run:
                animator.SetBool("Run", true);
                break;
        }
    }
}
