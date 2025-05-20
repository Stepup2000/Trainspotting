using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class PlayerSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject vrPlayerPrefab;
    [SerializeField] private GameObject pcPlayerPrefab;

    private bool vrConnected = false;
    private bool switchedToVR = false;
    private bool hasTriedBefore = false;

    private void Start()
    {
        // Check for missing references
        if (vrPlayerPrefab == null || pcPlayerPrefab == null)
        {
            Debug.LogError("PlayerSwitcher: One or both player prefabs are not assigned.");
            return;
        }

        StartCoroutine(CheckForVRCoroutine());
    }

    /// <summary>
    /// Repeatedly checks every 0.5 seconds if a VR device is active.
    /// If detected, switches to the VR player prefab.
    /// </summary>
    private IEnumerator CheckForVRCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (!switchedToVR)
        {
            try
            {
                vrConnected = XRSettings.isDeviceActive;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("PlayerSwitcher: XRSettings not available or XR not set up. Defaulting to PC mode.\n" + ex.Message);
                vrConnected = false;
            }

            if (vrConnected)
            {
                vrPlayerPrefab.SetActive(true);
                pcPlayerPrefab.SetActive(false);

                Debug.Log("PlayerSwitcher: VR player enabled.");
                switchedToVR = true;
            }
            else
            {
                vrPlayerPrefab.SetActive(false);
                pcPlayerPrefab.SetActive(true);
                hasTriedBefore = true;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
