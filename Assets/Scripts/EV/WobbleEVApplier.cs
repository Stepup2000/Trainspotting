using UnityEngine;

public class GrassEVApplier : BaseEVApplier
{
    [SerializeField] protected Material targetMaterial;
    [SerializeField] protected string amplitudeProperty = "_Amplitude";

    protected float latestAmplitude = 0f;

    protected override void OnEnable()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned.");
            return;
        }

        latestAmplitude = targetMaterial.GetFloat(amplitudeProperty);
        TriggerWobble(false);

        base.OnEnable();
        if (EVController.Instance != null)
        {
            EVController.Instance.ToggleWobble.AddListener(TriggerWobble);
            TriggerWobble(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (EVController.Instance != null)
        {
            EVController.Instance.ToggleWobble.RemoveListener(TriggerWobble);
        }
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
    protected void ApplyAmplitude(float value)
    {
        targetMaterial.SetFloat(amplitudeProperty, value);
    }
}
