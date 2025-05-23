using UnityEngine;

public class WobbleEVApplier : BaseEVApplier
{
    [SerializeField] protected Material targetMaterial;
    [SerializeField] protected string amplitudeProperty = "_Amplitude";

    protected float oldAmplitude = 0f;

    protected override void OnEnable()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned.");
            return;
        }

        oldAmplitude = targetMaterial.GetFloat(amplitudeProperty);
        TriggerWobble(false);

        base.OnEnable();

        EVController.Instance.ToggleWobble.AddListener(TriggerWobble);
        TriggerWobble(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.ToggleWobble.RemoveListener(TriggerWobble);
        ApplyAmplitude(oldAmplitude);
    }

    /// <summary>
    /// Turns the wobble shader effect on or off by adjusting the amplitude parameter.
    /// <param name="onOrOff">True to enable wobble, false to disable it.</param>
    /// </summary>
    public void TriggerWobble(bool onOrOff)
    {
        //if (!CanApplyEffect() && onOrOff) return;

        float targetAmplitude = onOrOff ? oldAmplitude : 0f;
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
