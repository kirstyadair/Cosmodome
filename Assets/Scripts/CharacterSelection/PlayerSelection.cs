using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        _controllerAllocations = GetComponent<ControllerAllocation>();

        _controllerAllocations.OnControllersAllocated += _controllerAllocations_OnControllersAllocated;
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
        float timeBetweenTicks = randomSpinnerStartingTime + UnityEngine.Random.Range(0, 0.1f);
        int currentTickedPlayer = 0;
        int lastTickedPlayer = 0;

        List<PlayerBox> choosablePlayerBoxes = _controllerAllocations.GetChoosablePlayerBoxes();
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
        RandomPlayerHasBeenPicked(selectedBox);
        _statusBar.ChangeText(RandomYourNextLine("Player " + selectedBox._playerNumber));
        yield return new WaitForSeconds(1f);
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
    void RandomPlayerHasBeenPicked(PlayerBox selectedBox)
    {
        selectedBox.Selecting();

        _animator.Play("RandomPickToCharacterSelection");
    }

}
