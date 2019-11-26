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


public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    static ScoreManager _instance;

    // Events
    public delegate void UpdateScores();
    public static event UpdateScores OnUpdateScore;

   // public List<float> playerApprovals = new List<float>();
    public List<PlayerScript> players = new List<PlayerScript>();

    public int numberOfPlayers;
    public float timeLeftInRound = 90;
    public Text timeText;

    [Header("Approval Rates")]
    public int bulletDamageRate;
    public int lowDamageRate;
    public int medDamageRate;
    public int highDamageRate;
    public int highestDamageRate;
    public int arenaCannonRate;

    public Color[] playerColours;

    public PlayerScript winningPlayer;
    public CrowdManager cm;

    public void Update()
    {
        timeLeftInRound -= Time.deltaTime;
        timeText.text = Mathf.RoundToInt(timeLeftInRound).ToString();

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
                firstPlayerWithoutAController.EnableRing(firstPlayerWithoutAController.playerColor);
                //InputManager.ActiveDevice.Vibrate(100);
            }

        }

        if (timeLeftInRound <= 0)
        {
            PlayerScript currentLowest = players[0];

            for (int i = 1; i < players.Count; i++)
            {
                if (players[i].approval.percentage < currentLowest.approval.percentage) currentLowest = players[i];
            }

            // do something interesting here for player elimination
            StartCoroutine(Explode(currentLowest));

            timeLeftInRound = 90.0f;
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
        foreach (PlayerScript player in players)
        {
            if (player.inputDevice == device) player.inputDevice = null;
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
        GameObject[] playerGOs = GameObject.FindGameObjectsWithTag("Ship");
        numberOfPlayers = playerGOs.Length;
        for (int i = 0; i < playerGOs.Length; i++)
        {
            players.Add(playerGOs[i].GetComponent<PlayerScript>());
        }
        UpdatePercentages();
    }

    private void Start()
    {

        InputManager.OnDeviceDetached += OnDeviceDetached;
        
    }

    private void OnEnable()
    {
        PlayerScript.OnPlayerShot += PlayerShot;
        PlayerScript.OnPlayerCollision += PlayerCollision;
        PlayerScript.OnPlayerHitByArenaCannon += PlayerHitByArenaCannon;
        WallScript.OnTrapHit += PlayerHitTrap;
        WallScript.OnTrapSabotaged += PlayerAttemptSabotage;
    }

    private void OnDisable()
    {
        PlayerScript.OnPlayerShot -= PlayerShot;
        PlayerScript.OnPlayerCollision -= PlayerCollision;
        WallScript.OnTrapHit -= PlayerHitTrap;
        WallScript.OnTrapSabotaged -= PlayerAttemptSabotage;
    }

    void PlayerShot(GameObject shotPlayer, GameObject shooter)
    {
        shotPlayer.GetComponent<PlayerScript>().approval.ChangeApproval(-bulletDamageRate);
        shooter.GetComponent<PlayerScript>().approval.ChangeApproval(bulletDamageRate);
        //Bens code change start
        StartCoroutine(shotPlayer.GetComponent<PlayerScript>().ArrowFlash(.5f,0,0));
        StartCoroutine(shooter.GetComponent<PlayerScript>().ArrowFlash(.5f,0,1));
        //Bens code change end
        OnUpdateScore?.Invoke();
        UpdatePercentages();

        StartCoroutine(shotPlayer.GetComponent<PlayerScript>().FlashWithDamage());
    }

    void PlayerHitByArenaCannon(GameObject shotPlayer, GameObject shooter)
    {
        shotPlayer.GetComponent<PlayerScript>().approval.ChangeApproval(-arenaCannonRate);
        shooter.GetComponent<PlayerScript>().approval.ChangeApproval(arenaCannonRate);
        //Bens code change start
        StartCoroutine(shotPlayer.GetComponent<PlayerScript>().ArrowFlash(1f, 0, 0));
        StartCoroutine(shooter.GetComponent<PlayerScript>().ArrowFlash(1f, 0, 1));
        //Bens code change end
        OnUpdateScore?.Invoke();
        UpdatePercentages();

        StartCoroutine(shotPlayer.GetComponent<PlayerScript>().FlashWithDamage());
    }


    void PlayerCollision(GameObject player)
    {
        Debug.Log(player.name + " hit");
        player.GetComponent<PlayerScript>().approval.ChangeApproval(-lowDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();
        //Bens code change start
        StartCoroutine(player.GetComponent<PlayerScript>().ArrowFlash(.5f,1,0));
        //Bens code change end
        StartCoroutine(player.GetComponent<PlayerScript>().FlashWithDamage());
    }

    void PlayerHitTrap(GameObject player, Traps trapType)
    {
        player.GetComponent<PlayerScript>().approval.ChangeApproval(-lowDamageRate);
        OnUpdateScore?.Invoke();
        UpdatePercentages();

        if (trapType == Traps.SPIKEWALL)
        {
            StartCoroutine(player.GetComponent<PlayerScript>().FlashWithDamage());
        }
    }

    void PlayerAttemptSabotage(GameObject player, Traps trapType, bool successful)
    {
        if (successful)
        {
            player.GetComponent<PlayerScript>().approval.ChangeApproval(highestDamageRate);
            OnUpdateScore?.Invoke();
            UpdatePercentages();
        }
        else
        {
            player.GetComponent<PlayerScript>().approval.ChangeApproval(-highestDamageRate);
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

    IEnumerator Explode(PlayerScript currentLowest)
    {
        GameObject.Instantiate(currentLowest.ps, currentLowest.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        players.Remove(currentLowest);
        cm.RecalculateCrowd();
        numberOfPlayers--;
        UpdatePercentages();
        currentLowest.gameObject.SetActive(false);
    }


}
