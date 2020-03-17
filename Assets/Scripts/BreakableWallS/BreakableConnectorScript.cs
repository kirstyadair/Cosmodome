using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableConnectorScript : MonoBehaviour
{
    Rigidbody rigidbody;
    BoxCollider collider;
    MeshRenderer meshRenderer;
    Vector3 prevPosition;
    Quaternion prevRotation;
    [SerializeField]
    Material standard;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    TilePrefabScript tilePrefabScript;
    int numOfHits = 0;
    [SerializeField]
    int maxNumOfHits;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
        //prevPosition = transform.position;
        //prevRotation = transform.rotation;
        //rigidbody.isKinematic = false;
        //rigidbody.useGravity = true;
        //meshRenderer.material = standard;
    }

    public void Reset()
    {
        //transform.position = prevPosition;
        //transform.rotation = prevRotation;
        //rigidbody.isKinematic = true;
        //rigidbody.useGravity = false;
        if (meshRenderer != null) meshRenderer.material = standard;
        gameObject.SetActive(false);
    }

    public void TurnRed()
    {
        if (meshRenderer != null) meshRenderer.material = redMaterial;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            numOfHits++;

            if (numOfHits == 1) tilePrefabScript.MakeAllRed();
            if (numOfHits >= maxNumOfHits) tilePrefabScript.DisableAll(other.gameObject);
        }
    }
}
