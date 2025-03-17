using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class InputActionButtonControl : OnScreenControl
{
    [SerializeField] private InputActionReference leftHandAction;
    [SerializeField] private InputActionReference rightHandAction;

    [SerializeField]
    private string mControlPath;

    protected override string controlPathInternal
    {
        get => mControlPath;
        set => mControlPath = value;
    }

    private void Update()
    {
        if (leftHandAction == null || leftHandAction.action == null)
        {
            Debug.Log("Select action not assigned!");
            return;
        }
        CheckForAction();        
    }

    private void CheckForAction()
    {
        if (leftHandAction.action.WasPressedThisFrame())
            DebugStart();
        else if (leftHandAction.action.WasReleasedThisFrame())
            DebugStop();
    }

    public void StartLeftGrab()
    {
        SendValueToControl(1.0f);
        Debug.Log("StartGrab");
    }

    public void EndLeftGrab()
    {
        SentDefaultValueToControl();
        Debug.Log("EndGrab");
    }

    private void DebugStart()
    {
        Debug.Log("InputActionRecognized");
    }

    private void DebugStop()
    {
        Debug.Log("InputActionStopped");
    }
}



/*using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class InputActionButtonControl : OnScreenControl
{
    [SerializeField] private InputActionReference _inputActionRef;

    public PlayerInput input;

    protected override string controlPathInternal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void TriggerButton()
    {
        StartCoroutine(SendTap_Coroutine());
    }

    public void Update()
    {
        var select = input.actions["Select"];
        if (select.WasPressedThisFrame() && select.IsPressed())
            DebugStart();
        else if (select.WasReleasedThisFrame())
            DebugStop();
    }

    private IEnumerator SendTap_Coroutine()
    {
        yield return null;
        SentDefaultValueToControl();
        yield return null;
        SendValueToControl<float>(1);
        yield return null;
        SentDefaultValueToControl();
    }

    public void DebugStart()
    {
        Debug.Log("InputActionRecognised");
    }

    public void DebugStop()
    {
        Debug.Log("InputActionStopped");
    }
}

*/