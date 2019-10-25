using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTypes
{
    COMEDIAN, DAREDEVIL, LIGHTWEIGHT, HEAVYWEIGHT, DAVE
}

public class PlayerScript : MonoBehaviour
{
    public delegate void PlayerShot(GameObject playerHit);
    public static event PlayerShot OnPlayerShot;
    public PlayerTypes playerType;
    public Light lightsource;
    public int approval;
    ScoreManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sm.showDamage)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GreenBullet" && playerType == PlayerTypes.LIGHTWEIGHT)
        {
            OnPlayerShot.Invoke(this.gameObject);
        }
        else if (other.tag == "RedBullet" && playerType == PlayerTypes.DAVE)
        {
            OnPlayerShot.Invoke(this.gameObject);
        }
    }
}
