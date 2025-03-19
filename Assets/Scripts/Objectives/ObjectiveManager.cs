using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private List<BaseObjective> objectiveList = new List<BaseObjective>();
    private BaseObjective currentObjective;

    private void Awake()
    {
        // Ensure that only one instance of ObjectiveManager exists
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Adds a new objective to the list and starts the next objective if there is none currently active.
    /// </summary>
    /// <param name="objective">The objective to add to the list.</param>
    public void AddObjective(BaseObjective objective)
    {
        if (objective != null)
        {
            objectiveList.Add(objective);
            if (currentObjective == null)
                StartNextObjective();
        }
    }

    /// <summary>
    /// Starts the next objective in the list. Removes the objective from the list and starts it.
    /// </summary>
    private void StartNextObjective()
    {
        if (objectiveList.Count > 0)
        {
            // Set the current objective to the first one in the list and start it
            currentObjective = objectiveList[0];
            objectiveList.RemoveAt(0);
            currentObjective.StartObjective();
            Debug.Log($"Starting objective: {currentObjective.ObjectiveTitle}");
        }
        else
            Debug.Log("No more objectives to complete.");
    }

    /// <summary>
    /// Completes the current objective and starts the next one if available.
    /// </summary>
    public void CompleteCurrentObjective()
    {
        if (currentObjective != null)
        {
            currentObjective.CompleteObjective();
            Debug.Log($"Objective '{currentObjective.ObjectiveTitle}' completed");

            // Reset the current objective and start the next one
            currentObjective = null;
            StartNextObjective();
        }
    }

    /// <summary>
    /// Logs all objectives and their completion status.
    /// </summary>
    public void ListObjectives()
    {
        foreach (var objective in objectiveList)
        {
            Debug.Log($"Objective: {objective.ObjectiveTitle}, Completed: {objective.IsCompleted}");
        }
    }
}
