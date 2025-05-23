using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class AudioParameter
{
    public string name;
    public float value;
}

[System.Serializable]
public class AudioData
{
    public enum PlaybackMode
    {
        OneShot,       // For quick, disposable sounds
        Instance,      // For overlapping SFX
        Persistent     // For looping or long-term control
    }

    //getting FMOD parameters
    [Tooltip("Optional FMOD parameters to pass into the event.")]
    public List<AudioParameter> parameters = new List<AudioParameter>();

    //getting FMOD event
    public EventReference soundEffect;

    //get which gameObject we want the sound to play from
    public GameObject soundSource;

    //delays the start of the sound by this ammount
    public float delay;

    //Selecting which mode to play it at
    public PlaybackMode mode = PlaybackMode.OneShot;
}
