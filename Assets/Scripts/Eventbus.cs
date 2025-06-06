using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A generic event bus for handling events of type <typeparamref name="T"/> where <typeparamref name="T"/> is a subclass of <see cref="Event"/>.
/// This allows subscribing, unsubscribing, and publishing events of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the event that is derived from the base class <see cref="Event"/>.</typeparam>
public class EventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    private static readonly Dictionary<Type, float> _lastPublishTime = new Dictionary<Type, float>();
    private const float PublishCooldown = 1f;

    /// <summary>
    /// Subscribes a handler to the event. The handler will be called when the event is published.
    /// </summary>
    /// <param name="handler">The action to handle the event of type <typeparamref name="T"/>.</param>
    public static void Subscribe(Action<T> handler)
    {
        OnEvent += handler;
    }

    /// <summary>
    /// Unsubscribes a handler from the event. The handler will no longer be called when the event is published.
    /// </summary>
    /// <param name="handler">The action to remove from the event subscription.</param>
    public static void UnSubscribe(Action<T> handler)
    {
        OnEvent -= handler;
    }

    /// <summary>
    /// Publishes an event of type <typeparamref name="T"/> to all subscribed handlers.
    /// Only allows publishing once per second per event type.
    /// </summary>
    /// <param name="pEvent">The event to publish.</param>
    public static void Publish(T pEvent)
    {
        var eventType = typeof(T);
        float time = Time.time;

        if (_lastPublishTime.TryGetValue(eventType, out float lastTime))
        {
            if (time - lastTime < PublishCooldown)
                return;
        }

        _lastPublishTime[eventType] = time;
        OnEvent?.Invoke(pEvent);
    }
}


/// <summary>
/// Represents an event indicating a thumbs up reaction.
/// </summary>
public class OnThumbsUpEvent : Event
{
    public OnThumbsUpEvent()
    {
    }
}

/// <summary>
/// Represents an event indicating a thumbs down reaction.
/// </summary>
public class OnThumbsDownEvent : Event
{
    public OnThumbsDownEvent()
    {
    }
}
