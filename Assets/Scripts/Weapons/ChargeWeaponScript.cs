using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChargeWeaponScript : MonoBehaviour
{
    InputDevice controller;
    PlayerScript playerScript;
    [SerializeField]
    float chargeAmount;

    [SerializeField]
    bool isCharged = false;

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


    [Header("How much to multiply the charge level by when not being held")]
    public float decreaseMuliplier = 0.85f;

    public delegate void ChargeWeaponFire();
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
        playerRings.IsChargingWeapon();
    }

    public void StartCharging()
    {
        isCharging = true;
        charge2Ps.gameObject.SetActive(true);
        playerRings.StartCharging();
    }

    public void StopCharging()
    {
        isCharging = false;
        isCharged = false;
        charge2Ps.gameObject.SetActive(false);
        playerRings.StopCharging();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.inputDevice == null) return;

        controller = playerScript.inputDevice;

        // If the button is held
        if (controller.RightBumper.IsPressed && !isFiring)
        {
            // Only charge when we  are not firing
            if (!isCharging) StartCharging();

            chargeAmount += Time.deltaTime;

            // Fully charged?
            if (chargeAmount >= chargeNeeded)
            {
                isCharged = true;
                playerRings.FullyCharged();
                chargeAmount = chargeNeeded;
            }
           
        } else
        {
            // If  fully charge and button released,  fire!
            if (isCharged && !isFiring)
            {
                Fire();
            }

            if (isCharging) StopCharging();

            // Otherwise start reducing charge level
            if (chargeAmount > 0.1)
            {
                chargeAmount *= decreaseMuliplier;
            }  else
            {
                chargeAmount = 0;
            }
        }

        playerRings.UpdateCharge(chargePercentage);
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
        Debug.Log("stopped firing"); 
    }

    void Fire()
    {
        isFiring = true;
        isCharged = false;
        OnChargeWeaponFire?.Invoke();
        shootPs.gameObject.SetActive(true);
        laser.GetComponent<Animator>().SetBool("LaserOn", true);
        deleter.enabled = true;

        StartCoroutine(StopFiringAfter(laserHoldTime));
    }
}
