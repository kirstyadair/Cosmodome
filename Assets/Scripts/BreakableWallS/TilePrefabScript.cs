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
    public delegate void WallHit();
    public static event WallHit OnWallHit;
    public delegate void WallExplode();
    public static event WallExplode OnWallExplode;

    public void DisableAll(GameObject ship)
    {
        OnWallExplode?.Invoke();
        foreach (BreakableWallScript breakableWall in connectedWalls)
        {
            if (breakableWall.gameObject.activeInHierarchy) breakableWall.ExplodeChildren(ship);
        }

        foreach (BreakableConnectorScript breakableConnector in connectorScripts)
        {
            if (breakableConnector.CompareTag("Connector")) breakableConnector.Disappear();
        }
    }

    public void MakeAllRed()
    {
        OnWallHit?.Invoke();
        foreach (BreakableWallScript wall in connectedWalls)
        {
            if (wall.gameObject.activeInHierarchy) wall.TurnChildrenRed();
        }

        foreach (BreakableConnectorScript breakableConnector in connectorScripts)
        {
            if (breakableConnector.gameObject.activeInHierarchy && breakableConnector.CompareTag("Connector")) breakableConnector.TurnRed();
        }
    }
}
