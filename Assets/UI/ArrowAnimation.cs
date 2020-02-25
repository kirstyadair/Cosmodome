using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnimation : MonoBehaviour
{
    public int currentMultiplier;
    private int previousMultiplier;

    public Text currentMultiplierText;
    public Text currentMultiplierTextDropShadow;

    public GameObject player;

    private Animator[] animators;
    private bool isJumping = false;
    private bool isEmpty;

    void Start()
    {
        currentMultiplier = player.GetComponent<ExcitementMeterScript>().comboScore;
        previousMultiplier = currentMultiplier;
        animators = GetComponentsInChildren<Animator>();

        currentMultiplierText.text = currentMultiplier.ToString();
        currentMultiplierTextDropShadow.text = currentMultiplier.ToString();

        StartCoroutine(IdleMovements());
    }

    IEnumerator FillDelay(int multiplier)
    {

        animators[multiplier - 1].SetBool("isEmpty", false);
        yield return new WaitForSeconds(.3f);




    }

    IEnumerator EmptyDelay(int multiplier)
    {
        animators[multiplier-1].SetBool("isFull", false);
        yield return new WaitForSeconds(.2f);
        animators[multiplier-1].SetBool("isEmpty", true);
        
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

    

    IEnumerator IdleMovements()
    {
        StartCoroutine(Bounce());
        yield return new WaitForSeconds(3f);
        StartCoroutine(IdleMovements());
    }

    

    void Update()
    {
            

        
        if (previousMultiplier != currentMultiplier)
        {
            currentMultiplierText.text = currentMultiplier.ToString();
            currentMultiplierTextDropShadow.text = currentMultiplier.ToString();


            if (previousMultiplier > currentMultiplier)
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    if (animators[i].GetBool("isFull") == true)
                    {
                        StartCoroutine(EmptyDelay(i));
                    }
                }
                    
            }
            if (previousMultiplier < currentMultiplier)
            {
                //Add to the combo
                StartCoroutine(FillDelay(currentMultiplier));
                animators[currentMultiplier - 1].SetBool("isFull", true);

            }
           
            previousMultiplier = currentMultiplier;
        }
        if (currentMultiplier != 0)
        {
            animators[currentMultiplier - 1].SetBool("pulse", true);

            if(currentMultiplier>1)
            {
                animators[currentMultiplier - 2].SetBool("pulse", false);
            }
            
        }



        currentMultiplier = player.GetComponent<ExcitementMeterScript>().comboScore;
           
      

        
    }

}