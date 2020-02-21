using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssignment : MonoBehaviour
{
    [Header("The player gameobjects that will be enabled/disabled accordingly")]
    public GameObject[] playerGameObjects;

    public delegate void PlayerAssignmentEvent();
    public static event PlayerAssignmentEvent OnPlayersAssigned;

    ScoreManager sm;

    public void Start()
    {
        sm = ScoreManager.Instance;

        // Look for the ControllerAllocation gameobject passed in from the character selection screen
        GameObject controllerAllocationGO = GameObject.Find("ControllerAllocation");


        // If it doesn't exist in the scene then we're probably testing, so just enable in Dave and Heavyweight and start the game
        if (controllerAllocationGO == null)
        {
            EnableTestingPlayers();
        } else
        {
            EnableControlledPlayers(controllerAllocationGO.GetComponent<ControllerAllocation>());
        }


        // start the game
        OnPlayersAssigned?.Invoke();
    }

    public void EnableTestingPlayers()
    {
        EnablePlayer(PlayerTypes.DAVE);
        EnablePlayer(PlayerTypes.BIG_SCHLUG);

    }

    public void EnableControlledPlayers(ControllerAllocation controllerAllocation)
    {
        foreach (AllocatedController allocated in controllerAllocation.allocatedControllers)
        {
            PlayerScript plr = EnablePlayer(allocated.playerType);
            plr.inputDevice = allocated.controller;
            plr.EnableRing(plr.playerColor.color);
        }
    }

    public PlayerScript EnablePlayer(PlayerTypes type)
    {
        foreach (GameObject player in playerGameObjects)
        {
            PlayerScript plrScript = player.GetComponent<PlayerScript>();
            if (plrScript.playerType == type)
            {
                player.SetActive(true);
                return plrScript;
            }
        }

        return null;
    }
}
