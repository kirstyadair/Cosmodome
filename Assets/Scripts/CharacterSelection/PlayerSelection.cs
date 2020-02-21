﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    ControllerAllocation _controllerAllocations;

    [SerializeField]
    Text characterNameText;

    [SerializeField]
    [Header("The time between 'ticks' that the random spinner starts at, this is reduced steadily")]
    float randomSpinnerStartingTime;

    [SerializeField]
    [Header("How much randomSpinnerStartingTime is increased by to slow down")]
    float randomSpinnerTickTimeIncrease;

    [SerializeField]
    Animator _animator;

    [SerializeField]
    StatusBar _statusBar;

    [SerializeField]
    CharacterSelectionStats _stats;

    public CharacterBox[] characterBoxes;

    PlayerBox _currentSelectingPlayer;
    int _currentSelectedCharacter = 0;

    // Start is called before the first frame update
    void Start()
    {
        _controllerAllocations = GetComponent<ControllerAllocation>();

        _controllerAllocations.OnControllersAllocated += _controllerAllocations_OnControllersAllocated;

        // Assign the CharacterOption relating to each PlayerType (so we can just define the player in one place)
        foreach (CharacterBox characterBox in characterBoxes)
        {
            characterBox.SetCharacterOption(_controllerAllocations.GetOptionFromPlayerTypes(characterBox.type));
        }
    }

    private void _controllerAllocations_OnControllersAllocated()
    {
        _animator.Play("CAToRandomPick");
    }

    /// <summary>
    /// Called by the animation when we need to update the length of the player box row, deleting the boxes that are unassigned
    /// </summary>
    public void AnimatorUpdatePlayerBoxRow()
    {
        _controllerAllocations.UpdatePlayerBoxRow();

        _statusBar.ChangeText("Let's go!");
    }


    /// <summary>
    /// Called by animation when we've transitioned to the character select screen
    /// </summary>
    public void Animator_FinishedCharacterControllerTransition()
    {
        PickRandomPlayerToChoose();
        _statusBar.ChangeTextImportant("Picking first player randomly...");
    }
    
    /// <summary>
    /// Starts the random spinner to pick a random player
    /// </summary>
    public void PickRandomPlayerToChoose()
    {
        StartCoroutine(RandomPlayerPickerFX());
    }


    /// <summary>
    /// Coroutine that players the random player picker animation and settles on an answer
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomPlayerPickerFX()
    {
        List<PlayerBox> choosablePlayerBoxes = _controllerAllocations.GetChoosablePlayerBoxes();
        float timeBetweenTicks = randomSpinnerStartingTime;
        int currentTickedPlayer = UnityEngine.Random.Range(0, choosablePlayerBoxes.Count);
        int lastTickedPlayer = 0;

       
        while (timeBetweenTicks < 0.5f)
        {
            PlayerBox prevBox = choosablePlayerBoxes[lastTickedPlayer];
            prevBox.selectorArrowEnabled = false; // disable selector arrow on last box

            lastTickedPlayer = currentTickedPlayer;

            PlayerBox playerBox = choosablePlayerBoxes[currentTickedPlayer];
            playerBox.selectorArrowEnabled = true;

            currentTickedPlayer++;
            if (currentTickedPlayer >= choosablePlayerBoxes.Count) currentTickedPlayer = 0;

            // increase time between ticks to slow it down
            timeBetweenTicks += randomSpinnerTickTimeIncrease;

            yield return new WaitForSeconds(timeBetweenTicks);
        }

        PlayerBox selectedBox = choosablePlayerBoxes[lastTickedPlayer];
        _controllerAllocations.Vibrate(selectedBox.controller, 1f, 0.8f);
        PlayerReadyForSelectingCharacter(selectedBox);
        _statusBar.ChangeText(RandomYourNextLine("Player " + selectedBox._playerNumber));
        yield return new WaitForSeconds(1f);
        //_animator.Play("RandomPickToCharacterSelection");
        selectedBox.selectorArrowEnabled = false;
    }

    /// <summary>
    /// Produces a random "You're next" line with the given playername
    /// </summary>
    /// <param name="playerName">Player name to mention</param>
    /// <returns></returns>
    public string RandomYourNextLine(string playerName)
    {
        string[] randomLines = new string[] { 
            "You're up, {name}",
            "{name}! Choose your pilot!",
            "You're next, {name}",
            "Next up: {name}"
        };

        string chosenLine = randomLines[UnityEngine.Random.Range(0, randomLines.Length)];
        chosenLine = chosenLine.Replace("{name}", playerName);

        return chosenLine;
    }

    /// <summary>
    /// Called when the random player picker has finished, which then plays the RandomPickToCharacterSelection animation
    /// </summary>
    void PlayerReadyForSelectingCharacter(PlayerBox selectedBox)
    {
        if (_currentSelectingPlayer != null)
        {
            _currentSelectingPlayer.OnSelectLeft -= OnPlayerBoxLeft;
            _currentSelectingPlayer.OnSelectRight -= OnPlayerBoxRight;
            _currentSelectingPlayer.OnSelect -= OnPlayerBoxSelect;
        }

        selectedBox.Selecting();

        _currentSelectingPlayer = selectedBox;

        _currentSelectingPlayer.OnSelectLeft += OnPlayerBoxLeft;
        _currentSelectingPlayer.OnSelectRight += OnPlayerBoxRight;
        _currentSelectingPlayer.OnSelect += OnPlayerBoxSelect;

        _currentSelectedCharacter = 0; // TODO: snap to a free character
        Hover(characterBoxes[0]);
    }

    void OnPlayerBoxLeft()
    {
        _currentSelectedCharacter--;
        if (_currentSelectedCharacter < 0) _currentSelectedCharacter = characterBoxes.Length - 1;

        Hover(characterBoxes[_currentSelectedCharacter]);
    }

    void OnPlayerBoxRight()
    {
        _currentSelectedCharacter++;
        if (_currentSelectedCharacter > characterBoxes.Length - 1) _currentSelectedCharacter = 0;

        Hover(characterBoxes[_currentSelectedCharacter]);
    }

    void OnPlayerBoxSelect()
    {

    }

    void Hover(CharacterBox characterBox)
    {
        foreach (CharacterBox otherBox in characterBoxes) if (otherBox != characterBox) otherBox.Unhover(); // Unhover the other boxes
        characterNameText.text = characterBox.option.characterName;
        characterBox.Hover(_currentSelectingPlayer);
        _stats.ChangeStats(characterBox.option);
    }
}
