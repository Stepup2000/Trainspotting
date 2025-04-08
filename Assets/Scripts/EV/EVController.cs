using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the EV value, representing the intensity of a simulated experience.
/// -5 = Bad trip, 5 = Good trip. Notifies subscribers when the value changes.
/// </summary>
public class EVController : MonoBehaviour
{
    private static EVController instance;

    [SerializeField, Range(-5f, 5f)]
    private float ev = 0f;

    /// <summary>
    /// Event triggered when EV changes. Supports both Unity Editor and C# subscriptions.
    /// </summary>
    public UnityEvent<float> OnEVChanged { get; } = new UnityEvent<float>();

    /// <summary>
    /// Event triggered if the pixiedust should start appearing.
    /// </summary>
    public UnityEvent<bool> TogglePixiedust { get; } = new UnityEvent<bool>();

    /// <summary>
    /// Event triggered if the pixiedust should start appearing.
    /// </summary>
    public UnityEvent<bool> ToggleWobble { get; } = new UnityEvent<bool>();

    /// <summary>
    /// Provides access to the singleton instance of EVController
    /// </summary>
    public static EVController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<EVController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("EVController");
                    instance = singletonObject.AddComponent<EVController>();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Gets or sets the EV value and triggers change events.
    /// </summary>
    public float EV
    {
        get => ev;
        set
        {
            float clampedValue = Mathf.Clamp(value, -5f, 5f);
            if (ev != clampedValue)
            {
                ev = clampedValue;
                OnEVChanged.Invoke(ev);
            }
        }
    }

    private void Awake()
    {
        _ = Instance;
    }

    /// <summary>
    /// Adjusts the EV value by a given amount.
    /// </summary>
    /// /// <param name="change">The amount the EV will change.</param>
    public void AdjustEV(float change)
    {
        EV += change;
    }

    /// <summary>
    /// Resets the EV back to 0.
    /// </summary>
    public void ResetEV()
    {
        EV = 0;
    }

    /// <summary>
    /// Sends an event out to start or stop the pixiedust.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the pixiedust effect.</param>
    /// </summary>
    public void TriggerPixiedust(bool onOrOff)
    {
        TogglePixiedust.Invoke(onOrOff);
    }

    /// <summary>
    /// Sends an event out to start or stop the wobble.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the wobble effect.</param>
    /// </summary>
    public void TriggerWobble(bool onOrOff)
    {
        ToggleWobble.Invoke(onOrOff);
    }
}
