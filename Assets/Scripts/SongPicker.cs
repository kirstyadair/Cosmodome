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
   
    public AudioSource currentSong;
    public AudioSource currentSongFill;

    

    public int roundCounter = 1;
    int songChosen;

    


    public void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.COUNTDOWN)
        {

        }
        if (newState == GameState.ROUND_END_CUTSCENE)
        {
            roundCounter++;
        }
        if(newState==GameState.ROUND_START_CUTSCENE)
        {
            
        }
        if (newState == GameState.INGAME)
        {
            if(roundCounter==1)
            {
                StartCoroutine(PlaySong());
            }
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

        currentSong.clip = theSongs[songtoPick].song;
        currentSongFill.clip = theSongs[songtoPick].songFills[0];

        

    }


    void CheckCurrentRound()
    {
        if (roundCounter > 2)
        {
            currentSongFill.clip = null;
        }
        else
        {
            currentSongFill.clip = theSongs[songChosen].songFills[roundCounter - 1];
        }
    }


    IEnumerator PlaySong()
    {
        CheckCurrentRound();
        currentSong.Play();
        yield return new WaitForSeconds(60.0f);
        currentSong.Pause();
        StartCoroutine(PlaySongFill());
    }

    IEnumerator PlaySongFill()
    {
        currentSongFill.Play();
        yield return new WaitForSeconds(15.0f);
        currentSongFill.Stop();
        StartCoroutine(PlaySong());
    }

    // Update is called once per frame
    void Update()
    {


    }
}
