using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberScript : MonoBehaviour
{
    public Material mat;
    public Material[] bothMats;
    public MeshRenderer mr;
    public MeshRenderer gmr1;
    public MeshRenderer gmr2;
    public MeshRenderer gmr3;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bothMats = mr.materials;
        mat = Instantiate<Material>(bothMats[1]);
    }
}
