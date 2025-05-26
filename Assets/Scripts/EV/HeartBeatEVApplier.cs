using UnityEngine;

public class HeartBeatEVApplier : BaseEVApplier
{
    [SerializeField] private AudioData receiver;

    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.ToggleHeartbeat.AddListener(TriggerHeartbeat);
        TriggerHeartbeat(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.ToggleHeartbeat.RemoveListener(TriggerHeartbeat);
        TriggerHeartbeat(false);
    }


    /// <summary>
    /// Starts or stops the heartbeat effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the heartbeat effect.</param>
    /// </summary>
    public void TriggerHeartbeat(bool onOrOff)
    {
        if (!CanApplyEffect() && onOrOff == true) return;

        if (onOrOff == true)
            AudioManager.instance.CreatePersistentEvent(receiver.soundEffect, receiver.soundSource, receiver.parameters);
        else
            AudioManager.instance.StopPersistentEvent(receiver.soundEffect, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EVController.Instance.ToggleRadar.Invoke(true);
    }
}
