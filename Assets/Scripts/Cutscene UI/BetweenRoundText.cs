using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetweenRoundText : MonoBehaviour
{
    Animator animator;

    [Header("Changes colour and number")]
    public Text playerNameText;

    [Header("Shows the player this number increasing")]
    public Text currentRoundText;

    [Header("Max rounds")]
    public Text totalRoundText;

    int _currentRound;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator ShowBetweenRoundText(int playerNumber, Color playerColor, int currentRound, int totalRounds)
    {
        animator.Play("Player eliminated text", -1, 0);
        playerNameText.text = "PLAYER " + playerNumber;
        playerNameText.color = playerColor;

        currentRoundText.text = currentRound.ToString();
        totalRoundText.text = "/" + totalRounds;

        _currentRound = currentRound;

        yield return new WaitForSeconds(1f);

        // wait until the animation is complete
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0));
    }

    public IEnumerator ShowGameOverText(int playerNumber, Color playerColor)
    {
        animator.Play("Game over text", -1, 0);
        playerNameText.text = "PLAYER " + playerNumber;
        playerNameText.color = playerColor;

        yield return new WaitForSeconds(1f);

        // wait until the animation is complete
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0));
    }


    public void OnChangeRoundNumber()
    {
        currentRoundText.text = (_currentRound + 1).ToString();
    }
}
