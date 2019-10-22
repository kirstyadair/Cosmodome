using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTypes
{
    COMEDIAN, DAREDEVIL, LIGHTWEIGHT, HEAVYWEIGHT, DAVE
}

public class PlayerScript : MonoBehaviour
{
    public delegate void PlayerShot(PlayerTypes playerShooting, PlayerTypes playerHit);
    public static event PlayerShot OnPlayerShot;
    public PlayerTypes playerType;
    public int approval;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GreenBullet" && playerType == PlayerTypes.LIGHTWEIGHT)
        {
            OnPlayerShot.Invoke(PlayerTypes.DAVE, playerType);
        }
        else if (other.tag == "RedBullet" && playerType == PlayerTypes.DAVE)
        {
            OnPlayerShot.Invoke(PlayerTypes.LIGHTWEIGHT, playerType);
        }
    }
}
