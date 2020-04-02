using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesManager : MonoBehaviour
{
    public delegate void CutsceneEvent();
    public static event CutsceneEvent OnPlayCountdown;
    public static event CutsceneEvent OnPlayCharacterIntro;
    public static event CutsceneEvent OnRoundStart;
    public delegate void CutSceneAudio();
    public static event CutSceneAudio DaveIntro;
    public static event CutSceneAudio BigSchlugIntro;
    public static event CutSceneAudio HHHIntro;
    public static event CutSceneAudio ElMoscoIntro;




    [Header("Amount of 'Cutscene n' animations available")]
    public int amountOfAudiencePanAnimations;

    [Space(10)]
    public BetweenRoundText betweenRoundText;
    public GameObject recordingSquare;
    public Animator cameraAnimator;
    public CameraMovement cameraMovement;
    public DeathSpotlight deathSpotlight;
    public EndScreenStats endScreenStats;
    public SkipButton skipButton;
    public ControlsUIScript controls;
    public AudioEvent announcer;
    public Animator countdownAnimator;
    public Text playerNameIntroText;
    public Text characterNameIntroText;
    public Pedastals pedastals;

    [Space(10)]
    public PilotStand davePilotStand;
    public PilotStand bigSchlugPilotStand;
    public PilotStand elMoscoPilotStand;
    public PilotStand hhhPilotStand;


    [Header("How long to count down for before starting game")]
    public int countdownFrom = 3;

    [Header("Disable to not play intro cutscenes")]
    public bool shouldShowIntroCutscenes;

    [Header("Disable to not play inbebtween-round cutscenes")]
    public bool shouldShowEndofRoundCutscenes;

    [Header("How much player needs to hold skip button for")]
    [SerializeField]
    float _holdSkipButtonFor;

    PlayerTypes _currentIntroCutscene;
    bool _isPlayingInBetweenRoundCutscenes = false;
    bool _skipCutscene = false;

    ScoreManager sm;

    public IEnumerator PlayCountdownAfterSeeconds(float time)
    {
        if (time < 0) time = 0;
        yield return new WaitForSeconds(time);

        OnPlayCountdown?.Invoke();
    }

    void Start()
    {
        sm = ScoreManager.Instance;
    }

    public IEnumerator EndOfGameCutscene(PlayerScript eliminatedPlayer)
    {
        List<PlayerData> playerData = sm.GetFinalPlayerData();
        //if (!shouldShowIntroCutscenes) yield break;

        StartCoroutine(DeathHighlightPlayer(eliminatedPlayer.gameObject, 2f));
        //StartCoroutine(StartPlayingInbetweenRoundCutscenes(3f));

        // show the text that comes up with info between rounds
        yield return betweenRoundText.ShowGameOverText(eliminatedPlayer.playerNumber, eliminatedPlayer.playerColor.color);

        // once text animation is done, stop playing the inbetween round cutscenes

        yield return new WaitForSeconds(0.5f);

        yield return StartShowingPedastals(playerData);

        endScreenStats.gameObject.SetActive(true);
        endScreenStats.Setup(playerData);

        // now we hand control to endScreenStats to see when start is pressed
    }

    public IEnumerator InbetweenRoundCutscene(int round, int maxRounds, PlayerScript eliminatedPlayer)
    {
        if (!shouldShowEndofRoundCutscenes) yield break;

        // show the player being highlighted with the red beam for 2 seconds
        StartCoroutine(DeathHighlightPlayer(eliminatedPlayer.gameObject, 2f));
     

        // show the "player n is eliminated" and then a few seconds later it will show the "round x/y"
        StartCoroutine(betweenRoundText.ShowBetweenRoundText(eliminatedPlayer.playerNumber, eliminatedPlayer.playerColor.color, round, maxRounds));

        // after we've shown the player exploding, zoom into the character looking sad
        yield return new WaitForSeconds(2.5f);

        recordingSquare.SetActive(true);
        cameraAnimator.enabled = true;
        sm.isCameraEnabled = false;

        string eliminationCameraAnimation = "Nope";
        PilotStand pilotStand = null;

        switch (eliminatedPlayer.playerType) {
            case PlayerTypes.DAVE:
                eliminationCameraAnimation = "Dave elimination";
                pilotStand = davePilotStand;
                break;
            case PlayerTypes.BIG_SCHLUG:
                eliminationCameraAnimation = "Big Schlug elimination";
                pilotStand = bigSchlugPilotStand;
                break;
            case PlayerTypes.HAMMER:
                eliminationCameraAnimation = "Hammer elimination";
                pilotStand = hhhPilotStand;
                break;
            case PlayerTypes.EL_MOSCO:
                eliminationCameraAnimation = "El Mosco elimination";
                pilotStand = elMoscoPilotStand;
                break;
            
        }

        pilotStand.Enable();
        pilotStand.SitDown();

        cameraAnimator.Play(eliminationCameraAnimation);
        pilotStand.Eliminated();

        yield return new WaitForSeconds(3f);

        pilotStand.Disable();
        StartCoroutine(StartPlayingInbetweenRoundCutscenes(0f));

        yield return new WaitForSeconds(4f);

        StopPlayingInbetweenRoundCutscenes();
    }

    IEnumerator StartShowingPedastals(List<PlayerData> playerData)
    {
        sm.isCameraEnabled = false;
        recordingSquare.SetActive(true);
        cameraAnimator.enabled = true;
        cameraAnimator.Play("End of game cutscene", -1, 0);
        pedastals.gameObject.SetActive(true);

        pedastals.Setup(playerData);

        yield return new WaitForSeconds(1f);

        pedastals.Show();

        yield return new WaitForSeconds(5f);
    }

    IEnumerator StartPlayingInbetweenRoundCutscenes(float after)
    {
        yield return new WaitForSeconds(after);
        _isPlayingInBetweenRoundCutscenes = true;


        int currentClip = UnityEngine.Random.Range(1, amountOfAudiencePanAnimations + 1);
        cameraAnimator.Play("Cutscene " + currentClip);

        while (_isPlayingInBetweenRoundCutscenes)
        {
            // TODO: Possible that the last animation loops so we never enter this?
            // animation is done
            if (cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !cameraAnimator.IsInTransition(0))
            {
                int nextClip = UnityEngine.Random.Range(1, amountOfAudiencePanAnimations + 1);

       
                // make sure nextClip is never the same as the currentClip
                while (nextClip == currentClip) nextClip = UnityEngine.Random.Range(1, amountOfAudiencePanAnimations + 1);

                cameraAnimator.Play("Cutscene " + nextClip);
            }

            yield return null;
        }

        sm.isCameraEnabled = true;
        cameraAnimator.enabled = false;
        recordingSquare.SetActive(false);
        cameraMovement.ResetCamera();
    }

    void  StopPlayingInbetweenRoundCutscenes()
    {
        _isPlayingInBetweenRoundCutscenes = false;
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

        int i = 0;
        foreach (GameObject plrShip in playerShips)
        {
            PlayerScript playerScript = plrShip.GetComponent<PlayerScript>();
            ShipController shipController = plrShip.GetComponent<ShipController>();

            PlayerTypes playerType = playerScript.playerType;
            PilotStand pilotStand = null;

            playerNameIntroText.color = playerScript.playerColor.color;
            playerNameIntroText.text = "PLAYER " + playerScript.playerNumber;


            // Reduces brightness of emitters on ship for close up view
            shipController.ToneDownShip();

            string animationName = "";

            switch (playerType)
            {
                case PlayerTypes.BIG_SCHLUG:
                    BigSchlugIntro?.Invoke();
                    pilotStand = bigSchlugPilotStand;
                    animationName = "Schlug intro";
                    characterNameIntroText.text = "AS BIG SCHLUG";
                    break;
                case PlayerTypes.DAVE:
                    DaveIntro?.Invoke();
                    pilotStand = davePilotStand;
                    animationName = "Dave intro";
                    characterNameIntroText.text = "AS DAVE";
                    break;
                case PlayerTypes.EL_MOSCO:
                    pilotStand = elMoscoPilotStand;
                    ElMoscoIntro?.Invoke();
                    animationName = "El Mosco intro";
                    characterNameIntroText.text = "AS EL MOSCO";
                    break;
                case PlayerTypes.HAMMER:
                    HHHIntro?.Invoke();
                    pilotStand = hhhPilotStand;
                    animationName = "Hammer intro";
                    characterNameIntroText.text = "AS HAMMERHEAD HENRY";
                    break;
            }

            // play the correct cutscene animation
            cameraAnimator.Play(animationName);

            pilotStand.Enable();
            pilotStand.WalkAndWave(2f);

            _currentIntroCutscene = playerType;
            yield return new WaitForSeconds(1f);

            // Bring skip button up, only the cutscened player can skip
            StartCoroutine(ShowSkipButton(playerScript.inputDevice, playerType, 8f, playerScript.playerData.playerColor, playerScript));
        
            yield return new WaitUntil(() => _skipCutscene || cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !cameraAnimator.IsInTransition(0));

            if (i == playerShips.Count - 1) {
                announcer.Cancel(); // if we skipped into the game, cancel the subtitles and announcer
            }

            pilotStand.SitDown();

            _skipCutscene = false;

            // Get ship ready for in-game view
            shipController.StopToningDownShip();

            i++;
        }

        playerNameIntroText.gameObject.SetActive(false);
        cameraAnimator.enabled = false;
        recordingSquare.SetActive(false);

        controls.ShowControlsAfter(0.5f);
        // done all the cutscenes
        yield return null;
    }

    IEnumerator ShowSkipButton(InputDevice controller, PlayerTypes type, float time, Color color, PlayerScript player) {
        if (controller == null) controller = InputManager.ActiveDevice;
        else {
            player.Vibrate(0.5f, 0.2f);
        }

        skipButton.gameObject.SetActive(true); // Enable skip button if not active
        skipButton.Appear(color);

        

        float t = 0;

        float heldDownFor = 0;

        bool isSelecting = false;

        // While we are playing the intro cutscene for the right type
        while (t < time) {
            // Wait until X is pressed on the controller
            if (Input.GetKeyDown(KeyCode.X) || controller.Action1.WasPressed) {
                skipButton.StartSelecting();
                isSelecting = true;
  
            } else if (Input.GetKeyUp(KeyCode.X) || controller.Action1.WasReleased) {
               
                isSelecting = false;
                skipButton.StopSelecting();
            }

            t += Time.deltaTime;

            if (isSelecting) {
                heldDownFor += Time.deltaTime;

                 if (heldDownFor > _holdSkipButtonFor) {
                    
                    _skipCutscene = true;

                    skipButton.StopSelecting();
                    t = time + 1; // force out of this loop
                }
            } else {
                heldDownFor -= Time.deltaTime * 2;

                if (heldDownFor < 0) {
                    heldDownFor = 0;
                }
            }

            skipButton.UpdateProgress(heldDownFor / _holdSkipButtonFor);
            yield return null;
        }

        // Have changed intro cutscene
        skipButton.Dissapear();
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
