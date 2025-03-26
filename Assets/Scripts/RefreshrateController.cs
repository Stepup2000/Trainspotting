using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Adjusts Time.fixedDeltaTime based on the refresh rate of an active XR device.
/// </summary>
public class RefreshrateController : MonoBehaviour
{
    private void Start()
    {
        AttemptChangeDeltaTime();
    }

    /// <summary>
    /// Attempts to set Time.fixedDeltaTime if an XR device is active; retries if none is detected.
    /// </summary>
    private void AttemptChangeDeltaTime()
    {
        if (XRSettings.isDeviceActive)
            ChangeDeltaTime();
        else
        {
            Debug.LogWarning("No XR device is active.");
            Invoke(nameof(AttemptChangeDeltaTime), 0.25f);
        }
    }

    /// <summary>
    /// Updates Time.fixedDeltaTime based on the XR device's refresh rate.
    /// </summary>
    private void ChangeDeltaTime()
    {
        float refreshRate = XRDevice.refreshRate;

        if (refreshRate > 0)
            Time.fixedDeltaTime = 1.0f / refreshRate;
        else
        {
            Debug.LogWarning("Invalid XR device refresh rate: " + refreshRate);
            Invoke(nameof(AttemptChangeDeltaTime), 0.25f);
        }
    }
}
