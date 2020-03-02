using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPicker : MonoBehaviour
{
    public List<AudioClip> songs;
    

    // Start is called before the first frame update
    void Start()
    {
        int songtoPick = Random.Range(0, songs.Count);
        gameObject.GetComponent<AudioSource>().clip = songs[songtoPick];

        if (songtoPick == 0)
        {
            gameObject.GetComponent<AudioSource>().volume = 0.5f;
        }
        if (songtoPick==1)
        {
            gameObject.GetComponent<AudioSource>().volume = 0.4f;
        }
        if (songtoPick == 2)
        {
            gameObject.GetComponent<AudioSource>().volume = 0.6f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
