using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class SimulateTriggerPress : MonoBehaviour
{
    public InputActionProperty triggerAction;  // This will be your trigger input action.
    public void SimulateTrigger()
    {
        triggerAction.action.PerformInteractiveRebinding(); // Manipulate for real-time simulations
    }
}
