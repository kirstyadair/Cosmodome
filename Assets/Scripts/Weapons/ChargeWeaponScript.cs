using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChargeWeaponScript : MonoBehaviour
{
    InputDevice controller;
    PlayerScript playerScript;
    
    float chargeCount;
    bool decreasing = false;
    bool charged = false;

    public GameObject spawnPoint;
    public GameObject bulletPrefab;
    public ParticleSystem chargePs;
    public ParticleSystem charge2Ps;
    public ParticleSystem shootPs;
    [Header("Weapon charge time before firing")]
    public float chargeNeeded;
    [Header("How fast charge decreases")]
    public float decreaseMultiplier;

    public delegate void ChargeWeaponFire();
    public static event ChargeWeaponFire OnChargeWeaponFire;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.inputDevice != null)
        {
            controller = playerScript.inputDevice;

            if (controller.RightBumper.IsPressed && chargeCount < chargeNeeded)
            {
                if (!chargePs.gameObject.activeInHierarchy) chargePs.gameObject.SetActive(true);
                if (!charge2Ps.gameObject.activeInHierarchy) charge2Ps.gameObject.SetActive(true);

                decreasing = false;
                chargeCount += Time.deltaTime;

                if (chargeCount >= chargeNeeded)
                {
                    charged = true;
                }
            }

            if (controller.RightBumper.WasReleased)
            {
                chargePs.gameObject.SetActive(false);
                charge2Ps.gameObject.SetActive(false);
                decreasing = true;

                if (charged == true)
                {
                    OnChargeWeaponFire?.Invoke();
                    Fire();
                }
            }
        }

        if (decreasing && chargeCount > 0)
        {
            chargeCount -= Time.deltaTime * decreaseMultiplier;
        }
        else if (chargeCount <= 0) decreasing = false;

        Debug.Log(chargeCount);
    }


    void Fire()
    {
        shootPs.gameObject.SetActive(true);
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.identity);
    }
}
