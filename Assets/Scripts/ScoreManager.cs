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
    float timeSinceLastShot = 0.0f;

    // Events
    public delegate void UpdateScores();
    public static event UpdateScores OnUpdateScore;

    public List<float> playerApprovals = new List<float>();

    public bool showDamage = true;
    public int numberOfPlayers;

    [Header("Approval Rates")]
    public float bulletDamageRate;
    public float lowDamageRate;
    public float medDamageRate;
    public float highDamageRate;
    public float highestDamageRate;

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

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayers = players.Length;
        for (int i = 0; i < players.Length; i++)
        {
            playerApprovals.Add(players[i].GetComponent<PlayerScript>().approval);
            players[i].GetComponent<PlayerScript>().placeInScoresList = i;
        }
    }

    private void OnEnable()
    {
        PlayerScript.OnPlayerShot += PlayerShot;
        PlayerScript.OnPlayerCollision += PlayerCollision;
        WallScript.OnTrapHit += PlayerHitTrap;
    }

    private void OnDisable()
    {
        PlayerScript.OnPlayerShot -= PlayerShot;
        PlayerScript.OnPlayerCollision -= PlayerCollision;
        WallScript.OnTrapHit -= PlayerHitTrap;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > 0.5f)
        {
            showDamage = false;
        }
        else
        {
            showDamage = true;
        }
    }

    void PlayerShot(GameObject shotPlayer)
    {
        Material mat = shotPlayer.GetComponent<Renderer>().material;
        timeSinceLastShot = 0;
        playerApprovals[shotPlayer.GetComponent<PlayerScript>().placeInScoresList] -= bulletDamageRate;
        OnUpdateScore.Invoke();
        UpdatePercentages(shotPlayer.GetComponent<PlayerScript>().placeInScoresList);

        if (showDamage)
        {
            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color color = Color.red;
            Color emissionColour = color * Mathf.LinearToGammaSpace(emission);
            mat.SetColor("_EmissionColor", emissionColour);
        }
    }

    void PlayerCollision(GameObject player)
    {
        Debug.Log(player.name + " hit");
        playerApprovals[player.GetComponent<PlayerScript>().placeInScoresList] -= lowDamageRate;
        OnUpdateScore.Invoke();
        UpdatePercentages(player.GetComponent<PlayerScript>().placeInScoresList);
        StartCoroutine(FlashDamage(player));
    }

    void PlayerHitTrap(GameObject player, Traps trapType)
    {
        playerApprovals[player.GetComponent<PlayerScript>().placeInScoresList] -= lowDamageRate;
        OnUpdateScore.Invoke();
        UpdatePercentages(player.GetComponent<PlayerScript>().placeInScoresList);
        if (trapType == Traps.SPIKEWALL)
        {
            StartCoroutine(FlashDamage(player));
        }
    }

    IEnumerator FlashDamage(GameObject player)
    {
        player.GetComponent<PlayerScript>().lightsource.enabled = true;
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<PlayerScript>().lightsource.enabled = false;
    }

    void UpdatePercentages(int positionToPrioritise)
    {
        float allScores = 0;
        for (int i = 0; i < playerApprovals.Count; i++)
        {
            allScores += playerApprovals[i];
        }

        if (allScores < 100)
        {
            float a = 100 - playerApprovals[positionToPrioritise];
            for (int i = 0; i < playerApprovals.Count; i++)
            {
                if (i != positionToPrioritise)
                {
                    playerApprovals[i] = a / (numberOfPlayers - 1);
                }
            }

            OnUpdateScore.Invoke();

        }
        else if (allScores > 100)
        {
            float a = playerApprovals[positionToPrioritise] - 100;
            for (int i = 0; i < playerApprovals.Count; i++)
            {
                if (i != positionToPrioritise)
                {
                    playerApprovals[i] = a / (numberOfPlayers - 1);
                }
            }

            OnUpdateScore.Invoke();
        }
    }


}
