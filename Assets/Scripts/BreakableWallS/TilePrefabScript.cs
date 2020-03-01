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

    public void DisableAll(Collision ship)
    {
        foreach (BreakableWallScript breakableWall in connectedWalls)
        {
            Debug.Log("B");
            if (breakableWall.gameObject.activeInHierarchy)
            {
                Debug.Log("C");
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
