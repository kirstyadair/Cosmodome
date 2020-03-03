using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    [Header("How long between trap spawns?")]
    public float timeBetweenSpawns;
    public float timeSinceLastSpawn;
    [Header("How long is the trap active?")]
    public float maxTimeActive;
    public float timeActive;
    bool trapsActive = false;
    [Header("How many traps (max) spawn at once?")]
    public float numberOfTraps;

    List<SpikeTrapScript> spikeWalls = new List<SpikeTrapScript>();
    List<SpikeTrapScript> activeTraps = new List<SpikeTrapScript>();

    void Start()
    {
        GameObject[] allSpikeWalls = GameObject.FindGameObjectsWithTag("SpikeWall");
        foreach (GameObject wall in allSpikeWalls)
        {
            spikeWalls.Add(wall.GetComponent<SpikeTrapScript>());
        }
    }

    void Update()
    {
        if (!trapsActive) timeSinceLastSpawn += Time.deltaTime;
        else timeActive += Time.deltaTime;

        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            SpawnInTraps();
            timeSinceLastSpawn = 0;
        } 
        if (timeActive >= maxTimeActive) 
        {
            DisableTraps();
            timeActive = 0;
        }
    }

    void SpawnInTraps()
    {
        trapsActive = true;

        for (int i = 0; i < numberOfTraps; i++)
        {
            int randomNumber = Random.Range(0, spikeWalls.Count);
            spikeWalls[randomNumber].SpawnInWall();
            activeTraps.Add(spikeWalls[randomNumber]);
        }
    }

    void DisableTraps()
    {
        foreach (SpikeTrapScript trap in activeTraps)
        {
            trap.DisableTrap();
        }

        trapsActive = false;
    }
}
