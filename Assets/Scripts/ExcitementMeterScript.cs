﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcitementMeterScript : MonoBehaviour
{
    public int comboScore;
    public ExcitementManager em;
    ScoreManager sm;
    PlayerScript ps;

    // Start is called before the first frame update
    void OnEnable()
    {
       
    }

    public void Start()
    {
        sm = ScoreManager.Instance;
        ScoreManager.OnStateChanged += OnStateChanged;
        ps = GetComponent<PlayerScript>();
    }

    public void OnStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.INGAME)
        {
            PlayerScript.OnPlayerCollision += OnCollision;
            PlayerScript.OnPlayerShot += OnShot;
            PlayerScript.OnPlayerHitByArenaCannon += OnACShot;
            BumperBall.OnBumperBallExplodeOnPlayer += OnBBExplode;
            BumperBall.OnBumperBallHitPlayer += OnBBHit;
            ExcitementManager.OnResetHype += Reset;
        }
    }

    void OnCollision(GameObject playerHit, GameObject playerAttacking)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }

        em.UpdateHype();
    }

    void OnShot(PlayerScript playerHit, PlayerScript playerShooting)
    {
        if (playerHit == ps)
        {
            comboScore = 0;
        }

        em.UpdateHype();
    }

    void OnACShot(GameObject playerHit)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }

        em.UpdateHype();
    }

    void OnBBHit(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }

        em.UpdateHype();
    }

    void OnBBExplode(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }

        em.UpdateHype();
    }

    private void Reset()
    {
        comboScore = 0;
    }
}
