using System.Collections;
using UnityEngine;

public class DissolveEVApplier : BaseEVApplier
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private string cutOffProperty = "_CutOffHeight";
    [SerializeField] private float dissolveDuration = 2f;
    [SerializeField] private float minSize = 35f;

    private float maxSize;
    private Coroutine dissolveCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned.");
            return;
        }

        if (EVController.Instance != null)
            EVController.Instance.ToggleDissolve.AddListener(TriggerDissolve);

        maxSize = targetMaterial.GetFloat(cutOffProperty);
        if (maxSize == 0f) maxSize = 1f; // fallback default

        TriggerDissolve(false); // ensure it's disabled on start
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (EVController.Instance != null)
            EVController.Instance.ToggleDissolve.RemoveListener(TriggerDissolve);

        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
            dissolveCoroutine = null;
        }

        targetMaterial.SetFloat(cutOffProperty, maxSize);
    }

    public void TriggerDissolve(bool onOrOff)
    {
        if (!CanApplyEffect() && onOrOff) return;
        if (targetMaterial == null) return;

        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
            dissolveCoroutine = null;
        }

        if (onOrOff)
        {
            dissolveCoroutine = StartCoroutine(DissolveFadeIn());
        }
        else
        {
            targetMaterial.SetFloat(cutOffProperty, minSize);
        }
    }

    private IEnumerator DissolveFadeIn()
    {
        targetMaterial.SetFloat(cutOffProperty, minSize);

        float elapsed = 0f;
        while (elapsed < dissolveDuration)
        {
            float t = elapsed / dissolveDuration;
            float value = Mathf.Lerp(minSize, maxSize, t);
            targetMaterial.SetFloat(cutOffProperty, value);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetMaterial.SetFloat(cutOffProperty, maxSize);
        dissolveCoroutine = null;
    }
}
