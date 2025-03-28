using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private BaseObjective startingObjective;

    private BaseObjective currentObjective;

    private static ObjectiveManager instance;

    /// <summary>
    /// Provides access to the singleton instance of ObjectiveManager
    /// </summary>
    public static ObjectiveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<ObjectiveManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ObjectiveManager");
                    instance = singletonObject.AddComponent<ObjectiveManager>();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Starts the first objective.
    /// </summary>
    private void Start()
    {
        StartObjective(startingObjective);
    }

    /// <summary>
    /// Starts a new objective by completing the current one (if any) and setting the new objective as the current one.
    /// </summary>
    /// <param name="baseObject">The new objective to start.</param>
    public void StartObjective(BaseObjective baseObject)
    {
        currentObjective?.CompleteObjective();
        currentObjective = baseObject;
        baseObject.StartObjective();        
    }

    // <summary>
    /// Public method that other scripts/instances can access to change the EV through the EVController.
    /// </summary>
    public void ChangeEV(float amount)
    {
        EVController.Instance.AdjustEV(amount);
    }
}
