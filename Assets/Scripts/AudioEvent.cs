using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    public AudioClip[] AudioArrayPlayerShot;
    public AudioClip[] AudioArrayPlayerTrapTrigger;
    public AudioClip[] AudioArrayPlayerTrapSetup;
    public AudioClip[] AudioArrayPlayerOnPlayerCollision;
    public AudioClip[] AudioArrayPlayerTaunting;

    public AudioSource AudioPlayer;
    
    bool isPlaying = false;
    

    void OnEnable()
    {
        PlayerScript.PlayerShotHit += AudioPlayerShot;
        WallScript.PlayerTrapTrigger += AudioPlayerTrapTrigger;
        WallScript.PlayerTrapSetup += AudioPlayerTrapSetup;
        PlayerScript.PlayerOnPlayerCollision += AudioPlayerOnPlayerCollision;
        PlayerScript.PlayerTaunting += AudioPlayerTaunting;
    }

     void OnDisable()
    {
        PlayerScript.PlayerShotHit -= AudioPlayerShot;
        WallScript.PlayerTrapTrigger -= AudioPlayerTrapTrigger;
        WallScript.PlayerTrapSetup -= AudioPlayerTrapSetup;
        PlayerScript.PlayerOnPlayerCollision -= AudioPlayerOnPlayerCollision;
        PlayerScript.PlayerTaunting -= AudioPlayerTaunting;
    }

    void Start()
    {
        
    }

    IEnumerator WaitForSound(AudioSource source)
    {
        isPlaying = true;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);

        isPlaying = false;
        
    }
    void AudioPlayerChangeSoundClip(AudioClip[] audioArray, AudioSource source)
    {
        source.clip = audioArray[Random.Range(0, audioArray.Length)];
        AudioPlayerPlaySound();
    }

    void AudioPlayerPlaySound()
    {
            StartCoroutine(WaitForSound(AudioPlayer));
    }

   
    
    

    void AudioPlayerShot()
    {
        if (!isPlaying)
        {
            AudioPlayerChangeSoundClip(AudioArrayPlayerShot, AudioPlayer);
        }  
    }

    void AudioPlayerTrapTrigger()
    {
        if (!isPlaying)
        {
            AudioPlayerChangeSoundClip(AudioArrayPlayerTrapTrigger, AudioPlayer);
        }

    }

    void AudioPlayerTrapSetup()
    {
        if (!isPlaying)
        {
            AudioPlayerChangeSoundClip(AudioArrayPlayerTrapSetup, AudioPlayer);
        }
    }

    void AudioPlayerOnPlayerCollision()
    {
        if (!isPlaying)
        {
            AudioPlayerChangeSoundClip(AudioArrayPlayerOnPlayerCollision, AudioPlayer);
        }
    }

    void AudioPlayerTaunting()
    {

    }

    
}
