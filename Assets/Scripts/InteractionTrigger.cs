using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

public class InteractionTrigger : MonoBehaviour
{
    public InputActionAsset inputActions;

    public static InputDevice SetUpMockInputForActions(InputActionAsset actions)
    {
        var layoutName = actions.name;

        // Build a device layout that simply has one control for each action in the asset.
        InputSystem.RegisterLayoutBuilder(() =>
        {
            var builder = new InputControlLayout.Builder()
                .WithName(layoutName);

            foreach (var action in actions)
            {
                builder.AddControl(action.name) // Must not have actions in separate maps with the same name.
                    .WithLayout(action.expectedControlType);
            }

            return builder.Build();
        }, name: layoutName);

        // Create the device.
        var device = InputSystem.AddDevice(layoutName);

        // Add a control scheme for it to the actions.
        actions.AddControlScheme("MockInput")
            .WithRequiredDevice($"<{layoutName}>");

        // Bind the actions in the newly added control scheme.
        foreach (var action in actions)
            action.AddBinding($"<{layoutName}>/{action.name}", groups: "MockInput");

        // Restrict the actions to bind only to our newly created
        // device using the bindings we just added.
        actions.bindingMask = InputBinding.MaskByGroup("MockInput");
        actions.devices = new[] { device };

        return device;
    }

    public void SendEvent()
    {
        var mockInput = SetUpMockInputForActions(inputActions);
        //inputActions.Press((ButtonControl)mockInput["fire"]);

    }
}