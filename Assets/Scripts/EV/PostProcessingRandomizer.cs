using UnityEngine;

public class PostProcessingRandomizer : BaseEVApplier
{
    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.TogglePostProcessing.AddListener(ApplyProfileBasedOnEV);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.TogglePostProcessing.RemoveListener(ApplyProfileBasedOnEV);
    }

    private void ApplyProfileBasedOnEV(bool onOrOff)
    {
        var dataList = PPController.Instance.postProcessingDataList;

        if (dataList == null || dataList.Count == 0)
        {
            Debug.LogWarning("No post-processing data available.");
            return;
        }

        float clampedEV = Mathf.Clamp(currentEV, -5f, 5f);

        foreach (var data in dataList)
        {
            if (clampedEV >= data.minEV && clampedEV < data.maxEV)
            {
                PPController.Instance.SetPostProcessingStyle(data.style);
                Debug.Log(data.style);
                return;
            }
        }

        Debug.LogWarning("No matching style found for EV: " + currentEV);
    }

    /// <summary>
    /// Called when the EV value changes. Override this method to implement custom behavior.
    /// </summary>
    /// <param name="newEV">The new EV value.</param>
    protected override void OnEVChanged(float newEV)
    {
        currentEV = newEV;
        ApplyProfileBasedOnEV(true);
    }
}
