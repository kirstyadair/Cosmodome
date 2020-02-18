using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class CharacterSelectionOption
{
    public string characterName;
    public PlayerTypes playerType;
    public SelectionBox chosenBy = null;
}

[Serializable]
public class AllocatedController
{
    public PlayerTypes playerType;
    public InputDevice controller;
}

public class ControllerAllocation : MonoBehaviour
{
    [SerializeField]
    StatusBar _statusBar;

    [SerializeField]
    PlayerBox[] _playerBoxes;



    public CharacterSelectionOption[] selectableCharacters;
    public List<AllocatedController> allocatedControllers = new List<AllocatedController>();
    public delegate void ControllerAllocationEvent();
    public event ControllerAllocationEvent OnAnotherBoxChanged;

    Coroutine countdownCoroutine;


    string _statusBarText;

    [Header("How many seconds to count down before starting")]
    public int countdownFrom = 3;
    private void Update()
    {
        InputDevice controller = InputManager.ActiveDevice;


        // Was X pressed?
        if (controller.Action1.WasPressed) AllocateController(controller);
    }

    public void AllocateController(InputDevice controller)
    {
        PlayerBox chosenBox = null;
        foreach (PlayerBox box in _playerBoxes)
        {
            if (box.controller == controller) return; // this controller is already allocated, ignore
            if (box.controller == null)
            {
                chosenBox = box; // This is a box with no controller assigned, so assign it
                break;
            }
        }

        if (chosenBox == null) return; // Tried to connect more than 4 controllers
        chosenBox.AssignController(controller);
        UpdateStatusBar();

        /*
        //chosenBox.AllocateController(controller, this);
        chosenBox.OnReadyUp += CheckIfReadyToGo;
        chosenBox.OnUnReady += CheckIfReadyToGo;


        CheckIfReadyToGo();*/
    }




    /// <summary>
    /// Updates the status bar depending on how many controllers have joined
    /// </summary>
    public void UpdateStatusBar()
    {
        int controllers = this.numControllersAssigned;

        if (controllers == 0) _statusBar.ChangeText("Press X to join");
        if (controllers == 1) _statusBar.ChangeText("Need at least 2 players");
        if (controllers > 1) _statusBar.ChangeTextImportant("Press START once everyone's joined!");
    }

    public void Start()
    {
        UpdateStatusBar();
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Number of controllers currently assigned
    /// </summary>
    public int numControllersAssigned
    {
        get
        {
            int result = 0;

            foreach (PlayerBox playerBox in _playerBoxes) if (playerBox.controller != null) result++;

            return result;
        }
    }

    public void Ready()
    {
        foreach (CharacterSelectionOption option in selectableCharacters)
        {
            if (option.chosenBy == null) continue;

            AllocatedController allocatedController = new AllocatedController
            {
                playerType = option.playerType,
                controller = option.chosenBy.controller
            };

            allocatedControllers.Add(allocatedController);
        }

        // this GO is passed onto the main scene for allocating characters
        SceneManager.LoadScene("Main");
    }
}
