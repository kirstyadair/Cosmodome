using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class ScreenAnim : MonoBehaviour
{
    public GameObject screen;
    public GameObject player;
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

    public void ShowingOff()
    {
        StartCoroutine(PlayAnimation("IsShowboating", 2));

    }

    public void Scared()
    {
        StartCoroutine(PlayAnimation("IsScared",1.5f));
        
    }
    

    public void Eliminated()
    {
        animator.SetBool("IsEliminated", true);
    }

    IEnumerator PlayAnimation(string animatorBoolName,float timeToWait)
    {
        animator.SetBool(animatorBoolName, true);
        yield return new WaitForSeconds(timeToWait);
        animator.SetBool(animatorBoolName, false);
    }

    void Update()
    {
        if(player.activeSelf==false)
        {
            Eliminated();
        }

    }

}

