using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChargeWeaponScript : MonoBehaviour
{
    InputDevice controller;
    PlayerScript ps;
    
    float chargeCount;
    bool decreasing = false;
    bool charged = false;

    [Header("Weapon charge time before firing")]
    public float chargeNeeded;
    [Header("How fast charge decreases")]
    public float decreaseMultiplier;

    public delegate void ChargeWeaponFire();
    public static event ChargeWeaponFire OnChargeWeaponFire;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps.inputDevice != null)
        {
            controller = ps.inputDevice;

            if (controller.RightBumper.IsPressed && chargeCount < chargeNeeded)
            {
                decreasing = false;
                chargeCount += Time.deltaTime;

                if (chargeCount >= chargeNeeded)
                {
                    charged = true;
                }
            }

            if (controller.RightBumper.WasReleased)
            {
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

    }
}
