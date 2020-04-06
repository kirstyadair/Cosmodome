using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcitementMeterScript : MonoBehaviour
{
    public int comboScore;
    public ExcitementManager em;
    ScoreManager sm;
    PlayerScript ps;
    public float timer;

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

    void Update()
    {
        if (comboScore > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                ResetHype();
            }
        }
    }

    
    void OnDisable() {
        PlayerScript.OnPlayerCollision -= OnCollision;
        PlayerScript.OnPlayerShot -= OnShot;
        PlayerScript.OnPlayerHitByArenaCannon -= OnACShot;
        BumperBall.OnBumperBallExplodeOnPlayer -= OnBBExplode;
        BumperBall.OnBumperBallHitPlayer -= OnBBHit;
        ExcitementManager.OnResetHype -= Reset;
        ScoreManager.OnStateChanged -= OnStateChanged;
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
        } else {
            PlayerScript.OnPlayerCollision -= OnCollision;
            PlayerScript.OnPlayerShot -= OnShot;
            PlayerScript.OnPlayerHitByArenaCannon -= OnACShot;
            BumperBall.OnBumperBallExplodeOnPlayer -= OnBBExplode;
            BumperBall.OnBumperBallHitPlayer -= OnBBHit;
            ExcitementManager.OnResetHype -= Reset;
        }
    }

    void OnCollision(PlayerScript playerHit, PlayerScript playerAttacking)
    {
        if (playerHit == this.gameObject.GetComponent<PlayerScript>())
        {
            ResetHype();
        }

        em.UpdateHype();
    }

    void OnShot(PlayerScript playerHit, PlayerScript playerShooting)
    {
        if (playerHit == ps)
        {
            ResetHype();
        }

        em.UpdateHype();
    }

    void OnACShot(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject.GetComponent<PlayerScript>())
        {
            ResetHype();
        }

        em.UpdateHype();
    }

    void OnBBHit(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject)
        {
            ResetHype();
        }

        em.UpdateHype();
    }

    void OnBBExplode(PlayerScript playerHit)
    {
        if (playerHit == this.gameObject)
        {
            ResetHype();
        }

        em.UpdateHype();
    }

    private void ResetHype()
    {
        // Trigger a boo from the crowd if the combo was over 4
        if (comboScore > 4) em.TriggerReset();
    }

    private void Reset()
    {
        comboScore = 0;
    }
}
