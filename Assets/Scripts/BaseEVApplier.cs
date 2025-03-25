using UnityEngine;

/// <summary>
/// Base class for components that react to EV changes.
/// </summary>
public class BaseEVApplier : MonoBehaviour
{
    /// <summary>
    /// Subscribes to the EV change event when the object is enabled.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (EVController.Instance != null)
            EVController.Instance.OnEVChanged.AddListener(OnEVChanged);
        else
            Debug.LogWarning("Could not find EVController");
    }

    /// <summary>
    /// Unsubscribes from the EV change event when the object is disabled.
    /// </summary>
    protected virtual void OnDisable()
    {
        if (EVController.Instance != null)
            EVController.Instance.OnEVChanged.RemoveListener(OnEVChanged);
    }

    /// <summary>
    /// Called when the EV value changes. Override this method to implement custom behavior.
    /// </summary>
    /// <param name="newEV">The new EV value.</param>
    protected virtual void OnEVChanged(float newEV)
    {
        //Debug.Log($"EV changed: {newEV}");
    }
}
