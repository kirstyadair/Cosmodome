using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
    public int numOfHits = 0;
    int maxHits = 2;
    public ParticleSystem ps;
    public GameObject rockHitPs;
    public TilePrefabScript tilePrefabScript;
    [SerializeField]
    Material redMat;
    [SerializeField]
    Material standardMat;
    MeshRenderer[] meshRenderers;
    Rigidbody[] children;
    Vector3[] previousPositions;
    Quaternion[] previousRotations;
    BoxCollider collider;
    public bool isExploding = false;

    void Start()
    {
        children = GetComponentsInChildren<Rigidbody>();
        previousPositions = new Vector3[children.Length];
        previousRotations = new Quaternion[children.Length];
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        collider = GetComponent<BoxCollider>();
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            numOfHits++;

            if (numOfHits == 1)
            {
                GameObject ps = Instantiate(rockHitPs, other.transform.position, Quaternion.identity);
                collider.isTrigger = true; // so the ship can pass straight through
                if (this.gameObject.tag == "SideWall")
                {
                    TurnChildrenRed();
                }
                else
                {
                    tilePrefabScript.MakeAllRed();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bumperball"))
        {
            this.gameObject.SetActive(false);
            ps.Play();
        } else if (other.gameObject.CompareTag("Ship"))
        {
            numOfHits++;
            if (numOfHits >= maxHits && !isExploding)
            {
                isExploding = true;
                // Pass the ship to tile prefab script which then calls ExplodeChildren() on all active walls
                if (this.gameObject.tag == "SideWall") ExplodeChildren(other.gameObject);
                else tilePrefabScript.DisableAll(other.gameObject);
            }
            else
            {
                GameObject ps = Instantiate(rockHitPs, other.transform.position, Quaternion.identity);
            }
        }
    }

    // This is called in TilePrefabScript
    public void ExplodeChildren(GameObject ship)
    {
        collider.enabled = false;

        for (int i = 0; i < children.Length; i++)
        {
            meshRenderers[i].material = standardMat;
            children[i].isKinematic = false;
            children[i].useGravity = true;
            previousPositions[i] = children[i].transform.position;
            previousRotations[i] = children[i].transform.rotation;
            children[i].AddExplosionForce(2, ship.transform.position, 1, 0.5f, ForceMode.Impulse);
        }
    }


    // This needs to be called ONLY on walls that have been destroyed
    public void Reset()
    {
        if (isExploding)
        {
            isExploding = false;
            collider.isTrigger = false;
            collider.enabled = true;
            
            for (int i = 0; i < children.Length; i++)
            {
                children[i].isKinematic = true;
                children[i].useGravity = false;
                children[i].transform.position = previousPositions[i];
                children[i].transform.rotation = previousRotations[i];
            }
        }

        numOfHits = 0;
        this.gameObject.SetActive(false);
    }

    public void TurnChildrenRed()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = redMat;
        }
    }
}
