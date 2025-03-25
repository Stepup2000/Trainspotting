using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for quest objectives.
/// Implements IObjective and provides common functionality for derived classes.
/// </summary>
public class BaseObjective : MonoBehaviour, IObjective
{
    [SerializeField] 
    [Tooltip("The object that will be turned off/on based on the state of the objective.")]
    private GameObject targetObject;

    [SerializeField]
    [Tooltip("The event fired when the objective has been completed.")]
    private UnityEvent OnObjectiveComplete;

    public string ObjectiveTitle { get; protected set; }
    public string ObjectiveDescription { get; protected set; }

    public bool IsActive { get; protected set; }
    public bool IsCompleted { get; protected set; }

    /// <summary>
    /// Disables the associated object(s) at the start of the scene.
    /// </summary>
    protected void Awake()
    {
        targetObject?.SetActive(false);
        IsCompleted = false;
        IsActive = false;
    }

    /// <summary>
    /// Starts the objective and triggers all associated logic.
    /// </summary>
    public virtual void StartObjective()
    {
        //Debug.Log($"Starting quest: {ObjectiveTitle}");
        targetObject?.SetActive(true);
        IsActive = true;
    }

    /// <summary>
    /// Completes the objective triggers all associated logic.
    /// </summary>
    public virtual void CompleteObjective()
    {
        if (!IsCompleted && IsActive == true)
        {           
            IsCompleted = true;
            IsActive = false;
            OnQuestCompleted();
        }
    }

    /// <summary>
    /// Method to define the behavior when the objective is completed.
    /// </summary>
    protected virtual void OnQuestCompleted()
    {
        targetObject?.SetActive(false);
        ObjectiveManager.Instance.GoToNextObjective();
        OnObjectiveComplete?.Invoke();
        Debug.Log($"Completed quest: {ObjectiveTitle}");
    }

    /// <summary>
    /// Public method that other scripts/instances can access to change the EV through the EVController.
    /// </summary>
    public virtual void ChangeEV(float amount)
    {
        EVController.Instance.AdjustEV(amount);
    }
}
