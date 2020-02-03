using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcitementMeterScript : MonoBehaviour
{
    public int comboScore;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerScript.OnPlayerCollision += OnCollision;
        PlayerScript.OnPlayerShot += OnShot;
        PlayerScript.OnPlayerHitByArenaCannon += OnACShot;
        BumperBall.OnBumperBallExplodeOnPlayer += OnBBExplode;
        BumperBall.OnBumperBallHitPlayer += OnBBHit;
    }

    void OnCollision(GameObject playerHit, GameObject playerAttacking)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }
        else if (playerAttacking == this.gameObject)
        {
            comboScore++;
        }
    }

    void OnShot(GameObject playerHit, GameObject playerShooting)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }
    }

    void OnACShot(GameObject playerHit, GameObject playerShooting)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }
        else if (playerShooting == this.gameObject)
        {
            comboScore++;
        }
    }

    void OnBBHit(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }
    }

    void OnBBExplode(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }
    }
}
