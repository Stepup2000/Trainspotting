using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Receives the thumbs-up and thumbs-down gesture events and fires corresponding Unity events when detected.
/// Implements a cooldown to prevent rapid repeated detections.
/// </summary>
public class GestureReceiver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The event fired when the thumbs-up gesture is performed.")]
    UnityEvent ThumbsUpPerformed;

    [SerializeField]
    [Tooltip("The event fired when the thumbs-down gesture is performed.")]
    UnityEvent ThumbsDownPerformed;

    private bool isOnCooldown = false;

    private readonly float cooldownDuration = 1.5f;

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
    }

    /// <summary>
    /// Called when a thumbs-up gesture is detected; invokes the thumbs-up event if not on cooldown.
    /// </summary>
    /// <param name="thumbsUpEvent">The thumbs-up event data.</param>
    private void ThumbsUpDetected(OnThumbsUpEvent thumbsUpEvent)
    {
        if (isOnCooldown) return;

        ThumbsUpPerformed?.Invoke();
        StartCooldown();
    }

    /// <summary>
    /// Called when a thumbs-down gesture is detected; invokes the thumbs-down event if not on cooldown.
    /// </summary>
    /// <param name="thumbsDownEvent">The thumbs-down event data.</param>
    private void ThumbsDownDetected(OnThumbsDownEvent thumbsDownEvent)
    {
        if (isOnCooldown) return;

        ThumbsDownPerformed?.Invoke();
        StartCooldown();
    }

    /// <summary>
    /// Starts the cooldown coroutine to block further gesture events for the cooldown duration.
    /// </summary>
    private void StartCooldown()
    {
        if (!isOnCooldown)
            StartCoroutine(CooldownCoroutine());
    }

    /// <summary>
    /// Coroutine that manages the cooldown timing.
    /// </summary>
    /// <returns>IEnumerator for coroutine handling.</returns>
    private IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }
}
