using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    private List<BaseObjective> objectivesInQueue = new List<BaseObjective>();
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
    /// Fills in the objective queue and starts the first objective.
    /// </summary>
    private void Start()
    {
        CollectObjectivesRecursively(transform);

        StartNextObjective();
    }

    /// <summary>
    /// Recursively collects all objectives in the hierarchy starting from the provided transform.
    /// </summary>
    /// <param name="parent">The starting transform to search from.</param>
    private void CollectObjectivesRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            BaseObjective objective = child.GetComponent<BaseObjective>();
            if (objective != null)
            {
                AddObjective((BaseObjective)objective);
                Debug.Log("Objective added");
            }
        }
    }

    /// <summary>
    /// Adds a new objective to the list and starts the next objective if there is none currently active.
    /// </summary>
    /// <param name="objective">The objective to add to the list.</param>
    public void AddObjective(BaseObjective objective)
    {
        if (objective != null && !objectivesInQueue.Contains(objective))
            objectivesInQueue.Add(objective);
    }

    /// <summary>
    /// Starts the next objective in the list. Removes the objective from the list and starts it.
    /// </summary>
    private void StartNextObjective()
    {
        if (objectivesInQueue.Count > 0)
        {
            // Set the current objective to the first one in the list and start it
            currentObjective = objectivesInQueue[0];
            objectivesInQueue.RemoveAt(0);
            currentObjective.StartObjective();
        }
        else
        {
            Debug.Log("No more objectives to complete.");
        }
    }

    /// <summary>
    /// Completes the current objective and starts the next one if available.
    /// </summary>
    public void GoToNextObjective()
    {
        if (currentObjective != null)
        {
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
        foreach (var objective in objectivesInQueue)
        {
            Debug.Log($"Objective: {objective.ObjectiveTitle}, Completed: {objective.IsCompleted}");
        }
    }
}
