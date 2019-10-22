using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    private static ScoreManager _instance;

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
    }

    private void Start()
    {
        PlayerScript.OnPlayerShot += PlayerShot;
    }

    void PlayerShot(PlayerTypes shootingPlayer, PlayerTypes shotPlayer)
    {
        Debug.Log("Shot: " + shotPlayer + ", shooting: " + shootingPlayer);
    }
}
