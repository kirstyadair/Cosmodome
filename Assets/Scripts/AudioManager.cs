using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip metalScrape;
    public AudioClip crowdCheer;
    public AudioClip eliminationBuzz;
    public AudioClip ballAppears;
    public AudioClip ballHits;
    public AudioClip cannonAppears;
    public AudioClip cannonHits;
    public AudioClip crash;
    public AudioClip wallCrash;
    public AudioClip reload;
    public AudioClip sabotage;
    public AudioClip weaponFire1;
    public AudioClip weaponFire2;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerScript.OnPlayerCollision += PlayerCollision;
        ShipController.OnPlayerShooting += PlayerShooting;
    }

    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayerCollision(GameObject playerHit)
    {
        audioSource.volume = 0.2f;
        audioSource.pitch = 0.5f + Random.value;
        audioSource.PlayOneShot(crash);
        audioSource.PlayOneShot(metalScrape);
    }


    void PlayerShooting()
    {
        audioSource.volume = 0.2f;
        audioSource.pitch = Random.value * 2;
        if (Random.value > 0.5f) audioSource.PlayOneShot(weaponFire1);
        else audioSource.PlayOneShot(weaponFire2);
    }
}
