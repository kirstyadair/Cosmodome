using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Traps
{
    SPIKEWALL, NULL
}

/// <summary>
/// WAITING_FOR_CONTROLLERS when we are still waiting for the player prefabs to spawn
/// INGAME we are currently playing
/// ROUND_START_CUTSCENE is the cutscene that shows all the players
/// ROUND_END_CUTSCENE is where the random animations play of the audience and such after a player is eliminated
/// COUNTDOWN is the countdown just before start of game
/// END_OF_GAME is when all rounds have finished
/// </summary>
public enum GameState
{
    WAITING_FOR_CONTROLLERS, INGAME, ROUND_START_CUTSCENE, ROUND_END_CUTSCENE, COUNTDOWN, END_OF_GAME
}

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    static ScoreManager _instance;

    // Events
    public delegate void UpdateScores();
    public static event UpdateScores OnUpdateScore;
    public delegate void ExplodePlayer();
    public static event ExplodePlayer OnExplodePlayer;
    public delegate void PlayerEliminated();
    public static event PlayerEliminated OnPlayerEliminated;

    public delegate void StateEvent(GameState newState, GameState oldState);
    public static event StateEvent OnStateChanged;

   // public List<float> playerApprovals = new List<float>();
    public List<PlayerScript> players = new List<PlayerScript>();

    public int numberOfPlayers;
    public float timeLeftInRound = 60;
    public float roundLength;
    float maxTime;
    public Text timeText;
    public Image timeBarLeft;
    public Image timeBarRight;


    [Header("Approval Rates")]
    //public int bulletDamageRate;
    public int lowDamageRate;
    public int medDamageRate;
    public int highDamageRate;
    public int highestDamageRate;
    public int arenaCannonRate;
    public int bumperBallExplosionRate;
    public int spikeHitDamageRate;

    public Color[] playerColours;

   
    public CutscenesManager cutscenesManager;
    public PlayerScript winningPlayer;
    public CrowdManager cm;
    public ExcitementManager em;

    public bool isCameraEnabled = false;

    int _currentRound = 1;
    int _maxRounds;

    public GameState gameState = GameState.WAITING_FOR_CONTROLLERS;

    public void ChangeState(GameState newState)
    {
        GameState oldState = gameState;
        gameState = newState;

        OnStateChanged?.Invoke(newState, oldState);
    }


    public void Update()
    {
        if (gameState != GameState.INGAME) return;

        timeLeftInRound -= Time.deltaTime;
        timeText.text = Mathf.RoundToInt(timeLeftInRound).ToString();

        timeBarLeft.fillAmount = timeLeftInRound / maxTime;
        timeBarRight.fillAmount = timeLeftInRound / maxTime;


        if (timeLeftInRound < 10) timeText.color = Color.red;
        else timeText.color = Color.white;

        if (InputManager.ActiveDevice.Action1.WasPressed)
        {
            
            PlayerScript firstPlayerWithoutAController = null;

            int lowestPlayerNumber = 100;

            foreach (PlayerScript player in players)
            {
                // make sure no other player is using this controller
                if (player.inputDevice == InputManager.ActiveDevice) return;

                // find the first player without a controller
                if (player.inputDevice == null && player.playerNumber < lowestPlayerNumber)
                {
                    firstPlayerWithoutAController = player;
                    lowestPlayerNumber = player.playerNumber;
                }
            }

            if (firstPlayerWithoutAController != null)
            {
                firstPlayerWithoutAController.inputDevice = InputManager.ActiveDevice;

                InputManager.ActiveDevice.SetLightColor(firstPlayerWithoutAController.playerColor.color);// firstPlayerWithoutAController.playerColor);
                firstPlayerWithoutAController.Vibrate(0.5f, 0.5f);
                firstPlayerWithoutAController.EnableRing(firstPlayerWithoutAController.playerColor.color);
            }

        }

        if (timeLeftInRound <= 0)
        {
            if (_currentRound < _maxRounds)
            {
                EndOfRound();
            }
            {
                EndOfGame();
            }
        }
        else
        {
            PlayerScript currentHighest = players[0];

            for (int i = 1; i < players.Count; i++)
            {
                if (players[i].approval.percentage > currentHighest.approval.percentage) currentHighest = players[i];
            }
            winningPlayer = currentHighest;
        }
    }


    public void OnDeviceDetached(InputDevice device)
    {
        // commented this out to see if the controller will reattach automatically

        
        foreach (PlayerScript player in players)
        {
            if (player.inputDevice == device)
            {
                player.inputDevice = null;
                player.DisableRing();
            }
        }
    }

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject g = new GameObject("Level Manager");
                _instance = g.AddComponent<ScoreManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        PlayerAssignment.OnPlayersAssigned += OnPlayersReady;
        InputManager.OnDeviceDetached += OnDeviceDetached;
        PlayerScript.OnPlayerShot += PlayerShot;
        PlayerScript.OnPlayerCollision += PlayerCollision;
        PlayerScript.OnPlayerHitByArenaCannon += PlayerHitByArenaCannon;
        WallScript.OnTrapHit += PlayerHitTrap;
        WallScript.OnTrapSabotaged += PlayerAttemptSabotage;
        SpikeTrapScript.OnPlayerSpikeHit += PlayerHitSpike;
        BumperBall.OnBumperBallExplodeOnPlayer += BumperBallExplodesOnPlayer;
        ScoreManager.OnStateChanged += WhenStateHasChanged;
    }

    public void WhenStateHasChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.INGAME)
        {
            isCameraEnabled = true;
        }
    }

    public void OnPlayersReady()
    {
        maxTime = timeLeftInRound;
        GameObject[] playerGOs = GameObject.FindGameObjectsWithTag("Ship");
        numberOfPlayers = playerGOs.Length;
       
        for (int i = 0; i < playerGOs.Length; i++)
        {
            players.Add(playerGOs[i].GetComponent<PlayerScript>());
        }

        _maxRounds = players.Count - 1;
        UpdatePercentages();
        cm.SetUpCrowd();

        StartCoroutine(PlayStartofRoundCutscene());
    }

    /// <summary>
    /// Start the start of round cutscene sequence
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayStartofRoundCutscene()
    {
        isCameraEnabled = false;

        ChangeState(GameState.ROUND_START_CUTSCENE);

        timeText.text = "START YOUR ENGINES";

        yield return cutscenesManager.StartRoundCutscene();

        ChangeState(GameState.COUNTDOWN);
        isCameraEnabled = true;

        yield return cutscenesManager.StartCountdown();

        ChangeState(GameState.INGAME);
    }

    void EndOfGame()
    {
        PlayerScript currentLowest = players[0];

        for (int i = 1; i < players.Count; i++)
        {
            if (players[i].approval.percentage < currentLowest.approval.percentage) currentLowest = players[i];
        }

        timeLeftInRound = roundLength;
        maxTime = timeLeftInRound;

        StartCoroutine(Explode(currentLowest, 2f));
        StartCoroutine(EndOfGameCutscene(currentLowest));
    }

    void EndOfRound()
    {
        PlayerScript currentLowest = players[0];

        for (int i = 1; i < players.Count; i++)
        {
            if (players[i].approval.percentage < currentLowest.approval.percentage) currentLowest = players[i];
        }

        timeLeftInRound = roundLength;
        maxTime = timeLeftInRound;

        StartCoroutine(Explode(currentLowest, 2f));
        StartCoroutine(BetweenRoundsCutscene(_currentRound, _maxRounds, currentLowest));

        _currentRound++;

    }

    public IEnumerator EndOfGameCutscene(PlayerScript eliminatedPlayer)
    {
        ChangeState(GameState.END_OF_GAME);

        timeText.color = Color.white;
        timeText.text = "GAME OVER";

        yield return cutscenesManager.EndOfGameCutscene(eliminatedPlayer);
    }


    public IEnumerator BetweenRoundsCutscene(int round, int maxRounds, PlayerScript eliminatedPlayer)
    {

        ChangeState(GameState.ROUND_END_CUTSCENE);

        timeText.color = Color.white;
        timeText.text = "ROUND OVER";

        yield return cutscenesManager.InbetweenRoundCutscene(round, maxRounds, eliminatedPlayer);

        ChangeState(GameState.COUNTDOWN);

        timeText.text = "ROUND " + (round + 1).ToString();

        yield return cutscenesManager.StartCountdown();

   

        ChangeState(GameState.INGAME);
    }

    void BumperBallExplodesOnPlayer(PlayerScript hitPlayer)
    {
        //hitPlayer.approval.ChangeApproval(-bumperBallExplosionRate);
        StartCoroutine(hitPlayer.FlashWithDamage());

        StartCoroutine(hitPlayer.ArrowFlash(.5f, 0, 0));
    }

    void PlayerShot(PlayerScript shotPlayer, PlayerScript shooter)
    {
        int bulletDamageRate = 0;
        if (shooter.basicWeaponScript != null) bulletDamageRate = shooter.basicWeaponScript.damage;
        if (shooter.chargeWeaponScript != null) bulletDamageRate = shooter.chargeWeaponScript.damage;

        shotPlayer.approval.ChangeApproval(-bulletDamageRate);
        shooter.approval.ChangeApproval(bulletDamageRate);

        StartCoroutine(shotPlayer.ArrowFlash(.5f,0,0));
        StartCoroutine(shooter.ArrowFlash(.5f,0,1));

        OnUpdateScore?.Invoke();
        UpdatePercentages();

        StartCoroutine(shotPlayer.FlashWithDamage());
    }

    void PlayerHitByArenaCannon(PlayerScript shotPlayer)
    {
        shotPlayer.approval.ChangeApproval(-arenaCannonRate);
        //Bens code change start
        StartCoroutine(shotPlayer.ArrowFlash(1f, 0, 0));
        //Bens code change end
        OnUpdateScore?.Invoke();
        UpdatePercentages();

        StartCoroutine(shotPlayer.FlashWithDamage());
    }


    void PlayerCollision(PlayerScript player, PlayerScript playerAttacking)
    {
        player.approval.ChangeApproval(-lowDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();
        //Bens code change start
        StartCoroutine(player.ArrowFlash(.5f,1,0));
        

        //Bens code change end
        StartCoroutine(player.FlashWithDamage());
    }

    void PlayerHitTrap(PlayerScript player, Traps trapType)
    {
        player.approval.ChangeApproval(-lowDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();

        if (trapType == Traps.SPIKEWALL)
        {
            StartCoroutine(player.FlashWithDamage());
        }
    }

    void PlayerAttemptSabotage(PlayerScript player, Traps trapType, bool successful)
    {
        if (successful)
        {
            player.approval.ChangeApproval(highestDamageRate);
            OnUpdateScore?.Invoke();
            UpdatePercentages();
        }
        else
        {
            player.approval.ChangeApproval(-highestDamageRate);
            OnUpdateScore?.Invoke();
            UpdatePercentages();
        }
    }

    void UpdatePercentages()
    {
        int total = 0;

        foreach (PlayerScript player in players)
        {
            //player.approval.value = Mathf.Max(0, player.approval.value);
            total += player.approval.value;
            
        }

        foreach (PlayerScript player in players)
        {
            try
            {
                player.approval.percentage = Mathf.RoundToInt(((float)player.approval.value / (float)total) * 100);
            } catch (DivideByZeroException e)
            {
                player.approval.percentage = 0;
            }
        }
    }

    public void EnableCameraFollow()
    {
        
    }


    public void DisableCameraFollow()
    {

    }

    void PlayerHitSpike(PlayerScript ship)
    {
        ship.approval.ChangeApproval(-spikeHitDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();
    }

    IEnumerator Explode(PlayerScript currentLowest, float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Instantiate(currentLowest.ps, currentLowest.transform.position, Quaternion.identity);
        currentLowest.Die();
        yield return new WaitForSeconds(0.1f);
        OnExplodePlayer.Invoke();
        OnPlayerEliminated?.Invoke();


        players.Remove(currentLowest);
        //cm.colors.Remove(currentLowest.playerColor);
        cm.RecalculateCrowd();
        numberOfPlayers--;
        UpdatePercentages();
        currentLowest.gameObject.SetActive(false);
    }


}
