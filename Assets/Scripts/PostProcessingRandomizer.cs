using UnityEngine;

public class PostProcessingRandomizer : BaseEVApplier
{
    private void Start()
    {
        ApplyProfileBasedOnEV();
    }

    private void ApplyProfileBasedOnEV()
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
        ApplyProfileBasedOnEV();
    }
}
