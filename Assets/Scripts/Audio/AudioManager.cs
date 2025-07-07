using UnityEngine;
using FMODUnity;
using System.Collections;
using FMOD.Studio;
using System.Collections.Generic;

/// <summary>
/// Manages audio playback using FMOD, including one-shot, instance, and persistent sounds.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the AudioManager.
    /// </summary>
    public static AudioManager instance { get; private set; }

    private Dictionary<EventReference, EventInstance> persistentEvents = new();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one AudioManager in the scene!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Plays a one-shot sound at a specified world position.
    /// </summary>
    /// <param name="sound">The FMOD event to play.</param>
    /// <param name="worldPos">The world position of the sound.</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /// <summary>
    /// Plays a sound as an instance that allows overlapping and parameter customization.
    /// </summary>
    /// <param name="sound">The FMOD event to play.</param>
    /// <param name="source">The GameObject serving as the sound source.</param>
    /// <param name="parameters">Optional parameters to set on the event.</param>
    /// <returns>The created FMOD event instance.</returns>
    public EventInstance PlayEventInstance(EventReference sound, GameObject source, List<AudioParameter> parameters = null)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(source));
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                instance.setParameterByName(param.name, param.value);
            }
        }
        instance.start();
        instance.release();
        return instance;
    }

    /// <summary>
    /// Creates and starts a persistent sound event that can be stopped later.
    /// </summary>
    /// <param name="sound">The FMOD event to play.</param>
    /// <param name="source">The GameObject serving as the sound source.</param>
    /// <param name="parameters">Optional parameters to set on the event.</param>
    /// <returns>The created FMOD event instance.</returns>
    public EventInstance CreatePersistentEvent(EventReference sound, GameObject source, List<AudioParameter> parameters = null)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(source));
        persistentEvents.Add(sound, instance);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                instance.setParameterByName(param.name, param.value);
            }
        }
        instance.start();
        return instance;
    }

    /// <summary>
    /// Stops and removes a persistent event if it exists.
    /// </summary>
    /// <param name="key">The event reference used as the dictionary key.</param>
    /// <param name="stopMode">Stop mode (e.g., allow fadeout).</param>
    public void StopPersistentEvent(EventReference key)
    {
        if (persistentEvents.TryGetValue(key, out EventInstance instance))
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
            persistentEvents.Remove(key);
            Debug.Log("Sound is stopping");
        }
        else
        {
            Debug.LogWarning($"Persistent sound with key '{key}' not found.");
        }
    }

    public void StopAllPersistentSounds()
    {
        var keys = new List<EventReference>(persistentEvents.Keys);
        foreach (EventReference reference in keys)
        {
            StopPersistentEvent(reference);
        }
    }


    /// <summary>
    /// Plays an audio event after a specified delay.
    /// </summary>
    /// <param name="receiver">Audio data containing playback settings.</param>
    public void PlayWithDelay(AudioData receiver)
    {
        if (receiver != null && !receiver.soundEffect.IsNull)
        {
            StartCoroutine(PlaySoundCoroutine(receiver));
        }
    }

    /// <summary>
    /// Coroutine that handles delayed audio playback based on AudioData.
    /// </summary>
    /// <param name="receiver">Audio data containing playback settings.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator PlaySoundCoroutine(AudioData receiver)
    {
        yield return new WaitForSeconds(receiver.delay);

        if (receiver.soundSource != null)
        {
            switch (receiver.mode)
            {
                case AudioData.PlaybackMode.OneShot:
                    PlayOneShot(receiver.soundEffect, receiver.soundSource.transform.position);
                    break;

                case AudioData.PlaybackMode.Instance:
                    PlayEventInstance(receiver.soundEffect, receiver.soundSource, receiver.parameters);
                    break;

                case AudioData.PlaybackMode.Persistent:
                    CreatePersistentEvent(receiver.soundEffect, receiver.soundSource, receiver.parameters);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("AudioReceiver has no soundSource.");
        }
    }
}
