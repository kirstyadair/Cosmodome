using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class ScreenAnim : MonoBehaviour
{
    public GameObject screen;
    public Renderer renderer;
    public Material matOn;
    public Material matOff;
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
        Flash();
    }

    public void Scared()
    {
        StartCoroutine(PlayAnimation("IsScared",1.5f));
        Flash();

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

    IEnumerator FlashMats(Material mat1, Material mat2)
    {
            renderer.material = mat2;
            yield return new WaitForSeconds(.25f);
            renderer.material = mat1;
            yield return new WaitForSeconds(.25f);
            renderer.material = mat2;
            yield return new WaitForSeconds(.25f);
            renderer.material = mat1;
            yield return new WaitForSeconds(.25f);
            renderer.material = mat2;
            yield return new WaitForSeconds(.25f);
            renderer.material = mat1;



    }

    void Flash()
    {
        StartCoroutine(FlashMats(matOn, matOff));
        

    }

    void Update()
    {
        if(player.activeSelf==false)
        {
            Eliminated();
        }

    }

}

