using System.Collections;
using UnityEngine;

public class GrassEVApplier : BaseEVApplier
{
    [SerializeField] protected Material targetMaterial;
    [SerializeField] protected string growProperty = "_Grow";
    [SerializeField] protected float transitionDuration = 1f; // Time in seconds for transition

    protected float latestAmplitude = 0f;
    protected Coroutine transitionCoroutine;

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
        {
            EVController.Instance.ToggleGrass.AddListener(TriggerGrass);
            TriggerGrass(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (EVController.Instance != null)
        {
            EVController.Instance.ToggleGrass.RemoveListener(TriggerGrass);
        }

        // Stop any transition and set amplitude to 0 instantly
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        ApplyGrowth(0f);
    }

    /// <summary>
    /// Turns the grass shader effect on or off by adjusting the amplitude parameter over time.
    /// </summary>
    /// <param name="onOrOff">True to enable wobble, false to disable it.</param>
    public void TriggerGrass(bool onOrOff)
    {
        float targetAmplitude = onOrOff ? 1f : 0f;

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(SmoothTransition(targetAmplitude));
    }

    /// <summary>
    /// Coroutine that smoothly transitions the amplitude value.
    /// </summary>
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
    /// Applies the growth value to the material.
    /// </summary>
    protected void ApplyGrowth(float value)
    {
        targetMaterial.SetFloat(growProperty, value);
    }
}
