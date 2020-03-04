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
    [SerializeField]
    GameObject warningPopup;
    Animator warningPopupAnim;
    SpikeManager spikeManager;
    bool isActive = false;

    public delegate void PlayerSpikeHit(PlayerScript hitPlayer);
    public static event PlayerSpikeHit OnPlayerSpikeHit;

    void Start()
    {
        spikeManager = GameObject.Find("SpikeManager").GetComponent<SpikeManager>();
        warningPopupAnim = warningPopup.GetComponent<Animator>();
        // Get the spikes for this wall
        GameObject[] allSpikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach (GameObject spike in allSpikes)
        {
            if (spike.transform.parent.transform.parent.name == this.gameObject.name)
            {
                // Add the spike object and its mesh renderer to lists
                spikes.Add(spike);
                spikeMRs.Add(spike.GetComponent<SkinnedMeshRenderer>());
                spike.SetActive(false);
            }
        }
    }

    public void SpawnInWall()
    {
        // Play the warning first
        StartCoroutine(WarnWallSpawn());
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

    IEnumerator WarnWallSpawn()
    {
        warningPopup.SetActive(true);
        yield return new WaitForSeconds(1);
        warningPopupAnim.SetFloat("Speed", -1);
        yield return new WaitForSeconds(1);
        warningPopup.SetActive(false);
        warningPopupAnim.SetFloat("Speed", 1);

        isActive = true;
        // Spawn in each spike and play a particle effect
        for (int i = 0; i < spikes.Count; i++)
        {
            spikes[i].SetActive(true);
            spikeMRs[i].material = redMat;
            GameObject ps = Instantiate(particleEffect, spikes[i].transform.position, spikes[i].transform.rotation);
        }
    }

    IEnumerator FreezeShip(Collider ship)
    {
        Rigidbody shipRB = ship.gameObject.GetComponent<Rigidbody>();
        shipRB.constraints = RigidbodyConstraints.FreezePosition;

        yield return new WaitForSeconds(spikeManager.timeStuck);

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
