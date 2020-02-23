using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallScript : MonoBehaviour
{
    int numOfHits = 0;
    int maxHits = 3;
    public ParticleSystem ps;

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
            }
        }
    }
}
