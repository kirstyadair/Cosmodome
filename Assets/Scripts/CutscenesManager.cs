using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesManager : MonoBehaviour
{

    public GameObject recordingSquare;
    public Animator cameraAnimator;
    public CameraMovement cameraMovement;

    public Animator countdownAnimator;

    public Text playerNameIntroText;
    public Text characterNameIntroText;

    [Header("How long to count down for before starting game")]
    public int countdownFrom = 3;

    [Header("Disable to not play intro cutscenes")]
    public bool shouldShowIntroCutscenes;

    public IEnumerator StartRoundCutscene()
    {
        if (!shouldShowIntroCutscenes) yield return null;

        recordingSquare.SetActive(true);
        List<GameObject> playerShips = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ship"));

        playerShips.Sort((GameObject a, GameObject b) => a.GetComponent<PlayerScript>().playerNumber - b.GetComponent<PlayerScript>().playerNumber);

        playerNameIntroText.gameObject.SetActive(true);
        cameraAnimator.enabled = true;

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

        countdownAnimator.gameObject.SetActive(false);
    }
}
