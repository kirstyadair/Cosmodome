using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    private static ScoreManager _instance;
    float timeSinceLastShot = 0.0f;
    public bool showDamage = true;

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
        Debug.Log("Shot: " + shotPlayer);
        Material mat = shotPlayer.GetComponent<Renderer>().material;
        timeSinceLastShot = 0;
        
        if (showDamage)
        {
            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color color = Color.red;
            Color emissionColour = color * Mathf.LinearToGammaSpace(emission);
            mat.SetColor("_EmissionColor", emissionColour);
        }
    }
}
