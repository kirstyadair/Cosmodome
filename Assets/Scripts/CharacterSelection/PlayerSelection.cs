﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    ControllerAllocation _controllerAllocations;
    
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
    GameObject _characterBoxes;

    [SerializeField]
    CharacterSelectionStats _stats;

    [SerializeField]
    CharacterModelDisplay _characterModelDisplay;

    [SerializeField] HorizontalScrollingBackground characterScrollingBackground;
    [SerializeField] Sprite neutralBackground;
    [SerializeField] Sprite daveBackground;
    [SerializeField] Sprite bigSchlugBackground;
    [SerializeField] Sprite elMoscoBackground;
    [SerializeField] Sprite hhhBackground;

    public CharacterBox[] characterBoxes;

    bool _readyToActivateNextScene = false;
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

        _statusBar.ChangeText("Choose your fighters");
    }


    /// <summary>
    /// Called by animation when we've transitioned to the character select screen
    /// </summary>
    public void Animator_FinishedCharacterControllerTransition()
    {
        PickRandomPlayerToChoose();
        _characterModelDisplay.SwapModel("");
    }
    
    /// <summary>
    /// Starts the random spinner to pick a random player
    /// </summary>
    public void PickRandomPlayerToChoose()
    {
        StartCoroutine(PickNextPlayer());
    }

    /// <summary>
    /// Start acivating new scene
    /// </summary>
    public void Animator_FinishedFadingToBlack()
    {
       _readyToActivateNextScene = true;
    }


    public void Ready()
    {
    
        _animator.Play("Ready to play");


        _controllerAllocations.Ready();
    }

    public void Animator_NextScene()
    {


        StartCoroutine(NextScene());
    }

    IEnumerator NextScene() {
        AsyncOperation async = SceneManager.LoadSceneAsync("Main");

        async.allowSceneActivation = false;

        while (async.progress < 0.9f) {
            yield return null;
        }

        _animator.Play("Fade to black");

        // Wait until we've finished fading to black
        while (!_readyToActivateNextScene) yield return null;

        async.allowSceneActivation = true;
    }

    /// <summary>
    /// Coroutine that players the random player picker animation and settles on an answer
    /// </summary>
    /// <returns></returns>
    IEnumerator PickNextPlayer()
    {

        List<PlayerBox> choosablePlayerBoxes = _controllerAllocations.GetChoosablePlayerBoxes();

        if (choosablePlayerBoxes.Count == 0)
        {
            _statusBar.ChangeTextImportant("WELCOME TO THE COSMODOME");
            yield return new WaitForSeconds(1f);
            // all player boxes have chosen
            Ready();
            yield break;
        }

        float timeBetweenTicks = randomSpinnerStartingTime;
        int currentTickedPlayer = UnityEngine.Random.Range(0, choosablePlayerBoxes.Count);
        int lastTickedPlayer = 0;

        // if we have more than one, pick a random player. otherwise just pick the only one there
        if (choosablePlayerBoxes.Count > 1)
        {

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

                //yield return new WaitForSeconds(timeBetweenTicks);
            }
        } else
        {
            yield return new WaitForSeconds(0.2f);
        }

        PlayerBox selectedBox = choosablePlayerBoxes[lastTickedPlayer];
        _controllerAllocations.Vibrate(selectedBox.controller, 1f, 0.8f);

        selectedBox.Selecting();

        _statusBar.ChangeText(RandomYourNextLine("Player " + selectedBox._playerNumber));
        yield return new WaitForSeconds(0.5f);

        PlayerReadyForSelectingCharacter(selectedBox);
        _animator.Play("RandomPickToCharacterSelection");

        _statusBar.ChangeText("NEXT");
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
        _characterBoxes.SetActive(true);

        if (_currentSelectingPlayer != null)
        {
            _currentSelectingPlayer.OnSelectLeft -= OnPlayerBoxLeft;
            _currentSelectingPlayer.OnSelectRight -= OnPlayerBoxRight;
            _currentSelectingPlayer.OnSelect -= OnPlayerBoxSelect;
        }

        _currentSelectingPlayer = selectedBox;

        _currentSelectingPlayer.OnSelectLeft += OnPlayerBoxLeft;
        _currentSelectingPlayer.OnSelectRight += OnPlayerBoxRight;
        _currentSelectingPlayer.OnSelect += OnPlayerBoxSelect;

        _currentSelectedCharacter = 0; // TODO: snap to a free character

        int boxToHoverOn = 0;

        // Select the first available box

        for (boxToHoverOn = 0; boxToHoverOn < characterBoxes.Length - 1; boxToHoverOn++) if (characterBoxes[boxToHoverOn].selectedBy == null) break;
        _currentSelectedCharacter = boxToHoverOn;

        Hover(characterBoxes[boxToHoverOn]);
    }

    void OnPlayerBoxLeft()
    {
        _currentSelectedCharacter--;
        if (_currentSelectedCharacter < 0) _currentSelectedCharacter = characterBoxes.Length - 1;

        _controllerAllocations.Vibrate(_currentSelectingPlayer.controller, 1, 0.1f);

        Hover(characterBoxes[_currentSelectedCharacter]);
    }

    void OnPlayerBoxRight()
    {
        _currentSelectedCharacter++;
        if (_currentSelectedCharacter > characterBoxes.Length - 1) _currentSelectedCharacter = 0;

        _controllerAllocations.Vibrate(_currentSelectingPlayer.controller, 1, 0.1f);

        Hover(characterBoxes[_currentSelectedCharacter]);
    }

    void OnPlayerBoxSelect()
    {
        CharacterBox character = characterBoxes[_currentSelectedCharacter];

        if (character.selectedBy != null)
        {
            character.Unavailable();
            return;
        }

        _controllerAllocations.Vibrate(_currentSelectingPlayer.controller, 1, 0.5f);

        character.Selected(_currentSelectingPlayer);

        _currentSelectingPlayer.Selected(character.option);

        _characterModelDisplay.ReadyUp();

        _animator.Play("Character selection to random screen");

        _currentSelectingPlayer.OnSelectLeft -= OnPlayerBoxLeft;
        _currentSelectingPlayer.OnSelectRight -= OnPlayerBoxRight;
        _currentSelectingPlayer.OnSelect -= OnPlayerBoxSelect;
    }

    void Hover(CharacterBox characterBox)
    {
        foreach (CharacterBox otherBox in characterBoxes) if (otherBox != characterBox) otherBox.Unhover(); // Unhover the other boxes
        characterBox.Hover(_currentSelectingPlayer);
        _stats.ChangeStats(characterBox.option);

        // swap out the character model
        _characterModelDisplay.SwapModel(characterBox.option.characterName);

        Sprite newCharBackground = null;
        switch (characterBox.type) {
            case PlayerTypes.DAVE:
                newCharBackground = daveBackground;
                break;
            case PlayerTypes.EL_MOSCO:
                newCharBackground = elMoscoBackground;
                break;
            case PlayerTypes.HAMMER:
                newCharBackground = hhhBackground;
                break;
            case PlayerTypes.BIG_SCHLUG:
                newCharBackground = bigSchlugBackground;
                break;

        }

        characterScrollingBackground.ChangeBackground(newCharBackground);
    }
}
