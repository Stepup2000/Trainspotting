using UnityEngine;

public class PixiedustEVApplier : BaseEVApplier
{
    [SerializeField] private ParticleSystem pixiedust;

    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.TogglePixiedust.AddListener(TriggerPixiedust);
        TriggerPixiedust(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.TogglePixiedust.RemoveListener(TriggerPixiedust);
    }


    /// <summary>
    /// Starts or stops the pixiedust effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the pixiedust effect.</param>
    /// </summary>
    public void TriggerPixiedust(bool onOrOff)
    {
        if (!CanApplyEffect() && onOrOff == true) return;
        if (onOrOff)
        {
            if (!pixiedust.isPlaying)
                pixiedust.Play();
        }
        else
        {
            if (pixiedust.isPlaying)
                pixiedust.Stop();
        }
    }
}
