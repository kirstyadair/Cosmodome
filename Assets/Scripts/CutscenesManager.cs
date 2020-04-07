using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesManager : MonoBehaviour
{
    public delegate void CutsceneCharacterEvent(PlayerScript player);
    public static event CutsceneCharacterEvent OnCharacterIntroStarted;

    // Called when the current character intro has ended, either naturally or when skipped
    public static event CutsceneCharacterEvent OnCharacterIntroEnded;



    public delegate void CutsceneEvent();
    public static event CutsceneEvent OnRoundStart; 


    public delegate void CountdownEvent(float offset);
    public static event CountdownEvent OnPlayCountdown;

    public delegate void CutSceneAudio();
    public static event CutSceneAudio DaveIntro;
    public static event CutSceneAudio BigSchlugIntro;
    public static event CutSceneAudio HHHIntro;
    public static event CutSceneAudio ElMoscoIntro;

    public delegate void BetweenRoundSound(PlayerTypes playerType);
    public static event BetweenRoundSound OnCharacterOut;



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

    [Header("Disable to not play inbetweene-round cutscenes")]
    public bool shouldShowEndofRoundCutscenes;

    [Header("How much player needs to hold skip button for")]
    [SerializeField]
    float _holdSkipButtonFor;

    PlayerTypes _currentIntroCutscene;
    bool _isPlayingInBetweenRoundCutscenes = false;
    bool _skipCutscene = false;
    PilotStand _currentShowingPilotStand = null;
    
    ScoreManager sm;
    Coroutine playCountdownafterSecondsCoroutine = null;

    public IEnumerator PlayCountdownAfterSeeconds(float time)
    {
        if (time < 0) time = 0;
        yield return new WaitForSeconds(time);

        OnPlayCountdown?.Invoke(0);
        playCountdownafterSecondsCoroutine = null;
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

        // show the player being highlighted with the red beam for 2 seconds
        StartCoroutine(DeathHighlightPlayer(eliminatedPlayer.gameObject, 2f));
     

        // show the "player n is eliminated" and then a few seconds later it will show the "round x/y"
        StartCoroutine(betweenRoundText.ShowBetweenRoundText(eliminatedPlayer.playerNumber, eliminatedPlayer.playerColor.color, round, maxRounds));

        yield return new WaitForSeconds(1f);
        
        OnCharacterOut?.Invoke(eliminatedPlayer.playerType);

        // after we've shown the player exploding, zoom into the character looking sad
        yield return new WaitForSeconds(1f);

        recordingSquare.SetActive(true);
        cameraAnimator.enabled = true;
        sm.isCameraEnabled = false;



        pilotStand.Enable();
        pilotStand.SitDown();

        cameraAnimator.Play(eliminationCameraAnimation);
        pilotStand.Eliminated();

        yield return new WaitForSeconds(3f);

        pilotStand.Disable();
        StartCoroutine(StartPlayingInbetweenRoundCutscenes(0f));

        yield return new WaitForSeconds(7f);

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

    /// <summary>
    /// Triggered by the camera animator when the character should sit down
    /// </summary>
    public void Animator_OnCharactersSitDown()
    {
        _currentShowingPilotStand.SitDown();
    }

    void  StopPlayingInbetweenRoundCutscenes()
    {
        _isPlayingInBetweenRoundCutscenes = false;
    }

    public IEnumerator StartRoundCutscene()
    {
        if (!shouldShowIntroCutscenes) yield break;

        recordingSquare.SetActive(true);
        List<GameObject> playerShips = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ship"));

        playerShips.Sort((GameObject a, GameObject b) => a.GetComponent<PlayerScript>().playerNumber - b.GetComponent<PlayerScript>().playerNumber);

        playerNameIntroText.gameObject.SetActive(true);
        cameraAnimator.enabled = true;

        float totalSecondsOfCharacterCutscenes = playerShips.Count * 10f;

        // The countdown sound is 3 seconds of buildup and 3 seconds of actually counting down, so start it 2    seconds early before the numbers show up
        // We store this coroutine in case we need to cancel it if the player skips straight into the game
        playCountdownafterSecondsCoroutine = StartCoroutine(PlayCountdownAfterSeeconds(totalSecondsOfCharacterCutscenes - 4f));
        
        int i = 0;
        foreach (GameObject plrShip in playerShips)
        {
            // This is a new cutscene, so we haven't skipped it yet
            _skipCutscene = false;

            PlayerScript playerScript = plrShip.GetComponent<PlayerScript>();
            ShipController shipController = plrShip.GetComponent<ShipController>();

            PlayerTypes playerType = playerScript.playerType;
            _currentShowingPilotStand = null;

            playerNameIntroText.color = playerScript.playerColor.color;
            playerNameIntroText.text = "PLAYER " + playerScript.playerNumber;


            // Reduces brightness of emitters on ship for close up view
            shipController.ToneDownShip();

            string animationName = "";

            switch (playerType)
            {
                case PlayerTypes.BIG_SCHLUG:
                    BigSchlugIntro?.Invoke();
                    _currentShowingPilotStand = bigSchlugPilotStand;
                    animationName = "Schlug intro";
                    characterNameIntroText.text = "AS BIG SCHLUG";
                    break;
                case PlayerTypes.DAVE:
                    DaveIntro?.Invoke();
                    _currentShowingPilotStand = davePilotStand;
                    animationName = "Dave intro";
                    characterNameIntroText.text = "AS DAVE";
                    break;
                case PlayerTypes.EL_MOSCO:
                    _currentShowingPilotStand = elMoscoPilotStand;
                    ElMoscoIntro?.Invoke();
                    animationName = "El Mosco intro";
                    characterNameIntroText.text = "AS EL MOSCO";
                    break;
                case PlayerTypes.HAMMER:
                    HHHIntro?.Invoke();
                    _currentShowingPilotStand = hhhPilotStand;
                    animationName = "Hammer intro";
                    characterNameIntroText.text = "AS HAMMERHEAD HENRY";
                    break;
            }

            // For triggering the character intro
            OnCharacterIntroStarted?.Invoke(playerScript);

            // play the correct cutscene animation
            cameraAnimator.Play(animationName);

            _currentShowingPilotStand.Enable();
            _currentShowingPilotStand.WalkAndWave(2f);

            _currentIntroCutscene = playerType;
            yield return new WaitForSeconds(1f);

            // Bring skip button up, only the cutscened player can skip
            StartCoroutine(ShowSkipButton(playerScript.inputDevice, playerType, 8f, playerScript.playerData.playerColor, playerScript));
        
            yield return new WaitUntil(() => _skipCutscene || cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !cameraAnimator.IsInTransition(0));

            if (i == playerShips.Count - 1) {
                announcer.Cancel(); // if we skipped into the game, cancel the subtitles and announcer
                OnCharacterIntroEnded?.Invoke(playerScript);
            }

            if (_skipCutscene) {
                if (playCountdownafterSecondsCoroutine != null) StopCoroutine(playCountdownafterSecondsCoroutine);
                totalSecondsOfCharacterCutscenes = (playerShips.Count - i - 1) * 10f; // recalculate the seconds left with the remaining cutscenes
                playCountdownafterSecondsCoroutine = StartCoroutine(PlayCountdownAfterSeeconds(totalSecondsOfCharacterCutscenes - 4f));
            }

            _currentShowingPilotStand.SitDown();


            // Get ship ready for in-game view
            shipController.StopToningDownShip();

            i++;
        }

        playerNameIntroText.gameObject.SetActive(false);
        cameraAnimator.enabled = false;
        recordingSquare.SetActive(false);



        
        if (_skipCutscene) {  
            // If the last cutscene was skipped, play the countdown sound immediately, with offset of 3 seconds so it starts right from the numbers
            if (playCountdownafterSecondsCoroutine != null) StopCoroutine(playCountdownafterSecondsCoroutine);
            OnPlayCountdown?.Invoke(3);
        }


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
