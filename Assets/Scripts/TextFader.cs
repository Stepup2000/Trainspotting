using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Fades a TextMeshProUGUI element in, waits, then fades it out.
/// </summary>
public class TextFader : MonoBehaviour
{
    /// <summary>
    /// Duration for the fade-in effect.
    /// </summary>
    [SerializeField] private float fadeInDuration = 1f;

    /// <summary>
    /// How long the text stays fully visible before fading out.
    /// </summary>
    [SerializeField] private float visibleDuration = 2f;

    /// <summary>
    /// Duration for the fade-out effect.
    /// </summary>
    [SerializeField] private float fadeOutDuration = 1f;

    /// <summary>
    /// The TextMeshProUGUI component attached to this GameObject.
    /// </summary>
    [SerializeField ]private TMP_Text textComponent;

    /// <summary>
    /// Reference to the currently running fade coroutine.
    /// </summary>
    private Coroutine fadeCoroutine;

    /// <summary>
    /// Gets the TextMeshProUGUI component when the object is initialized.
    /// </summary>
    void Awake()
    {
        if (textComponent == null)
        {
            Debug.LogWarning("Text component does not exist");
            Destroy(gameObject);
            return;
        }

        // Ensure it's fully transparent before starting fade-in
        Color color = textComponent.color;
        color.a = 0f;
        textComponent.color = color;

        FadeInAndOut();
    }

    /// <summary>
    /// Starts the full sequence: fade in → stay → fade out.
    /// </summary>
    public void FadeInAndOut()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeSequence());
    }

    /// <summary>
    /// Runs the fade in → wait → fade out sequence.
    /// </summary>
    private IEnumerator FadeSequence()
    {
        yield return FadeTo(1f, fadeInDuration);
        yield return new WaitForSeconds(visibleDuration);
        yield return FadeTo(0f, fadeOutDuration);
    }

    /// <summary>
    /// Smoothly fades the text to a target alpha over the given duration.
    /// </summary>
    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = textComponent.alpha;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            textComponent.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            time += Time.deltaTime;
            yield return null;
        }

        textComponent.alpha = targetAlpha;
    }
}
