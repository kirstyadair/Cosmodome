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


    public GameObject subtitle;
    
    public bool isPlaying = false;
    

    void OnEnable()
    {
        PlayerScript.OnPlayerShot += AudioPlayerShot;
        WallScript.OnTrapHit += AudioPlayerTrapTrigger;
        WallScript.OnTrapSabotaged += AudioPlayerTrapSetup;
        PlayerScript.OnPlayerCollision += AudioPlayerOnPlayerCollision;
        ScoreManager.OnPlayerEliminated += AudioPlayerEliminated;
    }

     void OnDisable()
    {
        PlayerScript.OnPlayerShot -= AudioPlayerShot;
        WallScript.OnTrapHit -= AudioPlayerTrapTrigger;
        WallScript.OnTrapSabotaged -= AudioPlayerTrapSetup;
        PlayerScript.OnPlayerCollision -= AudioPlayerOnPlayerCollision;
        ScoreManager.OnPlayerEliminated -= AudioPlayerEliminated;
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

    void AudioPlayerShot(PlayerScript playerHit, PlayerScript shooter)
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

    void AudioPlayerTrapTrigger(PlayerScript playerHit, Traps trapType)
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

    void AudioPlayerTrapSetup(PlayerScript playerImmune, Traps trapType, bool successful)
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

    void AudioPlayerOnPlayerCollision(PlayerScript playerHit, PlayerScript playerAttacking)
    {
        if (!isPlaying)
        {
            float chance = 0.50f;
            float rand = Random.value;
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
