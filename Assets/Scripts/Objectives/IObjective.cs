using UnityEngine;

/// <summary>
/// Interface defining the basic structure for quest objectives.
/// Any class implementing this interface will have essential properties and methods for managing objectives within a quest system.
/// </summary>
public interface IObjective
{
    public string ObjectiveTitle { get; }
    public string ObjectiveDescription { get; }
    public bool IsCompleted { get; }
    public void StartObjective();
    public void CompleteObjective();
}
