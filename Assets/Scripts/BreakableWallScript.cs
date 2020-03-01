using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
    int numOfHits = 0;
    int maxHits = 2;
    public ParticleSystem ps;
    public GameObject rockHitPs;
    public Material redMat;
    MeshRenderer[] meshRenderers;
    Rigidbody[] children;
    Vector3[] previousPositions;
    Quaternion[] previousRotations;
    bool isExploding = false;

    void Start()
    {
        children = GetComponentsInChildren<Rigidbody>();
        previousPositions = new Vector3[children.Length];
        previousRotations = new Quaternion[children.Length];
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            if (numOfHits == 1)
            {
                foreach (MeshRenderer mr in meshRenderers)
                {
                    mr.material = redMat;
                }
            }
            if (numOfHits >= maxHits && !isExploding)
            {
                isExploding = true;
                ExplodeChildren(other);
                //ps.Play();
            } 
            else 
            {
                numOfHits++;
                GameObject ps = Instantiate(rockHitPs, other.transform.position, Quaternion.identity);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bumperball"))
        {
            this.gameObject.SetActive(false);
            ps.Play();
        }
    }

    void ExplodeChildren(Collision ship)
    {
        this.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < children.Length; i++)
        {
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
            
            this.GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < children.Length; i++)
            {
                children[i].isKinematic = true;
                children[i].useGravity = false;
                children[i].transform.position = previousPositions[i];
                children[i].transform.rotation = previousRotations[i];
            }
        }

        this.gameObject.SetActive(false);
    }
}
