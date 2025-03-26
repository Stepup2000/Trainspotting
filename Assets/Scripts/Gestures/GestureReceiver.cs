using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Receives the thumbs-up and thumbs-down gesture events and fires corresponding Unity events when detected.
/// </summary>
public class GestureReceiver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The event fired when the thumbs-up gesture is performed.")]
    UnityEvent ThumbsUpPerformed;

    [SerializeField]
    [Tooltip("The event fired when the thumbs-down gesture is performed.")]
    UnityEvent ThumbsDownPerformed;

    private void OnEnable()
    {
        // Subscribe to the thumbs-up and thumbs-down events
        EventBus<OnThumbsUpEvent>.Subscribe(ThumbsUpDetected);
        EventBus<OnThumbsDownEvent>.Subscribe(ThumbsDownDetected);
    }

    private void OnDisable()
    {
        // Unsubscribe from the events when the object is disabled
        EventBus<OnThumbsUpEvent>.UnSubscribe(ThumbsUpDetected);
        EventBus<OnThumbsDownEvent>.UnSubscribe(ThumbsDownDetected);
    }

    /// <summary>
    /// This method is called when the thumbs-up event is detected.
    /// </summary>
    /// <param name="thumbsUpEvent">The thumbs-up event.</param>
    private void ThumbsUpDetected(OnThumbsUpEvent thumbsUpEvent)
    {
        ThumbsUpPerformed?.Invoke();
    }

    /// <summary>
    /// This method is called when the thumbs-down event is detected.
    /// </summary>
    /// <param name="thumbsDownEvent">The thumbs-down event.</param>
    private void ThumbsDownDetected(OnThumbsDownEvent thumbsDownEvent)
    {
        ThumbsDownPerformed?.Invoke();
    }
}
