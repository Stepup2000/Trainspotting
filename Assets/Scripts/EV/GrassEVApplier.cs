using System.Collections;
using UnityEngine;

public class GrassEVApplier : BaseEVApplier
{
    [SerializeField] protected Material targetMaterial;
    [SerializeField] protected string growProperty = "_Grow";
    [SerializeField] protected float transitionDuration = 1f;
    [SerializeField] protected float minAmplifier = 0.2f;
    [SerializeField] protected float maxAmplifier = 3f;

    protected float latestAmplitude = 0f;
    protected Coroutine transitionCoroutine;
    protected GameObject grass;
    protected float originalYScale = 1f;

    /// <summary>
    /// Initializes the component, sets initial growth value, subscribes to grass toggle events, and applies initial scale.
    /// </summary>
    protected override void OnEnable()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned.");
            return;
        }

        latestAmplitude = targetMaterial.GetFloat(growProperty);
        TriggerGrass(false);

        base.OnEnable();
        if (EVController.Instance != null)
            EVController.Instance.ToggleGrass.AddListener(TriggerGrass);

        if (grass == null)
            grass = gameObject;

        originalYScale = grass.transform.localScale.y;
        ApplyScale(currentEV);
    }

    /// <summary>
    /// Cleans up event subscriptions and stops any running transitions on disable.
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();
        if (EVController.Instance != null)
            EVController.Instance.ToggleGrass.RemoveListener(TriggerGrass);

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        ApplyGrowth(0f);
    }

    /// <summary>
    /// Starts a smooth transition of the grass growth effect based on the on/off state.
    /// </summary>
    /// <param name="onOrOff">Whether to enable or disable the grass growth effect.</param>
    public void TriggerGrass(bool onOrOff)
    {
        Debug.Log(CanApplyEffect());
        if (CanApplyEffect())
        {
            float targetAmplitude = onOrOff ? CalculateTransitionAmplitude(currentEV) : 0f;

            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            transitionCoroutine = StartCoroutine(SmoothTransition(targetAmplitude));
        }
    }

    /// <summary>
    /// Coroutine that interpolates the growth value smoothly over time to the target value.
    /// </summary>
    /// <param name="targetValue">Target growth value for the transition.</param>
    protected IEnumerator SmoothTransition(float targetValue)
    {
        float startValue = targetMaterial.GetFloat(growProperty);
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            float value = Mathf.Lerp(startValue, targetValue, t);
            ApplyGrowth(value);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ApplyGrowth(targetValue);
        latestAmplitude = targetValue;
        transitionCoroutine = null;
    }

    /// <summary>
    /// Applies the growth value to the target material's growth property.
    /// </summary>
    /// <param name="value">Growth value to apply.</param>
    protected void ApplyGrowth(float value)
    {
        targetMaterial.SetFloat(growProperty, value);
    }

    /// <summary>
    /// Reacts to EV value changes by updating the grass scale accordingly.
    /// </summary>
    /// <param name="newEV">New EV value.</param>
    protected override void OnEVChanged(float newEV)
    {
        base.OnEVChanged(newEV);

        if (grass != null)
            ApplyScale(newEV);
    }

    /// <summary>
    /// Applies a vertical scale to the grass GameObject based on the amplified EV value.
    /// </summary>
    /// <param name="ev">EV value to calculate scale from.</param>
    protected void ApplyScale(float ev)
    {
        Vector3 scale = grass.transform.localScale;
        float amplifiedY = CalculateAmplifiedY(ev);
        grass.transform.localScale = new Vector3(scale.x, amplifiedY, scale.z);
    }

    /// <summary>
    /// Calculates the amplified vertical scale based on the EV value, clamped within configured triggers.
    /// </summary>
    /// <param name="evValue">EV value.</param>
    /// <returns>Amplified vertical scale factor.</returns>
    protected float CalculateAmplifiedY(float evValue)
    {
        evValue = Mathf.Clamp(evValue, minEVTrigger, maxEVTrigger);

        float t = 1f - ((evValue - minEVTrigger) / (maxEVTrigger - minEVTrigger));

        float amplifier = Mathf.Lerp(minAmplifier, maxAmplifier, t);

        Debug.Log(amplifier);
        return originalYScale * amplifier;
    }

    /// <summary>
    /// Calculates the amplitude value used for the growth transition based on the EV value.
    /// </summary>
    /// <param name="evValue">EV value.</param>
    /// <returns>Amplitude value between 0 and 1.</returns>
    protected float CalculateTransitionAmplitude(float evValue)
    {
        evValue = Mathf.Clamp(evValue, -5f, 5f);
        float t = 1f - (evValue + 5f) / 10f;
        return Mathf.Lerp(0f, 1f, t);
    }
}
