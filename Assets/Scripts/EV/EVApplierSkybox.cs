using System.Collections;
using UnityEngine;

public class EVApplierSkybox : BaseEVApplier
{
    [SerializeField]
    protected Material skyboxMaterial;
    [SerializeField]
    protected Material targetSkyboxMaterial;

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
    protected Material initialTargetSkyboxMaterial;

    [SerializeField]
    private float transitionDuration = 5f; // Time to transition from one skybox to the other
    private bool transitioningToTarget = false;
    private float transitionProgress = 0f;

    /// <summary>
    /// Initializes the skybox materials, their properties, and starts the transition cycle.
    /// </summary>
    private void Start()
    {
        if (skyboxMaterial == null || targetSkyboxMaterial == null)
        {
            Debug.LogError("Skybox materials not assigned!");
            return;
        }

        initialSkyboxMaterial = new Material(skyboxMaterial);
        initialTargetSkyboxMaterial = new Material(targetSkyboxMaterial);

        InitializeMaterialProperties(skyboxMaterial);
        InitializeMaterialProperties(targetSkyboxMaterial);

        StartCoroutine(TransitionCycle());
    }

    /// <summary>
    /// Initializes material properties by extracting values from the given material.
    /// </summary>
    /// <param name="material">The material to initialize properties from.</param>
    private void InitializeMaterialProperties(Material material)
    {
        zenithBlend = material.GetFloat("_ZenithBlend");
        nadirBlend = material.GetFloat("_NadirBlend");
        horizonBlend = material.GetFloat("_HorizonBlend");
        skyColor = material.GetColor("_SkyColor");
        groundColor = material.GetColor("_GroundColor");
        horizonColor = material.GetColor("_HorizonColor");
        starHeight = material.GetFloat("_StarHeight");
        starPower = material.GetFloat("_StarPower");
        starIntensity = material.GetFloat("_StarIntensity");
        starRotation = material.GetFloat("_StarRotation");
    }

    /// <summary>
    /// Updates the skybox material properties with the current values of the associated properties.
    /// </summary>
    /// <param name="material">The material whose properties will be updated.</param>
    private void UpdateSkyboxProperties(Material material)
    {
        if (material != null)
        {
            material.SetFloat("_ZenithBlend", zenithBlend);
            material.SetFloat("_NadirBlend", nadirBlend);
            material.SetFloat("_HorizonBlend", horizonBlend);
            material.SetColor("_SkyColor", skyColor);
            material.SetColor("_GroundColor", groundColor);
            material.SetColor("_HorizonColor", horizonColor);
            material.SetFloat("_StarHeight", starHeight);
            material.SetFloat("_StarPower", starPower);
            material.SetFloat("_StarIntensity", starIntensity);
            material.SetFloat("_StarRotation", starRotation);
        }
    }

    /// <summary>
    /// Starts the cycle of transitioning between the two skybox materials. It will repeatedly transition from one to the other.
    /// </summary>
    /// <returns>An IEnumerator that represents the ongoing transition cycle.</returns>
    private IEnumerator TransitionCycle()
    {
        while (true)
        {
            yield return StartCoroutine(TransitionSkybox(true));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(TransitionSkybox(false));
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Handles the transition between the current skybox material and the target skybox material.
    /// It smoothly interpolates properties of the materials over a specified duration.
    /// </summary>
    /// <param name="toTarget">True if transitioning to the target material, false if transitioning back to the initial material.</param>
    /// <returns>An IEnumerator that represents the transition process.</returns>
    private IEnumerator TransitionSkybox(bool toTarget)
    {
        float startTime = Time.time;
        Material fromMaterial = toTarget ? skyboxMaterial : targetSkyboxMaterial;
        Material toMaterial = toTarget ? targetSkyboxMaterial : skyboxMaterial;

        while (transitionProgress < 1f)
        {
            transitionProgress = (Time.time - startTime) / transitionDuration;
            transitionProgress = Mathf.Clamp01(transitionProgress);

            BlendSkyboxes(fromMaterial, toMaterial, transitionProgress);

            yield return null;
        }

        BlendSkyboxes(fromMaterial, toMaterial, 1f);
    }

    /// <summary>
    /// Blends the properties between two materials based on the transition progress.
    /// The properties include blend values, colors, and star-related values.
    /// </summary>
    /// <param name="fromMaterial">The material to blend from.</param>
    /// <param name="toMaterial">The material to blend to.</param>
    /// <param name="blend">A value between 0 and 1 representing the progress of the transition.</param>
    private void BlendSkyboxes(Material fromMaterial, Material toMaterial, float blend)
    {
        skyboxMaterial.SetFloat("_ZenithBlend", Mathf.Lerp(fromMaterial.GetFloat("_ZenithBlend"), toMaterial.GetFloat("_ZenithBlend"), blend));
        skyboxMaterial.SetFloat("_NadirBlend", Mathf.Lerp(fromMaterial.GetFloat("_NadirBlend"), toMaterial.GetFloat("_NadirBlend"), blend));
        skyboxMaterial.SetFloat("_HorizonBlend", Mathf.Lerp(fromMaterial.GetFloat("_HorizonBlend"), toMaterial.GetFloat("_HorizonBlend"), blend));
        skyboxMaterial.SetColor("_SkyColor", Color.Lerp(fromMaterial.GetColor("_SkyColor"), toMaterial.GetColor("_SkyColor"), blend));
        skyboxMaterial.SetColor("_GroundColor", Color.Lerp(fromMaterial.GetColor("_GroundColor"), toMaterial.GetColor("_GroundColor"), blend));
        skyboxMaterial.SetColor("_HorizonColor", Color.Lerp(fromMaterial.GetColor("_HorizonColor"), toMaterial.GetColor("_HorizonColor"), blend));
        skyboxMaterial.SetFloat("_StarHeight", Mathf.Lerp(fromMaterial.GetFloat("_StarHeight"), toMaterial.GetFloat("_StarHeight"), blend));
        skyboxMaterial.SetFloat("_StarPower", Mathf.Lerp(fromMaterial.GetFloat("_StarPower"), toMaterial.GetFloat("_StarPower"), blend));
        skyboxMaterial.SetFloat("_StarIntensity", Mathf.Lerp(fromMaterial.GetFloat("_StarIntensity"), toMaterial.GetFloat("_StarIntensity"), blend));
        skyboxMaterial.SetFloat("_StarRotation", Mathf.Lerp(fromMaterial.GetFloat("_StarRotation"), toMaterial.GetFloat("_StarRotation"), blend));
    }

    /// <summary>
    /// This function is called when the exposure value (EV) changes. 
    /// Currently, it calls the base method but can be extended to adjust behavior based on the EV change.
    /// </summary>
    /// <param name="newEV">The new exposure value.</param>
    protected override void OnEVChanged(float newEV)
    {
        base.OnEVChanged(newEV);
    }
}
