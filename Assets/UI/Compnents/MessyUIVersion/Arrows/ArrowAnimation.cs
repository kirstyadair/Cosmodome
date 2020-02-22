using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    private Animator[] animators;
    private bool isJumping = false;
    private bool isEmpty;

    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        animators[0].SetBool("isFull", true);
        animators[1].SetBool("isFull", true);
        animators[2].SetBool("isFull", true);
        animators[3].SetBool("isFull", true);
        animators[4].SetBool("isFull", true);
        animators[5].SetBool("isFull", true);
        StartCoroutine(Bounce());
        




    }


    IEnumerator FillDelay()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("isEmpty", false);
            yield return new WaitForSeconds(.3f);
        }

        

    }


    IEnumerator Bounce()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if(animators[i].GetBool("isFull")==true)
            {
                animators[i].SetBool("isJumping", true);
                yield return new WaitForSeconds(.1f);
                animators[i].SetBool("isJumping", false);
            }
            



        }

    }


    
    private void LateUpdate()
    {
        
    }

}