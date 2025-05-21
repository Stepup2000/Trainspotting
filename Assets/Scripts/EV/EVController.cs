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
    /// Event triggered if the sky should start blinking.
    /// </summary>
    public UnityEvent<bool> ToggleSky { get; } = new UnityEvent<bool>();

    /// <summary>
    /// Event triggered if the tree should start bending.
    /// </summary>
    public UnityEvent<bool> ToggleTree { get; } = new UnityEvent<bool>();

    /// <summary>
    /// Event triggered if the grass should start growing..
    /// </summary>
    public UnityEvent<bool> ToggleGrass { get; } = new UnityEvent<bool>();

    /// <summary>
    /// Event triggered if the grass should start growing..
    /// </summary>
    public UnityEvent<bool> TogglePostProcessing { get; } = new UnityEvent<bool>();

    /// <summary>
    /// Event triggered if the aurora should appear..
    /// </summary>
    public UnityEvent<bool> ToggleAurora { get; } = new UnityEvent<bool>();

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

    /// <summary>
    /// Sends an event out to start or stop the sky effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the sky effect.</param>
    /// </summary>
    public void TriggerSky(bool onOrOff)
    {
        ToggleSky.Invoke(onOrOff);
    }

    /// <summary>
    /// Sends an event out to start or stop the tree effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the tree effect.</param>
    /// </summary>
    public void TriggerTreeAnimation(bool onOrOff)
    {
        ToggleTree.Invoke(onOrOff);
    }

    /// <summary>
    /// Sends an event out to start or stop the grass effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the grass effect.</param>
    /// </summary>
    public void TriggerGrass(bool onOrOff)
    {
        ToggleGrass.Invoke(onOrOff);
    }

    /// <summary>
    /// Sends an event out to start or stop the post processing effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the post processing effect.</param>
    /// </summary>
    public void TriggerPostProcessing(bool onOrOff)
    {
        TogglePostProcessing.Invoke(onOrOff);
    }

    /// <summary>
    /// Sends an event out to show or hide the aurora.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the post processing effect.</param>
    /// </summary>
    public void TriggerAurora(bool onOrOff)
    {
        ToggleAurora.Invoke(onOrOff);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdjustEV(1);
        }
    }
}
