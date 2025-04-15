using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles movement of the GameObject using Unity's NavMeshAgent.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshWalker : MonoBehaviour
{
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Moves the agent to the position of the specified Transform.
    /// </summary>
    /// <param name="targett">The Transform to move toward.</param>
    public void MoveToTarget(Transform target)
    {
        if (target != null)
            agent.SetDestination(target.position);
    }

    /// <summary>
    /// Moves the agent to a specific world position.
    /// </summary>
    /// <param name="position">The target position to move toward.</param>
    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
