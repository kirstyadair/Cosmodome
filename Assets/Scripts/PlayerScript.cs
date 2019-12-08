using InControl;
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
    public Light spotlight;
    public int maxBrightness;

    public void ChangeApproval(int amount)
    {
        Debug.Log("changing approval by " + amount);
        value += amount;
        if (value > 100) value = 100;
        if (value < 0) value = 0;
    }
}

public class PlayerScript : MonoBehaviour
{
    public delegate void PlayerShot(GameObject playerHit, GameObject shooter);
    public static event PlayerShot OnPlayerShot;
    public static event PlayerShot OnPlayerHitByArenaCannon;
    public delegate void PlayerCollision(GameObject playerHit);
    public static event PlayerCollision OnPlayerCollision;

    //AnnouncerAudio Events Could be used for more though :3
    public delegate void AnnouncerEvent();
    public static event AnnouncerEvent PlayerShotHit;
    
    
    public static event AnnouncerEvent PlayerOnPlayerCollision;
    public static event AnnouncerEvent PlayerTaunting;
    //The maybe do pile if there is time
    public static event AnnouncerEvent ComedianInactive;
    public static event AnnouncerEvent DaredevilNearPlayerMiss;
    public static event AnnouncerEvent DaredevilNearTrapMiss;
    public static event AnnouncerEvent LightweightEngineDisable;

    public int playerNumber;
    public PlayerTypes playerType;
    public Text score;
    public int placeInScoresList;
    ScoreManager sm;
    public Material playerColor;
    public Material normalMaterial;
    public Material flashMaterial;
    public MeshRenderer[] parts;
    public int flashTimes;
    public float flashDuration;
    public float flashGaps;

    public float hitByBulletCooldown = 0;
    public float timeBetweenHitByBullet = 0.5f;

    public GameObject ring;
    public GameObject turretRing;
    public float ringHeight;

    ShipController controller;
    public PlayerApproval approval = new PlayerApproval();
    //Bens code change start
    public RawImage smallArrow;
    public RawImage largeArrow;
    public RawImage smallArrowPlus;
    public RawImage largeArrowPlus;

    public GameObject playersScreen;

    //Bens Code change end

    public InputDevice inputDevice;

    public bool isActivatingTrap = false;
    //public bool isActive = true;

    public GameObject ps;
    
    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
        controller = GetComponent<ShipController>();
        approval.ChangeApproval(0);

        Color tempColor = largeArrow.color;
        tempColor.a = 0f;
        largeArrow.color = tempColor;
        smallArrow.color = tempColor;
        smallArrowPlus.color = tempColor;
        largeArrowPlus.color = tempColor;
        
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

    
    //Bens code change start
    public IEnumerator ArrowFlash(float timeMultiplier, int arrowType,int plusOrMinus)
    {
        //Small arrow = 0 | Larg arrow = 1
        //Plus arrow = 1 | Minus arrow = 0

        if(arrowType == 0 && plusOrMinus == 0)
        {
            smallArrow.color = new Color(smallArrow.color.r, smallArrow.color.g, smallArrow.color.b, 1);

            while (smallArrow.color.a > 0.0f)
            {
                smallArrow.color = new Color(smallArrow.color.r, smallArrow.color.g, smallArrow.color.b, smallArrow.color.a - (Time.deltaTime * timeMultiplier));
                yield return null;
            }
        }

        if(arrowType ==1&&plusOrMinus ==0)
        {
            largeArrow.color = new Color(largeArrow.color.r, largeArrow.color.g, largeArrow.color.b, 1);

            while (largeArrow.color.a > 0.0f)
            {
                largeArrow.color = new Color(largeArrow.color.r, largeArrow.color.g, largeArrow.color.b, largeArrow.color.a - (Time.deltaTime * timeMultiplier));
                yield return null;
            }
        }
        if (arrowType == 0 && plusOrMinus == 1)
        {
            smallArrowPlus.color = new Color(smallArrowPlus.color.r, smallArrowPlus.color.g, smallArrowPlus.color.b, 1);

            while (smallArrowPlus.color.a > 0.0f)
            {
                smallArrowPlus.color = new Color(smallArrowPlus.color.r, smallArrowPlus.color.g, smallArrowPlus.color.b, smallArrowPlus.color.a - (Time.deltaTime * timeMultiplier));
                yield return null;
            }
        }
        if (arrowType == 1 && plusOrMinus == 1)
        {
            largeArrowPlus.color = new Color(largeArrowPlus.color.r, largeArrowPlus.color.g, largeArrowPlus.color.b, 1);

            while (largeArrowPlus.color.a > 0.0f)
            {
                largeArrowPlus.color = new Color(largeArrowPlus.color.r, largeArrowPlus.color.g, largeArrowPlus.color.b, largeArrowPlus.color.a - (Time.deltaTime * timeMultiplier));
                yield return null;
            }
        }

    }
    //Bens code change end

