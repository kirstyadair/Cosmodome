using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WallPattern
{
    public GameObject[] walls;
    public float timeActive;
}

public class BreakableWallSpawner : MonoBehaviour
{
    public WallPattern[] wallPatterns;
}
