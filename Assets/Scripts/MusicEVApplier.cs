using UnityEngine;

public class MusicEVApplier : BaseEVApplier
{
    [SerializeField] private AudioData audioData;

    /// <summary>
    /// Triggers the backgroud music based on EV
    /// </summary>
    public void TriggerBackgroundMusic()
    {
        AudioParameter audioParameter = new AudioParameter();
        audioParameter.name = "BackgroundMusic";
        audioParameter.value = currentEV;
        audioData.parameters.Add(audioParameter);
        AudioManager.instance.CreatePersistentEvent(audioData.soundEffect, audioData.soundSource, audioData.parameters);
    }
}
