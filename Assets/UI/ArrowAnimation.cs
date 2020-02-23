using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public int currentMultiplier;
    private int previousMultiplier;

    public GameObject player;

    private Animator[] animators;
    private bool isJumping = false;
    private bool isEmpty;

    void Start()
    {
        currentMultiplier = player.GetComponent<ExcitementMeterScript>().comboScore;
        previousMultiplier = currentMultiplier;
        animators = GetComponentsInChildren<Animator>();
        

    }


    IEnumerator FillDelay(int multiplier)
    {
        
        animators[multiplier-1].SetBool("isEmpty", false);
        yield return new WaitForSeconds(.3f);
        

        

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



     void Update()
    {
        if(currentMultiplier!= previousMultiplier)
        {
            if(previousMultiplier>currentMultiplier)
            {
                //Remove the combo
            }
            if(previousMultiplier<currentMultiplier)
            {
                //Add to the combo
                StartCoroutine(FillDelay(currentMultiplier));
                animators[currentMultiplier - 1].SetBool("isFull", true);

            }

            currentMultiplier = player.GetComponent<ExcitementMeterScript>().comboScore;
            previousMultiplier = currentMultiplier;
        }
    }

}