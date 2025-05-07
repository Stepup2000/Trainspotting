using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PPController : MonoBehaviour
{
    public static PPController Instance { get; private set; }

    [Header("References")]
    public Volume volume;

    [Header("Post Processing Data")]
    public List<PostProcessingData> postProcessingDataList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist between scenes
    }

    public void SetPostProcessingStyle(PostProcessingStyle style)
    {
        foreach (var data in postProcessingDataList)
        {
            if (data.style == style)
            {
                volume.profile = data.profile;
                return;
            }
        }

        Debug.LogWarning($"Post-processing profile for '{style}' not found.");
    }
}
