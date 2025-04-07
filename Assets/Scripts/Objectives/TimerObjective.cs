using UnityEngine;
using System.Collections;

/// <summary>
/// An objective class where an object needs to be picked up after a delay.
/// </summary>
public class TimedObjective : BaseObjective
{
    [SerializeField] private float objectiveTime = 5f;

    protected Coroutine objectiveCoroutine;

    /// <summary>
    /// Starts the timed collection objective using a coroutine.
    /// </summary>
    public override void StartObjective()
    {
        //base.StartObjective();
        Debug.Log("Started");
        if (objectiveCoroutine != null)
            StopCoroutine(objectiveCoroutine);

        objectiveCoroutine = StartCoroutine(ActivateObjective());
    }

    /// <summary>
    /// Coroutine to handle the timer logic for enabling and disabling the objective.
    /// </summary>
    protected IEnumerator ActivateObjective()
    {
        Debug.Log("Started timer");
        yield return new WaitForSeconds(objectiveTime);
        CompleteObjective();
    }
}
