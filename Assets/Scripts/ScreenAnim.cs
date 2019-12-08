using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class ScreenAnim : MonoBehaviour
{
    public GameObject screen;
    Animator animator;

  
   

    private void Start()
    {
        animator = screen.GetComponent<Animator>();
        
        
    }

    void Idle()
    {
        animator.SetBool("IsScared", false);
        animator.SetBool("IsEliminated",false);
        animator.SetBool("IsShowboating",false);
    }

    void ShowingOff()
    {
        animator.SetBool("IsShowboating", true);
    }

    void Scared()
    {
        animator.SetBool("IsScared", true);
    }
    

    void Eliminated()
    {
        animator.SetBool("IsEliminated", true);
    }



}

