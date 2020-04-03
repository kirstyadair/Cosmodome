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

    [Header("End round Lines")]
    public AudioClip[] AudioArray_Announcer_Banter;
    public AudioClip[] AudioArray_Dave_Out;
    public AudioClip[] AudioArray_HHH_Out;
    public AudioClip[] AudioArray_Mosco_Out;
    public AudioClip[] AudioArray_Schlug_Out;




[Header("Dave Collision Lines")]
    public AudioClip[] AudioArray_Coll_Dave_VS_Mosco;
    public AudioClip[] AudioArray_Coll_Dave_VS_HHH;
    public AudioClip[] AudioArray_Coll_Dave_VS_Schlug;

    [Header("Schlug Collision Lines")]
    public AudioClip[] AudioArray_Coll_Schlug_VS_Dave;
    public AudioClip[] AudioArray_Coll_Schlug_VS_Mosco;
    public AudioClip[] AudioArray_Coll_Schlug_VS_HHH;

    [Header("Mosco Collision Lines")]
    public AudioClip[] AudioArray_Coll_Mosco_VS_Dave;
    public AudioClip[] AudioArray_Coll_Mosco_VS_Schlug;
    public AudioClip[] AudioArray_Coll_Mosco_VS_HHH;

    [Header("HHH Collision Lines")]
    public AudioClip[] AudioArray_Coll_HHH_VS_Dave;
    public AudioClip[] AudioArray_Coll_HHH_VS_Mosco;
    public AudioClip[] AudioArray_Coll_HHH_VS_Schlug;


    public AudioSource AudioPlayer;

    Coroutine _waitForSoundCoroutine;
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
        CutscenesManager.ElMoscoIntro += PlayMoscoIntro;
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
        CutscenesManager.ElMoscoIntro -= PlayMoscoIntro;

    }

    void Start()
    {
        
    }

    /// <summary>
    /// Cancel a playing announcer line and close their subtitles
    /// </summary>
    public void Cancel() {
        if (!isPlaying) return;

        AudioPlayer.Stop();
        isPlaying = false;
        subtitle.GetComponent<AnnouncerDialouge>().CancelSubtitles();
        
        if (_waitForSoundCoroutine != null) {
            StopCoroutine(_waitForSoundCoroutine);
            _waitForSoundCoroutine = null;
        }
    }

    IEnumerator WaitForSound(AudioSource source)
    {
        isPlaying = true;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);

        _waitForSoundCoroutine = null;
        isPlaying = false;
        
    }
    void AudioPlayerChangeSoundClip(AudioClip[] audioArray, AudioSource source, int randomClip)
    {
        source.clip = audioArray[randomClip];
        AudioPlayerPlaySound();
    }

    void AudioPlayerPlaySound()
    {
        if (_waitForSoundCoroutine != null) StopCoroutine(_waitForSoundCoroutine);
        _waitForSoundCoroutine = StartCoroutine(WaitForSound(AudioPlayer));
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
    void PlayMoscoIntro()
    {
        int randomClip = Random.Range(0, AudioArray_ElMoscoIntro.Length);
        AudioPlayerChangeSoundClip(AudioArray_ElMoscoIntro, AudioPlayer, randomClip);
        subtitle.GetComponent<AnnouncerDialouge>().DisplayMoscoIntoSubtile(randomClip, AudioArray_ElMoscoIntro[randomClip].length);
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
            float chance = 0.75f;
            float rand = Random.value;
            if (rand <= chance)
            {
                if (playerAttacking.GetComponent<PlayerScript>().playerType == PlayerTypes.DAVE)
                {
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.EL_MOSCO)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Dave_VS_Mosco.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Dave_VS_Mosco, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayDaveCollisionSubtitle("Mosco", randomClip, AudioArray_Coll_Dave_VS_Mosco[randomClip].length);

                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.BIG_SCHLUG)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Dave_VS_Schlug.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Dave_VS_Schlug, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayDaveCollisionSubtitle("Schlug", randomClip, AudioArray_Coll_Dave_VS_Schlug[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.HAMMER)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Dave_VS_HHH.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Dave_VS_HHH, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayDaveCollisionSubtitle("HHH", randomClip, AudioArray_Coll_Dave_VS_HHH[randomClip].length);
                    }
                    
                }

                if (playerAttacking.GetComponent<PlayerScript>().playerType == PlayerTypes.BIG_SCHLUG)
                {
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.DAVE)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Schlug_VS_Dave.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Schlug_VS_Dave, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplaySchlugCollisionSubtitle("Dave", randomClip, AudioArray_Coll_Schlug_VS_Dave[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.EL_MOSCO)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Schlug_VS_Mosco.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Schlug_VS_Mosco, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplaySchlugCollisionSubtitle("Mosco", randomClip, AudioArray_Coll_Schlug_VS_Mosco[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.HAMMER)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Schlug_VS_HHH.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Schlug_VS_HHH, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplaySchlugCollisionSubtitle("HHH", randomClip, AudioArray_Coll_Schlug_VS_HHH[randomClip].length);
                    }
                }

                if (playerAttacking.GetComponent<PlayerScript>().playerType == PlayerTypes.EL_MOSCO)
                {
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.DAVE)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Mosco_VS_Dave.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Mosco_VS_Dave, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayMoscoCollisionSubtitle("Dave", randomClip, AudioArray_Coll_Mosco_VS_Dave[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.BIG_SCHLUG)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Mosco_VS_Schlug.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Mosco_VS_Schlug, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayMoscoCollisionSubtitle("Schlug", randomClip, AudioArray_Coll_Mosco_VS_Schlug[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.HAMMER)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_Mosco_VS_HHH.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_Mosco_VS_HHH, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayMoscoCollisionSubtitle("HHH", randomClip, AudioArray_Coll_Mosco_VS_HHH[randomClip].length);
                    }
                }

                if (playerAttacking.GetComponent<PlayerScript>().playerType == PlayerTypes.HAMMER)
                {
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.DAVE)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_HHH_VS_Dave.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_HHH_VS_Dave, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayHHHCollisionSubtitle("Dave", randomClip, AudioArray_Coll_HHH_VS_Dave[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.EL_MOSCO)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_HHH_VS_Mosco.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_HHH_VS_Mosco, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayHHHCollisionSubtitle("Mosco", randomClip, AudioArray_Coll_HHH_VS_Mosco[randomClip].length);
                    }
                    if (playerHit.GetComponent<PlayerScript>().playerType == PlayerTypes.BIG_SCHLUG)
                    {
                        int randomClip = Random.Range(0, AudioArray_Coll_HHH_VS_Schlug.Length);
                        AudioPlayerChangeSoundClip(AudioArray_Coll_HHH_VS_Schlug, AudioPlayer, randomClip);
                        subtitle.GetComponent<AnnouncerDialouge>().DisplayHHHCollisionSubtitle("Schlug", randomClip, AudioArray_Coll_HHH_VS_Schlug[randomClip].length);
                    }
                }
            }
            


        }
    }

    void AudioPlayerTaunting()
    {

    }

    
}
