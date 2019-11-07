using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Approval Rates")]
    public int bulletDamageRate;
    public int lowDamageRate;
    public int medDamageRate;
    public int highDamageRate;
    public int highestDamageRate;

    public void Update()
    {
        if (InputManager.ActiveDevice.Action1.WasPressed)
        {
            PlayerScript firstPlayerWithoutAController = null;

            foreach (PlayerScript player in players)
            {
                // make sure no other player is using this controller
                if (player.inputDevice == InputManager.ActiveDevice) return;

                // find the first player without a controller
                if (firstPlayerWithoutAController == null && player.inputDevice == null) firstPlayerWithoutAController = player;
            }

            if (firstPlayerWithoutAController != null) firstPlayerWithoutAController.inputDevice = InputManager.ActiveDevice;

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

    void PlayerShot(GameObject shotPlayer)
    {
        shotPlayer.GetComponent<PlayerScript>().approval.ChangeApproval(-bulletDamageRate);
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
            player.approval.value = Mathf.Max(0, player.approval.value);
            total += player.approval.value;
        }
        foreach (PlayerScript player in players)
        {
            Debug.Log(total);
            try
            {
                player.approval.percentage = Mathf.RoundToInt(((float)player.approval.value / (float)total) * 100);
            } catch (DivideByZeroException e)
            {
                player.approval.percentage = 0;
            }
        }
    }


}
