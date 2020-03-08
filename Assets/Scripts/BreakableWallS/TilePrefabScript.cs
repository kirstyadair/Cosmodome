using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefabScript : MonoBehaviour
{
    [SerializeField]
    BreakableWallScript[] connectedWalls;
    [SerializeField]
    BreakableConnectorScript[] connectorScripts;
    BreakableWallScript[] breakableWalls;
    MeshRenderer[] meshRenderers;

    public void DisableAll(GameObject ship)
    {
        foreach (BreakableWallScript breakableWall in connectedWalls)
        {
            if (breakableWall.gameObject.activeInHierarchy) breakableWall.ExplodeChildren(ship);
        }

        foreach (BreakableConnectorScript breakableConnector in connectorScripts)
        {
            breakableConnector.Disappear();
        }
    }

    public void MakeAllRed()
    {
        foreach (BreakableWallScript wall in connectedWalls)
        {
            if (wall.gameObject.activeInHierarchy) wall.TurnChildrenRed();
        }

        foreach (BreakableConnectorScript breakableConnector in connectorScripts)
        {
            //if (breakableConnector.gameObject.activeInHierarchy) breakableConnector.TurnRed();
        }
    }
}
