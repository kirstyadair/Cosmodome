using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApprovalChangeUI : MonoBehaviour
{
    public Image barLeft;
    public Image barRight;
    public GameObject player;
    private Animator animator;

  
    
    public float approval;
    private float previousApproval;

    private void Start()
    {


        approval = player.GetComponent<PlayerScript>().approval.percentage;
        previousApproval = approval;

        animator = GetComponent<Animator>();

        barLeft.fillAmount = previousApproval / 100;
        barRight.fillAmount = previousApproval / 100;
    }
    public void Update()
    {
        approval = player.GetComponent<PlayerScript>().approval.percentage;
        if (approval != previousApproval)
        {
            if(approval<previousApproval)
            {
                animator.SetBool("Hit", true);
            }

            previousApproval = approval;

            barLeft.fillAmount = previousApproval / 100;
            barRight.fillAmount = previousApproval / 100;
        }
        
    }



}
