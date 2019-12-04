using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class ScreenAnim : MonoBehaviour
{
    public VideoPlayer activePlayer, otherPlayer;
    public VideoClip[] clips;
    VideoClip nextClip;
    public GameObject player;

  
    public enum Animation
    {
        IDLE,
        ELIMINATED,
        SHOWBOAT,
        SCARED
    }

    private void Start()
    {
        PrepareChange(Animation.IDLE);
        SwitchVideoPlayerTo(activePlayer);
        
        
    }

    void Idle()
    {
        PrepareChange(Animation.IDLE);
        SwitchVideoPlayerTo(activePlayer);
    }

    void ShowingOff()
    {
    }

    void Scared()
    {
    }
    

    void Eliminated()
    {
        PrepareChange(Animation.ELIMINATED);
        SwitchVideoPlayerTo(activePlayer);
    }


    void PrepareChange(Animation clipToPlay)
    {
        nextClip = clips[(int)clipToPlay];
        otherPlayer.clip = nextClip;
        otherPlayer.Play();

    }

    void SwitchVideoPlayerTo(VideoPlayer thisCam)
    {
        activePlayer = otherPlayer;
        otherPlayer = thisCam;
        activePlayer.targetCameraAlpha = 1;
        otherPlayer.targetCameraAlpha = 0f;
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
