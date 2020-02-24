using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
    int numOfHits = 0;
    int maxHits = 3;
    public ParticleSystem ps;
    public GameObject rockHitPs;
    Rigidbody[] children;
    bool isExploding;

    void Start()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
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
        children = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody child in children)
        {
            child.isKinematic = false;
            child.useGravity = true;
            child.AddExplosionForce(5, ship.transform.position, 1, 0.5f, ForceMode.Impulse);
        }
    }
}
