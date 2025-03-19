using UnityEngine;
using System.Collections;

/// <summary>
/// An objective class where an object needs to be picked up after a delay.
/// </summary>
public class TimedObjective : BaseObjective
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float objectiveTime = 5f;

    private Coroutine objectiveCoroutine;

    /// <summary>
    /// Disables the associated object(s) at the start of the scene.
    /// </summary>
    protected void Start()
    {
        targetObject.SetActive(false);
    }

    /// <summary>
    /// Starts the timed collection objective using a coroutine.
    /// </summary>
    public override void StartObjective()
    {
        base.StartObjective();
        targetObject.SetActive(true);

        if (objectiveCoroutine != null)
            StopCoroutine(objectiveCoroutine);

        objectiveCoroutine = StartCoroutine(ActivateObjective());
    }

    /// <summary>
    /// Coroutine to handle the timer logic for enabling and disabling the objective.
    /// </summary>
    private IEnumerator ActivateObjective()
    {
        yield return new WaitForSeconds(objectiveTime);
        OnQuestCompleted();
    }

    /// <summary>
    /// Defines the behavior when the collection objective is completed.
    /// </summary>
    protected override void OnQuestCompleted()
    {
        Debug.Log($"Objective '{ObjectiveTitle}' completed");
        targetObject.SetActive(false);
    }
}
