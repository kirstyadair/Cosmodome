using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
    int numOfHits = 0;
    int maxHits = 3;
    public ParticleSystem ps;
    public GameObject rockHitPs;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            if (numOfHits >= maxHits)
            {
                this.gameObject.SetActive(false);
                ps.Play();
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
}
