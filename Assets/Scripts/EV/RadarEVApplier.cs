using UnityEngine;

public class RadarEVApplier : BaseEVApplier
{
    [SerializeField] private ParticleSystem radar;

    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.ToggleRadar.AddListener(TriggerRadar);
        TriggerRadar(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.ToggleRadar.RemoveListener(TriggerRadar);
    }


    /// <summary>
    /// Starts or stops the radar effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the radar effect.</param>
    /// </summary>
    public void TriggerRadar(bool onOrOff)
    {
        if (!CanApplyEffect() && onOrOff == true) return;

        if (onOrOff == true)
            radar.Play();
        else
            radar.Stop();
    }
}
