using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using System.Collections.Generic;
using System.Collections;
using Unity.Android.Gradle.Manifest;

/// <summary>
/// Base class for quest objectives.
/// Implements IObjective and provides common functionality for derived classes.
/// </summary>
public class BaseObjective : MonoBehaviour, IObjective
{
    [SerializeField]
    [Tooltip("The event fired when the objective has been completed.")]
    private UnityEvent OnObjectiveComplete;

    //receiving the sound effect through editor
    [Header("Effects to preload")]
    [SerializeField] private List<AudioReceiver> sfxToPreload = new List<AudioReceiver>();

    public string ObjectiveTitle { get; protected set; }
    public string ObjectiveDescription { get; protected set; }
    public bool IsCompleted { get; protected set; }

    /// <summary>
    /// Disables the associated object(s) at the start of the scene.
    /// </summary>
    protected void Awake()
    {
        gameObject.SetActive(false);
        IsCompleted = false;
    }

    /// <summary>
    /// Starts the objective and triggers all associated logic.
    /// </summary>
    public virtual void StartObjective()
    {
        Debug.Log($"Starting quest: {ObjectiveTitle}" + gameObject.name);
        gameObject.SetActive(true);
        IsCompleted = false;

        // Trigger all sounds via the AudioManager
        foreach (var sfx in sfxToPreload)
        {
            AudioManager.instance.PlayWithDelay(sfx);
        }
    }

    /// <summary>
    /// Completes the objective triggers all associated logic.
    /// </summary>
    public virtual void CompleteObjective()
    {
        if (!IsCompleted)
        {           
            IsCompleted = true;
            OnQuestCompleted();
        }
    }

    /// <summary>
    /// Method to define the behavior when the objective is completed.
    /// </summary>
    protected virtual void OnQuestCompleted()
    {
        Debug.Log($"Completed quest: {ObjectiveTitle}" + gameObject.name);
        OnObjectiveComplete?.Invoke();        
        gameObject.SetActive(false);        
    }
}
