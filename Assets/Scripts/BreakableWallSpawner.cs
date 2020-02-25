﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WallPattern
{
    public string name;
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
    float timeUntilNextSpawn = 0;
    bool wallNull = true;

    void ChooseRandomPattern()
    {
        // Choose a random wall in the array of possible patterns
        int randomInt = UnityEngine.Random.Range(0, wallPatterns.Length);
        currentWall = wallPatterns[randomInt];
        wallNull = false;
        // Activate all walls in this pattern
        for (int i = 0; i < currentWall.walls.Length; i++)
        {
            currentWall.walls[i].SetActive(true);
            if (i < currentWall.particleSystems.Length) currentWall.particleSystems[i].Play();
        }
    }

    void DeactivateWall(WallPattern currentWall)
    {
        // Deactivate all walls in the pattern
        for (int i = 0; i < currentWall.walls.Length; i++)
        {
            if (currentWall.walls[i].GetComponent<BreakableWallScript>() != null) currentWall.walls[i].GetComponent<BreakableWallScript>().Reset();
            else currentWall.walls[i].SetActive(false);
            
            if (i < currentWall.particleSystems.Length) currentWall.particleSystems[i].Play();
        }
        // Reset the time active
        currentWall.timeActive = 0;
        // Set currnt wall to null
        wallNull = true;
        
        timeUntilNextSpawn = 0;
    }

    void Update()
    {
        if (!wallNull) 
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
