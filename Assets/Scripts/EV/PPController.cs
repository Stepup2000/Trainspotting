using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PPController : MonoBehaviour
{
    public static PPController Instance { get; private set; }

    [Header("References")]
    public Volume mainVolume;
    public Volume transitionVolume;

    [Header("Post Processing Data")]
    public List<PostProcessingData> postProcessingDataList;

    [Header("Transition Settings")]
    public float transitionDuration = 5f;

    private Coroutine transitionCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (transitionVolume != null)
            transitionVolume.weight = 0f;
    }

    public void SetPostProcessingStyle(PostProcessingStyle style)
    {
        foreach (var data in postProcessingDataList)
        {
            if (data.style == style)
            {
                if (transitionCoroutine != null)
                    StopCoroutine(transitionCoroutine);

                transitionCoroutine = StartCoroutine(TransitionToProfile(data.profile));
                return;
            }
        }

        Debug.LogWarning($"Post-processing profile for '{style}' not found.");
    }

    private IEnumerator TransitionToProfile(VolumeProfile newProfile)
    {
        transitionVolume.profile = newProfile;

        float time = 0f;
        while (time < transitionDuration)
        {
            float t = time / transitionDuration;
            mainVolume.weight = 1f - t;
            transitionVolume.weight = t;

            time += Time.deltaTime;
            yield return null;
        }

        mainVolume.profile = newProfile;
        mainVolume.weight = 1f;
        transitionVolume.weight = 0f;
        transitionVolume.profile = null;

        transitionCoroutine = null;
    }
}
