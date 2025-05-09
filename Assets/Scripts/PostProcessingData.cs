using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class PostProcessingData
{
    public PostProcessingStyle style;
    public VolumeProfile profile;

    public float minEV;
    public float maxEV;
}
