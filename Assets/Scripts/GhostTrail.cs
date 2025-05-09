/*
using UnityEngine;
using System;

public class GhostTrail : MonoBehaviour
{
    public GameObject Source;
    public GameObject MyBoneRoot;

    private Transform[] SourceBones;
    private Transform[] MyBones;

    void Start()
    {
        if (Source != null && MyBoneRoot != null)
        {
            Initialize();
        }
    }

    int CompareName(Transform a, Transform b)
    {
        return a.name.CompareTo(b.name);
    }

    public void Initialize()
    {
        SourceBones = Source.GetComponentsInChildren<Transform>();
        MyBones = MyBoneRoot.GetComponentsInChildren<Transform>();

        Array.Sort(SourceBones, CompareName);
        Array.Sort(MyBones, CompareName);
    }

    public void CopyBones()
    {
        for (int i = 0; i < SourceBones.Length; i++)
        {
            MyBones[i].localPosition = SourceBones[i].localPosition;
            MyBones[i].localRotation = SourceBones[i].localRotation;
            MyBones[i].localScale = SourceBones[i].localScale;
        }
    }
}
*/
