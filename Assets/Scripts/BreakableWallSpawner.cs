using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WallPattern
{
    public GameObject[] walls;
    public ParticleSystem[] particleSystems;
    [HideInInspector]
    public float timeActive;
}

public class BreakableWallSpawner : MonoBehaviour
{
    public WallPattern[] wallPatterns;
    public float timeBetweenSpawns;
    public float timeUntilDestroyed;
    WallPattern currentWall;
    float timeUntilNextSpawn;

    void ChooseRandomPattern()
    {
        // Choose a random wall in the array of possible patterns
        int randomInt = UnityEngine.Random.Range(0, wallPatterns.Length);
        currentWall = wallPatterns[randomInt];
        // Activate all walls in this pattern
        for (int i = 0; i < currentWall.walls.Length; i++)
        {
            currentWall.walls[i].SetActive(true);
            currentWall.particleSystems[i].Play();
        }
    }

    void DeactivateWall(WallPattern currentWall)
    {
        // Deactivate all walls in the pattern
        for (int i = 0; i < currentWall.walls.Length; i++)
        {
            currentWall.walls[i].SetActive(false);
            currentWall.particleSystems[i].Play();
        }
        // Reset the time active
        currentWall.timeActive = 0;
        // Set current wall to null
        currentWall = null;

        timeUntilNextSpawn = 0;
    }

    void Update()
    {
        if (currentWall != null) 
        {
            currentWall.timeActive += Time.deltaTime;
            if (currentWall.timeActive >= timeUntilDestroyed) DeactivateWall(currentWall);
        }
        else
        {
            timeUntilNextSpawn += Time.deltaTime;
            if (timeUntilNextSpawn >= timeBetweenSpawns) ChooseRandomPattern();
        }

    }
}
