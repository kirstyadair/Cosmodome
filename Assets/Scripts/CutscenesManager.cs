using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesManager : MonoBehaviour
{
    public delegate void CutsceneEvent();
    public static event CutsceneEvent OnPlayCountdown;
    public static event CutsceneEvent OnPlayCharacterIntro;
    public static event CutsceneEvent OnRoundStart;


    public BetweenRoundText betweenRoundText;
    public GameObject recordingSquare;
    public Animator cameraAnimator;
    public CameraMovement cameraMovement;
    public DeathSpotlight deathSpotlight;

    public Animator countdownAnimator;

    public Text playerNameIntroText;
    public Text characterNameIntroText;

    [Header("How long to count down for before starting game")]
    public int countdownFrom = 3;

    [Header("Disable to not play intro cutscenes")]
    public bool shouldShowIntroCutscenes;

    public IEnumerator PlayCountdownAfterSeeconds(float time)
    {
        if (time < 0) time = 0;
        yield return new WaitForSeconds(time);

        OnPlayCountdown?.Invoke();
    }

    public IEnumerator InbetweenRoundCutscene(int round, int maxRounds, PlayerScript eliminatedPlayer)
    {
        if (!shouldShowIntroCutscenes) yield break;

        StartCoroutine(DeathHighlightPlayer(eliminatedPlayer.gameObject, 2f));
        yield return betweenRoundText.ShowBetweenRoundText(eliminatedPlayer.playerNumber, eliminatedPlayer.playerColor.color, round, maxRounds);
    }

    public IEnumerator StartRoundCutscene()
    {
        if (!shouldShowIntroCutscenes) yield break;

        OnPlayCharacterIntro?.Invoke();

        recordingSquare.SetActive(true);
        List<GameObject> playerShips = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ship"));

        playerShips.Sort((GameObject a, GameObject b) => a.GetComponent<PlayerScript>().playerNumber - b.GetComponent<PlayerScript>().playerNumber);

        playerNameIntroText.gameObject.SetActive(true);
        cameraAnimator.enabled = true;

        float secondsInScene = playerShips.Count * 1.5f;

        // make sure we start playing the countdown sound 3 seconds before we start the countdown
        StartCoroutine(PlayCountdownAfterSeeconds(secondsInScene - 3f));

        foreach (GameObject plrShip in playerShips)
        {
            PlayerScript playerScript = plrShip.GetComponent<PlayerScript>();
            PlayerTypes playerType = playerScript.playerType;

            playerNameIntroText.color = playerScript.playerColor.color;
            playerNameIntroText.text = "PLAYER " + playerScript.playerNumber;


            string animationName = "";

            switch (playerType)
            {
                case PlayerTypes.BIG_SCHLUG:
                    animationName = "Schlug intro";
                    characterNameIntroText.text = "AS BIG SCHLUG";
                    break;
                case PlayerTypes.DAVE:
                    animationName = "Dave intro";
                    characterNameIntroText.text = "AS DAVE";
                    break;
                case PlayerTypes.EL_MOSCO:
                    animationName = "El Mosco intro";
                    characterNameIntroText.text = "AS EL MOSCO";
                    break;
                case PlayerTypes.HAMMER:
                    animationName = "Hammer intro";
                    characterNameIntroText.text = "AS HAMMERHEAD HENRY";
                    break;
            }

            // play the correct cutscene animation
            cameraAnimator.Play(animationName);

            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !cameraAnimator.IsInTransition(0));
        }

        playerNameIntroText.gameObject.SetActive(false);
        cameraAnimator.enabled = false;
        recordingSquare.SetActive(false);
        // done all the cutscenes
        yield return null;
    }

    public IEnumerator StartCountdown()
    {
        countdownAnimator.gameObject.SetActive(true);
        for (int i = countdownFrom; i > 0; i--)
        {
            countdownAnimator.gameObject.GetComponent<Text>().text = i.ToString();
            countdownAnimator.Play("Countdown", -1, 0);
            yield return new WaitForSeconds(1f);
        }

        OnRoundStart?.Invoke();
        countdownAnimator.gameObject.SetActive(false);
    }

    public IEnumerator DeathHighlightPlayer(GameObject player, float time)
    {
        deathSpotlight.ShowSpotlight(player);
        yield return new WaitForSeconds(time);
        deathSpotlight.HideSpotlight();

    }

}
