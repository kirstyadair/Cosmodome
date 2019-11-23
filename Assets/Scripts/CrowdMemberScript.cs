using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberScript : MonoBehaviour
{
    public Material mat;
    public Material newMat;
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
        if (newMat != null)
        {
            if (newMat.name != mat.name)
            {
                Debug.Log("Updating");
                bothMats = mr.sharedMaterials;
                bothMats[1] = newMat;
                mr.sharedMaterials = bothMats;

                gmr1.material = newMat;
                gmr2.material = newMat;
                gmr3.material = newMat;
            }

        }
    }
}
