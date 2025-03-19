using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

/// <summary>
/// A class that overrides the input system to force a grab action.
/// Using this class the hand tracking system can use grabbing without a physical button.
/// </summary>
public class HandGrabOverride : OnScreenControl
{
    [SerializeField, Tooltip("Reference to the input action that should control the grab.")]
    private InputActionReference HandAction;

    [SerializeField, Tooltip("The control path used to bind the input action.")]
    private string mControlPath;


    /// <summary>
    /// Gets or sets the control path for this on-screen control.
    /// </summary>
    protected override string controlPathInternal
    {
        get => mControlPath;
        set => mControlPath = value;
    }

    private void Update()
    {
        if (HandAction == null || HandAction.action == null)
        {
            Debug.Log("Select action not assigned!");
            return;
        }
        CheckForAction();
    }

    /// <summary>
    /// Checks if the assigned input action was pressed or released and triggers corresponding events.
    /// </summary>
    private void CheckForAction()
    {
        if (HandAction.action.WasPressedThisFrame())
            DebugStart();
        else if (HandAction.action.WasReleasedThisFrame())
            DebugStop();
    }

    /// <summary>
    /// Sends a value to the control indicating the left grab action has started.
    /// </summary>
    public void StartGrab()
    {
        SendValueToControl(1.0f);
        Debug.Log("StartGrab");
    }

    /// <summary>
    /// Resets the control value, indicating the left grab action has ended.
    /// </summary>
    public void EndGrab()
    {
        SentDefaultValueToControl();
        Debug.Log("EndGrab");
    }

    /// <summary>
    /// Logs when an input action is recognized.
    /// </summary>
    private void DebugStart()
    {
        Debug.Log("InputActionRecognized");
    }

    /// <summary>
    /// Logs when an input action stops.
    /// </summary>
    private void DebugStop()
    {
        Debug.Log("InputActionStopped");
    }
}
