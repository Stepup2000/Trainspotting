using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Fades an image from full opacity to transparent when the scene starts.
/// Sets full alpha at Awake for editor visibility and resets alpha on destroy.
/// </summary>
public class SceneIntroFade : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 1.5f;

    [SerializeField]
    private Image fadeImage;

    /// <summary>
    /// Initializes the fadeImage reference if not assigned,
    /// sets the image fully opaque, and starts the fade-out coroutine.
    /// </summary>
    void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<Image>();

        if (fadeImage == null)
        {
            Debug.LogWarning("No Image assigned or found on this GameObject.");
            enabled = false;
            return;
        }

        SetAlpha(1f);
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// Resets the image alpha to fully transparent when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (fadeImage != null)
            SetAlpha(0f);
    }

    /// <summary>
    /// Coroutine that smoothly fades the image alpha from 1 (opaque) to 0 (transparent) over fadeDuration seconds.
    /// </summary>
    /// <returns>IEnumerator for coroutine handling.</returns>
    private IEnumerator FadeOut()
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(1f, 0f, t);
            fadeImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
    }

    /// <summary>
    /// Sets the alpha value of the fadeImage's color.
    /// </summary>
    /// <param name="alpha">The target alpha value, from 0 (transparent) to 1 (opaque).</param>
    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}
