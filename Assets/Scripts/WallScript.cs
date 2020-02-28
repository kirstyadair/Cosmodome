using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class WallScript : MonoBehaviour
{
    bool isTrap = false;
    bool playerTouching = false;
    PlayerScript playerTouchingType;
    PlayerScript immunePlayer;
    public delegate void TrapHit(PlayerScript playerHit, Traps trapType);
    public static event TrapHit OnTrapHit;
    public delegate void TrapSabotaged(PlayerScript playerImmune, Traps trapType, bool successful);
    public static event TrapSabotaged OnTrapSabotaged;

    public delegate void AnnouncerEvent();
    public static event AnnouncerEvent PlayerTrapSetup;
    public static event AnnouncerEvent PlayerTrapTrigger;

    public Material standardMat;

    private void Update()
    {
        if (playerTouching && (Input.GetKey(KeyCode.E) || InputManager.ActiveDevice.Action1.WasPressed))
        {
            isTrap = true;
            SetTrap();
            immunePlayer = playerTouchingType;
            PlayerTrapSetup?.Invoke();

        }

        if (playerTouching && (Input.GetKey(KeyCode.R) || InputManager.ActiveDevice.Action2.WasPressed))
        {
            isTrap = true;
            SabotageTrap();
            immunePlayer = playerTouchingType;
            if (Mathf.RoundToInt(Random.Range(0, 1.0f)) == 1)
            {
                Debug.Log("sabotage successful");
                OnTrapSabotaged.Invoke(immunePlayer, Traps.SPIKEWALL, true);
            }
            else
            {
                Debug.Log("sabotage failed");
                OnTrapSabotaged.Invoke(immunePlayer, Traps.SPIKEWALL, false);
            }

            isTrap = false;
            GetComponent<MeshRenderer>().material = standardMat;
        }

        if (playerTouching && isTrap)
        {
            if (playerTouchingType.playerType == immunePlayer.GetComponent<PlayerScript>().playerType)
            {
                //Debug.Log("This player is immune to the trap");
            }
            else
            {
                //Debug.Log("IsInvoking");
                OnTrapHit.Invoke(playerTouchingType, Traps.SPIKEWALL);
                isTrap = false;
                GetComponent<MeshRenderer>().material = standardMat;
                PlayerTrapTrigger?.Invoke();
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
            playerTouchingType = other.GetComponent<PlayerScript>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ship")
        {
            playerTouching = false;
            playerTouchingType = null;
        }
    }

    private void SetTrap()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void SabotageTrap()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
