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
    public delegate void PlayerCollision(GameObject playerHit);
    public static event PlayerCollision OnPlayerCollision;
    public PlayerTypes playerType;
    public Light lightsource;
    public float approval;
    public Text score;
    public int placeInScoresList;
    ScoreManager sm;

    // Start is called before the first frame update
    void Start()
    {
        lightsource.enabled = false;
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

        else if (other.tag == "Player")
        {
            float otherMag = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float thisMag = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            if (otherMag >= 2.5f && thisMag < otherMag)
            {
                OnPlayerCollision.Invoke(this.gameObject);
            }
        }
    }

    void UpdateScores()
    {
        approval = sm.playerApprovals[placeInScoresList];
        if (approval < 0)
        {
            approval = 0;
            sm.playerApprovals[placeInScoresList] = 0;
        }
        else if (approval > 100)
        {
            approval = 100;
            sm.playerApprovals[placeInScoresList] = 100;
        }
    }
}