    public void Vibrate(float strength, float time)
    {
        if (inputDevice == null) return;

        Debug.Log("Vibrating " + strength + " for " + time + "s", gameObject);
        inputDevice.Vibrate(strength);
        StartCoroutine(StopVibratingAfter(time));
    }

    IEnumerator StopVibratingAfter(float time)
    {
        yield return new WaitForSeconds(time);
        if (inputDevice == null) yield break;

        inputDevice.StopVibration();
    }


    public void EnableRing(Color color)
    {
        float alpha = ring.GetComponent<SpriteRenderer>().color.a;
        color.a = alpha;
        ring.SetActive(true);
        ring.GetComponent<SpriteRenderer>().color = color;

        turretRing.GetComponent<SpriteRenderer>().color = color;

        // put ring on the floor
        Vector3 position = this.transform.position;
        position.y = ringHeight;
        ring.transform.position = position;
        ring.transform.up = Vector3.forward;

    }

    public void DisableRing()
    {
        ring.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ring.activeSelf)
        {
            // put ring on the floor
            Vector3 position = this.transform.position;
            position.y = ringHeight;
            ring.transform.position = position;
            //ring.transform.up = Vector3.forward;
            ring.transform.rotation = Quaternion.LookRotation(Vector3.up, this.transform.forward);

            if (controller.turretDirection.magnitude > 0) turretRing.transform.rotation = Quaternion.LookRotation(Vector3.up, controller.turretDirection);
            else turretRing.transform.rotation = ring.transform.rotation;
        }

        //Bens code change start
        score.text =approval.percentage + "%";
        //Bens code change end
        if (hitByBulletCooldown > 0) hitByBulletCooldown -= Time.deltaTime;

        if (inputDevice != null)
        {
            controller.turretDirection = new Vector3(inputDevice.RightStick.Value.x, 0, inputDevice.RightStick.Value.y);

            if (inputDevice.RightBumper.IsPressed) controller.Fire();

            isActivatingTrap = inputDevice.Action1.IsPressed;
            controller.targetDirection = new Vector3(inputDevice.LeftStick.Value.x, 0, inputDevice.LeftStick.Value.y);
            controller.targetDirection.Normalize();
            controller.targetDirection *= inputDevice.LeftStick.Value.magnitude;

            if (inputDevice.LeftBumper.WasPressed)
            {
                controller.Boost();
            }

        } else if (playerNumber == 1)
        {
            // if player 1, default to keyboard controls
            controller.targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.targetDirection.Normalize();
            isActivatingTrap = Input.GetKey(KeyCode.X);
        } else
        {
            // just go to center
            isActivatingTrap = false;
            
            controller.targetDirection = GameObject.Find("Center").transform.position - this.transform.position;
            if (controller.targetDirection.magnitude < 1f) controller.targetDirection = Vector3.zero;
            else controller.targetDirection.Normalize();
        }
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

        Vibrate(0.5f, 0.2f);
        hitByBulletCooldown = timeBetweenHitByBullet;
        OnPlayerShot?.Invoke(this.gameObject, bullet.shooter);
        PlayerShotHit?.Invoke();

    }

    public void WasCollidedWith()
    {
        OnPlayerCollision.Invoke(this.gameObject);
        PlayerOnPlayerCollision?.Invoke();
        playersScreen.GetComponent<ScreenAnim>().Scared();
  
    }

    public void WasHitWithArenaCannon(PlayerScript shooter)
    {
        OnPlayerHitByArenaCannon?.Invoke(this.gameObject, shooter.gameObject);
        PlayerShotHit?.Invoke();
        StartCoroutine(controller.Careen(controller.disabledTime, controller.careenTime));

        Vibrate(5f,1f);
    }

    public void DisableTurretRing()
    {
        Color color = Color.black;
        float alpha = turretRing.GetComponent<SpriteRenderer>().color.a;
        color.a = alpha;

        turretRing.GetComponent<SpriteRenderer>().color = color;
    }

    public void EnableTurretRing()
    {
        Color color = playerColor.color;
        float alpha = turretRing.GetComponent<SpriteRenderer>().color.a;
        color.a = alpha;

        turretRing.GetComponent<SpriteRenderer>().color = color;
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

