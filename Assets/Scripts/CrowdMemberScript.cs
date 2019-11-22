using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberScript : MonoBehaviour
{
    public Material mat;
    public Material[] bothMats;
    MeshRenderer mr;
    public MeshRenderer gmr1;
    public MeshRenderer gmr2;
    public MeshRenderer gmr3;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        bothMats = mr.sharedMaterials;
        bothMats[1] = mat;
        mr.sharedMaterials = bothMats;

        gmr1.material = mat;
        gmr2.material = mat;
        gmr3.material = mat;
    }
}
