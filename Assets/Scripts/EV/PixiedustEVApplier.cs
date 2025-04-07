using UnityEngine;

public class PixiedustEVApplier : MonoBehaviour
{
    [SerializeField] private ParticleSystem pixiedust;

    private void OnEnable()
    {
        EVController.Instance.OnEVChanged.AddListener(HandleEVChanged);
        EVController.Instance.ChangePixydust.AddListener(TriggerPixiedust);
    }

    private void OnDisable()
    {
        EVController.Instance.OnEVChanged.RemoveListener(HandleEVChanged);
        EVController.Instance.ChangePixydust.RemoveListener(TriggerPixiedust);
    }

    /// <summary>
    /// Called when the EV value changes.
    /// <param name="newEV">The updated EV value that triggers the effect based on its sign.</param>
    /// </summary>
    private void HandleEVChanged(float newEV)
    {
        
    }

    /// <summary>
    /// Starts or stops the pixiedust effect.
    /// <param name="onOrOff">Boolean indicating whether to play or stop the pixiedust effect.</param>
    /// </summary>
    public void TriggerPixiedust(bool onOrOff)
    {
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
