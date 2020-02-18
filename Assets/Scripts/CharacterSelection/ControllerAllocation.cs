using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public Text statusText;

    [SerializeField]
    PlayerBox[] _playerBoxes;
    public CharacterSelectionOption[] selectableCharacters;
    public List<AllocatedController> allocatedControllers = new List<AllocatedController>();
    public delegate void ControllerAllocationEvent();
    public event ControllerAllocationEvent OnAnotherBoxChanged;

    Coroutine countdownCoroutine;

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

        /*
        //chosenBox.AllocateController(controller, this);
        chosenBox.OnReadyUp += CheckIfReadyToGo;
        chosenBox.OnUnReady += CheckIfReadyToGo;


        CheckIfReadyToGo();*/
    }
    /*
    public void CheckIfReadyToGo()
    {
        OnAnotherBoxChanged?.Invoke(); // let the other boxes know a selection was updated

        // Interrupted during countdown
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        int isReady = 0;
        int isAssigned = 0;
        foreach (SelectionBox box in selectionBoxes)
        {
            if (box.isReady) isReady++;
            if (box.controller != null) isAssigned++;
        }

        if (isAssigned == 0) statusText.text = "PRESS X TO JOIN";
        if (isAssigned == 1) statusText.text = "NEED AT LEAST 2 PLAYERS TO START";
        if (isAssigned >= 2) statusText.text = "WAITING FOR EVERYONE TO READY UP";
        if (isReady == isAssigned && isReady >= 2) StartCountdown();
    }

    public void Start()
    {
        CheckIfReadyToGo();
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartCountdown()
    {
        countdownCoroutine = StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        for (int count = countdownFrom; count > 0; count--)
        {
            statusText.text = "STARTING IN: " + count;
            yield return new WaitForSeconds(1f);
        }

        statusText.text = "LET'S GO!";

        Ready();
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
    }*/
}
