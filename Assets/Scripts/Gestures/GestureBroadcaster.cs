using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class is responsible for broadcasting gesture events, such as thumbs up or thumbs down, 
/// to the event bus. It triggers specific events when gestures are performed.
/// </summary>
public class GestureBroadcaster : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    private float thumbsUpCooldown = 1f;
    private float thumbsDownCooldown = 1f;
    private float lastThumbsUpTime = -1f;
    private float lastThumbsDownTime = -1f;

    /// <summary>
    /// Triggers the thumbs up event by publishing it to the event bus.
    /// </summary>
    public void TriggerThumbsupEvent()
    {
        if (Time.time - lastThumbsUpTime >= thumbsUpCooldown)
        {
            EventBus<OnThumbsUpEvent>.Publish(new OnThumbsUpEvent());
            lastThumbsUpTime = Time.time;
        }
    }

    /// <summary>
    /// Triggers the thumbs down event by publishing it to the event bus.
    /// </summary>
    public void TriggerThumbsdownEvent()
    {
        if (Time.time - lastThumbsDownTime >= thumbsDownCooldown)
        {
            EventBus<OnThumbsDownEvent>.Publish(new OnThumbsDownEvent());
            lastThumbsDownTime = Time.time;
        }
    }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    /// <summary>
    /// Enables input when the script becomes active.
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();
    }

    /// <summary>
    /// Disables input when the script becomes inactive.
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        moveInput = inputActions.PCPLayer.Move.ReadValue<Vector2>();

        if (moveInput.x != 0)
        {
            TriggerThumbsupEvent();
        }

        if (moveInput.y != 0)
        {
            TriggerThumbsdownEvent();
        }
    }
}
