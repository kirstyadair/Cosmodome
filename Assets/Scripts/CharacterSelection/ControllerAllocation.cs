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

    [HideInInspector]
    public string chosenBy = null;

    public int speedStat;
    public int sizeStat;
    public string weaponStat;
    public string infoStat;
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

    /// <summary>
    /// List of <see cref="PlayerBox"/> in the scene, which stores the selected character, <see cref="CharacterSelectionOption"/>
    /// </summary>
    [SerializeField]
    List<PlayerBox> _playerBoxes;

    
    public CharacterSelectionOption[] selectableCharacters;

    /// <summary>
    /// List of allocated controllers, which stores the <see cref="PlayerTypes"/> and the corresponding <see cref="InputDevice"/>
    /// </summary>
    public List<AllocatedController> allocatedControllers = new List<AllocatedController>();

    public delegate void ControllerAllocationEvent();

    /// <summary>
    /// Called when controllers have been allocated and one has pressed START
    /// </summary>
    public event ControllerAllocationEvent OnControllersAllocated;

    Coroutine countdownCoroutine;
    bool _isFinishedAllocating = false;

    string _statusBarText;
    private void Update()
    {
        InputDevice controller = InputManager.ActiveDevice;


        // Was X pressed?
        if (controller.Action1.WasPressed) AllocateController(controller);
        if (controller.Command.WasPressed) StartButtonPressed();
    }

    /// <summary>
    /// Called when start button is pressed, which passes on control to <see cref="PlayerSelection"/>
    /// </summary>
    void StartButtonPressed()
    {
        if (_isFinishedAllocating) return;

        if (this.numControllersAssigned > 1)
        {
            _isFinishedAllocating = true;
            OnControllersAllocated?.Invoke();
            VibrateAll(1, 1f);
        }
    }

    /// <summary>
    /// When a controller presses X, find it a player to link to
    /// </summary>
    /// <param name="controller">Controller to allocate</param>
    void AllocateController(InputDevice controller)
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

        Vibrate(controller, 1f, 0.5f);
        UpdateStatusBar();
    }

    /// <summary>
    /// Returns a subset of the <see cref="_playerBoxes"/> list that are available to start choosing a character, aka a controller is assigned and the player has not already selected
    /// </summary>
    /// <returns></returns>
    public List<PlayerBox> GetChoosablePlayerBoxes()
    {
        return _playerBoxes.Where(box => box.controller != null && box.hasChosenCharacter == false).ToList();
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

    /// <summary>
    /// Removes unassigned player boxes
    /// </summary>
    public void UpdatePlayerBoxRow()
    {
        // Iterate backwards so we can remove from list at same time
        for (int i = _playerBoxes.Count - 1; i >= 0; i--)
        {
            PlayerBox playerBox = _playerBoxes[i];
            if (playerBox.controller == null)
            {
                Destroy(playerBox.gameObject);
                _playerBoxes.RemoveAt(i);
            }
        }
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

    /// <summary>
    /// Utility funct to vibrate the passed controller for a certain amount of time
    /// </summary>
    /// <param name="controller">Controller to vibrate</param>
    /// <param name="strength">Strength to vibrate at </param>
    /// <param name="time">Time to vibrate for</param>
    public void Vibrate(InputDevice controller, float strength, float time)
    {

        controller.Vibrate(strength);
        StartCoroutine(StopVibratingAfter(controller, time));
    }

    /// <summary>
    /// Returns the <see cref="CharacterSelectionOption"/> connected to the given <see cref="PlayerTypes"/>
    /// </summary>
    /// <param name="type">The PlayerType to get the <see cref="CharacterSelectionOption"/> for</param>
    /// <returns></returns>
    public CharacterSelectionOption GetOptionFromPlayerTypes(PlayerTypes type)
    {
        foreach (CharacterSelectionOption option in selectableCharacters) if (option.playerType == type) return option;
        
        return null;
    }

    /// <summary>
    /// Vibrate all controllers for a given strength and time
    /// </summary>
    /// <param name="strength">Strength to vibrate at </param>
    /// <param name="time">Time to vibrate for</param>
    public void VibrateAll(float strength, float time)
    {
        foreach (PlayerBox playerBox in _playerBoxes) if (playerBox.controller != null) Vibrate(playerBox.controller, strength, time);
    }

    IEnumerator StopVibratingAfter(InputDevice controller, float time)
    {
        yield return new WaitForSeconds(time);

        controller.StopVibration();
    }


    public void Ready()
    {
        /*
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
        */
    }
}
