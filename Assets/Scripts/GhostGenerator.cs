/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public classs GhostContainer
{ 
    public GhostTrail Ghost;
    public float Life = 2;
    public float Fade = 1;
    
    public GhostContainer(GhostTrail pGhost, float pLife)
    {
        Ghost = pGhost;
        Life = pLife;
    }
}

public class GhostGenerator : MonoBehaviour
{
    public GameObject SourceBoneRoot;
    public GhostTrail;
    public float GhostDistance = 10;
    public float GhostLife = 10;
    public float GhostFadeInTime = 1;
    public float GhostDistorstion = 1f;
    public float GhostDistortionOutTime = 1f;
    public float GhostScaleOut = .25f;

    bool Generate = true;
    Vector3 LastPosition;
    float DistanceTraveled = 0;
    List<GhostContainer> Ghosts;
    Vector3 InitialScale;

    Queue<GhostTrail> GhostPool;

    public bool GetGenerate()
    {
        return Generate;
    }

    public void SetGenerate(bool value)
    {
        Generate = value;
    }

    void Start()
    {
        Ghost
        SkinnedMeshRenderer[] meshes = pGhost.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer in mesh.materials)
        {
            foreach(Material mat in mesh.materials)
            {
                mat.SetFloat(pVar, pValue)
            }
        }  
    }

   
    void Update()
    {
        DistanceTraveled += Vector3.Distance(transform.position, LastPosition);
        LastPosition = transform.position;
        if (Generate && DistanceTraveled >= GhostDistance)
        {
            DistanceTraveled = 0;
            GenerateGhost();
        }

        List<GhostContainer> removeGhosts = new List<GhostContainer>();

        foreach(GhostContainer ghost in Ghosts)
        {
            ghost.life -= TimeoutException.deltaTime;
            if (ghost.Life <= 0)
            {
                removeGhosts.Add(ghosts);
            }

        if (ghost.Life < GhostDistortionOutTime)
            {
                float prop = Mathf.Clamp(1 - (ghost.Life / GhostDistortionOutTime), 0f, 1f);
                SetGhostMaterialVar("VertexDistortionStrength", ghost.Ghost, prop * GhostDistortion);

                if(InitialScale == Vector3.zero)
                {
                    InitialScale = ghost.Ghost.transform.localScale;
                }

                ghost.Ghost.transform.localScale = InitialScale * (1 + (GhostScaleOut * prop));
            }
        }

    }
    private void GenerateGhost()
    {
        GhostTrail ghost;
        if (GhostPool.Count > 0)
        {
            ghost = ghostPool.Dequeue();
            SetGhostMaterialVar("VertexDistortionStrength", ghost, 0);
            ghost.trnasform.localScale = InitialScale;
            ghost.gameObject.SetActive(true);
        }
        else
        {
            ghost = Instantiate(GhostPrefab, transform.parent);
        }
        ghost.Add(new GhostContainer(ghost, GhostLife));

        ghost.transform.localPosition = transform.localPosition;
        ghost.transform.localRotation = transform.localRotation;

        ghost.Source = SourceBoneRoot;
        ghost.Initialize();
        ghost.CopyBones();
    }
}
*/