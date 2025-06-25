using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasObjectManager : MonoBehaviour
{
    public static CanvasObjectManager Instance { get; private set; }

    [SerializeField] private Canvas primaryCanvas;
    [SerializeField] private Canvas fallbackCanvas;
    [SerializeField] private float textDistance = 2.5f;

    private List<GameObject> createdObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple CanvasObjectManager instances found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Adds the given GameObject to the first active canvas, retrying every few seconds until successful.
    /// </summary>
    public void AddToCanvasWithRetry(GameObject prefab, float retryDelaySeconds = 1f)
    {
        StartCoroutine(AddToCanvasRoutine(prefab, retryDelaySeconds));
    }

    private IEnumerator AddToCanvasRoutine(GameObject prefab, float retryDelaySeconds)
    {
        while (true)
        {
            Transform parentTransform = GetActiveCanvasTransform();
            if (parentTransform != null)
            {
                GameObject instance = Instantiate(prefab, parentTransform);

                // Position it at the parent's position, offset backwards
                instance.transform.position = parentTransform.position + parentTransform.forward * textDistance;
                instance.transform.rotation = parentTransform.rotation;

                createdObjects.Add(instance);
                //Debug.Log("Child added to canvas at offset position.");
                yield break;
            }

            Debug.Log("No active canvas, retrying...");
            yield return new WaitForSeconds(retryDelaySeconds);
        }
    }


    /// <summary>
    /// Destroys all objects previously added to any canvas through this manager.
    /// </summary>
    public void ClearCanvasObjects()
    {
        foreach (GameObject obj in createdObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        createdObjects.Clear();
    }

    /// <summary>
    /// Returns the transform of the first active canvas (primary, then fallback).
    /// </summary>
    private Transform GetActiveCanvasTransform()
    {
        if (primaryCanvas != null && primaryCanvas.isActiveAndEnabled)
            return primaryCanvas.transform;

        if (fallbackCanvas != null && fallbackCanvas.isActiveAndEnabled)
            return fallbackCanvas.transform;

        return null;
    }
}
