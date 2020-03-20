﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapScript : MonoBehaviour
{
    // The spikes on this wall
    public List<GameObject> spikes = new List<GameObject>();
    List<SkinnedMeshRenderer> spikeMRs = new List<SkinnedMeshRenderer>();
    [SerializeField]
    GameObject particleEffect;
    [SerializeField]
    Material redMat;
    [SerializeField]
    Material standardMat;
    SpikeManager spikeManager;
    bool isActive = false;

    public delegate void PlayerSpikeHit(PlayerScript hitPlayer);
    public static event PlayerSpikeHit OnPlayerSpikeHit;

    void Start()
    {
        spikeManager = GameObject.Find("SpikeManager").GetComponent<SpikeManager>();
        // Get the spikes for this wall
        Spike[] allSpikes = GetComponentsInChildren<Spike>();
        foreach (Spike spike in allSpikes)
        {
            if (spike.gameObject.activeInHierarchy)
            {
                spikes.Add(spike.gameObject);
                spikeMRs.Add(spike.skinnedMeshRenderer);
                spike.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnInWall()
    {
        isActive = true;
        // Spawn in each spike and play a particle effect
        for (int i = 0; i < spikes.Count; i++)
        {
            spikes[i].SetActive(true);
            spikeMRs[i].material = redMat;
            GameObject ps = Instantiate(particleEffect, spikes[i].transform.position, spikes[i].transform.rotation);
        }
    }

    public void DisableTrap()
    {
        isActive = false;
        for (int i = 0; i < spikes.Count; i++)
        {
            GameObject ps = Instantiate(particleEffect, spikes[i].transform.position, spikes[i].transform.rotation);
            spikeMRs[i].material = standardMat;
            spikes[i].SetActive(false);
        }
    }

    IEnumerator FreezeShip(Collider ship)
    {
        Rigidbody shipRB = ship.gameObject.GetComponent<Rigidbody>();
        shipRB.constraints = RigidbodyConstraints.FreezeAll;

        // If the trap is soon going to be deactivated, only trap the ship while the trap is active
        float time;
        if ((spikeManager.maxTimeActive - spikeManager.timeActive) > spikeManager.timeStuck) time = spikeManager.timeStuck;
        else time = spikeManager.maxTimeActive - spikeManager.timeActive;
        yield return new WaitForSeconds(time);

        shipRB.constraints = RigidbodyConstraints.None;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship") && isActive)
        { 
            StartCoroutine(FreezeShip(other));
            OnPlayerSpikeHit?.Invoke(other.gameObject.GetComponent<PlayerScript>());
        }
    }
}
