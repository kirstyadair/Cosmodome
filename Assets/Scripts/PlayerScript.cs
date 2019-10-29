using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerTypes
{
    COMEDIAN, DAREDEVIL, LIGHTWEIGHT, HEAVYWEIGHT, DAVE, NULL
}

public class PlayerScript : MonoBehaviour
{
    public delegate void PlayerShot(GameObject playerHit);
    public static event PlayerShot OnPlayerShot;
    public PlayerTypes playerType;
    public Light lightsource;
    public float approval;
    public Text score;
    public int placeInScoresList;
    ScoreManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
        ScoreManager.OnUpdateScore += UpdateScores;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sm.showDamage)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }

        score.text = playerType.ToString() + ": " + System.Math.Round(approval, 0) + "%";
        //sm.playerApprovals[placeInScoresList] = approval;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GreenBullet" && playerType == PlayerTypes.LIGHTWEIGHT)
        {
            OnPlayerShot.Invoke(this.gameObject);
        }
        else if (other.tag == "RedBullet" && playerType == PlayerTypes.DAVE)
        {
            OnPlayerShot.Invoke(this.gameObject);
        }
    }

    void UpdateScores()
    {
        approval = sm.playerApprovals[placeInScoresList];
    }
}

