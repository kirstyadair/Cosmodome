using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerTypes
{
    COMEDIAN, DAREDEVIL, LIGHTWEIGHT, HEAVYWEIGHT, DAVE, NULL
}

[Serializable]
public class PlayerApproval {
    public int value = 0;
    public int percentage;

    public void ChangeApproval(int amount)
    {
        if (percentage >= 100) return;

        value += amount;
    }
}

public class PlayerScript : MonoBehaviour
{
    public delegate void PlayerShot(GameObject playerHit);
    public static event PlayerShot OnPlayerShot;
    public delegate void PlayerCollision(GameObject playerHit);
    public static event PlayerCollision OnPlayerCollision;

    //AnnouncerAudio Events Could be used for more though :3
    public delegate void AnnouncerEvent();
    public static event AnnouncerEvent PlayerShotHit;
    public static event AnnouncerEvent PlayerTrapTrigger;
    public static event AnnouncerEvent PlayerTrapSetup;
    public static event AnnouncerEvent PlayerOnPlayerCollision;
    public static event AnnouncerEvent PlayerTaunting;
    //The maybe do pile if there is time
    public static event AnnouncerEvent ComedianInactive;
    public static event AnnouncerEvent DaredevilNearPlayerMiss;
    public static event AnnouncerEvent DaredevilNearTrapMiss;
    public static event AnnouncerEvent LightweightEngineDisable;


    public PlayerTypes playerType;
    public Text score;
    public int placeInScoresList;
    ScoreManager sm;

    public Material normalMaterial;
    public Material flashMaterial;
    public MeshRenderer[] parts;
    public int flashTimes;
    public float flashDuration;
    public float flashGaps;

    public float hitByBulletCooldown = 0;
    public float timeBetweenHitByBullet = 0.5f;

    public PlayerApproval approval = new PlayerApproval();
    
    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
        //ScoreManager.OnUpdateScore += UpdateScores;
    }

    public IEnumerator FlashWithDamage()
    {
        Debug.Log("Flsh");
        void Flash() { foreach (MeshRenderer part in parts) part.material = flashMaterial; }
        void Normal() { foreach (MeshRenderer part in parts) part.material = normalMaterial; }

        for (int i = 0; i < flashTimes; i++)
        {
            
            Flash();
            yield return new WaitForSeconds(flashDuration);
            Normal();
            yield return new WaitForSeconds(flashGaps);
        }

        Normal();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = playerType.ToString() + ": " + approval.percentage + "%";
        if (hitByBulletCooldown > 0) hitByBulletCooldown -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        /*
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
        }*/
    }

    public void WasHitByBullet(BulletDeleter bullet)
    {
        if (hitByBulletCooldown > 0) return;

        hitByBulletCooldown = timeBetweenHitByBullet;
        OnPlayerShot?.Invoke(this.gameObject);
        PlayerShotHit();

    }

    public void WasCollidedWith()
    {
        OnPlayerCollision.Invoke(this.gameObject);
        PlayerOnPlayerCollision();
    }

    /*
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
    }*/
}

