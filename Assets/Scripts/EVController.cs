using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the EV value, representing the intensity of a simulated experience.
/// -5 = Bad trip, 5 = Good trip. Notifies subscribers when the value changes.
/// </summary>
public class EVController : MonoBehaviour
{
    public static EVController Instance { get; private set; }

    [SerializeField, Range(-5f, 5f)]
    private float ev = 0f;

    /// <summary>
    /// Event triggered when EV changes. Supports both Unity Editor and C# subscriptions.
    /// </summary>
    public UnityEvent<float> OnEVChanged { get; } = new UnityEvent<float>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    /// <summary>
    /// Adjusts the EV value by a given amount.
    /// </summary>
    public void AdjustEV(float change)
    {
        EV += change;
    }
}
