using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public AudioClip song;
    public AudioClip[] songFills;
    public int songBPM;
}


public class SongPicker : MonoBehaviour
{
    public List<Song> theSongs = new List<Song>();

    public List<AudioClip> songs;
   
    public AudioSource currentSong;
    public AudioSource currentSongFill;

    public int roundCounter = 1;
    int songChosen;

    public float timepassed;
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
            timeToStartFrom = songPosition;
            currentSong.Stop();
            countBeats = false;
            currentSongFill.Play();
            roundCounter++;
        }
        if(newState==GameState.ROUND_START_CUTSCENE)
        {
            countBeats = false;
        }
        if (newState == GameState.INGAME)
        {
            currentSongFill.Stop();
            if(roundCounter>2)
            {
                currentSongFill.clip = null;
            }
            else
            {
                currentSongFill.clip = theSongs[songChosen].songFills[roundCounter - 1];
            }
            

            countBeats = true;
            currentSong.time = timeToStartFrom;
            currentSong.Play();
        }

    }

    

    void OnDestroy() {
        ScoreManager.OnStateChanged -= OnStateChange;
    }
    // Start is called before the first frame update
    
    void Start()
    {
        ScoreManager.OnStateChanged += OnStateChange;

        int songtoPick = Random.Range(0, theSongs.Count);
        songChosen = songtoPick;
        
        secPerBeat = 60f/theSongs[songtoPick].songBPM;

        currentSong.clip = theSongs[songtoPick].song;
        currentSongFill.clip = theSongs[songtoPick].songFills[0];

        currentSongTime = (float)AudioSettings.dspTime;


    }

    // Update is called once per frame
    void Update()
    {
        if (currentSong.isPlaying && countBeats == true)
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
