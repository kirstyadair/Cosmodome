using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ChargeWeaponScript : MonoBehaviour
{
    InputDevice controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerScript>().inputDevice;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerScript>().inputDevice == null) Debug.Log("F");
        
        if (controller.RightBumper.IsPressed)
        {
            Debug.Log("down");
        }

        if (controller.RightBumper.WasReleased)
        {
            Debug.Log("up");
        }
        
    }
}
