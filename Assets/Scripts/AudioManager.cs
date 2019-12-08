using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClips[] clips = new AudioClips[13];

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerScript.OnPlayerCollision += PlayerCollision;
        ShipController.OnPlayerShooting += PlayerShooting;
        ArenaCannonManager.OnOpenCannon += OpenCannon;
        ArenaCannonMissile.OnFireCannon += FireCannon;
        ScoreManager.OnExplodePlayer += ExplodePlayer;
        ArenaCannonScript.OnTrapActivate += TrapActivated;
        ShipController.OnPlayerReload += Reload;
    }

    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayerCollision(GameObject playerHit)
    {
        audioSource.volume = clips[5].volume;
        audioSource.pitch = 0.5f + UnityEngine.Random.value;
        audioSource.PlayOneShot(clips[5].clip);
        audioSource.PlayOneShot(clips[8].clip);
    }


    void PlayerShooting()
    {
        audioSource.volume = clips[13].volume;
        audioSource.pitch = UnityEngine.Random.value * 2;
        if (UnityEngine.Random.value > 0.5f) audioSource.PlayOneShot(clips[13].clip);
        else audioSource.PlayOneShot(clips[14].clip);
    }

    void PlayerScream()
    {
        int rand = UnityEngine.Random.Range(16, 18);
        audioSource.volume = clips[rand].volume;
        audioSource.PlayOneShot(clips[rand].clip);
    }

    void OpenCannon()
    {
        audioSource.volume = clips[3].volume;
        audioSource.pitch = UnityEngine.Random.value * 2;
        audioSource.PlayOneShot(clips[3].clip);
    }


    void FireCannon()
    {
        audioSource.volume = clips[4].volume;
        audioSource.pitch = UnityEngine.Random.value * 2;
        audioSource.PlayOneShot(clips[4].clip);
    }


    void ExplodePlayer()
    {
        audioSource.volume = clips[15].volume;
        audioSource.PlayOneShot(clips[15].clip);
        audioSource.volume = clips[7].volume;
        audioSource.PlayOneShot(clips[7].clip);
        audioSource.volume = clips[0].volume;
        audioSource.PlayOneShot(clips[0].clip);
        PlayerScream();
        
    }


    void TrapActivated()
    {
        audioSource.volume = clips[12].volume;
        audioSource.PlayOneShot(clips[12].clip);
    }


    void Reload()
    {
        audioSource.volume = clips[9].volume;
        audioSource.PlayOneShot(clips[9].clip);
    }
}

[Serializable]
public class AudioClips
{
    public AudioClip clip;
    [Range(0,1)]public float volume;

}
