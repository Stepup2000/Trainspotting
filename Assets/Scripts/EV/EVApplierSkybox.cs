using UnityEngine;

/// <summary>
/// Handles transitioning skybox material properties between two states when triggered.
/// </summary>
public class EVApplierSkybox : BaseEVApplier
{
    [SerializeField] protected Material skyboxMaterial;
    [SerializeField] protected Material targetMaterial;
    [SerializeField] protected float transitionSpeed = 1f;
    [SerializeField] protected bool loop = true;

    private Material initialSkyboxMaterial;
    private float transitionProgress = 0f;
    private bool transitioningToTarget = true;
    private bool isActive = false;

    private float initialStarHeight, initialStarPower, initialStarIntensity, initialStarRotation;
    private float targetStarHeight, targetStarPower, targetStarIntensity, targetStarRotation;

    /// <summary>
    /// Subscribes to the ToggleSky event.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        EVController.Instance.ToggleSky.AddListener(TriggerSky);
    }

    /// <summary>
    /// Unsubscribes from the ToggleSky event.
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();
        EVController.Instance.ToggleSky.RemoveListener(TriggerSky);
    }

    /// <summary>
    /// Reverts skybox when the object is destroyed (e.g., on scene unload).
    /// </summary>
    protected void OnDestroy()
    {
        RevertSkyboxProperties();
    }

    /// <summary>
    /// Resets skybox to initial values when the application quits.
    /// </summary>
    protected void OnApplicationQuit()
    {
        RevertSkyboxProperties();
    }

    /// <summary>
    /// Initializes material references and stores initial/target skybox values.
    /// </summary>
    protected void Start()
    {
        if (skyboxMaterial == null || targetMaterial == null)
        {
            Debug.LogError("Skybox or Target Material not assigned!");
            return;
        }

        initialSkyboxMaterial = new Material(skyboxMaterial);

        initialStarHeight = skyboxMaterial.GetFloat("_StarHeight");
        initialStarPower = skyboxMaterial.GetFloat("_StarPower");
        initialStarIntensity = skyboxMaterial.GetFloat("_StarIntensity");
        initialStarRotation = skyboxMaterial.GetFloat("_StarRotation");

        targetStarHeight = targetMaterial.GetFloat("_StarHeight");
        targetStarPower = targetMaterial.GetFloat("_StarPower");
        targetStarIntensity = targetMaterial.GetFloat("_StarIntensity");
        targetStarRotation = targetMaterial.GetFloat("_StarRotation");
    }

    /// <summary>
    /// Handles transition between initial and target skybox properties over time.
    /// </summary>
    protected void Update()
    {
        if (!isActive || skyboxMaterial == null || targetMaterial == null) return;

        transitionProgress += Time.deltaTime;
        float t = Mathf.Clamp01(transitionProgress / transitionSpeed);

        float height = transitioningToTarget
            ? Mathf.Lerp(initialStarHeight, targetStarHeight, t)
            : Mathf.Lerp(targetStarHeight, initialStarHeight, t);

        float power = transitioningToTarget
            ? Mathf.Lerp(initialStarPower, targetStarPower, t)
            : Mathf.Lerp(targetStarPower, initialStarPower, t);

        float intensity = transitioningToTarget
            ? Mathf.Lerp(initialStarIntensity, targetStarIntensity, t)
            : Mathf.Lerp(targetStarIntensity, initialStarIntensity, t);

        float rotation = transitioningToTarget
            ? Mathf.Lerp(initialStarRotation, targetStarRotation, t)
            : Mathf.Lerp(targetStarRotation, initialStarRotation, t);

        skyboxMaterial.SetFloat("_StarHeight", height);
        skyboxMaterial.SetFloat("_StarPower", power);
        skyboxMaterial.SetFloat("_StarIntensity", intensity);
        skyboxMaterial.SetFloat("_StarRotation", rotation);

        if (t >= 1f && loop)
        {
            transitioningToTarget = !transitioningToTarget;
            transitionProgress = 0f;
        }
    }

    /// <summary>
    /// Restores the original skybox material values.
    /// </summary>
    protected void RevertSkyboxProperties()
    {
        if (skyboxMaterial != null && initialSkyboxMaterial != null)
            skyboxMaterial.CopyPropertiesFromMaterial(initialSkyboxMaterial);
    }

    /// <summary>
    /// Starts or stops the sky effect.
    /// </summary>
    /// <param name="onOrOff">True to activate transition, false to revert.</param>
    public void TriggerSky(bool onOrOff)
    {
        isActive = onOrOff;

        if (onOrOff)
        {
            transitionProgress = 0f;
            transitioningToTarget = true;
        }
        else
        {
            RevertSkyboxProperties();
        }
    }
}
