using UnityEngine;

public class WobbleEVApplier : MonoBehaviour
{
    [SerializeField] private Material targetMaterial; // Reference to the material
    [SerializeField] private string amplitudeProperty = "_Amplitude";

    private float latestAmplitude = 0f;

    private void OnEnable()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned.");
            return;
        }

        latestAmplitude = targetMaterial.GetFloat(amplitudeProperty);
        TriggerWobble(false);

        if (EVController.Instance != null)
        {
            EVController.Instance.OnEVChanged.AddListener(HandleEVChanged);
            EVController.Instance.ToggleWobble.AddListener(TriggerWobble);
            TriggerWobble(false);
        }
    }

    private void OnDisable()
    {
        if (EVController.Instance != null)
        {
            EVController.Instance.OnEVChanged.RemoveListener(HandleEVChanged);
            EVController.Instance.ToggleWobble.RemoveListener(TriggerWobble);
        }
        ApplyAmplitude(latestAmplitude);
    }

    /// <summary>
    /// Called when the EV value changes.
    /// <param name="newEV">The updated EV value used to set the current amplitude.</param>
    /// </summary>
    private void HandleEVChanged(float newEV)
    {
        if (IsWobbleActive())
            ApplyAmplitude(latestAmplitude);
    }

    /// <summary>
    /// Turns the wobble shader effect on or off by adjusting the amplitude parameter.
    /// <param name="onOrOff">True to enable wobble, false to disable it.</param>
    /// </summary>
    public void TriggerWobble(bool onOrOff)
    {
        float targetAmplitude = onOrOff ? latestAmplitude : 0f;
        ApplyAmplitude(targetAmplitude);
    }

    /// <summary>
    /// Applies the amplitude value to the material.
    /// </summary>
    private void ApplyAmplitude(float value)
    {
        targetMaterial.SetFloat(amplitudeProperty, value);
    }

    /// <summary>
    /// Checks if the current wobble is active (amplitude is not zero).
    /// </summary>
    private bool IsWobbleActive()
    {
        return targetMaterial.GetFloat(amplitudeProperty) != 0f;
    }
}
