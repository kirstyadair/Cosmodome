using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class ScreenAnim : MonoBehaviour
{
    public GameObject screens;
    public GameObject[] videoPlayers;
    public VideoClip[] clips;
    public GameObject player;

    int videoPlayerActive;

    public enum Animation
    {
        IDLE,
        ELIMINATED,
        SHOWBOAT,
        SCARED
    }

    private void Start()
    {
        
        videoPlayers[0].enabled = true;
        videoPlayers[1].enabled = false;
        videoPlayers[0].clip = clips[(int)Animation.IDLE];
        Eliminated();
    }

    void Idle()
    {
        PrepareChange(Animation.IDLE);
        
    }

    void ShowingOff()
    {
        PrepareChange(Animation.SHOWBOAT);
    }

    void Scared()
    {
        PrepareChange(Animation.SCARED);
    }
    

    void Eliminated()
    {
        PrepareChange(Animation.ELIMINATED);
    }


    IEnumerator PrepareChange(Animation clipToPlay)
    {
        if(videoPlayers[0].enabled==true)
        {
            videoPlayers[1].clip = clips[(int)clipToPlay];
        }
        if(videoPlayers[1].enabled==true)
        {
            videoPlayers[0].clip = clips[(int)clipToPlay];
        }

        yield return new WaitForSeconds(0.2f);

        if(videoPlayers[0].enabled == true)
        {
            videoPlayers[0].enabled = false;
            videoPlayers[1].enabled = true;
            videoPlayers[1].Play();
        }
        if (videoPlayers[1].enabled == true)
        {
            videoPlayers[1].enabled = false;
            videoPlayers[0].enabled = true;
            videoPlayers[0].Play();
        }

    }

    void Update()
    {


        if (player.activeInHierarchy == false)
        {
            Eliminated();
        }
        else
        {
            Idle();
        }
        


        
    }
}
