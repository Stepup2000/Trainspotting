using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Fades a TextMeshProUGUI or Image component in, waits, then fades it out.
/// </summary>
public class UIFader : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float visibleDuration = 2f;
    [SerializeField] private float fadeOutDuration = 1f;

    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private Image imageComponent;

    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (textComponent == null && imageComponent == null)
        {
            Debug.LogWarning("No UI component assigned to fade.");
            Destroy(gameObject);
            return;
        }

        SetAlpha(0f); // Fully transparent at start
        FadeInAndOut();
    }

    public void FadeInAndOut()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return FadeTo(1f, fadeInDuration);
        yield return new WaitForSeconds(visibleDuration);
        yield return FadeTo(0f, fadeOutDuration);
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = GetAlpha();
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (textComponent != null)
        {
            Color color = textComponent.color;
            color.a = alpha;
            textComponent.color = color;
        }

        if (imageComponent != null)
        {
            Color color = imageComponent.color;
            color.a = alpha;
            imageComponent.color = color;
        }
    }

    private float GetAlpha()
    {
        if (textComponent != null)
            return textComponent.color.a;

        if (imageComponent != null)
            return imageComponent.color.a;

        return 1f;
    }
}
