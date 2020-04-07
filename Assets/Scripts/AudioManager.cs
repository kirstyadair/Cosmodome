using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource characterIntroAudioSource; // has a seperate source so we can Stop() it when the cutscene is skipped
    [SerializeField] AudioSource countdownSource; // seperate source for countdown so we can start playing at a given offset
    

    public AudioClips[] clips;

    [Header("Character shooting sounds")]
    public AudioClips[] shootingSounds;

    [Header("Countdown sound")]
    public AudioClips countdownSound;

    [Header("Round start sound")]
    public AudioClips roundStartSound;

    [SerializeField] [Header("Dave character intro bgm")] AudioClips daveIntro;
    [SerializeField] [Header("Big Schlug character intro bgm")] AudioClips schlugIntro;
    [SerializeField] [Header("El Mosco character intro bgm")] AudioClips moscoIntro;
    [SerializeField] [Header("Hammerhead Henry character intro bgm")] AudioClips hhhIntro;



    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerScript.OnPlayerCollision += PlayerCollision;
        BasicWeaponScript.OnPlayerShooting += PlayerShooting;
        ChargeWeaponScript.OnChargeWeaponFire += PlayerShooting;
        ArenaCannonManager.OnOpenCannon += OpenCannon;
        ArenaCannonMissile.OnFireCannon += FireCannon;
        ScoreManager.OnExplodePlayer += ExplodePlayer;
        ArenaCannonScript.OnTrapActivate += TrapActivated;
        //ShipController.OnPlayerNoBullets += Reload;
        BumperBall.OnBumperBallFire += BBShoot;
        BumperBall.OnBumperBallExplode += BBExplode;
        BumperBall.OnBumperBallHitPlayer += BBHit;
        BumperBall.OnBumperBallHitWall += BBHitWall;
        TilePrefabScript.OnWallExplode += WallExplode;
        TilePrefabScript.OnWallHit += WallHit;
        ExcitementManager.OnResetHype += CrowdBoo;
        CutscenesManager.OnPlayCountdown += Countdown;
        CutscenesManager.OnCharacterIntroStarted += CharacterIntro;
        CutscenesManager.OnCharacterIntroEnded += OnCharacterIntroEnded;
        CutscenesManager.OnRoundStart += RoundStart;

        
    }

    void OnDisable() {
        PlayerScript.OnPlayerCollision -= PlayerCollision;
        BasicWeaponScript.OnPlayerShooting -= PlayerShooting;
        ChargeWeaponScript.OnChargeWeaponFire -= PlayerShooting;
        ArenaCannonManager.OnOpenCannon -= OpenCannon;
        ArenaCannonMissile.OnFireCannon -= FireCannon;
        ScoreManager.OnExplodePlayer -= ExplodePlayer;
        ArenaCannonScript.OnTrapActivate -= TrapActivated;
        //ShipController.OnPlayerNoBullets += Reload;
        BumperBall.OnBumperBallFire -= BBShoot;
        BumperBall.OnBumperBallExplode -= BBExplode;
        BumperBall.OnBumperBallHitPlayer -= BBHit;
        BumperBall.OnBumperBallHitWall -= BBHitWall;
        TilePrefabScript.OnWallExplode -= WallExplode;
        TilePrefabScript.OnWallHit -= WallHit;
        ExcitementManager.OnResetHype -= CrowdBoo;
        CutscenesManager.OnPlayCountdown -= Countdown;
        CutscenesManager.OnCharacterIntroStarted -= CharacterIntro;
        CutscenesManager.OnCharacterIntroEnded -= OnCharacterIntroEnded;
        CutscenesManager.OnRoundStart -= RoundStart;
    }

    // Update is called once per frame
    void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
    }

    void PlayerCollision(PlayerScript playerHit, PlayerScript playerAttacking)
    {
        mainAudioSource.volume = clips[5].volume;
        mainAudioSource.pitch = 0.5f + UnityEngine.Random.value;
        mainAudioSource.PlayOneShot(clips[5].clip);
        mainAudioSource.PlayOneShot(clips[8].clip);
    }


    void PlayerShooting(ShipController ship)
    {
        int shipSound = ship.shootingClipNumber - 1;

        mainAudioSource.volume = shootingSounds[shipSound].volume;
        //audioSource.pitch = UnityEngine.Random.value * 2;
        mainAudioSource.PlayOneShot(shootingSounds[shipSound].clip);
    }

    void PlayerScream()
    {
        int rand = UnityEngine.Random.Range(16, 18);
        mainAudioSource.volume = clips[rand].volume;
        mainAudioSource.PlayOneShot(clips[rand].clip);
    }

    void OpenCannon()
    {
        mainAudioSource.volume = clips[3].volume;
        mainAudioSource.pitch = UnityEngine.Random.value * 2;
        mainAudioSource.PlayOneShot(clips[3].clip);
    }


    void FireCannon()
    {
        mainAudioSource.volume = clips[4].volume;
        mainAudioSource.pitch = UnityEngine.Random.value * 2;
        mainAudioSource.PlayOneShot(clips[4].clip);
    }


    void ExplodePlayer()
    {
        mainAudioSource.volume = clips[15].volume;
        mainAudioSource.PlayOneShot(clips[15].clip);
        mainAudioSource.volume = clips[7].volume;
        mainAudioSource.PlayOneShot(clips[7].clip);
        mainAudioSource.volume = clips[0].volume;
        mainAudioSource.PlayOneShot(clips[0].clip);
        PlayerScream();
        
    }


    void TrapActivated()
    {
        mainAudioSource.volume = clips[12].volume;
        mainAudioSource.PlayOneShot(clips[12].clip);
    }


    void Reload()
    {
        mainAudioSource.volume = clips[9].volume;
        mainAudioSource.PlayOneShot(clips[9].clip);
    }


    void BBShoot()
    {
        mainAudioSource.volume = clips[1].volume;
        mainAudioSource.PlayOneShot(clips[1].clip);
    }


    void BBExplode()
    {
        mainAudioSource.volume = clips[2].volume;
        mainAudioSource.PlayOneShot(clips[2].clip);
    }


    void BBHit(PlayerScript p)
    {
        mainAudioSource.volume = clips[5].volume;
        mainAudioSource.pitch = UnityEngine.Random.value * 2;
        mainAudioSource.PlayOneShot(clips[5].clip);
    }


    void BBHitWall()
    {
        mainAudioSource.volume = clips[6].volume;
        mainAudioSource.PlayOneShot(clips[6].clip);
    }

    void WallHit()
    {
        mainAudioSource.volume = clips[20].volume;
        mainAudioSource.PlayOneShot(clips[20].clip);
    }

    void CrowdBoo()
    {
        Debug.Log("Booing");
        mainAudioSource.volume = clips[21].volume;
        mainAudioSource.PlayOneShot(clips[21].clip);
    }

    void WallExplode()
    {
        mainAudioSource.volume = clips[19].volume;
        mainAudioSource.PlayOneShot(clips[19].clip);
    }

    void CharacterIntro(PlayerScript player)
    {

        // Pick the intro BGM to match the character
        AudioClips chosenIntroSound = null;

        switch (player.playerType) {
            case PlayerTypes.DAVE:
                chosenIntroSound = daveIntro;
                break;
            
            case PlayerTypes.BIG_SCHLUG:
                chosenIntroSound = schlugIntro;
                break;
            
            case PlayerTypes.HAMMER:
                chosenIntroSound = hhhIntro;
                break;
            
            case PlayerTypes.EL_MOSCO:
                chosenIntroSound = moscoIntro;
                break;
        }

        characterIntroAudioSource.volume = chosenIntroSound.volume;
        characterIntroAudioSource.clip = chosenIntroSound.clip;
        characterIntroAudioSource.Play();
    }

    void OnCharacterIntroEnded(PlayerScript player) {
        // end the current character intro
        characterIntroAudioSource.Stop();
    }

    void Countdown(float offset)
    {
        // Stop any existing countdown sound - for example, if it starts playing then the character skips, start playing from offset
        countdownSource.Stop();

        countdownSource.volume = countdownSound.volume;
        countdownSource.clip = countdownSound.clip;
        

        countdownSource.Play();
        countdownSource.time = offset;
       
    }

    void RoundStart()
    {
        mainAudioSource.volume = roundStartSound.volume;
        mainAudioSource.PlayOneShot(roundStartSound.clip);
    }
}

[Serializable]
public class AudioClips
{
    public AudioClip clip;
    [Range(0,1)]public float volume;
}
