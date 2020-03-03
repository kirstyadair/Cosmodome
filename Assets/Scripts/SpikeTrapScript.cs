using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapScript : MonoBehaviour
{
    // The spikes on this wall
    List<GameObject> spikes = new List<GameObject>();
    List<SkinnedMeshRenderer> spikeMRs = new List<SkinnedMeshRenderer>();
    [SerializeField]
    GameObject particleEffect;
    [SerializeField]
    Material redMat;
    [SerializeField]
    Material standardMat;

    void Start()
    {
        GameObject[] allSpikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach (GameObject spike in allSpikes)
        {
            if (spike.transform.parent.transform.parent.name == this.gameObject.name)
            {
                spikes.Add(spike);
                spikeMRs.Add(spike.GetComponent<SkinnedMeshRenderer>());
                spike.SetActive(false);
            }
        }
    }

    public void SpawnInWall()
    {
        for (int i = 0; i < spikes.Count; i++)
        {
            spikes[i].SetActive(true);
            spikeMRs[i].material = redMat;
            GameObject ps = Instantiate(particleEffect, spikes[i].transform.position, spikes[i].transform.rotation);
        }
    }

    public void DisableTrap()
    {
        for (int i = 0; i < spikes.Count; i++)
        {
            GameObject ps = Instantiate(particleEffect, spikes[i].transform.position, spikes[i].transform.rotation);
            spikeMRs[i].material = standardMat;
            spikes[i].SetActive(false);
        }
    }
}
