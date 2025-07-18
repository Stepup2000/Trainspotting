using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private BaseObjective startingObjective;
    [SerializeField] private float objectiveCooldown = 1f;

    private BaseObjective currentObjective;
    private static ObjectiveManager instance;

    private Queue<BaseObjective> objectiveQueue = new Queue<BaseObjective>();
    private bool isWaitingToStart = false;

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
    /// Stops all coroutines and clears the queue when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        StopAllCoroutines();
        objectiveQueue.Clear();
    }

    /// <summary>
    /// Starts the first objective at the beginning of the game.
    /// </summary>
    private void Start()
    {
        StartObjective(startingObjective);
    }

    /// <summary>
    /// Starts a new objective immediately if no cooldown is active, otherwise adds it to the queue.
    /// </summary>
    /// <param name="baseObject">The objective to start.</param>
    public void StartObjective(BaseObjective baseObject)
    {
        if (isWaitingToStart)
        {
            objectiveQueue.Enqueue(baseObject);
        }
        else
        {
            StartObjectiveImmediately(baseObject);
        }
    }

    /// <summary>
    /// Immediately starts the given objective and begins the cooldown period.
    /// </summary>
    /// <param name="newObjective">The new objective to start.</param>
    private void StartObjectiveImmediately(BaseObjective newObjective)
    {
        currentObjective?.CompleteObjective();
        currentObjective = newObjective;
        newObjective.gameObject.SetActive(true);
        newObjective.StartObjective();

        StartCoroutine(CooldownRoutine());
    }

    /// <summary>
    /// Waits for the cooldown before allowing another objective to be started.
    /// Automatically dequeues the next objective if one exists.
    /// </summary>
    private IEnumerator CooldownRoutine()
    {
        isWaitingToStart = true;

        yield return new WaitForSeconds(objectiveCooldown);

        isWaitingToStart = false;

        if (objectiveQueue.Count > 0)
        {
            StartObjectiveImmediately(objectiveQueue.Dequeue());
        }
    }

    /// <summary>
    /// Adjusts the EV value using the EVController.
    /// </summary>
    /// <param name="amount">The amount to change the EV by.</param>
    public void ChangeEV(float amount)
    {
        EVController.Instance.AdjustEV(amount);
    }
}
