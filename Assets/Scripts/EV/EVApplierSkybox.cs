using UnityEngine;

public class EVApplierSkybox : BaseEVApplier
{
    [SerializeField]
    protected Material skyboxMaterial;

    protected float zenithBlend;
    protected float nadirBlend;
    protected float horizonBlend;
    protected Color skyColor;
    protected Color groundColor;
    protected Color horizonColor;
    protected float starHeight;
    protected float starPower;
    protected float starIntensity;
    protected float starRotation;

    protected Material initialSkyboxMaterial;

    /// <summary>
    /// Called when the script starts. Initializes the skybox material and stores its initial state.
    /// </summary>
    protected void Start()
    {
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox material not assigned!");
            return;
        }

        initialSkyboxMaterial = new Material(skyboxMaterial);

        // Get initial values from the shader
        zenithBlend = skyboxMaterial.GetFloat("_ZenithBlend");
        nadirBlend = skyboxMaterial.GetFloat("_NadirBlend");
        horizonBlend = skyboxMaterial.GetFloat("_HorizonBlend");
        skyColor = skyboxMaterial.GetColor("_SkyColor");
        groundColor = skyboxMaterial.GetColor("_GroundColor");
        horizonColor = skyboxMaterial.GetColor("_HorizonColor");
        starHeight = skyboxMaterial.GetFloat("_StarHeight");
        starPower = skyboxMaterial.GetFloat("_StarPower");
        starIntensity = skyboxMaterial.GetFloat("_StarIntensity");
        starRotation = skyboxMaterial.GetFloat("_StarRotation");

        UpdateSkyboxProperties();
    }

    /// <summary>
    /// Called when the script is disabled. Reverts the skybox properties to their initial state.
    /// </summary>
    protected override void OnDisable()
    {
        RevertSkyboxProperties();
    }

    /// <summary>
    /// Called when the application is quit. Reverts the skybox properties to their initial state.
    /// </summary>
    protected void OnApplicationQuit()
    {
        RevertSkyboxProperties();
    }

    /// <summary>
    /// Called every frame. Checks for input to update the skybox properties.
    /// </summary>
    protected void Update()
    {
        starRotation += 0.01f * Time.deltaTime;
        UpdateSkyboxProperties();
    }

    /// <summary>
    /// Updates the skybox material properties based on the current values.
    /// </summary>
    protected void UpdateSkyboxProperties()
    {
        if (skyboxMaterial != null)
        {
            // Set the properties to the material
            skyboxMaterial.SetFloat("_ZenithBlend", zenithBlend);
            skyboxMaterial.SetFloat("_NadirBlend", nadirBlend);
            skyboxMaterial.SetFloat("_HorizonBlend", horizonBlend);
            skyboxMaterial.SetColor("_SkyColor", skyColor);
            skyboxMaterial.SetColor("_GroundColor", groundColor);
            skyboxMaterial.SetColor("_HorizonColor", horizonColor);
            skyboxMaterial.SetFloat("_StarHeight", starHeight);
            skyboxMaterial.SetFloat("_StarPower", starPower);
            skyboxMaterial.SetFloat("_StarIntensity", starIntensity);
            skyboxMaterial.SetFloat("_StarRotation", starRotation);
        }
    }

    /// <summary>
    /// Reverts the skybox material properties to their initial state.
    /// </summary>
    protected void RevertSkyboxProperties()
    {
        if (skyboxMaterial != null && initialSkyboxMaterial != null)
            skyboxMaterial.CopyPropertiesFromMaterial(initialSkyboxMaterial);
    }

    /// <summary>
    /// Method that gets called when the EV in the EVController changes.
    /// </summary>
    protected override void OnEVChanged(float newEV)
    {
        base.OnEVChanged(newEV);
    }
}
