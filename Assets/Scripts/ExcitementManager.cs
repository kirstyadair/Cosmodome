using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ExcitementManager : MonoBehaviour
{
    List<ExcitementMeterScript> players = new List<ExcitementMeterScript>();
    List<ExcitementMeterScript> orderedPlayers = new List<ExcitementMeterScript>();
    ScoreManager sm;
    AudioSource audio;
    public AudioClip cheer1;
    public AudioClip cheer2;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public ParticleSystem ps4;
    ExcitementMeterScript topPlayer;
    public static event ResetHype OnResetHype;
    public delegate void ResetHype();
    public static event AddHype OnAddHype;
    public delegate void AddHype();
    public float speedIncrement;
    public float maxHypeTimer;
    public int hypeLevel;
    float hypeTimer;


    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        PlayerScript.OnPlayerCollision += AddToHype;
        PlayerScript.OnPlayerHitByArenaCannon += AddToHype;
        sm = ScoreManager.Instance;
        foreach (PlayerScript player in sm.players)
        {
            players.Add(player.gameObject.GetComponent<ExcitementMeterScript>());
        }
        UpdateHype();
    }

    // Update is called once per frame
    public void UpdateHype()
    {
        orderedPlayers = players.OrderByDescending(x => x.comboScore).ToList();
        topPlayer = orderedPlayers[0];

        if (topPlayer.comboScore <= 0) hypeLevel = 0;
    }

    void Update()
    {
        if (hypeTimer >= 0)
        {
            hypeTimer -= Time.deltaTime;
            if (hypeTimer < 0)
            {
                hypeLevel = 0;
                speedIncrement = 1;
                OnResetHype?.Invoke();
            }
        }
    }

    void AddToHype(GameObject hitPlayer, GameObject shootingPlayer)
    {
        UpdateHype();
        if (shootingPlayer == topPlayer.gameObject)
        {
            hypeLevel++;
            hypeTimer = maxHypeTimer;
            speedIncrement += 2;
            OnAddHype?.Invoke();
            audio.pitch = ((float)hypeLevel / 10) + 0.5f;
            audio.PlayOneShot(cheer1);
            audio.PlayOneShot(cheer2);
            ps1.Play();
            ps2.Play();
            ps3.Play();
            ps4.Play();
        }
    }
}
