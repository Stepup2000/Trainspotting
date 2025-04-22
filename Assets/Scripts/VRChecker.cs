using UnityEngine;
using UnityEngine.XR;

public class PlayerSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject vrPlayerPrefab;
    [SerializeField] private GameObject pcPlayerPrefab;

    private void Start()
    {
        // Check for missing references
        if (vrPlayerPrefab == null || pcPlayerPrefab == null)
        {
            Debug.LogError("PlayerSwitcher: One or both player prefabs are not assigned.");
            return;
        }

        bool vrConnected;

        // Check if XR system is active
        try
        {
            vrConnected = XRSettings.isDeviceActive;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("PlayerSwitcher: XRSettings not available or XR not set up. Defaulting to PC mode.\n" + ex.Message);
            vrConnected = false;
        }

        // Activate the appropriate prefab
        vrPlayerPrefab.SetActive(vrConnected);
        pcPlayerPrefab.SetActive(!vrConnected);

        Debug.Log($"PlayerSwitcher: {(vrConnected ? "VR player enabled." : "PC player enabled.")}");
    }
}
