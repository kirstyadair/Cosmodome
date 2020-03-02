using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefabScript : MonoBehaviour
{
    [SerializeField]
    BreakableWallScript[] connectedWalls;
    BreakableWallScript[] breakableWalls;
    MeshRenderer[] meshRenderers;

    void Start()
    {
        
    }

    public void DisableAll(GameObject ship)
    {
        foreach (BreakableWallScript breakableWall in connectedWalls)
        {
            if (breakableWall.gameObject.activeInHierarchy)
            {
                breakableWall.ExplodeChildren(ship);
            }
        }
    }

    public void MakeAllRed()
    {
        foreach (BreakableWallScript wall in connectedWalls)
        {
            if (wall.gameObject.activeInHierarchy) wall.TurnChildrenRed();
        }
    }
}
