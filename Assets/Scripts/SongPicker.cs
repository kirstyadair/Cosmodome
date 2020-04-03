using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPicker : MonoBehaviour
{
    public List<AudioClip> songs;
    public List<AudioClip> songFills;

    public AudioSource musicSource;
    public AudioSource fillSource;
    AudioClip currentSong;
    AudioClip currentSongFill;

    public float timepassed;
    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;

    public bool countBeats = true;

    public float timeToStartFrom;

    public float firstBeatOffset;
    public float currentSongTime;


    public void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.COUNTDOWN)
        {

        }
        if (newState == GameState.ROUND_END_CUTSCENE)
        {
            Mathf.Round(songPosition);
            timeToStartFrom = songPosition;
            musicSource.Stop();
            countBeats = false;
            fillSource.clip = currentSongFill;
            fillSource.Play();
        }
        if(newState==GameState.ROUND_START_CUTSCENE)
        {
            countBeats = false;
        }
        if (newState == GameState.INGAME)
        {
            Mathf.Round(timepassed);
            fillSource.Stop();
            countBeats = true;
            musicSource.clip = currentSong;
            musicSource.loop = false;
            musicSource.time = timeToStartFrom;
            musicSource.Play();
        }

    }

    

    void OnDestroy() {
        ScoreManager.OnStateChanged -= OnStateChange;
    }
    // Start is called before the first frame update
    
    void Start()
    {
        ScoreManager.OnStateChanged += OnStateChange;

        int songtoPick = 3;//Random.Range(0, songs.Count);
        currentSong = songs[songtoPick];
        musicSource.clip = currentSong;



        if (songtoPick == 0)
        {
            songBpm = 110f;
            currentSongFill = songFills[0];
            musicSource.volume = 0.5f;
            fillSource.volume = 0.5f;
        }
        if (songtoPick==1)
        {
            songBpm = 128f;
            currentSongFill = songFills[1];
            musicSource.volume = 0.4f;
            fillSource.volume = 0.4f;

        }
        if (songtoPick == 2)
        {
            songBpm = 128f;
            currentSongFill = songFills[2];
            musicSource.volume = 0.6f;
            fillSource.volume = 0.6f;

        }
        if (songtoPick == 3)
        {
            songBpm = 128f;
            currentSongFill = songFills[3];
            musicSource.volume = 0.6f;
            fillSource.volume = 0.6f;

        }


        secPerBeat = 60f / songBpm;
        currentSongTime = (float)AudioSettings.dspTime;


    }

    // Update is called once per frame
    void Update()
    {
        if (musicSource.isPlaying && countBeats == true)
        {
            songPosition = (float)(AudioSettings.dspTime - currentSongTime - firstBeatOffset - timepassed);
            songPositionInBeats = songPosition / secPerBeat;
        }
        
        if(countBeats == false)
        {
            timepassed += Time.deltaTime;
        }
    }
}
