using UnityEngine;

/// <summary>
/// Base class for quest objectives.
/// Implements IObjective and provides common functionality for derived classes.
/// </summary>
public abstract class BaseObjective : MonoBehaviour, IObjective
{
    public string ObjectiveTitle { get; protected set; }
    public string ObjectiveDescription { get; protected set; }

    public bool IsCompleted { get; protected set; }

    /// <summary>
    /// Starts the objective and triggers all associated logic.
    /// </summary>
    public virtual void StartObjective()
    {
        Debug.Log($"Starting quest: {ObjectiveTitle}");
        IsCompleted = false;
    }

    /// <summary>
    /// Completes the objective triggers all associated logic.
    /// </summary>
    public virtual void CompleteObjective()
    {
        if (!IsCompleted)
        {
            Debug.Log($"Completing quest: {ObjectiveTitle}");
            IsCompleted = true;
            OnQuestCompleted();
        }
    }

    /// <summary>
    /// Method to define the behavior when the objective is completed.
    /// This method must be implemented in the derived class to specify what happens when the objective is finished.
    /// </summary>
    protected abstract void OnQuestCompleted();
}
