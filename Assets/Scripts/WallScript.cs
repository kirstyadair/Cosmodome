using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class WallScript : MonoBehaviour
{
    bool isTrap = false;
    bool playerTouching = false;
    GameObject playerTouchingType;
    PlayerTypes immunePlayer;
    public delegate void TrapHit(GameObject playerHit, Traps trapType);
    public static event TrapHit OnTrapHit;
    public Material standardMat;

    private void Update()
    {
        if (playerTouching && Input.GetKey(KeyCode.E) || InputManager.ActiveDevice.Action1.WasPressed)
        {
            isTrap = true;
            SetTrap();
            immunePlayer = playerTouchingType.GetComponent<PlayerScript>().playerType;

        }

        if (playerTouching && isTrap)
        {
            if (playerTouchingType.GetComponent<PlayerScript>().playerType == immunePlayer)
            {
                //Debug.Log("This player is immune to the trap");
            }
            else
            {
                //Debug.Log("IsInvoking");
                OnTrapHit.Invoke(playerTouchingType, Traps.SPIKEWALL);
                isTrap = false;
                GetComponent<MeshRenderer>().material = standardMat;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedBullet" || other.tag == "GreenBullet")
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "Ship")
        {
            playerTouching = true;
            playerTouchingType = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerTouching = false;
            playerTouchingType = null;
        }
    }

    private void SetTrap()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
