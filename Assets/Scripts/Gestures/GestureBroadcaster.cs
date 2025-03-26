using UnityEngine;

/// <summary>
/// This class is responsible for broadcasting gesture events, such as thumbs up or thumbs down, 
/// to the event bus. It triggers specific events when gestures are performed.
/// </summary>
public class GestureBroadcaster : MonoBehaviour
{
    /// <summary>
    /// Triggers the thumbs up event by publishing it to the event bus.
    /// </summary>
    public void TriggerThumbsupEvent()
    {
        EventBus<OnThumbsUpEvent>.Publish(new OnThumbsUpEvent());
    }

    /// <summary>
    /// Triggers the thumbs down event by publishing it to the event bus.
    /// </summary>
    public void TriggerThumbsdownEvent()
    {
        EventBus<OnThumbsDownEvent>.Publish(new OnThumbsDownEvent());
    }
}
