﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ExcitementManager : MonoBehaviour
{
    List<ExcitementMeterScript> players = new List<ExcitementMeterScript>();
    List<ExcitementMeterScript> orderedPlayers = new List<ExcitementMeterScript>();
    ScoreManager sm;
    AudioSource audio;
    public AudioClip cheer1;
    public AudioClip cheer2;
    public AudioClip booing;
    ExcitementMeterScript topPlayer;
    public static event ResetHype OnResetHype;
    public delegate void ResetHype();
    public static event AddHype OnAddHype;
    public delegate void AddHype();
    public float speedIncrement;
    public float maxHypeTimer;
    public int hypeLevel;
    public float hypeTimer;

    [SerializeField] GameObject crowdCheerPrefab;

    public delegate void ComboIncrease(PlayerData playerData);
    public static event ComboIncrease OnComboIncrease;


    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        PlayerScript.OnPlayerCollision += AddToHype;

        sm = ScoreManager.Instance;
        ScoreManager.OnStateChanged += OnStateChange;
    }
    
    
    void OnDisable() {
        ScoreManager.OnStateChanged -= OnStateChange;
        PlayerScript.OnPlayerCollision -= AddToHype;
    }

    public void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.INGAME)
        {

            foreach (PlayerScript player in sm.players)
            {
                players.Add(player.gameObject.GetComponent<ExcitementMeterScript>());
            }
            UpdateHype();
        }
    }

    // Update is called once per frame
    public void UpdateHype()
    {
        orderedPlayers = players.OrderByDescending(x => x.comboScore).ToList();
        topPlayer = orderedPlayers[0];
        if (topPlayer.comboScore <= 0) hypeLevel = 0;
    }

    void Update()
    {
        if (hypeTimer > 0)
        {
            hypeTimer -= Time.deltaTime;
            if (hypeTimer <= 0)
            {
                hypeLevel = 0;
                speedIncrement = 1;
                TriggerReset();
            }
        }

        
    }

    void AddToHype(PlayerScript hitPlayer, PlayerScript shootingPlayer)
    {
        // Don't let hype get over 10
        if (hypeLevel >= 10) hypeLevel=10;
        
        ExcitementMeterScript shootingPlayerExcitement = shootingPlayer.gameObject.GetComponent<ExcitementMeterScript>();
        if(shootingPlayerExcitement.comboScore<10)
        {
            // Add to the combo of the attacking player
            shootingPlayerExcitement.comboScore++;
            shootingPlayerExcitement.timer = maxHypeTimer;
            OnComboIncrease?.Invoke(shootingPlayer.playerData);

            if (shootingPlayerExcitement.comboScore > shootingPlayer.playerData.comboHi) shootingPlayer.playerData.comboHi = shootingPlayerExcitement.comboScore;
            
            if (shootingPlayerExcitement.comboScore==10)
            {
                return;
            }
            
        }

        // Update the list of top players
        UpdateHype();

        // Add hype
        if (shootingPlayer == topPlayer.GetComponent<PlayerScript>())
        {
            hypeLevel++;
            hypeTimer = maxHypeTimer;
            speedIncrement += 0.1f;
            OnAddHype?.Invoke();
            //audio.pitch = ((float)hypeLevel / 10) + 0.5f;
            audio.volume += 0.05f;
            audio.PlayOneShot(cheer2);
        }
    }

    public void TriggerReset()
    {
        audio.Stop();
        StartCoroutine(Boo());
        
        OnResetHype?.Invoke();
    }

    IEnumerator Boo()
    {
        audio.PlayOneShot(booing);
        yield return new WaitForSeconds(booing.length);
        audio.volume = 0.1f;
    }
}
