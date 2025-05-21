using UnityEngine;
using System.Collections;

public class AuroraEVApplier : BaseEVApplier
{
    [SerializeField] private GameObject auroraMesah;
    [SerializeField] private float dissolveDuration = 1f;
    [SerializeField] private Material auroraMaterial;
    [SerializeField] private float offDissolveValue = 20;

    private float originalDissolvePower;
    private Coroutine dissolveCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.ToggleAurora.AddListener(TriggerAurora);

        originalDissolvePower = auroraMaterial.GetFloat("_Disolve_Power");

        auroraMaterial.SetFloat("_Disolve_Power", offDissolveValue);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.ToggleAurora.RemoveListener(TriggerAurora);
        auroraMaterial.SetFloat("_Disolve_Power", originalDissolvePower);
    }

    /// <summary>
    /// Starts or stops the aurora dissolve effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the dissolve effect.</param>
    /// </summary>
    public void TriggerAurora(bool onOrOff)
    {
        if (!CanApplyEffect() && onOrOff) return;

        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
        }

        dissolveCoroutine = StartCoroutine(DissolveCoroutine(onOrOff));
    }

    /// <summary>
    /// Starts or stops the aurora dissolve effect gradually with a coroutine.
    /// <param name="turnOn">Boolean indicating whether to play or stop the Aurora effect.</param>
    /// </summary>
    private IEnumerator DissolveCoroutine(bool turnOn)
    {
        if (auroraMaterial == null) yield break;

        float startValue = turnOn ? offDissolveValue : originalDissolvePower;
        float endValue = turnOn ? originalDissolvePower : offDissolveValue;

        float elapsed = 0f;
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dissolveDuration);
            float current = Mathf.Lerp(startValue, endValue, t);
            auroraMaterial.SetFloat("_Disolve_Power", current);
            yield return null;
        }

        auroraMaterial.SetFloat("_Disolve_Power", endValue);
    }
}
