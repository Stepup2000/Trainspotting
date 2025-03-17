using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class InputActionButtonControl : OnScreenControl
{
    [SerializeField] private InputActionReference selectAction;

    
    [SerializeField]
    private string mControlPath;

    protected override string controlPathInternal
    {
        get => mControlPath;
        set => mControlPath = value;
    }






    public void TriggerButton()
    {
        StartCoroutine(SendTapCoroutine());
    }

    private void Update()
    {
        if (selectAction == null || selectAction.action == null)
        {
            Debug.Log("Select action not assigned!");
            return;
        }

        if (selectAction.action.WasPressedThisFrame())
            DebugStart();
        else if (selectAction.action.WasReleasedThisFrame())
            DebugStop();
    }

    private IEnumerator SendTapCoroutine()
    {
        yield return null;
        SentDefaultValueToControl();
        SendValueToControl(1.0f);
        yield return new WaitForSeconds(0.1f);
        SentDefaultValueToControl();
        Debug.Log("SendTapCoroutine");
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