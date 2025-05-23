using UnityEngine;
using FMODUnity;
using System.Collections;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
   public static AudioManager instance { get; private set; }

   private Dictionary<string, EventInstance> persistentEvents = new();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one AudioManager in the scene!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //used for simple one time sound effects
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    //used for all sound effects that overlap on top of each other
    public EventInstance PlayEventInstance(EventReference sound, GameObject source, List<AudioParameter> parameters = null)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(source));
        // Apply parameters
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                instance.setParameterByName(param.name, param.value);
            }
        }
        instance.start();
        instance.release(); // cleanup
        return instance;
    }

    //used for all sound effects that loop or for whenever you want to use paramaters with them
    public EventInstance CreatePersistentEvent(EventReference sound, GameObject source, List<AudioParameter> parameters = null)
    {
        Debug.Log("Sound plays");
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
        return instance;
    }

    public void StopPersistentEvent(string key, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
    {
        if (persistentEvents.TryGetValue(key, out EventInstance instance))
        {
            instance.stop(stopMode);   // Stop the sound (with fadeout or immediate)
            instance.release();        // Release FMOD resources
            persistentEvents.Remove(key); // Clean up tracking
        }
        else
        {
            Debug.LogWarning($"Persistent sound with key '{key}' not found.");
        }
    }


    public void PlayWithDelay(AudioReceiver receiver)
    {
        if (receiver != null && !receiver.soundEffect.IsNull)
        {
            StartCoroutine(PlaySoundCoroutine(receiver));
        }
    }

    private IEnumerator PlaySoundCoroutine(AudioReceiver receiver)
    {
        yield return new WaitForSeconds(receiver.delay);

        if (receiver.soundSource != null)
        {
            switch (receiver.mode)
            {
                case AudioReceiver.PlaybackMode.OneShot:
                    PlayOneShot(receiver.soundEffect, receiver.soundSource.transform.position);
                    break;

                case AudioReceiver.PlaybackMode.Instance:
                    PlayEventInstance(receiver.soundEffect, receiver.soundSource, receiver.parameters);
                    break;

                case AudioReceiver.PlaybackMode.Persistent:
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
