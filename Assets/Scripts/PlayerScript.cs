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
    public float approval;
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
    
    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
        ScoreManager.OnUpdateScore += UpdateScores;
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

        score.text = playerType.ToString() + ": " + System.Math.Round(approval, 0) + "%";
        if (hitByBulletCooldown > 0) hitByBulletCooldown -= Time.deltaTime;
        //sm.playerApprovals[placeInScoresList] = approval;
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
    }

    public void WasCollidedWith()
    {
        OnPlayerCollision.Invoke(this.gameObject);
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

