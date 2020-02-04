using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharacterSelectionOption
{
    public string characterName;
    public GameObject characterPrefab;
    public SelectionBox chosenBy = null;
}

public class ControllerAllocation : MonoBehaviour
{
    public Text statusText;

    public SelectionBox[] selectionBoxes;
    public CharacterSelectionOption[] selectableCharacters;

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
        SelectionBox chosenBox = null;
        foreach (SelectionBox box in selectionBoxes)
        {
            if (box.controller == controller) return; // this controller is already allocated, ignore
            if (box.controller == null)
            {
                chosenBox = box; // This is a box with no controller assigned, so assign it
                break;
            }
        }

        if (chosenBox == null) return; // Tried to connect more than 4 controllers

        chosenBox.AllocateController(controller, this);
        chosenBox.OnReadyUp += CheckIfReadyToGo;
        chosenBox.OnUnReady += CheckIfReadyToGo;


        CheckIfReadyToGo();
    }

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
    }
}
