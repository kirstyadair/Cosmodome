using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChargeWeaponScript : MonoBehaviour
{
    InputDevice controller;
    PlayerScript playerScript;
    ShipController shipController;
    ScoreManager sm;
    [SerializeField]
    float chargeAmount;

    [SerializeField]
    bool isCharged = true;

    [SerializeField]
    [Header("Does the weapon need full charge to fire?")]
    bool requireFullCharge;
    bool canFire;

    public GameObject spawnPoint;
    public GameObject laser;
    //public ParticleSystem chargePs;
    public ParticleSystem charge2Ps;
    public ParticleSystem shootPs;
    public BulletDeleter deleter;
    public PlayerRing playerRings;

    [Header("Weapon charge time before firing")]
    public float chargeNeeded;

    [Header("How long to hold the laser on for")]
    public float laserHoldTime;

    [Header("How fast the laser should rotate when firing")]
    public float laserRotateSpeedWhenFiring;

    [Header("Damage that this will cause the attacked player")]
    public int damage;

    [Header("How much to multiply the charge level by when not being held")]
    public float decreaseMuliplier = 0.85f;

    public delegate void ChargeWeaponFire(ShipController ship);
    public static event ChargeWeaponFire OnChargeWeaponFire;

    /// <summary>
    /// If the laser is currently firing
    /// </summary>
    public bool isFiring = false;
    public bool isCharging = false;

    /// <summary>
    ///  0 not charged,  1 fully charged
    /// </summary>
    public float chargePercentage
    {
        get
        {
            return chargeAmount / chargeNeeded;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        shipController = GetComponent<ShipController>();
        sm = ScoreManager.Instance;
    }

    public void StartCharging()
    {
        isCharging = true;
        charge2Ps.gameObject.SetActive(true);
    }

    public void StopCharging()
    {
        isCharging = false;
        isCharged = false;
        charge2Ps.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.inputDevice == null) return;

        controller = playerScript.inputDevice;

        if (isFiring)
        {
            OnChargeWeaponFire?.Invoke(shipController);
            chargeAmount -= Time.deltaTime;
        }

        if (chargeAmount >= 1 && requireFullCharge) canFire = true;
        if (chargeAmount <= 0) StopFiring();

        // If the button is held
        if (!isFiring)
        {
            // Only charge when we  are not firing
            if (!isCharging) StartCharging();

            

            // Fully charged?
            if (chargeAmount >= chargeNeeded)
            {
                isCharged = true;
                chargeAmount = chargeNeeded;
            }
           
        }

        if (controller.RightBumper.IsPressed && sm.gameState == GameState.INGAME)
        {
            if (!requireFullCharge)
            {
                if (chargeAmount > 0) Fire();

                if (isCharging) StopCharging();
            }
            else if (requireFullCharge && canFire)
            {
                // If  fully charge and button released,  fire!
                if (isCharged && !isFiring)
                {
                    Fire();
                }

                if (isCharging) StopCharging();
            }
        }
        else
        {
            StopFiring();
            chargeAmount += Time.deltaTime;
            if (requireFullCharge) canFire = false;
        }
    }


    IEnumerator StopFiringAfter(float time)
    {
        yield return new WaitForSeconds(time);

        StopFiring();
    }

    public void StopFiring()
    {
        isFiring = false;
        shootPs.gameObject.SetActive(false);
        laser.GetComponent<Animator>().SetBool("LaserOn", false);
        deleter.enabled = false;
    }

    void Fire()
    {
        isFiring = true;
        //isCharged = false;
        OnChargeWeaponFire?.Invoke(shipController);
        shootPs.gameObject.SetActive(true);
        laser.GetComponent<Animator>().SetBool("LaserOn", true);
        deleter.enabled = true;
    }
}
