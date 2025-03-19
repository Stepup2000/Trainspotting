using UnityEngine;

/// <summary>
/// An objective class where an object needs to be picked up.
/// </summary>
public class ExternalTriggerObjective : BaseObjective
{
    [SerializeField] private GameObject ExternalObject;

    /// <summary>
    /// Disables the associated object(s) at the start of the scene.
    /// </summary>
    protected void Start()
    {
        ExternalObject.SetActive(false);
    }

    /// <summary>
    /// Starts the collection objective.
    /// </summary>
    public override void StartObjective()
    {
        base.StartObjective();
        ExternalObject.SetActive(true);
    }

    /// <summary>
    /// Defines the behavior when the collection objective is completed.
    /// </summary>
    protected override void OnQuestCompleted()
    {
        Debug.Log($"Objective '{ObjectiveTitle}' completed");
        ExternalObject.SetActive(false);
    }
}
