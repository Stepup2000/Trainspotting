using UnityEngine;

public class EVApplierSkybox : MonoBehaviour
{
    [SerializeField]
    private Material skyboxMaterial;

    // New properties with specified ranges
    [SerializeField, Range(0.01f, 50f)]
    private float ZenithBlend = 1f;

    [SerializeField, Range(0.01f, 50f)]
    private float NadirBlend = 1f;

    [SerializeField, Range(0.01f, 50f)]
    private float HorizonBlend = 1f;

    [SerializeField]
    private Color SkyColor = Color.white;

    [SerializeField]
    private Color GroundColor = Color.black;

    [SerializeField]
    private Color HorizonColor = Color.gray;

    [SerializeField, Range(150f, 1000f)]
    private float StarHeight = 500f;

    [SerializeField, Range(22.5f, 40f)]
    private float StarPower = 30f;

    [SerializeField, Range(1000f, 10000f)]
    private float StarIntensity = 5000f;

    private Material initialSkyboxMaterial;

    /// <summary>
    /// Called when the script starts. Initializes the skybox material and stores its initial state.
    /// </summary>
    void Start()
    {
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox material not assigned!");
            return;
        }

        initialSkyboxMaterial = new Material(skyboxMaterial);

        UpdateSkyboxProperties();
    }

    /// <summary>
    /// Called when the script is disabled. Reverts the skybox properties to their initial state.
    /// </summary>
    void OnDisable()
    {
        RevertSkyboxProperties();
    }

    /// <summary>
    /// Called when the application is quit. Reverts the skybox properties to their initial state.
    /// </summary>
    void OnApplicationQuit()
    {
        RevertSkyboxProperties();
    }

    /// <summary>
    /// Called every frame. Checks for input to update the skybox properties.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UpdateSkyboxProperties();
    }

    /// <summary>
    /// Updates the skybox material properties based on the current values.
    /// This method applies the specified parameters (ZenithBlend, NadirBlend, etc.) to the material.
    /// </summary>
    private void UpdateSkyboxProperties()
    {
        if (skyboxMaterial != null)
        {
            // Set the properties to the material
            skyboxMaterial.SetFloat("_ZenithBlend", ZenithBlend);
            skyboxMaterial.SetFloat("_NadirBlend", NadirBlend);
            skyboxMaterial.SetFloat("_HorizonBlend", HorizonBlend);
            skyboxMaterial.SetColor("_SkyColor", SkyColor);
            skyboxMaterial.SetColor("_GroundColor", GroundColor);
            skyboxMaterial.SetColor("_HorizonColor", HorizonColor);
            skyboxMaterial.SetFloat("_StarHeight", StarHeight);
            skyboxMaterial.SetFloat("_StarPower", StarPower);
            skyboxMaterial.SetFloat("_StarIntensity", StarIntensity);
        }
    }

    /// <summary>
    /// Reverts the skybox material properties to their initial state.
    /// This method restores the material to the original values saved when the script started.
    /// </summary>
    private void RevertSkyboxProperties()
    {
        if (skyboxMaterial != null && initialSkyboxMaterial != null)
            skyboxMaterial.CopyPropertiesFromMaterial(initialSkyboxMaterial);
    }
}
