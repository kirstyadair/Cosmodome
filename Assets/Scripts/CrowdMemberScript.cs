using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberScript : MonoBehaviour
{
    public Material mat;
    public Material[] bothMats;

    void Start()
    {
        
    }

    void Update()
    {
        bothMats = GetComponent<MeshRenderer>().materials;
        bothMats[1] = mat;
        GetComponent<MeshRenderer>().materials = bothMats;
    }
}
