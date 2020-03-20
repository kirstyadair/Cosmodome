using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is necessary, I know it seems redundant but it's needed :)
public class Spike : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
}
