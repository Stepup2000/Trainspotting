using UnityEngine;

public class EVApplierSkybox : BaseEVApplier
{
    [SerializeField] protected Material skyboxMaterial;
    [SerializeField] protected Material targetMaterial;
    [SerializeField] protected float transitionSpeed = 1f;
    [SerializeField] protected bool loop = true;

    private Material initialSkyboxMaterial;
    private float transitionProgress = 0f;
    private bool transitioningToTarget = true;

    private float initialStarHeight, initialStarPower, initialStarIntensity, initialStarRotation;
    private float targetStarHeight, targetStarPower, targetStarIntensity, targetStarRotation;

    protected void Start()
    {
        if (skyboxMaterial == null || targetMaterial == null)
        {
            Debug.LogError("Skybox or Target Material not assigned!");
            return;
        }

        // Backup current material properties
        initialSkyboxMaterial = new Material(skyboxMaterial);

        initialStarHeight = skyboxMaterial.GetFloat("_StarHeight");
        initialStarPower = skyboxMaterial.GetFloat("_StarPower");
        initialStarIntensity = skyboxMaterial.GetFloat("_StarIntensity");
        initialStarRotation = skyboxMaterial.GetFloat("_StarRotation");

        // Fetch target values
        targetStarHeight = targetMaterial.GetFloat("_StarHeight");
        targetStarPower = targetMaterial.GetFloat("_StarPower");
        targetStarIntensity = targetMaterial.GetFloat("_StarIntensity");
        targetStarRotation = targetMaterial.GetFloat("_StarRotation");
    }

    protected void Update()
    {
        if (skyboxMaterial == null || targetMaterial == null) return;

        transitionProgress += Time.deltaTime;
        float t = Mathf.Clamp01(transitionProgress / transitionSpeed);


        // Lerp between original and target based on direction
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

        // Apply interpolated values
        skyboxMaterial.SetFloat("_StarHeight", height);
        skyboxMaterial.SetFloat("_StarPower", power);
        skyboxMaterial.SetFloat("_StarIntensity", intensity);
        skyboxMaterial.SetFloat("_StarRotation", rotation);

        // When done transitioning, flip direction if looping
        if (t >= 1f)
        {
            if (loop)
            {
                transitioningToTarget = !transitioningToTarget;
                transitionProgress = 0f;
            }
        }
    }

    protected override void OnDisable()
    {
        RevertSkyboxProperties();
    }

    protected void OnApplicationQuit()
    {
        RevertSkyboxProperties();
    }

    protected void RevertSkyboxProperties()
    {
        if (skyboxMaterial != null && initialSkyboxMaterial != null)
            skyboxMaterial.CopyPropertiesFromMaterial(initialSkyboxMaterial);
    }

    protected override void OnEVChanged(float newEV)
    {
        base.OnEVChanged(newEV);
        // Could adjust `transitionSpeed` or direction based on newEV
    }
}
