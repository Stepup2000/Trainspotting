using UnityEngine;

public class PostProcessingRandomizer : BaseEVApplier
{
    private bool canTrigger = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.TogglePostProcessing.AddListener(TurnEffectOn);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.TogglePostProcessing.RemoveListener(TurnEffectOn);
    }

    private void ApplyProfileBasedOnEV(bool onOrOff)
    {
        var dataList = PPController.Instance.postProcessingDataList;

        if (dataList == null || dataList.Count == 0)
        {
            Debug.LogWarning("No post-processing data available.");
            return;
        }

        foreach (var data in dataList)
        {
            if (currentEV >= data.minEV && currentEV < data.maxEV)
            {
                PPController.Instance.SetPostProcessingStyle(data.style);
                Debug.Log(data.style);
                return;
            }
        }

        Debug.LogWarning("No matching style found for EV: " + currentEV);
    }

    protected void TurnEffectOn(bool onOrOff)
    {
        canTrigger = true;
        ApplyProfileBasedOnEV(onOrOff);
    }

    /// <summary>
    /// Called when the EV value changes. Override this method to implement custom behavior.
    /// </summary>
    /// <param name="newEV">The new EV value.</param>
    protected override void OnEVChanged(float newEV)
    {
        if (canTrigger == false) return;
        currentEV = newEV;
        ApplyProfileBasedOnEV(true);
    }
}
