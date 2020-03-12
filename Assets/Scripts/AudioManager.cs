using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClips[] clips;

    [Header("Sounds to randomly play on character intro")]
    public AudioClips[] introSounds;

    [Header("Character shooting sounds")]
    public AudioClips[] shootingSounds;

    [Header("Countdown sound")]
    public AudioClips countdownSound;

    [Header("Round start sound")]
    public AudioClips roundStartSound;


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

        CutscenesManager.OnPlayCountdown += Countdown;
        CutscenesManager.OnPlayCharacterIntro += CharacterIntro;
        CutscenesManager.OnRoundStart += RoundStart;
    }

    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayerCollision(PlayerScript playerHit, PlayerScript playerAttacking)
    {
        audioSource.volume = clips[5].volume;
        audioSource.pitch = 0.5f + UnityEngine.Random.value;
        audioSource.PlayOneShot(clips[5].clip);
        audioSource.PlayOneShot(clips[8].clip);
    }


    void PlayerShooting(ShipController ship)
    {
        int shipSound = ship.shootingClipNumber - 1;

        audioSource.volume = shootingSounds[shipSound].volume;
        //audioSource.pitch = UnityEngine.Random.value * 2;
        audioSource.PlayOneShot(shootingSounds[shipSound].clip);
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


    void BBShoot()
    {
        audioSource.volume = clips[1].volume;
        audioSource.PlayOneShot(clips[1].clip);
    }


    void BBExplode()
    {
        audioSource.volume = clips[2].volume;
        audioSource.PlayOneShot(clips[2].clip);
    }


    void BBHit(PlayerScript p)
    {
        audioSource.volume = clips[5].volume;
        audioSource.pitch = UnityEngine.Random.value * 2;
        audioSource.PlayOneShot(clips[5].clip);
    }


    void BBHitWall()
    {
        audioSource.volume = clips[6].volume;
        audioSource.PlayOneShot(clips[6].clip);
    }

    void WallHit()
    {
        audioSource.volume = clips[20].volume;
        audioSource.PlayOneShot(clips[20].clip);
    }

    void WallExplode()
    {
        audioSource.volume = clips[19].volume;
        audioSource.PlayOneShot(clips[19].clip);
    }

    void CharacterIntro()
    {
        // pick a random character intro sound
        AudioClips chosenIntroSound = introSounds[UnityEngine.Random.Range(0, introSounds.Length)];
        audioSource.volume = chosenIntroSound.volume;
        audioSource.PlayOneShot(chosenIntroSound.clip);
    }

    void Countdown()
    {
        audioSource.volume = countdownSound.volume;
        audioSource.PlayOneShot(countdownSound.clip);
    }

    void RoundStart()
    {
        audioSource.volume = roundStartSound.volume;
        audioSource.PlayOneShot(roundStartSound.clip);
    }
}

[Serializable]
public class AudioClips
{
    public AudioClip clip;
    [Range(0,1)]public float volume;
}
