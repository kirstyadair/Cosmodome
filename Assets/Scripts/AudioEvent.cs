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

    public AudioClip[] AudioArray_WelcomeToTheCosmodome;
    public AudioClip[] AudioArray_DaveIntro;
    public AudioClip[] AudioArray_BigSchlugIntro;
    public AudioClip[] AudioArray_HHHIntro;
    public AudioClip[] AudioArray_ElMoscoIntro;



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

        CutscenesManager.DaveIntro +=PlayDaveIntro;
        CutscenesManager.BigSchlugIntro +=PlayBigSchlugIntro;
        CutscenesManager.HHHIntro +=PlayHHHIntro;
        //CutscenesManager.ElMoscoIntro +=;
    }

     void OnDisable()
    {
        PlayerScript.OnPlayerShot -= AudioPlayerShot;
        WallScript.OnTrapHit -= AudioPlayerTrapTrigger;
        WallScript.OnTrapSabotaged -= AudioPlayerTrapSetup;
        PlayerScript.OnPlayerCollision -= AudioPlayerOnPlayerCollision;
        ScoreManager.OnPlayerEliminated -= AudioPlayerEliminated;

        CutscenesManager.DaveIntro -= PlayDaveIntro;
        CutscenesManager.BigSchlugIntro -= PlayBigSchlugIntro;
        CutscenesManager.HHHIntro -= PlayHHHIntro;
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
            subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerShotSubtitle(randomClip, AudioArrayPlyerEliminated[randomClip].length);
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
                subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerShotSubtitle(randomClip, AudioArrayPlayerShot[randomClip].length);
            }
            
        }  
    }

    void PlayDaveIntro()
    {
        int randomClip = Random.Range(0, AudioArray_DaveIntro.Length);
        AudioPlayerChangeSoundClip(AudioArray_DaveIntro, AudioPlayer, randomClip);
        subtitle.GetComponent<AnnouncerDialouge>().DisplayDaveIntoSubtile(randomClip, AudioArray_DaveIntro[randomClip].length);
    }
    void PlayBigSchlugIntro()
    {
        int randomClip = Random.Range(0, AudioArray_BigSchlugIntro.Length);
        AudioPlayerChangeSoundClip(AudioArray_BigSchlugIntro, AudioPlayer, randomClip);
        subtitle.GetComponent<AnnouncerDialouge>().DisplayBigSchlugIntoSubtile(randomClip, AudioArray_BigSchlugIntro[randomClip].length);
    }
    void PlayHHHIntro()
    {
        int randomClip = Random.Range(0, AudioArray_HHHIntro.Length);
        AudioPlayerChangeSoundClip(AudioArray_HHHIntro, AudioPlayer, randomClip);
        subtitle.GetComponent<AnnouncerDialouge>().DisplayHHHIntoSubtile(randomClip, AudioArray_HHHIntro[randomClip].length);
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
                subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerTrapTriggerSubtitle(randomClip, AudioArrayPlayerTrapTrigger[randomClip].length);
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
                subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerTrapSetupSubtitle(randomClip, AudioArrayPlayerTrapSetup[randomClip].length);
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
                subtitle.GetComponent<AnnouncerDialouge>().DisplayPlayerOnPlayerCollisionSubtitle(randomClip, AudioArrayPlayerOnPlayerCollision[randomClip].length);
            }
            
        }
    }

    void AudioPlayerTaunting()
    {

    }

    
}
