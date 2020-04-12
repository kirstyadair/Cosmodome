using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Traps
{
    SPIKEWALL, NULL
}

/// <summary>
/// Represents data of a player when it comes to approval etc after the game has finished
/// </summary>
[Serializable]
public class PlayerData
{
    public int playerNumber;
    public Color playerColor;
    public int placed;
    public int approvalPercentage;
    public string characterName;
    public int rams;
    public int comboHi;
    public PlayerTypes playerType;
    public PlayerScript player;
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
    public delegate void RemovePlayer(PlayerData playerData);
    public static event RemovePlayer OnRemovePlayer;

    public delegate void StateEvent(GameState newState, GameState oldState);
    public static event StateEvent OnStateChanged;

   // public List<float> playerApprovals = new List<float>();
    public List<PlayerScript> players = new List<PlayerScript>();

    List<PlayerScript> allPlayers;

    public int numberOfPlayers;
    public float timeLeftInRound = 60;
    public float roundLength;
    float maxTime;
    public Text timeText;
    public Image timeBarLeft;
    public Image timeBarRight;


    [Header("Approval Rates")]
    //public int bulletDamageRate;
    public int collisionDamageRate;
    public int trapDamageRate;
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
            else 
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

    public List<PlayerData> GetFinalPlayerData()
    {
        List<PlayerData> results = new List<PlayerData>();

        foreach (PlayerScript player in allPlayers)
        {
            PlayerData data = player.playerData;

            data.playerNumber = player.playerNumber;
            data.characterName = player.gameObject.name;
            data.playerType = player.playerType;
            data.player = player;

            results.Add(data);
        }

        results = results.OrderByDescending((a) => a.playerNumber).ToList();

        return results;
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

        // for use later when scoring
        allPlayers = new List<PlayerScript>(players);

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
    }

    void PlayerShot(PlayerScript shotPlayer, PlayerScript shooter)
    {
   
        int bulletDamageRate = 0;
        if (shooter.basicWeaponScript != null) bulletDamageRate = shooter.basicWeaponScript.damage;
        if (shooter.chargeWeaponScript != null) bulletDamageRate = shooter.chargeWeaponScript.damage;
        
        shotPlayer.approval.ChangeApproval(-bulletDamageRate);
        shooter.approval.ChangeApproval(bulletDamageRate);

        OnUpdateScore?.Invoke();
        UpdatePercentages();

        StartCoroutine(shotPlayer.FlashWithDamage());
    }

    void PlayerHitByArenaCannon(PlayerScript shotPlayer)
    {
        //Debug.Log("AC changed approval of " + shotPlayer + " by " + (-arenaCannonRate));
        shotPlayer.approval.ChangeApproval(-arenaCannonRate);

        OnUpdateScore?.Invoke();
        UpdatePercentages();

        StartCoroutine(shotPlayer.FlashWithDamage());
    }


    void PlayerCollision(PlayerScript player, PlayerScript playerAttacking)
    {
        player.approval.ChangeApproval(-collisionDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();
        StartCoroutine(player.FlashWithDamage());
    }

    void PlayerHitTrap(PlayerScript player, Traps trapType)
    {
        player.approval.ChangeApproval(-collisionDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();

        if (trapType == Traps.SPIKEWALL)
        {
            StartCoroutine(player.FlashWithDamage());
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

            player.playerData.approvalPercentage = player.approval.percentage;
        }
    }

    public void EnableCameraFollow()
    {
        
    }

    void OnDestroy() {
        PlayerAssignment.OnPlayersAssigned -= OnPlayersReady;
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        PlayerScript.OnPlayerShot -= PlayerShot;
        PlayerScript.OnPlayerCollision -= PlayerCollision;
        PlayerScript.OnPlayerHitByArenaCannon -= PlayerHitByArenaCannon;
        WallScript.OnTrapHit -= PlayerHitTrap;
        SpikeTrapScript.OnPlayerSpikeHit -= PlayerHitSpike;
        BumperBall.OnBumperBallExplodeOnPlayer -= BumperBallExplodesOnPlayer;
        ScoreManager.OnStateChanged -= WhenStateHasChanged;
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
        currentLowest.playerData.placed = players.Count;
        yield return new WaitForSeconds(time);

        GameObject.Instantiate(currentLowest.ps, currentLowest.transform.position, Quaternion.identity);
        currentLowest.Die();
    
        
        yield return new WaitForSeconds(0.1f);
        OnExplodePlayer.Invoke();
        OnPlayerEliminated?.Invoke();
        OnRemovePlayer?.Invoke(currentLowest.playerData);

        players.Remove(currentLowest);
        //cm.colors.Remove(currentLowest.playerColor);
        cm.RecalculateCrowd();
        numberOfPlayers--;
        UpdatePercentages();
        currentLowest.gameObject.SetActive(false);
    }


}
