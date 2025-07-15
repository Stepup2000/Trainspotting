using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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

    /// <summary>
    /// Subscribes to gesture events when the script is enabled.
    /// </summary>
    private void OnEnable()
    {
        EventBus<OnThumbsUpEvent>.Subscribe(ThumbsUpDetected);
        EventBus<OnThumbsDownEvent>.Subscribe(ThumbsDownDetected);
    }

    /// <summary>
    /// Unsubscribes from gesture events when the script is disabled.
    /// </summary>
    private void OnDisable()
    {
        EventBus<OnThumbsUpEvent>.UnSubscribe(ThumbsUpDetected);
        EventBus<OnThumbsDownEvent>.UnSubscribe(ThumbsDownDetected);
        StopAllCoroutines();
    }

    /// <summary>
    /// Called when a thumbs-up gesture is detected; invokes the thumbs-up event.
    /// </summary>
    /// <param name="thumbsUpEvent">The thumbs-up event data.</param>
    private void ThumbsUpDetected(OnThumbsUpEvent thumbsUpEvent)
    {
        ThumbsUpPerformed?.Invoke();
    }

    /// <summary>
    /// Called when a thumbs-down gesture is detected; invokes the thumbs-down event.
    /// </summary>
    /// <param name="thumbsDownEvent">The thumbs-down event data.</param>
    private void ThumbsDownDetected(OnThumbsDownEvent thumbsDownEvent)
    {
        ThumbsDownPerformed?.Invoke();
    }
}
