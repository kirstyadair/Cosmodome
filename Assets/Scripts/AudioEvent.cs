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
    public AudioClip[] AudioArrayPlyerEliminated;

    public AudioSource AudioPlayer;


    private GameObject subtitle;
    
    public bool isPlaying = false;
    

    void OnEnable()
    {
        PlayerScript.PlayerShotHit += AudioPlayerShot;
        WallScript.PlayerTrapTrigger += AudioPlayerTrapTrigger;
        WallScript.PlayerTrapSetup += AudioPlayerTrapSetup;
        PlayerScript.PlayerOnPlayerCollision += AudioPlayerOnPlayerCollision;
        PlayerScript.PlayerTaunting += AudioPlayerTaunting;
        ScoreManager.OnPlayerEliminated += AudioPlayerEliminated;
    }

     void OnDisable()
    {
        PlayerScript.PlayerShotHit -= AudioPlayerShot;
        WallScript.PlayerTrapTrigger -= AudioPlayerTrapTrigger;
        WallScript.PlayerTrapSetup -= AudioPlayerTrapSetup;
        PlayerScript.PlayerOnPlayerCollision -= AudioPlayerOnPlayerCollision;
        PlayerScript.PlayerTaunting -= AudioPlayerTaunting;
        ScoreManager.OnPlayerEliminated -= AudioPlayerEliminated;
    }

    void Start()
    {
        subtitle = GameObject.Find("Subtitles");
    }

    IEnumerator WaitForSound(AudioSource source)
    {
        isPlaying = true;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);

        
        

        isPlaying = false;
        
    }
    void AudioPlayerChangeSoundClip(AudioClip[] audioArray, AudioSource source, int randomClip)
    {
        
        source.clip = audioArray[randomClip];
        AudioPlayerPlaySound();
        

    }

    void AudioPlayerPlaySound()
    {
            StartCoroutine(WaitForSound(AudioPlayer));
    }

   
    
    void AudioPlayerEliminated()
    {
        if (!isPlaying)
        {
            int randomClip = Random.Range(0, AudioArrayPlyerEliminated.Length);
            AudioPlayerChangeSoundClip(AudioArrayPlyerEliminated, AudioPlayer, randomClip);
            subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerShotSubtitle(randomClip);
        }
    }

    void AudioPlayerShot()
    {
        if (!isPlaying)
        {
            float chance = 0.50f;
            float rand = Random.value;
            if (rand <= chance)
            {
                int randomClip = Random.Range(0, AudioArrayPlayerShot.Length);
                AudioPlayerChangeSoundClip(AudioArrayPlayerShot, AudioPlayer, randomClip);
                subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerShotSubtitle(randomClip);
            }
            
        }  
    }

    void AudioPlayerTrapTrigger()
    {
        if (!isPlaying)
        {
            float chance = 0.50f;
            float rand = Random.value;
            if (rand <= chance)
            {
                int randomClip = Random.Range(0, AudioArrayPlayerTrapTrigger.Length);
                AudioPlayerChangeSoundClip(AudioArrayPlayerTrapTrigger, AudioPlayer, randomClip);
                subtitle.GetComponent<AnnouncerDialouge>().DiaplayPlayerTrapTriggerSubtitle(randomClip);
            }
            
        }

    }

    void AudioPlayerTrapSetup()
    {
        if (!isPlaying)
        {
            float chance = 0.50f;
            float rand = Random.value;
            if (rand <= chance)
            {
                int randomClip = Random.Range(0, AudioArrayPlayerTrapSetup.Length);
                AudioPlayerChangeSoundClip(AudioArrayPlayerTrapSetup, AudioPlayer, randomClip);
                subtitle.GetComponent<AnnouncerDialouge>().DiaplayPlayerTrapSetupSubtitle(randomClip);
            }
            
        }
    }

    void AudioPlayerOnPlayerCollision()
    {
        if (!isPlaying)
        {
            float chance = 0.50f;
            float rand = Random.value;
            print(rand);
            if (rand <= chance)
            {
                int randomClip = Random.Range(0, AudioArrayPlayerOnPlayerCollision.Length);
                AudioPlayerChangeSoundClip(AudioArrayPlayerOnPlayerCollision, AudioPlayer, randomClip);
                subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerOnPlayerCollisionSubtitle(randomClip);
            }
            
        }
    }

    void AudioPlayerTaunting()
    {

    }

    
}
