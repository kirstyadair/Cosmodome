using System;
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
    ScoreManager scoreManager;

    void Start()
    {
        ScoreManager.OnStateChanged += OnStateChange;
        scoreManager = ScoreManager.Instance;
    }

    void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.ROUND_START_CUTSCENE || newState == GameState.END_OF_GAME)
        {
            if (!wallNull) DeactivateWall(currentWall);
        }
    }

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
            BreakableWallScript bws = currentWall.walls[i].GetComponent<BreakableWallScript>();
            BreakableConnectorScript bcs = currentWall.walls[i].GetComponent<BreakableConnectorScript>();

            if (bws != null) bws.Reset();
            else if (bcs != null) bcs.Reset();
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
        if (scoreManager.gameState != GameState.INGAME) return;


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

        if (Input.GetKeyDown(KeyCode.Alpha1)) ForcePattern(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ForcePattern(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ForcePattern(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ForcePattern(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ForcePattern(5);

    }

    void ForcePattern(int patternNumber)
    {
        currentWall = wallPatterns[patternNumber - 1];
        wallNull = false;
        // Activate all walls in this pattern
        for (int i = 0; i < currentWall.walls.Length; i++)
        {
            currentWall.walls[i].SetActive(true);
            if (i < currentWall.particleSystems.Length) currentWall.particleSystems[i].Play();
        }
    }
}
